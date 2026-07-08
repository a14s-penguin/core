using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{

    public float speed;

    Rigidbody rb;

    float xinput;
    float yinput;

    int score = 0;
    public int winScore;
    public GameObject Wintext;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


   

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -5f)
        {
            SceneManager.LoadScene("Games");
        }
        
    }

    private void FixedUpdate()
    {
        xinput = Input.GetAxis("Horizontal");
        yinput = Input.GetAxis("Vertical");

        rb.AddForce(xinput * speed,0,yinput * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "coin")
        {
            other.gameObject.SetActive(false);
            score++;
            if(score >= winScore)
            {
                Wintext.SetActive(true);
            }
        }
    }
    
}
