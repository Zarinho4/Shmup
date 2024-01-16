using UnityEngine;
using UnityEngine.UI;

public class HoverEffect : MonoBehaviour
{
    // Reference to the button's Image component
    private Image buttonImage;

    // Colors for normal and hover states
    public Color normalColor = Color.white;
    public Color hoverColor = Color.grey;

    void Start()
    {
        // Get the Image component of the button
        buttonImage = GetComponent<Image>();

        // Set the initial color to normal color
        buttonImage.color = normalColor;
    }

    // Called when the mouse enters the button
    public void OnMouseEnter()
    {
        // Change the color to the hover color
        buttonImage.color = hoverColor;
    }

    // Called when the mouse exits the button
    public void OnMouseExit()
    {
        // Change the color back to normal color
        buttonImage.color = normalColor;
    }
}
