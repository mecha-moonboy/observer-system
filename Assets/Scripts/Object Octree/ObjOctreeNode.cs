using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

namespace OctreeDecimal
{
    public class ObjNode
    {
        private Bounds _bounds;
        private ObjNode[] _children;


        private List<ObjLeaf> _leaves;
        private int _capacity;
        private bool _inner_node;

        public ObjNode(decimal x_min, decimal x_max, decimal y_min, decimal y_max, decimal z_min, decimal z_max, int capacity)
        {
            this._bounds = new Bounds(x_min, x_max, y_min, y_max, z_min, z_max);

            this._leaves = new List<ObjLeaf>();

            this._inner_node = false;

            _capacity = capacity;
        }

        private bool Divide()
        {
            decimal _size_x = (this._bounds.GetXMax() - this._bounds.GetXMin()) / 2;
            decimal _size_y = (this._bounds.GetYMax() - this._bounds.GetYMin()) / 2;
            decimal _size_z = (this._bounds.GetZMax() - this._bounds.GetZMin()) / 2;

            this._children = new ObjNode[8];

            this._children[0] = new ObjNode(
                this._bounds.GetXMin(), this._bounds.GetXMax() - _size_x,
                this._bounds.GetYMin(), this._bounds.GetYMax() - _size_y,
                this._bounds.GetZMin(), this._bounds.GetZMax() - _size_z,
                this._capacity);
            this._children[1] = new ObjNode(
                this._bounds.GetXMin() + _size_x, this._bounds.GetXMax(),
                this._bounds.GetYMin(), this._bounds.GetYMax() - _size_y,
                this._bounds.GetZMin(), this._bounds.GetZMax() - _size_z,
                this._capacity);
            this._children[2] = new ObjNode(
                this._bounds.GetXMin(), this._bounds.GetXMax() - _size_x,
                this._bounds.GetYMin() + _size_y, this._bounds.GetYMax(),
                this._bounds.GetZMin(), this._bounds.GetZMax() - _size_z,
                this._capacity);
            this._children[3] = new ObjNode(
                this._bounds.GetXMin() + _size_x, this._bounds.GetXMax(),
                this._bounds.GetYMin() + _size_y, this._bounds.GetYMax(),
                this._bounds.GetZMin(), this._bounds.GetZMax() - _size_z,
                this._capacity);
            this._children[4] = new ObjNode(
                this._bounds.GetXMin(), this._bounds.GetXMax() - _size_x,
                this._bounds.GetYMin() + _size_y, this._bounds.GetYMax(),
                this._bounds.GetZMin() + _size_z, this._bounds.GetZMax(),
                this._capacity);
            this._children[5] = new ObjNode(
                this._bounds.GetXMin() + _size_x, this._bounds.GetXMax(),
                this._bounds.GetYMin() + _size_y, this._bounds.GetYMax(),
                this._bounds.GetZMin() + _size_z, this._bounds.GetZMax(),
                this._capacity);
            this._children[6] = new ObjNode(
                this._bounds.GetXMin() + _size_x, this._bounds.GetXMax(),
                this._bounds.GetYMin(), this._bounds.GetYMax() - _size_y,
                this._bounds.GetZMin() + _size_z, this._bounds.GetZMax(),
                this._capacity);
            this._children[7] = new ObjNode(
                this._bounds.GetXMin(), this._bounds.GetXMax() - _size_x,
                this._bounds.GetYMin(), this._bounds.GetYMax() - _size_y,
                this._bounds.GetZMin() + _size_z, this._bounds.GetZMax(),
                this._capacity);


            for (int i = 0; i < _leaves.Count; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (this._children[j].AddLeaf(this._leaves[i]))
                    {
                        break;
                    }
                }
            }
            this._inner_node = true;
            this._leaves = null;

