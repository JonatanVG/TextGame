using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
