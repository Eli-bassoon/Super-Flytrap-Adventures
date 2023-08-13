using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChompyPlush : MonoBehaviour
{
    [SerializeField] AudioClip thunkSound;
    [Range(0, 2f)][SerializeField] float thunkDelay = 0.5f;
    bool canThunk = true;

    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement.instance.CheckJoltAwakeCollision(collision);

        if (collision.relativeVelocity.magnitude > 2f && canThunk)
        {
            SoundManager.SM.PlaySound(thunkSound);
            StartCoroutine(ThunkDelayCR());
        }
    }

    IEnumerator ThunkDelayCR()
    {
        canThunk = false;
        yield return new WaitForSeconds(thunkDelay);
        canThunk = true;
    }
}
