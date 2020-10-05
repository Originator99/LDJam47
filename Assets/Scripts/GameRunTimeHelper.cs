using System;
using System.Linq;
using System.Text;

public static class GameRunTimeHelper {
    public static bool GameOver;
    public static float CurrentTimer;

    public static string GetUniqueID() {
		StringBuilder builder = new StringBuilder();
		Enumerable
		   .Range(65, 26)
			.Select(e => ((char)e).ToString())
			.Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
			.Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
			.OrderBy(e => Guid.NewGuid())
			.Take(11)
			.ToList().ForEach(e => builder.Append(e));
		return builder.ToString();
	}
}
