using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupPanel : MonoBehaviour
{
    [SerializeField] Image _itemImage;
    [SerializeField] Text _itemName;
    [SerializeField] InputField _txtitemAmount;
    [SerializeField] Text _itemTotalPrice;
        
    public Button _yesButton;

    public int _itemAmount = 1;
    stDataTable.stItemData _data;
    private void OnEnable()
    {
        _itemAmount = 1;
        _txtitemAmount.text = _itemAmount.ToString();
        _itemTotalPrice.text = (_itemAmount * _data._price).ToString() + " " + "Gold";
        _txtitemAmount.ActivateInputField();
    }
    public void ItemData(int key,stDataTable.stItemData data)
    {
        _data = data;
        if ((DefineEnumHelper.ItemType)data._itemType == DefineEnumHelper.ItemType.UsedItem)
        {
            _itemImage.sprite = ResoucePollManager._instance.GetItemImage(data._itemImage);
            _itemName.text = data._name;
        }
    }
    public void ClickPlusButton()
    {
        _txtitemAmount.text = _itemAmount.ToString();
        if (int.Parse(_txtitemAmount.text) >= 99)
        {
            return;
        }
        _itemAmount++;
        _txtitemAmount.text = _itemAmount.ToString();

    }
    public void ClickMinusButton()
    {
        _txtitemAmount.text = _itemAmount.ToString();
        if (int.Parse(_txtitemAmount.text)<= 1)
        {
            return;
        }
        _itemAmount--;
        _txtitemAmount.text = _itemAmount.ToString();
    }
    public void ChangeValue()
    {
        _itemAmount = int.Parse(_txtitemAmount.text);
        _itemTotalPrice.text = (_itemAmount * _data._price).ToString() + " " + "Gold";
    }
    public void CancleBuyItem()
    {
        _itemAmount = 1;
        _txtitemAmount.text = _itemAmount.ToString();
        gameObject.SetActive(false);
    }
}
