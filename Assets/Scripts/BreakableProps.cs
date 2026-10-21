using UnityEngine;

public class BreakableProps : MonoBehaviour
{
    public float health;

    public void TakeDamage(float dmg)
    {
        health -= dmg;

        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
