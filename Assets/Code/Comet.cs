using System;
using System.Linq;
using UnityEngine;

public class Comet : MonoBehaviour
{
	public float Speed;

	private Player _player;
	private bool _moving = true;
	private TrailRenderer _trailRenderer;
	private GameObject _particles;
	private GameObject _target;

	public static void SetAllPaused(bool paused)
	{
		foreach (var comet in FindObjectsOfType(typeof(Comet)).Cast<Comet>())
			comet.enabled = !paused;
	}

	public static void Spawn(Room room)
	{
		var newComet = (GameObject)Instantiate(Game.CometPrefab);
		newComet.name = "Comet_" + DateTime.Now.Ticks;
		var rndFloorPos = room.GetRandomPositionOnFloor();
		newComet.transform.position = rndFloorPos + Vector3.up * 40; // TODO make height parameter
	}

	// ReSharper disable UnusedMember.Local
	private void Start()
		// ReSharper restore UnusedMember.Local
	{
		_trailRenderer = GetComponent<TrailRenderer>();
		_particles = transform.FindChild("Particles").gameObject;
		_target = transform.FindChild("Target").gameObject;
		_player = Player.Instance;
		PlaceTarget();
	}

	private void PlaceTarget()
	{
		foreach (var hit in Physics.RaycastAll(transform.position, Vector3.down, float.MaxValue))
		{
			if (hit.collider.tag == "FloorCollider")
			{
				_target.transform.parent = null;
				_target.transform.position = hit.point;
			}
		}
	}

// ReSharper disable UnusedMember.Local
	private void Update()
	// ReSharper restore UnusedMember.Local
	{
		if (_moving)
			transform.position += Vector3.down * Time.deltaTime * Speed;
		if (!_trailRenderer)
			Destroy(gameObject);
	}

	// ReSharper disable UnusedMember.Local
	private void OnTriggerEnter(Collider other)
	// ReSharper restore UnusedMember.Local
	{
		if (other.tag != "FloorCollider" && other != _player.collider)
			return;
		Destroy(_target);
		if (other == _player.collider)
			_player.DecreaseHealth(20);
		_moving = false;
		if (!_particles)
			return;
		_particles.transform.parent = null;
		_particles.particleSystem.Emit(100);
		var autoDestruction = _particles.gameObject.AddComponent<AutoDestruction>();
		autoDestruction.Duration = 3;
	}
}
