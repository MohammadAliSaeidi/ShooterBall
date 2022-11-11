using UnityEngine;

namespace BallShooter.Player
{
	public class DropedGrenade : MonoBehaviour
	{
		public Throwable grenade;
		[HideInInspector] public bool hasBeenPicked = false;
	}
}