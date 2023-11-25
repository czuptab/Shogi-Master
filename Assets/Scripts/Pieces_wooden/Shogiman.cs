using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shogiman : MonoBehaviour {
    public int CurrentX { set; get; }
    public int CurrentY { set; get; }
    public bool isAttacker;
    public Animator animator;

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