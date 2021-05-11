using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OctreeDecimal
{
    public class ObjOctree
    {

        private ObjNode _root;

        public ObjOctree(decimal x_min, decimal x_max, decimal y_min, decimal y_max, decimal z_min, decimal z_max)
        {
            this._root = new ObjNode(x_min, x_max, y_min, y_max, z_min, z_max, 25);
        }

        public ObjOctree(decimal x_min, decimal x_max, decimal y_min, decimal y_max, decimal z_min, decimal z_max, int capacity)
        {
            this._root = new ObjNode(x_min, x_max, y_min, y_max, z_min, z_max, capacity);
        }

        public bool Add(GameObject obj, decimal x, decimal y, decimal z)
        {
            return this._root.AddLeaf(obj, x, y, z);
        }
        public bool Add(GameObject obj, Vector3Decimal g_pos)
        {
            return this._root.AddLeaf(obj, g_pos.x, g_pos.y, g_pos.z);
        }

        public void MoveLeaf(ObjLeaf leaf, Vector3Decimal diff)
        {
            _root.MoveLeaf(leaf, diff);
        }

        public int GetDepth()
        {
            return this._root.GetDepth();
        }

        public int GetNumberOfOuterNodes()
        {
            return this._root.GetNumberOfOuterNodes();
        }

        public List<ObjLeaf> GetLeavesInVolume(decimal x_min, decimal x_max, decimal y_min, decimal y_max, decimal z_min, decimal z_max)
        {
            List<ObjLeaf> ret = new List<ObjLeaf>();
            ret = _root.GetLeavesInVolume(x_min, x_max, y_min, y_max, z_min, z_max);
            return ret;

        }

        public List<ObjLeaf> GetLeavesInVolume(Bounds bounds)
        {
            List<ObjLeaf> ret = new List<ObjLeaf>();
            ret = _root.GetLeavesInVolume(bounds);
            return ret;

        }

        public List<ObjLeaf> GetRepresentation()
        {
            
            return this._root.GetRepresentation();
        }

        public bool RemoveLeaf(ObjLeaf leaf)
        {
            return _root.RemoveLeaf(leaf);

        }

        public override string ToString()
        {
            string ret = "Root:\n";
            ret += this._root.ToString();
            return ret;
        }

    }



}