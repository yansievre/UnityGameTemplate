using System;
using UnityEngine;

namespace Utility
{
    public class TransformChangeChecker : MonoBehaviour
    {
        public float positionThreshold = 0.01f;
        public float scaleThreshold = 0.01f;
        public float angleThreshold = 1;
        private Vector3 _lastPosition;
        private Vector3 _lastScale;
        private Quaternion _lastRotation;

        public bool ChangeCheck()
        {
            if (!this)
                return false;
            if (!transform)
                return false;
            var p = transform.position;
            var l = transform.lossyScale;
            var r = transform.rotation;
            var hasChange = (p - _lastPosition).magnitude > positionThreshold 
                            ||  (l - _lastScale).magnitude > scaleThreshold 
                            || Quaternion.Angle(r, _lastRotation) > angleThreshold;

            if (!hasChange) 
                return false;
            _lastPosition = p;
            _lastScale = l;
            _lastRotation = r;

            return true;
        }
    }
}