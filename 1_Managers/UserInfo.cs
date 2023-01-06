using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    static UserInfo _uniqueInstance;
    public static UserInfo _instance => _uniqueInstance;


    public GameSaveData _userSaveData { get; private set; } = new GameSaveData(); 
    public long _myUUid { get; private set; }
    public string _id { get; private set; }
    public string _pw { get; private set; }
    public string _nickName { get; private set; }
    public int _level { get; private set; }
    public float _curHP { get; private set; }
    public float _curMP { get; private set; }
    public int _money { get; private set; }
    public int _curEx { get; private set; }
    public int[] _itemdata { get; private set; }
    public int[] _mountItemData { get; private set; }
    //public Vector3 _curPos { get; private set; }

    public float _helmetDef { get; private set; }
    public float _armorDef { get; private set; }
    public float _weaponDam { get; private set; }
    public float _glovesDam { get; private set; }
    public float _bootsDef { get; private set; }

    void Awake()
    {
        if (_uniqueInstance == null)
        {
            _uniqueInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MountItem(int key)
    {
        if(DataTableManager._instance._itemDataDic.TryGetValue(key,out stDataTable.stItemData data))
        {
            switch((DefineEnumHelper.ItemKind)data._itemKind)
            {
                case DefineEnumHelper.ItemKind.HelMat:
                    _helmetDef = data._value;
                    break;
                case DefineEnumHelper.ItemKind.Armor:
                    _armorDef = data._value;
                    break;
                case DefineEnumHelper.ItemKind.Weapon:
                    _weaponDam = data._value;
                    break;
                case DefineEnumHelper.ItemKind.Glove:
                    _glovesDam = data._value;
                    break;
                case DefineEnumHelper.ItemKind.Boots:
                    _bootsDef = data._value;
                    break;
            }
            var character = GameManager._instance._character;
            character.MountItemDam((_weaponDam + _glovesDam));
            character.MountItemDef(_helmetDef + _armorDef + _bootsDef);

            character.FinalDamage(character._mountItemDam + character._buffDam);
            character.FinalDefence(character._mountItemDef);
            UIManager._instance._characterInfoWindow.UpdateCharacterInfo();
        }
    }
    public void DisMountItem(int key)
    {
        if (DataTableManager._instance._itemDataDic.TryGetValue(key, out stDataTable.stItemData data))
        {
            switch ((DefineEnumHelper.ItemKind)data._itemKind)
            {
                case DefineEnumHelper.ItemKind.HelMat:
                    _helmetDef =0;
                    break;
                case DefineEnumHelper.ItemKind.Armor:
                    _armorDef = 0;
                    break;
                case DefineEnumHelper.ItemKind.Weapon:
                    _weaponDam = 0;
                    break;
                case DefineEnumHelper.ItemKind.Glove:
                    _glovesDam = 0;
                    break;
                case DefineEnumHelper.ItemKind.Boots:
                    _bootsDef = 0;
                    break;
            }
            var character = GameManager._instance._character;
            character.MountItemDam((_weaponDam + _glovesDam));
            character.MountItemDef(_helmetDef + _armorDef + _bootsDef);

            character.FinalDamage(character._mountItemDam + character._buffDam);
            character.FinalDefence(character._mountItemDef);
            UIManager._instance._characterInfoWindow.UpdateCharacterInfo();
        }
    }

    public GameSaveData ToSave()
    {
        GameSaveData saveData = _userSaveData;
        return saveData;
    }
    public void LoadFrom(GameSaveData saveData)
    {
        _userSaveData = saveData;
    }

}
