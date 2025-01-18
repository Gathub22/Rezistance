using NUnit.Framework;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{

	public int Height;
	public int Width;
	public float Margin;

	void Start()
	{
		Square[][] map = new Square[Width][];
		for (int i = 0; i < map.Length; i++) {
			map[i] = new Square[Height];
		}

		GameObject sqr = Resources.Load<GameObject>("Camp/Square");

		Vector3 sqr_size = sqr.GetComponent<Renderer>().bounds.size;
		float starting_x = transform.position.x - (((sqr_size.x + Margin) * Width - 1)) / 2;
		float starting_y = transform.position.y - (((sqr_size.y + Margin) * Height - 1)) / 2;

		for (int i = 0; i < Width; i++) {
			for (int j = 0; j < Height; j++){
				float x = starting_x + (i * (sqr_size.x + Margin));
				float y = starting_y + (j * (sqr_size.y + Margin));
				map[i][j] = Instantiate(sqr, new Vector3(x, y, 0), Quaternion.identity).GetComponent<Square>();
				map[i][j].Position = new Vector3(i,j,0);
			}
		}

		int half_x = Width/2;
		if(half_x < Width/2) {
			half_x++;
		} else if(half_x > Width/2){
			half_x--;
		}

		int half_y = Height/2;
		if(half_y < Height/2) {
			half_y++;
		} else if(half_y > Height/2){
			half_y--;
		}

		map[half_x][half_y].Child = Instantiate(Resources.Load<GameObject>("Camp/Base"), transform.position, Quaternion.identity);

		GameObject.Find("GameManager").GetComponent<GameManager>().SetMap(map);
	}
}
