using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyAction { Inventory, CharacterInfo, Option, InterPlay, Slot_1, Slot_2, Slot_3, Slot_4, Skill_A, Skill_B, Skill_C, KeyCount }

public class HotKeyManager : MonoBehaviour
{
    static HotKeyManager _uniqueInstance;
    public static HotKeyManager _instance => _uniqueInstance;

    [SerializeField] KeyCode _openInventoryKey;
    [SerializeField] KeyCode _openCharacterInfoKey;
    [SerializeField] KeyCode _interPlayKey;
    [SerializeField] KeyCode _openOptionKey;

    public KeyCode OpenInventoryKey => _openInventoryKey;
    public KeyCode OpenCharacterInfoKey => _openCharacterInfoKey;
    public KeyCode OpenOptionKey => _openOptionKey;
    public KeyCode OpenInterPlayKey => _interPlayKey;
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
    public IEnumerator OpenWindow(GameObject obj, KeyCode key)
    {
        GameObject go = obj;
        while (true)
        {
            if (Input.GetKey(key))
            {
                if (!go.activeSelf)
                {
                    go.SetActive(true);
                }
                else
                {
                    CloseWindow(go);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    void CloseWindow(GameObject obj)
    {
        obj.SetActive(false);
    }
}
