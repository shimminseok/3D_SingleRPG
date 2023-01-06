using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionNPC : MonoBehaviour
{
    [SerializeField] KeyCode _keyCode;
    public Cinemachine.CinemachineVirtualCamera _cam2;
    NPC _npc;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            if (_npc == null)
            {
                UIManager._instance._inGameWindow.InterationNpc(true);
                _npc = other.GetComponentInParent<NPC>();
            }
            if (_npc._isInterPlay)
            {
                return;
            }
            else
            {
                _cam2.gameObject.SetActive(false);
            }
            if (Input.GetKeyDown(_keyCode))
            {
                UIManager._instance._inGameWindow.InterationNpc(false);
                transform.LookAt(_npc.transform);
                _cam2.gameObject.SetActive(true);
                _cam2.m_LookAt = _npc._cameraRoot.transform;
                _npc.transform.root.LookAt(transform.root);
                _npc._window.SetActive(true);
                _npc._window.transform.GetChild(0).gameObject.SetActive(true);
                _npc._isInterPlay = true;
                StartCoroutine(_npc.StartDialogue());
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (_npc == null)
        {
            return;
        }
        if (other.gameObject.CompareTag("NPC"))
        {
            UIManager._instance._inGameWindow.InterationNpc(false);
            _cam2.gameObject.SetActive(false);
            _npc.transform.LookAt(_npc.transform.forward);
            _npc._window.SetActive(false);
            _npc._isInterPlay = false;
            _npc = null;
        }
    }
}
