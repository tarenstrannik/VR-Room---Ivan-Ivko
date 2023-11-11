using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class PlateRecordPlayer : AudioPlayer
{

    private Coroutine rotationCoroutine;
    private Coroutine handleAudioCoroutine;
    private Coroutine handlePrepareCoroutine;
    private Coroutine afterHandlePrepareCoroutine;

    [SerializeField] private float RotationSpeed = 2f;

    [SerializeField] private GameObject recordPlate;

    [SerializeField] private float baseHandleYAngle = 0f;
    [SerializeField] private float startHandleYAngle = 45f;
    [SerializeField] private float finishHandleYAngle = 63.2f;

    [SerializeField] private GameObject handle;
    private float audioLengthTime;
    [SerializeField] private float prepareTime = 1f;
    [SerializeField] private float unprepareTime = 1f;
    private float curHandleTime;

    private bool isHandleReady = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public override void PlayRecord()
    {
        if (handlePrepareCoroutine != null) StopCoroutine(handlePrepareCoroutine);
        handlePrepareCoroutine = StartCoroutine(MoveHandle(handle.transform.localEulerAngles.y, startHandleYAngle, prepareTime));

        if (afterHandlePrepareCoroutine != null) StopCoroutine(afterHandlePrepareCoroutine);
        afterHandlePrepareCoroutine = StartCoroutine(AfterHadlePrepare());
        

    }
    IEnumerator AfterHadlePrepare()
    {
        while(!isHandleReady)
        {
            
            yield return null;
        }
        
        base.PlayRecord();
        if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);
        rotationCoroutine = StartCoroutine(RotatePlate());

        if (handleAudioCoroutine != null) StopCoroutine(handleAudioCoroutine);

        audioLengthTime = audioClip.length;
        //handle.transform.rotation = Quaternion.Euler(0, startHandleYAngle, 0);

        handleAudioCoroutine = StartCoroutine(MoveHandle(startHandleYAngle, finishHandleYAngle, audioLengthTime));
    }
    public override void StopRecord()
    {
        base.StopRecord();
        StopCoroutine(rotationCoroutine);

        StopCoroutine(handleAudioCoroutine);
        if (afterHandlePrepareCoroutine != null) StopCoroutine(afterHandlePrepareCoroutine);
        if (handlePrepareCoroutine != null) StopCoroutine(handlePrepareCoroutine);
        
        handlePrepareCoroutine = StartCoroutine(MoveHandle(handle.transform.localEulerAngles.y, baseHandleYAngle, unprepareTime));
        //handle.transform.localEulerAngles = new Vector3(0, baseHandleYAngle, 0);
    }

    IEnumerator RotatePlate()
    {
        while (true)
        {
            recordPlate.transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
            yield return null;
        }
    }


    IEnumerator MoveHandle(float startYAngle, float endYAngle,float moveTime)
    {
        curHandleTime = 0f;
        isHandleReady = false;
        while (curHandleTime < moveTime)
        {
            handle.transform.localEulerAngles = Vector3.Slerp(new Vector3(0, startYAngle, 0), new Vector3(0, endYAngle, 0), curHandleTime / moveTime);
            curHandleTime += Time.deltaTime;

            yield return null;
        }
        handle.transform.localEulerAngles = new Vector3(0, endYAngle, 0);
        isHandleReady = true;

    }

}
