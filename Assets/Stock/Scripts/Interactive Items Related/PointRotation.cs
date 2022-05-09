using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointRotation : MonoBehaviour
{
    public float speed = 1;

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0,1,0), speed);
    }
}
