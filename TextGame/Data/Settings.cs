using System.Diagnostics.CodeAnalysis;

namespace TextGame.Data
{
  internal static class Settings
  {
    public static (int, int) ConsoleSize { get; private set; } = (100, 100);


    [MemberNotNull(
      nameof(ConsoleSize)
      )]

    public static void Initialize( (int, int) consoleSize )
    {
        ConsoleSize = consoleSize;
    }
  }
}
