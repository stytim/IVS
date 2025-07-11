//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Iiwa
{
    [Serializable]
    public class MoveToCartesianPoseFeedback : Message
    {
        public const string k_RosMessageName = "iiwa_msgs/MoveToCartesianPose";
        public override string RosMessageName => k_RosMessageName;

        //  Feedback

        public MoveToCartesianPoseFeedback()
        {
        }
        public static MoveToCartesianPoseFeedback Deserialize(MessageDeserializer deserializer) => new MoveToCartesianPoseFeedback(deserializer);

        private MoveToCartesianPoseFeedback(MessageDeserializer deserializer)
        {
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
        }

        public override string ToString()
        {
            return "MoveToCartesianPoseFeedback: ";
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize, MessageSubtopic.Feedback);
        }
    }
}
