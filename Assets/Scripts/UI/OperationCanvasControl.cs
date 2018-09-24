using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// this class turns on / off the operation canvas 
/// when the player looks down
/// </summary>
[RequireComponent(typeof(CanvasFade))]
public class OperationCanvasControl : MonoBehaviour {

    [SerializeField]
    private Transform cameraTransform;
    private CanvasFade canvasFade;
    private bool lastPlayerLooksAtState = false;

	private void Start () {
        canvasFade = GetComponent<CanvasFade>();
        GetComponent<CanvasGroup>().alpha = 0f;
	}
	
	private void Update () {
        bool isLookedAt = WhenPlayerLookDown();
        if (isLookedAt == lastPlayerLooksAtState) { return; }
        canvasFade.Fade(isLookedAt);
        lastPlayerLooksAtState = isLookedAt;
	}

    private bool WhenPlayerLookDown()
    {
        Vector3 canvasDir = Vector3.Normalize(transform.position - cameraTransform.position);
        Vector3 viewDir = cameraTransform.forward;
        float lookAtThresh = 0.85f;

        return Vector3.Dot(canvasDir, viewDir) > lookAtThresh;
    }
}
