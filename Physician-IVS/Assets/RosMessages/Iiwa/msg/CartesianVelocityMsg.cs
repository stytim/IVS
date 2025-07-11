//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;

namespace RosMessageTypes.Iiwa
{
    [Serializable]
    public class CartesianVelocityMsg : Message
    {
        public const string k_RosMessageName = "iiwa_msgs/CartesianVelocity";
        public override string RosMessageName => k_RosMessageName;

        public HeaderMsg header;
        public CartesianQuantityMsg velocity;

        public CartesianVelocityMsg()
        {
            this.header = new HeaderMsg();
            this.velocity = new CartesianQuantityMsg();
        }

        public CartesianVelocityMsg(HeaderMsg header, CartesianQuantityMsg velocity)
        {
            this.header = header;
            this.velocity = velocity;
        }

        public static CartesianVelocityMsg Deserialize(MessageDeserializer deserializer) => new CartesianVelocityMsg(deserializer);

        private CartesianVelocityMsg(MessageDeserializer deserializer)
        {
            this.header = HeaderMsg.Deserialize(deserializer);
            this.velocity = CartesianQuantityMsg.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.Write(this.velocity);
        }

        public override string ToString()
        {
            return "CartesianVelocityMsg: " +
            "\nheader: " + header.ToString() +
            "\nvelocity: " + velocity.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize);
        }
    }
}
