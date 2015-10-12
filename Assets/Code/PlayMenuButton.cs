using UnityEngine;

public class PlayMenuButton : MonoBehaviour
{
	// ReSharper disable UnusedMember.Local
	private void OnMouseDown()
	// ReSharper restore UnusedMember.Local
	{
		Task.Create<FadeOut>(1, false).EndCallback(Game.StartLevel).Run();
		Destroy(this);
	}
}
