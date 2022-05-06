using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPathGizmos : MonoBehaviour
{
    public BoxCollider boxCollider;


    private void OnDrawGizmos()
    {
        //if (boxCollider != null)
        //{
        //    Gizmos.color = new Color(1, 0, 0, 0.5f);
        //    Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        //    Gizmos.DrawCube(Vector3.zero + boxCollider.center, boxCollider.size);
        //}
    }
}
