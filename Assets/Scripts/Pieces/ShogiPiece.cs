using UnityEngine;

public abstract class ShogiPiece : MonoBehaviour {
    public int CurrentX { set; get; }
    public int CurrentY { set; get; }
    public bool IsAttacker;
    public Animator animator;
    public PieceType pieceType;

    public void SetPosition(int x, int y) {
        CurrentX = x;
        CurrentY = y;
    }

    public void SelectPiece()
    {
        animator.SetTrigger("IsSelected");
    }
    

    public virtual bool[,] PossibleMove() {
        return new bool[9,9];
    }

    public abstract void Move(int x, int y, Vector3 tileCenter, float movementDuration);
}