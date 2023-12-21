using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountBoxes : MonoBehaviour
{
    [SerializeField] GameManager m_gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Box"))
        {
            
            m_gameManager.UpdateBoxCount(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            
            m_gameManager.UpdateBoxCount(-1);
           
        }
    }
}
