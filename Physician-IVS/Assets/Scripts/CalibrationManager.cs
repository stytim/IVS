using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationManager : MonoBehaviour
{

    public AvatarCalibration avatarCalibration;
    public RobotCalibration robotCalibration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            robotCalibration.enabled = true;
        }
    }


    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, 140, 60));

        if (GUILayout.Button("Enable Calibration"))
        {
            // avatarCalibration.enabled = true;
            robotCalibration.enabled = true;
        }

        if (GUILayout.Button("Disable Calibration"))
        {
            //avatarCalibration.enabled = false;
            robotCalibration.enabled = false;
        }

        GUILayout.EndArea();
    }
}
