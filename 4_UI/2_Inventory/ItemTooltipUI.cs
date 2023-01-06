using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary> ���� ���� ������ �����ܿ� ���콺�� �÷��� �� ���̴� ���� </summary>
public class ItemTooltipUI : MonoBehaviour
{
    [SerializeField]
    Text _itemType; // ������ Ÿ��
    [SerializeField]
    private Text _titleText;   // ������ �̸� �ؽ�Ʈ
    [SerializeField]
    Text _contentText; // ������ ���� �ؽ�Ʈ
    [SerializeField]
    Text _statText; //���� ����
    RectTransform _rt;
    CanvasScaler _canvasScaler;

    private static readonly Vector2 LeftTop = new Vector2(0f, 1f);
    private static readonly Vector2 LeftBottom = new Vector2(0f, 0f);
    private static readonly Vector2 RightTop = new Vector2(1f, 1f);
    private static readonly Vector2 RightBottom = new Vector2(1f, 0f);
    void Awake()
    {
        Init();
    }
    void Start()
    {
        Hide();
    }
    void Init()
    {
        TryGetComponent(out _rt);
        _rt.pivot = LeftTop;
        _canvasScaler = GetComponentInParent<CanvasScaler>();

        DisableAllChildrenRaycastTarget(transform);
    }

    /// <summary> ��� �ڽ� UI�� ����ĳ��Ʈ Ÿ�� ���� </summary>
    void DisableAllChildrenRaycastTarget(Transform tr)
    {
        // ������ Graphic(UI)�� ����ϸ� ����ĳ��Ʈ Ÿ�� ����
        tr.TryGetComponent(out Graphic gr);
        if (gr != null)
            gr.raycastTarget = false;

        int childCount = tr.childCount;
        if (childCount == 0) return;

        for (int i = 0; i < childCount; i++)
        {
            DisableAllChildrenRaycastTarget(tr.GetChild(i));
        }
    }
    /// <summary> ���� UI�� ������ ���� ��� </summary>
    public void SetItemInfo(int index)
    {
        if (DataTableManager._instance._itemDataDic.TryGetValue(index, out stDataTable.stItemData itemData))
        {
            _itemType.text = ((DefineEnumHelper.ItemType)itemData._itemType).ToString();
            _titleText.text = itemData._name;
            _statText.text = string.Format("{0} : {1}", (DefineEnumHelper.ItemStat)itemData._status, itemData._value);
            _contentText.text = itemData._toolTip;
        }
    }
    /// <summary> ������ ��ġ ���� </summary>
    public void SetRectPosition(RectTransform slotRect)
    {
        // ĵ���� �����Ϸ��� ���� �ػ� ����
        float wRatio = Screen.width / _canvasScaler.referenceResolution.x;
        float hRatio = Screen.height / _canvasScaler.referenceResolution.y;
        float ratio =
            wRatio * (1f - _canvasScaler.matchWidthOrHeight) +
            hRatio * (_canvasScaler.matchWidthOrHeight);

        float slotWidth = slotRect.rect.width * ratio;
        float slotHeight = slotRect.rect.height * ratio;

        // ���� �ʱ� ��ġ(���� ���ϴ�) ����
        _rt.position = slotRect.position + new Vector3(slotWidth, -slotHeight);
        Vector2 pos = _rt.position;

        // ������ ũ��
        float width = _rt.rect.width * ratio;
        float height = _rt.rect.height * ratio;

        // ����, �ϴ��� �߷ȴ��� ����
        bool rightTruncated = pos.x + width > Screen.width;
        bool bottomTruncated = pos.y - height < 0f;

        ref bool R = ref rightTruncated;
        ref bool B = ref bottomTruncated;

        // �����ʸ� �߸� => ������ Left Bottom �������� ǥ��
        if (R && !B)
        {
            _rt.position = new Vector2(pos.x - width - slotWidth, pos.y);
        }
        // �Ʒ��ʸ� �߸� => ������ Right Top �������� ǥ��
        else if (!R && B)
        {
            _rt.position = new Vector2(pos.x, pos.y + height + slotHeight);
        }
        // ��� �߸� => ������ Left Top �������� ǥ��
        else if (R && B)
        {
            _rt.position = new Vector2(pos.x - width - slotWidth, pos.y + height + slotHeight);
        }
        // �߸��� ���� => ������ Right Bottom �������� ǥ��
        // Do Nothing
    }

    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);

}
