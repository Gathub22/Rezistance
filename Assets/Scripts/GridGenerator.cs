using UnityEngine;

public class GridGenerator : MonoBehaviour
{

	public int Height;
	public int Width;
	public float Margin;

	void Start()
	{
		GameObject sqr = Resources.Load<GameObject>("Camp/Square");

		Vector3 sqr_size = sqr.GetComponent<Renderer>().bounds.size;
		float starting_x = transform.position.x - (((sqr_size.x + Margin) * Width - 1)) / 2;
		float starting_y = transform.position.y - (((sqr_size.y + Margin) * Height - 1)) / 2;

		for (int i = 0; i < Width; i++) {
			for (int j = 0; j < Height; j++){
				float x = starting_x + (i * (sqr_size.x + Margin));
				float y = starting_y + (j * (sqr_size.y + Margin));
				Instantiate(sqr, new Vector3(x, y, 0), Quaternion.identity);
			}
		}

	}

}
