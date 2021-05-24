using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Shogiman {
    public override bool[,] PossibleMove() {
        bool[,] r = new bool[9, 9];
        Shogiman c;

        if (isAttacker) {
            //Attacker team move
            if (CurrentY != 8) {
                if (CurrentX != 0) {
                    c = BoardManager.Instance.Shogimans[CurrentX - 1, CurrentY + 2];
                    if (c == null)
                        r[CurrentX - 1, CurrentY + 2] = true;
                    else if (isAttacker != c.isAttacker)
                        r[CurrentX - 1, CurrentY + 2] = true;
                }

                if (CurrentX != 8) {
                    c = BoardManager.Instance.Shogimans[CurrentX + 1, CurrentY + 2];
                    if (c == null)
                        r[CurrentX + 1, CurrentY + 2] = true;
                    else if (isAttacker != c.isAttacker)
                        r[CurrentX + 1, CurrentY + 2] = true;
                }
            }
        } else {
            //Defender team move
            if (CurrentY != 0) {
                if (CurrentX != 0) {
                    c = BoardManager.Instance.Shogimans[CurrentX - 1, CurrentY - 2];
                    if (c == null)
                        r[CurrentX - 1, CurrentY - 2] = true;
                    else if (isAttacker != c.isAttacker)
                        r[CurrentX - 1, CurrentY - 2] = true;
                }

                if (CurrentX != 8) {
                    c = BoardManager.Instance.Shogimans[CurrentX + 1, CurrentY - 2];
                    if (c == null)
                        r[CurrentX + 1, CurrentY - 2] = true;
                    else if (isAttacker != c.isAttacker)
                        r[CurrentX + 1, CurrentY - 2] = true;
                }
            }
        }

        return r;
    }
}