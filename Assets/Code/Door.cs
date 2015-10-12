using UnityEngine;

public class Door : MonoBehaviour
{
	public GameObject Lock { get { return transform.FindChild("Lock").gameObject; } }
}
