using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fan_sara : MonoBehaviour
{
    private Animator _fanAnim;
    // Start is called before the first frame update
    void Start()
    {
        _fanAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _fanAnim.Play("Fan_on");
        }
    }
}
