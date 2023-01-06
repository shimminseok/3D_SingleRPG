using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : NPC
{
    void Awake()
    {
        _txtName.text = _name;
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            _isNext = true;
        }
    }
}
