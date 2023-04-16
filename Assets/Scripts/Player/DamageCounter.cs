using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCounter : MonoBehaviour
{
    [SerializeField] DamageHandler damageHandler;
    // this script only returns damage
    // all health and respawning is handled in centralized respawn script
    //[SerializeField] int fullHealth = 100;
    //[SerializeField] int currentHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision != null)
        {
            GameObject obj = collision.gameObject;
            Rigidbody2D rb;
            if (obj != null) { rb = obj.GetComponent<Rigidbody2D>();  }
            else return;
            if (obj.tag == "Instant Death")
            {
                damageHandler.Respawn();
            }
            else if (obj.tag == "Enemy")
            {
                print("ENEMYENEMY");
                if (obj.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    damageHandler.TakeDamage(enemy.damage);
                }
            }
        }
    }

}
