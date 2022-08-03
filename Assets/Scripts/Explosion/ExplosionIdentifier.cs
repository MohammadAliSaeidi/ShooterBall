using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionIdentifier : MonoBehaviour
{
	public ExplosionTags explosionTag = ExplosionTags.Default;
}

public enum ExplosionTags
{
	Default,
	Metal,
	Soil,
	Water,
	Grass,
	Wood,
	Stone
}
