using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.Rendering.GPUSort;

public class ToggleObjectRayInteractor : MonoBehaviour
{
    [SerializeField] private GameObject m_rayInteractor;
    [SerializeField] private bool m_isRayInteractorActive = false;

    public void SetRayIntersactorActivity(bool state)
    {
        m_isRayInteractorActive = state;
    }

    public void EnableRayInteractorIfObjectInHand(SelectExitEventArgs args)
    {
        if(m_isRayInteractorActive)
        {

                m_rayInteractor.SetActive(true);

        }
    }

    public void DisableRayInteractorIfObjectInHand(SelectEnterEventArgs args)
    {
        if(m_isRayInteractorActive)
        {
            if (args.interactableObject != null)
            {
                m_rayInteractor.SetActive(false);
            }
        }
    }
    public void EnableRayInteractorNoCondition()
    {
        if (m_isRayInteractorActive)
        {

            m_rayInteractor.SetActive(true);

        }
    }
    public void DisableRayInteractorNoCondition()
    {
        if (m_isRayInteractorActive)
        {
           
                m_rayInteractor.SetActive(false);

        }
    }
}
