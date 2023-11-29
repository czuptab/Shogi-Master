﻿using DG.Tweening;
using UnityEngine;

public class King : ShogiPiece {
    public override bool[,] PossibleMove() {
        bool[,] r = new bool[9, 9];
        ShogiPiece c;
        int i, j;

        // Top Side
        i = CurrentX - 1;
        j = CurrentY + 1;
        if (CurrentY != 8) {
            for (int k = 0; k < 3; k++) {
                if (i >= 0 && i < 9) {
                    c = BoardManager.Instance.ShogiPieces[i, j];
                    if (c == null)
                        r[i, j] = true;
                    else if (IsAttacker != c.IsAttacker)
                        r[i, j] = true;
                }

                i++;
            }
        }

        // Down Side
        i = CurrentX - 1;
        j = CurrentY - 1;
        if (CurrentY != 0) {
            for (int k = 0; k < 3; k++) {
                if (i >= 0 && i < 9) {
                    c = BoardManager.Instance.ShogiPieces[i, j];
                    if (c == null)
                        r[i, j] = true;
                    else if (IsAttacker != c.IsAttacker)
                        r[i, j] = true;
                }

                i++;
            }
        }

        // Middle Left
        if (CurrentX != 0) {
            c = BoardManager.Instance.ShogiPieces[CurrentX - 1, CurrentY];
            if (c == null)
                r[CurrentX - 1, CurrentY] = true;
            else if (IsAttacker != c.IsAttacker)
                r[CurrentX - 1, CurrentY] = true;
        }

        // Middle Right
        if (CurrentX != 8) {
            c = BoardManager.Instance.ShogiPieces[CurrentX + 1, CurrentY];
            if (c == null)
                r[CurrentX + 1, CurrentY] = true;
            else if (IsAttacker != c.IsAttacker)
                r[CurrentX + 1, CurrentY] = true;
        }

        return r;
    }

    public override void Move(int x, int y, Vector3 tileCenter, float movementDuration)
    {
        transform.DOMove(tileCenter, movementDuration).SetEase(Ease.OutQuad)
            .OnComplete(() => {
                BoardManager.Instance.CompleteMovement(x, y);
            });
    }
}
