using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utility
{
    [Serializable]
    public class PossibilityGraph
    {
        private enum ExceedingValues
        {
            Clamp,
            Zero
        }
        [SerializeField, InlineButton("Normalize")]
        private AnimationCurve _possibilityCurve;
        [SerializeField, MinMaxSlider(0, 20)]
        private Vector2Int _minMaxValues;
        [SerializeField]
        private float _baseMultiplier;
        [SerializeField]
        private ExceedingValues _leftExceed;
        [SerializeField]
        private ExceedingValues _rightExceed;

        public void Normalize()
        {
            if (_possibilityCurve.keys.Length == 0)
                return;

            var minKey = _possibilityCurve.keys[0].time;
            var maxKey = _possibilityCurve.keys[^1].time;
            var range = maxKey - minKey;
            var scale = (_minMaxValues.y - _minMaxValues.x) / range;
            var offset = _minMaxValues.x - minKey * scale;

            // Calculate the sum of the values at each integer in the range
            float sum = 0;
            for (int i = _minMaxValues.x; i <= _minMaxValues.y; i++)
            {
                sum += _possibilityCurve.Evaluate(i);
            }

            if (Mathf.Approximately(sum, 0f))
                return;
            // Create new keys with normalized values
            var newKeys = new Keyframe[_possibilityCurve.keys.Length];
            for (var i = 0; i < _possibilityCurve.keys.Length; i++)
            {
                var key = _possibilityCurve.keys[i];
                var newTime = key.time * scale + offset;
                var newValue = key.value / sum; // Normalize the value
                newKeys[i] = new Keyframe(newTime, newValue);
            }

            _possibilityCurve.keys = newKeys;
        }
        
        public int PickKeyFromValues()
        {
            var total = Enumerable.Range(_minMaxValues.x, _minMaxValues.y - _minMaxValues.x + 1)
                .Sum(x => _possibilityCurve.Evaluate(x));
            // Generate a random number between 0 and 1
            float random = UnityEngine.Random.value*total;

            // Calculate the cumulative probabilities
            float cumulative = 0;
            for (int i = _minMaxValues.x; i <= _minMaxValues.y; i++)
            {
                cumulative += _possibilityCurve.Evaluate(i);
                if (random <= cumulative)
                {
                    // Return the time of the key as it represents the actual value in the curve
                    return i;
                }
            }

            // If no key is found (which should not happen if the probabilities are normalized), return a default value
            return 0;
        }
        
        public float GetValue(int value)
        {
            if (value < _minMaxValues.x)
                return _leftExceed == ExceedingValues.Clamp ? _possibilityCurve.Evaluate(_minMaxValues.x) * _baseMultiplier : 0;
            if(value > _minMaxValues.y)
                return _rightExceed == ExceedingValues.Clamp ? _possibilityCurve.Evaluate(_minMaxValues.y) * _baseMultiplier : 0;
            return _possibilityCurve.Evaluate(value) * _baseMultiplier;
        }
    }
}