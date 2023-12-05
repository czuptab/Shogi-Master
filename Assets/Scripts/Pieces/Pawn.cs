public class Pawn : ShogiPiece {
    public override bool[,] PossibleMove() {
        bool[,] r = new bool[9, 9];
        ShogiPiece c;

        //Attacker team move
        if(IsAttacker) {
            if(CurrentY != 8) {
                c = BoardController.Instance.ShogiPieces[CurrentX, CurrentY + 1];
                if(c == null || !c.IsAttacker) {
                    r[CurrentX, CurrentY + 1] = true;
                }
            }
        } else {
            //Defender team move
            if (CurrentY != 0) {
                c = BoardController.Instance.ShogiPieces[CurrentX, CurrentY - 1];
                if (c == null || c.IsAttacker) {
                    r[CurrentX, CurrentY - 1] = true;
                    
                }
            }
        }

        return r;
    }
}
