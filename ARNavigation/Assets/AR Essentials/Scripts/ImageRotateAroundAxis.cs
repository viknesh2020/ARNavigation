using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageRotateAroundAxis : MonoBehaviour
{
    private bool toggle =false;

    public void RotateAroundZAxis()
    {
        toggle = !toggle;

        if (toggle)
        {
            LeanTween.rotateAroundLocal(GetComponent<RectTransform>(), Vector3.forward, 180f, 0.5f).
                                        setEase(LeanTweenType.linear);
        } else
        {
            LeanTween.rotateAroundLocal(GetComponent<RectTransform>(), Vector3.forward, 180f, 0.5f).
                                        setEase(LeanTweenType.linear);
        }
    }

}
