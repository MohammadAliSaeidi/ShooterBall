using UnityEngine;

[CreateAssetMenu(fileName = "Gun Specifics", menuName = "Gun/Gun Specifics", order = 50)]
public class GunSpecifics : ScriptableObject
{
	public FiringMode firingModes;
	public MagazineInfo magazineInfo;
	public RecoilInfo recoilInfo;
	public Bullet bulletPrefab;
	public Bullet mp_bulletPrefab;
	public BulletSpeicifcs bulletSpecifics;
	public float ReloadTime = 1;
	public float FireRate = 10;
}
