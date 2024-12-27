using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ShowInventoryType
{
    Inventory, Storage, Shop
}

public class UIManager : Singleton<UIManager>
{
    [Header("===== Interactive =====")]
    [Header("- Key")]
    [SerializeField] GameObject interactiveKey;
    [Header("- Choice")]
    [SerializeField] GameObject interactiveChoiceBorder;
    [SerializeField] Transform interactiveChoiceParent;
    [SerializeField] GameObject interactiveChoicePrefab;
    [SerializeField] Vector3 interactiveChoiceParentOffset;
    [Header("===== Inventory =====")]
    [SerializeField] GameObject inventoryPanel;
    [Header("- Inventory")]
    [SerializeField] GameObject inventoryBorder;
    [SerializeField] Transform inventoryParent;
    [SerializeField] GameObject itemSlotPrefab;
    [SerializeField] TextMeshProUGUI inventoryWeightText;
    [Header("- Storage")]
    [SerializeField] GameObject storageBorder;
    [Header("- Shop")]
    [SerializeField] GameObject shopBorder;

    private void Awake()
    {
        Button border = interactiveChoiceBorder.GetComponent<Button>();
        border.onClick.AddListener(CloseInteractiveChoice);
    }

    public void ClearParent(Transform parent)
    {
        if (parent.childCount > 0)
        {
            for (int i = 0; i < parent.childCount; ++i)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }
    }

    #region Interactive

    public void ShowInteractiveKey(Vector3 worldPos)
    {
        interactiveKey.SetActive(true);
        Vector3 scenePoint = Camera.main.WorldToScreenPoint(worldPos);
        interactiveKey.transform.position = scenePoint;
    }

    public void HideInteractiveKey()
    {
        interactiveKey.SetActive(false);
    }

    public void ShowInteractiveChoice()
    {
        GameObject interactiveObj = GameManager.Instance.curInteractiveObj;
        if (interactiveObj == null) return;

        GameManager.Instance.curPlayer.SwitchState(PersonState.Interact);

        interactiveChoiceBorder.SetActive(true);

        Vector3 scenePoint = Camera.main.WorldToScreenPoint(interactiveObj.transform.position);
        interactiveChoiceParent.transform.position = scenePoint + interactiveChoiceParentOffset;

        ClearParent(interactiveChoiceParent);
        if (interactiveObj.TryGetComponent<IActionObject>(out IActionObject actionObject))
        {
            GameObject actionChoice = Instantiate(interactiveChoicePrefab, interactiveChoiceParent);
            InteractiveChoicePrefab choice = actionChoice.GetComponent<InteractiveChoicePrefab>();
            choice.Setup(actionObject);
        }

    }

    public void HideInteractiveChoice()
    {
        interactiveChoiceBorder.SetActive(false);
    }

    public void CloseInteractiveChoice()
    {
        HideInteractiveChoice();
        GameManager.Instance.curPlayer.SwitchState(PersonState.CancleInteract);
    }

    #endregion

    #region Inventory

    void InitItemSlotToParent(List<ItemSlot> slots, Transform parent)
    {
        ClearParent(parent);
        if (slots.Count > 0)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                GameObject obj = Instantiate(itemSlotPrefab, parent);
                ItemSlotPrefab slotprefab = obj.GetComponent<ItemSlotPrefab>();
                slotprefab.Setup(slots[i]);
            }
        }
    }

    void UpdateInventory()
    {
        InitItemSlotToParent(GameManager.Instance.playerInventory.slots, inventoryParent);
        inventoryWeightText.text = $"{GameManager.Instance.playerInventory.GetCurWeight().ToString()} / {GameManager.Instance.playerInventory.maxWeight}";
    }

    public void ToggleInventory(ShowInventoryType showType)
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        storageBorder.SetActive(false);
        shopBorder.SetActive(false);
        inventoryBorder.SetActive(true);
        if (inventoryPanel.activeSelf) UpdateInventory();

        switch (showType)
        {
            case ShowInventoryType.Inventory:
                break;
            case ShowInventoryType.Storage:
                storageBorder.SetActive(true);
                break;
            case ShowInventoryType.Shop:
                shopBorder.SetActive(true);
                break;
        }
    }

    #endregion

}
