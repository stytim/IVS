using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTurn : MonoBehaviour
{
    public Transform headAnchor;
    public GameObject outline;
    public Camera mainCamera;
    public bool isLookingAtCamera = true;

    private Vector3 originalPos;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = headAnchor.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    LookatMainCamera();
        //}
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    WaitForResponse();
        //}
    }

    public void WaitForResponse()
    {
        headAnchor.localPosition = originalPos - Vector3.right;
        outline.SetActive(false);
        isLookingAtCamera = false;
    }

    public void ReceivedResponse()
    {
        //headAnchor.localPosition = originalPos;
        //outline.SetActive(true);
        LookatMainCamera();
    }

    public void LookatMainCamera()
    {
        Debug.Log("Look at main camera");
        // offset in the direction of camera local transform  
        headAnchor.position = mainCamera.transform.TransformPoint(new Vector3(0.523f, 0.466f, 0));
        outline.SetActive(true);
        isLookingAtCamera = true;
    }
}
