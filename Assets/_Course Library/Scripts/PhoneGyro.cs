using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneGyro : MonoBehaviour
{
    [SerializeField] private GameObject videoScreen;

    [SerializeField] private float verticalPhoneXAngle = 0;

    [SerializeField] private float verticalScreenYAngle = 0;
    [SerializeField] private float verticalScreenXscale = 0;
    [SerializeField] private float verticalScreenZScale = 0;

    [SerializeField] private float horizontalScreenYAngle = 0;
    [SerializeField] private float horizontalScreenXscale = 0;
    [SerializeField] private float horizontalScreenZScale = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
       
        if(GetXDegrees(transform) >= verticalPhoneXAngle && GetXDegrees(transform)< verticalPhoneXAngle + 90)
        {

            videoScreen.transform.localScale=new Vector3(verticalScreenXscale, videoScreen.transform.localScale.y, verticalScreenZScale);
            videoScreen.transform.localEulerAngles = new Vector3(videoScreen.transform.localEulerAngles.x, verticalScreenYAngle, videoScreen.transform.localEulerAngles.z);
        }
        else if(GetXDegrees(transform) >= verticalPhoneXAngle + 90 && GetXDegrees(transform) < verticalPhoneXAngle + 180)
        {
            videoScreen.transform.localScale = new Vector3(horizontalScreenXscale, videoScreen.transform.localScale.y, horizontalScreenZScale);
            videoScreen.transform.localEulerAngles = new Vector3(videoScreen.transform.localEulerAngles.x, horizontalScreenYAngle, videoScreen.transform.localEulerAngles.z);

        }
        else if (GetXDegrees(transform) >= verticalPhoneXAngle + 180 && GetXDegrees(transform) < verticalPhoneXAngle + 270)
        {
            videoScreen.transform.localScale = new Vector3(verticalScreenXscale, videoScreen.transform.localScale.y, verticalScreenZScale);
            videoScreen.transform.localEulerAngles = new Vector3(videoScreen.transform.localEulerAngles.x, verticalScreenYAngle - 180, videoScreen.transform.localEulerAngles.z);

        }
        else if ((GetXDegrees(transform) >= verticalPhoneXAngle + 270 && GetXDegrees(transform) <= 360)|| (GetXDegrees(transform) >=0 && GetXDegrees(transform)< verticalPhoneXAngle))
        {
            
            videoScreen.transform.localScale = new Vector3(horizontalScreenXscale, videoScreen.transform.localScale.y, horizontalScreenZScale);
            videoScreen.transform.localEulerAngles = new Vector3(videoScreen.transform.localEulerAngles.x, horizontalScreenYAngle - 180, videoScreen.transform.localEulerAngles.z);

        }
    }

    

    float GetXDegrees(Transform t)
    {
        // Get the angle about the world x axis in range -pi to +pi,
        // with 0 corresponding to a 180 degree rotation.
        var radians = Mathf.Atan2(t.forward.y, -t.forward.z);

        
        return 180+radians * Mathf.Rad2Deg;
    }
}
