using UnityEngine;

namespace Utility.ProgressTracker
{
    /// <summary>
    /// Progress info for e.g a progressbar.
    /// Used by the scan functions in the project
    /// See: <see cref="AstarPath.ScanAsync"/>
    /// </summary>
    public struct Progress {
        /// <summary>Current progress as a value between 0 and 1</summary>
        public readonly float progress;
        /// <summary>Description of what is currently being done</summary>
        public readonly string description;

        public object result;

        public bool HasResult => !ReferenceEquals(result, null);

        public Progress (float progress, string description) {
            this.progress = progress;
            this.description = description;
            result = null;
        }

        public Progress (float progress, string description, object result) {
            this.progress = progress;
            this.description = description;
            this.result = result;
        }
        
        public Progress MapTo (float min, float max, string prefix = null) {
            return new Progress(Mathf.Lerp(min, max, progress), prefix + description, this.result);
        }

        public override string ToString () {
            return progress.ToString("0.0") + " " + description;
        }
    }
}