using System;
using Unity.Collections;
using UnityEngine;

namespace Util {
	[RequireComponent(typeof(CharacterController2D))]
	public class MovementInput : MonoBehaviour
	{
		public static MovementInput Instance { get; private set; }
		private static int MovementDisableCount = 0;
		private static bool MovementEnabled => MovementDisableCount == 0;
		public bool BlockNegativeInput = false;
		
		public static IDisposable DisableMovement() 
		{
			MovementDisableCount++;
			return Disposable.Create(() => MovementDisableCount--);
		}
		
		[ReadOnly, SerializeField]
		private CharacterController2D _characterController2D;
		private void Awake() 
		{
			_characterController2D ??= GetComponent<CharacterController2D>();
			Instance = this;
		}

		private void Update() 
		{
			if (!MovementEnabled)
				return;
			var movementInput = GetMovementInput();
			if (BlockNegativeInput)
				movementInput.x = Mathf.Clamp01(movementInput.x);
			
			_characterController2D.Move(movementInput.x, false, false);
		}

		private Vector2 GetMovementInput() {
			//FILL
			return Vector2.zero;
		}
		

		private void Reset() 
		{
			_characterController2D ??= GetComponent<CharacterController2D>();
		}
	}
}