            return true;

        }
        void Collapse()
        {
            List<ObjLeaf> rep = GetRepresentation();
            if (_inner_node)
            {
                if(rep.Count == 0)
                {
                    _inner_node = false;
                    _children = null;
                    _leaves = new List<ObjLeaf>(_capacity);
                }else if (rep.Count == 1)
                {
                    _inner_node = false;
                    _children = null;
                    _leaves.Add(rep[0]);
                }
                else
                {
                    foreach(ObjNode child in _children)
                    {
                        Collapse();
                    }
                }
                
            }
        }

        public bool AddLeaf(GameObject obj, decimal x, decimal y, decimal z)
        {
            if (!this._bounds.PointIsInBounds(x, y, z))
            {
                Debug.Log("Position out of bounds!");
                return false;
            }
            if (this._inner_node)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (this._children[i].AddLeaf(obj, x, y, z))
                    {
                        Debug.Log(this._children[i].ToString());
                        break;
                    }
                }
            }
            else
            {
                if (this._leaves.Count + 1 > this._capacity)
                {
                    this.Divide();
                }
                //Debug.Log("Leaf added!");
                this._leaves.Add(new ObjLeaf(obj, x, y, z));
            }
            return true;
        }
        public bool AddLeaf(GameObject obj, Vector3Decimal pos)
        {
            if (!this._bounds.PointIsInBounds(pos))
            {
                Debug.Log("Position out of bounds!");
                return false;
            }
            if (this._inner_node)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (this._children[i].AddLeaf(obj, pos))
                    {
                        //Debug.Log(this._children[i].ToString());
                        break;
                    }
                }
            }
            else
            {
                if (this._leaves.Count + 1 > this._capacity)
                {
                    this.Divide();
                }
                //Debug.Log("Leaf added!");
                this._leaves.Add(new ObjLeaf(obj, pos));
            }
            return true;
        }


        public bool AddLeaf(ObjLeaf leaf)
        {
            decimal x = leaf.GetX();
            decimal y = leaf.GetY();
            decimal z = leaf.GetZ();

            if (!this._bounds.PointIsInBounds(x, y, z))
            {
                Debug.Log("Position out of bounds!");
                return false;
            }

            if (this._leaves.Count + 1 > this._capacity)
            {
                this.Divide();
            }

            if (this._inner_node)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (this._children[i].AddLeaf(leaf))
                    {
                        
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("Leaf added!");
                this._leaves.Add(leaf);
            }
            return true;
        }

        public int GetDepth()
        {
            if (this._inner_node)
            {
                int max = 0;

                for (int i = 0; i < 8; i++)
                {
                    int depth = this._children[i].GetDepth() + 1;
                    if (depth > max)
                    {
                        max = depth;
                    }
                }
                return max;
            }
            else
            {
                return 1;
            }
        }

        public int GetNumberOfOuterNodes()
        {
            if (this._inner_node)
            {
                int sum = 0;

                for (int i = 0; i < 8; i++)
                {
                    sum += this._children[i].GetNumberOfOuterNodes();
                }
                return sum;
            }
            else
            {
                return 1;
            }
        }

        public List<ObjLeaf> GetRepresentation()
        {
            if (this._inner_node)
            {
                List<ObjLeaf> ret = new List<ObjLeaf>();

                foreach(ObjNode child in _children)
                {
                    ret.AddRange(child.GetRepresentation());
                }
                return ret;
            }
            else
            {
                 
                List<ObjLeaf> ret = new List<ObjLeaf>();
                foreach(ObjLeaf leaf in _leaves)
                {
                    ret.Add(leaf);
                }
                return ret;


            }
           
        }

        public void DebugRenderNode(Vector3Decimal pos)
        {

            if (_children != null)
            {
                foreach (ObjNode child in _children)
                {
                    child.DebugRenderNode(pos);
                }
            }
                Debug.DrawLine(new Vector3((float)_bounds.GetXMax(), (float)_bounds.GetYMax(), (float)_bounds.GetZMax()) + pos.ToVector3f(), new Vector3((float)_bounds.GetXMin(), (float)_bounds.GetYMin(), (float)_bounds.GetZMax()) + pos.ToVector3f(), Color.red, 10f);
                Debug.DrawLine(new Vector3((float)_bounds.GetXMax(), (float)_bounds.GetYMax(), (float)_bounds.GetZMax()) + pos.ToVector3f(), new Vector3((float)_bounds.GetXMax(), (float)_bounds.GetYMax(), (float)_bounds.GetZMax()) + pos.ToVector3f(), Color.red, 10f);
                Debug.DrawLine(new Vector3((float)_bounds.GetXMax(), (float)_bounds.GetYMax(), (float)_bounds.GetZMax()) + pos.ToVector3f(), new Vector3((float)_bounds.GetXMax(), (float)_bounds.GetYMax(), (float)_bounds.GetZMin()), Color.red, 10f);

                Debug.DrawLine(new Vector3((float)_bounds.GetXMin(), (float)_bounds.GetYMin(), (float)_bounds.GetZMin()) + pos.ToVector3f(), new Vector3((float)_bounds.GetXMax(), (float)_bounds.GetYMin(), (float)_bounds.GetZMin()) + pos.ToVector3f(), Color.red, 10f);
                Debug.DrawLine(new Vector3((float)_bounds.GetXMin(), (float)_bounds.GetYMin(), (float)_bounds.GetZMin()) + pos.ToVector3f(), new Vector3((float)_bounds.GetXMin(), (float)_bounds.GetYMax(), (float)_bounds.GetZMin()) + pos.ToVector3f(), Color.red, 10f);
                Debug.DrawLine(new Vector3((float)_bounds.GetXMin(), (float)_bounds.GetYMin(), (float)_bounds.GetZMin()) + pos.ToVector3f(), new Vector3((float)_bounds.GetXMin(), (float)_bounds.GetYMin(), (float)_bounds.GetZMax()) + pos.ToVector3f(), Color.red, 10f);


            
        }

        public List<ObjLeaf> GetLeavesInVolume(decimal x_min, decimal x_max, decimal y_min, decimal y_max, decimal z_min, decimal z_max)
        {
            Bounds vol = new Bounds(x_min, x_max, y_min, y_max, z_min, z_max);

            List<ObjLeaf> ret = new List<ObjLeaf>();

            if (!_inner_node)
            {
                foreach (ObjLeaf leaf in _leaves)
                {

                    if (vol.PointIsInBounds(leaf.GetX(), leaf.GetY(), leaf.GetZ()))
                    {
                        Debug.Log(vol.ToString() + " contains " + new Vector3Decimal(leaf.GetX(), leaf.GetY(), leaf.GetZ()).ToString() + "   ...   apperently");
                        //Debug.Log("Added leaf to ret.");
                        ret.Add(leaf);
                    }
                }

            }
            else
            {
                foreach (ObjNode child in _children)
                {
                    if (child._bounds.CheckIsInsideVolume(vol) > 0)
                    {
                        ret.AddRange(child.GetLeavesInVolume(vol));
                    }
                    else if (child._bounds.CheckIsInsideVolume(vol) > 5)
                    {
                        //Debug.Log("All six sides inside the bounds, adding all representations.");
                        ret.AddRange(GetRepresentation());
                    }
                }
            }


            //Debug.Log("Done, returning.");
            return ret;
        }
        public List<ObjLeaf> GetLeavesInVolume(Bounds vol) 
        {
            List<ObjLeaf> ret = new List<ObjLeaf>();

            if (!_inner_node)
            {
                foreach (ObjLeaf leaf in _leaves)
                {

                    if (vol.PointIsInBounds(leaf.GetX(), leaf.GetY(), leaf.GetZ()))
                    {
                        //Debug.Log(vol.ToString() + " contains " + new Vector3Decimal(leaf.GetX(), leaf.GetY(), leaf.GetZ()).ToString() + "   ...   apperently");
                        //Debug.Log("Added leaf to ret.");
                        ret.Add(leaf);
                    }
                }

            }
            else
            {
                foreach(ObjNode child in _children)
                {
                    if(child._bounds.CheckIsInsideVolume(vol) > 0)
                    {
                        ret.AddRange(child.GetLeavesInVolume(vol));
                    }
                    else if (child._bounds.CheckIsInsideVolume(vol) > 5)
                    {
                        //Debug.Log("All six sides inside the bounds, adding all representations.");
                        ret.AddRange(GetRepresentation());
                    }
                }
            }
            

            //Debug.Log("Done, returning.");
            return ret;
        }

        public void MoveLeaf(ObjLeaf leaf, Vector3Decimal pos) // Sturdy
        {
            if(RemoveLeaf(leaf))
            {
                AddLeaf(leaf.GetObject(), pos);
                //Debug.Log(leaf.GetObject().name + " moved to " + pos.ToString());
                return;
            }
            Debug.Log(leaf.GetObject().name + " did not exist in the octree.");
        }

            
        
        public bool RemoveLeaf(ObjLeaf leaf) // Sturdy
        {
            Vector3Decimal leaf_pos = new Vector3Decimal(leaf.GetX(), leaf.GetY(), leaf.GetZ());
            if(_bounds.PointIsInBounds(leaf_pos))
            {
                if (_inner_node)
                {
                    foreach (ObjNode child in _children)
                    {
                        child.RemoveLeaf(leaf);
                    }
                }
                else
                {
                    if (!_leaves.Contains(leaf)) { 
                        Debug.Log(leaf.GetObject().name + " not found in octree");
                        return false;
                    }

                    _leaves.Remove(leaf);

                    Collapse();
                    return true;
                }
            }
            return false;
        }


        private ObjLeaf GetCentroid()
        {
            decimal min = decimal.MaxValue;
            ObjLeaf representation = null;

            for (int i = 0; i < this._leaves.Count; i++)
            {
                decimal dist = this._bounds.GetDistToCenter(this._leaves[i]);
                if (dist < min)
                {
                    min = dist;
                    representation = this._leaves[i];
                }
            }
            return representation;
        }

        private ObjLeaf GetMedoid()
        {
            decimal x = 0, y = 0, z = 0;

            for (int i = 0; i < this._leaves.Count; i++)
            {
                x += this._leaves[i].GetX();
                y += this._leaves[i].GetY();
                z += this._leaves[i].GetZ();
            }
            if (this._leaves.Count > 0)
            {
                x = x / this._leaves.Count;
                y = y / this._leaves.Count;
                z = z / this._leaves.Count;
            }

            decimal min = decimal.MaxValue;
            ObjLeaf representation = null;

            for (int i = 0; i < this._leaves.Count; i++)
            {
                decimal sum = 0;

                sum += DecimalMath.DecimalEx.Pow(this._leaves[i].GetX() - x, 2);
                sum += DecimalMath.DecimalEx.Pow(this._leaves[i].GetY() - y, 2);
                sum += DecimalMath.DecimalEx.Pow(this._leaves[i].GetZ() - z, 2);

                decimal dist = DecimalMath.DecimalEx.Sqrt(sum);

                if (dist < min)
                {
                    min = dist;
                    representation = this._leaves[i];
                }
            }
            return representation;
        }

        private List<GameObject> GetObjects()
        {
            List<GameObject> ret = new List<GameObject>();

            List<ObjLeaf> leaves = this.GetRepresentation();

            foreach (ObjLeaf leaf in leaves)
            {
                ret.Add(leaf.GetObject());
            }

            return ret;

        }

        public override string ToString()
        {
            string ret = "";
            if (this._inner_node)
            {
                ret = "Children:\n";
                for (int i = 0; i < 8; i++)
                {
                    ret += this._children[i].ToString() + "\n\n\n";
                }
            }
            else
            {
                for (int i = 0; i < this._leaves.Count; i++)
                {
                    ret += "\t" + this._leaves[i].ToString() + "\n";
                }
                ret += "\n";
                if (this.GetRepresentation() != null && this.GetRepresentation()[0] != null)
                {
                    ret += "Rep: " + this.GetRepresentation()[0].ToString() + "\n";
                }
            }

            return ret;
        }

    }
}
