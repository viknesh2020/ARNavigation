using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject closePanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject instrPanel;

    private bool instWasOpen = false;

    private void Start()
    {
        closePanel.SetActive(false);
        instrPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void CloseButton()
    {
        if (mainPanel.activeSelf)
        {
            mainPanel.SetActive(false);            
            closePanel.SetActive(true);
            if (instrPanel.activeSelf && !instWasOpen) {
                instrPanel.SetActive(false);
                instWasOpen = true;
            }            
        }
    }

    public void CloseNoButton()
    {
        if (closePanel.activeSelf) closePanel.SetActive(false);
        mainPanel.SetActive(true);
        if(instWasOpen)
        {
            instrPanel.SetActive(true);
            instWasOpen = false;
        }
    }

    public void LoadARScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
