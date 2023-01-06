using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public abstract class Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected Image _slotImage;
    [SerializeField] protected Sprite _nullImage;
    [SerializeField] protected Text _amountTxt;

    protected Inventory _inventory;

    protected ItemTooltipUI _tooltipUI;
    public Sprite NullImage => _nullImage;
    public Image SlotImage => _slotImage;
    public void TooltipUI() => _tooltipUI = _slotImage.canvas.GetComponentInChildren<ItemTooltipUI>(true);
    public abstract void OnPointerClick(PointerEventData eventData);
    public abstract void OnPointerEnter(PointerEventData eventData);
    public abstract void OnPointerExit(PointerEventData eventData);
}
