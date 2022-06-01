using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorStatus : MonoBehaviour
{
    public bool DoorOpen;
    public bool DoorClose;

    private void Start()
    {
        DoorOpen = false;
        DoorClose = true;
    }
}
