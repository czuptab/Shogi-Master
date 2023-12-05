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

                c = BoardController.Instance.ShogiPieces[CurrentX, i];
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

                c = BoardController.Instance.ShogiPieces[CurrentX, i];
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
}
