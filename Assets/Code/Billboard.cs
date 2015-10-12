using UnityEngine;

public class Billboard : MonoBehaviour
{
	private Camera _playerCamera;

	// ReSharper disable UnusedMember.Local
	private void Start()
	// ReSharper restore UnusedMember.Local
	{
		_playerCamera = Player.Instance.Camera;
	}

	// ReSharper disable UnusedMember.Local
	private void Update()
	// ReSharper restore UnusedMember.Local
	{
		transform.LookAt(_playerCamera.transform.position);
	}
}
