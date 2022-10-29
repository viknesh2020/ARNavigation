using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class SceneLoaderImageTracker : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager imageManager;
    [SerializeField] private Canvas loadingScreen;
    [SerializeField] private Canvas scanQR;
    [SerializeField] private Image fillImage;

    private void Awake()
    {
        scanQR.enabled = true;
        loadingScreen.enabled = false;
    }

    private void Start()
    {
       // StartCoroutine(OnImageDetectionLoadScene()); //For debugging
    }

    private void OnEnable()
    {
        imageManager.trackedImagesChanged += OnImageUpdate;  
    }

    private void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnImageUpdate;
    }

    private void OnImageUpdate(ARTrackedImagesChangedEventArgs obj)
    {
        foreach(var image in obj.added)
        {
            StartCoroutine(OnImageDetectionLoadScene());
        }

        foreach(var image in obj.updated)
        {
            //Not necessary for now
        }

        foreach(var image in obj.removed)
        {
            //Not necessary for now
        }
    }

    IEnumerator OnImageDetectionLoadScene()
    {
        scanQR.enabled = false;
        yield return new WaitForSeconds(1f);
        loadingScreen.enabled = true;
        yield return new WaitForEndOfFrame();

        LeanTween.value(0f, 1f, 4f).setEase(LeanTweenType.linear).setOnUpdate((float val) =>

        fillImage.fillAmount = val

        ).setOnComplete(() => SceneManager.LoadScene(2));
       
    }

}