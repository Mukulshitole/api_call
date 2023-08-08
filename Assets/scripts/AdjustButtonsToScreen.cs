using UnityEngine;
using UnityEngine.UI;

public class AdjustButtonsToScreen : MonoBehaviour
{
    public Canvas canvas; // Reference to the Canvas component
    public float referenceAspectRatio = 1366f / 2488f; // Reference aspect ratio (width/height)

    private void Start()
    {
        AdjustButtonSizesAndPositions();
    }

    private void AdjustButtonSizesAndPositions()
    {
        float screenAspectRatio = (float)Screen.width / Screen.height;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        float aspectRatioMultiplier = screenAspectRatio / referenceAspectRatio;

        Button[] buttons = canvas.GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
            RectTransform buttonRect = button.GetComponent<RectTransform>();

            Vector2 originalSize = buttonRect.sizeDelta;
            buttonRect.sizeDelta = new Vector2(originalSize.x * aspectRatioMultiplier, originalSize.y);

            Vector2 originalPosition = buttonRect.anchoredPosition;
            buttonRect.anchoredPosition = new Vector2(originalPosition.x * aspectRatioMultiplier, originalPosition.y);
        }
    }
}
