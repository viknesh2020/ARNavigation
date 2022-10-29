using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private GameObject go;
    private RaycastHit hit;
    private float timer = 120;
    [SerializeField] private TMP_Text timerText;

    private void Update()
    {
        timer -= Time.deltaTime;
        //Debug.Log("Time: " +timer);
        timerText.text = "Time: " + Mathf.FloorToInt(timer).ToString("");

        if (!Input.GetMouseButtonDown(0)) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (go != null) return;
            go = Instantiate(prefab, hit.point, Quaternion.identity);
            var rot = go.transform.rotation;
            rot.eulerAngles = new Vector3(go.transform.rotation.eulerAngles.x,
                                    Camera.main.transform.rotation.eulerAngles.y, go.transform.rotation.eulerAngles.z);
            go.transform.rotation = rot;
            Debug.Log("Camera Rot Y: " + Camera.main.transform.rotation.eulerAngles.y);
            //CalculateRotation();
        }
    }

    void CalculateRotation()
    {
        float angle = Vector3.Angle(Camera.main.transform.forward, go.transform.forward);
        Debug.Log("Angle to rotate: " + angle);
        go.transform.Rotate(go.transform.up, angle);
    }
}
