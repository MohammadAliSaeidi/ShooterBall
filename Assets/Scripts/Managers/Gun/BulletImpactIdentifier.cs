using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpactIdentifier : MonoBehaviour
{
	public BulletImpactTags bulletImpactTags = BulletImpactTags.Default;
}

public enum BulletImpactTags
{
	Default,
	Metal,
	Soil,
	Water,
	Grass,
	Wood,
	Stone
}
