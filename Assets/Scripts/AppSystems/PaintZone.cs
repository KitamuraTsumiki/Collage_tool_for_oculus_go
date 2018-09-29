using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintZone : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        var paintable = other.GetComponent<Paintable>();
        if (paintable == null) { return; }

        paintable.PaintAlpha();
    }
}
