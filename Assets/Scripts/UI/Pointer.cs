using UnityEngine;
/// <summary>
/// this class enables hand controller object to cast a ray to point ui elements
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class Pointer : MonoBehaviour {

    private LineRenderer pointerLine;
    [SerializeField]
    private GameObject point;
    [SerializeField]
    private Transform viewPoint;

    private IPointableUIElement lastPointedElement;

    private Vector2 lastTouchPoint;

	private void Start () {
        pointerLine = GetComponent<LineRenderer>();
	}
	
	private void Update () {
        
        CastPointer();
	}

    private void DisplayPointerLine(float lineLength = 5f)
    {
        Vector3 endPos = transform.position + transform.forward * lineLength;
        Vector3[] points = { transform.position, endPos };
        pointerLine.SetPositions(points);
    }

    private void DisplayPoint(bool activated, Vector3 pos)
    {
        if(point == null) { return; }

        point.SetActive(activated);
        point.transform.position = pos;
        point.transform.LookAt(viewPoint);
        point.transform.Rotate(new Vector3(0f, 180f, 0f));
    }

    private RaycastHit FindNearestHit(RaycastHit[] hits)
    {
        if(hits.Length == 1) { return hits[0]; }

        RaycastHit nearestHit = hits[0];
        for (int i = 1; i < hits.Length; i++)
        {
            if (hits[i].distance > hits[i-1].distance) { continue; }
            nearestHit = hits[i];
        }

        return nearestHit;
    }

    private IPointableUIElement FindPointables(RaycastHit[] hits)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            IPointableUIElement pointable = hits[i].collider.GetComponent<IPointableUIElement>();
            if(pointable != null) { return pointable; }
        }

        return null;
    }

    private void CastPointer() {
        RaycastHit[]  hits = Physics.RaycastAll(transform.position, transform.forward);
        if(hits.Length < 1) {
            DisplayPointerLine();
            Vector3 tempPointPos = Vector3.zero;
            DisplayPoint(false, tempPointPos);

            if (lastPointedElement == null) { return; }
            lastPointedElement.OnPointerLeft();
            lastPointedElement = null;
            return;
        }
        RaycastHit nearestHit = FindNearestHit(hits);
        DisplayPointerLine(nearestHit.distance);
        DisplayPoint(true, nearestHit.point);
        
        IPointableUIElement pointableElement = FindPointables(hits);

        // call feedbacks when the pointer left a interactable UI element
        if (pointableElement == null) {
            if (lastPointedElement == null) { return; }
            lastPointedElement.OnPointerLeft();
            lastPointedElement = pointableElement;
            return;
        }

        // call feedbacks when the pointer hits a interactable UI element
        if (pointableElement != lastPointedElement) {
            pointableElement.OnPointed();
            lastPointedElement = pointableElement;
        }

        bool isClicked = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
        if (isClicked) {
            SetGrabbableObjectParam(nearestHit.collider.gameObject);
            pointableElement.OnClicked();
        }

        bool isHeld = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
        if (isHeld)
        {
            //SetObjectDistFromController(nearestHit.collider.gameObject);
        }

        bool isReleased = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger);
        if (isReleased) {
            pointableElement.OnReleased();
            lastTouchPoint = Vector2.zero;
        }
    }

    private void SetGrabbableObjectParam(GameObject _pointedObj)
    {
        var grababbleObj = _pointedObj.GetComponent<GrababbleObject>();

        if(grababbleObj == null || (grababbleObj != null && grababbleObj.isGrabbed)) { return; }

        grababbleObj.controller = transform;
    }

    private void SetObjectDistFromController(GameObject _pointedObj)
    {
        if(!OVRInput.Get(OVRInput.RawTouch.LTouchpad) && !OVRInput.Get(OVRInput.RawTouch.RTouchpad)) { return; }

        Vector2 rawDir = GetSwipeDirection();
        Vector2 dirToCheck = GetSimplifiedDirection(rawDir);

        if (!(dirToCheck == Vector2.up) && !(dirToCheck == Vector2.down)) { return; }

        float distMul = 0.01f;
        Vector3 vecToPointedObj = (_pointedObj.transform.position - transform.position);
        float modifiedDist = vecToPointedObj.magnitude + distMul * rawDir.magnitude * dirToCheck.y;

        Vector3 newPos = _pointedObj.transform.position + vecToPointedObj.normalized * modifiedDist;
        _pointedObj.GetComponent<GrababbleObject>().OverridePosition(newPos);
    }

    private Vector2 GetSimplifiedDirection(Vector2 _rawDir)
    {
        Vector2[] directions = new Vector2[] {
            Vector2.up,
            Vector2.right,
            Vector2.down,
            Vector2.left
        };

        Vector2 swipeDir = _rawDir;
        Vector2 direction = Vector2.zero;
        float max = Mathf.NegativeInfinity;

        foreach (Vector2 dir in directions)
        {
            float dot = Vector2.Dot(dir, swipeDir.normalized);

            if (dot < max) { continue; }
            max = dot;
            direction = dir;
        }

        return direction;
    }

    private Vector2 GetTouchPadPos()
    {
        return OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
    }

    private Vector2 GetSwipeDirection()
    {
        Vector2 currTouchPos = GetTouchPadPos();
        Vector2 swipeDir = lastTouchPoint != Vector2.zero ? currTouchPos - lastTouchPoint : Vector2.zero;

        lastTouchPoint = currTouchPos;

        return swipeDir;
    }
    
}
