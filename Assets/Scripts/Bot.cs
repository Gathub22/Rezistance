using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

		Zombies = (int) (gameManager.Round * 1.5f) + 5;
		gameManager.Zombies = Zombies;

		int half_x = gameManager.Map.Count()/2;
		if(half_x < gameManager.Map.Count()/2) {
			half_x++;
		} else if(half_x > gameManager.Map.Count()/2){
			half_x--;
		}

		int half_y = gameManager.Map[0].Count()/2;
		if(half_y < gameManager.Map[0].Count()/2) {
			half_y++;
		} else if(half_y > gameManager.Map[0].Count()/2){
			half_y--;
		}

		base_square = gameManager.Map[half_x][half_y];
	}

	public void Calculate()
	{
		List<Square> fixed_zombie_list = zombie_square_list.Select(item => item).ToList();
		for (int i = 0; i < fixed_zombie_list.Count; i++){
			Square z_s = fixed_zombie_list[i];
			if (z_s.Child == null) {
				// deletingSquares.Add(z_s);
				zombie_square_list.Remove(z_s);
				break;
			}
			Vector3 distance = new Vector3(
				(z_s.Position.x - base_square.Position.x),
				(z_s.Position.y - base_square.Position.y)
			);

			if (distance.y > 1) {
				ProcessDistance(-1, z_s, false);
			} else if (distance.y < -1) {
				ProcessDistance(1, z_s, false);
			} else if (distance.x > 1) {
				ProcessDistance(-1, z_s, true);
			} else if (distance.x < -1) {
				ProcessDistance(1, z_s, true);
			} else {
				base_square.ApplyDamage(z_s.Child.GetComponent<Zombie>().damage);
			}
		}


		// Zombie spawning
		if (Zombies > 0) {
			int quantity = (int) (gameManager.Round * 1.5);
			if (quantity <= Zombies) {
				SpawnZombies(quantity);
				Zombies -= quantity;
			} else {
				SpawnZombies(Zombies);
				Zombies = 0;
			}
		}

		gameManager.EndTurn();
	}

	private void SpawnZombies(int quantity)
	{
		for (int i = 0; i < quantity; i++) {

			int x, y;
			do {
				if (Random.value > 0.5f) {
					x = Random.Range(0, gameManager.Map.Count() - 1);
					y = (int) Random.value;
				} else {
					x = (int) Random.value;
					y = Random.Range(0, gameManager.Map[0].Count() - 1);
				}
			} while(gameManager.Map[x][y].Child != null);

			Square s = gameManager.Map[x][y];
			s.Child = Instantiate(Resources.Load<GameObject>("Zombie/Zombie"));
			zombie_square_list.Add(s);
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
			zombie_square_list.Remove(z_s);
		} else if ((sol = s.Child.GetComponent<Soldier>()) != null) {
			s.ApplyDamage(sol.damage);
		}
	}
}
