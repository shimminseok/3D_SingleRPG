using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] NPC _owner;
    [SerializeField] PopupPanel _popUpPanel;
    [SerializeField] GameObject _slot;
    [SerializeField] Transform _root;
    [SerializeField] int _verticalSlotCount;
    [SerializeField] int _horizontalSlotCount;

    Inventory _inven;

    void OnEnable()
    {
        UIManager._instance._inventoryWindow.transform.GetChild(0).gameObject.SetActive(true);
    }
    void Start()
    {
        _inven = UIManager._instance._inventoryWindow;
        for (int n = 0; n < _verticalSlotCount * _horizontalSlotCount; n++)
        {
            GameObject go = Instantiate(_slot, _root);
            var shopslot = go.GetComponentInChildren<ShopSlotData>();
            Button b = go.GetComponentInChildren<Button>();
            int temp = n;
            if (DataTableManager._instance._itemDataDic.TryGetValue(n + 10, out stDataTable.stItemData data))
            {
                shopslot._key = n + 10;
                shopslot.SlotImage.sprite = ResoucePollManager._instance.GetItemImage(n);
                shopslot._itemName.text = data._name;
                shopslot._itemPrice.text = string.Format("{0} : {1}", "Gold", data._price);
                shopslot._buyButton.onClick.AddListener(() => BuyItem(temp + 10, data._price));
            }
        }
    }
    void BuyItem(int key, int price)
    {
        if (DataTableManager._instance._itemDataDic.TryGetValue(key, out stDataTable.stItemData data))
        {
            if ((DefineEnumHelper.ItemType)data._itemType == DefineEnumHelper.ItemType.UsedItem)
            {
                _popUpPanel.gameObject.SetActive(true);
                _popUpPanel.ItemData(key,data);
                _popUpPanel._yesButton.onClick.RemoveAllListeners();
                _popUpPanel._yesButton.onClick.AddListener(() => _inven.BuyItem(key, _popUpPanel._itemAmount));
            }
            else
            {
                _inven.BuyItem(key);
            }
        }
    }
    void OnDisable()
    {
        _owner._isInterPlay = false;
        _owner.GetComponent<Animator>().SetTrigger("ShopExit");
        UIManager._instance._inGameWindow.InterationNpc(true);
    }
}
