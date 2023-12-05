﻿using Assets.Scripts.Controllers;

namespace Assets.Scripts.Pieces
{
    public class SilverGeneral : ShogiPiece
    {
        public override bool[,] PossibleMove()
        {
            bool[,] r = new bool[9, 9];
            ShogiPiece c;
            int i, j;
            if (IsAttacker)
            {
                //Top side
                i = CurrentX - 1;
                j = CurrentY + 1;
                if (CurrentY != 8)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        if (i >= 0 && i < 9)
                        {
                            c = BoardController.Instance.ShogiPieces[i, j];
                            if (c == null)
                                r[i, j] = true;
                            else if (IsAttacker != c.IsAttacker)
                                r[i, j] = true;
                        }

                        i++;
                    }
                }

                // Down Side
                j = CurrentY - 1;
                if (CurrentY != 0)
                {
                    if (CurrentX > 0)
                    {
                        c = BoardController.Instance.ShogiPieces[CurrentX - 1, j];
                        if (c == null)
                            r[CurrentX - 1, j] = true;
                        else if (IsAttacker != c.IsAttacker)
                            r[CurrentX - 1, j] = true;
                    }

                    if (CurrentX < 8)
                    {
                        c = BoardController.Instance.ShogiPieces[CurrentX + 1, j];
                        if (c == null)
                            r[CurrentX + 1, j] = true;
                        else if (IsAttacker != c.IsAttacker)
                            r[CurrentX + 1, j] = true;
                    }
                }
            }
            else
            {
                // Down side
                i = CurrentX - 1;
                j = CurrentY - 1;
                if (CurrentY != 0)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        if (i >= 0 && i < 9)
                        {
                            c = BoardController.Instance.ShogiPieces[i, j];
                            if (c == null)
                                r[i, j] = true;
                            else if (IsAttacker != c.IsAttacker)
                                r[i, j] = true;
                        }

                        i++;
                    }
                }

                // Top Side
                j = CurrentY + 1;
                if (CurrentY != 8)
                {
                    if (CurrentX > 0)
                    {
                        c = BoardController.Instance.ShogiPieces[CurrentX - 1, j];
                        if (c == null)
                            r[CurrentX - 1, j] = true;
                        else if (IsAttacker != c.IsAttacker)
                            r[CurrentX - 1, j] = true;
                    }

                    if (CurrentX < 8)
                    {
                        c = BoardController.Instance.ShogiPieces[CurrentX + 1, j];
                        if (c == null)
                            r[CurrentX + 1, j] = true;
                        else if (IsAttacker != c.IsAttacker)
                            r[CurrentX + 1, j] = true;
                    }
                }
            }



            return r;
        }
    }

}