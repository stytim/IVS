using UnityEngine;
using RosMessageTypes.Sensor;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;

public class TrajectorySubscriber : MonoBehaviour
{
    ROSConnection m_Ros;
    public string m_TopicName = "/trajTopic";
    public string m_CorrectedTopicName = "/trajTopic";
    public LLMHandler LLMHandler;
    public LineRenderer trajectoryLine;
    public bool pauseUpdate = false;
    public bool scanbeforedetected = false;
    private float lastPublishTime = 0f;
    private const float publishInterval = 1f;

    //[SerializeField]
    //private StylusHandler _stylusHandler;

    // Struct to hold position and rotation
    public struct TrajectoryPoint
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    // List of trajectory points
    public List<TrajectoryPoint> trajectoryPoints = new List<TrajectoryPoint>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Ros = ROSConnection.GetOrCreateInstance();
        //m_Ros.RegisterPublisher<PoseArrayMsg>(m_TopicName);
        m_Ros.Subscribe<PoseArrayMsg>(m_TopicName, ReceiveTrajectory);
        m_Ros.RegisterPublisher<PoseArrayMsg>(m_CorrectedTopicName);
    }

    // Update is called once per frame
    void Update()
    {
        //if (_stylusHandler.CurrentState.cluster_back_value)
        //{
        //    pauseUpdate = true;
        //}

        //if (_stylusHandler.CurrentState.cluster_front_value && Time.time - lastPublishTime > publishInterval)
        //{
        //    PublishTrajectory();
        //    trajectoryLine.material.color = Color.green;
        //    lastPublishTime = Time.time;
        //}
    }


    [ContextMenu("Refresh Trajectory")]
    public void RefreshPointCloud()
    {
        pauseUpdate = false;
    }

    void ReceiveTrajectory(PoseArrayMsg msg)
    {
        Debug.Log("Trajectory msg received");
        if (!pauseUpdate || true)
        {
            // Get the poses from the message
            PoseMsg[] poses = msg.poses;

            // Check if the count matches the existing trajectory points
            if (trajectoryPoints.Count != poses.Length)
            {
                // Clear previous data if count is different
                trajectoryPoints.Clear();

                // Loop through the poses and create new trajectory points
                foreach (PoseMsg pose in poses)
                {
                    Vector3 position = pose.position.From<FLU>();
                    Quaternion rotation = pose.orientation.From<FLU>();
                    trajectoryPoints.Add(new TrajectoryPoint { position = position, rotation = rotation });
                }
            }
            else
            {
                // Modify existing trajectory points if count matches
                for (int i = 0; i < poses.Length; i++)
                {
                    trajectoryPoints[i] = new TrajectoryPoint
                    {
                        position = poses[i].position.From<FLU>(),
                        rotation = poses[i].orientation.From<FLU>()
                    };
                }
            }

            // Update the LineRenderer to visualize the trajectory
            trajectoryLine.useWorldSpace = false;
            trajectoryLine.material.color = Color.red;
            trajectoryLine.positionCount = trajectoryPoints.Count;
            trajectoryLine.SetPositions(trajectoryPoints.ConvertAll(p => p.position).ToArray());
            if (trajectoryPoints.Count > 0)
            {
                if (!scanbeforedetected)
                {
                    LLMHandler.PatientQuestion("System: Ask for permission to scan.");
                }

            }

            Debug.Log("Trajectory updated");
            //pauseUpdate = true;
        }
    }

    [ContextMenu("Publish Trajectory")]
    public void PublishTrajectory()
    {
        // Create a message to hold the poses
        PoseArrayMsg msg = new PoseArrayMsg();

        // Create a list to hold the poses
        List<PoseMsg> poses = new List<PoseMsg>();

        // Loop through the positions in the LineRenderer
        for (int i = 0; i < trajectoryLine.positionCount; i++)
        {
            var pos = trajectoryLine.GetPosition(i).To<FLU>();
            Quaternion rot = Quaternion.identity;
            PoseMsg pose = new PoseMsg
            {
                position = new PointMsg
                {
                    x = pos.x,
                    y = pos.y,
                    z = pos.z
                },
                orientation = new QuaternionMsg
                {
                    x = rot.x,
                    y = rot.y,
                    z = rot.z,
                    w = rot.w
                }
            };
            poses.Add(pose);
        }

        // Set the poses in the message
        msg.poses = poses.ToArray();

        // Publish the message
        m_Ros.Publish(m_CorrectedTopicName, msg);
        Debug.Log("Trajectory published");
    }
}
