using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInfoUI : MonoBehaviour
{
    [SerializeField] Image _questinfoImage;
    [SerializeField] Sprite[] _questCompleteInfoImages;

    [SerializeField] QuestNPC _owner;


    void Update()
    {
        transform.LookAt(Camera.main.transform);
        if(_owner.QuestList.Count < 1)
        {
            _questinfoImage.gameObject.SetActive(false);
        }
        else if (_owner.CurQuset != null)
        {
            if(_owner.CurQuset.State.Equals(QuestState.WaitingForCompletion))
            {
                _questinfoImage.sprite = _questCompleteInfoImages[1];
                _questinfoImage.color = Color.yellow;
            }
            else
            {
                _questinfoImage.color = Color.cyan;
            }

        }
        else
        {
            _questinfoImage.gameObject.SetActive(true);
            _questinfoImage.sprite = _questCompleteInfoImages[0];
            _questinfoImage.color = Color.white;
        }

    }
}
