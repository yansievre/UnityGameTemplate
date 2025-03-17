using System;

namespace Utility.ScriptableSingleton
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AssetPathAttribute : Attribute
    {
        public string Path { get; }

        public AssetPathAttribute(string filePath)
        {
            Path = filePath;
        }
    }
}