using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(camera.transform.position);
        gameObject.transform.forward = -transform.forward;
    }
}
