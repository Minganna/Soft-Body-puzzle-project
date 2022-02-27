using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonePos : MonoBehaviour
{
    public Vector3 startPos;
    public Quaternion startRot;
    void Start()
    {
        startPos = this.transform.localPosition;
        startRot = this.transform.rotation;
    }
}
