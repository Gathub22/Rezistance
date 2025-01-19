using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Bot : MonoBehaviour
{

	public int Zombies;
	[SerializeField] private Square base_square;
	[SerializeField] private List<Square> zombie_square_list;
	[SerializeField] private GameManager gameManager;
	[SerializeField] private List<Square> deletingSquares;

	void Start()
	{
		zombie_square_list = new List<Square>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	public void Calculate()
	{
		for (int i = 0; i < zombie_square_list.Count; i++){
			Square z_s = zombie_square_list[i];
			if (z_s.Child == null) {
				deletingSquares.Add(z_s);
				break;
			}
			Vector3 distance = new Vector3(
				(z_s.Position.x - base_square.Position.x),
				(z_s.Position.y - base_square.Position.y)
			);

			if (distance.y > 1) {
				ProcessDistance(1, z_s, false);
			} else if (distance.y < -1) {
				ProcessDistance(-1, z_s, false);
			} else if (distance.x > 1) {
				ProcessDistance(1, z_s, true);
			} else if (distance.x < -1) {
				ProcessDistance(-1, z_s, true);
			} else {
				base_square.ApplyDamage(z_s.Child.GetComponent<Zombie>().damage);
			}
		}

		for(int i = 0; i < deletingSquares.Count; i++) {
			zombie_square_list.Remove(deletingSquares[i]);
		}

		// Zombie spawning
		if (Zombies > 0) {
			int quantity = (int) (gameManager.Round * 1.5);
			if (quantity <= Zombies) {
				for (int i = 0; i < quantity; i++) {

					int x, y;
					do {
						if (Random.value > 0.5f) {
							x = gameManager.Map.Count()-1;
						} else {
							x = 0;
						}

						if (Random.value > 0.5f) {
							y = gameManager.Map[0].Count()-1;
						} else {
							y = 0;
						}
					} while(gameManager.Map[x][y].Child != null);

					Square s = gameManager.Map[x][y];
					s.Child = Instantiate(Resources.Load<GameObject>("Zombie/Zombie"));
					zombie_square_list.Add(s);
				}
			}
		}
	}

	private void ProcessDistance(int distance, Square z_s, bool OnX)
	{
		Soldier sol;
		Square s;
		if (OnX)
			s = gameManager.Map[(int) z_s.Position.x + distance][(int) z_s.Position.y];
		else
			s = gameManager.Map[(int) z_s.Position.x][(int) z_s.Position.y + distance];

		if (s.Child == null) {
			s.Child = z_s.Child;
			z_s.Child = null;
			zombie_square_list.Add(s);
			deletingSquares.Add(z_s);
		} else if ((sol = s.Child.GetComponent<Soldier>()) == null) {
			s.ApplyDamage(sol.damage);
		}
	}
}
