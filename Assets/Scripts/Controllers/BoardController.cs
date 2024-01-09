using Assets.Scripts.Consts;
using Assets.Scripts.Enum;
using Assets.Scripts.Helpers;
using Assets.Scripts.Pieces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Controllers
{
    public class BoardController : MonoBehaviour
    {
        #region VARIABLES

        public static BoardController Instance { set; get; }
        public ShogiPiece[,] ShogiPieces { set; get; }

        [SerializeField] private CameraController cameraController;
        [SerializeField] private InputController inputController;
        [SerializeField] private List<PiecePrefabPair> shogiPrefabsList;
        [SerializeField] private GameObject attackerWin;
        [SerializeField] private GameObject defenderWin;

        private bool[,] _allowedMoves { set; get; }
        private ShogiPiece _selectedShogiPiece;
        private int _selectionX = -1;
        private int _selectionY = -1;
        private List<GameObject> _activeShogiPieces;
        private bool isAttackerTurn = true;
        private Dictionary<PieceType, GameObject> shogiPrefabsDictionary;

        private readonly Dictionary<PieceType, PieceType> _promotionMap = new()
        {
            { PieceType.Lance, PieceType.GoldGeneral },
            { PieceType.Pawn, PieceType.GoldGeneral },
            { PieceType.SilverGeneral, PieceType.GoldGeneral },
            { PieceType.Knight, PieceType.GoldGeneral },
            { PieceType.Bishop, PieceType.BishopPromoted },
            { PieceType.Rook, PieceType.RookPromoted }
        };

        #endregion

        #region METHODS
        private void Start()
        {
            Instance = this;
            SpawnAllShogiPieces();
        }

        private void Awake()
        {
            if (cameraController == null)
            {
                Debug.LogError("[BoardController] CameraController dependency is not set.");
                return;
            }
            if (inputController == null)
            {
                Debug.LogError("[BoardController] InputController dependency is not set.");
                return;
            }

            inputController.OnSelect += HandleSelectShogiPiece;
            inputController.OnPromote += HandlePromoteShogiPiece;
            InitializePrefabDictionary();
        }

        private void HandleSelectShogiPiece(Vector2Int boardPosition)
        {
            int x = boardPosition.x;
            int y = boardPosition.y;

            // If a piece is already selected, attempt to move it
            if (_selectedShogiPiece != null)
            {
                if (_allowedMoves[x, y])
                {
                    MoveShogiPiece(x, y);
                    //_selectedShogiPiece = null;
                    _allowedMoves = null;
                }
                return;
            }

            // If no piece is selected, handle selection
            ShogiPiece selectedPiece = ShogiPieces[x, y];
            if (selectedPiece == null || selectedPiece.IsAttacker != isAttackerTurn)
                return;

            bool hasAtLeastOneMove = CheckForPossibleMoves(selectedPiece);
            if (!hasAtLeastOneMove)
                return;

            _selectedShogiPiece = selectedPiece;
            _selectedShogiPiece.SelectPiece();
            HighlightController.Instance.HighlightAllowedMoves(_allowedMoves);
        }

        private bool CheckForPossibleMoves(ShogiPiece piece)
        {
            _allowedMoves = piece.PossibleMove();
            for (int i = 0; i < BoardConsts.BOARD_SIZE; i++)
            {
                for (int j = 0; j < BoardConsts.BOARD_SIZE; j++)
                {
                    if (_allowedMoves[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void HandlePromoteShogiPiece(Vector2Int boardPosition)
        {
            if (_selectedShogiPiece != null && ShouldPromote(_selectedShogiPiece))
            {
                _activeShogiPieces.Remove(_selectedShogiPiece.gameObject);
                SpawnShogiPiece(_selectedShogiPiece.IsAttacker, _promotionMap[_selectedShogiPiece.PieceType], _selectedShogiPiece.CurrentX, _selectedShogiPiece.CurrentY);
                Destroy(_selectedShogiPiece.gameObject);
                HighlightController.Instance.HideHighlights();
            }
        }

        private bool IsValidSelection()
        {
            return _selectionX >= 0 && _selectionY >= 0;
        }

        private void MoveShogiPiece(int x, int y)
        {
            if (_allowedMoves[x, y])
            {
                ShogiPiece c = ShogiPieces[x, y];

                if (c != null && c.IsAttacker != isAttackerTurn)
                {
                    // TODO:Capture a piece into new list

                    if (c.PieceType == PieceType.King)
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

                _selectedShogiPiece.Move(x, y, BoardHelpers.GetTileCenter(x, y), moveDuration);

                HighlightController.Instance.HideHighlights();
            }
        }

        private void InitializePrefabDictionary()
        {
            shogiPrefabsDictionary = new Dictionary<PieceType, GameObject>();
            foreach (PiecePrefabPair pair in shogiPrefabsList)
            {
                if (pair.prefab != null)
                {
                    shogiPrefabsDictionary[pair.pieceType] = pair.prefab;
                }
                else
                {
                    Debug.LogWarning("Prefab for " + pair.pieceType + " is not set.");
                }
            }
        }

        private void PromoteShogiPiece()
        {
            if (ShouldPromote(_selectedShogiPiece))
            {
                _activeShogiPieces.Remove(_selectedShogiPiece.gameObject);
                SpawnShogiPiece(_selectedShogiPiece.IsAttacker, _promotionMap[_selectedShogiPiece.PieceType], _selectedShogiPiece.CurrentX, _selectedShogiPiece.CurrentY);
                Destroy(_selectedShogiPiece.gameObject);
                HighlightController.Instance.HideHighlights();
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

        private void SpawnShogiPiece(bool isAttacker, PieceType pieceType, int x, int y)
        {
            GameObject go = Instantiate(shogiPrefabsDictionary[pieceType], BoardHelpers.GetTileCenter(x, y), isAttacker ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(-90, 0, 0));
            go.transform.SetParent(transform);
            ShogiPiece shogiPiece = go.GetComponent<ShogiPiece>();
            shogiPiece.IsAttacker = isAttacker;
            shogiPiece.PieceType = pieceType;
            ShogiPieces[x, y] = go.GetComponent<ShogiPiece>();
            ShogiPieces[x, y].SetPosition(x, y);
            _activeShogiPieces.Add(go);
        }

        private void SpawnAllShogiPieces()
        {
            _activeShogiPieces = new List<GameObject>();
            ShogiPieces = new ShogiPiece[BoardConsts.BOARD_SIZE, BoardConsts.BOARD_SIZE];

            //Spawn attackers

            //King
            SpawnShogiPiece(true, PieceType.King, 4, 0);

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
            for (int i = 0; i < BoardConsts.BOARD_SIZE; i++)
            {
                SpawnShogiPiece(true, PieceType.Pawn, i, 2);
            }


            //Spawn defenders

            //King
            SpawnShogiPiece(false, PieceType.King, 4, 8);

            //Golden Generals
            SpawnShogiPiece(false, PieceType.GoldGeneral, 3, 8);
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
            for (int i = 0; i < BoardConsts.BOARD_SIZE; i++)
            {
                SpawnShogiPiece(false, PieceType.Pawn, i, 6);
            }
        }
        private void DrawShogiBoard()
        {
            Vector3 widthLine = Vector3.right * BoardConsts.BOARD_SIZE;
            Vector3 heightLine = Vector3.forward * BoardConsts.BOARD_SIZE;

            for (int i = 0; i <= BoardConsts.BOARD_SIZE; i++)
            {
                Vector3 start = Vector3.forward * i;
                Debug.DrawLine(start, start + widthLine);
                for (int j = 0; j <= BoardConsts.BOARD_SIZE; j++)
                {
                    start = Vector3.right * j;
                    Debug.DrawLine(start, start + heightLine);
                }
            }

            if (IsValidSelection())
            {
                Debug.DrawLine(
                    Vector3.forward * _selectionY + Vector3.right * _selectionX,
                    Vector3.forward * (_selectionY + 1) + Vector3.right * (_selectionX + 1));

                Debug.DrawLine(
                    Vector3.forward * (_selectionY + 1) + Vector3.right * _selectionX,
                    Vector3.forward * _selectionY + Vector3.right * (_selectionX + 1));
            }
        }

        private void EndGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnDestroy()
        {
            if (inputController != null)
            {
                inputController.OnSelect -= HandleSelectShogiPiece;
                inputController.OnPromote -= HandlePromoteShogiPiece;
            }
        }

        #endregion
    }
}