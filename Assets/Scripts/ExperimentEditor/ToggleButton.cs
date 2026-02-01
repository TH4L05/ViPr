/// <author>Thomas Krahl</author>

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonLabel;
    [SerializeField] private bool useText;
    [SerializeField] private string button_state_closed;
    [SerializeField] private string button_state_open;
    [SerializeField] private bool useImage;
    [SerializeField] private Image toggleImage;
    [SerializeField] private Sprite toggleDefault;
    [SerializeField] private Sprite toggleToggled;

    private void OnEnable()
    {
        if (buttonLabel != null && useText) buttonLabel.text = button_state_closed;
        if (toggleImage != null && useImage) toggleImage.sprite = toggleDefault;
    }

    public void ToggleContent(bool active)
    {
        if(active)
        {
            if (useText) buttonLabel.text = button_state_open;
            if (useImage) toggleImage.sprite = toggleToggled;
        }
        else
        {
            if (useText) buttonLabel.text = button_state_closed;
            if (useImage) toggleImage.sprite = toggleDefault;
        }
    }
}
