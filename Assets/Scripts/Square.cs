using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class Square : MonoBehaviour
{
	public GameObject Child {
		set {
			this._child = value;
			value.transform.position = transform.position;
			value.GetComponent<Renderer>().sortingOrder = 1;

			Zombie z = value.GetComponent<Zombie>();
			if (z != null) {
				HealthText.text = z.Health.ToString();
				return;
			}

			Soldier s = value.GetComponent<Soldier>();
			if (s != null) {
				HealthText.text = s.Health.ToString();
				return;
			}

			Base b = value.GetComponent<Base>();
			if (b != null) {
				HealthText.text = b.Health.ToString();
				return;
			}
		}

		get {
			return _child;
		}
	}

	public Vector3 Position;
	public TMP_Text HealthText;
	public GameObject _child;

	void Start()
	{
		if (_child == null) {
			HealthText.text = "";
		}
	}
	private GameObject child;

	private void OnMouseOver()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		if (spriteRenderer != null && Child != null)
		{
			spriteRenderer.sprite = hightlightSquareSprite;
		}
	}

	private void OnMouseExit()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		if (spriteRenderer != null && Child != null)
		{
			spriteRenderer.sprite = normalSquareSprite;
		}
	}

	public Sprite normalSquareSprite;
	public Sprite hightlightSquareSprite;
}
