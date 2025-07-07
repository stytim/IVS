using RosMessageTypes.Std;
using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;

public class ProcedureStateSubscriber : MonoBehaviour
{
    ROSConnection m_Ros;
    public string m_TopicName = "/robot_end";
    public ConversationHandler conversationHandler;
    public MovementController movementController;
    // Start is called before the first frame update
    void Start()
    {
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.Subscribe<BoolMsg>(m_TopicName, ReceiveStatus);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReceiveStatus(BoolMsg msg)
    {
        if (msg.data)
        {
            conversationHandler.procedureEnded = true;
            movementController.StopInteraction();
            Debug.Log("System: Procedure has ended");
        }
        else
        {
            Debug.Log("System: Procedure has started");
        }
    }
}
