using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityConstants {
	public abstract class Interactable : MonoBehaviour
	{
		public static List<Interactable> Interactables { get; } = new List<Interactable>();
		[SerializeField]
		private float _interactionRange = 1;
		
		public float InteractionRange => _interactionRange;
		public abstract string InteractionPrompt { get; }

		private void OnEnable() {
			Interactables.Add(this);
		}
		
		private void OnDisable() {
			Interactables.Remove(this);
		}
	}
}