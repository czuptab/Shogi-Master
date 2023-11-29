using DG.Tweening;
using UnityEngine;

public class Knight : ShogiPiece {
    public override bool[,] PossibleMove() {
        bool[,] r = new bool[9, 9];
        ShogiPiece c;

        if (IsAttacker) {
            //Attacker team move
            if (CurrentY != 8) {
                if (CurrentX != 0) {
                    c = BoardManager.Instance.ShogiPieces[CurrentX - 1, CurrentY + 2];
                    if (c == null)
                        r[CurrentX - 1, CurrentY + 2] = true;
                    else if (IsAttacker != c.IsAttacker)
                        r[CurrentX - 1, CurrentY + 2] = true;
                }

                if (CurrentX != 8) {
                    c = BoardManager.Instance.ShogiPieces[CurrentX + 1, CurrentY + 2];
                    if (c == null)
                        r[CurrentX + 1, CurrentY + 2] = true;
                    else if (IsAttacker != c.IsAttacker)
                        r[CurrentX + 1, CurrentY + 2] = true;
                }
            }
        } else {
            //Defender team move
            if (CurrentY != 0) {
                if (CurrentX != 0) {
                    c = BoardManager.Instance.ShogiPieces[CurrentX - 1, CurrentY - 2];
                    if (c == null)
                        r[CurrentX - 1, CurrentY - 2] = true;
                    else if (IsAttacker != c.IsAttacker)
                        r[CurrentX - 1, CurrentY - 2] = true;
                }

                if (CurrentX != 8) {
                    c = BoardManager.Instance.ShogiPieces[CurrentX + 1, CurrentY - 2];
                    if (c == null)
                        r[CurrentX + 1, CurrentY - 2] = true;
                    else if (IsAttacker != c.IsAttacker)
                        r[CurrentX + 1, CurrentY - 2] = true;
                }
            }
        }

        return r;
    }

    public override void Move(int x, int y, Vector3 tileCenter, float movementDuration)
    {
        Vector3 jumpMidpoint = (transform.position + tileCenter) / 2;
        jumpMidpoint.y += 2;

        transform.DOPath(new Vector3[] { transform.position, jumpMidpoint, tileCenter }, movementDuration)
        .SetEase(Ease.InOutQuad)
        .OnComplete(() => {
            BoardManager.Instance.CompleteMovement(this, x, y);
        });
    }
}