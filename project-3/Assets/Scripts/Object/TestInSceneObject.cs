using UnityEngine;

public class TestInSceneObject : MonoBehaviour, IActionObject
{
    public void Action()
    {
        Debug.Log("Test Action");

        UIManager.Instance.CloseInteractiveChoice();
    }

    public string ActionName()
    {
        return "Action";
    }
}
