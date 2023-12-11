using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedCaptions : MonoBehaviour
{
    private bool m_isClosedCaptionsEnabled = false;
    private bool m_isClosedCaptionsActive = false;
    [SerializeField] private GameObject m_closedCaptions;
    public GameObject ClosedCaption
    { 
        get
        {
            return m_closedCaptions;
        }
    }

    public void ToggleClosedCaptionsEnable(bool value)
    {
        m_isClosedCaptionsEnabled = value;
    }

    private void ToggleClosedCaptionsActivity(bool value)
    {
        m_isClosedCaptionsActive = value;
    }

    private void Update()
    {
        m_closedCaptions.SetActive(m_isClosedCaptionsEnabled && m_isClosedCaptionsActive);
    }

}
