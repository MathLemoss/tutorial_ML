using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ResultButtonSelection : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    public TMP_Text label;
    public string caption;

    void OnEnable()
    {
        Refresh(EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject);
    }

    public void OnSelect(BaseEventData eventData) => Refresh(true);
    public void OnDeselect(BaseEventData eventData) => Refresh(false);

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventSystem.current != null && GetComponent<Button>().IsInteractable())
            EventSystem.current.SetSelectedGameObject(gameObject);
    }

    void OnDisable() => Refresh(false);

    private void Refresh(bool selected)
    {
        label.text = selected ? "> " + caption : caption;
        label.color = selected ? new Color(0.025f, 0.035f, 0.08f) : Color.white;
        label.fontStyle = selected ? FontStyles.Bold : FontStyles.Normal;
    }
}
