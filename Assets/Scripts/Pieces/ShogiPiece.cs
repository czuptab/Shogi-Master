using UnityEngine;

public abstract class ShogiPiece : MonoBehaviour {
    public int CurrentX { get; set; }
    public int CurrentY { get; set; }
    public bool IsAttacker;
    public Animator animator;
    public PieceType PieceType;

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