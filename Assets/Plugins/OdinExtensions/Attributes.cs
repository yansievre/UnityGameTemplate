using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

namespace Plugins.OdinExtensions {
	[IncludeMyAttributes]
	[ValueDropdown("@TagSelectionAttribute.GetTags()")]
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class TagSelectionAttribute : Attribute
	{
		private static IEnumerable<string> GetTags() {
			return UnityEditorInternal.InternalEditorUtility.tags;
		}
	}
	
	[IncludeMyAttributes]
	[ValueDropdown("@InputActionSelectionAttribute.GetActionInputs()")]
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class InputActionSelectionAttribute : Attribute
	{
		private static IEnumerable<ValueDropdownItem<string>> GetActionInputs() {
			return AssetDatabaseUtility.FindAssetsByType<InputActionAsset>().First().actionMaps
				.SelectMany(map => map.actions.Select(action => {
					string t = $"{map.name}/{action.name}";
					return new ValueDropdownItem<string>(t,t);
				}));
		}
	}
}