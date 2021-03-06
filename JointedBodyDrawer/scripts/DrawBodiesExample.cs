﻿using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

namespace JointedBodyDrawer
{
    /// <summary>
    /// An example body drawer using the Kinect v2.0
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class DrawBodiesExample : MonoBehaviour
    {
        /// <summary>
        /// The jointed bodies in the scene
        /// </summary>
        ConcurrentDictionary<ulong, JointedBody> bodies = new ConcurrentDictionary<ulong, JointedBody>();

        void Update()
        {
            // Check if kinect has any body data
            if (Kinect.HasBodyData())
            {
                // Get Tracked Bodies
                var trackedBodies = Kinect.GetTrackedBodies();

                // Track bodies in the frame
                List<ulong> trackedBodiesIDsThisFrame = new List<ulong>();

                // Loop through tracked bodies
                foreach (var detectedBody in trackedBodies)
                {
                    // Determine if the body is new to the scene
                    bool isNew = !bodies.ContainsKey(detectedBody.TrackingId);
                    
                    if (isNew)
                    {
                        // Create new body
                        bodies.Add(detectedBody.TrackingId, new JointedBody());
                    }

                    // Update the joints in the body
                    UpdateBody(bodies[detectedBody.TrackingId], detectedBody.Joints);

                    if (isNew)
                    {
                        // Add the bones to the body
                        AddBones(bodies[detectedBody.TrackingId]);

                        // Set the joints' color
                        SetJointColor(bodies[detectedBody.TrackingId]);
                    }

                    // Add body id to tracked
                    trackedBodiesIDsThisFrame.Add(detectedBody.TrackingId);
                }

                foreach (var key in bodies.GetKeysArray())
                {
                    // Find old bodies in the dictionary
                    if (!trackedBodiesIDsThisFrame.Contains(key))
                    {
                        // Clear them
                        bodies[key].Clear();

                        // Remove them from the dictionary
                        bodies.Remove(key);
                    }
                }
            }
            else
            {
                foreach (var key in bodies.GetKeysArray())
                {
                    bodies[key].Clear();
                }

                bodies = new ConcurrentDictionary<ulong, JointedBody>();
            }
        }

        /// <summary>
        /// Adds the bones to a new body
        /// </summary>
        /// <param name="body">The body.</param>
        private void AddBones(JointedBody body)
        {
            // Head - Shoulder
            body.AddBone(JointType.Head.ToString(), JointType.SpineShoulder.ToString(), .05f);
            // Sholder - Shoulder Right
            body.AddBone(JointType.SpineShoulder.ToString(), JointType.ShoulderRight.ToString(), .05f);
            // Shoulder Right - Elbow
            body.AddBone(JointType.ShoulderRight.ToString(), JointType.ElbowRight.ToString(), .05f);
            // Elbow Right - Wrist
            body.AddBone(JointType.ElbowRight.ToString(), JointType.WristRight.ToString(), .05f);
            // Wrist Right - Hand
            body.AddBone(JointType.WristRight.ToString(), JointType.HandRight.ToString(), .05f);
            // Wrist Right - Thumb
            body.AddBone(JointType.WristRight.ToString(), JointType.ThumbRight.ToString(), .05f);
            // Shoulder - Shoulder Left
            body.AddBone(JointType.SpineShoulder.ToString(), JointType.ShoulderLeft.ToString(), .05f);
            // Shoulder Left - Elbow
            body.AddBone(JointType.ShoulderLeft.ToString(), JointType.ElbowLeft.ToString(), .05f);
            // Elbow Left - Wrist
            body.AddBone(JointType.ElbowLeft.ToString(), JointType.WristLeft.ToString(), .05f);
            // Wrist Left - Hand
            body.AddBone(JointType.WristLeft.ToString(), JointType.HandLeft.ToString(), .05f);
            // Wrist Left - Thumb
            body.AddBone(JointType.WristLeft.ToString(), JointType.ThumbLeft.ToString(), .05f);
            // Shoulder - Spine Mid
            body.AddBone(JointType.SpineShoulder.ToString(), JointType.SpineMid.ToString(), .05f);
            // Spine Mid - Spine Base
            body.AddBone(JointType.SpineMid.ToString(), JointType.SpineBase.ToString(), .05f);
            // Spine Base - Hip Right
            body.AddBone(JointType.SpineBase.ToString(), JointType.HipRight.ToString(), .05f);
            // Hip Right - Knee
            body.AddBone(JointType.HipRight.ToString(), JointType.KneeRight.ToString(), .05f);
            // Knee Right - Ankle
            body.AddBone(JointType.KneeRight.ToString(), JointType.AnkleRight.ToString(), .05f);
            //Ankle Right - Foot Right
            body.AddBone(JointType.AnkleRight.ToString(), JointType.FootRight.ToString(), .05f);
            // Spine Base - Hip Left
            body.AddBone(JointType.SpineBase.ToString(), JointType.HipLeft.ToString(), .05f);
            // Hip Left - Knee
            body.AddBone(JointType.HipLeft.ToString(), JointType.KneeLeft.ToString(), .05f);
            // Knee Left - Ankle
            body.AddBone(JointType.KneeLeft.ToString(), JointType.AnkleLeft.ToString(), .05f);
            //Ankle Left - Foot Left
            body.AddBone(JointType.AnkleLeft.ToString(), JointType.FootLeft.ToString(), .05f);
        }

        /// <summary>
        /// Updates the joint positions in the jointed body based on the kinect data
        /// </summary>
        /// <param name="jointedBody">The jointed body.</param>
        /// <param name="joints">The kinect joints.</param>
        private void UpdateBody(JointedBody jointedBody, Dictionary<JointType, Windows.Kinect.Joint> joints)
        {
            foreach (var joint in joints)
            {
                string jointName = joint.Key.ToString();
                Vector3 jointPosition = new Vector3(joint.Value.Position.X, joint.Value.Position.Y + 1, joint.Value.Position.Z);
                jointedBody.CreateOrUpdateJoint(jointName, jointPosition);
            }
        }

        /// <summary>
        /// Sets the color of the joint.
        /// </summary>
        /// <param name="jointedBody">The jointed body.</param>
        private void SetJointColor(JointedBody jointedBody)
        {
            Color meshColor = GetNextColor();

            foreach (var key in jointedBody.Joints.GetKeysArray())
            {
                var materialColored = new Material(Shader.Find("Diffuse"));
                materialColored.color = meshColor;
                jointedBody.Joints[key].GetComponent<Renderer>().material = materialColored;
            }
        }

        /// <summary>
        /// Gets the next color in the array of colors
        /// </summary>
        /// <returns></returns>
        private static Color GetNextColor()
        {
            Color returnColor = Color.black;
            colorCounter++;

            if (colorCounter == colors.Length)
            {
                colorCounter = 0;
            }

            returnColor = colors[colorCounter];

            return returnColor;
        }

        /// <summary>
        /// The possible colors for the joints
        /// </summary>
        private static Color[] colors = new Color[] { Color.red, Color.green, Color.yellow, Color.blue, Color.magenta, Color.cyan, Color.black };
        private static int colorCounter = 0;
    }
}