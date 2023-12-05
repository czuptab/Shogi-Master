public class Bishop : ShogiPiece {

    public override bool[,] PossibleMove() {
        bool[,] r = new bool[9, 9];

        ShogiPiece c;
        int i, j;

        // Top Left
        i = CurrentX;
        j = CurrentY;
        while (true) {
            i--;
            j++;
            if (i < 0 || j >= 9)
                break;

            c = BoardController.Instance.ShogiPieces[i, j];
            if (c == null)
                r[i, j] = true;
            else {
                if (IsAttacker != c.IsAttacker)
                    r[i, j] = true;

                break;
            }
        }

        // Top Right
        i = CurrentX;
        j = CurrentY;
        while (true) {
            i++;
            j++;
            if (i >= 9 || j >= 9)
                break;

            c = BoardController.Instance.ShogiPieces[i, j];
            if (c == null)
                r[i, j] = true;
            else {
                if (IsAttacker != c.IsAttacker)
                    r[i, j] = true;

                break;
            }
        }

        // Down Left
        i = CurrentX;
        j = CurrentY;
        while (true) {
            i--;
            j--;
            if (i < 0 || j < 0)
                break;

            c = BoardController.Instance.ShogiPieces[i, j];
            if (c == null)
                r[i, j] = true;
            else {
                if (IsAttacker != c.IsAttacker)
                    r[i, j] = true;

                break;
            }
        }

        // Down Right
        i = CurrentX;
        j = CurrentY;
        while (true) {
            i++;
            j--;
            if (i >= 9 || j < 0)
                break;

            c = BoardController.Instance.ShogiPieces[i, j];
            if (c == null)
                r[i, j] = true;
            else {
                if (IsAttacker != c.IsAttacker)
                    r[i, j] = true;

                break;
            }
        }

        return r;
    }
}
