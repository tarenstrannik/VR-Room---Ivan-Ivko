using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.Rendering.GPUSort;

public class OnAttachDisableCollisionWithLayers : MonoBehaviour
{
    [SerializeField] private int m_collisionLayerToExcludeBody;
    [SerializeField] private int m_collisionLayerToExcludeBodyIfFiringTorch;

    [SerializeField] private int m_defaultCollisionLayer;
    [SerializeField] private int m_defaultFireCollisionLayer;


    [SerializeField] private float m_changingDelay = 0.5f;
    [SerializeField] private float m_changingBackDelay = 0.5f;

    private Coroutine m_changeWithDelay=null;
    private Coroutine m_unchangeWithDelay = null;
    public void ChangeCollisionLayer(SelectEnterEventArgs args)
    {

        StopCoroutines();
        if (args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>()!=null && !args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>().IsFireStopped)
        {
            args.interactableObject.transform.gameObject.SetLayerRecursively(m_collisionLayerToExcludeBodyIfFiringTorch);
        }
        else
        {
            args.interactableObject.transform.gameObject.SetLayerRecursively(m_collisionLayerToExcludeBody);
        }
        
    }

    public void UnchangeCollisionLayer(SelectExitEventArgs args)
    {

        StopCoroutines();
        if (args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>() != null && !args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>().IsFireStopped)
        {
            args.interactableObject.transform.gameObject.SetLayerRecursively(m_defaultFireCollisionLayer);
        }
        else
        {
            args.interactableObject.transform.gameObject.SetLayerRecursively(m_defaultCollisionLayer);
        }
        
    }

    private void StopCoroutines()
    {
        if (m_changeWithDelay != null) StopCoroutine(m_changeWithDelay);
        m_changeWithDelay = null;
        if (m_unchangeWithDelay != null) StopCoroutine(m_unchangeWithDelay);
        m_unchangeWithDelay = null;
    }


    public void ChangeCollisionLayerWithDelay(SelectEnterEventArgs args)
    {

        StopCoroutines();
        m_changeWithDelay=StartCoroutine(DelayBeforeChangeCollisionLayer(args));
    }

    private IEnumerator DelayBeforeChangeCollisionLayer(SelectEnterEventArgs args)
    {
        var curDelay = m_changingDelay;
        while(curDelay>=0)
        {
            curDelay -= Time.deltaTime;
            yield return null;
        }
        ChangeCollisionLayer(args);
    }

    public void UnchangeCollisionLayerWithDelay(SelectExitEventArgs args)
    {

        StopCoroutines();
        m_unchangeWithDelay = StartCoroutine(DelayBeforeUnChangeCollisionLayer(args));
    }

    private IEnumerator DelayBeforeUnChangeCollisionLayer(SelectExitEventArgs args)
    {
        var curDelay = m_changingBackDelay;
        while (curDelay >= 0)
        {
            curDelay -= Time.deltaTime;
            yield return null;
        }
        UnchangeCollisionLayer(args);
    }


}

