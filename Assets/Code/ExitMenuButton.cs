using UnityEngine;

public class ExitMenuButton : MonoBehaviour
{
	// ReSharper disable UnusedMember.Local
	private void OnMouseDown()
	// ReSharper restore UnusedMember.Local
	{
		if (Application.isWebPlayer || Application.isEditor)
			return;
		Task.Create<FadeOut>(1, false).EndCallback(Application.Quit).Run();
		Destroy(this);
	}
}
