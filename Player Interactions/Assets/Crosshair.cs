using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Set the position of the crosshair to the center of the screen
        rectTransform.position = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
    }
}
