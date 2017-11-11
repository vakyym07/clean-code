namespace Chess
{
	public class ChessProblem
	{
		private static Board board;

		public static void LoadFrom(string[] lines)
		{
			board = new BoardParser().ParseBoard(lines);
		}

		// Определяет мат, шах или пат белым.
		public static ChessStatus CalculateChessStatus()
		{
			var isCheck = IsCheckForWhite();
			var hasMoves = false;
			foreach (var locationFrom in board.GetPieces(PieceColor.White))
			{
				foreach (var locationTo in board.GetPiece(locationFrom).GetMoves(locationFrom, board))
				{
					if (IsFigureHasMoves(locationFrom, locationTo))
                        hasMoves = true;
				}
			}

            return GetChessStatus(isCheck, hasMoves);
		}

	    private static ChessStatus GetChessStatus(bool isCheck, bool hasMoves)
	    {
	        if (isCheck)
	            return hasMoves ? ChessStatus.Check : ChessStatus.Mate;

	        return hasMoves ? ChessStatus.Ok : ChessStatus.Stalemate;
	    }

	    private static bool IsFigureHasMoves(Location locationFrom, Location locationTo)
	    {
	        using (board.PerformTemporaryMove(locationFrom, locationTo))
	            return !IsCheckForWhite();
	    }

		// check — это шах
		private static bool IsCheckForWhite()
		{
			foreach (var location in board.GetPieces(PieceColor.Black))
			{
				var piece = board.GetPiece(location);
				var moves = piece.GetMoves(location, board);
				foreach (var destination in moves)
				{
					if (board.GetPiece(destination).Is(PieceColor.White, PieceType.King))
						return true;
				}
			}

			return false;
		}
	}
}