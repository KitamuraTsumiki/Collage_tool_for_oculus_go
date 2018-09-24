using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
/// <summary>
/// this class receives a ray from the hand controller and call predefined events.
/// </summary>
[RequireComponent(typeof(BoxCollider)), RequireComponent(typeof(Image))]
public class VRToggleButton : MonoBehaviour, IPointableUIElement
{
    [SerializeField]
    private Text buttonLabel;
    [SerializeField]
    private string buttonTextForTrue;
    [SerializeField]
    private string buttonTextForFalse;
    [SerializeField]
    private bool toggleState;

    [SerializeField]
    private Image targetGraphic;
    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color pressedColor;
    [SerializeField]
    private Color disabledColor;

    [SerializeField]
    private UnityEvent onClicked;
    [SerializeField]
    private UnityEvent onReleased;
    [SerializeField]
    private UnityEvent onPointed;
    [SerializeField]
    private UnityEvent onPointerLeft;
    [SerializeField]
    private UnityEvent OnToggleStateTrue;
    [SerializeField]
    private UnityEvent OnToggleStateFalse;

    private bool isSelectable = true;
    private bool isPointerFeedbackActive = false;
    private bool toShowPointedFeedback = true;

    // pointer feedback status
    // when the button is pointed, it scales
    private Vector3 originalScale;
    [SerializeField]
    private float targetScaleRatio;
    private Vector3 targetScale;
    private float feedbackPhase = 0f;
    [SerializeField]
    private float feedbackSpeed = 0.05f;
    private bool isScaleParamInitialized = false;
    /// <summary>
    /// OnClicked can be called by the pointer
    /// </summary>
    public void OnClicked()
    {
        if (!isSelectable) { return; }
        //SetPressedColor();
        SwitchToggleState();
        onClicked.Invoke();
        ResetButtonState();
    }

    /// <summary>
    /// OnReleased can be called by the pointer
    /// </summary>
    public void OnReleased()
    {
        if (!isSelectable) { return; }
        //SetNormalColor();
        onReleased.Invoke();
    }

    /// <summary>
    /// OnPointed can be called by the pointer
    /// </summary>
    public void OnPointed()
    {
        if (!isSelectable) { return; }
        SetButtonFeedbackState(true);
        onPointed.Invoke();
    }

    /// <summary>
    /// OnPointerLeft can be called by the pointer
    /// </summary>
    public void OnPointerLeft()
    {
        if (!isSelectable) { return; }
        SetButtonFeedbackState(false);
        onPointerLeft.Invoke();
    }

    private void SwitchToggleState()
    {
        toggleState = !toggleState;

        if (toggleState) {
            buttonLabel.text = buttonTextForTrue;
            SetPressedColor();
            OnToggleStateTrue.Invoke();
            return;
        }

        buttonLabel.text = buttonTextForFalse;
        SetNormalColor();
        OnToggleStateFalse.Invoke();
    }

    private void ResetButtonState()
    {
        targetGraphic.color = normalColor;
        if (isSelectable) { return; }
        isSelectable = true;
    }

    public void InitButtonState()
    {
        targetGraphic.color = normalColor;
        toggleState = false;
        buttonLabel.text = buttonTextForFalse;

        if (isSelectable) { return; }
        isSelectable = true;
    }

    public void DisableSelectability()
    {
        isSelectable = false;

        if (targetGraphic.color == pressedColor) { return; }
        SetDisabledColor();
    }

    private void SetNormalColor()
    {
        targetGraphic.color = normalColor;
    }

    private void SetPressedColor()
    {
        targetGraphic.color = pressedColor;
    }

    private void SetDisabledColor()
    {
        targetGraphic.color = disabledColor;
    }

    public void SetButtonFeedbackState(bool _isPointerComing)
    {
        isPointerFeedbackActive = true;
        toShowPointedFeedback = _isPointerComing;
    }

    private void InitScaleParameters()
    {
        if (isScaleParamInitialized) { return; }
        originalScale = transform.localScale;
        targetScale = originalScale * targetScaleRatio;
        isScaleParamInitialized = true;
    }

    protected void ScaleUp()
    {
        if (!isPointerFeedbackActive || !toShowPointedFeedback || !isScaleParamInitialized) { return; }

        feedbackPhase = Mathf.Min(feedbackPhase + feedbackSpeed, 1f);
        transform.localScale = Vector3.Lerp(originalScale, targetScale, feedbackPhase);

        if (feedbackPhase < 1f) { return; }
        isPointerFeedbackActive = false;
    }

    protected void ScaleDown()
    {
        if (!isPointerFeedbackActive || toShowPointedFeedback || !isScaleParamInitialized) { return; }

        feedbackPhase = Mathf.Max(feedbackPhase - feedbackSpeed, 0f);
        transform.localScale = Vector3.Lerp(originalScale, targetScale, feedbackPhase);

        if (feedbackPhase > 0f) { return; }
        isPointerFeedbackActive = false;
    }

    private void OnEnable()
    {
        InitButtonState();
    }

    protected virtual void Start()
    {
        if (targetGraphic != null) { return; }
        targetGraphic = GetComponent<Image>();
        InitButtonState();
    }

    private void Update () {
        InitScaleParameters();
        ScaleUp();
        ScaleDown();
    }
}
