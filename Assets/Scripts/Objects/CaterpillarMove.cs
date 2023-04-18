using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaterpillarMove : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float moveTime = 3f;
    [SerializeField] List<GameObject> food;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        float startTime = Time.time;

        while ((Time.time - startTime) < moveTime)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (food.Contains(collision.gameObject))
        {
            Destroy(collision.gameObject);
        }
    }
}
