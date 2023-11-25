using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

public class BoardManager : MonoBehaviour {
    #region VARIABLES

    public static BoardManager Instance { set; get; }
    private bool[,] allowedMoves { set; get; }

    public Shogiman[,] Shogimans { set; get; }
    private Shogiman selectedShogiman;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> shogimanPrefabs;
    private List<GameObject> activeShogiman;

    public bool isAttackerTurn = true;

    public GameObject attacker_win;
    public GameObject defender_win;

    public CameraController cameraController;

    #endregion

    #region UNITY METHODS
    private void Start() {
        Instance = this;
        SpawnAllShogimans();
    }

    private void Update()
    {
        UpdateSelection();
        DrawChessBoard();

        if(Input.GetMouseButtonDown(0)) {
            if(selectionX >= 0 && selectionY >= 0) {
                if(selectedShogiman == null) {
                    //Select the shogiman
                    SelectShogiman(selectionX, selectionY);
                } else {
                    //Move the shogiman
                    MoveShogiman(selectionX, selectionY);
                }
            }
        }
    }

    private void SelectShogiman(int x, int y) {
        if (Shogimans[x, y] == null)
            return;

        if (Shogimans[x, y].isAttacker != isAttackerTurn)
            return;

        bool hasAtleastOneMove = false;
        allowedMoves = Shogimans[x, y].PossibleMove();
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                if (allowedMoves[i, j])
                    hasAtleastOneMove = true;

        if (!hasAtleastOneMove)
            return;
   
        selectedShogiman = Shogimans[x, y];
        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
    }

    private void MoveShogiman(int x, int y)
    {
        if (allowedMoves[x, y])
        {
            Shogiman c = Shogimans[x, y];

            if (c != null && c.isAttacker != isAttackerTurn)
            {
                // Capture a piece

                if (c.GetType() == typeof(King))
                {
                    EndGame();
                    return;
                }
                activeShogiman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            Shogimans[selectedShogiman.CurrentX, selectedShogiman.CurrentY] = null;

            selectedShogiman.Move(x, y, GetTileCenter(x, y));
            
            BoardHighlights.Instance.HideHighlights();
        }
    }

    public void CompleteMovement(Shogiman shogiman, int x, int y)
    {
        selectedShogiman.SetPosition(x, y);
        Shogimans[x, y] = selectedShogiman;
        isAttackerTurn = !isAttackerTurn;

        selectedShogiman = null;
        cameraController.RotateCamera(180f);
    }

    private void UpdateSelection() {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ShogiPlane"))) {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        } else {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void SpawnAttackerShogiman(int index, int x, int y) {
        
        GameObject go = Instantiate(shogimanPrefabs[index], GetTileCenter(x, y), Quaternion.Euler(-90, 180, 0));
        go.transform.SetParent(transform);
        Shogimans[x, y] = go.GetComponent<Shogiman>();
        Shogimans[x, y].SetPosition(x, y);
        activeShogiman.Add(go);
    }

    private void SpawnDefenderShogiman(int index, int x, int y) {

        GameObject go = Instantiate(shogimanPrefabs[index], GetTileCenter(x,y), Quaternion.Euler(-90, 0, 0));
        go.transform.SetParent(transform);
        Shogimans[x, y] = go.GetComponent<Shogiman>();
        Shogimans[x, y].SetPosition(x, y);
        activeShogiman.Add(go);
    }

    private void SpawnAllShogimans() {
        activeShogiman = new List<GameObject>();
        Shogimans = new Shogiman[9, 9];

        //Spawn attackers

        //King
        SpawnAttackerShogiman(0, 4,0);

        //Golden Generals
        SpawnAttackerShogiman(1, 3, 0);
        SpawnAttackerShogiman(1, 5, 0);

        //Silver Generals
        SpawnAttackerShogiman(2, 2, 0);
        SpawnAttackerShogiman(2, 6, 0);

        //Knights
        SpawnAttackerShogiman(3, 1, 0);
        SpawnAttackerShogiman(3, 7, 0);

        //Lances
        SpawnAttackerShogiman(4, 0, 0);
        SpawnAttackerShogiman(4, 8, 0);

        //Rook and Bishop
        SpawnAttackerShogiman(5, 1, 1);
        SpawnAttackerShogiman(6, 7, 1);

        //Pawns
        for(int i = 0; i < 9; i++) {
            SpawnAttackerShogiman(7, i, 2);
        }

        
        //Spawn defenders

        //King
        SpawnDefenderShogiman(10, 4, 8);

        //Golden Generals
        SpawnDefenderShogiman(11,3, 8);
        SpawnDefenderShogiman(11, 5, 8);

        //Silver Generals
        SpawnDefenderShogiman(12, 2, 8);
        SpawnDefenderShogiman(12, 6, 8);

        //Knights
        SpawnDefenderShogiman(13, 1, 8);
        SpawnDefenderShogiman(13, 7, 8);

        //Lances
        SpawnDefenderShogiman(14, 0, 8);
        SpawnDefenderShogiman(14, 8, 8);

        //Rook and Bishop
        SpawnDefenderShogiman(15, 7, 7);
        SpawnDefenderShogiman(16, 1, 7);

        //Pawns
        for (int i = 0; i < 9; i++) {
            SpawnDefenderShogiman(17, i, 6);
        }
    }

    private Vector3 GetTileCenter(int x, int y) {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;

        return origin;
    }

    private void DrawChessBoard()
    {
        Vector3 widthLine = Vector3.right * 9;
        Vector3 heightLine = Vector3.forward * 9;

        for (int i = 0; i <= 9; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 9; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        // Draw the selection
        if(selectionX >=0 && selectionY >=0) {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

            Debug.DrawLine(
                Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }

    private void EndGame() {
        if(isAttackerTurn) {
            GameObject win = Instantiate(attacker_win);
            Destroy(win, 2);
        } else {
            GameObject win = Instantiate(defender_win);
            Destroy(win, 2);
        }

        foreach (GameObject go in activeShogiman)
            Destroy(go);

        isAttackerTurn = true;
        BoardHighlights.Instance.HideHighlights();
        SpawnAllShogimans();
    }
    
    #endregion
}