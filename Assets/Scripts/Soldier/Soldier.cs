using UnityEngine;

public class Soldier : MonoBehaviour
{
    public int Health;
    public string soldierType;
    public bool isDead;
    public bool isSelected;

    public Square square;
		public int rangeAttack;
    public int damage;

    private GameManager gameManager;

    void Start()
    {

    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            isDead = true;
        }
    }

    public void Die()
    {
        isDead = true;
    }

    // public void Move(Vector3 direction)
    // {
    //     if (square != null)
    //     {
    //         square.Child.set(null);
    //     }
    //     transform.Translate(direction);
    //     square.Child.set(this);
    // }
}
