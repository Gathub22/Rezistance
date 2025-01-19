using NUnit.Framework;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class Square : MonoBehaviour
{
	public GameObject Child {
		set {
			_child = value;

			if (value == null) {
				HealthText.text = "";
				return;
			}

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
	public bool IsUsable = true;

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

	public void DetectNeighbourSquares()
	{
		GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		Soldier sol =  Child.GetComponent<Soldier>();

		for (int i = (int) Position.x - sol.rangeAttack; i < Position.x + sol.rangeAttack; i++) {
			try {
				if (gm.Map[i][(int) Position.y].Child.GetComponent<Zombie>() != null) {
					gm.Map[i][(int) Position.y].EnableOverlay();
				}
			} catch{}

			if (i - 1 == Position.x || i + 1 == Position.x) {
				gm.Map[i][(int) Position.y].EnableOverlay();
			}
		}

		for (int i = (int) Position.y - sol.rangeAttack; i < Position.y + sol.rangeAttack; i++) {
			try {
				if (gm.Map[(int) Position.x][i].Child.GetComponent<Zombie>() != null) {
					gm.Map[(int) Position.x][i].EnableOverlay();
				}
			} catch{}

			if (i - 1 == Position.y || i + 1 == Position.y) {
				gm.Map[(int) Position.x][i].EnableOverlay();
			}
		}
	}

	public void ApplyDamage(int damage)
	{
		Soldier soldier = Child.GetComponent<Soldier>();
		if (soldier != null) {
			soldier.Health -= damage;
			if (soldier.Health < 1) {
				Destroy(Child);
			}
			return;
		}

		Base b = Child.GetComponent<Base>();
		if (b != null) {
			b.Health -= damage;
			if (b.Health < 1) {
				Destroy(Child);
				GameObject.Find("GameManager").GetComponent<GameManager>().LoseRound();
			}
			return;
		}
	}
}
