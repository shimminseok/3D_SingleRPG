using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : SlotData
{
    public KeyCode _keycode;
    public Button _usedButton;
    public Text _keyCodeTxt;


    void Start()
    {
        _keyCodeTxt.text = _keycode.ToString();
        HideText();
    }
    void Update()
    {
        if (_slotData._key != 0 && Input.GetKeyDown(_keycode))
        {
            ButtonEvent(this);
        }
    }
    public void SetQuickSlot(int key)
    {
        if (DataTableManager._instance._itemDataDic.TryGetValue(key, out stDataTable.stItemData data))
        {
            if (!data._itemType.Equals((int)DefineEnumHelper.ItemType.UsedItem))
            {
                return;
            }
            _slotData._key = key;
            _slotData._amount = 9;
            _slotData._itemData = data;
            _slotImage.sprite = ResoucePollManager._instance.GetItemImage(_slotData._itemData._itemImage);
            _usedButton.onClick.AddListener(() => ButtonEvent(this));
            SetItemAmount(_slotData._amount);
        }
    }

    public void ButtonEvent(SlotData data)
    {
        Inventory inven = UIManager._instance._inventoryWindow;
        inven.UseItem(data);
    }
    
}
