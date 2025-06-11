// Decompiled with JetBrains decompiler
// Type: NLog.Targets.ColoredConsoleTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace NLog.Targets;

[Target("ColoredConsole")]
public sealed class ColoredConsoleTarget : TargetWithLayoutHeaderAndFooter
{
  private bool _pauseLogging;
  private bool _disableColors;
  private IColoredConsolePrinter _consolePrinter;
  private Encoding _encoding;

  public ColoredConsoleTarget()
  {
    this.WordHighlightingRules = (IList<ConsoleWordHighlightingRule>) new List<ConsoleWordHighlightingRule>();
    this.RowHighlightingRules = (IList<ConsoleRowHighlightingRule>) new List<ConsoleRowHighlightingRule>();
    this.UseDefaultRowHighlightingRules = true;
    this.OptimizeBufferReuse = true;
    this._consolePrinter = ColoredConsoleTarget.CreateConsolePrinter(this.EnableAnsiOutput);
  }

  public ColoredConsoleTarget(string name)
    : this()
  {
    this.Name = name;
  }

  [DefaultValue(false)]
  public bool ErrorStream { get; set; }

  [DefaultValue(true)]
  public bool UseDefaultRowHighlightingRules { get; set; }

  public Encoding Encoding
  {
    get
    {
      return ConsoleTargetHelper.GetConsoleOutputEncoding(this._encoding, this.IsInitialized, this._pauseLogging);
    }
    set
    {
      if (!ConsoleTargetHelper.SetConsoleOutputEncoding(value, this.IsInitialized, this._pauseLogging))
        return;
      this._encoding = value;
    }
  }

  [DefaultValue(false)]
  public bool DetectConsoleAvailable { get; set; }

  [DefaultValue(false)]
  public bool DetectOutputRedirected { get; set; }

  [DefaultValue(false)]
  public bool AutoFlush { get; set; }

  [DefaultValue(false)]
  public bool EnableAnsiOutput { get; set; }

  [ArrayParameter(typeof (ConsoleRowHighlightingRule), "highlight-row")]
  public IList<ConsoleRowHighlightingRule> RowHighlightingRules { get; private set; }

  [ArrayParameter(typeof (ConsoleWordHighlightingRule), "highlight-word")]
  public IList<ConsoleWordHighlightingRule> WordHighlightingRules { get; private set; }

  protected override void InitializeTarget()
  {
    this._pauseLogging = false;
    this._disableColors = false;
    if (this.DetectConsoleAvailable)
    {
      string reason;
      this._pauseLogging = !ConsoleTargetHelper.IsConsoleAvailable(out reason);
      if (this._pauseLogging)
        InternalLogger.Info<string, string>("ColoredConsole(Name={0}): Console detected as turned off. Disable DetectConsoleAvailable to skip detection. Reason: {1}", this.Name, reason);
    }
    if (this._encoding != null)
      ConsoleTargetHelper.SetConsoleOutputEncoding(this._encoding, true, this._pauseLogging);
    if (this.DetectOutputRedirected)
    {
      try
      {
        this._disableColors = this.ErrorStream ? Console.IsErrorRedirected : Console.IsOutputRedirected;
        if (this._disableColors)
        {
          InternalLogger.Info<string>("ColoredConsole(Name={0}): Console output is redirected so no colors. Disable DetectOutputRedirected to skip detection.", this.Name);
          if (!this.AutoFlush)
          {
            if (this.GetOutput() is StreamWriter output)
            {
              if (!output.AutoFlush)
                this.AutoFlush = true;
            }
          }
        }
      }
      catch (Exception ex)
      {
        object[] objArray = new object[1]
        {
          (object) this.Name
        };
        InternalLogger.Error(ex, "ColoredConsole(Name={0}): Failed checking if Console Output Redirected.", objArray);
      }
    }
    base.InitializeTarget();
    if (this.Header != null)
    {
      LogEventInfo nullEvent = LogEventInfo.CreateNullEvent();
      this.WriteToOutput(nullEvent, this.RenderLogEvent(this.Header, nullEvent));
    }
    this._consolePrinter = ColoredConsoleTarget.CreateConsolePrinter(this.EnableAnsiOutput);
  }

