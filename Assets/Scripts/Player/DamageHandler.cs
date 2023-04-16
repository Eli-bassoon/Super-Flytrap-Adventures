using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    [SerializeField] int fullHealth = 100;
    [SerializeField] int currHealth; // = fullHealth;
    [SerializeField] GameObject checkpointPrefab; // drag prefab here 
    [SerializeField] Transform checkpointLatest;
    Rigidbody2D rb;
    Rigidbody2D flowerpot;
    Rigidbody2D tongue;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = fullHealth;
        // spawn prefab & save its transform
        checkpointLatest = Instantiate(checkpointPrefab, transform.position, Quaternion.identity).transform;
        rb = GetComponent<Rigidbody2D>();
        flowerpot = GetComponent<PlayerMovement>().flowerpot;
        tongue = GetComponent<PlayerMovement>().tongue;
    }

    // Update is called once per frame
    void Update()
    {   
    }

    public void SaveCheckpoint(Transform transform)
    {
        print("this worked");
        if (transform != null)
        {
            checkpointLatest = transform;
            print("checkpoint saved");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        print("ooga booga");
        if (collision != null && collision.gameObject.CompareTag("Respawn"))
        {
            GameObject obj = collision.gameObject;
            SaveCheckpoint(obj.transform);
            obj.SetActive(false);
        }
    }

    public void TakeDamage(int dmg)
    {
        currHealth -= dmg;
        if (currHealth < 0)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        rb.position = checkpointLatest.position;
        flowerpot.position = checkpointLatest.position; // problem: we don't always land on our feet
        tongue.position = checkpointLatest.position;
        rb.velocity = Vector2.zero;
        flowerpot.velocity = Vector2.zero;
    }
}
