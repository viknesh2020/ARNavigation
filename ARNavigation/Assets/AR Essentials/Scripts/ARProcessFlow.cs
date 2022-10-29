using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ARProcessFlow : MonoBehaviour
{

    // [SerializeField] private List<Image> assetButtons = new List<Image>(); //Can be used later for differentiating selected asset.
    [SerializeField] private List<string> assetLabels = new List<string>();

    [SerializeField] private TMP_Text infoText;
    [SerializeField] private GameObject scanAnim;
    [SerializeField] private GameObject tapAnim;

    [SerializeField] private RectTransform menuDrawer;
    [SerializeField] private GameObject drawerButton;

    private SurfaceDetector sd;
    private PlaceObjects po;
    private ImageRotateAroundAxis irAxis;

    private bool isDrawerMenuUp = false;
    private bool assetValidCount = false;

    private void Awake()
    {
        scanAnim.SetActive(false);
        tapAnim.SetActive(false);
        sd = this.gameObject.GetComponent<SurfaceDetector>();
        po = GetComponent<PlaceObjects>();
        irAxis = drawerButton.GetComponent<ImageRotateAroundAxis>();
        StartCoroutine(ARProcess());
    }

    public IEnumerator ARProcess()
    {
        infoText.text = "Scanning...";
        if (!scanAnim.activeSelf) scanAnim.SetActive(true);
        yield return new WaitUntil(() => sd.hitSurface);
        yield return new WaitForSeconds(2f);

        infoText.text = "Surface detected !";
        if (scanAnim.activeSelf) scanAnim.SetActive(false);
        yield return new WaitForSeconds(1f);

        infoText.text = "Select an asset ";
        irAxis.RotateAroundZAxis();
        ToggleAssetMenuDrawer();

        yield return new WaitUntil(() => assetValidCount);
        if (!tapAnim.activeSelf) tapAnim.SetActive(true);
        yield return new WaitUntil(() => po.placePrefabList.Count > 0);
        if (tapAnim.activeSelf) tapAnim.SetActive(false);
    }

    public void ToggleAssetMenuDrawer()
    {
        isDrawerMenuUp = !isDrawerMenuUp;
        if (isDrawerMenuUp)
        {
            LeanTween.move(menuDrawer, new Vector3(0, 152f, 0), 0.5f).setEase(LeanTweenType.linear);
        }
        else
        {
            LeanTween.move(menuDrawer, new Vector3(0, -152f, 0), 0.5f).setEase(LeanTweenType.linear);
        }
    }

    public void ChangeAssetLabel(int index)
    {       
        infoText.text = assetLabels[index] + " " + "selected";
        assetValidCount = true;
    }
}