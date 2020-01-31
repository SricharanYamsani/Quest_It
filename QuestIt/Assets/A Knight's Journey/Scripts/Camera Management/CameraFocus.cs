using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFocus : MonoBehaviour
{
    public Vector3 offset = Vector3.zero;

    private Vector3 lastDirection = Vector3.zero;

    private Transform lookAtObject, prevLookAtObject = null;

    private void Awake()
    {
        BattleManager.Instance.TurnStart += GetLookAt;
    }

    private void GetLookAt(BattlePlayer player)
    {
        lookAtObject = player.LookAt;

        if (lookAtObject)
        {
            this.transform.DORotate(new Vector3(0,30,0), 0.5f);

            this.transform.DOMove(lookAtObject.localPosition - offset, 0.5f);
        }
    }

    private void OnDestroy()
    {
        BattleManager.Instance.TurnStart -= GetLookAt;
    }
}
