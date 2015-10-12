using System.Collections.Generic;
using UnityEngine;

public static class UITools
{
	private static readonly Stack<Color> _colors = new Stack<Color>();
	private static readonly Texture2D _blackPixelTex;

	private static GUIStyle _labelStyle;
	private static GUIStyle _buttonStyle;

	static UITools()
	{
		_blackPixelTex = new Texture2D(1, 1);
		_blackPixelTex.SetPixel(0, 0, Color.black);
		_blackPixelTex.Apply();

		_labelStyle = Game.Instance.Skin.FindStyle("label");
		_buttonStyle = Game.Instance.Skin.FindStyle("button");
	}

	public static void Label(Rect pos, string text)
	{
		GUI.Label(pos, text, _labelStyle);
	}

	public static bool Button(Rect pos, string text)
	{
		return GUI.Button(pos, text, _buttonStyle);
	}

	public static void PushColor()
	{
		_colors.Push(GUI.color);
	}

	public static void PopColor()
	{
		GUI.color = _colors.Pop();
	}

	public static void ShadeScreen(float alpha)
	{
		PushColor();
		var c = GUI.color;
		GUI.color = new Color(c.r, c.g, c.b, alpha);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _blackPixelTex, ScaleMode.StretchToFill);
		PopColor();
	}
}
