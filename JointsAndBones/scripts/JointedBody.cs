using System.Collections.Generic;
using UnityEngine;

namespace JointAndBoneDrawer
{
    public class JointedBody
    {
        /// <summary>
        /// The joint type
        /// </summary>
        public PrimitiveType JointType = PrimitiveType.Sphere;

        /// <summary>
        /// The joint size
        /// </summary>
        public Vector3 JointSize = new Vector3(.1f, .1f, .1f);

        /// <summary>
        /// The collection of joints. Use this collection to set properties on the joints (color, shape, ect.) but 
        /// do not modify this collection.
        /// </summary>
        public ConcurrentDictionary<string, GameObject> Joints = new ConcurrentDictionary<string, GameObject>();


        /// <summary>
        /// The collection of bones. Do not modify this collection, but use the static methods defined in this class
        /// </summary>
        public List<Bone> bones = new List<Bone>();

        public GameObject GetJointGameObject(string name)
        {
            GameObject joint = null;

            if (Joints.ContainsKey(name))
            {
                joint = Joints[name];
            }

            return joint;
        }

        /// <summary>
        /// Creates the or updates a joint.
        /// </summary>
        /// <param name="name">The name of the joint to create or update.</param>
        /// <param name="position">The position of the joint.</param>
        public void CreateOrUpdateJoint(string name, Vector3 position)
        {
            if (!Joints.ContainsKey(name))
            {
                CreateAndAddJoint(name);
            }

            Joints[name].transform.position = new Vector3(position.x, position.y, position.z);

            foreach (var bone in bones)
            {
                if (bone.ContainsJoint(Joints[name]))
                {
                    bone.Update();
                }
            }
        }

        /// <summary>
        /// Creates the joint game object and adds it to the collection of joints.
        /// </summary>
        /// <param name="name">The name of the joint.</param>
        private void CreateAndAddJoint(string name)
        {
            var go = GameObject.CreatePrimitive(JointType);
            go.transform.localScale = new Vector3(JointSize.x, JointSize.y, JointSize.z);
            Joints.Add(name, go);
        }

        /// <summary>
        /// Destroys the specified joint.
        /// </summary>
        /// <param name="name">The name.</param>
        public void DestroyJoint(string name)
        {
            if (Joints.ContainsKey(name))
            {
                var joint = Joints[name];
                RemoveBonesForJoint(joint);
                Object.Destroy(Joints[name]);
                Joints.Remove(name);
            }
        }

        public void Clear()
        {
            foreach (var bone in bones)
            {
                Object.Destroy(bone.GetBoneGameObject());
            }

            foreach (var key in Joints.GetKeysArray())
            {
                Object.Destroy(Joints[key]);
            }
        }

        #region Bones
        public void AddBone(string joint1, string joint2, float thickness = .05f)
        {
            bones.Add(new Bone(Joints[joint1], Joints[joint2], thickness));
        }

        /// <summary>
        /// Removes the bones for joint.
        /// </summary>
        /// <param name="joint">The joint.</param>
        private void RemoveBonesForJoint(GameObject joint)
        {
            foreach (var bone in bones)
            {
                if (bone.ContainsJoint(joint))
                {
                    Object.Destroy(bone.GetBoneGameObject());
                    bones.Remove(bone);
                }
            }
        }
        #endregion
    }
}