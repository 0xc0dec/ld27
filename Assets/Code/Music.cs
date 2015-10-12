using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
	public static Music Instance { get { return GameObject.Find("Music").GetComponent<Music>(); } }

	public void Toggle()
	{
		if (audio.isPlaying)
			audio.Pause();
		else
			audio.Play();
	}
}
