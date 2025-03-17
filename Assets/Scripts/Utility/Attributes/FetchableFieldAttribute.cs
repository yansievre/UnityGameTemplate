using System;
using UnityEngine;

namespace Utility.Attributes
{
    public enum FetchStrategy
    {
        GetComponent = 0,
        GetComponentInParent = 1,
        GetComponentInChildren = 2,
        FindFirstInScene = 4,
        /// <summary>
        /// Collections only
        /// </summary>
        GetComponents = 10,
        /// <summary>
        /// Collections only
        /// </summary>
        GetComponentsInChildren = 11,
        /// <summary>
        /// Collections only
        /// </summary>
        GetComponentsInParent = 12,
        /// <summary>
        /// Collections only
        /// </summary>
        FindAllInScene = 13,
    }
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class FetchableFieldAttribute : PropertyAttribute
    {
        public bool AutoFetch { get; set; }
        public FetchStrategy FetchStrategy { get; set; }

        public FetchableFieldAttribute(FetchStrategy fetchStrategy = FetchStrategy.GetComponent, bool autoFetch = false) : base(true)
        {
            AutoFetch = autoFetch;
            FetchStrategy = fetchStrategy;
        }
    }
}