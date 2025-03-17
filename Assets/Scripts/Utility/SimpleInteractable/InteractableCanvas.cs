using Plugins.InputSystemActionPromptsExtras;
using System;
using UnityEngine;

namespace UnityConstants 
{
	public class InteractableCanvas : SingletonMonoBehaviour<InteractableCanvas>
	{
		[SerializeField]
		private PromptImage _promptImage;

		private Interactable _currentInteractable;

		public void SetCurrentInteractable(Interactable interactable) {
			_currentInteractable = interactable;
			if(_currentInteractable == null) return;
			if(_currentInteractable.InteractionPrompt == null) return;
			_promptImage.SetAction(_currentInteractable.InteractionPrompt);
		}

		private void Update() {
			if (_currentInteractable == null || _currentInteractable.InteractionPrompt == null) {
				_promptImage.gameObject.SetActive(false);
				return;
			}
			_promptImage.gameObject.SetActive(true);
			transform.position = _currentInteractable.transform.position;
		}
	}
}