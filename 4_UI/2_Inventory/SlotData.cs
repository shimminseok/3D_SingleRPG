using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public struct stSlotData
{
    public int _key;
    public stDataTable.stItemData _itemData;
    public int _amount;

    public stSlotData(int key = 0, stDataTable.stItemData data = new stDataTable.stItemData(), int amount = 0)
    {
        _key = key;
        _itemData = data;
        _amount = amount;
    }
}
public class SlotData : Slot
{
    public stSlotData _slotData = new stSlotData();
    public int _index { get; private set; }
    public void SetSlotIndex(int index) => _index = index;
    public int _maxAmount { get; private set; } = 99;

    ItemDragPr _itemDrag;
    ItemDropPr _itemDrop;
    private void Awake()
    {
        TooltipUI();
    }
    void Start()
    {
        SetItemAmount(0);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (_slotData._key != 0)
        {
            _slotImage.color = Color.yellow;
            _tooltipUI.Show();
            _tooltipUI.SetRectPosition(_slotImage.rectTransform);
            _tooltipUI.SetItemInfo(_slotData._key);
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        _slotImage.color = Color.white;
        _tooltipUI.Hide();
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button.Equals(PointerEventData.InputButton.Right))
        {
            _inventory = UIManager._instance._inventoryWindow;
            _inventory.UseItem(this);
        }
    }
    public void SetItemAmount(int amount)
    {
        _slotData._amount = amount;
        if (amount > 1)
            ShowText();
        else
            HideText();
        _amountTxt.text = _slotData._amount.ToString();
    }
    public void ShowText() => _amountTxt.gameObject.SetActive(true);
    public void HideText() => _amountTxt.gameObject.SetActive(false);

}
