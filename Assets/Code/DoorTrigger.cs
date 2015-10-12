using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
	private Player _player;

	// ReSharper disable UnusedMember.Local
	private void Start()
	// ReSharper restore UnusedMember.Local
	{
		_player = Player.Instance;
	}

	// ReSharper disable UnusedMember.Local
	private void OnTriggerEnter(Collider other)
	// ReSharper restore UnusedMember.Local
	{
		if (other != _player.collider)
			return;

		var currentRoom = Room.Current;
		if (!currentRoom)
		{
			Debug.Log("No current room");
			return;
		}

		currentRoom.SpawningComets = true;
		currentRoom.DoorLight.Toggle();

		var prevRoom = Room.Prev;

		if (prevRoom)
		{
			var door = prevRoom.Door;
			var vertSize = door.collider.bounds.size.y;
			var timerIndicator = currentRoom.TimerIndicator;
			var speed = vertSize / 10;

			var prevPrevRoom = Room.PrevPrev;

			Task.Create(10)
				.UpdateCallback(time =>
					{
						door.transform.position += speed * Vector3.down * Time.deltaTime;
						timerIndicator.SetText(string.Format("Closing door in: {0}", 10 - (int)time));
					})
				.EndCallback(() =>
					{
						if (prevPrevRoom)
							Destroy(prevPrevRoom.gameObject);
						prevRoom.SpawningComets = false;
						OpenDoor(currentRoom);
					})
				.Run();
		}
		else
			OpenDoor(currentRoom);

		Destroy(this);
	}

	private void OpenDoor(Room currentRoom)
	{
		var nextRoom = Room.Spawn();
		nextRoom.SpawningGatherables = false;

		var door = currentRoom.Door;
		var doorLock = door.Lock;
		var timerIndicator = currentRoom.TimerIndicator;
		var vertSize = door.collider.bounds.size.y;
		var speed = vertSize / 2;

		Task.Create(10)
			.UpdateCallback(time =>
			{
				doorLock.transform.RotateAround(doorLock.transform.position, doorLock.transform.up, Time.deltaTime * 50);
				timerIndicator.SetText(string.Format("Opening door in: {0}", 10 - (int)time));
			})
			.EndCallback(() => Task.Create(2)
				.StartCallback(() => timerIndicator.SetText(string.Format("Opening door...")))
				.UpdateCallback(t => door.transform.position += speed * Vector3.up * Time.deltaTime)
				.EndCallback(() =>
					{
						timerIndicator.SetText("Door is open");
						currentRoom.SpawningGatherables = false;
						nextRoom.SpawningGatherables = true;
						currentRoom.DoorLight.Toggle();
					})
				.Run())
			.Run();
	}
}
