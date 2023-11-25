using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Shogiman {
    public override bool[,] PossibleMove() {
        bool[,] r = new bool[9, 9];
        Shogiman c;

        //Attacker team move
        if(isAttacker) {
            if(CurrentY != 8) {
                c = BoardManager.Instance.Shogimans[CurrentX, CurrentY + 1];
                if(c == null || !c.isAttacker) {
                    r[CurrentX, CurrentY + 1] = true;
                }
            }
        } else {
            //Defender team move
            if (CurrentY != 0) {
                c = BoardManager.Instance.Shogimans[CurrentX, CurrentY - 1];
                if (c == null || c.isAttacker) {
                    r[CurrentX, CurrentY - 1] = true;
                    
                }
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