  private static IColoredConsolePrinter CreateConsolePrinter(bool enableAnsiOutput)
  {
    return !enableAnsiOutput ? (IColoredConsolePrinter) new ColoredConsoleSystemPrinter() : (IColoredConsolePrinter) new ColoredConsoleAnsiPrinter();
  }

  protected override void CloseTarget()
  {
    if (this.Footer != null)
    {
      LogEventInfo nullEvent = LogEventInfo.CreateNullEvent();
      this.WriteToOutput(nullEvent, this.RenderLogEvent(this.Footer, nullEvent));
    }
    this.ExplicitConsoleFlush();
    base.CloseTarget();
  }

  protected override void FlushAsync(AsyncContinuation asyncContinuation)
  {
    try
    {
      this.ExplicitConsoleFlush();
      base.FlushAsync(asyncContinuation);
    }
    catch (Exception ex)
    {
      asyncContinuation(ex);
    }
  }

  private void ExplicitConsoleFlush()
  {
    if (this._pauseLogging || this.AutoFlush)
      return;
    this.GetOutput().Flush();
  }

  protected override void Write(LogEventInfo logEvent)
  {
    if (this._pauseLogging)
      return;
    this.WriteToOutput(logEvent, this.RenderLogEvent(this.Layout, logEvent));
  }

  private void WriteToOutput(LogEventInfo logEvent, string message)
  {
    try
    {
      this.WriteToOutputWithColor(logEvent, message ?? string.Empty);
    }
    catch (Exception ex) when (
    {
      // ISSUE: unable to correctly present filter
      int num;
      switch (ex)
      {
        case OverflowException _:
        case IndexOutOfRangeException _:
          num = 1;
          break;
        default:
          num = ex is ArgumentOutOfRangeException ? 1 : 0;
          break;
      }
      if ((uint) num > 0U)
      {
        SuccessfulFiltering;
      }
      else
        throw;
    }
    )
    {
      this._pauseLogging = true;
      InternalLogger.Warn(ex, "ColoredConsole(Name={0}): {1} has been thrown and this is probably due to a race condition.Logging to the console will be paused. Enable by reloading the config or re-initialize the targets", (object) this.Name, (object) ex.GetType());
    }
  }

  private void WriteToOutputWithColor(LogEventInfo logEvent, string message)
  {
    string colorMessage = message;
    ConsoleColor? newForegroundColor = new ConsoleColor?();
    ConsoleColor? newBackgroundColor = new ConsoleColor?();
    if (!this._disableColors)
    {
      ConsoleRowHighlightingRule highlightingRule = this.GetMatchingRowHighlightingRule(logEvent);
      if (this.WordHighlightingRules.Count > 0)
        colorMessage = this.GenerateColorEscapeSequences(logEvent, message);
      newForegroundColor = highlightingRule.ForegroundColor != ConsoleOutputColor.NoChange ? new ConsoleColor?((ConsoleColor) highlightingRule.ForegroundColor) : new ConsoleColor?();
      newBackgroundColor = highlightingRule.BackgroundColor != ConsoleOutputColor.NoChange ? new ConsoleColor?((ConsoleColor) highlightingRule.BackgroundColor) : new ConsoleColor?();
    }
    TextWriter output = this.GetOutput();
    if ((object) colorMessage == (object) message && !newForegroundColor.HasValue && !newBackgroundColor.HasValue)
    {
      ConsoleTargetHelper.WriteLineThreadSafe(output, message, this.AutoFlush);
    }
    else
    {
      bool wordHighlighting = (object) colorMessage != (object) message;
      if (!wordHighlighting && message.IndexOf('\n') >= 0)
      {
        wordHighlighting = true;
        colorMessage = ColoredConsoleTarget.EscapeColorCodes(message);
      }
      this.WriteToOutputWithPrinter(output, colorMessage, newForegroundColor, newBackgroundColor, wordHighlighting);
    }
  }

