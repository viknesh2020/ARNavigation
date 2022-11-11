using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnim : MonoBehaviour
{
    [SerializeField] private float rotSpeed;
    [SerializeField] private GameObject particleEffect;
    [SerializeField] private AudioSource touchAudio;

    void Update()
    {
        transform.Rotate(Vector3.right * rotSpeed * Time.deltaTime);
    }
    private void OnEnable()
    {        
        particleEffect.SetActive(false);
        LeanTween.moveLocalX(this.gameObject, 0.05f, 1f).setLoopPingPong();
    }

    public void OnCoinSelected()
    {
        ARGameManager.Instance.UpdateScore();
        particleEffect.SetActive(true);
        if(!touchAudio.isPlaying) touchAudio.Play();
        Destroy(this.gameObject);
    }
}
