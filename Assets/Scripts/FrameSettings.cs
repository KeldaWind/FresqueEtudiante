using UnityEngine;

[CreateAssetMenu(menuName = "Fresque Etudiante/Frame Settings")]
public class FrameSettings : ScriptableObject
{
    public Vector2 illustrationMaskDimensions = new Vector2(10, 15);
    public Vector2 borderDimensionsOffset = Vector2.one;
}
