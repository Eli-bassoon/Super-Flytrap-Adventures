using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BreakableCage : MonoBehaviour
{
    [SerializeField] int maxHealth = 1;
    [SerializeField] int health;
    [SerializeField] float speedThreshold = 1.0f;
    [SerializeField] float explodeSpeed = 0.5f;
    [SerializeField] float beforeFadeTime = 2f;
    [SerializeField] float fadeTime = 1f;

    bool exploded = false;
    int numChildren;
    LayerMask ghostLayer;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        numChildren = gameObject.transform.childCount;
        ghostLayer = LayerMask.NameToLayer("Ghost");
    }

    // Update is called once per frame
    void Update()
    {
        if (!exploded && health <= 0)
        {
            Destroy(GetComponent<CompositeCollider2D>());

            for (int i = 0; i < numChildren; i++)
            {
                GameObject child = gameObject.transform.GetChild(i).gameObject;
                child.layer = ghostLayer;
                child.GetComponent<Collider2D>().usedByComposite = false;

                Rigidbody2D childRb = child.AddComponent<Rigidbody2D>();
                childRb.velocity = child.transform.localPosition.normalized * explodeSpeed;
                Timer.Register(beforeFadeTime, () => child.GetComponent<SpriteFader>().FadeOut(fadeTime));
            }

            exploded = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;
        GameObject obj = collision.gameObject;
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

        if (obj.CompareTag("Center in Mouth") && rb.velocity.magnitude >= speedThreshold)
        {
            health--;
        }
    }
}
