using UnityEngine;

namespace JointedBodyDrawer
{
    /// <summary>
    /// Creates a bone between two joints
    /// </summary>
    public class Bone
    {
        /// <summary>
        /// The first joint
        /// </summary>
        private GameObject joint1;

        /// <summary>
        /// The second joint
        /// </summary>
        private GameObject joint2;

        /// <summary>
        /// The bone
        /// </summary>
        private GameObject bone;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bone"/> class.
        /// </summary>
        /// <param name="joint1">The first joint.</param>
        /// <param name="joint2">The second joint.</param>
        /// <param name="thickness">The thickness of the bone.</param>
        internal Bone(GameObject joint1, GameObject joint2, float thickness)
        {
            bone = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            bone.transform.localScale = new Vector3(thickness, thickness, thickness);
            bone.name = joint1.name + "-" + joint2.name + "Bone";
            this.joint1 = joint1;
            this.joint2 = joint2;
        }

        /// <summary>
        /// Updates the bone's position and rotation.
        /// </summary>
        public void Update()
        {
            float length = GetBoneLength(joint1, joint2) / 2;

            // Scale bone along local y to the length between two points
            bone.transform.localScale = new Vector3(bone.transform.localScale.x, length, bone.transform.localScale.z);

            // Set position to midpoint
            bone.transform.position = GetBoneMidpoint(joint1, joint2);

            // Set "up" vector to match line between two points
            bone.transform.up = joint2.transform.position - joint1.transform.position;
        }

        /// <summary>
        /// Determines whether the specified bone contains joint.
        /// </summary>
        /// <param name="joint">The joint.</param>
        /// <returns>
        ///   <c>true</c> if the specified bone contains joint; otherwise, <c>false</c>.
        /// </returns>
        internal bool ContainsJoint(GameObject joint)
        {
            return joint1.Equals(joint) || joint2.Equals(joint);
        }

        /// <summary>
        /// Gets the bone game object.
        /// </summary>
        /// <returns></returns>
        public GameObject GetBoneGameObject()
        {
            return bone;
        }

        /// <summary>
        /// Gets the length of the bone.
        /// </summary>
        /// <param name="joint1">The first joint.</param>
        /// <param name="joint2">The second joint.</param>
        /// <returns></returns>
        private float GetBoneLength(GameObject joint1, GameObject joint2)
        {
            return Vector3.Distance(joint1.transform.position, joint2.transform.position);
        }

        /// <summary>
        /// Gets the bone midpoint.
        /// </summary>
        /// <param name="joint1">The first joint.</param>
        /// <param name="joint2">The second joint.</param>
        /// <returns></returns>
        private Vector3 GetBoneMidpoint(GameObject joint1, GameObject joint2)
        {
            return Vector3.Lerp(joint1.transform.position, joint2.transform.position, 0.5f);
        }
    }
}