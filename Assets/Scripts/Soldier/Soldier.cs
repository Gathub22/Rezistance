using UnityEngine;

public class Soldier : MonoBehaviour
{
    public int Health;
    public bool isDead;

    [SerializeField] private int rangeAttack;
    [SerializeField] private int damage;

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
}
