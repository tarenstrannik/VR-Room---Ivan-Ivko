using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class PlateRecordPlayer : AudioPlayer
{

    private Coroutine rotationCoroutine;
    private Coroutine handleMovingWhileAudioPlayingCoroutine;
    private Coroutine handlePrepareCoroutine;
    private Coroutine afterHandlePrepareCoroutine;

    [SerializeField] private float RotationSpeed = 2f;

    [SerializeField] private GameObject recordPlate;

    [SerializeField] private float baseHandleYAngle = 0f;
    [SerializeField] private float startHandleYAngle = 45f;
    [SerializeField] private float finishHandleYAngle = 63.2f;

    [SerializeField] private GameObject handle;
    [SerializeField] private GameObject handleDynamicAttach;
    private float audioLengthTime;
    [SerializeField] private float prepareTime = 1f;
    [SerializeField] private float unprepareTime = 1f;
    private float curHandleTime;

    private bool isHandleReady = false;
    //=====================manual


    private Coroutine m_MoveDynamicHandleAttachOnHover=null;

    private Coroutine m_RotatePlate = null;

    private float m_localHandleEulerY;
    private float m_clipTimePercent;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    

    public void AutoPlayRecord()
    {
        if (handlePrepareCoroutine != null) StopCoroutine(handlePrepareCoroutine);
        handlePrepareCoroutine = StartCoroutine(MoveHandle(handle,transform.localEulerAngles.y, startHandleYAngle, prepareTime));

        if (afterHandlePrepareCoroutine != null) StopCoroutine(afterHandlePrepareCoroutine);
        afterHandlePrepareCoroutine = StartCoroutine(AutoAfterHadlePrepare());
        

    }
    IEnumerator AutoAfterHadlePrepare()
    {
        while(!isHandleReady)
        {
            
            yield return null;
        }
        
        base.PlayRecord();
        if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);
        rotationCoroutine = StartCoroutine(RotatePlate());

        if (handleMovingWhileAudioPlayingCoroutine != null) StopCoroutine(handleMovingWhileAudioPlayingCoroutine);

        audioLengthTime = audioClipFromRecord.length;
        //handle.transform.rotation = Quaternion.Euler(0, startHandleYAngle, 0);

        handleMovingWhileAudioPlayingCoroutine = StartCoroutine(MoveHandle(handle,startHandleYAngle, finishHandleYAngle, audioLengthTime));
    }
    public void AutoStopRecord()
    {
        base.StopRecord();
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }

        StopCoroutine(handleMovingWhileAudioPlayingCoroutine);
        if (afterHandlePrepareCoroutine != null) StopCoroutine(afterHandlePrepareCoroutine);
        if (handlePrepareCoroutine != null) StopCoroutine(handlePrepareCoroutine);
        
        handlePrepareCoroutine = StartCoroutine(MoveHandle(handle, handle.transform.localEulerAngles.y, baseHandleYAngle, unprepareTime));
        //handle.transform.localEulerAngles = new Vector3(0, baseHandleYAngle, 0);
    }
    public void StartRotatePlate()
    {
        if (m_RotatePlate == null) m_RotatePlate = StartCoroutine(RotatePlate());
    }
    public void StopRotatePlate()
    {
        if (m_RotatePlate != null)
        {
            StopCoroutine(m_RotatePlate);
            m_RotatePlate = null;
        }
    }
    IEnumerator RotatePlate()
    {
        while (true)
        {
            recordPlate.transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
            yield return null;
        }
    }


    IEnumerator MoveHandle(GameObject handle, float startYAngle, float endYAngle,float moveTime)
    {
        curHandleTime = 0f;
        isHandleReady = false;

        var startGlobalEulerY= startYAngle+ transform.eulerAngles.y;
        var endGlobalEulerY= endYAngle + transform.eulerAngles.y;
        var startGlobalVector = new Vector3(handle.transform.eulerAngles.x, startGlobalEulerY, handle.transform.eulerAngles.z);
        var endGlobalVector = new Vector3(handle.transform.eulerAngles.x, endGlobalEulerY, handle.transform.eulerAngles.z);
        while (curHandleTime < moveTime)
        {
           
            handle.transform.eulerAngles = Vector3.Slerp(startGlobalVector, endGlobalVector, curHandleTime / moveTime);
           
            curHandleTime += Time.deltaTime;

            yield return null;
        }
        handle.transform.eulerAngles = endGlobalVector;
        isHandleReady = true;

    }
    public void MoveHandleOnPlay()
    {
        if (handleMovingWhileAudioPlayingCoroutine == null)
        {
            if (m_localHandleEulerY < startHandleYAngle)
            {
                m_localHandleEulerY = startHandleYAngle;
            }
            handleMovingWhileAudioPlayingCoroutine = StartCoroutine(MoveHandle(handleDynamicAttach, m_localHandleEulerY, finishHandleYAngle, audioClipFromRecord.length * (1 - m_clipTimePercent)));
        }
    }
    public void StopHandle()
    {
        if (handleMovingWhileAudioPlayingCoroutine != null)
        {
            StopCoroutine(handleMovingWhileAudioPlayingCoroutine);
            handleMovingWhileAudioPlayingCoroutine = null;
        }
    }

    public void CalculateStartPlayParameters()
    {
        m_localHandleEulerY = (handle.transform.eulerAngles.y - transform.eulerAngles.y) < startHandleYAngle ? startHandleYAngle : (handle.transform.eulerAngles.y - transform.eulerAngles.y);
        m_clipTimePercent = Mathf.InverseLerp(startHandleYAngle, finishHandleYAngle, m_localHandleEulerY);
        
    }
    //====================manual

    public void StartMoveDynamicSocketAttachOnHover()
    {
       if(m_MoveDynamicHandleAttachOnHover==null) m_MoveDynamicHandleAttachOnHover = StartCoroutine(C_MoveDynamicSocketAttachOnHover());
        
    }
    public void StopMoveDynamicSocketAttachOnHover()
    {

        if (m_MoveDynamicHandleAttachOnHover != null)
        {
            StopCoroutine(m_MoveDynamicHandleAttachOnHover);

            m_MoveDynamicHandleAttachOnHover = null;
        }
    }
    private IEnumerator C_MoveDynamicSocketAttachOnHover()
    {
   
        while(true)
        {
            var localHandleEulerY = handle.transform.eulerAngles.y - transform.eulerAngles.y;
            if (localHandleEulerY >= startHandleYAngle && localHandleEulerY <= finishHandleYAngle)
            {
                var curRotation = handleDynamicAttach.transform.localEulerAngles;
                handleDynamicAttach.transform.localEulerAngles = new Vector3(curRotation.x, localHandleEulerY, curRotation.z);
            }
           
            yield return null;
        }
    }
    
    public void PlayRecordAtMoment()
    {
        
        audioClipFromRecord = recordInteractor.GetOldestInteractableSelected().transform.gameObject.GetComponent<AudioContainer>().AudioRecord;
        
        if (audioClipFromRecord != null)
        {
            audioSource.time = audioClipFromRecord.length * m_clipTimePercent< audioClipFromRecord.length ? audioClipFromRecord.length * m_clipTimePercent : audioClipFromRecord.length * m_clipTimePercent -0.001f;
            
            base.PlayRecord();
        }
    }
    public override void StopRecord()
    {
        base.StopRecord();

    }

    public void DisablePlateRecordCollider()
    {
        if (recordInteractor.GetOldestInteractableSelected() != null)
        {
            var collider = recordInteractor.GetOldestInteractableSelected().transform.gameObject.GetComponent<Collider>();
           
            if (collider != null) collider.enabled = false;
        }
    }
    public void EnablePlateRecordCollider()
    {
        if (recordInteractor.GetOldestInteractableSelected() != null)
        {
            var collider = recordInteractor.GetOldestInteractableSelected().transform.gameObject.GetComponent<Collider>();
            
            if (collider != null) collider.enabled = true;
        }

    }
    public void DisablePlateRecordSocket()
    {
        if (recordInteractor.GetOldestInteractableSelected() == null)
        {
            recordInteractor.enabled = false;
        }
    }

    public void EnablePlateRecordSocket()
    {

            recordInteractor.enabled = true;

    }
}
