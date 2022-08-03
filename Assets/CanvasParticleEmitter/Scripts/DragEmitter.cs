using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragEmitter : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{


    [SerializeField] private CanvasParticleEmitter m_particleEmitter;
    [SerializeField] private Toggle m_worldSpaceToggle;
    protected RectTransform m_rectTransform;

    public virtual void Awake()
    {
        m_rectTransform = m_particleEmitter.GetComponent<RectTransform>();

        m_worldSpaceToggle.onValueChanged.AddListener((isOn) => {
            m_particleEmitter.WorldSpace = isOn;
        });
    }


    private void FixedUpdate()
    {
        //if (isDragging == true && isDraggable == true)
        //{
        //    Vector2 localPos = Vector2.zero;
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, Input.mousePosition, null, out localPos);
        //    m_rectTransform.position = m_rectTransform.TransformPoint(localPos);
        //    return;
        //}
    }

    public void OnDrag(PointerEventData eventData)
    {
        //m_touchOffset = eventData.position;// + m_originalPosition;
        m_rectTransform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //isDragging = false;
    }
}
