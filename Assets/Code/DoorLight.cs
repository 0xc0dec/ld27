using System.Linq;
using UnityEngine;

public class DoorLight : MonoBehaviour
{
	public float RotationSpeed;

	private Light _light;
	private Renderer _lightEmitterRenderer;

	// ReSharper disable UnusedMember.Local
	private void Start()
	// ReSharper restore UnusedMember.Local
	{
		_light = GetComponentInChildren<Light>();
		_lightEmitterRenderer = FindLightEmitterRenderer();
		_lightEmitterRenderer.material.color = Color.black;
		enabled = false;
		_light.enabled = false;
	}

	// ReSharper disable UnusedMember.Local
	private void Update()
	// ReSharper restore UnusedMember.Local
	{
		transform.Rotate(Vector3.right, Time.deltaTime * RotationSpeed, Space.Self);
	}

	public void SetColor(Color color)
	{
		_light.color = color;
	}

	public void Toggle()
	{
		enabled = !enabled;
		_light.enabled = !_light.enabled;
		_lightEmitterRenderer.material.color = enabled ? Color.white : Color.black;
	}

	private Renderer FindLightEmitterRenderer()
	{
		return transform.GetComponentsInChildren<Renderer>().First(r => r.name == "Light emitter");
	}
}
