using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;
using System.Linq;
using System;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using System.Collections.Generic;

public class RSPointCloud2Visualizer : MonoBehaviour
{
    ROSConnection rosConnection;

    [SerializeField]
    string pointCloudTopic = "/point_cloud";

    [SerializeField]
    Transform pointCloudParent; // Parent transform for organizing the point cloud

    [SerializeField]
    Transform RealsensePose; // Parent transform for organizing the point cloud

    [SerializeField]
    Material pointMaterial; // Material for the point cloud mesh

    [SerializeField]
    float pointSize = 0.01f; // Size of each point (quad or sphere)

    [SerializeField]
    bool visualizeAsMesh = false;

    bool isPointCloudInitialized = false;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    void Start()
    {
        rosConnection = ROSConnection.GetOrCreateInstance();
        rosConnection.Subscribe<PointCloud2Msg>(pointCloudTopic, OnPointCloudReceived);

        // Create a GameObject to hold the point cloud mesh
        GameObject pointCloudObject = new GameObject("PointCloudMesh");
        if (pointCloudParent != null)
        {
            pointCloudObject.transform.SetParent(pointCloudParent, false);
        }
        meshFilter = pointCloudObject.AddComponent<MeshFilter>();
        meshRenderer = pointCloudObject.AddComponent<MeshRenderer>();
        meshRenderer.material = pointMaterial;
    }

    [ContextMenu("Refresh Point Cloud")]
    public void RefreshPointCloud()
    {
        isPointCloudInitialized = false;
    }

    void OnPointCloudReceived(PointCloud2Msg message)
    {

        // Only update the point cloud if it has not yet been initialized
        if (!isPointCloudInitialized)
        {
            pointCloudParent.position = RealsensePose.position;
            pointCloudParent.rotation = RealsensePose.rotation;
            // Create dynamic lists to store only the points within 2 meters.
            var verticesList = new List<Vector3>();
            var colorsList = new List<Color>();

            // var indicesList = new List<int>();

            // Get the offsets for the x, y, and z fields
            int xChannelOffset = (int)message.fields.First(f => f.name == "x").offset;
            int yChannelOffset = (int)message.fields.First(f => f.name == "y").offset;
            int zChannelOffset = (int)message.fields.First(f => f.name == "z").offset;

            // Find the offset for the rgb/rgba field (if available)
            var rgbField = message.fields.FirstOrDefault(f => f.name == "rgb" || f.name == "rgba");
            int rgbChannelOffset = rgbField != null ? (int)rgbField.offset : -1;

            // Calculate the number of points in the message
            var pointCount = message.data.Length / message.point_step;

            // Loop through each point in the cloud
            for (int i = 0; i < pointCount; i++)
            {
                int iPointStep = i * (int)message.point_step;

                // Extract the x, y, z coordinates from the byte array
                float x = BitConverter.ToSingle(message.data, iPointStep + xChannelOffset);
                float y = BitConverter.ToSingle(message.data, iPointStep + yChannelOffset);
                float z = BitConverter.ToSingle(message.data, iPointStep + zChannelOffset);

                // Ignore NaN values
                if (float.IsNaN(x) || float.IsNaN(y) || float.IsNaN(z))
                {
                    continue;
                }

                // Convert from ROS FLU to Unity coordinates
                Vector3<FLU> rosPoint = new Vector3<FLU>(x, y, z);
                Vector3 unityPoint = rosPoint.toUnity;

                // Only add points that are within 2 meters (distance from origin)
                if (unityPoint.magnitude > 0.8f)
                {
                    continue; // Skip this point if it is more than 2 meters away.
                }

                // Add the valid point to the list of vertices
                verticesList.Add(unityPoint);

                // Extract and add the color information
                if (rgbChannelOffset >= 0)
                {
                    int packedColor = BitConverter.ToInt32(message.data, iPointStep + rgbChannelOffset);
                    byte r = (byte)((packedColor >> 16) & 0xFF);
                    byte g = (byte)((packedColor >> 8) & 0xFF);
                    byte b = (byte)(packedColor & 0xFF);
                    colorsList.Add(new Color32(r, g, b, 255));
                }
                else
                {
                    colorsList.Add(Color.white); // Use white as a default if no color is provided
                }

                // The index for this vertex is its position in the list.
               // indicesList.Add(verticesList.Count - 1);
            }

            // Create a new mesh and assign the filtered vertices, colors, and indices.
            Mesh mesh = new Mesh();

            // Use a 32-bit index format if needed.
            mesh.indexFormat = verticesList.Count > 65535 ?
                               UnityEngine.Rendering.IndexFormat.UInt32 :
                               UnityEngine.Rendering.IndexFormat.UInt16;
            
            if (verticesList.Count != 0)
            {

                mesh.SetVertices(verticesList);
                mesh.SetColors(colorsList);

                if (visualizeAsMesh)
                {
                    // Generate faces using Convex Hull algorithm
                    int[] triangleIndices = ConvexHull.Generate(verticesList.ToArray());
                    mesh.SetTriangles(triangleIndices, 0);
                }
                else
                {
                    int[] indicesList = Enumerable.Range(0, verticesList.Count).ToArray();
                    mesh.SetIndices(indicesList, MeshTopology.Points, 0);
                }

                //mesh.SetIndices(indicesList.ToArray(), MeshTopology.Points, 0);

                // Assign the mesh to the MeshFilter to render it.
                meshFilter.mesh = mesh;
                isPointCloudInitialized = true;
                Debug.Log("Point cloud initialized with " + verticesList.Count + " points.");
            }

    }
    }

}

