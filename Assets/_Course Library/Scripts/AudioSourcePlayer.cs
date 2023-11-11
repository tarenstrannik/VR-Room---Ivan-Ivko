using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
[RequireComponent(typeof(AudioSource))] 

public class AudioSourcePlayer : MonoBehaviour
{

    protected AudioSource audioSource;
    [SerializeField] protected XRSocketInteractor recordInteractor;
    protected AudioClip audioClip;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        audioSource=GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void PlayRecord()
    {
       
        audioClip =recordInteractor.GetOldestInteractableSelected().transform.gameObject.GetComponent<AudioSourceObject>().AudioRecord;
        
        if (audioClip != null) audioSource.PlayOneShot(audioClip);
    }

    public virtual void StopRecord()
    {
        audioSource.Stop();
    }
}
