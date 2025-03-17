using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utility.Optional
{
    [Serializable]
    public struct OverrideField<T> : ISerializationCallbackReceiver
    {
        public bool hasOverride;
        [SerializeField]
        private T _overrideValue;
        
        public T DefaultValue { get; set; }
        
        public T Value => Application.isPlaying ? _overrideValue : hasOverride ? _overrideValue : DefaultValue;

        //implicit cast
        public static implicit operator T(OverrideField<T> overrideField) => overrideField.hasOverride ? overrideField._overrideValue : default;
        
        public void OnBeforeSerialize()
        {
            if (!hasOverride)
                _overrideValue = DefaultValue;
        }

        public void OnAfterDeserialize()
        {
            if (Application.isPlaying)
                DefaultValue = _overrideValue;
        }
    }
    
    [Serializable]
    public struct OverrideFieldRef<T> : ISerializationCallbackReceiver
    {
        public bool hasOverride;
        [SerializeReference]
        private T _overrideValue;
        
        public T DefaultValue { get; set; }
        
        public T Value => Application.isPlaying ? _overrideValue : hasOverride ? _overrideValue : DefaultValue;

        //implicit cast
        public static implicit operator T(OverrideFieldRef<T> overrideField) => overrideField.hasOverride ? overrideField._overrideValue : default;
        
        public void OnBeforeSerialize()
        {
            if (!hasOverride)
                _overrideValue = DefaultValue;
        }

        public void OnAfterDeserialize()
        {
            if (Application.isPlaying)
                DefaultValue = _overrideValue;
        }
    }
    
}