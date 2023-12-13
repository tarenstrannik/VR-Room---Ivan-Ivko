using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.Rendering.GPUSort;

public class ToggleObjectRayInteractor : MonoBehaviour
{
    [SerializeField] private GameObject m_rayInteractor;
    [SerializeField] private bool m_isRayInteractorActive = false;
    [SerializeField] private bool m_isHoveringOnUI = false;
    [SerializeField]  private XRDirectInteractor m_directInteractor;

    private void Awake()
    {
        m_directInteractor = GetComponent<XRDirectInteractor>();
    }
    public void SetIsHoveringOnUI(bool state)
    {
        m_isHoveringOnUI = state;
    }

    public void SetRayInteractorActivity(bool state)
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
        //Debug.Log(m_isRayInteractorActive + " " + m_isHoveringOnUI + " " + m_directInteractor.interactablesSelected.Count);
        if (m_isRayInteractorActive&&!m_isHoveringOnUI && m_directInteractor.interactablesSelected.Count==0)
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
    public void ToggleRayInteractor(bool value)
    {
        if (!m_isHoveringOnUI && m_directInteractor.interactablesSelected.Count == 0)
        {
            m_rayInteractor.SetActive(value);
        }
    }


}
