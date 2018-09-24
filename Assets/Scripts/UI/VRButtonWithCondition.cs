using UnityEngine;
/// <summary>
/// multidimensional array to manage timecodes for subtitles
/// </summary>
[System.Serializable]
public class MultiDimensionalArray<T>
{
    public T[] array;

    public MultiDimensionalArray(int _arrayLength)
    {
        array = new T[_arrayLength];
    }
}

public class VRButtonWithCondition : VRButton
{
    [SerializeField]
    private VRButton[] buttonGroup_distance;
    [SerializeField]
    private VRButton[] buttonGroup_speed;
    [SerializeField]
    private VRButton[] buttonGroup_actionType;
    [SerializeField]
    private VRButton[] buttonGroup_subtitles;

    private bool isActivated = false;

    private MultiDimensionalArray<VRButton>[] buttonGroups;

    protected override void OnEnable()
    {
        DisableSelectability();
        isActivated = false;
    }

    private void OnDisable()
    {
        DisableSelectability();
    }

    public override void DisableSelectability()
    {
        base.DisableSelectability();
        SetDisabledColor();
    }

    protected override void Start()
    {
        base.Start();
        DisableSelectability();

        InitButtonGroupArrays();
    }

    private void InitButtonGroupArrays()
    {
        int buttonGroupNum = 4;
        buttonGroups = new MultiDimensionalArray<VRButton>[buttonGroupNum];
        
        buttonGroups[0] = new MultiDimensionalArray<VRButton>(buttonGroup_distance.Length);
        buttonGroups[0].array = buttonGroup_distance;
        buttonGroups[1] = new MultiDimensionalArray<VRButton>(buttonGroup_speed.Length);
        buttonGroups[1].array = buttonGroup_speed;
        buttonGroups[2] = new MultiDimensionalArray<VRButton>(buttonGroup_actionType.Length);
        buttonGroups[2].array = buttonGroup_actionType;
        buttonGroups[3] = new MultiDimensionalArray<VRButton>(buttonGroup_subtitles.Length);
        buttonGroups[3].array = buttonGroup_subtitles;
    }

    protected override void Update()
    {
        if (!isScaleParamInitialized)
        {
            DisableSelectability();
        }

        base.Update();
        CheckActivationCondition();
    }

    private void CheckActivationCondition()
    {
        if (isActivated) { return; }

        for (int i = 0; i < buttonGroups.Length; i++)
        {
            if (!isVRButtonInGroupPressed(buttonGroups[i].array)) { return; }
        }
        
        InitButtonState(); // activate the button
        isActivated = true;
    }

    private bool isVRButtonInGroupPressed(VRButton[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].isPressed()) { return true; }
        }

        return false;
    }
}
