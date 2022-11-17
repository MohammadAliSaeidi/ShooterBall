using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet Specifics", menuName = "Gun/Bullet Specifics", order = 50)]
public class BulletSpeicifcs : ScriptableObject
{
	public float BulletForce = 10;
	public List<BulletImpact> bulletImpacts;
}