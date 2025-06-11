// Decompiled with JetBrains decompiler
// Type: NLog.Targets.ColoredConsoleAnsiPrinter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Conditions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.Targets;

internal class ColoredConsoleAnsiPrinter : IColoredConsolePrinter
{
  public TextWriter AcquireTextWriter(TextWriter consoleStream, StringBuilder reusableBuilder)
  {
    return (TextWriter) new StringWriter(reusableBuilder ?? new StringBuilder(50), consoleStream.FormatProvider);
  }

  public void ReleaseTextWriter(
    TextWriter consoleWriter,
    TextWriter consoleStream,
    ConsoleColor? oldForegroundColor,
    ConsoleColor? oldBackgroundColor,
    bool flush)
  {
    StringBuilder stringBuilder = consoleWriter is StringWriter stringWriter ? stringWriter.GetStringBuilder() : (StringBuilder) null;
    if (stringBuilder == null)
      return;
    stringBuilder.Append(ColoredConsoleAnsiPrinter.TerminalDefaultColorEscapeCode);
    ConsoleTargetHelper.WriteLineThreadSafe(consoleStream, stringBuilder.ToString(), flush);
  }

  public ConsoleColor? ChangeForegroundColor(
    TextWriter consoleWriter,
    ConsoleColor? foregroundColor,
    ConsoleColor? oldForegroundColor = null)
  {
    if (foregroundColor.HasValue)
      consoleWriter.Write(ColoredConsoleAnsiPrinter.GetForegroundColorEscapeCode(foregroundColor.Value));
    return new ConsoleColor?();
  }

  public ConsoleColor? ChangeBackgroundColor(
    TextWriter consoleWriter,
    ConsoleColor? backgroundColor,
    ConsoleColor? oldBackgroundColor = null)
  {
    if (backgroundColor.HasValue)
      consoleWriter.Write(ColoredConsoleAnsiPrinter.GetBackgroundColorEscapeCode(backgroundColor.Value));
    return new ConsoleColor?();
  }

  public void ResetDefaultColors(
    TextWriter consoleWriter,
    ConsoleColor? foregroundColor,
    ConsoleColor? backgroundColor)
  {
    consoleWriter.Write(ColoredConsoleAnsiPrinter.TerminalDefaultColorEscapeCode);
  }

  public void WriteSubString(TextWriter consoleWriter, string text, int index, int endIndex)
  {
    for (int index1 = index; index1 < endIndex; ++index1)
      consoleWriter.Write(text[index1]);
  }

  public void WriteChar(TextWriter consoleWriter, char text) => consoleWriter.Write(text);

  public void WriteLine(TextWriter consoleWriter, string text) => consoleWriter.Write(text);

  private static string GetForegroundColorEscapeCode(ConsoleColor color)
  {
    switch (color)
    {
      case ConsoleColor.Black:
        return "\u001B[30m";
      case ConsoleColor.DarkBlue:
        return "\u001B[34m";
      case ConsoleColor.DarkGreen:
        return "\u001B[32m";
      case ConsoleColor.DarkCyan:
        return "\u001B[36m";
      case ConsoleColor.DarkRed:
        return "\u001B[31m";
      case ConsoleColor.DarkMagenta:
        return "\u001B[35m";
      case ConsoleColor.DarkYellow:
        return "\u001B[33m";
      case ConsoleColor.Gray:
        return "\u001B[37m";
      case ConsoleColor.DarkGray:
        return "\u001B[90m";
      case ConsoleColor.Blue:
        return "\u001B[94m";
      case ConsoleColor.Green:
        return "\u001B[92m";
      case ConsoleColor.Cyan:
        return "\u001B[96m";
      case ConsoleColor.Red:
        return "\u001B[91m";
      case ConsoleColor.Magenta:
        return "\u001B[95m";
      case ConsoleColor.Yellow:
        return "\u001B[93m";
      case ConsoleColor.White:
        return "\u001B[97m";
      default:
        return ColoredConsoleAnsiPrinter.TerminalDefaultForegroundColorEscapeCode;
    }
  }

  private static string TerminalDefaultForegroundColorEscapeCode => "\u001B[39m\u001B[22m";

  private static string GetBackgroundColorEscapeCode(ConsoleColor color)
  {
    switch (color)
    {
      case ConsoleColor.Black:
        return "\u001B[40m";
      case ConsoleColor.DarkBlue:
        return "\u001B[44m";
      case ConsoleColor.DarkGreen:
        return "\u001B[42m";
      case ConsoleColor.DarkCyan:
        return "\u001B[46m";
      case ConsoleColor.DarkRed:
        return "\u001B[41m";
      case ConsoleColor.DarkMagenta:
        return "\u001B[45m";
      case ConsoleColor.DarkYellow:
        return "\u001B[43m";
      case ConsoleColor.Gray:
        return "\u001B[47m";
      case ConsoleColor.DarkGray:
        return "\u001B[100m";
      case ConsoleColor.Blue:
        return "\u001B[104m";
      case ConsoleColor.Green:
        return "\u001B[102m";
      case ConsoleColor.Cyan:
        return "\u001B[106m";
      case ConsoleColor.Red:
        return "\u001B[101m";
      case ConsoleColor.Magenta:
        return "\u001B[105m";
      case ConsoleColor.Yellow:
        return "\u001B[103m";
      case ConsoleColor.White:
        return "\u001B[107m";
      default:
        return ColoredConsoleAnsiPrinter.TerminalDefaultBackgroundColorEscapeCode;
    }
  }

  private static string TerminalDefaultBackgroundColorEscapeCode => "\u001B[49m";

  private static string TerminalDefaultColorEscapeCode => "\u001B[0m";

  public IList<ConsoleRowHighlightingRule> DefaultConsoleRowHighlightingRules { get; } = (IList<ConsoleRowHighlightingRule>) new List<ConsoleRowHighlightingRule>()
  {
    new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Fatal", ConsoleOutputColor.DarkRed, ConsoleOutputColor.NoChange),
    new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Error", ConsoleOutputColor.DarkYellow, ConsoleOutputColor.NoChange),
    new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Warn", ConsoleOutputColor.DarkMagenta, ConsoleOutputColor.NoChange),
    new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Info", ConsoleOutputColor.NoChange, ConsoleOutputColor.NoChange),
    new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Debug", ConsoleOutputColor.NoChange, ConsoleOutputColor.NoChange),
    new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Trace", ConsoleOutputColor.NoChange, ConsoleOutputColor.NoChange)
  };
}
