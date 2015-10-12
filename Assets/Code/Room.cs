using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
	public static Room Current { get; private set; }
	public static Room Prev { get; private set; }
	public static Room PrevPrev { get; private set; }

	public Transform Pivot { get { return transform.FindChild("Pivot").transform; } }
	public Door Door { get { return transform.FindChild("Door").GetComponent<Door>(); } }
	public TextIndicator TimerIndicator { get { return transform.FindChild("Timer indicator text").GetComponent<TextIndicator>(); } }
	public DoorLight DoorLight { get { return transform.GetComponentInChildren<DoorLight>(); } }

	public bool SpawningGatherables { get; set; }
	public bool SpawningComets { get; set; }

	public int GatherablesCount { get; set; }

	private static int _roomsCount;

	private float _cometTimer;
	private float _gatherableTimer;
	private bool _firstGatherable;
	private GameObject _floor;

	public static void ResetCount()
	{
		_roomsCount = 0;
	}

	public static Room Spawn()
	{
		if (Current == null || Current.Pivot == null)
		{
			Debug.Log("Unable to spawn room: no current room, or no pivot");
			return null;
		}

		var newRoom = (GameObject)Instantiate(Game.RoomPrefab);

		var removableWallSegments = GetRemovableWallSegments(newRoom).ToArray();
		if (removableWallSegments.Length == 0)
		{
			Debug.Log("Unable to spawn room: no removable wall segments found");
			Destroy(newRoom);
			return null;
		}

		newRoom.name = "Room " + _roomsCount++;

		var rndIdx = Game.GetRandomInt(removableWallSegments.Length);
		var randomSegment = removableWallSegments[rndIdx];
		randomSegment.renderer.enabled = false;
		randomSegment.collider.enabled = false;

		var host = new GameObject("Room host");
		host.transform.position = randomSegment.transform.position;
		host.transform.rotation = randomSegment.transform.rotation;
		newRoom.transform.parent = host.transform;

		var pivot = Current.Pivot.transform;
		host.transform.position = pivot.position;
		host.transform.rotation = pivot.rotation;

		newRoom.transform.parent = null;
		Destroy(host);

		PrevPrev = Prev;
		Prev = Current;
		Current = newRoom.GetComponent<Room>();

		return Current;
	}

	public Vector3 GetRandomPositionOnFloor()
	{
		var x = _floor.collider.bounds.extents.x * (-1 + Game.GetRandomFloat(2));
		var y = _floor.collider.bounds.extents.y;
		var z = _floor.collider.bounds.extents.z * (-1 + Game.GetRandomFloat(2));
		return _floor.transform.position + new Vector3(x, y, z);
	}

	public Vector3 GetCenterFloorPosition()
	{
		return _floor.transform.position + Vector3.up * _floor.collider.bounds.extents.y;
	}

	private static IEnumerable<GameObject> GetRemovableWallSegments(GameObject room)
	{
		return room.transform.GetComponentsInChildren<RemovableWallSegment>().Select(rws => rws.gameObject);
	}

	// ReSharper disable UnusedMember.Local
	private void Start()
	// ReSharper restore UnusedMember.Local
	{
		if (Current == null)
			Current = this;
		_floor = transform.FindChild("Floor collider").gameObject;
		SpawningGatherables = true;
		_firstGatherable = true;
	}

	// ReSharper disable UnusedMember.Local
	private void Update()
	// ReSharper restore UnusedMember.Local
	{
		if (SpawningComets)
			SpawnCometPeriodically();
		if (SpawningGatherables)
			SpawnGatherablesPeriodically();
	}

	private void SpawnCometPeriodically()
	{
		_cometTimer += Time.deltaTime;
		if (_cometTimer >= 1.0f / (_roomsCount + 2))
		{
			Comet.Spawn(this);
			_cometTimer = 0;
		}
	}

	private void SpawnGatherablesPeriodically()
	{
		_gatherableTimer += Time.deltaTime;
		if (_gatherableTimer >= 1)
		{
			if (GatherablesCount == 0)
			{
				if (Gatherable.Spawn(this, _firstGatherable) != null)
					_firstGatherable = false;
			}
			_gatherableTimer = 0;
		}
	}
}
