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
    [Header("- Status")]
    [SerializeField] Transform playerStatusPanel;
    [SerializeField] Transform hpStatutParent;
    [SerializeField] Transform hungryStatutParent;
    [SerializeField] Transform thirstyStatutParent;
    [SerializeField] Transform playerStatus_HandSlot_1_Border;
    [SerializeField] Transform playerStatus_HandSlot_2_Border;
    [Header("- Day")]
    [SerializeField] TextMeshProUGUI dayTextOnPlayerStatus;
    [Header("- Coin")]
    [SerializeField] TextMeshProUGUI coinText;
    [Header("===== Interactive =====")]
    [Header("- Key")]
    [SerializeField] GameObject interactiveKey;
    [Header("- Choice")]
    [SerializeField] GameObject interactiveChoiceBorder;
    [SerializeField] Transform interactiveChoiceParent;
    [SerializeField] GameObject interactiveChoicePrefab;
    [SerializeField] Vector3 interactiveChoiceParentOffset;
    [Header("- Vehicle Status")]
    [SerializeField] GameObject vehicleStatusPrefab;
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
    public Button driveButton;
    [SerializeField] Transform previousMapAndSelectMapParent;
    [SerializeField] GameObject previousMapPrefab;
    [SerializeField] GameObject selectMapPrefab;
    [SerializeField] GameObject selectMapLinePrefab;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI dayTextOnSelectMap;

    MapTypeSO select_1;
    MapTypeSO select_2;

    [Header("===== Throwing Item =====")]
    [SerializeField] Transform throwingArea;
    MeshRenderer throwingAreaMeshRen;
    [SerializeField] Material throwingAbleMat;
    [SerializeField] Material unThrowingAbleMat;
    [SerializeField] Transform throwingRange;
    [Header("===== Lock Pick =====")]
    [SerializeField] GameObject lockpickPanel;
    [SerializeField] Transform lockParent;
    [SerializeField] GameObject lockPrefab;
    [SerializeField] Transform pickVisual;
    ILockable curLockPicking;
    List<LockPrefab> lockPrefabs = new List<LockPrefab>();

    int NextLockCount;
    int CurLockPickPosition;
    int MaxLockCount;

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

            UpdateThrowingAreaPosition();
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
        UpdateDayOnPlayerStatus();
    }

    public void HidePlayerStatusPanel()
    {
        playerStatusPanel.gameObject.SetActive(false);
    }

    public void UpdatePlayerStatus()
    {
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;

        PlayerDatas playerDatas = GameManager.Instance.curPlayer.playerDatas;
        UpdateStatusPrefab(playerDatas.hpColor, playerDatas.maxHP, playerDatas.curHP, hpStatutParent);
        UpdateStatusPrefab(playerDatas.hungryColor, playerDatas.maxHungry, playerDatas.curHungry, hungryStatutParent);
        UpdateStatusPrefab(playerDatas.thirstyColor, playerDatas.maxThirsty, playerDatas.curThirsty, thirstyStatutParent);

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

    void UpdateStatusPrefab(Color color, int max, int cur, Transform parent)
    {
        ClearParent(parent);

        if (cur > 0)
        {
            for (int i = 0; i < cur; i++)
            {
                InitStatusPrefab(color, i.ToString(), parent);
            }
        }

        int emptyCount = max - cur;
        if (emptyCount > 0)
        {
            for (int i = 0; i < emptyCount; i++)
            {
                InitStatusPrefab(GameManager.Instance.curPlayer.playerDatas.emptyColor, i.ToString(), parent);
            }
        }
    }

    GameObject InitStatusPrefab(Color color, string name, Transform statusParent)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(statusParent);
        Image img = obj.AddComponent<Image>();
        img.color = color;
        return obj;
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
        if (interactiveObj.TryGetComponent<VehicleObject>(out VehicleObject vehicleObject))
        {
            GameObject vehicleStatusObj = Instantiate(vehicleStatusPrefab, interactiveChoiceParent);
            VehicleStatusPrefab vSP = vehicleStatusObj.GetComponent<VehicleStatusPrefab>();
            vehicleObject.curStatusInfo = vSP;
            vSP.UpdateStatus(vehicleObject.maxGas, vehicleObject.curGas, vehicleObject.maxHP, vehicleObject.curHP);

            GameObject actionChoice_drive = Instantiate(interactiveChoicePrefab, interactiveChoiceParent);
            InteractiveChoicePrefab choice_drive = actionChoice_drive.GetComponent<InteractiveChoicePrefab>();
            choice_drive.Setup("Drive", vehicleObject.onDrive, () => { return true; });

            GameObject actionChoice_fillGas = Instantiate(interactiveChoicePrefab, interactiveChoiceParent);
            InteractiveChoicePrefab choice_fillGas = actionChoice_fillGas.GetComponent<InteractiveChoicePrefab>();
            choice_fillGas.Setup("Fill Gas", vehicleObject.onFillGas, () =>
            {
                if (GameManager.Instance.curHandSlot.HasItemInSlot(out ItemSlotPrefab itemSlot)
                && itemSlot.curSlot.item is GasTankItemSO gasTank && itemSlot.curSlot.curValue > 0)
                {
                    return true;

                }

                return false;
            });

            GameObject actionChoice_repair = Instantiate(interactiveChoicePrefab, interactiveChoiceParent);
            InteractiveChoicePrefab choice_repair = actionChoice_repair.GetComponent<InteractiveChoicePrefab>();
            choice_repair.Setup("Repair", vehicleObject.onRepair, () =>
            {
                if (GameManager.Instance.curHandSlot.HasItemInSlot(out ItemSlotPrefab itemSlot)
                && itemSlot.curSlot.item is GearBoxItemSO gearBox && itemSlot.curSlot.curValue > 0)
                {
                    return true;
                }

                return false;
            });
        }

        if (interactiveObj.TryGetComponent<IActionObject>(out IActionObject actionObject))
        {
            GameObject actionChoice = Instantiate(interactiveChoicePrefab, interactiveChoiceParent);
            InteractiveChoicePrefab choice = actionChoice.GetComponent<InteractiveChoicePrefab>();
            choice.Setup(actionObject, () =>
            {
                if (interactiveObj.TryGetComponent<ILockable>(out ILockable lockable))
                {
                    if (lockable.IsLocked)
                    {
                        if (lockable.IsLocked && GameManager.Instance.curHandSlot.HasItemInSlot(out ItemSlotPrefab itemSlot)
                        && itemSlot.curSlot.item.itemID == 99)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else return true;
                }
                return true;

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

    #region Throwing Item
    public void ShowThrowingVisual(float area, float range)
    {
        throwingArea.gameObject.SetActive(true);
        throwingRange.gameObject.SetActive(true);
        throwingArea.localScale = new Vector3(area * 2, area * 2, 1);
        throwingRange.localScale = new Vector3(range * 2, range * 2, 1);
        if (throwingAreaMeshRen == null)
        {
            throwingAreaMeshRen = throwingArea.GetComponent<MeshRenderer>();
        }
    }

    void UpdateThrowingAreaPosition()
    {
        if (throwingArea.gameObject.activeSelf)
        {

            Vector3 mousePos = GameManager.Instance.GetWorldPosFormMouse();
            Vector3 areaPos = mousePos + new Vector3(0, 0.02f, 0);
            throwingArea.transform.position = areaPos;
            if (ThrowingPointInRange()) throwingAreaMeshRen.material = throwingAbleMat;
            else throwingAreaMeshRen.material = unThrowingAbleMat;
        }

        if (throwingRange.gameObject.activeSelf)
        {
            Vector3 rangePos = GameManager.Instance.curPlayer.transform.position + new Vector3(0, 0.01f, 0);
            throwingRange.transform.position = rangePos;
        }
    }

    public void HideThrowingVisual()
    {
        throwingArea.gameObject.SetActive(false);
        throwingRange.gameObject.SetActive(false);
    }

    public bool ThrowingPointInRange()
    {
        float dis = Vector3.Distance(throwingArea.transform.position, GameManager.Instance.curPlayer.transform.position);
        float attackRange = GameManager.Instance.curPlayer.throwingRange;
        return (dis < attackRange);
    }

    #endregion

    #region SelectMap
    public void ShowSelectMap()
    {
        selectMapPanel.gameObject.SetActive(true);
        driveButton.interactable = false;

        select_1 = null;
        select_2 = null;

        UpdateSelectMapInfo();
        UpdateDistanceText();
        UpdateDayOnSelectMap();
    }

    public void HideSelectMap()
    {
        selectMapPanel.gameObject.SetActive(false);
    }

    void UpdateSelectMapInfo()
    {
        ClearParent(previousMapAndSelectMapParent);
        InitSelectMap();
        InitPreviousMap();
    }

    void InitSelectMap()
    {
        if (GameManager.Instance.currentDistance <= 0)
        {
            CustomMap map = MapGenerator.Instance.RandomStartMap();
            if (map != null)
            {
                select_1 = map;
                GameObject obj = Instantiate(selectMapPrefab, previousMapAndSelectMapParent);
                SelectMapPrefab select = obj.GetComponent<SelectMapPrefab>();
                select.Setup(map);
            }
        }
        else if (MapGenerator.Instance.IsObjectiveDay(GameManager.Instance.curDay, GameManager.Instance.curTimeOfDay, out CustomMap map))
        {
            if (map != null)
            {
                select_1 = map;
                GameObject obj = Instantiate(selectMapPrefab, previousMapAndSelectMapParent);
                SelectMapPrefab select = obj.GetComponent<SelectMapPrefab>();
                select.Setup(map);
            }
        }
        else
        {
            List<MapTypeSO> maps = MapGenerator.Instance.RandomMap(2);
            if (maps.Count > 0)
            {
                MapTypeSO map_1 = maps[0];
                MapTypeSO map_2 = maps[1];

                select_1 = map_1;
                select_2 = map_2;

                GameObject obj = Instantiate(selectMapPrefab, previousMapAndSelectMapParent);
                SelectMapPrefab select = obj.GetComponent<SelectMapPrefab>();
                select.Setup(map_1, map_2);
            }
        }
    }

    void InitPreviousMap()
    {
        if (MapGenerator.Instance.previousMap.Count > 0)
        {
            for (int i = 0; i < MapGenerator.Instance.previousMap.Count; i++)
            {
                Instantiate(selectMapLinePrefab, previousMapAndSelectMapParent);
                MapTypeSO map_1 = MapGenerator.Instance.previousMap[i].map_1;
                MapTypeSO map_2 = MapGenerator.Instance.previousMap[i].map_2;
                MapTypeSO selectMap = MapGenerator.Instance.previousMap[i].selectMap;
                GameObject obj = Instantiate(selectMapPrefab, previousMapAndSelectMapParent);
                SelectMapPrefab select = obj.GetComponent<SelectMapPrefab>();
                if (map_2 != null)
                {
                    select.SetupPreviousMap(map_1, map_2, selectMap);
                }
                else
                {
                    select.SetupPreviousMap(map_1);
                }
            }
        }
    }

    void DriveButton()
    {
        if (GameManager.Instance.curMapSelect != null)
        {
            MapGenerator.Instance.GenerateMap(GameManager.Instance.curMapSelect);

            previousMapSlot previousMapSlot = new previousMapSlot();
            previousMapSlot.map_1 = select_1;
            previousMapSlot.map_2 = select_2;
            previousMapSlot.selectMap = GameManager.Instance.curMapSelect;
            MapGenerator.Instance.previousMap.Insert(0, previousMapSlot);
            GameManager.Instance.currentDistance += GameManager.Instance.currentMapDistanceOnSelect;
            GameManager.Instance.currentMapDistanceOnSelect = 0;
            GameManager.Instance.SwitchPhase(GamePhase.GameStart);
        }
    }

    public void UpdateDistanceText()
    {
        if (GameManager.Instance.currentMapDistanceOnSelect <= 0)
        {
            distanceText.text = $"{GameManager.Instance.currentDistance} / {GameManager.Instance.currentTargetDistance} kg.";
        }
        else
        {
            distanceText.text = $"{GameManager.Instance.currentDistance}(+ {GameManager.Instance.currentMapDistanceOnSelect}) / {GameManager.Instance.currentTargetDistance} kg.";
        }
    }

    #endregion

    #region Day
    public void UpdateDayOnPlayerStatus()
    {
        dayTextOnPlayerStatus.text = $"Day {GameManager.Instance.curDay} ({GameManager.Instance.curTimeOfDay})";
    }

    public void UpdateDayOnSelectMap()
    {
        dayTextOnSelectMap.text = $"Day {GameManager.Instance.curDay} ({GameManager.Instance.curTimeOfDay})";
    }

    #endregion

    #region Coin
    public void UpdateCoin()
    {
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;
        coinText.text = GameManager.Instance.curPlayer.playerDatas.coin.ToString();
    }
    #endregion

    #region Lock Pick

    public void ShowLockPick(List<int> lockPos, int startShowCount, ILockable locking)
    {
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;
        lockpickPanel.gameObject.SetActive(true);
        NextLockCount = 1;
        CurLockPickPosition = 1;
        curLockPicking = locking;
        MaxLockCount = lockPos.Count;
        InitLock(lockPos, startShowCount);
        UpdatePickPosition();
        GameManager.Instance.curPlayer.SwitchState(PlayerState.ShowUI);
    }

    public void MoveLockPick(int dir)
    {
        CurLockPickPosition += dir;
        if (CurLockPickPosition < 1) CurLockPickPosition = 1;
        if (CurLockPickPosition > MaxLockCount) CurLockPickPosition = MaxLockCount;
        UpdatePickPosition();
    }

    public void TryPickLock()
    {
        if (lockPrefabs[CurLockPickPosition - 1].number == NextLockCount)
        {
            lockPrefabs[CurLockPickPosition - 1].ShowNumber();
            lockPrefabs[CurLockPickPosition - 1].SetVisualColor(Color.green);
            if (NextLockCount < MaxLockCount)
                NextLockCount++;
            else
            {
                curLockPicking.IsLocked = false;
                curLockPicking.ActionAfterUnlock();
                HideLockPick();
            }
        }
        else if (lockPrefabs[CurLockPickPosition - 1].number > NextLockCount)
        {
            for (int i = 0; i < lockPrefabs.Count; i++)
            {
                if (lockPrefabs[i].number == 1 || lockPrefabs[i].number == 2)
                {
                    lockPrefabs[i].ShowNumber();
                }
                else
                {
                    lockPrefabs[i].ShowQuestionMask();
                }
                lockPrefabs[i].SetVisualColor(Color.white);
            }
            NextLockCount = 1;
        }
    }

    void InitLock(List<int> lockPos, int startShowCount)
    {
        if (lockPos.Count == 0) return;

        ClearParent(lockParent);
        lockPrefabs.Clear();

        for (int i = 0; i < lockPos.Count; i++)
        {
            bool isShow = lockPos[i] <= startShowCount;
            GameObject obj = Instantiate(lockPrefab, lockParent);
            LockPrefab lockprefab = obj.GetComponent<LockPrefab>();
            lockprefab.Setup(lockPos[i], isShow);
            lockPrefabs.Add(lockprefab);
        }
    }

    public void UpdatePickPosition()
    {
        float posX = lockPrefabs[CurLockPickPosition - 1].GetComponent<RectTransform>().anchoredPosition.x;
        pickVisual.GetComponent<RectTransform>().anchoredPosition = new Vector3(posX, 0, 0);
    }


    public void HideLockPick()
    {
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;
        curLockPicking = null;
        lockpickPanel.gameObject.SetActive(false);
    }

    public bool IsLockPickActive()
    {
        return lockpickPanel.activeSelf;
    }

    #endregion

}
