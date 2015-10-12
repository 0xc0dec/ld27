using UnityEngine;

public class AutoDestruction : MonoBehaviour
{
	public float Duration;

	private float _timer;

	// ReSharper disable UnusedMember.Local
	private void Update()
	// ReSharper restore UnusedMember.Local
	{
		_timer += Time.deltaTime;
		if (_timer >= Duration)
			Destroy(gameObject);
	}
}
