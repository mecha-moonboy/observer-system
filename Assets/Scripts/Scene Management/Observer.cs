using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OctreeDecimal;

[RequireComponent(typeof(Body))]
public class Observer : MonoBehaviour
{



    Continuum _continuum;
    Body _obs_body;
    //Observer _observer;

    public float loading_range;
    decimal margin = (decimal)10;
    public float position_threshold;
    public float velocity_threshold;

    public List<Body> _loaded_bodies;
    //public List<GameObject> _gameObjects;
    // Start is called before the first frame update
    void Start()
    {
        _obs_body = GetComponent<Body>();
        _continuum = FindObjectOfType<Continuum>();
        
        _loaded_bodies = new List<Body>();
        _loaded_bodies.AddRange(FindObjectsOfType<Body>());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(_continuum._bodies.ToString());
        }
        foreach(Body bod in _loaded_bodies)
        {
            bod._global_position = _obs_body._global_position + bod.transform.position ;
            bod._global_velocity = _obs_body._global_velocity + bod.physics_properties.velocity;
            //Debug.Log(bod.name + ": " + bod._global_position.ToVector3f());

        }
    }

    private void FixedUpdate()
    {
        foreach(Body bod in _loaded_bodies)
        {
            //RefreshLoaded(bod);
        }
        LoadUnload();
        // Floating origin and velocity frame of reference
        CenterOriginAndVelocity();

        //LoadObjects();

        //_continuum.Iterate(Time.deltaTime);

        //if(_loaded_bodies.Count > 0) 
            //UnloadObjects();

        
    }

    void CenterOriginAndVelocity()
    {
        Vector3 vel = _obs_body.physics_properties.velocity; // Vel of observer object
        if (vel.magnitude > velocity_threshold) // is above threshold?
        {
            _obs_body._global_velocity += vel; // Then store Vel globally
            foreach (Body bod in _loaded_bodies)
            {
                bod.physics_properties.velocity -= vel; // subtract Vel from all scene bods
            }
        }


        Vector3 pos = _obs_body. transform.position; // Pos of observer object
        if(pos.magnitude > position_threshold) // is above threshold?
        {
            _obs_body._global_position += pos; // Store Pos globally 
            foreach(Body bod in _loaded_bodies)
            {
                bod.transform.position -= pos; // subtract pos from scene bods
            }
        }
    }

    // Load object
    /*bool LoadObjects()
    {
        bool did_load = false;
        List<ObjLeaf> leaves;

        OctreeDecimal.Bounds zone = new OctreeDecimal.Bounds(
            _obs_body._global_position.x - (decimal)loading_range, _obs_body._global_position.x + (decimal)loading_range,
            _obs_body._global_position.y - (decimal)loading_range, _obs_body._global_position.y + (decimal)loading_range,
            _obs_body._global_position.z - (decimal)loading_range, _obs_body._global_position.z + (decimal)loading_range); // Bounds representation of loading zone

        leaves = _continuum._bodies.GetLeavesInVolume(zone);

        Debug.Log(leaves.Count);
        //Debug.Log(leaves.Count);
        //int loaded = 0;
        foreach (ObjLeaf leaf in leaves)
        {
            if (leaf != null)
            {   
                if (!leaf.GetObject().activeSelf)
                {
                    //loaded++;
                    Body bod = leaf.GetObject().GetComponent<Body>();
                    LoadBody(bod);
                    did_load = true;
                }
            }
            else
            {
                Debug.LogWarning("Invalid Leaf: null");
            }
        }

        //Debug.Log(did_load);
        
        return did_load;

    }*/

    
    void LoadUnload()
    {
        OctreeDecimal.Bounds margin_zone = new OctreeDecimal.Bounds(
            _obs_body._global_position.x - (decimal)loading_range - margin, _obs_body._global_position.x + (decimal)loading_range + margin,
            _obs_body._global_position.y - (decimal)loading_range - margin, _obs_body._global_position.y + (decimal)loading_range + margin,
            _obs_body._global_position.z - (decimal)loading_range - margin, _obs_body._global_position.z + (decimal)loading_range + margin); // Bounds representation of loading zone

        OctreeDecimal.Bounds zone = new OctreeDecimal.Bounds(
            _obs_body._global_position.x - (decimal)loading_range, _obs_body._global_position.x + (decimal)loading_range,
            _obs_body._global_position.y - (decimal)loading_range, _obs_body._global_position.y + (decimal)loading_range,
            _obs_body._global_position.z - (decimal)loading_range, _obs_body._global_position.z + (decimal)loading_range); //  Bounds representation of loading zone



        List<ObjLeaf> leaves = _continuum._bodies.GetLeavesInVolume(margin_zone);
        //Debug.Log("Load/Unload: leaves contains " + leaves.Count + " members.");

        List<Body> bods = new List<Body>();
        foreach(ObjLeaf leaf in leaves)
        {
            bods.Add(leaf.GetObject().GetComponent<Body>());
        }
        foreach(Body bod in bods)
        {
            if (!zone.PointIsInBounds(bod._global_position) && bod.gameObject.activeSelf)
            {
                Debug.Log(bod.name + " is outside of loading range. Unloading...");
                UnloadBody(bod);
            }
            else if(zone.PointIsInBounds(bod._global_position) && !bod.gameObject.activeSelf)
            {
                Debug.Log(bod.name + " is inside loading range. Loading...");
                LoadBody(bod);            
            }
        }


    }

    void LoadBody(Body bod) // Sturdy
    {
        bod.transform.position = (bod._global_position - _obs_body._global_position).ToVector3f();
        bod.gameObject.SetActive(true);
        bod.physics_properties.velocity = (bod._global_velocity - _obs_body._global_velocity).ToVector3f();
        _loaded_bodies.Add(bod);
        Debug.Log("Loaded: " + bod.gameObject.name);
    }

    void UnloadBody(Body bod) // Sturdy
    {
        Vector3Decimal pos = _obs_body._global_position + bod.transform.position;
        bod._global_position = pos;
        bod._global_velocity = _obs_body._global_velocity + bod.physics_properties.velocity; 
        _loaded_bodies.Remove(bod);
        bod.gameObject.SetActive(false);
        //Debug.Log("Unloaded: " + bod.gameObject.name + ".");
    } 

    void RefreshLoaded(Body bod)
    {
        bod._global_position = _obs_body._global_position + bod.transform.position;
        bod._global_velocity = _obs_body._global_velocity + bod.physics_properties.velocity;
    }

    public Body GetObserverBody()
    {
        return _obs_body;
    }

}
