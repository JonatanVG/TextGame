using System.Text.RegularExpressions;

namespace TextGame.Utilites
{
  internal static class ActionResolver
  {
    private static readonly List<(Regex Pattern, Action<Match> Handler)> _handlers = [];

    public static void Register(string pattern, Action<Match> handler)
    {
      _handlers.Add((new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled), handler));
    }

    public static bool TryExecute(string action)
    {
      foreach (var (pattern, handler) in _handlers)
      {
        var match = pattern.Match(action);
        if (match.Success)
        {
          handler(match);
          return true;
        }
      }
      return false;
    }
  }
}
