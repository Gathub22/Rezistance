using UnityEngine;

public class Soldier : MonoBehaviour
{
    public int Health;
    public bool isDead;

    [SerializeField] private int rangeAttack;
    [SerializeField] private int damage;
    public string soldierType;
    public bool isDead;
    public bool isSelected;

    public Square square;
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
