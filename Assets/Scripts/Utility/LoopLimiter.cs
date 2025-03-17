using System;
using System.Diagnostics;
using UnityEngine;

namespace Utility
{
    public class LoopLimiter
    {
        private const uint K_Maximum = (uint) short.MaxValue;
        private uint _count = 0;
        private uint _maximum;
        private bool _throwOnExceed;


        public LoopLimiter()
        {
            _maximum = K_Maximum;
            _throwOnExceed = true;
        }
        public void Reset()
        {
            _count = 0;
        }
        public void Reset(uint maximum, bool throwOnExceed = true)
        {
            _count = 0;
            _maximum = maximum;
            _throwOnExceed = throwOnExceed;
        }
        
        public LoopLimiter(int maximum = (int) K_Maximum, bool throwOnExceed = true) : this((uint) maximum,
            throwOnExceed){}
        
        public LoopLimiter(uint maximum = K_Maximum, bool throwOnExceed = true)
        {
            _maximum = maximum;
            _throwOnExceed = throwOnExceed;
        }
        
        public bool CanLoop()
        {
            if (_count >= _maximum)
            {
                if (_throwOnExceed)
                    throw new TimeoutException();
                return false;
            }
            _count++;
            return true;
        }
        
        //implicit cast to bool
        public static implicit operator bool(LoopLimiter limiter)
        {
            return limiter.CanLoop();
        }
        
    }
    public class LoopTimeout
    {
        private const uint K_MaximumMilliseconds = 5000;
        private readonly Stopwatch _stopWatch;
        private readonly uint _maximum;
        

        public LoopTimeout(uint maximumMilliseconds = K_MaximumMilliseconds)
        {
            _maximum = maximumMilliseconds;
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }
        
        public bool CanLoop()
        {
            if (_stopWatch.ElapsedMilliseconds >= _maximum)
            {
                _stopWatch.Stop();
                return false;
            }
            return true;
        }
        
        //implicit cast to bool
        public static implicit operator bool(LoopTimeout limiter)
        {
            return limiter.CanLoop();
        }
    }
}