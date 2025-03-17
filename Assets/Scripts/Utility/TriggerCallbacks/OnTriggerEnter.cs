using Plugins.OdinExtensions;
using UnityEngine;
using UnityEngine.Events;

namespace UnityConstants {
	public class OnTriggerEnter : MonoBehaviour
	{
		public UnityEvent onTriggerEnter;
		[TagSelection]
		public string tag;

		private void OnTriggerEnter2D(Collider2D other) {
			if (other.CompareTag(tag)) {
				onTriggerEnter.Invoke();
			}
		}
	}
}