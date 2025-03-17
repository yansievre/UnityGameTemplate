using System;
using UnityEngine;
using Utility.GuidCompare;

namespace Utility
{
    public struct PerFrameData<TValue> : IGuidOwner
    {
        private PerChangeData<TValue> _perChangeData;
        private int _lastFrame;

        public PerFrameData(Guid guid, Func<TValue> getter) : this()
        {
            _lastFrame = -1;
            _perChangeData = new PerChangeData<TValue>(guid, getter, CheckChange);
        }

        private bool CheckChange()
        {
            if(Time.frameCount != _lastFrame)
            {
                _lastFrame = Time.frameCount;

                return true;
            }
            return false;
        }

        public TValue Value => _perChangeData.Value;
        public Guid Guid => _perChangeData.Guid;
    }
}