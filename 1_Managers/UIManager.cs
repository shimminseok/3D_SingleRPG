using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager _uniqueInstance;
    public static UIManager _instance => _uniqueInstance;

    public InGameWindow _inGameWindow { get; private set; }
    public Inventory _inventoryWindow { get; private set; }
    public CharacterInfoWindow _characterInfoWindow { get; private set; }

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
        _inGameWindow = Instantiate(ResoucePollManager._instance.GetWindowObj(DefineEnumHelper.WindowObj.InGameWindow).GetComponent<InGameWindow>(), transform);
        _inventoryWindow = Instantiate(ResoucePollManager._instance.GetWindowObj(DefineEnumHelper.WindowObj.InventoryWindow).GetComponent<Inventory>(), transform);
        _characterInfoWindow = Instantiate(ResoucePollManager._instance.GetWindowObj(DefineEnumHelper.WindowObj.CharacterInfoWindow).GetComponent<CharacterInfoWindow>(), transform);
    }
    void Start()
    {
        
    }
}
