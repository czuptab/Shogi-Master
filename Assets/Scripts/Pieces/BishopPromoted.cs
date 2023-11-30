using DG.Tweening;
using UnityEngine;

public class BishopPromoted : ShogiPiece
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[9, 9];

        ShogiPiece c;
        int i, j;

        // Top Left
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j++;
            if (i < 0 || j >= 9)
                break;

            c = BoardManager.Instance.ShogiPieces[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (IsAttacker != c.IsAttacker)
                    r[i, j] = true;

                break;
            }
        }

        // Top Right
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j++;
            if (i >= 9 || j >= 9)
                break;

            c = BoardManager.Instance.ShogiPieces[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (IsAttacker != c.IsAttacker)
                    r[i, j] = true;

                break;
            }
        }

        // Down Left
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j--;
            if (i < 0 || j < 0)
                break;

            c = BoardManager.Instance.ShogiPieces[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (IsAttacker != c.IsAttacker)
                    r[i, j] = true;

                break;
            }
        }

        // Down Right
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j--;
            if (i >= 9 || j < 0)
                break;

            c = BoardManager.Instance.ShogiPieces[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (IsAttacker != c.IsAttacker)
                    r[i, j] = true;

                break;
            }
        }

        // Up
        if (CurrentY != 8)
        {
            c = BoardManager.Instance.ShogiPieces[CurrentX, CurrentY + 1];
            if (c == null || IsAttacker != c.IsAttacker)
                r[CurrentX, CurrentY + 1] = true;
        }

        // Down
        if (CurrentY != 0)
        {
            c = BoardManager.Instance.ShogiPieces[CurrentX, CurrentY - 1];
            if (c == null || IsAttacker != c.IsAttacker)
                r[CurrentX, CurrentY - 1] = true;
        }

        // Left
        if (CurrentX != 0)
        {
            c = BoardManager.Instance.ShogiPieces[CurrentX - 1, CurrentY];
            if (c == null || IsAttacker != c.IsAttacker)
                r[CurrentX - 1, CurrentY] = true;
        }

        // Right
        if (CurrentX != 8)
        {
            c = BoardManager.Instance.ShogiPieces[CurrentX + 1, CurrentY];
            if (c == null || IsAttacker != c.IsAttacker)
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
