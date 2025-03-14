using InputSystemActionPrompts;
using Plugins.OdinExtensions;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Plugins.InputSystemActionPromptsExtras 
{
	[RequireComponent(typeof(Image))]
	[ExecuteAlways]
	public class PromptImage : MonoBehaviour 
	{
		[SerializeField] private Image _image;
		/// <summary>
		/// This should be the full path, including binding map and action, eg "Player/Move"
		/// </summary>
		[InputActionSelection]
		[OnValueChanged(nameof(RefreshIcon))]
		[SerializeField] private string m_Action = "Player/Move";
		
		[SerializeField] private bool _setNativeSize = true;

		protected void Start()
		{
			_image ??= GetComponent<Image>();
			RefreshIcon();
			// Listen to device changing
			if (Application.isPlaying)
			{
				InputDevicePromptSystem.OnActiveDeviceChanged += DeviceChanged;
			}
		}

		protected void OnDestroy()
		{
			// Remove listener
			if (Application.isPlaying) 
			{
				InputDevicePromptSystem.OnActiveDeviceChanged -= DeviceChanged;
			}
		}
        
		/// <summary>
		/// Called when active input device changed
		/// </summary>
		private void DeviceChanged(InputDevice device)
		{
			RefreshIcon();
		}

		/// <summary>
		/// Sets the icon for the current action.
		/// Called from the editor when the action is changed.
		/// </summary>
		public void RefreshIcon()
		{
			var sourceSprite=InputDevicePromptSystem.GetActionPathBindingSprite(m_Action);
			if (sourceSprite == null)
				return;
			_image.sprite = sourceSprite;
			
			if (_setNativeSize)
				_image.SetNativeSize();
			
#if UNITY_EDITOR
			if (!Application.isPlaying) 
				UnityEditor.EditorUtility.SetDirty(_image);
#endif
		}

		private void Reset() {
			_image ??= GetComponent<Image>();
		}
	}
}