using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component which listens to UI-related unity events from the GA controller and updates
/// the assigned text when appropriate.
/// </summary>
public class TextController : MonoBehaviour
{
    public GAController BasketballGAController;     // Reference to the genetic algorithm controller
    public Text TextObject;                         // Reference to text game object which is updated
    public string UIEventKey;                       // Event key which determines which UI-related unity event is listened to

    void Start()
    {
        if (BasketballGAController)
            BasketballGAController.AddUIEvent(UIEventKey, UpdateText);
    }

    /// <summary>
    /// Callback method which updates the text of the assigned text game object
    /// using the received value.
    /// </summary>
    /// <param name="value">Value to be used for updating the text of the text game object.</param>
    public void UpdateText(float value)
    {
        if (TextObject)
            TextObject.text = value.ToString("F2");
    }
}
