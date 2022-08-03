using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallShooter.InputManager {
	public static class InputManager {

		#region Varialbes

		public static bool BindingMode { get; private set; }


		//public static KeyCode keyCode;
		public struct Actions { 
			public KeyCode Jump;
			public KeyCode OpenInventory;
			public KeyCode PauseGame;
			public KeyCode Break;
		}

		static Actions actions = new Actions();

		#endregion

		#region Native Methods

		public static void SetToBindingMode () {
			BindingMode = true;
		}

		public static void CancelBindingMode () {
			BindingMode = false;
		}

		public static void SetBindingKey (KeyCode aKey, Actions aAction) {
			
		}

		#endregion
	}
}
