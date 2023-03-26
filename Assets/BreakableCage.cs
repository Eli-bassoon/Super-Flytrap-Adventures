using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakableCage : MonoBehaviour
{
    [SerializeField] int maxHealth = 5;
    [SerializeField] int health;
    [SerializeField] int dmg = 0;
    [SerializeField] float speedThreshold = 1.0f;

    int numChildren;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth - dmg;
        numChildren = gameObject.transform.childCount;
        for (int i = 0; i < numChildren; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).GameObject();
            child.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        health = maxHealth - dmg;
        if (health <= 0)
        {
            //gameObject.SetActive(false);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            for (int i = 0; i < numChildren; i++)
            {
                GameObject child = gameObject.transform.GetChild(i).gameObject;
                child.SetActive(true);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;
        GameObject obj = collision.gameObject;
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (obj.CompareTag("Center in Mouth") && rb.velocity.magnitude >= speedThreshold)
        {
            dmg++;
        }
    }
}
