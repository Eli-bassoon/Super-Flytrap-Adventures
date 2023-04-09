using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandling : MonoBehaviour
{
    [SerializeField] int fullHealth = 100;
    [SerializeField] int currHealth; // = fullHealth;
    [SerializeField] GameObject checkpointPrefab; // drag prefab here 
    [SerializeField] Transform checkpointLatest;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = fullHealth;
        // spawn prefab & save its transform
        checkpointLatest = Instantiate(checkpointPrefab, transform.position, Quaternion.identity).transform;
    }

    // Update is called once per frame
    void Update()
    {   
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
        GetComponent<Rigidbody2D>().position = checkpointLatest.position;
    }
}
