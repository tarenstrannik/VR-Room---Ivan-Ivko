using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlateRecordPlayer : AudioSourcePlayer
{

    private Coroutine rotationCoroutine;

    [SerializeField] private float RotationSpeed = 2f;

    [SerializeField] private GameObject recordPlate;
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
        base.PlayRecord();
            if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);
            rotationCoroutine = StartCoroutine(RotatePlate());

    }
    public override void StopRecord()
    {
        base.StopRecord();
        StopCoroutine(rotationCoroutine);

    }

    IEnumerator RotatePlate()
    {
        while (true)
        {
            recordPlate.transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

}
