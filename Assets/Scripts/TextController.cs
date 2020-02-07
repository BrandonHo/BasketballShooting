using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public GAInterface BasketballGAInterface;
    public Text TextObject;
    public string UIEventKey;

    void Start()
    {
        if (BasketballGAInterface)
            BasketballGAInterface.AddUIEvent(UIEventKey, UpdateText);
    }

    public void UpdateText(float value)
    {
        if (TextObject)
            TextObject.text = value.ToString("F2");
    }
}
