using UnityEngine;
using DG.Tweening;

public class CoinPicker : MonoBehaviour
{
  
  
    [SerializeField] private float pointLightIntencity;
    [SerializeField] private float pointLightIntencityTime;
    [SerializeField] private AnimationCurve pointLightIntencityCurve;
    [SerializeField] private AudioClip coinPickupSound;

    [SerializeField] private BoxCollider boxCollider;

    [SerializeField] private Vector3 defaultCenter;
    [SerializeField] private Vector3 defaultSize;
    [SerializeField] private Vector3 bigCenter;
    [SerializeField] private Vector3 bigSize;


    [SerializeField] private Light moneyPointLight;
    [SerializeField] private AudioSource audioSource;
    

    public void ChangeColliderSize(bool isBig)
    {
        if (isBig)
        {
            boxCollider.center = bigCenter;
            boxCollider.size = bigSize;
        }
        else
        {
            boxCollider.center = defaultCenter;
            boxCollider.size = defaultSize;
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PickablePoint>())
        {
            other.GetComponent<PickablePoint>().OnInteractivePointPickup();
            audioSource.PlayOneShot(coinPickupSound, 0.5f);
            LevelManager.Instance.dataManager.SetPoint();
            Sequence pointSequence = DOTween.Sequence();
            pointSequence.Append(moneyPointLight.DOIntensity(pointLightIntencity, pointLightIntencityTime).SetEase(pointLightIntencityCurve));
            pointSequence.Append(moneyPointLight.DOIntensity(0, pointLightIntencityTime).SetEase(pointLightIntencityCurve));
        }
    }
}