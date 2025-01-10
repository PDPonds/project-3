using System;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase
{
    SelectMap, GameStart, DuringGame, EndGame
}

public class GameManager : Singleton<GameManager>
{
    GamePhase phase;
    [Header("===== Init On Game Start ======")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject cameraPrefab;
    [Header("===== Player =====")]
    public InventorySO playerInventory;
    [HideInInspector] public PlayerManager curPlayer;
    [HideInInspector] public CameraController curCameraController;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public DropItemSlot curHandSlot;
    [Header("===== Player Interactive =====")]
    [HideInInspector] public GameObject curInteractiveObj;
    [HideInInspector] public StorageObject curStorageObj;
    [Header("===== Input =====")]
    [SerializeField] LayerMask mousePosMask;
    [HideInInspector] public Vector2 mousePos;
    [HideInInspector] public Vector2 moveInput;
    [Header("===== Select Map =====")]
    [HideInInspector] public MapTypeSO curMapSelect;

    private void Start()
    {
        SwitchPhase(GamePhase.SelectMap);
    }

    private void Update()
    {
        UpdatePhase();
    }

    #region Init On Game Start

    void InitPlayer()
    {
        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        curPlayer = playerManager;
        playerManager.Setup();

        curCameraController.Setup(player.transform);
    }

    void InitCamera(Transform target)
    {
        if (curCameraController != null) return;

        GameObject camera = Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);
        CameraController camControl = camera.GetComponent<CameraController>();
        curCameraController = camControl;
        camControl.Setup(target);
    }

    #endregion

    #region Mouse

    public Vector3 GetWorldPosFormMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        Vector3 worldPos = Vector3.zero;
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, mousePosMask))
        {
            worldPos = hit.point;
        }

        return worldPos;
    }

    public Vector3 GetDirToMouse(Vector3 origin)
    {
        Vector3 mouseWorldPos = GetWorldPosFormMouse();
        Vector3 dir = mouseWorldPos - origin;
        dir.Normalize();
        dir.y = 0;
        return dir;
    }

    #endregion

    #region Select Hand Slot

    public void SelectHandSlot(int handSlot)
    {
        if (!IsPhase(GamePhase.DuringGame)) return;

        if (curPlayer.IsState(PlayerState.Aim)) curPlayer.SwitchState(PlayerState.EndAnyAction);

        if (curPlayer.IsState(PlayerState.Draging))
        {
            UIManager.Instance.GenerateText("Draging", 2f);
            return;
        }

        if (curPlayer.IsState(PlayerState.Action))
        {
            curPlayer.SwitchState(PlayerState.EndAnyAction);
        }

        switch (handSlot)
        {
            case 1:
                curHandSlot = UIManager.Instance.handSlotParent_1.GetComponent<DropItemSlot>(); ;
                break;
            case 2:
                curHandSlot = UIManager.Instance.handSlotParent_2.GetComponent<DropItemSlot>(); ;
                break;
        }

        if (curHandSlot.HasItemInSlot(out ItemSlotPrefab slotPrefab) && slotPrefab.curSlot.item is GunWeaponItemSO gun)
        {
            curPlayer.reloadTime = gun.reloadTime;
        }

        UIManager.Instance.UpdatePlayerStatus();

    }

    #endregion

    #region Game Phase
    public void SwitchPhase(GamePhase phase)
    {
        this.phase = phase;
        switch (phase)
        {
            case GamePhase.SelectMap:
                if (curPlayer != null)
                {
                    Destroy(curPlayer.gameObject);
                }

                InitCamera(transform);
                UIManager.Instance.ShowSelectMap();

                break;
            case GamePhase.GameStart:
                UIManager.Instance.HideSelectMap();
                InitPlayer();
                SwitchPhase(GamePhase.DuringGame);
                break;
            case GamePhase.DuringGame:
                UIManager.Instance.ShowPlayerStatusPanel();
                SelectHandSlot(1);
                break;
            case GamePhase.EndGame:
                UIManager.Instance.HideActionDuration();
                UIManager.Instance.HideInteractiveChoice();
                UIManager.Instance.HideInteractiveKey();
                SwitchPhase(GamePhase.SelectMap);
                curMapSelect = null;
                break;
        }
    }

    void UpdatePhase()
    {
        switch (phase)
        {
            case GamePhase.SelectMap:
                break;
            case GamePhase.GameStart:
                break;
            case GamePhase.DuringGame:
                break;
            case GamePhase.EndGame:
                break;
        }
    }

    public bool IsPhase(GamePhase phase)
    {
        return this.phase == phase;
    }

    #endregion

    #region Select Map

    public void SelectMap(MapTypeSO map)
    {
        curMapSelect = map;
    }

    #endregion

}
