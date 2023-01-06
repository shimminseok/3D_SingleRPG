using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ItemDropPr : MonoBehaviour, IDropHandler
{
    [Header("Parameta")]
    [SerializeField] Color _enterColor = Color.yellow;

    SlotData _slotData;
    Color _originColor;
    Sprite _originImage;
    public Sprite _slotImage
    {
        get { return _originImage; }
    }
    void Awake()
    {
        _slotData = GetComponent<SlotData>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        _originImage = _slotData.SlotImage.sprite;
        transform.GetChild(0).gameObject.SetActive(true);
        Image dropImage = eventData.pointerDrag.transform.GetChild(0).GetComponent<Image>();
        _slotData.SlotImage.sprite= dropImage.sprite;
        _slotData.SlotImage.color = Color.white;
    }
}
