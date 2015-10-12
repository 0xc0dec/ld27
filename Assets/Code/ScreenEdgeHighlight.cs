using UnityEngine;

public class ScreenEdgeHighlight : MonoBehaviour
{
	public static ScreenEdgeHighlight Instance { get { return Player.Instance.Camera.GetComponent<ScreenEdgeHighlight>(); } }

	public Material Material;

	private float _timer;
	private float _duration;

	public void Highlight(Color color, float duration)
	{
		_timer = 0;
		Material.SetColor("_Color", color);
	}

	// ReSharper disable UnusedMember.Local
	private void Start()
	// ReSharper restore UnusedMember.Local
	{
		_duration = 1;
		_timer = _duration + 1;
	}

	// ReSharper disable UnusedMember.Local
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	// ReSharper restore UnusedMember.Local
	{
		if (_timer < _duration)
		{
			_timer += Time.deltaTime;
			Material.SetFloat("_Strength", 1 - _timer / _duration);
		}
		Graphics.Blit(src, dest, Material);
	}
}
