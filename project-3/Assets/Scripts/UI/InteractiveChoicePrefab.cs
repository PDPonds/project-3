using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveChoicePrefab : MonoBehaviour
{
    [SerializeField] Image interactiveChoiceIcon;
    [SerializeField] TextMeshProUGUI interactiveChoiceText;
    Button button;

    public void Setup(IActionObject actionObj)
    {
        interactiveChoiceText.text = actionObj.ActionName();

        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(actionObj.Action);
    }

}
