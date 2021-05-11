using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Body : MonoBehaviour
{


    public Vector3Decimal _global_position;
    public Vector3Decimal _global_velocity;

    public Rigidbody physics_properties;

    private void Awake()
    {
        _global_position = new Vector3Decimal();
        _global_velocity = new Vector3Decimal();
        physics_properties = GetComponent<Rigidbody>();
        //FindObjectOfType<Continuum>()._bodies.Add(this.gameObject, (decimal)transform.position.x, (decimal)transform.position.y, (decimal)transform.position.z);
    }

    

    


}
