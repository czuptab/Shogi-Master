using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silver_General : Shogiman {
    public override bool[,] PossibleMove() {
        bool[,] r = new bool[9, 9];
        Shogiman c;
        int i, j;
        if (isAttacker) {
            //Top side
            i = CurrentX - 1;
            j = CurrentY + 1;
            if (CurrentY != 8) {
                for (int k = 0; k < 3; k++) {
                    if (i >= 0 && i < 9) {
                        c = BoardManager.Instance.Shogimans[i, j];
                        if (c == null)
                            r[i, j] = true;
                        else if (isAttacker != c.isAttacker)
                            r[i, j] = true;
                    }

                    i++;
                }
            }

            // Down Side
            j = CurrentY - 1;
            if (CurrentY != 0) {
                if (CurrentX > 0) {
                    c = BoardManager.Instance.Shogimans[CurrentX - 1, j];
                    if (c == null)
                        r[CurrentX - 1, j] = true;
                    else if (isAttacker != c.isAttacker)
                        r[CurrentX - 1, j] = true;
                }

                if (CurrentX < 8) {
                    c = BoardManager.Instance.Shogimans[CurrentX + 1, j];
                    if (c == null)
                        r[CurrentX + 1, j] = true;
                    else if (isAttacker != c.isAttacker)
                        r[CurrentX + 1, j] = true;
                }
            }
        } else {
            // Down side
            i = CurrentX - 1;
            j = CurrentY - 1;
            if (CurrentY != 0) {
                for (int k = 0; k < 3; k++) {
                    if (i >= 0 && i < 9) {
                        c = BoardManager.Instance.Shogimans[i, j];
                        if (c == null)
                            r[i, j] = true;
                        else if (isAttacker != c.isAttacker)
                            r[i, j] = true;
                    }

                    i++;
                }
            }

            // Top Side
            j = CurrentY + 1;
            if (CurrentY != 8) {
                if (CurrentX > 0) {
                    c = BoardManager.Instance.Shogimans[CurrentX - 1, j];
                    if (c == null)
                        r[CurrentX - 1, j] = true;
                    else if (isAttacker != c.isAttacker)
                        r[CurrentX - 1, j] = true;
                }

                if (CurrentX < 8) {
                    c = BoardManager.Instance.Shogimans[CurrentX + 1, j];
                    if (c == null)
                        r[CurrentX + 1, j] = true;
                    else if (isAttacker != c.isAttacker)
                        r[CurrentX + 1, j] = true;
                }
            }
        }

        

        return r;
    }

    public override void Move(int x, int y, Vector3 tileCenter)
    {
        transform.DOMove(tileCenter, 0.5f).SetEase(Ease.OutQuad)
            .OnComplete(() => {
                BoardManager.Instance.CompleteMovement(this, x, y);
            });
    }
}
