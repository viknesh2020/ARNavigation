using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAlphaPingPong : MonoBehaviour
{
    private int tweenID;

    private void OnEnable()
    {
       tweenID = LeanTween.alpha(this.GetComponent<RectTransform>(), 0f, 1f)
                 .setEase(LeanTweenType.linear).setLoopPingPong().id;
    }

    private void OnDisable()
    {
        LeanTween.cancel(tweenID);
        var alpha = this.GetComponent<Image>().color;
        alpha.a = 1;
        this.GetComponent<Image>().color = alpha;
    }
}
