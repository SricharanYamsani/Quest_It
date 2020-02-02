using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Transform spawn;

    public bool isTeamRed;

    public bool IsOccupied { get; set; } = false;

    public bool isPlayerSpot;
}