  private void WriteToOutputWithPrinter(
    TextWriter consoleStream,
    string colorMessage,
    ConsoleColor? newForegroundColor,
    ConsoleColor? newBackgroundColor,
    bool wordHighlighting)
  {
    using (ReusableObjectCreator<StringBuilder>.LockOject lockOject = this.OptimizeBufferReuse ? this.ReusableLayoutBuilder.Allocate() : this.ReusableLayoutBuilder.None)
    {
      TextWriter consoleWriter1 = this._consolePrinter.AcquireTextWriter(consoleStream, lockOject.Result);
      ConsoleColor? nullable1 = new ConsoleColor?();
      ConsoleColor? nullable2 = new ConsoleColor?();
      try
      {
        if (wordHighlighting)
        {
          nullable1 = this._consolePrinter.ChangeForegroundColor(consoleWriter1, newForegroundColor);
          nullable2 = this._consolePrinter.ChangeBackgroundColor(consoleWriter1, newBackgroundColor);
          ConsoleColor? rowForegroundColor = newForegroundColor ?? nullable1;
          ConsoleColor? rowBackgroundColor = newBackgroundColor ?? nullable2;
          ColoredConsoleTarget.ColorizeEscapeSequences(this._consolePrinter, consoleWriter1, colorMessage, nullable1, nullable2, rowForegroundColor, rowBackgroundColor);
          this._consolePrinter.WriteLine(consoleWriter1, string.Empty);
        }
        else
        {
          ConsoleColor? nullable3;
          if (newForegroundColor.HasValue)
          {
            nullable1 = this._consolePrinter.ChangeForegroundColor(consoleWriter1, new ConsoleColor?(newForegroundColor.Value));
            ConsoleColor? nullable4 = nullable1;
            nullable3 = newForegroundColor;
            if (nullable4.GetValueOrDefault() == nullable3.GetValueOrDefault() & nullable4.HasValue == nullable3.HasValue)
              nullable1 = new ConsoleColor?();
          }
          if (newBackgroundColor.HasValue)
          {
            IColoredConsolePrinter consolePrinter = this._consolePrinter;
            TextWriter consoleWriter2 = consoleWriter1;
            ConsoleColor? backgroundColor = new ConsoleColor?(newBackgroundColor.Value);
            nullable3 = new ConsoleColor?();
            ConsoleColor? oldBackgroundColor = nullable3;
            nullable2 = consolePrinter.ChangeBackgroundColor(consoleWriter2, backgroundColor, oldBackgroundColor);
            nullable3 = nullable2;
            ConsoleColor? nullable5 = newBackgroundColor;
            if (nullable3.GetValueOrDefault() == nullable5.GetValueOrDefault() & nullable3.HasValue == nullable5.HasValue)
              nullable2 = new ConsoleColor?();
          }
          this._consolePrinter.WriteLine(consoleWriter1, colorMessage);
        }
      }
      finally
      {
        this._consolePrinter.ReleaseTextWriter(consoleWriter1, consoleStream, nullable1, nullable2, this.AutoFlush);
      }
    }
  }

  private ConsoleRowHighlightingRule GetMatchingRowHighlightingRule(LogEventInfo logEvent)
  {
    ConsoleRowHighlightingRule highlightingRule = this.GetMatchingRowHighlightingRule(this.RowHighlightingRules, logEvent);
    if (highlightingRule == null && this.UseDefaultRowHighlightingRules)
      highlightingRule = this.GetMatchingRowHighlightingRule(this._consolePrinter.DefaultConsoleRowHighlightingRules, logEvent);
    return highlightingRule ?? ConsoleRowHighlightingRule.Default;
  }

  private ConsoleRowHighlightingRule GetMatchingRowHighlightingRule(
    IList<ConsoleRowHighlightingRule> rules,
    LogEventInfo logEvent)
  {
    for (int index = 0; index < rules.Count; ++index)
    {
      ConsoleRowHighlightingRule rule = rules[index];
      if (rule.CheckCondition(logEvent))
        return rule;
    }
    return (ConsoleRowHighlightingRule) null;
  }

