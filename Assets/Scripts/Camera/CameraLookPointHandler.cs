using UnityEngine;

public class CameraLookPointHandler : MonoBehaviour
{
	public Vector3 PointOfCameraLooking { get; private set; }
	[SerializeField] private Camera _camera;
	[SerializeField] private LayerMask _cameraPointOfLookingLayerMask;

	private readonly float _maxRayDist = 500;

	private void Update()
	{
		if (_camera)
		{
			RaycastHit hit = new RaycastHit();
			Ray cameraRay = new Ray(_camera.transform.position, _camera.transform.forward);

			if (Physics.Raycast(cameraRay, out hit, _maxRayDist, _cameraPointOfLookingLayerMask))
			{
				PointOfCameraLooking = hit.point;
			}
			else
			{
				PointOfCameraLooking = _camera.transform.forward * _maxRayDist;
			}
		}
	}
}
