using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using RosMessageTypes.Std;

public class VesselLocationSubscriber : MonoBehaviour
{
    ROSConnection m_Ros;
    public string m_TopicName = "/vessel_location";
    public LLMHandler llmHandler;
    private bool  firsttime = true;
    // Start is called before the first frame update
    void Start()
    {
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.Subscribe<StringMsg>(m_TopicName, ReceiveVesselLocation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Test")]
    void Test()
    {
        llmHandler.PatientQuestion("System: Vessel is at location bottom left, notify the doctor");
    }

    void ReceiveVesselLocation(StringMsg msg)
    {

        if (firsttime)
        {
            llmHandler.DoctorQuestion("System: Found blood vessel, notify the doctor.");
            //// switch case to determine the vessel location
            // switch (msg.data)
            // {
            //     case "1":
            //         llmHandler.DoctorQuestion("System: Vessel is at location top right, notify the doctor");
            //         break;
            //     case "2":
            //         llmHandler.DoctorQuestion("System: Vessel is at location top left, notify the doctor");
            //         break;
            //     case "3":
            //         llmHandler.DoctorQuestion("System: Vessel is at location bottom left, notify the doctor");
            //         break;
            //     case "4":
            //         llmHandler.DoctorQuestion("System: Vessel is at location bottom right, notify the doctor");
            //         break;
            //     default:
            //         Debug.Log("System: Vessel location is unknown");
            //         break;
            // }
            firsttime = false;
        }
    }
}
