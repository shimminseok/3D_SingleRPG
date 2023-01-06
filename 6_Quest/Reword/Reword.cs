using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reword : ScriptableObject
{
    [SerializeField] Sprite _icon;
    [SerializeField] string _description;
    [SerializeField] int _quantity;


    public Sprite Icon => _icon;
    public string Description => _description;
    public int Quantity
    {
        get { return _quantity; }
        set { _quantity = value; }
    }
    public abstract void Give(Quest quest);

}
