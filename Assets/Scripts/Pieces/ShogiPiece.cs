using Assets.Scripts.Controllers;
using Assets.Scripts.Enum;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Pieces
{
    public abstract class ShogiPiece : MonoBehaviour
    {
        public int CurrentX { get; set; }
        public int CurrentY { get; set; }
        public bool IsAttacker;
        public Animator animator;
        public PieceType PieceType;

        public void SetPosition(int x, int y)
        {
            CurrentX = x;
            CurrentY = y;
        }

        public void SelectPiece()
        {
            animator.SetTrigger("IsSelected");
        }


        public virtual bool[,] PossibleMove()
        {
            return new bool[9, 9];
        }

        public virtual void Move(int x, int y, Vector3 tileCenter, float movementDuration)
        {
            transform.DOMove(tileCenter, movementDuration).SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    BoardController.Instance.CompleteMovement(x, y);
                });
        }
    } 
}