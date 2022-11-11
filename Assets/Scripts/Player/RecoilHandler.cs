using UnityEngine;

public class RecoilHandler : MonoBehaviour
{
	[SerializeField] private SmoothMouseLook _mouseLook;
	[SerializeField] private Camera _camera;
	[SerializeField] private float _recoilDuration = 0.2f;
	[SerializeField] private Cinemachine.CinemachineImpulseSource cameraShake;
	
	private float _recoilTimeout;
	private float _horizontalRecoil, _verticalRecoil;

	private void Update()
	{
		if (_recoilTimeout > 0)
		{
			ApplyRecoil();
			_recoilTimeout -= Time.deltaTime;
		}
		else
		{
			_recoilTimeout = 0;
		}
	}

	public void GenerateRecoil(float horizontalRecoil, float verticalRecoil)
	{
		_recoilTimeout = _recoilDuration;

		if (cameraShake != null && _camera)
		{
			cameraShake.GenerateImpulse(_camera.transform.forward);
		}

		_horizontalRecoil = Random.Range(-_horizontalRecoil, _horizontalRecoil);
		_verticalRecoil = Random.Range(verticalRecoil, verticalRecoil * 0.8f);
	}

	private void ApplyRecoil()
	{
		_mouseLook.HandleLook(_verticalRecoil * Time.deltaTime / _recoilDuration, _horizontalRecoil * Time.deltaTime / _recoilDuration);
	}
}
