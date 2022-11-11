using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Player Settings", order = 50)]
public class PlayerConfig : ScriptableObject
{
	[Range(0, 4)]
	public int MaxGunSlots;

	[Range(0, 4)]
	public int MaxThrowableSlots;

	public PlayerMovementConfig playerMovementConfig;
}
