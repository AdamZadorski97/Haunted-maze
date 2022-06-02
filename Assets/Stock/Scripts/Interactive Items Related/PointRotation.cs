using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PointRotation : MonoBehaviour
{
    public float speed = 1;

    //private void FixedUpdate()
    //{
    //    transform.Rotate(new Vector3(0,0,1), speed);
    //}

    private void Start()
    {
        Vector3 rot = new Vector3(0, 0, 360);
        transform.DOLocalRotate(rot, 2f, RotateMode.FastBeyond360).SetRelative(true).SetLoops(-1).SetEase(Ease.Linear);
    }
}
