using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class enables canvas groups to fade in / fade out
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class CanvasFade : MonoBehaviour {
    
    [SerializeField]
    private float fadeSpeed = 0.01f;
    private CanvasGroup canvas;

    private bool toFadeIn;
    private bool isActive;

    private void Start () {
        canvas = GetComponent<CanvasGroup>();
	}
	
	private void Update () {
        if (!isActive) { return; }

        FadeIn();
        FadeOut();
	}

    /// <summary>
    /// fade in / fade out functions are called by the UnityEvent
    /// when the relative canvases are activated / deactivated.
    /// </summary>
    public void Fade(bool _state)
    {
        toFadeIn = _state;
        isActive = true;
    }


    private void FadeIn()
    {
        if (!toFadeIn) { return; }
        //canvas.gameObject.SetActive(true);
        canvas.alpha = Mathf.Min(canvas.alpha + fadeSpeed, 1f);

        if (canvas.alpha < 1f) { return; }
        isActive = false;
    }

    private void FadeOut()
    {
        if (toFadeIn) { return; }
        canvas.alpha = Mathf.Max(canvas.alpha - fadeSpeed, 0f);

        if(canvas.alpha > 0f) { return; }
        //canvas.gameObject.SetActive(false);
        isActive = false;
        
    }
}
