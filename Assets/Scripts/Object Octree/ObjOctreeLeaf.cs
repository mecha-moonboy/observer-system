using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OctreeDecimal
{
    public class ObjLeaf
    {
        private GameObject _obj;
        private decimal _x, _y, _z;

        public ObjLeaf(GameObject obj, decimal x, decimal y, decimal z) // Sturdy
        {
            _obj = obj;
            _x = x;
            _y = y;
            _z = z;
           //Debug.Log("Added: " + obj.name);

        }
        public ObjLeaf(GameObject obj, Vector3Decimal v3d) // Sturdy
        {
            _obj = obj;
            _x = v3d.x;
            _y = v3d.y;
            _z = v3d.z;
            //Debug.Log("Added: " + obj.name);
        }


        public override string ToString()
        {
            return "[ " + _x.ToString() + " , " + _y.ToString() + " , " + _z.ToString() + " ]";
        }


        public decimal GetX()
        {
            return _x;
        }
        public decimal GetY()
        {
            return _y;
        }
        public decimal GetZ()
        {
            return _z;
        }

        public GameObject GetObject()
        {
            return _obj;
        }

        /*public void MoveLeaf(Vector3Decimal diff)
        {

        }*/

    }

}
