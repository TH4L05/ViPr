/// <author>Thomas Krahl</author>

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject contentObject;
    [SerializeField] private Image toggleImage;
    [SerializeField] private Sprite untoggled;
    [SerializeField] private Sprite toggled;
    private bool isToggled;

    private void OnEnable()
    {
        if (toggleImage != null) toggleImage.sprite = untoggled;
        if(contentObject != null) contentObject.SetActive(false);
    }

    public void ToggleContent(bool active)
    {
        isToggled = active;
        if(isToggled)
        {
            toggleImage.sprite = toggled;
        }
        else
        {
            toggleImage.sprite = untoggled;
        }
        if (contentObject != null) contentObject.SetActive(isToggled);
    }

    public void ToggleContent()
    {
        isToggled = !isToggled;
        if (isToggled)
        {
            toggleImage.sprite = toggled;
        }
        else
        {
            toggleImage.sprite = untoggled;
        }
        if (contentObject != null) contentObject.SetActive(isToggled);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleContent();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
