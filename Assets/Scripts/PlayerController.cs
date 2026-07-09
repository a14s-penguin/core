using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{

    public static event Action<int> ScoreChanged;
    public static event Action WinReached;

    public float speed;

    Rigidbody rb;

    float xinput;
    float yinput;

    public int score = 0;
    public int winScore;
    public GameObject Wintext;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (Wintext != null)
        {
            Wintext.SetActive(false);
        }
        ScoreChanged?.Invoke(score);
    }


   

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5f)
        {
            RestartGame();
        }
    }

    private void FixedUpdate()
    {
        xinput = Input.GetAxis("Horizontal");
        yinput = Input.GetAxis("Vertical");

        rb.AddForce(xinput * speed, 0, yinput * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("coin"))
        {
            other.gameObject.SetActive(false);
            score++;
            ScoreChanged?.Invoke(score);
            if (score >= winScore)
            {
                Wintext?.SetActive(true);
                WinReached?.Invoke();
            }
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
