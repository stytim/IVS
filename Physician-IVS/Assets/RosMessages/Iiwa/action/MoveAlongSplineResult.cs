//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Iiwa
{
    [Serializable]
    public class MoveAlongSplineResult : Message
    {
        public const string k_RosMessageName = "iiwa_msgs/MoveAlongSpline";
        public override string RosMessageName => k_RosMessageName;

        //  Result
        public bool success;
        public string error;

        public MoveAlongSplineResult()
        {
            this.success = false;
            this.error = "";
        }

        public MoveAlongSplineResult(bool success, string error)
        {
            this.success = success;
            this.error = error;
        }

        public static MoveAlongSplineResult Deserialize(MessageDeserializer deserializer) => new MoveAlongSplineResult(deserializer);

        private MoveAlongSplineResult(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.success);
            deserializer.Read(out this.error);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.success);
            serializer.Write(this.error);
        }

        public override string ToString()
        {
            return "MoveAlongSplineResult: " +
            "\nsuccess: " + success.ToString() +
            "\nerror: " + error.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize, MessageSubtopic.Result);
        }
    }
}
