using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using TMPro;

public class PlaceObjects : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;
    [HideInInspector]
    public List<GameObject> placePrefabList = new List<GameObject>();

    [SerializeField]
    private int maxPrefabSpawnCount = 0;
    private int placedPrefabCount = 0;

    [SerializeField]
    private GameObject PlaceablePrefab;
    private ARPlaneManager pm;
    private bool pmEnabled = false;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private ARProcessFlow processFlow;
    private int assetIndex;
    private Camera cam;

    //public TMP_Text debugText;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        pm = this.gameObject.GetComponent<ARPlaneManager>();
        processFlow = GetComponent<ARProcessFlow>();
        pm.enabled = true;
        cam = Camera.main;
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            Ray ray = cam.ScreenPointToRay(touchPosition);
            LayerMask mask = LayerMask.GetMask("Asset");

            if(Physics.Raycast(ray, Mathf.Infinity, mask))
            {
                return false;
            } else
            {
                return true;
            }           
        }

        touchPosition = default;
        return false;
    }
        
    void Update()
    {        
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if (raycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon)&& !IsPointerOverUIObject(touchPosition))
        {
            var hitPose = s_Hits[0].pose;
            if (placedPrefabCount < maxPrefabSpawnCount)
            {
                SpawnPrefab(hitPose);
                //EnablePrefabs(hitPose);
            } 
        }

        if (placedPrefabCount == maxPrefabSpawnCount && pmEnabled == false)
        {
            DisablePlane();
            pmEnabled = true;
        }

        //Debug
        //debugText.text = spawnedObject.GetComponent<LeanSelectable>().IsSelected.ToString();
    }

    bool IsPointerOverUIObject(Vector2 pos)
    {
        if (EventSystem.current == null)
            return false;
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(pos.x, pos.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void SpawnPrefab(Pose hitPose)
    {
        spawnedObject = Instantiate(PlaceablePrefab, hitPose.position + new Vector3(0, 0f, 0), Quaternion.identity);
        var rot = spawnedObject.transform.rotation;
        rot.eulerAngles = new Vector3(spawnedObject.transform.rotation.eulerAngles.x, 
                                        Camera.main.transform.rotation.eulerAngles.y,
                                        spawnedObject.transform.rotation.z);
        spawnedObject.transform.rotation = rot;
        placePrefabList.Add(spawnedObject);
        placedPrefabCount++;
    }

    private void EnablePrefabs(Pose hitPose)
    {
        if (PlaceablePrefab.activeSelf) return;
        PlaceablePrefab.SetActive(true);
        spawnedObject = PlaceablePrefab;
        spawnedObject.transform.position = hitPose.position + new Vector3(0, 0.1f, 0);
        spawnedObject.transform.rotation = Quaternion.identity;
        placePrefabList.Add(spawnedObject);
        placedPrefabCount++;
    }

    public void AssignObject(GameObject sign)
    {
        PlaceablePrefab = sign;
    }

    public void AssignIndex(int index)
    {
        assetIndex = index;
        processFlow.ChangeAssetLabel(assetIndex);
    }

    public void DisablePlane()
    {
        pm.enabled = false;
        foreach (var plane in pm.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }
}
