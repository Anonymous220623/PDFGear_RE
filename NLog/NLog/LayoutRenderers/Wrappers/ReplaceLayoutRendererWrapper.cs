// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.ReplaceLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("replace")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public sealed class ReplaceLayoutRendererWrapper : WrapperLayoutRendererBase
{
  private RegexHelper _regexHelper;
  private MatchEvaluator _groupMatchEvaluator;

  [DefaultValue(null)]
  public string SearchFor { get; set; }

  [DefaultValue(false)]
  public bool Regex { get; set; }

  [DefaultValue(null)]
  public string ReplaceWith { get; set; }

  [DefaultValue(null)]
  public string ReplaceGroupName { get; set; }

  [DefaultValue(false)]
  public bool IgnoreCase { get; set; }

  [DefaultValue(false)]
  public bool WholeWords { get; set; }

  [DefaultValue(false)]
  public bool CompileRegex { get; set; }

  protected override void InitializeLayoutRenderer()
  {
    base.InitializeLayoutRenderer();
    this._regexHelper = new RegexHelper()
    {
      IgnoreCase = this.IgnoreCase,
      WholeWords = this.WholeWords,
      CompileRegex = this.CompileRegex
    };
    if (this.Regex)
      this._regexHelper.RegexPattern = this.SearchFor;
    else
      this._regexHelper.SearchText = this.SearchFor;
    if (string.IsNullOrEmpty(this.ReplaceGroupName))
      return;
    System.Text.RegularExpressions.Regex regex = this._regexHelper.Regex;
    int num;
    if (regex == null)
    {
      num = 0;
    }
    else
    {
      string[] groupNames = regex.GetGroupNames();
      bool? nullable = groupNames != null ? new bool?(((IEnumerable<string>) groupNames).Contains<string>(this.ReplaceGroupName)) : new bool?();
      bool flag = false;
      num = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
    }
    if (num == 0)
      return;
    InternalLogger.Warn<string>("Replace-LayoutRenderer assigned unknown ReplaceGroupName: {0}", this.ReplaceGroupName);
  }

  protected override string Transform(string text)
  {
    if (string.IsNullOrEmpty(this.ReplaceGroupName))
      return this._regexHelper.Replace(text, this.ReplaceWith);
    if (this._groupMatchEvaluator == null)
      this._groupMatchEvaluator = (MatchEvaluator) (m => ReplaceLayoutRendererWrapper.ReplaceNamedGroup(this.ReplaceGroupName, this.ReplaceWith, m));
    return this._regexHelper.Regex?.Replace(text, this._groupMatchEvaluator) ?? text;
  }

  [Obsolete("This method should not be used. Marked obsolete on NLog 4.7")]
  public static string ReplaceNamedGroup(
    string input,
    string groupName,
    string replacement,
    Match match)
  {
    return ReplaceLayoutRendererWrapper.ReplaceNamedGroup(groupName, replacement, match);
  }

  internal static string ReplaceNamedGroup(string groupName, string replacement, Match match)
  {
    StringBuilder stringBuilder = new StringBuilder(match.Value);
    int length = match.Length;
    foreach (Capture capture in (IEnumerable<Capture>) match.Groups[groupName].Captures.OfType<Capture>().OrderByDescending<Capture, int>((Func<Capture, int>) (c => c.Index)))
    {
      length += replacement.Length - capture.Length;
      stringBuilder.Remove(capture.Index - match.Index, capture.Length);
      stringBuilder.Insert(capture.Index - match.Index, replacement);
    }
    int startIndex = length;
    stringBuilder.Remove(startIndex, stringBuilder.Length - startIndex);
    return stringBuilder.ToString();
  }

  [ThreadAgnostic]
  [Obsolete("This class should not be used. Marked obsolete on NLog 4.7")]
  public class Replacer
  {
    private readonly string _text;
    private readonly string _replaceGroupName;
    private readonly string _replaceWith;

    internal Replacer(string text, string replaceGroupName, string replaceWith)
    {
      this._text = text;
      this._replaceGroupName = replaceGroupName;
      this._replaceWith = replaceWith;
    }

    internal string EvaluateMatch(Match match)
    {
      return ReplaceLayoutRendererWrapper.ReplaceNamedGroup(this._replaceGroupName, this._replaceWith, match);
    }
  }
}
