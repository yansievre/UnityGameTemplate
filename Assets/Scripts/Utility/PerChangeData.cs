using System;
using Utility.GuidCompare;

namespace Utility
{
    public struct PerChangeData<TValue> : IGuidOwner
    { 
        private readonly Guid _guid;
        private readonly Func<TValue> _getter;
        private readonly Func<bool> _changeChecker;
        private TValue _value;

        public PerChangeData(Guid guid, Func<TValue> getter, Func<bool> changeChecker)
        {
            _guid = guid;
            _getter = getter;
            _changeChecker = changeChecker;
            _value = default;
        }
        
        public TValue Value 
        {
            get
            {
                try
                {
                    if (_changeChecker() || _value == null)
                    {
                        _value = _getter();
                    }
                }
                catch (Exception e)
                {
                    // ignored
                }

                return _value;
            }
        }

        /// <summary>
        /// Ensures that the change check doesn't run
        /// </summary>
        public TValue MostRecentValue => _value;

        public Guid Guid => _guid;
    }
}