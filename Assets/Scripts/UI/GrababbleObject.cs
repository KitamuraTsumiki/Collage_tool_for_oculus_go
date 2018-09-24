using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class manages transform modification of the grabbable objects
/// </summary>
[RequireComponent(typeof(Collider))]
public class GrababbleObject : MonoBehaviour, IPointableUIElement {

    public  bool        isGrabbed { get; private set; }
    public  Transform   controller; // given by Pointer
    private Vector3     relativePos;
    private Quaternion  relativeRot;

    private void Start () {
		
	}
	
	private void Update () {
        Move();

    }

    private void Move()
    {
        if (!isGrabbed || controller == null) { return; }

        transform.position = controller.TransformPoint(relativePos);
    }

    public void OnPointed()
    {

    }

    public void OnPointerLeft()
    {

    }

    public void OnReleased()
    {
        isGrabbed = false;
    }

    public void OnClicked()
    {
        isGrabbed = true;

        SetRelativeTransform();
    }
    
    private void SetRelativeTransform()
    {
        if(controller == null) { return; }

        relativePos = controller.InverseTransformPoint(transform.position);
        relativeRot = Quaternion.Inverse(transform.rotation) * controller.rotation;
    }
}
