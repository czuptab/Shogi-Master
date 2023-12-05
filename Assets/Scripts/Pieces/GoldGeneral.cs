public class GoldGeneral : ShogiPiece {
    public override bool[,] PossibleMove() {
        bool[,] r = new bool[9, 9];
        ShogiPiece c;
        int i, j;
        if (IsAttacker) {
            //Top side
            i = CurrentX - 1;
            j = CurrentY + 1;
            if (CurrentY != 8) {
                for (int k = 0; k < 3; k++) {
                    if (i >= 0 && i < 9) {
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
            if (CurrentY != 0) {
                c = BoardController.Instance.ShogiPieces[CurrentX, j];
                if (c == null)
                    r[CurrentX, j] = true;
                else if (IsAttacker != c.IsAttacker)
                    r[CurrentX, j] = true;
            }

        } else {
            // Down side
            i = CurrentX - 1;
            j = CurrentY - 1;
            if (CurrentY != 0) {
                for (int k = 0; k < 3; k++) {
                    if (i >= 0 && i < 9) {
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
            if (CurrentY != 8) {
                c = BoardController.Instance.ShogiPieces[CurrentX, j];
                if (c == null)
                    r[CurrentX, j] = true;
                else if (IsAttacker != c.IsAttacker)
                    r[CurrentX, j] = true;
            }
        }

        // Middle Left
        if (CurrentX != 0) {
            c = BoardController.Instance.ShogiPieces[CurrentX - 1, CurrentY];
            if (c == null)
                r[CurrentX - 1, CurrentY] = true;
            else if (IsAttacker != c.IsAttacker)
                r[CurrentX - 1, CurrentY] = true;
        }

        // Middle Right
        if (CurrentX != 8) {
            c = BoardController.Instance.ShogiPieces[CurrentX + 1, CurrentY];
            if (c == null)
                r[CurrentX + 1, CurrentY] = true;
            else if (IsAttacker != c.IsAttacker)
                r[CurrentX + 1, CurrentY] = true;
        }

        return r;
    }
}
