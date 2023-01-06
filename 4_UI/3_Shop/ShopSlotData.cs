using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotData : Slot
{
    public Text _itemName;
    public Text _itemPrice;
    public Button _buyButton;
    public int _key;

    void Start()
    {
        TooltipUI();
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject == gameObject.gameObject || eventData.pointerCurrentRaycast.gameObject == _slotImage.gameObject)
        {
            _tooltipUI.Show();
            _tooltipUI.SetRectPosition(_slotImage.rectTransform);
            _tooltipUI.SetItemInfo(_key);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        _tooltipUI.Hide();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
