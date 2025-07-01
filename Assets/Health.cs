using UnityEngine;

public class Health : MonoBehaviour, IHealable, IDamageable
{
    public float max_health = 100f;

    public float _health;

    public float health
    {
        get { return _health; }
        set
        {
            _health = Mathf.Clamp(value, 0f, max_health);

            if (_health <= 0f)
            {
                Die();
            }
        }
    }

    public Animator anim;
    private void Start()
    {
        health = max_health; // Initialize health to max_health at the start
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) // For testing purposes
        {
            TakeDamage(10f); // Simulate taking damage
        }
        if (Input.GetKeyDown(KeyCode.N)) // For testing purposes
        {
            Heal(5f); // Simulate healing
        }
    }
    void Die()
    {
        // Handle death logic here
        Debug.Log($"{gameObject.name} has died.");
        // Optionally, you can disable the game object or trigger a death animation
        //gameObject.SetActive(false);
        if (anim != null)
        {
            anim.SetTrigger("Die"); // Trigger death animation if Animator is set
        }
    }
    public void TakeDamage(float damage)
    {
        if (health > 0f)
        {
            health -= damage;
        }
        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {health}");
    }
    public void Heal(float amount)
    {
        health += amount;
        Debug.Log($"{gameObject.name} healed {amount}. Current health: {health}");
    }
}
