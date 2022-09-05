using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class PickablePoint : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshBorder;
    [SerializeField] private MeshRenderer meshPlate;
    [SerializeField] private MeshRenderer glow;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Color glowAlpha;
    private Vector3 tempScale;
    private Vector3 tempPosition;

    private void Start()
    {
        tempScale = transform.localScale;
        tempPosition = transform.position;
        glowAlpha = glow.material.GetColor("_Color");
    }
    public void OnInteractivePointPickup()
    {
        boxCollider.enabled = false;
        Sequence pointSequence = DOTween.Sequence();
        pointSequence.Append( transform.DOScale(Vector3.zero, 0.45f));
        pointSequence.Join(transform.DOMove(PlayerController.Instance.transform.position, 0.45f));
        pointSequence.Join(glow.material.DOColor(Vector4.zero, "_Color", 0.45f));


        pointSequence.AppendCallback(()=>
        {
            transform.gameObject.SetActive(false);
            transform.localScale = tempScale;
            transform.position = tempPosition;
            glow.gameObject.SetActive(false);
            glow.material.SetColor("_Color", glowAlpha);
        });
    }

    public void SetMultipler(Texture texture, Color borderColor, Color plateColor)
    {
        glow.gameObject.SetActive(true);
        boxCollider.enabled = true;
        meshBorder.material.SetColor("_Color", borderColor);
        meshPlate.material.SetTexture("_BaseMap", texture);
        meshPlate.material.SetColor("_BaseColor", plateColor);
        glow.material.SetColor("_Color", plateColor);
    }
}
