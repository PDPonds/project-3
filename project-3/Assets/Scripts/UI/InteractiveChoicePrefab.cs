using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static InteractiveChoicePrefab;

public class InteractiveChoicePrefab : MonoBehaviour
{
    public delegate bool EnableInteractiveCondition();
    public EnableInteractiveCondition enableInteractiveCondition;

    [SerializeField] Image interactiveChoiceIcon;
    [SerializeField] TextMeshProUGUI interactiveChoiceText;
    Button button;

    private void Update()
    {
        if (button != null && enableInteractiveCondition != null)
        {
            button.interactable = enableInteractiveCondition.Invoke();
        }
    }

    public void Setup(IActionObject actionObj, EnableInteractiveCondition condition)
    {
        interactiveChoiceText.text = actionObj.ActionName();

        enableInteractiveCondition += condition;

        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(actionObj.Action);
    }

    public void Setup(IDragable dragable, EnableInteractiveCondition condition)
    {
        interactiveChoiceText.text = dragable.DragName();

        enableInteractiveCondition += condition;

        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(dragable.BeginDrag);
    }

    public void Setup(string actionName, UnityAction action, EnableInteractiveCondition condition)
    {
        interactiveChoiceText.text = actionName;

        enableInteractiveCondition += condition;

        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

}
