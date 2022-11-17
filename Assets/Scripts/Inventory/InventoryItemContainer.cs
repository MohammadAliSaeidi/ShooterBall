using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryItemContainer : MonoBehaviour
{
	[Range(0.01f, 10)]
	[SerializeField]
	private float _rotationVelocityMultiplyer = 1;

	[SerializeField]
	private float _rotationSmooth = 1;

	[SerializeField]
	private Transform _pivot;

	private PlayerControls _playerControls;
	private Vector2 rotationVelocity;
	private bool _dragging = false;
	private bool autoRotation;

	private void Awake()
	{
		_playerControls = new PlayerControls();
		_playerControls.MainMenu.LeftClick.performed += (InputAction.CallbackContext context) => CheckForRotation();
		_playerControls.MainMenu.LeftClick.canceled += (InputAction.CallbackContext context) => _dragging = false;
	}

	private void Start()
	{
		_dragging = false;
		autoRotation = true;
	}

	private void OnEnable()
	{
		_playerControls.Enable();
	}

	private void OnDisable()
	{
		_playerControls.Disable();
	}

	private void CheckForRotation()
	{
		var rayOrigin = Camera.main.ScreenToWorldPoint(_playerControls.MainMenu.MousePosition.ReadValue<Vector2>());
		var ray = new Ray(rayOrigin, Camera.main.transform.forward);
		if (Physics.Raycast(ray, out RaycastHit hitInfo))
		{
			if (hitInfo.collider.gameObject == this.gameObject)
			{
				_dragging = true;
			}
			else
			{
				_dragging = false;
			}
		}
	}

	private void Update()
	{
		Vector2 mouse = Vector2.zero;
		if (_dragging)
		{
			mouse = new Vector2(_playerControls.MainMenu.MouseDelta.ReadValue<Vector2>().y, _playerControls.MainMenu.MouseDelta.ReadValue<Vector2>().x);
			rotationVelocity = mouse / 10 * _rotationVelocityMultiplyer;
			if (alignHorizontal != null)
			{
				StopCoroutine(alignHorizontal);
				alignHorizontal = null;
			}
			autoRotation = false;
		}
		else if (!autoRotation)
		{
			rotationVelocity = Vector2.Lerp(rotationVelocity, mouse / 10 * _rotationVelocityMultiplyer, Time.deltaTime * _rotationSmooth);
		}

		if ((rotationVelocity.magnitude < 0.01f || autoRotation) && !_dragging)
		{
			rotationVelocity.y = Mathf.Lerp(rotationVelocity.y, 0.14f, Time.deltaTime * _rotationSmooth);

			if (alignHorizontal == null)
			{
				alignHorizontal = StartCoroutine(Co_AlignHorizontal());
			}

			autoRotation = true;
		}

		_pivot.Rotate(new Vector3(rotationVelocity.x, 0, 0), Space.World);
		_pivot.Rotate(new Vector3(0, rotationVelocity.y, 0), Space.Self);
	}

	private Coroutine alignHorizontal;
	private IEnumerator Co_AlignHorizontal()
	{
		while (_pivot.eulerAngles.x >= 0.01f)
		{
			_pivot.localEulerAngles = Vector3.Lerp(_pivot.localEulerAngles, new Vector3(0, _pivot.localEulerAngles.y, 0), Time.deltaTime * _rotationSmooth);
			yield return null;
		}
	}
}
