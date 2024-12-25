using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [Header("===== Init On Game Start ======")]
    [SerializeField] GameObject personPrefab;
    [SerializeField] GameObject cameraPrefab;
    [Header("===== Player =====")]
    [HideInInspector] public bool isRunning;
    [Header("===== Input =====")]
    [SerializeField] LayerMask mousePosMask;
    [HideInInspector] public Vector2 mousePos;
    [HideInInspector] public Vector2 moveInput;

    private void Awake()
    {
        InitGame();
    }

    #region Init On Game Start

    void InitGame()
    {
        GameObject player = Instantiate(personPrefab, Vector3.zero, Quaternion.identity);
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
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

}
