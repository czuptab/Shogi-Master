using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

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
    private List<GameObject> activeShogiPieces;

    public bool isAttackerTurn = true;

    public GameObject attackerWin;
    public GameObject defenderWin;

    public CameraController cameraController;

    public Dictionary<PieceType, PieceType> PromotionMap = new()
    {
        { PieceType.Lance, PieceType.GoldGeneral },
        { PieceType.Pawn, PieceType.GoldGeneral },
        { PieceType.SilverGeneral, PieceType.GoldGeneral },
        { PieceType.Knight, PieceType.GoldGeneral },
        { PieceType.Bishop, PieceType.BishopPromoted },
        { PieceType.Rook, PieceType.RookPromoted }
    };

    #endregion

    #region UNITY METHODS
    private void Start() {
        Instance = this;
        SpawnAllShogiPieces();
    }

    private void Update()
    {
        UpdateSelection();
        DrawShogiBoard();

        if(Input.GetMouseButtonDown(0)) {
            if(selectionX >= 0 && selectionY >= 0) {
                if(selectedShogiPiece == null) {
                    //Select the shogiman
                    SelectShogiPiece(selectionX, selectionY);
                } else {
                    //Move the shogiman
                    MoveShogiPiece(selectionX, selectionY);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (selectedShogiPiece != null)
                {
                    PromoteShogiPiece();
                }
            }
        }
    }

    private void SelectShogiPiece(int x, int y) {
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

    private void MoveShogiPiece(int x, int y)
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
                activeShogiPieces.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            ShogiPieces[selectedShogiPiece.CurrentX, selectedShogiPiece.CurrentY] = null;

            int distanceX = Mathf.Abs(x - selectedShogiPiece.CurrentX);
            int distanceY = Mathf.Abs(y - selectedShogiPiece.CurrentY);
            //todo: pitagoras, not sum
            int totalDistance = distanceX + distanceY;

            float moveDuration = totalDistance * 0.5f;

            selectedShogiPiece.Move(x, y, GetTileCenter(x, y), moveDuration);
            
            BoardHighlights.Instance.HideHighlights();
        }
    }

    private void PromoteShogiPiece()
    {
        if(ShouldPromote(selectedShogiPiece))
        {
            activeShogiPieces.Remove(selectedShogiPiece.gameObject);
            SpawnShogiPiece(selectedShogiPiece.IsAttacker, PromotionMap[selectedShogiPiece.PieceType], selectedShogiPiece.CurrentX, selectedShogiPiece.CurrentY);
            Destroy(selectedShogiPiece.gameObject);
            BoardHighlights.Instance.HideHighlights();
        }
    }

    private bool ShouldPromote(ShogiPiece shogiPiece)
    {
        if (PromotionMap.ContainsKey(shogiPiece.PieceType) &&
           (shogiPiece.IsAttacker && shogiPiece.CurrentY >= 6) || (!shogiPiece.IsAttacker && shogiPiece.CurrentY <= 2) &&
           shogiPiece.IsAttacker == isAttackerTurn)
        {
            return true;
        }

        return false;
    }

    public void CompleteMovement(int x, int y)
    {
        selectedShogiPiece.SetPosition(x, y);
        ShogiPieces[x, y] = selectedShogiPiece;
        if (
          ((selectedShogiPiece.PieceType == PieceType.Pawn || selectedShogiPiece.PieceType == PieceType.Lance) &&
          (selectedShogiPiece.IsAttacker && selectedShogiPiece.CurrentY >= 8) || (!selectedShogiPiece.IsAttacker && y <= 0)) ||
          (selectedShogiPiece.PieceType == PieceType.Knight &&
          (selectedShogiPiece.IsAttacker && selectedShogiPiece.CurrentY >= 7) || (!selectedShogiPiece.IsAttacker && y <= 1))
        )
        {
            PromoteShogiPiece();
        }

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

    private void SpawnShogiPiece(bool isAttacker, PieceType pieceType, int x, int y)
    {
        GameObject go = Instantiate(shogiPrefabs[(int)pieceType], GetTileCenter(x, y), isAttacker ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(-90, 0, 0));
        go.transform.SetParent(transform);
        ShogiPiece shogiPiece = go.GetComponent<ShogiPiece>();
        shogiPiece.IsAttacker = isAttacker;
        shogiPiece.PieceType = pieceType;
        ShogiPieces[x, y] = go.GetComponent<ShogiPiece>();
        ShogiPieces[x, y].SetPosition(x, y);
        activeShogiPieces.Add(go);
    }

    private void SpawnAllShogiPieces() {
        activeShogiPieces = new List<GameObject>();
        ShogiPieces = new ShogiPiece[9, 9];

        //Spawn attackers

        //King
        SpawnShogiPiece(true, PieceType.King, 4,0);

        //Golden Generals
        SpawnShogiPiece(true, PieceType.GoldGeneral, 3, 0);
        SpawnShogiPiece(true, PieceType.GoldGeneral, 5, 0);

        //Silver Generals
        SpawnShogiPiece(true, PieceType.SilverGeneral, 2, 0);
        SpawnShogiPiece(true, PieceType.SilverGeneral, 6, 0);

        //Knights
        SpawnShogiPiece(true, PieceType.Knight, 1, 0);
        SpawnShogiPiece(true, PieceType.Knight, 7, 0);

        //Lances
        SpawnShogiPiece(true, PieceType.Lance, 0, 0);
        SpawnShogiPiece(true, PieceType.Lance, 8, 0);

        //Rook and Bishop
        SpawnShogiPiece(true, PieceType.Rook, 1, 1);
        SpawnShogiPiece(true, PieceType.Bishop, 7, 1);

        //Pawns
        for(int i = 0; i < 9; i++) {
            SpawnShogiPiece(true, PieceType.Pawn, i, 2);
        }

        
        //Spawn defenders

        //King
        SpawnShogiPiece(false, PieceType.King, 4, 8);

        //Golden Generals
        SpawnShogiPiece(false, PieceType.GoldGeneral,3, 8);
        SpawnShogiPiece(false, PieceType.GoldGeneral, 5, 8);

        //Silver Generals
        SpawnShogiPiece(false, PieceType.SilverGeneral, 2, 8);
        SpawnShogiPiece(false, PieceType.SilverGeneral, 6, 8);

        //Knights
        SpawnShogiPiece(false, PieceType.Knight, 1, 8);
        SpawnShogiPiece(false, PieceType.Knight, 7, 8);

        //Lances
        SpawnShogiPiece(false, PieceType.Lance, 0, 8);
        SpawnShogiPiece(false, PieceType.Lance, 8, 8);

        //Rook and Bishop
        SpawnShogiPiece(false, PieceType.Rook, 7, 7);
        SpawnShogiPiece(false, PieceType.Bishop, 1, 7);

        //Pawns
        for (int i = 0; i < 9; i++) {
            SpawnShogiPiece(false, PieceType.Pawn, i, 6);
        }
    }

    private Vector3 GetTileCenter(int x, int y) {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;

        return origin;
    }

    private void DrawShogiBoard()
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

        foreach (GameObject go in activeShogiPieces)
            Destroy(go);

        isAttackerTurn = true;
        BoardHighlights.Instance.HideHighlights();
        SpawnAllShogiPieces();
    }
    
    #endregion
}