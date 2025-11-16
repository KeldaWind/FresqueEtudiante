using UnityEngine;
using UnityEngine.UI;

public class FrameComponent : MonoBehaviour
{
    public enum IllustrationDimenionsMode
    {
        Fit,
        Overflow,
        Stretch,
    }

    [Header("Static References")]
    public RectTransform selfRectTransform;
    public Image illustrationImage;
    public Mask illustrationImageMask;
    public Color placeholderColor = Color.magenta;

    [Header("Settings")]
    public FrameSettings frameSettings;
    public Sprite illustration;
    public IllustrationDimenionsMode illustrationDimenionsMode = IllustrationDimenionsMode.Fit;
    public Vector2 illustrationOffset;
    public bool flipOrientation;

    private Vector2 finalIllustrationMaskDimensions
    {
        get
        {
            if (frameSettings == null)
                return Vector2.one;

            if (flipOrientation)
                return new Vector2(frameSettings.illustrationMaskDimensions.y, frameSettings.illustrationMaskDimensions.x);

            return frameSettings.illustrationMaskDimensions;
        }
    }

    private float illustrationMaskRatio => illustrationImageMask.rectTransform.sizeDelta.x / illustrationImageMask.rectTransform.sizeDelta.y;
    private float illustrationRatio => (float)illustration.texture.width / illustration.texture.height;

    void OnValidate()
    {
        if (selfRectTransform == null)
            selfRectTransform = GetComponent<RectTransform>();

        UpdateIllustration();
        UpdateDimensions();
        UpdatePosition();
    }

    private void UpdateIllustration()
    {
        illustrationImage.sprite = illustration;
        illustrationImage.color = illustration != null ? Color.white : placeholderColor;
    }

    private void UpdateDimensions()
    {
        selfRectTransform.sizeDelta = finalIllustrationMaskDimensions;
        if (frameSettings != null)
            selfRectTransform.sizeDelta += frameSettings.borderDimensionsOffset * 2.0f;

        illustrationImageMask.rectTransform.sizeDelta = finalIllustrationMaskDimensions;

        if (illustration == null)
        {
            illustrationImage.rectTransform.sizeDelta = finalIllustrationMaskDimensions;
            return;
        }

        switch (illustrationDimenionsMode)
        {
            case IllustrationDimenionsMode.Fit:
                if (illustrationRatio > illustrationMaskRatio)
                {
                    var size = finalIllustrationMaskDimensions;
                    size.y *= illustrationMaskRatio / illustrationRatio;
                    illustrationImage.rectTransform.sizeDelta = size;
                }
                else if (illustrationRatio < illustrationMaskRatio)
                {
                    var size = finalIllustrationMaskDimensions;
                    size.x *= illustrationRatio / illustrationMaskRatio;
                    illustrationImage.rectTransform.sizeDelta = size;
                }
                break;

            case IllustrationDimenionsMode.Overflow:
                if (illustrationRatio > illustrationMaskRatio)
                {
                    var size = finalIllustrationMaskDimensions;
                    size.x *= illustrationRatio / illustrationMaskRatio;
                    illustrationImage.rectTransform.sizeDelta = size;
                }
                else if (illustrationRatio < illustrationMaskRatio)
                {
                    var size = finalIllustrationMaskDimensions;
                    size.y *= illustrationMaskRatio / illustrationRatio;
                    illustrationImage.rectTransform.sizeDelta = size;
                }
                break;

            case IllustrationDimenionsMode.Stretch:
                illustrationImage.rectTransform.sizeDelta = finalIllustrationMaskDimensions;
                break;
        }
    }

    private void UpdatePosition()
    {
        illustrationImage.rectTransform.anchoredPosition = illustrationOffset;
    }
}
