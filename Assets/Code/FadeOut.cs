using UnityEngine;

// Fades SCENE out (not itself :))
public class FadeOut : Task
{
	private Texture2D _black;
	protected bool _inverseAlpha;

	// ReSharper disable UnusedMember.Global
	protected void OnGUI()
	// ReSharper restore UnusedMember.Global
	{
		UITools.PushColor();
		var c = GUI.color;
		var alpha = _inverseAlpha ? 1 - _time / _duration : _time / _duration;
		GUI.color = new Color(c.r, c.g, c.b, alpha);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _black, ScaleMode.StretchToFill);
		UITools.PopColor();
	}

	// ReSharper disable UnusedMemberHiearchy.Global
	protected virtual void Awake()
	// ReSharper restore UnusedMemberHiearchy.Global
	{
		_black = new Texture2D(1, 1, TextureFormat.RGBA32, false, true);
		_black.SetPixel(0, 0, Color.black);
		_black.Apply();
		_inverseAlpha = false;
	}
}
