using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageHandler : MonoBehaviour
{
    [SerializeField] float fullHealth = 100f;
    [SerializeField] float currHealth; // = fullHealth;
    [SerializeField] GameObject checkpointPrefab; // drag prefab here 
    [SerializeField] Transform checkpointLatest;
    [SerializeField] Image chompyIndicator;
    Rigidbody2D rb;
    Rigidbody2D flowerpot;
    Rigidbody2D tongue;

    bool damageable = true;

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
        chompyIndicator.fillAmount = currHealth / fullHealth;
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
        }
    }

    public void TakeDamage(int dmg)
    {
        if (!damageable) return;
        damageable = false;
        Timer.Register(1, () => { damageable = true; });
        currHealth -= dmg;
        if (currHealth <= 0)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        GetComponent<PlayerMovement>().LetGo();
        rb.position = checkpointLatest.position;
        flowerpot.position = checkpointLatest.position; // problem: we don't always land on our feet
        tongue.position = checkpointLatest.position;
        rb.velocity = Vector2.zero;
        flowerpot.velocity = Vector2.zero;
    }
}
