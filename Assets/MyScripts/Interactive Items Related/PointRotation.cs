using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PointRotation : MonoBehaviour
{
   [SerializeField] private float coinRotationSpeed = 2;

    private void OnEnable()
    {
        Vector3 rot = new Vector3(0, 360, 0);
        transform.DOLocalRotate(rot, coinRotationSpeed, RotateMode.FastBeyond360).SetRelative(true).SetLoops(-1).SetEase(Ease.Linear);
    }
}
