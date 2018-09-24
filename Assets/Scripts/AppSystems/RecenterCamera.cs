using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RecenterCamera : MonoBehaviour {

	public void Recenter()
    {
        InputTracking.Recenter();
    }
}
