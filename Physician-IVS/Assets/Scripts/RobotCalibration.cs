using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCalibration : MonoBehaviour
{
    public OVRInput.Controller controller = OVRInput.Controller.RTouch;
    public MachineHome machineHome;
    public GameObject robotDup;
    // public GameObject box;
    void Update()
    {
        if ((OVRInput.GetUp(OVRInput.Button.One, controller)))
        {
            // Display the box
            // box.SetActive(true);
            StartCalibration();
            //GetComponent<DisableArticulationBody>().enabled = true;
        }

        if (OVRInput.GetUp(OVRInput.Button.Two, controller))
        {
            EndCalibration();
            // box.SetActive(false);
            //GetComponent<DisableArticulationBody>().enabled = false;
            // machineHome.AddAnchor(transform.position, transform.rotation);
        }
    }


    [ContextMenu("Start Calibration")]
    void StartCalibration()
    {
        robotDup.SetActive(true);
        robotDup.transform.position = machineHome.transform.position;
        robotDup.transform.rotation = machineHome.transform.rotation;
        GameObject robot = machineHome.gameObject;
        // go throught all transforms of robot and copy the position and rotation to robotDup
        // Recursively copy transforms
        CopyTransformRecursive(robot.transform, robotDup.transform);

    }

    [ContextMenu("End Calibration")]
    void EndCalibration()
    {
        //DeleteAnchor();
        machineHome.AddAnchor(robotDup.transform.position, robotDup.transform.rotation);
        robotDup.SetActive(false);
    }

    void CopyTransformRecursive(Transform source, Transform target)
    {
        if (source.childCount == target.childCount)
        {
            //Debug.LogError(source.name + " " + target.name);
            Debug.LogError("Source and target do not have the same structure!");
            return;
        }

        for (int i = 0; i < source.childCount; i++)
        {
            Transform sourceChild = source.GetChild(i);
            Transform targetChild = target.GetChild(i);

            // Copy position and rotation
            targetChild.position = sourceChild.position;
            targetChild.rotation = sourceChild.rotation;

            // Recursively copy children
            CopyTransformRecursive(sourceChild, targetChild);
        }
    }


    [ContextMenu("Delete Anchor")]
    void DeleteAnchor()
    {
        StartCoroutine(EraseAnchor());
    }

    private IEnumerator EraseAnchor()
    {
        yield return SpatialAnchorHandler.EraseAnchor(gameObject);
    }
}