  private string GenerateColorEscapeSequences(LogEventInfo logEvent, string message)
  {
    if (string.IsNullOrEmpty(message))
      return message;
    message = ColoredConsoleTarget.EscapeColorCodes(message);
    using (ReusableObjectCreator<StringBuilder>.LockOject lockOject = this.OptimizeBufferReuse ? this.ReusableLayoutBuilder.Allocate() : this.ReusableLayoutBuilder.None)
    {
      StringBuilder stringBuilder = lockOject.Result;
      for (int index = 0; index < this.WordHighlightingRules.Count; ++index)
      {
        ConsoleWordHighlightingRule highlightingRule = this.WordHighlightingRules[index];
        MatchCollection matchCollection = highlightingRule.Matches(logEvent, message);
        if (matchCollection != null && matchCollection.Count != 0)
        {
          if (stringBuilder != null)
            stringBuilder.Length = 0;
          int startIndex = 0;
          foreach (Match match in matchCollection)
          {
            stringBuilder = stringBuilder ?? new StringBuilder(message.Length + 5);
            stringBuilder.Append(message, startIndex, match.Index - startIndex);
            stringBuilder.Append('\a');
            stringBuilder.Append((char) (highlightingRule.ForegroundColor + 65));
            stringBuilder.Append((char) (highlightingRule.BackgroundColor + 65));
            stringBuilder.Append(match.Value);
            stringBuilder.Append('\a');
            stringBuilder.Append('X');
            startIndex = match.Index + match.Length;
          }
          if (stringBuilder != null && stringBuilder.Length > 0)
          {
            stringBuilder.Append(message, startIndex, message.Length - startIndex);
            message = stringBuilder.ToString();
          }
        }
      }
    }
    return message;
  }

  private static string EscapeColorCodes(string message)
  {
    if (message.IndexOf("\a", StringComparison.Ordinal) >= 0)
      message = message.Replace("\a", "\a\a");
    return message;
  }

