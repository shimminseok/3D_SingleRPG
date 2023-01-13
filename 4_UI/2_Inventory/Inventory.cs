using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject _slot;
    [SerializeField] Transform _root;
    [SerializeField] ItemInfoPanel _itemInfoPanel;
    [SerializeField] Text _moneyText;

    [Range(0, 10)] [SerializeField] int _horizontalSlotCount = 7;
    [Range(0, 10)] [SerializeField] int _verticalSlotCount = 8;

    public List<SlotData> _slotList { get; private set; } = new List<SlotData>();
    List<int[]> _item = new List<int[]>();

    IEnumerator _coroutine;
    KeyCode _hotKey;

    public KeyCode HotKey
    {
        get { return _hotKey; }
        set
        {
            _hotKey = value;
            StopCoroutine(_coroutine);
            _coroutine = HotKeyManager._instance.OpenWindow(transform.GetChild(0).gameObject, _hotKey);
            StartCoroutine(_coroutine);
        }
    }
    public string MoneyText
    {
        get { return _moneyText.text; }
        set
        {
            _moneyText.text = value;
        }
    }
    void Awake()
    {
        CreateSlot();
        transform.GetChild(0).gameObject.SetActive(false);
    }
    void Start()
    {
        _hotKey = HotKeyManager._instance.OpenInventoryKey;
        _coroutine = HotKeyManager._instance.OpenWindow(transform.GetChild(0).gameObject, _hotKey);
        StartCoroutine(_coroutine);
        _moneyText.text = GameManager._instance._character.CurMoney.ToString();
    }
    void CreateSlot()
    {
        for (int n = 0; n < _verticalSlotCount; n++)
        {
            for (int m = 0; m < _horizontalSlotCount; m++)
            {
                int slotIndex = (_horizontalSlotCount * n) + m;
                GameObject go = Instantiate(_slot, _root);
                var itemslot = go.GetComponentInChildren<SlotData>();
                _slotList.Add(itemslot);
            }
        }
    }
    public void AddItem(int key, int amount = 1)
    {
        if (DataTableManager._instance._itemDataDic.TryGetValue(key, out stDataTable.stItemData data))
        {
            for (int n = 0; n < _slotList.Count; n++)
            {
                if (!_slotList[n]._slotData._itemData._itemType.Equals((int)DefineEnumHelper.ItemType.MountedItem)
                    && _slotList[n]._slotData._amount < _slotList[n]._maxAmount && key.Equals(_slotList[n]._slotData._key))
                {
                    UpdataSlotData(n, key, data, amount);
                    if (_slotList[n]._slotData._itemData._itemType.Equals((int)DefineEnumHelper.ItemType.OtherItem))
                    {
                        QuestManager.Instance.ReceiveReport("GetItem", key, amount);
                    }
                    return;
                }
            }
            for (int n = 0; n < _slotList.Count; n++)
            {
                if (_slotList[n]._slotData._key == 0)
                {
                    UpdataSlotData(n, key, data, amount);
                    if (_slotList[n]._slotData._itemData._itemType.Equals((int)DefineEnumHelper.ItemType.OtherItem))
                    {
                        QuestManager.Instance.ReceiveReport("GetItem", key, amount);
                    }
                    break;
                }
            }
        }
    }
    void UpdataSlotData(int index, int key, stDataTable.stItemData data, int amount)
    {
        _slotList[index].SlotImage.sprite = ResoucePollManager._instance.GetItemImage(data._itemImage);
        if (data._itemType.Equals((int)DefineEnumHelper.ItemType.OtherItem))
        {
            _slotList[index].SlotImage.sprite = ResoucePollManager._instance.GetQuestItemImage(data._itemImage);
        }
        _slotList[index]._slotData._key = key;
        _slotList[index]._slotData._itemData = data;
        _slotList[index]._slotData._amount += amount;
        _slotList[index].SetItemAmount(_slotList[index]._slotData._amount);
    }
    public void BuyItem(int key, int amount = 1)
    {
        if (DataTableManager._instance._itemDataDic.TryGetValue(key, out stDataTable.stItemData data))
        {
            int totalGold = data._price * amount;
            if (GameManager._instance._character.CurMoney < totalGold)
            {
                UIManager._instance._inGameWindow.Message("소지골드가 부족합니다.");
                return;
            }
            GameManager._instance._character.CurMoney -= totalGold;
        }
        AddItem(key, amount);
    }

    public void SwapItem(SlotData originSlot, SlotData swapSlot)
    {
        stSlotData temp = swapSlot._slotData;
        swapSlot._slotData = originSlot._slotData;
        originSlot._slotData = temp;
        swapSlot.SetItemAmount(swapSlot._slotData._amount);
        originSlot.SetItemAmount(originSlot._slotData._amount);
    }
    public void RemoveItem(SlotData data)
    {
        data.SlotImage.sprite = data.NullImage;
        data._slotData._key = 0;
        data._slotData._itemData = default;
        data.SetItemAmount(0);
    }
    public void DropItem(SlotData data)
    {
        _itemInfoPanel.gameObject.SetActive(true);
        _itemInfoPanel._itemName.text = data._slotData._itemData._name;

        _itemInfoPanel._yesButton.onClick.RemoveAllListeners();
        _itemInfoPanel._yesButton.onClick.AddListener(() => RemoveItem(data));
    }
    public void UseItem(SlotData data)
    {
        CharacterCtrl character = GameManager._instance._character;
        float value = 0;
        Color color = Color.white;
        switch ((DefineEnumHelper.ItemType)data._slotData._itemData._itemType)
        {
            case DefineEnumHelper.ItemType.UsedItem:
                if (data._slotData._amount > 0)
                {
                    switch ((DefineEnumHelper.ItemKind)data._slotData._itemData._itemKind)
                    {
                        case DefineEnumHelper.ItemKind.HPPortion:
                            if (character.CurHP >= character._maxHp)
                            {
                                UIManager._instance._inGameWindow.Message("체력이 가득 찼습니다.");
                                return;
                            }
                            character.CurHP += Mathf.Round(data._slotData._itemData._value * character._maxHp);
                            Debug.Log(Mathf.Round(data._slotData._itemData._value * character._maxHp));
                            value = character.CurHP > character._maxHp ? character.CurHP - character._maxHp : (data._slotData._itemData._value * character._maxHp);
                            character.CurHP = character.ClampValue(character.CurHP, character._maxHp);
                            color = Color.green;
                            break;
                        case DefineEnumHelper.ItemKind.MPPortion:
                            if (character.CurMP >= character._maxMp)
                            {
                                UIManager._instance._inGameWindow.Message("마나가 가득 찼습니다.");
                                return;
                            }
                            character.CurMP += Mathf.Round(data._slotData._itemData._value * character._maxMp);
                            value = character.CurMP > character._maxMp ? character.CurMP - character._maxMp : (data._slotData._itemData._value * character._maxMp);
                            character.CurMP = character.ClampValue(character.CurMP, character._maxMp);
                            color = Color.cyan;
                            break;
                    }
                    data._slotData._amount -= 1;
                    data.SetItemAmount(data._slotData._amount);
                    character.GetComponent<HPScript>().ChangeHP(Mathf.Round(value), character.transform.position, color);
                }
                break;
            case DefineEnumHelper.ItemType.MountedItem:
                UIManager._instance._characterInfoWindow.MountItem(data._slotData._key);
                data._slotData._amount = 0;
                break;
        }
        if (data._slotData._amount < 1)
            RemoveItem(data);
    }
    public int CheakQuestItem(TaskTarget target)
    {
        for (int n = 0; n < _slotList.Count; n++)
        {
            if(int.Parse(target.Value.ToString()) == _slotList[n]._slotData._key)
            {
                return _slotList[n]._slotData._amount;
            }
        }
        return 0;
    }
}
