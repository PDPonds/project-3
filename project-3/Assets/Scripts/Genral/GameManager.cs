using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [Header("===== Init On Game Start ======")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject cameraPrefab;
    [Header("===== Player =====")]
    public InventorySO playerInventory;
    [HideInInspector] public PlayerManager curPlayer;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public DropItemSlot curHandSlot;
    [Header("===== Player Interactive =====")]
    [HideInInspector] public GameObject curInteractiveObj;
    [Header("===== Input =====")]
    [SerializeField] LayerMask mousePosMask;
    [HideInInspector] public Vector2 mousePos;
    [HideInInspector] public Vector2 moveInput;

    private void Awake()
    {
        InitGame();
    }

    private void Start()
    {
        SelectHandSlot(1);
    }


    #region Init On Game Start

    void InitGame()
    {
        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        curPlayer = playerManager;
        playerManager.Setup();

        GameObject camera = Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);
        CameraController camControl = camera.GetComponent<CameraController>();
        camControl.Setup(player.transform);
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
        switch (handSlot)
        {
            case 1:
                curHandSlot = UIManager.Instance.handSlotParent_1.GetComponent<DropItemSlot>(); ;
                break;
            case 2:
                curHandSlot = UIManager.Instance.handSlotParent_2.GetComponent<DropItemSlot>(); ;
                break;
        }

        UIManager.Instance.UpdatePlayerStatus();

    }

    #endregion

}
