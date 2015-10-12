using System;
using UnityEngine;

public class Task : MonoBehaviour
{
	protected float _duration;
	protected float _time;

	private Action _start;
	private Action<float> _update;
	private Action _end;
	private bool _started;
	private bool _autoDestroy;

	private static GameObject _host;

	public static Task Create(float duration, bool autoDestroy = true)
	{
		InitHost();
		var job = _host.AddComponent<Task>();
		job._duration = duration;
		job._autoDestroy = autoDestroy;
		job._started = false;
		return job;
	}

	public static T Create<T>(float duration, bool autoDestroy = true) where T : Task
	{
		InitHost();
		var task = _host.AddComponent<T>();
		task._duration = duration;
		task._autoDestroy = autoDestroy;
		return task;
	}

	public Task StartCallback(Action callback)
	{
		_start = callback;
		return this;
	}

	public Task UpdateCallback(Action<float> callback)
	{
		_update = callback;
		return this;
	}

	public Task EndCallback(Action callback)
	{
		_end = callback;
		return this;
	}

	public void Run()
	{
		_started = true;
		if (_start != null)
			_start();
		StartImpl();
	}

	protected virtual void UpdateImpl()
	{
	}

	protected virtual void StartImpl()
	{
	}

	protected virtual void FinishImpl()
	{
	}

	private static void InitHost()
	{
		if (!_host)
			_host = new GameObject("Tasks");
	}

	// ReSharper disable UnusedMember.Local
	private void Update()
	// ReSharper restore UnusedMember.Local
	{
		if (!_started)
			return;
		if (_update != null)
			_update(_time);
		UpdateImpl();
		// The job will continue working even after _time >= _duration.
		// Actually its time will stop increasing.
		if (_time >= _duration)
			return;
		_time += Time.deltaTime;
		if (_time >= _duration)
		{
			if (_end != null)
				_end();
			FinishImpl();
			if (_autoDestroy)
				Destroy(this);
		}
	}
}
