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
    public Transform canvasTransform;
    [Header("===== PlayerStatus =====")]
    [SerializeField] Transform playerStatusPanel;
    [SerializeField] Transform playerStatus_HandSlot_1_Border;
    [SerializeField] Transform playerStatus_HandSlot_2_Border;
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
    public Transform inventoryParent;
    [SerializeField] GameObject itemSlotPrefab;
    [SerializeField] TextMeshProUGUI inventoryWeightText;
    [Header("- Hand")]
    public Transform handSlotParent_1;
    public Transform handSlotParent_2;
    [Header("- Storage")]
    [SerializeField] GameObject storageBorder;
    public Transform storageParent;
    [Header("- Shop")]
    [SerializeField] GameObject shopBorder;
    [Header("===== Generate Text =====")]
    [SerializeField] Transform textParent;
    [SerializeField] GameObject textPrefab;

    private void Awake()
    {
        Button border = interactiveChoiceBorder.GetComponent<Button>();
        border.onClick.AddListener(CloseInteractiveChoice);
    }

    private void Start()
    {
        ShowPlayerStatusPanel();
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

    #region PlayerStatusPanel

    void ShowPlayerStatusPanel()
    {
        playerStatusPanel.gameObject.SetActive(true);
        UpdatePlayerStatus();
    }

    void HidePlayerStatusPanel()
    {
        playerStatusPanel.gameObject.SetActive(false);
    }

    public void UpdatePlayerStatus()
    {
        InitItemSlotToParent(GameManager.Instance.curPlayer.handSlot_1, playerStatus_HandSlot_1_Border);
        InitItemSlotToParent(GameManager.Instance.curPlayer.handSlot_2, playerStatus_HandSlot_2_Border);
        Image img_1 = playerStatus_HandSlot_1_Border.GetComponent<Image>();
        Image img_2 = playerStatus_HandSlot_2_Border.GetComponent<Image>();
        img_1.color = new Color(1, 1, 1, 0);
        img_2.color = new Color(1, 1, 1, 0);
        if (GameManager.Instance.curHandSlot != null)
        {
            if (GameManager.Instance.curHandSlot.transform == handSlotParent_1)
            {
                img_1.color = new Color(1, 1, 1, 1);
            }
            else if (GameManager.Instance.curHandSlot.transform == handSlotParent_2)
            {
                img_2.color = new Color(1, 1, 1, 1);
            }
        }
    }

    #endregion

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

        GameManager.Instance.curPlayer.SwitchState(PlayerState.ShowUI);

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

        if (interactiveObj.TryGetComponent<IDragable>(out IDragable dragable))
        {
            GameObject actionChoice = Instantiate(interactiveChoicePrefab, interactiveChoiceParent);
            InteractiveChoicePrefab choice = actionChoice.GetComponent<InteractiveChoicePrefab>();
            choice.Setup(dragable);
        }

    }

    public void HideInteractiveChoice()
    {
        interactiveChoiceBorder.SetActive(false);
    }

    public void CloseInteractiveChoice()
    {
        HideInteractiveChoice();
        GameManager.Instance.curPlayer.SwitchState(PlayerState.EndAnyAction);
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

    void InitItemSlotToParent(ItemSlot slot, Transform parent)
    {
        ClearParent(parent);
        if (slot.item != null)
        {
            GameObject obj = Instantiate(itemSlotPrefab, parent);
            ItemSlotPrefab slotprefab = obj.GetComponent<ItemSlotPrefab>();
            slotprefab.Setup(slot);
        }
    }

    public void UpdateInventory()
    {
        InitItemSlotToParent(GameManager.Instance.playerInventory.slots, inventoryParent);
        InitItemSlotToParent(GameManager.Instance.curPlayer.handSlot_1, handSlotParent_1);
        InitItemSlotToParent(GameManager.Instance.curPlayer.handSlot_2, handSlotParent_2);
        inventoryWeightText.text = $"{GameManager.Instance.playerInventory.GetCurWeight().ToString()} / {GameManager.Instance.playerInventory.maxWeight}";
    }

    public void UpdateStorage()
    {
        InitItemSlotToParent(GameManager.Instance.curStorageObj.slots, storageParent);
    }

    public void ToggleInventory(ShowInventoryType showType)
    {
        if (inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);
            ShowPlayerStatusPanel();
            GameManager.Instance.curPlayer.SwitchState(PlayerState.EndAnyAction);
        }
        else
        {
            inventoryPanel.SetActive(true);
            CloseInteractiveChoice();
            switch (showType)
            {
                case ShowInventoryType.Inventory:
                    if (GameManager.Instance.curPlayer.IsState(PlayerState.ShowUI)) return;
                    storageBorder.SetActive(false);
                    shopBorder.SetActive(false);
                    inventoryBorder.SetActive(true);
                    break;
                case ShowInventoryType.Storage:
                    storageBorder.SetActive(true);
                    UpdateStorage();
                    break;
                case ShowInventoryType.Shop:
                    shopBorder.SetActive(true);
                    break;
            }
            GameManager.Instance.curPlayer.SwitchState(PlayerState.ShowUI);
            UpdateInventory();
            HidePlayerStatusPanel();
        }
    }

    #endregion

    #region GenerateText
    public GameObject GenerateText(string text)
    {
        GameObject obj = Instantiate(textPrefab, textParent);
        TextMeshProUGUI tmpro = obj.GetComponent<TextMeshProUGUI>();
        tmpro.text = text;
        return obj;
    }

    #endregion

}
