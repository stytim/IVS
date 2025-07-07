using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchUSProbe : MonoBehaviour
{

    public Transform USprobe;
    public Transform Handtarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Handtarget.transform.position = USprobe.transform.position;
        Handtarget.transform.rotation = USprobe.transform.rotation;
    }
}
