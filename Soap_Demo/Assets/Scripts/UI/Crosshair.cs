using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private List<CrosshairScriptableObject> m_Crosshairs;

    [SerializeField]
    private Vector2 m_Position;

    [SerializeField]
    private Vector3 m_Scale;

    private CrosshairScriptableObject m_ActiveCrosshairData;
    private Image m_CrosshairImage;

    // Start is called before the first frame update
    void Start()
    {
        GameObject crosshairCanvasObject = new GameObject("CrosshairCanvas");
        crosshairCanvasObject.AddComponent<CanvasScaler>();
        crosshairCanvasObject.AddComponent<GraphicRaycaster>();
        Canvas crosshairCanvas = crosshairCanvasObject.GetComponent<Canvas>();
        crosshairCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        GameObject crosshairPanel = new GameObject("CrosshairPanel");
        crosshairPanel.transform.SetParent(crosshairCanvasObject.transform, false);
        crosshairPanel.AddComponent<CanvasRenderer>();

        GameObject crosshairImageObject = new GameObject("CrosshairImage");
        crosshairImageObject.transform.SetParent(crosshairPanel.transform, false);
        m_CrosshairImage = crosshairImageObject.AddComponent<Image>();
        m_CrosshairImage.rectTransform.localPosition = m_Position;
        m_CrosshairImage.raycastTarget = false;
        SetCrosshair(CrosshairType.Default);
    }

    public void SetCrosshair(CrosshairType a_Type)
    {
        if (m_ActiveCrosshairData != null && m_ActiveCrosshairData.m_Type == a_Type)
            return;

        foreach (CrosshairScriptableObject crosshair in m_Crosshairs)
        {
            if (crosshair.m_Type == a_Type)
            {
                m_CrosshairImage.sprite = crosshair.m_Sprite;
                m_CrosshairImage.color = crosshair.m_Color;
                m_CrosshairImage.rectTransform.localScale = crosshair.m_Scale;
                m_ActiveCrosshairData = crosshair;
            }
        }
    }
}
