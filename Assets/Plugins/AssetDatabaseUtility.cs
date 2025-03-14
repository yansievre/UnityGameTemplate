using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

namespace Plugins {
	public class AssetDatabaseUtility {
		/// <summary>
		/// Will return empty if not in editor
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static IEnumerable<T> FindAssetsByType<T>() where T : UnityEngine.Object
		{
#if UNITY_EDITOR
			string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T)}");

			for( int i = 0; i < guids.Length; i++ )
			{
				string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);	
				T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>( assetPath );
				if( asset != null )
					yield return asset;
			}
#else
			yield break;
#endif
		}
		
		private static IEnumerable<string> GetActionInputs() {
			
			return FindAssetsByType<InputActionAsset>().First().actionMaps.SelectMany(x => x.actions).Select(x => x.name);
		}
	}
}