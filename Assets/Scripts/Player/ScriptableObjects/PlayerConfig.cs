using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Player Settings", order = 50)]
public class PlayerConfig : ScriptableObject
{
	public bool IsMultiplayer = false;

	[Range(0, 4)]
	public int MaxGunSlots;

	[Range(0, 4)]
	public int MaxThrowableSlots;

	public PlayerMovementConfig playerMovementConfig;
}
