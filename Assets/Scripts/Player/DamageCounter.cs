using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCounter : MonoBehaviour
{
    [SerializeField] int fullHealth = 100;
    [SerializeField] int currentHealth = 100;
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
                print("You died");
            }
            else if (obj.tag == "Enemy")
            {
                // implement damage system
            }
        }
    }
}
