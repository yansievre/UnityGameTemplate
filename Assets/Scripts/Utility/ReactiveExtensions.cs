#if R3
using System.Collections.Generic;
using R3;
using Unity.Mathematics;
using UnityEngine;

namespace Utility
{
	public static class ReactiveExtensions
	{
		private static readonly Dictionary<int, Observable<float3>> ObservePositionCache = new ();
		public static Observable<float3> ObservePosition(this Transform transform, EqualityComparer<float3> comparer = null)
		{
			comparer ??= EqualityComparer<float3>.Default;
			var hash = transform.GetHashCode() ^ comparer.GetHashCode();
			if(ObservePositionCache.TryGetValue(hash, out var observable))
				return observable;
			ObservePositionCache[hash] = Observable.EveryValueChanged(transform, tr => tr.position, comparer);
			return ObservePositionCache[hash];
		}
        
		private static readonly Dictionary<int, Observable<quaternion>> ObserveRotationCache = new ();
		public static Observable<quaternion> ObserveRotation(this Transform transform, EqualityComparer<quaternion> comparer = null)
		{
			comparer ??= EqualityComparer<quaternion>.Default;
			var hash = transform.GetHashCode() ^ comparer.GetHashCode();
			if(ObserveRotationCache.TryGetValue(hash, out var observable))
				return observable;
			ObserveRotationCache[hash] =  Observable.EveryValueChanged(transform, tr => tr.rotation, comparer);
			return ObserveRotationCache[hash];
		}
        
		private static readonly Dictionary<int, Observable<float3>> ObserveLossyScaleCache = new ();
		public static Observable<float3> ObserveLossyScale(this Transform transform, EqualityComparer<float3> comparer = null)
		{
			comparer ??= EqualityComparer<float3>.Default;
			var hash = transform.GetHashCode() ^ comparer.GetHashCode();
			if(ObserveLossyScaleCache.TryGetValue(hash, out var observable))
				return observable;
			ObserveLossyScaleCache[hash] = Observable.EveryValueChanged(transform, tr => tr.lossyScale, comparer);
			return ObserveLossyScaleCache[hash];
		}

		public static Observable<Unit> ObserveTransform(this Transform transform,  EqualityComparer<float3> float3Comparer = null,  EqualityComparer<quaternion> quaternionComparer = null)
		{
			return transform.ObservePosition(float3Comparer).AsUnitObservable()
				.Merge(transform.ObserveRotation(quaternionComparer).AsUnitObservable())
				.Merge(transform.ObserveLossyScale(float3Comparer).AsUnitObservable());
		}
        
	}
}
#endif