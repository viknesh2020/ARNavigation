using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SurfaceDetector : MonoBehaviour
{
    //public TMP_Text bugText;
    [HideInInspector]
    public bool hitSurface = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray arRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit arHit;

        if (Physics.Raycast(arRay, out arHit, Mathf.Infinity))
        {
            //bugText.text = "Ray cast working!: " + arHit.collider.gameObject.GetComponent<ARPlane>();
            
            if (arHit.collider.gameObject.GetComponent<ARPlane>() != null && hitSurface == false)
            {
                //bugText.text = "Ray hit";
                hitSurface = true;                
            }
        }
    }
}