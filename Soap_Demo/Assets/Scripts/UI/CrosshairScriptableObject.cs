using UnityEngine;

public enum CrosshairType
{
    Default,
    Hover
}

[CreateAssetMenu(fileName = "Crosshair", menuName = "ScriptableObjects/Crosshair", order = 1)]
public class CrosshairScriptableObject : ScriptableObject
{
    public CrosshairType m_Type;
    public Sprite m_Sprite;
    public Color m_Color;
    public Vector2 m_Scale;
}
