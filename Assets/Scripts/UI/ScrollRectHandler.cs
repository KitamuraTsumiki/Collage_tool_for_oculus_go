using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectHandler : MonoBehaviour, IPointableUIElement
{
    [SerializeField]
	private ScrollRect scrollRect;

    private float verticalScroll;

	// initialize
	private void OnEnable(){
		//scrollRect.verticalNormalizedPosition = 1f;
	}

    private void GetVerticalScroll()
    {
        //verticalScroll = 
    }

    public void OnPointed() {
		// scroll the contents by the input from thumbstick / touchpad
		scrollRect.verticalNormalizedPosition += 0.05f * verticalScroll;
        /*
		if (_verticalScroll != 0f && !audio.isPlaying) {
			audio.Play ();
		} else if (_verticalScroll == 0f && audio.isPlaying) {
			audio.Stop ();
		}
        */
		// constrain the range of scroll
		if (scrollRect.verticalNormalizedPosition < 0f) {
			scrollRect.verticalNormalizedPosition = 0f;
		} else if (scrollRect.verticalNormalizedPosition > 1f) {
			scrollRect.verticalNormalizedPosition = 1f;
		}

	}

    public void OnReleased() { }
    public void OnClicked() { }
    public void OnPointerLeft() { }
}
