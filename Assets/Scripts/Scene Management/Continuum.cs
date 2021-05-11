using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OctreeDecimal;

public class Continuum : MonoBehaviour
{
    public float size_coeffiecient;
    public int size_e;

    public ObjOctree _bodies;
    Observer _observer;

    public float T = 0;
    // Start is called before the first frame update
    void Start()
    {
        _observer = FindObjectOfType<Observer>();
        decimal range = DecimalMath.DecimalEx.PowersOf10[size_e] * (decimal)size_coeffiecient;
        _bodies = new ObjOctree(-range, range, -range, range, -range, range);
        Body[] start_bods = FindObjectsOfType<Body>();
        foreach (Body bod in start_bods)
        {
            bod._global_position = _observer.GetObserverBody()._global_position + bod.transform.position;
            _bodies.Add(bod.gameObject, bod._global_position);
        }

    }

    // Update is called once per frame
    void Update()
    {
        T += Time.deltaTime;
        Iterate(T);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(_bodies.ToString());
        }
    }

    private void FixedUpdate()
    {

    }

    public void Iterate(float T)
    {
        
        List<ObjLeaf> leaves = new List<ObjLeaf>();
        leaves = _bodies.GetRepresentation();
        //Debug.Log("Iterate.leaves has " + leaves.Count + " items.");
        foreach (ObjLeaf leaf in leaves)
        {
            
            Body bod = leaf.GetObject().GetComponent<Body>();
            Vector3Decimal diff = bod._global_velocity * (decimal)T;
            Vector3Decimal new_pos = bod._global_position + diff;

            //ObjLeaf new_leaf = new ObjLeaf(bod.gameObject, new_pos);
            _bodies.MoveLeaf(leaf, new_pos);
            //Debug.Log("Moving: " + leaf.GetObject().name);

        }
    }

    // Iterate orbital motion in octree


}
