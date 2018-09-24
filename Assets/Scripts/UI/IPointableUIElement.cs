using UnityEngine;
/// <summary>
/// this class receives a ray from the hand controller and call predefined events.
/// </summary>

public interface IPointableUIElement {

    void OnPointed();
    void OnReleased();
    void OnClicked();
    void OnPointerLeft();
}
