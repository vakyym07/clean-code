using System;
using System.IO;
using NUnit.Framework;

namespace Chess
{
	[TestFixture]
	public class ChessProblem_Test
	{
		private static void TestOnFile(string filename)
		{
			var board = File.ReadAllLines(filename);
			ChessProblem.LoadFrom(board);
			var expectedAnswer = File.ReadAllText(Path.ChangeExtension(filename, ".ans")).Trim();
			var status = ChessProblem.CalculateChessStatus();
			Assert.AreEqual(expectedAnswer, status.ToString().ToLower(), "Failed test " + filename);
		}

		[Test]
		public void RepeatedMethodCallDoNotChangeBehaviour()
		{
			var board = new[]
			{
				"        ",
				"        ",
				"        ",
				"   q    ",
				"    K   ",
				" Q      ",
				"        ",
				"        ",
			};
			ChessProblem.LoadFrom(board);
			var status = ChessProblem.CalculateChessStatus();
			Assert.AreEqual(ChessStatus.Check, status);
			
			// Now check that internal board modifictions during the first call do not change answer
			status = ChessProblem.CalculateChessStatus();
			Assert.AreEqual(ChessStatus.Check, status);
		}

		[Test]
		public void FullTests()
		{
			var dir = TestContext.CurrentContext.TestDirectory;
			var testsCount = 0;
			foreach (var filename in Directory.GetFiles(Path.Combine(dir, "ChessTests"), "*.in"))
			{
				TestOnFile(filename);
				testsCount++;
			}
			Console.WriteLine("Tests passed: " + testsCount);
		}
	}
}