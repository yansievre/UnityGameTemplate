using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Progress = Utility.ProgressTracker.Progress;
#if  UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Utility
{
    public static class EnumerableUtility
    {
        private const int K_YieldEvery = 1000/30;
        /// <summary>
        /// Returns true if you should yield
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<(T item, bool yieldFrame)> EnumerateStable<T>(this IEnumerable<T> enumerable)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var elapsed_time = 0L;
            List<T> list = new List<T>();
            bool yieldFrame = false;
            
            foreach (var item in enumerable)
            {
                yieldFrame = false;
                if (elapsed_time > K_YieldEvery)
                {
                    elapsed_time -= K_YieldEvery;
                    yieldFrame = true;
                }
                stopwatch.Start();
                yield return (item, yieldFrame);
                stopwatch.Stop();
                elapsed_time += stopwatch.ElapsedMilliseconds;
                stopwatch.Reset();
            }
        }

#if  UNITASK
        public static async UniTask WithProgressBar(this IEnumerable<Progress> progressEnumerable, string title)
        {
            try
            {
                foreach (var stableData in progressEnumerable.EnumerateStable())
                {
                    UnityEditor.EditorUtility.DisplayProgressBar(title, stableData.item.description,
                        stableData.item.progress);
                    if (stableData.yieldFrame)
                        await UniTask.Yield();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                UnityEditor.EditorUtility.DisplayDialog(title, "An exception occured during the operation", "ok");
            }
            UnityEditor.EditorUtility.ClearProgressBar();
        }
#endif

        public static IEnumerable WithProgressBarSync(this IEnumerable<Progress> progressEnumerable, string title)
        {
            try
            {
                foreach (var stableData in progressEnumerable)
                {
                    UnityEditor.EditorUtility.DisplayProgressBar(title, stableData.description, stableData.progress);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                UnityEditor.EditorUtility.DisplayDialog(title, "An exception occured during the operation", "ok");
                yield break;
            }
            UnityEditor.EditorUtility.ClearProgressBar();
        }
    }
}