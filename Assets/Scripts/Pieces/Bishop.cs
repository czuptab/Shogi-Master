using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ShogiPiece {

    public override bool[,] PossibleMove() {
        bool[,] r = new bool[9, 9];

        ShogiPiece c;
        int i, j;

        // Top Left
        i = CurrentX;
        j = CurrentY;
        while (true) {
            i--;
            j++;
            if (i < 0 || j >= 9)
                break;

            c = BoardManager.Instance.ShogiPieces[i, j];
            if (c == null)
                r[i, j] = true;
            else {
                if (IsAttacker != c.IsAttacker)
                    r[i, j] = true;

                break;
            }
        }

        // Top Right
        i = CurrentX;
        j = CurrentY;
        while (true) {
            i++;
            j++;
            if (i >= 9 || j >= 9)
                break;

            c = BoardManager.Instance.ShogiPieces[i, j];
            if (c == null)
                r[i, j] = true;
            else {
                if (IsAttacker != c.IsAttacker)
                    r[i, j] = true;

                break;
            }
        }

        // Down Left
        i = CurrentX;
        j = CurrentY;
        while (true) {
            i--;
            j--;
            if (i < 0 || j < 0)
                break;

            c = BoardManager.Instance.ShogiPieces[i, j];
            if (c == null)
                r[i, j] = true;
            else {
                if (IsAttacker != c.IsAttacker)
                    r[i, j] = true;

                break;
            }
        }

        // Down Right
        i = CurrentX;
        j = CurrentY;
        while (true) {
            i++;
            j--;
            if (i >= 9 || j < 0)
                break;

            c = BoardManager.Instance.ShogiPieces[i, j];
            if (c == null)
                r[i, j] = true;
            else {
                if (IsAttacker != c.IsAttacker)
                    r[i, j] = true;

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
