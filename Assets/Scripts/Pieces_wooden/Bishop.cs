using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Shogiman {
    public override bool[,] PossibleMove() {
        bool[,] r = new bool[9, 9];

        Shogiman c;
        int i, j;

        // Top Left
        i = CurrentX;
        j = CurrentY;
        while (true) {
            i--;
            j++;
            if (i < 0 || j >= 9)
                break;

            c = BoardManager.Instance.Shogimans[i, j];
            if (c == null)
                r[i, j] = true;
            else {
                if (isAttacker != c.isAttacker)
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

            c = BoardManager.Instance.Shogimans[i, j];
            if (c == null)
                r[i, j] = true;
            else {
                if (isAttacker != c.isAttacker)
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

            c = BoardManager.Instance.Shogimans[i, j];
            if (c == null)
                r[i, j] = true;
            else {
                if (isAttacker != c.isAttacker)
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

            c = BoardManager.Instance.Shogimans[i, j];
            if (c == null)
                r[i, j] = true;
            else {
                if (isAttacker != c.isAttacker)
                    r[i, j] = true;

                break;
            }
        }

        return r;
    }
}
