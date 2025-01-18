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
	public Sprite normalSquareSprite;
	public Sprite hightlightSquareSprite;
	public bool IsEnabled = false;

	void Start()
	{
		if (_child == null) {
			HealthText.text = "";
		}
	}

	public void EnableOverlay()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = hightlightSquareSprite;
		IsEnabled = true;
	}

	public void DisableOverlay()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = normalSquareSprite;
		IsEnabled = false;
	}

	private void OnMouseOver()
	{
		if (Child != null)
		{
			EnableOverlay();
		}
	}

	private void OnMouseExit()
	{
		if (Child != null)
		{
			DisableOverlay();
		}
	}

	private void OnMouseDown()
	{
		Inventory i = GameObject.Find("TroopsInventory").GetComponent<Inventory>();
		GameObject s = i.SelectedSoldier;

		if (s != null && IsEnabled && Child == null) {
			Child = Instantiate(s);
			i.UnselectUnit(true);
		}

	}
}
