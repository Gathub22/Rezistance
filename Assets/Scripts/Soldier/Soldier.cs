using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int rangeAttack;
    [SerializeField] private int damage;
    public bool isDead;

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
}
