using Meta.XR.MultiplayerBlocks.Fusion.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCalibration : MonoBehaviour
{
    public OVRInput.Controller controller = OVRInput.Controller.RTouch;
    public MachineHome machineHome;
    public GameObject box;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((OVRInput.GetUp(OVRInput.Button.One, controller)))
        {
            // Display the box
            box.SetActive(true);
            DeleteAnchor();
        }

        if (OVRInput.GetUp(OVRInput.Button.Two, controller))
        {
            box.SetActive(false);
            machineHome.AddAnchor(transform.position, transform.rotation);
        }
    }

    void DeleteAnchor()
    {
        StartCoroutine(EraseAnchor());
    }

    private IEnumerator EraseAnchor()
    {
        yield return SpatialAnchorHandler.EraseAnchor(gameObject);
    }
}
