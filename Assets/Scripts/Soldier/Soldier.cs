using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] private int health;
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
        health -= damage;
        if (health <= 0)
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
