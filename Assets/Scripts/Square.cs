using UnityEditor.Animations;
using UnityEngine;

public class Square : MonoBehaviour
{
	public GameObject Child {
		set {
			this.child = value;
			value.transform.position = transform.position;
			value.GetComponent<Renderer>().sortingOrder = 1;
		}

		get {
			return child;
		}
	}

	public Vector3 Position;
	private GameObject child;
}
