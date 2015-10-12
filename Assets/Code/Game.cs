using System;
using UnityEngine;
using Random = System.Random;

public class Game : MonoBehaviour
{
	public GUISkin Skin;

	public static Game Instance { get { return GameObject.Find("Game").GetComponent<Game>(); } }

	private static readonly Random _rnd = new Random(DateTime.Now.Second);

	public static GameObject RoomPrefab { get; private set; }
	public static GameObject CometPrefab { get; private set; }
	public static GameObject GatherablePrefab { get; private set; }

	public static void ToMainMenu()
	{
		Application.LoadLevel("Menu");
	}

	public static void StartLevel()
	{
		Room.ResetCount();
		Application.LoadLevel("Main");
	}

	public static void Restart()
	{
		Room.ResetCount();
		Application.LoadLevel(Application.loadedLevel);
	}

	public static int GetRandomInt(int max)
	{
		return _rnd.Next(max);
	}

	public static float GetRandomFloat(float max)
	{
		return (float)_rnd.NextDouble() * max;
	}

	// ReSharper disable UnusedMember.Local
	private void Awake()
	// ReSharper restore UnusedMember.Local
	{
		DontDestroyOnLoad(this);
		PreloadPrefabs();
	}

	private void PreloadPrefabs()
	{
		RoomPrefab = Resources.Load("Room") as GameObject;
		CometPrefab = Resources.Load("Comet") as GameObject;
		GatherablePrefab = Resources.Load("Gatherable") as GameObject;
	}
}
