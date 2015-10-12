using UnityEngine;

public class FloatingSpectator : MonoBehaviour
{
	public float MaxDeviation = 0.025f;
	public GameObject Target;

	private float _startPhase;
	private Vector3 _startPosition;

	// ReSharper disable UnusedMember.Local
	private void Start()
	// ReSharper restore UnusedMember.Local
	{
		_startPhase = 2 * Mathf.PI * Game.GetRandomFloat(1);
		_startPosition = gameObject.transform.localPosition;
	}

	// ReSharper disable UnusedMember.Local
	private void Update()
	// ReSharper restore UnusedMember.Local
	{
		var deviation = MaxDeviation * Mathf.Sin(_startPhase + Time.time);
		gameObject.transform.position = new Vector3(_startPosition.x, _startPosition.y + deviation, _startPosition.z);
		gameObject.transform.LookAt(Target.transform);
	}
}
