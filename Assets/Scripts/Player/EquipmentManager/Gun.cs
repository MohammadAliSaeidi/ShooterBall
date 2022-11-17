using Unity.Netcode;
using UnityEngine;

namespace BallShooter.Player
{
	public sealed class Gun : Equipment
	{
		public GunSpecifics Specifics;
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

		public void Fire(bool isServer)
		{
			ShootBullet(isServer);
			EmitMuzzleFlash();
			_bulletShellParticle.Emit(1);
			IncreaseSpread();
		}

		private void ShootBullet(bool isServer)
		{
			float xSpread = Random.Range(-1, 1);
			float ySpread = Random.Range(-1, 1);
			Vector3 spread = new Vector3(xSpread, ySpread, 0.0f).normalized * Specifics.recoilInfo.MaxConeSpreadSize;
			Quaternion spawnPointRotation = Quaternion.Euler(spread) * BulletSpawnPoint.rotation;

			var bullet = Instantiate(Specifics.bulletPrefab, BulletSpawnPoint.position, spawnPointRotation);
			bullet.IsVisual = !isServer;
			bullet.InitiateAndShoot(Specifics.bulletSpecifics);
		}

		[ClientRpc]
		private void ShootBulletClientRpc(Vector3 spread, Quaternion spawnPointRotation, ulong senderOwnerClientId)
		{
			var bullet = Instantiate(Specifics.bulletPrefab, BulletSpawnPoint.position, spawnPointRotation);
			//bullet.IsVisual = !IsServer;
			bullet.InitiateAndShoot(Specifics.bulletSpecifics);
		}

		private void DecreaseSpread()
		{
			if (_currentSpreadAmount > Specifics.recoilInfo.MinConeSpreadSize)
			{
				_currentSpreadAmount -= Specifics.recoilInfo.SpreadBackupSpeed * Time.deltaTime;
			}
			else
			{
				_currentSpreadAmount = Specifics.recoilInfo.MinConeSpreadSize;
			}
		}

		private void IncreaseSpread()
		{
			if (_currentSpreadAmount < Specifics.recoilInfo.MaxConeSpreadSize)
			{
				_currentSpreadAmount += Specifics.recoilInfo.SpreadIncrementAmountPerBulletShoot;
			}
			else
			{
				_currentSpreadAmount = Specifics.recoilInfo.MaxConeSpreadSize;
			}
		}
	}
}