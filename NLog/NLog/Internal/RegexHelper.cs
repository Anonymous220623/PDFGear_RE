// Decompiled with JetBrains decompiler
// Type: NLog.Internal.RegexHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System.Text.RegularExpressions;

#nullable disable
namespace NLog.Internal;

internal class RegexHelper
{
  private Regex _regex;
  private string _searchText;
  private string _regexPattern;
  private bool _wholeWords;
  private bool _ignoreCase;
  private bool _simpleSearchText;

  public string SearchText
  {
    get => this._searchText;
    set
    {
      this._searchText = value;
      this._regexPattern = (string) null;
      this.ResetRegex();
    }
  }

  public string RegexPattern
  {
    get => this._regexPattern;
    set
    {
      this._regexPattern = value;
      this._searchText = (string) null;
      this.ResetRegex();
    }
  }

  public bool CompileRegex { get; set; }

  public bool WholeWords
  {
    get => this._wholeWords;
    set
    {
      if (this._wholeWords == value)
        return;
      this._wholeWords = value;
      this.ResetRegex();
    }
  }

  public bool IgnoreCase
  {
    get => this._ignoreCase;
    set
    {
      if (this._ignoreCase == value)
        return;
      this._ignoreCase = value;
      this.ResetRegex();
    }
  }

  public Regex Regex
  {
    get
    {
      if (this._regex != null)
        return this._regex;
      string regexPattern = this.RegexPattern;
      return string.IsNullOrEmpty(regexPattern) ? (Regex) null : (this._regex = new Regex(regexPattern, this.GetRegexOptions()));
    }
  }

  private void ResetRegex()
  {
    this._simpleSearchText = !this.WholeWords && !this.IgnoreCase && !string.IsNullOrEmpty(this.SearchText);
    if (!string.IsNullOrEmpty(this.SearchText))
      this._regexPattern = Regex.Escape(this.SearchText);
    if (this.WholeWords && !string.IsNullOrEmpty(this._regexPattern))
      this._regexPattern = $"\\b{this._regexPattern}\\b";
    this._regex = (Regex) null;
  }

  private RegexOptions GetRegexOptions()
  {
    RegexOptions regexOptions = RegexOptions.None;
    if (this.IgnoreCase)
      regexOptions |= RegexOptions.IgnoreCase;
    if (this.CompileRegex)
      regexOptions |= RegexOptions.Compiled;
    return regexOptions;
  }

  public string Replace(string input, string replacement)
  {
    if (this._simpleSearchText)
      return input.Replace(this.SearchText, replacement);
    if (this.CompileRegex)
      return this.Regex?.Replace(input, replacement) ?? input;
    return this._regexPattern == null ? input : Regex.Replace(input, this._regexPattern, replacement, this.GetRegexOptions());
  }

  public MatchCollection Matches(string input)
  {
    return this.CompileRegex ? this.Regex?.Matches(input) : (this._regexPattern == null ? (MatchCollection) null : Regex.Matches(input, this._regexPattern, this.GetRegexOptions()));
  }
}
