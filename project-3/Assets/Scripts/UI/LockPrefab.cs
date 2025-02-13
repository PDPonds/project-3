using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockPrefab : MonoBehaviour
{
    [SerializeField] Image visual;
    [SerializeField] TextMeshProUGUI numberText;

    int number;

    public void Setup(int number, bool isShowNumber)
    {
        this.number = number;
        if (isShowNumber) ShowNumber();
        else ShowQuestionMask();
    }

    public void ShowQuestionMask()
    {
        numberText.text = "?";
    }

    public void ShowNumber()
    {
        numberText.text = number.ToString();
    }

    public void SetVisualColor(Color color)
    {
        visual.color = color;
    }

    public bool CheckCorrectNumber(int nextNumber)
    {
        return this.number == nextNumber;
    }

}
