using UnityEngine;

public class TextIndicator : MonoBehaviour
{
	public void SetText(string text)
	{
		GetComponent<TextMesh>().text = text;
	}
}
