using DG.Tweening;
using UnityEngine;

public class Lance : ShogiPiece {
    public override bool[,] PossibleMove() {
        bool[,] r = new bool[9, 9];
        ShogiPiece c;
        int i;
        
        if (IsAttacker) {
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
        } else {
            i = CurrentY;
            while (true) {
                
                i--;
                if (i <0)
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
        }
        

        return r;
    }

    public override void Move(int x, int y, Vector3 tileCenter, float movementDuration)
    {
        animator.SetBool("IsMoving", true);
        transform.DOMove(tileCenter, movementDuration).SetEase(Ease.OutQuad)
            .OnComplete(() => {
                BoardManager.Instance.CompleteMovement(x, y);
                animator.SetBool("IsMoving", false);
            });
    }
}
