using UnityEngine;

public class ExplosionIdentifier : MonoBehaviour
{
	public ExplosionTag explosionTag = ExplosionTag.Default;
}

public enum ExplosionTag
{
	Default,
	Metal,
	Soil,
	Water,
	Grass,
	Wood,
	Stone
}
