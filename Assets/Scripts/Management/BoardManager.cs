using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    #region VARIABLES

    public static BoardManager Instance { set; get; }
    private bool[,] allowedMoves { set; get; }

    public ShogiPiece[,] ShogiPieces { set; get; }
    private ShogiPiece selectedShogiPiece;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    [SerializeField] private List<GameObject> shogiPrefabs;
    private List<GameObject> activeShogiPiece;

    public bool isAttackerTurn = true;

    public GameObject attackerWin;
    public GameObject defenderWin;

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
                if(selectedShogiPiece == null) {
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
        if (ShogiPieces[x, y] == null)
            return;

        if (ShogiPieces[x, y].IsAttacker != isAttackerTurn)
            return;

        bool hasAtleastOneMove = false;
        allowedMoves = ShogiPieces[x, y].PossibleMove();
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                if (allowedMoves[i, j])
                    hasAtleastOneMove = true;

        if (!hasAtleastOneMove)
            return;
   
        selectedShogiPiece = ShogiPieces[x, y];
        selectedShogiPiece.SelectPiece();
        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
    }

    private void MoveShogiman(int x, int y)
    {
        if (allowedMoves[x, y])
        {
            ShogiPiece c = ShogiPieces[x, y];

            if (c != null && c.IsAttacker != isAttackerTurn)
            {
                // TODO:Capture a piece into new list

                if (c.GetType() == typeof(King))
                {
                    EndGame();
                    return;
                }
                activeShogiPiece.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            ShogiPieces[selectedShogiPiece.CurrentX, selectedShogiPiece.CurrentY] = null;

            int distanceX = Mathf.Abs(x - selectedShogiPiece.CurrentX);
            int distanceY = Mathf.Abs(y - selectedShogiPiece.CurrentY);
            int totalDistance = distanceX + distanceY;

            float moveDuration = totalDistance * 0.5f;

            selectedShogiPiece.Move(x, y, GetTileCenter(x, y), moveDuration);
            
            BoardHighlights.Instance.HideHighlights();
        }
    }

    public void CompleteMovement(int x, int y)
    {
        selectedShogiPiece.SetPosition(x, y);
        ShogiPieces[x, y] = selectedShogiPiece;
        isAttackerTurn = !isAttackerTurn;

        selectedShogiPiece = null;
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

    private void SpawnShogiman(bool isAttacker, PieceType pieceType, int x, int y)
    {
        GameObject go = Instantiate(shogiPrefabs[(int)pieceType], GetTileCenter(x, y), isAttacker ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(-90, 0, 0));
        go.transform.SetParent(transform);
        go.GetComponent<ShogiPiece>().IsAttacker = isAttacker;
        ShogiPieces[x, y] = go.GetComponent<ShogiPiece>();
        ShogiPieces[x, y].SetPosition(x, y);
        activeShogiPiece.Add(go);
    }

    private void SpawnAllShogimans() {
        activeShogiPiece = new List<GameObject>();
        ShogiPieces = new ShogiPiece[9, 9];

        //Spawn attackers

        //King
        SpawnShogiman(true, PieceType.King, 4,0);

        //Golden Generals
        SpawnShogiman(true, PieceType.GoldGeneral, 3, 0);
        SpawnShogiman(true, PieceType.GoldGeneral, 5, 0);

        //Silver Generals
        SpawnShogiman(true, PieceType.SilverGeneral, 2, 0);
        SpawnShogiman(true, PieceType.SilverGeneral, 6, 0);

        //Knights
        SpawnShogiman(true, PieceType.Knight, 1, 0);
        SpawnShogiman(true, PieceType.Knight, 7, 0);

        //Lances
        SpawnShogiman(true, PieceType.Lance, 0, 0);
        SpawnShogiman(true, PieceType.Lance, 8, 0);

        //Rook and Bishop
        SpawnShogiman(true, PieceType.Rook, 1, 1);
        SpawnShogiman(true, PieceType.Bishop, 7, 1);

        //Pawns
        for(int i = 0; i < 9; i++) {
            SpawnShogiman(true, PieceType.Pawn, i, 2);
        }

        
        //Spawn defenders

        //King
        SpawnShogiman(false, PieceType.King, 4, 8);

        //Golden Generals
        SpawnShogiman(false, PieceType.GoldGeneral,3, 8);
        SpawnShogiman(false, PieceType.GoldGeneral, 5, 8);

        //Silver Generals
        SpawnShogiman(false, PieceType.SilverGeneral, 2, 8);
        SpawnShogiman(false, PieceType.SilverGeneral, 6, 8);

        //Knights
        SpawnShogiman(false, PieceType.Knight, 1, 8);
        SpawnShogiman(false, PieceType.Knight, 7, 8);

        //Lances
        SpawnShogiman(false, PieceType.Lance, 0, 8);
        SpawnShogiman(false, PieceType.Lance, 8, 8);

        //Rook and Bishop
        SpawnShogiman(false, PieceType.Rook, 7, 7);
        SpawnShogiman(false, PieceType.Bishop, 1, 7);

        //Pawns
        for (int i = 0; i < 9; i++) {
            SpawnShogiman(false, PieceType.Pawn, i, 6);
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
            GameObject win = Instantiate(attackerWin);
            Destroy(win, 2);
        } else {
            GameObject win = Instantiate(defenderWin);
            Destroy(win, 2);
        }

        foreach (GameObject go in activeShogiPiece)
            Destroy(go);

        isAttackerTurn = true;
        BoardHighlights.Instance.HideHighlights();
        SpawnAllShogimans();
    }
    
    #endregion
}