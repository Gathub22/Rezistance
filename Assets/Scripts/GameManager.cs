using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Square[][] Map;

	public void SetMap(Square[][] map) {
		Map = map;
	}
}
