using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class ARGameManager : MonoBehaviour
{

    [SerializeField] private GameObject scanAnim;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private GameObject tapAnim;
    [SerializeField] private GameObject instructionPanel;

    private SurfaceDetector sd;
    private PlaceObjects po;
    private bool instructionClosed = false;

    private void Awake()
    {
        scanAnim.SetActive(false);
        instructionPanel.SetActive(false);
        sd = GetComponent<SurfaceDetector>();
        po = GetComponent<PlaceObjects>();
        StartCoroutine(GameProgress());
    }

    IEnumerator GameProgress()
    {
        infoText.text = "Scanning...";
        if (!scanAnim.activeSelf) scanAnim.SetActive(true);
        yield return new WaitUntil(() => sd.hitSurface);

        yield return new WaitForEndOfFrame();        
        infoText.text = "Surface Found !";
        if (scanAnim.activeSelf) scanAnim.SetActive(false);

        instructionPanel.SetActive(true);
        yield return new WaitUntil(() => instructionClosed);

        yield return new WaitForSeconds(2f);
        infoText.text = "Tap to place AR";
        if (!tapAnim.activeSelf) tapAnim.SetActive(true);

        yield return new WaitUntil(() => po.placePrefabList.Count > 0);
        if (tapAnim.activeSelf) tapAnim.SetActive(false);
        infoText.text = "Object placed";

        yield return new WaitForSeconds(2f);
        infoText.text = "";

    }

    public void CloseInstruction()
    {
        instructionClosed = true;
        if (instructionPanel.activeSelf) instructionPanel.SetActive(false);
    }
}
