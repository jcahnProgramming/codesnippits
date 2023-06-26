using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public BoxCollider collider;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (collider.tag == "GrappleBox")
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
