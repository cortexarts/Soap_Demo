using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawCrosshair : MonoBehaviour
{
    public Sprite crosshair;
    public Rect position;
    public float heightScale;
    public float widthScale;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newCanvas = new GameObject("Canvas");
        Canvas c = newCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        newCanvas.AddComponent<CanvasScaler>();
        newCanvas.AddComponent<GraphicRaycaster>();
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Image i = panel.AddComponent<Image>();
        i.sprite = crosshair;

        i.transform.localScale = new Vector3(heightScale, 0.1f, widthScale);

        panel.transform.SetParent(newCanvas.transform, false);
    }
   
}
