using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoWindow : MonoBehaviour
{
    [SerializeField] GameObject _window;
    [SerializeField] GameObject[] _mountSlot;
    [SerializeField] Text _name;
    [SerializeField] Text _lv;
    [SerializeField] Text _hp;
    [SerializeField] Text _mp;
    [SerializeField] Text _damage;
    [SerializeField] Text _defence;


    CharacterCtrl _character;
    KeyCode _hotKey;
    void Awake()
    {
        _window.SetActive(false);
    }
    private void OnEnable()
    {
        _character = GameManager._instance._character;
        UpdateCharacterInfo();
    }
    private void Start()
    {
        CharacterInfo();
        _hotKey = HotKeyManager._instance.OpenCharacterInfoKey;
        StartCoroutine(HotKeyManager._instance.OpenWindow(_window, _hotKey));
    }
    public void MountItem(int key)
    {
        if (DataTableManager._instance._itemDataDic.TryGetValue(key, out stDataTable.stItemData data))
        {
            _mountSlot[data._itemKind - 3].GetComponent<MountSlot>().SlotImage.sprite = ResoucePollManager._instance.GetItemImage(data._itemImage);
            if (!_mountSlot[data._itemKind - 3].GetComponent<MountSlot>()._isMounted)
            {
                _mountSlot[data._itemKind - 3].GetComponent<MountSlot>()._isMounted = true;
            }
            else
            {
                UIManager._instance._inventoryWindow.AddItem(_mountSlot[data._itemKind - 3].GetComponent<MountSlot>()._curItemkey);
            }
            _mountSlot[data._itemKind - 3].GetComponent<MountSlot>()._curItemkey = key;
            UserInfo._instance.MountItem(key);
            UpdateCharacterInfo();
        }
    }
    public void CharacterInfo()
    {
        _name.text = _character._name;
        UpdateCharacterInfo();
    }
    public void UpdateCharacterInfo()
    {
        _lv.text = string.Format("·¹º§ : {0}", _character.Level.ToString());
        _hp.text = string.Format("{0} / {1}", _character.CurHP, _character._maxHp);
        _mp.text = string.Format("{0} / {1}", _character.CurMP, _character._maxMp);
        _damage.text = _character.FinalDam.ToString();
        _defence.text = _character.FinalDef.ToString();
    }
}
