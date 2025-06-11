// Decompiled with JetBrains decompiler
// Type: NLog.Targets.ColoredConsoleSystemPrinter
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

internal class ColoredConsoleSystemPrinter : IColoredConsolePrinter
{
  public TextWriter AcquireTextWriter(TextWriter consoleStream, StringBuilder reusableBuilder)
  {
    return consoleStream;
  }

  public void ReleaseTextWriter(
    TextWriter consoleWriter,
    TextWriter consoleStream,
    ConsoleColor? oldForegroundColor,
    ConsoleColor? oldBackgroundColor,
    bool flush)
  {
    this.ResetDefaultColors(consoleWriter, oldForegroundColor, oldBackgroundColor);
    if (!flush)
      return;
    consoleWriter.Flush();
  }

  public ConsoleColor? ChangeForegroundColor(
    TextWriter consoleWriter,
    ConsoleColor? foregroundColor,
    ConsoleColor? oldForegroundColor = null)
  {
    ConsoleColor consoleColor = (ConsoleColor) ((int) oldForegroundColor ?? (int) Console.ForegroundColor);
    if (foregroundColor.HasValue && consoleColor != foregroundColor.Value)
      Console.ForegroundColor = foregroundColor.Value;
    return new ConsoleColor?(consoleColor);
  }

  public ConsoleColor? ChangeBackgroundColor(
    TextWriter consoleWriter,
    ConsoleColor? backgroundColor,
    ConsoleColor? oldBackgroundColor = null)
  {
    ConsoleColor consoleColor = (ConsoleColor) ((int) oldBackgroundColor ?? (int) Console.BackgroundColor);
    if (backgroundColor.HasValue && consoleColor != backgroundColor.Value)
      Console.BackgroundColor = backgroundColor.Value;
    return new ConsoleColor?(consoleColor);
  }

  public void ResetDefaultColors(
    TextWriter consoleWriter,
    ConsoleColor? foregroundColor,
    ConsoleColor? backgroundColor)
  {
    if (foregroundColor.HasValue)
      Console.ForegroundColor = foregroundColor.Value;
    if (!backgroundColor.HasValue)
      return;
    Console.BackgroundColor = backgroundColor.Value;
  }

  public void WriteSubString(TextWriter consoleWriter, string text, int index, int endIndex)
  {
    consoleWriter.Write(text.Substring(index, endIndex - index));
  }

  public void WriteChar(TextWriter consoleWriter, char text) => consoleWriter.Write(text);

  public void WriteLine(TextWriter consoleWriter, string text) => consoleWriter.WriteLine(text);

  public IList<ConsoleRowHighlightingRule> DefaultConsoleRowHighlightingRules { get; } = (IList<ConsoleRowHighlightingRule>) new List<ConsoleRowHighlightingRule>()
  {
    new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Fatal", ConsoleOutputColor.Red, ConsoleOutputColor.NoChange),
    new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Error", ConsoleOutputColor.Yellow, ConsoleOutputColor.NoChange),
    new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Warn", ConsoleOutputColor.Magenta, ConsoleOutputColor.NoChange),
    new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Info", ConsoleOutputColor.White, ConsoleOutputColor.NoChange),
    new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Debug", ConsoleOutputColor.Gray, ConsoleOutputColor.NoChange),
    new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Trace", ConsoleOutputColor.DarkGray, ConsoleOutputColor.NoChange)
  };
}
