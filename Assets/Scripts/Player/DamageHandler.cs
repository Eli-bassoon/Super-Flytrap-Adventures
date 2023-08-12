using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageHandler : MonoBehaviour
{
    public static DamageHandler instance;
    public int deaths = 0;

    [SerializeField] float fullHealth = 100f;
    [SerializeField] float currHealth; // = fullHealth;
    [SerializeField] float respawnTime = 1.5f;
    [SerializeField] GameObject checkpointPrefab; // drag prefab here 
    [SerializeField] Transform checkpointLatest;
    [SerializeField] Image chompyIndicator;
    [SerializeField] AudioClip deathSound;
    [SerializeField] GameObject deathParticles;
    Rigidbody2D rb;
    Rigidbody2D flowerpot;
    Rigidbody2D tongue;
    List<Rigidbody2D> rbList;

    bool damageable = true;
    [HideInInspector] public bool respawning = false;

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
        if (!respawning)
        {
            StartCoroutine(RespawnCR());
        }
    }

    IEnumerator RespawnCR()
    {
        print("Respawning");
        deaths++;
        respawning = true;

        // Make invisible and static
        ChangeVisibility(false);
        ChangeStatic(true);

        // Spawn death particles
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Instantiate(deathParticles, flowerpot.position, Quaternion.identity);

        // Camera effect and sounds
        Camera.main.GetComponent<CameraShaker>().Shake();
        SoundManager.SM.PlaySound(deathSound);

        yield return new WaitForSeconds(respawnTime);

        // Reset position and velocity
        foreach (Rigidbody2D body in rbList)
        {
            body.position = checkpointLatest.position;
            body.velocity = Vector2.zero;
            body.angularVelocity = 0;
            body.rotation = 0;
        }

        tongue.position = rb.position;

        // Reset more things
        GetComponent<PlayerMovement>().LetGo();
        currHealth = fullHealth;
        checkpointLatest.GetComponent<TriggerCheckpoint>().onRespawn.Invoke();
        ChangeStatic(false);

        // Wait to stop graphical glitches
        yield return new WaitForFixedUpdate();
        ChangeVisibility(true);
        respawning = false;
    }

    void ChangeVisibility(bool visible)
    {
        PlayerMovement.instance.flowerpot.GetComponent<SpriteRenderer>().enabled = visible;
        PlayerMovement.instance.rightMouthHalf.GetComponent<SpriteRenderer>().enabled = visible;
        PlayerMovement.instance.leftMouthHalf.GetComponent<SpriteRenderer>().enabled = visible;
        PlayerMovement.instance.tongue.GetComponent<SpriteRenderer>().enabled = visible;

        PlayerMovement.instance.tongue.GetComponent<LineRenderer>().enabled = visible;
        PlayerMovement.instance.neck.GetComponent<LineRenderer>().enabled = visible;
    }

    void ChangeStatic(bool isStatic)
    {
        if (isStatic)
        {
            rb.velocity = Vector2.zero;
            flowerpot.velocity = Vector2.zero;

            rb.angularVelocity = 0;
            flowerpot.angularVelocity = 0;

            rb.bodyType = RigidbodyType2D.Static;
            flowerpot.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            flowerpot.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}