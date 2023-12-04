using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    #region VARIABLES
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    public static BoardManager Instance { set; get; }
    public ShogiPiece[,] ShogiPieces { set; get; }

    [SerializeField] private List<GameObject> shogiPrefabs;
    [SerializeField] private GameObject attackerWin;
    [SerializeField] private GameObject defenderWin;
    [SerializeField] private CameraController cameraController;

    private bool[,] _allowedMoves { set; get; }
    private ShogiPiece _selectedShogiPiece;
    private int _selectionX = -1;
    private int _selectionY = -1;
    private List<GameObject> _activeShogiPieces;
    private bool isAttackerTurn = true;

    private Dictionary<PieceType, PieceType> _promotionMap = new()
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
        HandleInput();
    }

    private void HandleInput()
    {
        UpdateSelection();

        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftClick();
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick();
        }
    }

    private void HandleLeftClick()
    {
        if (IsValidSelection())
        {
            if (_selectedShogiPiece == null)
            {
                SelectShogiPiece(_selectionX, _selectionY);
            }
            else
            {
                MoveShogiPiece(_selectionX, _selectionY);
            }
        }
    }

    private void HandleRightClick()
    {
        if (IsValidSelection() && _selectedShogiPiece != null)
        {
            PromoteShogiPiece();
        }
    }

    private bool IsValidSelection()
    {
        return _selectionX >= 0 && _selectionY >= 0;
    }

    private void SelectShogiPiece(int x, int y) {
        if (ShogiPieces[x, y] == null)
            return;

        if (ShogiPieces[x, y].IsAttacker != isAttackerTurn)
            return;

        bool hasAtleastOneMove = false;
        _allowedMoves = ShogiPieces[x, y].PossibleMove();
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                if (_allowedMoves[i, j])
                    hasAtleastOneMove = true;

        if (!hasAtleastOneMove)
            return;
   
        _selectedShogiPiece = ShogiPieces[x, y];
        _selectedShogiPiece.SelectPiece();
        BoardHighlights.Instance.HighlightAllowedMoves(_allowedMoves);
    }

    private void MoveShogiPiece(int x, int y)
    {
        if (_allowedMoves[x, y])
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
                _activeShogiPieces.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            ShogiPieces[_selectedShogiPiece.CurrentX, _selectedShogiPiece.CurrentY] = null;

            int distanceX = Mathf.Abs(x - _selectedShogiPiece.CurrentX);
            int distanceY = Mathf.Abs(y - _selectedShogiPiece.CurrentY);
            double totalDistance = Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2));

            float moveDuration = (float)(totalDistance * 0.5f);

            _selectedShogiPiece.Move(x, y, GetTileCenter(x, y), moveDuration);
            
            BoardHighlights.Instance.HideHighlights();
        }
    }

    private void PromoteShogiPiece()
    {
        if(ShouldPromote(_selectedShogiPiece))
        {
            _activeShogiPieces.Remove(_selectedShogiPiece.gameObject);
            SpawnShogiPiece(_selectedShogiPiece.IsAttacker, _promotionMap[_selectedShogiPiece.PieceType], _selectedShogiPiece.CurrentX, _selectedShogiPiece.CurrentY);
            Destroy(_selectedShogiPiece.gameObject);
            BoardHighlights.Instance.HideHighlights();
        }
    }

    private bool ShouldPromote(ShogiPiece shogiPiece)
    {
        if (_promotionMap.ContainsKey(shogiPiece.PieceType) &&
           (shogiPiece.IsAttacker && shogiPiece.CurrentY >= 6) || (!shogiPiece.IsAttacker && shogiPiece.CurrentY <= 2) &&
           shogiPiece.IsAttacker == isAttackerTurn)
        {
            return true;
        }

        return false;
    }

    public void CompleteMovement(int x, int y)
    {
        _selectedShogiPiece.SetPosition(x, y);
        ShogiPieces[x, y] = _selectedShogiPiece;
        if (
          ((_selectedShogiPiece.PieceType == PieceType.Pawn || _selectedShogiPiece.PieceType == PieceType.Lance) &&
          (_selectedShogiPiece.IsAttacker && _selectedShogiPiece.CurrentY >= 8) || (!_selectedShogiPiece.IsAttacker && y <= 0)) ||
          (_selectedShogiPiece.PieceType == PieceType.Knight &&
          (_selectedShogiPiece.IsAttacker && _selectedShogiPiece.CurrentY >= 7) || (!_selectedShogiPiece.IsAttacker && y <= 1))
        )
        {
            PromoteShogiPiece();
        }

        isAttackerTurn = !isAttackerTurn;

        _selectedShogiPiece = null;
        cameraController.RotateCamera(180f);
    }

    private void UpdateSelection() {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ShogiPlane"))) {
            _selectionX = (int)hit.point.x;
            _selectionY = (int)hit.point.z;
        } else {
            _selectionX = -1;
            _selectionY = -1;
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
        _activeShogiPieces.Add(go);
    }

    private void SpawnAllShogiPieces() {
        _activeShogiPieces = new List<GameObject>();
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

        if(IsValidSelection()) {
            Debug.DrawLine(
                Vector3.forward * _selectionY + Vector3.right * _selectionX,
                Vector3.forward * (_selectionY + 1) + Vector3.right * (_selectionX + 1));

            Debug.DrawLine(
                Vector3.forward * (_selectionY + 1) + Vector3.right * _selectionX,
                Vector3.forward * _selectionY + Vector3.right * (_selectionX + 1));
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

        foreach (GameObject go in _activeShogiPieces)
            Destroy(go);

        isAttackerTurn = true;
        BoardHighlights.Instance.HideHighlights();
        SpawnAllShogiPieces();
    }
    
    #endregion
}