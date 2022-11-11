using UnityEngine;

namespace BallShooter.Player
{
	public class Gun_New : MonoBehaviour, IEquipment
	{

		public GunSpecifics specifics;
		public int CurrentMagazineAmmoCount;
		public int ExtraAmmoCount;


		[SerializeField]
		private ParticleSystem _bulletShellParticle;

		[SerializeField]
		private float _maxHeatParticles = 10;

		[SerializeField]
		private GameObject _cooldownContainer;

		[SerializeField]
		private GameObject MuzzleFlashContainer;

		[SerializeField]
		private Transform _bulletSpawnPoint;

		private float _currentHeatAmount = 0;
		public Transform BulletSpawnPoint { get { return _bulletSpawnPoint; } }

		private ParticleSystem[] _muzzleFlashParticles;
		private ParticleSystem[] _cooldownParticles;
		private float _currentSpreadAmount;
		private Transform _playerGunHolder;
		private bool _hasBeenPicked = false;

		private void Start()
		{
			_muzzleFlashParticles = MuzzleFlashContainer.GetComponentsInChildren<ParticleSystem>();
			_cooldownParticles = _cooldownContainer.GetComponentsInChildren<ParticleSystem>();

			StopCoolDownEffect();
		}

		private void Update()
		{
			DecreaseSpread();
		}

		public void EmitMuzzleFlash()
		{
			foreach (var m in _muzzleFlashParticles)
			{
				ParticleSystem.EmissionModule emit = m.emission;
				emit.enabled = true;
				float probability = emit.GetBurst(0).probability;
				float rand = Random.Range(0, 10) / 10;
				if (rand <= probability)
				{
					m.Emit(Random.Range(emit.GetBurst(0).minCount, emit.GetBurst(0).maxCount));
				}
			}

			if (_currentHeatAmount < _maxHeatParticles)
			{
				_currentHeatAmount += _maxHeatParticles / 10;
			}
		}

		public void StopMuzzleFlash()
		{
			foreach (var m in _muzzleFlashParticles)
			{
				ParticleSystem.EmissionModule emit = m.emission;
				m.Clear();
				emit.enabled = false;
			}
		}

		public void HandleCoolDown()
		{
			foreach (var m in _cooldownParticles)
			{
				ParticleSystem.EmissionModule emit = m.emission;
				ParticleSystem.MinMaxCurve rate = emit.rateOverTime;
				emit.enabled = true;
				rate.constant = _currentHeatAmount;
			}
		}

		private void StopCoolDownEffect()
		{
			foreach (var i in _muzzleFlashParticles)
			{
				ParticleSystem.EmissionModule emit = i.emission;
				emit.enabled = false;
			}
			foreach (var i in _cooldownParticles)
			{
				ParticleSystem.EmissionModule emit = i.emission;
				ParticleSystem.MinMaxCurve rate = emit.rateOverTime;
				rate.constant = 0;
				emit.enabled = false;
			}
		}

		public void Equip()
		{
			_hasBeenPicked = true;
			GetComponent<Rigidbody>().isKinematic = true;
			GetComponent<CapsuleCollider>().enabled = false;
		}

		public void Initiate(Transform playerGunHolder)
		{
			_playerGunHolder = playerGunHolder;
		}

		public void Fire()
		{
			ShootBullet();
			EmitMuzzleFlash();
			_bulletShellParticle.Emit(1);
			IncreaseSpread();
		}

		private void ShootBullet()
		{
			float xSpread = Random.Range(-1, 1);
			float ySpread = Random.Range(-1, 1);
			Vector3 spread = new Vector3(xSpread, ySpread, 0.0f).normalized * specifics.recoilInfo.MaxConeSpreadSize;
			Quaternion rotation = Quaternion.Euler(spread) * BulletSpawnPoint.rotation;
			var bullet = Instantiate(specifics.bulletPrefab, BulletSpawnPoint.position, rotation) as Bullet;
			bullet.InitiateAndShoot(specifics.bulletSpecifics);
		}

		private void DecreaseSpread()
		{
			if (_currentSpreadAmount > specifics.recoilInfo.MinConeSpreadSize)
			{
				_currentSpreadAmount -= specifics.recoilInfo.SpreadBackupSpeed * Time.deltaTime;
			}
			else
			{
				_currentSpreadAmount = specifics.recoilInfo.MinConeSpreadSize;
			}
		}

		private void IncreaseSpread()
		{
			if (_currentSpreadAmount < specifics.recoilInfo.MaxConeSpreadSize)
			{
				_currentSpreadAmount += specifics.recoilInfo.SpreadIncrementAmountPerBulletShoot;
			}
			else
			{
				_currentSpreadAmount = specifics.recoilInfo.MaxConeSpreadSize;
			}
		}
	}
}