using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : Shogiman {
    public override bool[,] PossibleMove() {
        bool[,] r = new bool[9, 9];
        Shogiman c;
        int i;
        
        if (isAttacker) {
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
        } else {
            i = CurrentY;
            while (true) {
                
                i--;
                if (i <0)
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
        }
        

        return r;
    }
}
