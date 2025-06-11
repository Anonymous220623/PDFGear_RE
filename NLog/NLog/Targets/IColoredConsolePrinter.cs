// Decompiled with JetBrains decompiler
// Type: NLog.Targets.IColoredConsolePrinter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.Targets;

internal interface IColoredConsolePrinter
{
  TextWriter AcquireTextWriter(TextWriter consoleStream, StringBuilder reusableBuilder);

  void ReleaseTextWriter(
    TextWriter consoleWriter,
    TextWriter consoleStream,
    ConsoleColor? oldForegroundColor,
    ConsoleColor? oldBackgroundColor,
    bool flush);

  ConsoleColor? ChangeForegroundColor(
    TextWriter consoleWriter,
    ConsoleColor? foregroundColor,
    ConsoleColor? oldForegroundColor = null);

  ConsoleColor? ChangeBackgroundColor(
    TextWriter consoleWriter,
    ConsoleColor? backgroundColor,
    ConsoleColor? oldBackgroundColor = null);

  void ResetDefaultColors(
    TextWriter consoleWriter,
    ConsoleColor? foregroundColor,
    ConsoleColor? backgroundColor);

  void WriteSubString(TextWriter consoleWriter, string text, int index, int endIndex);

  void WriteChar(TextWriter consoleWriter, char text);

  void WriteLine(TextWriter consoleWriter, string text);

  IList<ConsoleRowHighlightingRule> DefaultConsoleRowHighlightingRules { get; }
}
