using System;
using UnityEngine;

[Serializable]
public class MagazineInfo
{
	[Range(0, 150)]
	public int MagazineCapacity = 30;

	[Range(0, 300)]
	public int TotalAmmoCount = 3;
}

[Serializable]
public class RecoilInfo
{
	[Range(0, 10)]
	public float VerticalRecoil;

	[Range(0, 10)]
	public float HorizontalRecoil;

	[Range(0, 2)]
	public float MinConeSpreadSize;

	[Range(0, 10)]
	public float MaxConeSpreadSize;

	[Range(0.1f, 10)]
	public float SpreadIncrementAmountPerBulletShoot;

	[Range(0.1f, 10)]
	public float SpreadBackupSpeed;
}