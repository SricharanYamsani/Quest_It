using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFocus : MonoBehaviour
{
    public Vector3 offset = Vector3.zero;

    private Vector3 lastDirection = Vector3.zero;

    private Transform lookAtObject = null;

    private bool isMoving = false;

    private float journeyLength, distanceCovered, startTime;

    public float speed = 1.5f;

    private void Awake()
    {
        BattleManager.Instance.TurnStart += GetLookAt;
    }

    private void GetLookAt(BattlePlayer player)
    {
        lookAtObject = player.LookAt;

        lastDirection = transform.position;

        if (lastDirection != lookAtObject.position)
        {
            startTime = Time.time;

            journeyLength = Vector3.Distance(lastDirection, lookAtObject.position);

            isMoving = true;
        }
    }
    private void LateUpdate()
    {
        if (isMoving)
        {
            distanceCovered = (Time.time - startTime) * speed;

            float fracJourney = distanceCovered / journeyLength;

            float singleStep = speed * Time.deltaTime;

            transform.position = Vector3.Lerp(lastDirection, offset.magnitude * lookAtObject.position, fracJourney);

            Vector3 newDirection = Vector3.RotateTowards(lastDirection, lookAtObject.position, singleStep, 0.0f);

            if (newDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(-1 * newDirection);
            }
        }
    }
}
