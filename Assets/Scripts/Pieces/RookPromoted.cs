using DG.Tweening;
using UnityEngine;

public class RookPromoted : ShogiPiece
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[9, 9];
        ShogiPiece c;
        int i;

        //Right
        i = CurrentX;
        while (true)
        {
            i++;
            if (i >= 9)
                break;

            c = BoardManager.Instance.ShogiPieces[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                if (c.IsAttacker != IsAttacker)
                    r[i, CurrentY] = true;

                break;
            }
        }

        //Left
        i = CurrentX;
        while (true)
        {
            i--;
            if (i < 0)
                break;

            c = BoardManager.Instance.ShogiPieces[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                if (c.IsAttacker != IsAttacker)
                    r[i, CurrentY] = true;

                break;
            }
        }

        //Up
        i = CurrentY;
        while (true)
        {
            i++;
            if (i >= 9)
                break;

            c = BoardManager.Instance.ShogiPieces[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else
            {
                if (c.IsAttacker != IsAttacker)
                    r[CurrentX, i] = true;

                break;
            }
        }

        //Down
        i = CurrentY;
        while (true)
        {
            i--;
            if (i < 0)
                break;

            c = BoardManager.Instance.ShogiPieces[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else
            {
                if (c.IsAttacker != IsAttacker)
                    r[CurrentX, i] = true;

                break;
            }
        }

        // Top Left
        i = CurrentX;
        int j = CurrentY;
        i--;
        j++;
        if (i >= 0 && j < 9)
        {
            c = BoardManager.Instance.ShogiPieces[i, j];
            if (c == null || c.IsAttacker != IsAttacker)
                r[i, j] = true;
        }

        // Top Right
        i = CurrentX;
        j = CurrentY;
        i++;
        j++;
        if (i < 9 && j < 9)
        {
            c = BoardManager.Instance.ShogiPieces[i, j];
            if (c == null || c.IsAttacker != IsAttacker)
                r[i, j] = true;
        }

        // Down Left
        i = CurrentX;
        j = CurrentY;
        i--;
        j--;
        if (i >= 0 && j >= 0)
        {
            c = BoardManager.Instance.ShogiPieces[i, j];
            if (c == null || c.IsAttacker != IsAttacker)
                r[i, j] = true;
        }

        // Down Right
        i = CurrentX;
        j = CurrentY;
        i++;
        j--;
        if (i < 9 && j >= 0)
        {
            c = BoardManager.Instance.ShogiPieces[i, j];
            if (c == null || c.IsAttacker != IsAttacker)
                r[i, j] = true;
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