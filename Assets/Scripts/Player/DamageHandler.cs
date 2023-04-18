using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageHandler : MonoBehaviour
{
    public static DamageHandler instance;

    [SerializeField] float fullHealth = 100f;
    [SerializeField] float currHealth; // = fullHealth;
    [SerializeField] GameObject checkpointPrefab; // drag prefab here 
    [SerializeField] Transform checkpointLatest;
    [SerializeField] Image chompyIndicator;
    Rigidbody2D rb;
    Rigidbody2D flowerpot;
    Rigidbody2D tongue;
    List<Rigidbody2D> rbList;

    bool damageable = true;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currHealth = fullHealth;
        // spawn prefab & save its transform
        checkpointLatest = Instantiate(checkpointPrefab, transform.position, Quaternion.identity).transform;
        rb = GetComponent<Rigidbody2D>();
        flowerpot = GetComponent<PlayerMovement>().flowerpot;
        tongue = GetComponent<PlayerMovement>().tongue;

        rbList = new List<Rigidbody2D>() { rb, flowerpot, tongue };
    }

    // Update is called once per frame
    void Update()
    {
        if (chompyIndicator != null) chompyIndicator.fillAmount = currHealth / fullHealth;
    }

    public void SaveCheckpoint(Transform transform)
    {
        checkpointLatest = transform;
        print("Checkpoint saved");
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
        foreach (Rigidbody2D tmpBody in rbList)
        {
            tmpBody.position = checkpointLatest.position;
            tmpBody.velocity = Vector2.zero;
            tmpBody.angularVelocity = 0;
        }

        GetComponent<PlayerMovement>().LetGo();
        currHealth = fullHealth;
    }
}
