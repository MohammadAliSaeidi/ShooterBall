using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ForceAtStart : MonoBehaviour
{
    public Vector3 Force;
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.right * Force.x);
        GetComponent<Rigidbody>().AddForce(transform.up * Force.y);
        GetComponent<Rigidbody>().AddForce(transform.forward * Force.z);
    }
}
