using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class PickablePoint : MonoBehaviour
{
    public MeshRenderer InteractivePointMesh;
   [SerializeField] private TMP_Text textFrontMultipler;
    [SerializeField] private TMP_Text textBackMultipler;
    private Vector3 tempScale;
    private void Start()
    {
        tempScale = transform.localScale;
    }
    public void OnInteractivePointPickup()
    {
        Sequence pointSequence = DOTween.Sequence();
        pointSequence.Append( transform.DOScale(Vector3.zero, 0.2f));


        pointSequence.AppendCallback(()=>
        {
            transform.gameObject.SetActive(false);
            transform.localScale = tempScale;
        });
    }

    public void SetMultipler(string value)
    {
        textFrontMultipler.text = value;
        textBackMultipler.text = value;
    }



}
