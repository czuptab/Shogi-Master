using Assets.Scripts.Controllers;

namespace Assets.Scripts.Pieces
{
    public class Rook : ShogiPiece
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

                c = BoardController.Instance.ShogiPieces[i, CurrentY];
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

                c = BoardController.Instance.ShogiPieces[i, CurrentY];
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

                c = BoardController.Instance.ShogiPieces[CurrentX, i];
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

                c = BoardController.Instance.ShogiPieces[CurrentX, i];
                if (c == null)
                    r[CurrentX, i] = true;
                else
                {
                    if (c.IsAttacker != IsAttacker)
                        r[CurrentX, i] = true;

                    break;
                }
            }

            return r;
        }
    }

}