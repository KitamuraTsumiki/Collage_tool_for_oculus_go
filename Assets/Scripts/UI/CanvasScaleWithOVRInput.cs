using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScaleWithOVRInput : MonoBehaviour {

    public bool isActive { get; set; }
    private RectTransform rectTransform;
    private Vector2 lastTouchPadPos;

    public void SetInitTouchPadPos()
    {
        lastTouchPadPos = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update () {
        ScaleCanvas();
    }

    private void ScaleCanvas()
    {
        if (!isActive) { return; }
        Vector2 currTouchPadPos  = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
        float modifiedSizeRatio = 1f + (lastTouchPadPos.y - currTouchPadPos.y);
        rectTransform.sizeDelta = new Vector2(modifiedSizeRatio, modifiedSizeRatio);
    }
}
