using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerascript : MonoBehaviour
{

    public Transform target;

  
    private void FixedUpdate()
    {
        transform.LookAt(target);
    }
}
