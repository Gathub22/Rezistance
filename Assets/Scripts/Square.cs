using TMPro;
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

	[Header("Audio")]
	public AudioSource biteSound;

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
			if (Child.GetComponent<Soldier>()){
				EnableOverlay();
			}
		}
	}

	private void OnMouseExit()
	{
		if (Child != null)
		{
			if (Child.GetComponent<Soldier>()){
				DisableOverlay();
			}
		}
	}

	public void DetectNeighbourSquares()
	{
		GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		Soldier sol =  Child.GetComponent<Soldier>();

		for (int i = (int) Position.x - sol.rangeAttack; i <= Position.x + sol.rangeAttack; i++) {
			Square s = null;

			try {
				s = gm.Map[i][(int) Position.y];
				if (s.Child.GetComponent<Zombie>() != null) {
					s.EnableOverlay();
				}
			} catch{}

			if ((i - 1 == Position.x || i + 1 == Position.x) && s != null) {
				if (s.Child == null){
					s.EnableOverlay();
				}
			}
		}

		for (int i = (int) Position.y - sol.rangeAttack; i <= Position.y + sol.rangeAttack; i++) {
			Square s = null;

			try {
				s = gm.Map[(int) Position.x][i];
				if (s.Child.GetComponent<Zombie>() != null) {
					s.EnableOverlay();
				}
			} catch{}

			if ((i - 1 == Position.y || i + 1 == Position.y) && s != null) {
				if (s.Child == null){
					s.EnableOverlay();
				}
			}
		}
	}

	public void ApplyDamage(int damage)
	{
		Zombie zombie = Child.GetComponent<Zombie>();
		if (zombie != null) {
			zombie.Health -= damage;
			HealthText.text = zombie.Health.ToString();
			if (zombie.Health < 1) {
				Destroy(Child);
				GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
				gm.Points += 3;
				gm.Zombies--;
				HealthText.text = "";
			}

			return;
		}


		Soldier soldier = Child.GetComponent<Soldier>();
		if (soldier != null) {
			biteSound.Play();
			soldier.Health -= damage;
			HealthText.text = soldier.Health.ToString();
			if (soldier.Health < 1) {
				Destroy(Child);
				HealthText.text = "";
			}
			return;
		}

		Base b = Child.GetComponent<Base>();
		if (b != null) {
			biteSound.Play();
			b.Health -= damage;
			HealthText.text = b.Health.ToString();
			if (b.Health < 1) {
				Destroy(Child);
				HealthText.text = "";
				GameObject.Find("GameManager").GetComponent<GameManager>().LoseRound();
			}
			return;
		}

	}
}
