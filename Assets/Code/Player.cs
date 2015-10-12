using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player Instance { get { return GameObject.Find("Player").GetComponent<Player>(); } }

	public Camera Camera { get { return transform.FindChild("Player camera").camera; } }

	private float _healthPercent;
	private int _gatherablesCount;
	private float _time;
	private bool _dead;
	private bool _pause;

	public void DecreaseHealth(float amountPercent)
	{
		if (_dead || _pause)
			return;
		_healthPercent -= amountPercent;
		ScreenEdgeHighlight.Instance.Highlight(Color.red, 1);
		if (_healthPercent <= 0)
		{
			_healthPercent = 0;
			Die();
		}
	}

	public void CollectGatherable()
	{
		if (_dead || _pause)
			return;
		_gatherablesCount++;
		ScreenEdgeHighlight.Instance.Highlight(Color.blue, 1);
	}

	private void Die()
	{
		SetEnabled(false);
		_dead = true;
	}

	// ReSharper disable UnusedMember.Local
	private void Start()
	// ReSharper restore UnusedMember.Local
	{
		Task.Create<FadeIn>(1).EndCallback(Music.Instance.Toggle).Run();
		_healthPercent = 100;
	}

	// ReSharper disable UnusedMember.Local
	private void Update()
	// ReSharper restore UnusedMember.Local
	{
		if (!_dead && !_pause)
			_time += Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			_pause = !_pause;
			Comet.SetAllPaused(_pause);
			Music.Instance.Toggle();
			SetEnabled(!_pause);
		}
	}

	private void SetEnabled(bool isEnabled)
	{
		var player = GetComponent<vp_FPSPlayer>();
		player.LockCursor = isEnabled;
		player.Camera.enabled = isEnabled;
		player.Controller.enabled = isEnabled;
		player.enabled = isEnabled;
	}

	// ReSharper disable UnusedMember.Local
	private void OnGUI()
	// ReSharper restore UnusedMember.Local
	{
		if (_dead || _pause)
			DrawPauseUI();
		else
		{
			DrawHealthUI();
			DrawGatherablesUI();
			DrawTimeUI();
		}
	}

	private void DrawPauseUI()
	{
		UITools.ShadeScreen(0.8f);
		var gatherablesWord = _gatherablesCount == 1 ? "sphere" : "spheres";
		if (_dead)
			UITools.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2.0f - 25, 150, 50), "Dead");
		UITools.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2.0f, 600, 60), string.Format("Collected {0} {1} in {2:F} seconds", _gatherablesCount, gatherablesWord, _time));
		if (UITools.Button(new Rect(Screen.width / 2 - 120, Screen.height / 2 + 60, 100, 50), "Restart"))
			Task.Create<FadeOut>(1, false).EndCallback(Game.Restart).Run();
		if (UITools.Button(new Rect(Screen.width / 2 + 20, Screen.height / 2 + 60, 100, 50), "Menu"))
			Task.Create<FadeOut>(1, false).EndCallback(Game.ToMainMenu).Run();
	}

	private void DrawHealthUI()
	{
		UITools.Label(new Rect(5, 5, 150, 50), "Health: " + _healthPercent);
	}

	private void DrawGatherablesUI()
	{
		UITools.Label(new Rect(Screen.width - 150, 5, 150, 50), "Spheres: " + _gatherablesCount);
	}

	private void DrawTimeUI()
	{
		UITools.Label(new Rect(Screen.width / 2 - 75, 5, 150, 50), string.Format("Time: {0:F}", _time));
	}
}
