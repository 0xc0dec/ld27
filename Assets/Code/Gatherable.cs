using System;
using UnityEngine;

public class Gatherable : MonoBehaviour
{
	public float FloatingSpeed;
	public float FloatingAplitude;
	public float ApearingDuration;

	private Player _player;
	private Room _room;
	private Vector3 _startPosition;

	public static Gatherable Spawn(Room room, bool first)
	{
		var newGatherableObj = (GameObject)Instantiate(Game.GatherablePrefab);
		newGatherableObj.name = "Gatherable_" + DateTime.Now.Ticks;
		newGatherableObj.transform.position = (first ? room.GetCenterFloorPosition() : room.GetRandomPositionOnFloor()) + Vector3.up * 1;
		if (first)
			newGatherableObj.AddComponent<DoorTrigger>();
		var newGatherable = newGatherableObj.GetComponent<Gatherable>();
		newGatherable._room = room;
		room.GatherablesCount++;
		return newGatherable;
	}

	// ReSharper disable UnusedMember.Local
	private void Start()
	// ReSharper restore UnusedMember.Local
	{
		_player = Player.Instance;
		_startPosition = gameObject.transform.localPosition;
		var scale = transform.localScale;
		transform.localScale = Vector3.zero;
		Task.Create(ApearingDuration)
			.UpdateCallback(t => transform.localScale = Vector3.Lerp(Vector3.zero, scale, t / ApearingDuration))
			.Run();
	}

	// ReSharper disable UnusedMember.Local
	private void Update()
	// ReSharper restore UnusedMember.Local
	{
		var deviation = FloatingAplitude * Mathf.Sin(FloatingSpeed * Time.time);
		gameObject.transform.position = new Vector3(_startPosition.x, _startPosition.y + deviation, _startPosition.z);
	}

	// ReSharper disable UnusedMember.Local
	private void OnTriggerEnter(Collider other)
	// ReSharper restore UnusedMember.Local
	{
		if (other != _player.collider)
			return;
		_player.CollectGatherable();
		_room.GatherablesCount--;
		Destroy(gameObject);
	}
}
