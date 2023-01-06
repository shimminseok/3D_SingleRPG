using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MountSlot : Slot
{
    public bool _isMounted = false;
    public int _curItemkey = 0;

    void Awake()
    {
        TooltipUI();
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button.Equals(PointerEventData.InputButton.Right))
        {
            _inventory  = UIManager._instance._inventoryWindow;
            _inventory.AddItem(_curItemkey);
            UserInfo._instance.DisMountItem(_curItemkey);
            _isMounted = false;
            _curItemkey = 0;
            _slotImage.sprite = _nullImage;
        }

    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (_curItemkey != 0)
        {
            _tooltipUI.SetRectPosition(_slotImage.rectTransform);
            _tooltipUI.SetItemInfo(_curItemkey);
            _tooltipUI.Show();
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        _tooltipUI.Hide();
    }
}
