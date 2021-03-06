﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class defines the area of alpha map to paint
/// </summary>
[RequireComponent(typeof(Collider))]
public class PaintZone : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        var paintable = other.GetComponent<Paintable>();
        if (paintable == null) { return; }

        paintable.PaintAlpha();
    }
}
