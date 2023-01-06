using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragPr : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    SlotData _slotData;
    [SerializeField] Vector2 _dragOffset = Vector2.zero;


    Inventory _inventory;
    GameObject _dragginObject;
    RectTransform _canvasTf;
    Vector3 _originpos;

    void Start()
    {
        _inventory = UIManager._instance._inventoryWindow;
        _slotData = GetComponent<SlotData>();
    }
    void UpdateDragginObjectPos(PointerEventData eventData)
    {
        if (_slotData.SlotImage.sprite != _slotData.NullImage)
        {
            if (_dragginObject != null)
            {
                //드래그중 아이콘의 화면좌표계산
                Vector3 screenPos = eventData.position + _dragOffset;

                Vector3 newPos = Vector3.zero;
                Camera cam = eventData.pressEventCamera;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvasTf, screenPos, cam, out newPos))
                {
                    _dragginObject.transform.position = newPos;
                    _dragginObject.transform.rotation = _canvasTf.rotation;
                }
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_dragginObject != null)
        {
            Destroy(_dragginObject);
        }
        if (_slotData.SlotImage.sprite != _slotData.NullImage)
        {
            //드래그 중인 아이콘 생성
            _dragginObject = new GameObject("Dragging Object");

            // 드래그 아이콘을 해당 캔버스의 종속후 가장 마지막으로 옮겨서 가장 최상위에 그려지도록 한다.
            _dragginObject.transform.SetParent(_slotData.SlotImage.canvas.transform);
            _dragginObject.transform.SetAsLastSibling();
            _dragginObject.transform.localScale = Vector3.one;

            //블록 레이캐스트 속성을 블록되지 않게 한다.
            CanvasGroup canvasGrp = _dragginObject.AddComponent<CanvasGroup>();
            canvasGrp.blocksRaycasts = false;
            Image dragIcon = _dragginObject.AddComponent<Image>();
            dragIcon.enabled = true;
            dragIcon.sprite = _slotData.SlotImage.sprite;
            //dragIcon.rectTransform.sizeDelta = _slotData._slotImage.rectTransform.sizeDelta;
            dragIcon.rectTransform.sizeDelta = new Vector2(100, 100);
            dragIcon.color = Color.white;
            dragIcon.material = _slotData.SlotImage.material;

            _canvasTf = dragIcon.canvas.transform as RectTransform;
            UpdateDragginObjectPos(eventData);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_slotData.SlotImage.sprite != _slotData.NullImage)
        {
            _slotData.HideText();
            _originpos = transform.position;
            UpdateDragginObjectPos(eventData);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        try
        {
            SlotData slot = eventData.pointerEnter.gameObject.GetComponentInParent<SlotData>();
            ItemDropPr drop = eventData.pointerEnter.gameObject.GetComponentInParent<ItemDropPr>();
            _slotData.SlotImage.gameObject.SetActive(true);
            _slotData.SlotImage.sprite = drop._slotImage;
            _inventory.SwapItem(_slotData, slot);

        }
        catch
        {
            _inventory.DropItem(_slotData);

        }
        finally
        {
            _slotData.SetItemAmount(_slotData._slotData._amount);
            Destroy(_dragginObject);
        }
    }
}

