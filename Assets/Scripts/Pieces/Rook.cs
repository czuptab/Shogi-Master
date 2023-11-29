using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ShogiPiece {
    public override bool[,] PossibleMove() {
        bool[,] r = new bool[9, 9];
        ShogiPiece c;
        int i;

        //Right
        i = CurrentX;
        while(true) {
            i++;
            if (i >= 9)
                break;

            c = BoardManager.Instance.ShogiPieces[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else {
                if (c.IsAttacker != IsAttacker)
                    r[i, CurrentY] = true;

                break;
            } 
        }

        //Left
        i = CurrentX;
        while (true) {
            i--;
            if (i < 0)
                break;

            c = BoardManager.Instance.ShogiPieces[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else {
                if (c.IsAttacker != IsAttacker)
                    r[i, CurrentY] = true;

                break;
            }
        }

        //Up
        i = CurrentY;
        while (true) {
            i++;
            if (i >= 9)
                break;

            c = BoardManager.Instance.ShogiPieces[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else {
                if (c.IsAttacker != IsAttacker)
                    r[CurrentX, i] = true;

                break;
            }
        }

        //Down
        i = CurrentY;
        while (true) {
            i--;
            if (i < 0)
                break;

            c = BoardManager.Instance.ShogiPieces[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else {
                if (c.IsAttacker != IsAttacker)
                    r[CurrentX, i] = true;

                break;
            }
        }

        return r;
    }

    public override void Move(int x, int y, Vector3 tileCenter, float movementDuration)
    {
        transform.DOMove(tileCenter, movementDuration).SetEase(Ease.OutQuad)
            .OnComplete(() => {
                BoardManager.Instance.CompleteMovement(this, x, y);
            });
    }
}
