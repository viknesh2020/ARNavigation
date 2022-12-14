using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class ARGameManager : MonoBehaviourSingleton<ARGameManager>
{
    [HideInInspector] public static int score;

    [SerializeField] private GameObject scanAnim;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private GameObject tapAnim;
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float countDownTimer;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private GameObject endPopup;
    [SerializeField] private TMP_Text greetText;
    [SerializeField] private AudioSource bgAudio;
    [SerializeField] private GameObject closeButton;

    private SurfaceDetector sd;
    private PlaceObjects po;
    private bool instructionClosed = false;
    private bool timerOn = false;

    private void Awake()
    {
        scanAnim.SetActive(false);
        endPopup.SetActive(false);
        scoreText.enabled = false;
        timer.enabled = false;
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

        yield return new WaitForSeconds(1f);
        infoText.text = "Tap to place AR";
        if (!tapAnim.activeSelf) tapAnim.SetActive(true);

        yield return new WaitUntil(() => po.placePrefabList.Count > 0);
        if (tapAnim.activeSelf) tapAnim.SetActive(false);
        infoText.text = "Object placed";

        yield return new WaitForSeconds(2f);
        infoText.text = "";
        bgAudio.Play();
        scoreText.enabled = true;
        score = 0;
        scoreText.text = "SCORE: " + score.ToString();
        timer.enabled = true;
        timerOn = true;              
        yield return new WaitUntil(() => !timerOn || score == 50);

        if(!timerOn)
        {
            endPopup.SetActive(true);
            greetText.text = "Better luck next time !";
            if (closeButton.activeSelf) closeButton.SetActive(false);
        } else if(score == 50)
        {
            endPopup.SetActive(true);
            greetText.text = "Congratulations ! You WON !";
            timerOn = false;
            if (closeButton.activeSelf) closeButton.SetActive(false);
        }  

    }

    private void Update()
    {
        if (timerOn)
        {
            if (countDownTimer > 0)
            {
                countDownTimer -= Time.deltaTime;
                timer.text = "TIMER: " + Mathf.FloorToInt(countDownTimer).ToString();
            } else
            {
                countDownTimer = 0;
                timer.text = "TIMER: " + Mathf.FloorToInt(countDownTimer).ToString();
                timerOn = false;
            }                      
        }
    }

    public void CloseInstruction()
    {
        instructionClosed = true;
        if (instructionPanel.activeSelf) instructionPanel.SetActive(false);
    }

    public void UpdateScore()
    {
        score += 5;
        scoreText.text = "SCORE: " +score.ToString(); 
    }
}
