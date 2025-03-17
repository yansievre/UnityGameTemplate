using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class MovingAverage<T>
    {
        private readonly AnimationCurve _weightCurve;
        private readonly int _maxCount;
        private readonly LinkedList<T> _linkedList;
        private int _count;
            
        public MovingAverage(AnimationCurve weightCurve, int maxCount)
        {
            _weightCurve = weightCurve;
            _maxCount = maxCount;
            _linkedList = new LinkedList<T>();
            _count = 0;
        }

        public int Count => _count;
        public void Add(T value)
        {
            if (_count >= _maxCount)
            {
                _linkedList.RemoveFirst();
                _count--;
            }
            _linkedList.AddLast(value);
            _count++;
        }
        
        public void Clear()
        {
            _linkedList.Clear();
            _count = 0;
        }

#if NETFRAMEWORK
        public T Evaluate()
        {
            var enumerator = _linkedList.GetEnumerator();
            dynamic sum = default(T);
            float divisor = 0f;
            var i = 0;

            while (enumerator.MoveNext())
            {
                dynamic cur = enumerator.Current;
                var weight = _weightCurve.Evaluate(i);
                divisor += weight;
                sum += weight*cur;
                i++;
            }
            
            divisor = divisor == 0 ? 1 : divisor;

            return sum/divisor;
        }
#endif
    }
}