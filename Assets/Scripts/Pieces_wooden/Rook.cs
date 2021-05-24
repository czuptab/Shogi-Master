using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Shogiman {
    public override bool[,] PossibleMove() {
        bool[,] r = new bool[9, 9];
        Shogiman c;
        int i;

        //Right
        i = CurrentX;
        while(true) {
            i++;
            if (i >= 9)
                break;

            c = BoardManager.Instance.Shogimans[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else {
                if (c.isAttacker != isAttacker)
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

            c = BoardManager.Instance.Shogimans[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else {
                if (c.isAttacker != isAttacker)
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

            c = BoardManager.Instance.Shogimans[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else {
                if (c.isAttacker != isAttacker)
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

            c = BoardManager.Instance.Shogimans[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else {
                if (c.isAttacker != isAttacker)
                    r[CurrentX, i] = true;

                break;
            }
        }

        return r;
    }
}
