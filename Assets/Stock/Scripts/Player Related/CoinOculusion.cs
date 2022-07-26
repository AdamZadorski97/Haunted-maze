using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinOculusion : MonoBehaviour
{
    public float cullingRadius = 10;
    public GameObject rig;

    CullingGroup m_CullingGroup;

   
    void Start()
    {
        
        if (m_CullingGroup == null)
        {
            m_CullingGroup = new CullingGroup();
            m_CullingGroup.targetCamera = LevelManager.Instance.cameraMain;
            m_CullingGroup.SetBoundingSpheres(new[] { new BoundingSphere(transform.position, cullingRadius) });
            m_CullingGroup.SetBoundingSphereCount(1);
            m_CullingGroup.onStateChanged += OnStateChanged;

            Cull(m_CullingGroup.IsVisible(0));
        }

        m_CullingGroup.enabled = true;
    }

    void OnDisable()
    {
        if (m_CullingGroup != null)
            m_CullingGroup.enabled = false;

        SetRig(true);
    }

    void OnDestroy()
    {
        if (m_CullingGroup != null)
            m_CullingGroup.Dispose();
    }

    void OnStateChanged(CullingGroupEvent sphere)
    {
        Cull(sphere.isVisible);
    }

    void Cull(bool visible)
    {
        if (visible)
        {
            SetRig(true);
        }
        else
        {
            SetRig(false);
        }
    }

    void SetRig(bool enable)
    {
        rig.SetActive(enable);
    }

    private void OnDrawGizmosSelected()
    {
        if (enabled)
        {
            Color col = Color.red;
            if (m_CullingGroup != null && !m_CullingGroup.IsVisible(0))
                col = Color.gray;

            Gizmos.color = col;
            Gizmos.DrawWireSphere(transform.position, cullingRadius);
        }
    }
}

