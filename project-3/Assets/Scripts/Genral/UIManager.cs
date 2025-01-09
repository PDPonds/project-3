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
    [Header("===== Action Duration =====")]
    [SerializeField] GameObject actionDurationBorder;
    [SerializeField] Image actionDurationFill;
    [Header("===== Select Map =====")]
    [SerializeField] Transform selectMapPanel;
    [SerializeField] Button driveButton;

    private void Awake()
    {
        Button border = interactiveChoiceBorder.GetComponent<Button>();
        border.onClick.AddListener(CloseInteractiveChoice);

        driveButton.onClick.AddListener(DriveButton);

    }

    private void Start()
    {
        ShowPlayerStatusPanel();
    }

    private void Update()
    {
        if (GameManager.Instance.IsPhase(GamePhase.DuringGame))
        {
            if (GameManager.Instance.curPlayer.IsState(PlayerState.Action))
            {
                UpdateActionDurationFill(GameManager.Instance.curPlayer.actionTime, GameManager.Instance.curPlayer.maxActionTime);
            }
        }
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

    public void ShowPlayerStatusPanel()
    {
        playerStatusPanel.gameObject.SetActive(true);
        UpdatePlayerStatus();
    }

    public void HidePlayerStatusPanel()
    {
        playerStatusPanel.gameObject.SetActive(false);
    }

    public void UpdatePlayerStatus()
    {
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;

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
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;

        if (GameManager.Instance.curPlayer.IsState(PlayerState.Aim)) GameManager.Instance.curPlayer.SwitchState(PlayerState.EndAnyAction);


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
            choice.Setup(actionObject, () => { return true; });
        }

        if (interactiveObj.TryGetComponent<IDragable>(out IDragable dragable))
        {
            GameObject actionChoice = Instantiate(interactiveChoicePrefab, interactiveChoiceParent);
            InteractiveChoicePrefab choice = actionChoice.GetComponent<InteractiveChoicePrefab>();
            choice.Setup(dragable, () => { return true; });
        }

        if (interactiveObj.TryGetComponent<VehicleObject>(out VehicleObject vehicleObject))
        {
            GameObject actionChoice_drive = Instantiate(interactiveChoicePrefab, interactiveChoiceParent);
            InteractiveChoicePrefab choice_drive = actionChoice_drive.GetComponent<InteractiveChoicePrefab>();
            choice_drive.Setup("Drive", vehicleObject.onDrive, () => { return true; });

            GameObject actionChoice_fillGas = Instantiate(interactiveChoicePrefab, interactiveChoiceParent);
            InteractiveChoicePrefab choice_fillGas = actionChoice_fillGas.GetComponent<InteractiveChoicePrefab>();
            choice_fillGas.Setup("Fill Gas", vehicleObject.onFillGas, () =>
            {
                if (GameManager.Instance.curHandSlot.HasItemInSlot(out ItemSlotPrefab itemSlot))
                {
                    if (itemSlot.curSlot.item is GasTankItemSO gasTank &&
                    itemSlot.curSlot.curValue > 0)
                    {
                        return true;
                    }
                }

                return false;
            });

            GameObject actionChoice_repair = Instantiate(interactiveChoicePrefab, interactiveChoiceParent);
            InteractiveChoicePrefab choice_repair = actionChoice_repair.GetComponent<InteractiveChoicePrefab>();
            choice_repair.Setup("Repair", vehicleObject.onRepair, () =>
            {
                if (GameManager.Instance.curHandSlot.HasItemInSlot(out ItemSlotPrefab itemSlot))
                {
                    if (itemSlot.curSlot.item is GearBoxItemSO gearBox &&
                    itemSlot.curSlot.curValue > 0)
                    {
                        return true;
                    }
                }

                return false;
            });
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
        if (GameManager.Instance.curStorageObj != null)
            InitItemSlotToParent(GameManager.Instance.curStorageObj.slots, storageParent);
    }

    public void ToggleInventory(ShowInventoryType showType)
    {
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;

        if (inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);
            ShowPlayerStatusPanel();
            GameManager.Instance.curStorageObj = null;
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
    public void GenerateText(string text, float destroyDuration)
    {
        if (textParent.childCount < 5)
        {
            GameObject obj = Instantiate(textPrefab, textParent);
            TextMeshProUGUI tmpro = obj.GetComponent<TextMeshProUGUI>();
            tmpro.text = text;
            Destroy(obj, destroyDuration);
        }
    }

    #endregion

    #region Action
    public void ShowActionDuration()
    {
        actionDurationBorder.SetActive(true);
    }

    public void HideActionDuration()
    {
        actionDurationBorder.SetActive(false);
    }

    void UpdateActionDurationFill(float c, float m)
    {
        float p = c / m;
        actionDurationFill.fillAmount = p;
    }
    #endregion

    #region SelectMap
    public void ShowSelectMap()
    {
        selectMapPanel.gameObject.SetActive(true);
    }

    public void HideSelectMap()
    {
        selectMapPanel.gameObject.SetActive(false);
    }

    void DriveButton()
    {
        GameManager.Instance.SwitchPhase(GamePhase.GameStart);
    }

    #endregion

}