  private static void ColorizeEscapeSequences(
    IColoredConsolePrinter consolePrinter,
    TextWriter consoleWriter,
    string message,
    ConsoleColor? defaultForegroundColor,
    ConsoleColor? defaultBackgroundColor,
    ConsoleColor? rowForegroundColor,
    ConsoleColor? rowBackgroundColor)
  {
    Stack<KeyValuePair<ConsoleColor?, ConsoleColor?>> keyValuePairStack = new Stack<KeyValuePair<ConsoleColor?, ConsoleColor?>>();
    keyValuePairStack.Push(new KeyValuePair<ConsoleColor?, ConsoleColor?>(rowForegroundColor, rowBackgroundColor));
    int index = 0;
    while (index < message.Length)
    {
      int num = index;
      while (num < message.Length && message[num] >= ' ')
        ++num;
      if (num != index)
        consolePrinter.WriteSubString(consoleWriter, message, index, num);
      if (num >= message.Length)
      {
        index = num;
        break;
      }
      char text = message[num];
      ConsoleColor? nullable1;
      ConsoleColor? nullable2;
      switch (text)
      {
        case '\a':
          if (num + 1 < message.Length)
          {
            char ch = message[num + 1];
            switch (ch)
            {
              case '\a':
                consolePrinter.WriteChar(consoleWriter, '\a');
                index = num + 2;
                continue;
              case 'X':
                KeyValuePair<ConsoleColor?, ConsoleColor?> keyValuePair1 = keyValuePairStack.Pop();
                KeyValuePair<ConsoleColor?, ConsoleColor?> keyValuePair2 = keyValuePairStack.Peek();
                nullable1 = keyValuePair2.Key;
                nullable2 = keyValuePair1.Key;
                if (nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue)
                {
                  nullable2 = keyValuePair2.Value;
                  nullable1 = keyValuePair1.Value;
                  if (nullable2.GetValueOrDefault() == nullable1.GetValueOrDefault() & nullable2.HasValue == nullable1.HasValue)
                    goto label_31;
                }
                nullable1 = keyValuePair1.Key;
                if (nullable1.HasValue)
                {
                  nullable1 = keyValuePair2.Key;
                  if (!nullable1.HasValue)
                    goto label_29;
                }
                nullable1 = keyValuePair1.Value;
                if (nullable1.HasValue)
                {
                  nullable1 = keyValuePair2.Value;
                  if (nullable1.HasValue)
                    goto label_30;
                }
                else
                  goto label_30;
label_29:
                consolePrinter.ResetDefaultColors(consoleWriter, defaultForegroundColor, defaultBackgroundColor);
label_30:
                consolePrinter.ChangeForegroundColor(consoleWriter, keyValuePair2.Key, keyValuePair1.Key);
                consolePrinter.ChangeBackgroundColor(consoleWriter, keyValuePair2.Value, keyValuePair1.Value);
label_31:
                index = num + 2;
                continue;
              default:
                KeyValuePair<ConsoleColor?, ConsoleColor?> keyValuePair3 = keyValuePairStack.Peek();
                ConsoleColor? key = keyValuePair3.Key;
                keyValuePair3 = keyValuePairStack.Peek();
                ConsoleColor? nullable3 = keyValuePair3.Value;
                ConsoleOutputColor consoleOutputColor1 = (ConsoleOutputColor) ((int) ch - 65);
                ConsoleOutputColor consoleOutputColor2 = (ConsoleOutputColor) ((int) message[num + 2] - 65);
                if (consoleOutputColor1 != ConsoleOutputColor.NoChange)
                {
                  key = new ConsoleColor?((ConsoleColor) consoleOutputColor1);
                  IColoredConsolePrinter coloredConsolePrinter = consolePrinter;
                  TextWriter consoleWriter1 = consoleWriter;
                  ConsoleColor? foregroundColor = key;
                  nullable1 = new ConsoleColor?();
                  ConsoleColor? oldForegroundColor = nullable1;
                  coloredConsolePrinter.ChangeForegroundColor(consoleWriter1, foregroundColor, oldForegroundColor);
                }
                if (consoleOutputColor2 != ConsoleOutputColor.NoChange)
                {
                  nullable3 = new ConsoleColor?((ConsoleColor) consoleOutputColor2);
                  IColoredConsolePrinter coloredConsolePrinter = consolePrinter;
                  TextWriter consoleWriter2 = consoleWriter;
                  ConsoleColor? backgroundColor = nullable3;
                  nullable1 = new ConsoleColor?();
                  ConsoleColor? oldBackgroundColor = nullable1;
                  coloredConsolePrinter.ChangeBackgroundColor(consoleWriter2, backgroundColor, oldBackgroundColor);
                }
                keyValuePairStack.Push(new KeyValuePair<ConsoleColor?, ConsoleColor?>(key, nullable3));
                index = num + 3;
                continue;
            }
          }
          else
            break;
        case '\n':
        case '\r':
          KeyValuePair<ConsoleColor?, ConsoleColor?> keyValuePair = keyValuePairStack.Peek();
          nullable1 = keyValuePair.Key;
          nullable2 = defaultForegroundColor;
          ConsoleColor? nullable4;
          if (nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue)
          {
            nullable2 = new ConsoleColor?();
            nullable4 = nullable2;
          }
          else
            nullable4 = defaultForegroundColor;
          ConsoleColor? foregroundColor1 = nullable4;
          nullable2 = keyValuePair.Value;
          nullable1 = defaultBackgroundColor;
          ConsoleColor? nullable5;
          if (nullable2.GetValueOrDefault() == nullable1.GetValueOrDefault() & nullable2.HasValue == nullable1.HasValue)
          {
            nullable1 = new ConsoleColor?();
            nullable5 = nullable1;
          }
          else
            nullable5 = defaultBackgroundColor;
          ConsoleColor? backgroundColor1 = nullable5;
          consolePrinter.ResetDefaultColors(consoleWriter, foregroundColor1, backgroundColor1);
          if (num + 1 < message.Length && message[num + 1] == '\n')
          {
            consolePrinter.WriteSubString(consoleWriter, message, num, num + 2);
            index = num + 2;
          }
          else
          {
            consolePrinter.WriteChar(consoleWriter, text);
            index = num + 1;
          }
          consolePrinter.ChangeForegroundColor(consoleWriter, keyValuePair.Key, defaultForegroundColor);
          consolePrinter.ChangeBackgroundColor(consoleWriter, keyValuePair.Value, defaultBackgroundColor);
          continue;
      }
      consolePrinter.WriteChar(consoleWriter, text);
      index = num + 1;
    }
    if (index >= message.Length)
      return;
    consolePrinter.WriteSubString(consoleWriter, message, index, message.Length);
  }

  private TextWriter GetOutput() => !this.ErrorStream ? Console.Out : Console.Error;
}
