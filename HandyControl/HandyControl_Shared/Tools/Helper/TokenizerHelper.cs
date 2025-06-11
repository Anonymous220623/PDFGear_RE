// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.TokenizerHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Globalization;

#nullable disable
namespace HandyControl.Tools;

internal class TokenizerHelper
{
  private char _quoteChar;
  private char _argSeparator;
  private string _str;
  private int _strLen;
  private int _charIndex;
  private int _currentTokenIndex;
  private int _currentTokenLength;

  public bool FoundSeparator { get; private set; }

  public TokenizerHelper(string str, IFormatProvider formatProvider)
  {
    char numericListSeparator = TokenizerHelper.GetNumericListSeparator(formatProvider);
    this.Initialize(str, '\'', numericListSeparator);
  }

  private void Initialize(string str, char quoteChar, char separator)
  {
    this._str = str;
    this._strLen = str != null ? str.Length : 0;
    this._currentTokenIndex = -1;
    this._quoteChar = quoteChar;
    this._argSeparator = separator;
    while (this._charIndex < this._strLen && char.IsWhiteSpace(this._str, this._charIndex))
      ++this._charIndex;
  }

  public string GetCurrentToken()
  {
    return this._currentTokenIndex >= 0 ? this._str.Substring(this._currentTokenIndex, this._currentTokenLength) : (string) null;
  }

  internal bool NextToken() => this.NextToken(false);

  public bool NextToken(bool allowQuotedToken)
  {
    return this.NextToken(allowQuotedToken, this._argSeparator);
  }

  public bool NextToken(bool allowQuotedToken, char separator)
  {
    this._currentTokenIndex = -1;
    this.FoundSeparator = false;
    if (this._charIndex >= this._strLen)
      return false;
    char ch = this._str[this._charIndex];
    int num1 = 0;
    if (allowQuotedToken && (int) ch == (int) this._quoteChar)
    {
      ++num1;
      ++this._charIndex;
    }
    int charIndex = this._charIndex;
    int num2 = 0;
    while (this._charIndex < this._strLen)
    {
      char c = this._str[this._charIndex];
      if (num1 > 0)
      {
        if ((int) c == (int) this._quoteChar)
        {
          --num1;
          if (num1 == 0)
          {
            ++this._charIndex;
            break;
          }
        }
      }
      else if (char.IsWhiteSpace(c) || (int) c == (int) separator)
      {
        if ((int) c == (int) separator)
        {
          this.FoundSeparator = true;
          break;
        }
        break;
      }
      ++this._charIndex;
      ++num2;
    }
    if (num1 > 0)
      throw new InvalidOperationException("TokenizerHelperMissingEndQuote");
    this.ScanToNextToken(separator);
    this._currentTokenIndex = charIndex;
    this._currentTokenLength = num2;
    if (this._currentTokenLength < 1)
      throw new InvalidOperationException("TokenizerHelperEmptyToken");
    return true;
  }

  private void ScanToNextToken(char separator)
  {
    if (this._charIndex >= this._strLen)
      return;
    char c1 = this._str[this._charIndex];
    if ((int) c1 != (int) separator && !char.IsWhiteSpace(c1))
      throw new InvalidOperationException("TokenizerHelperExtraDataEncountered");
    int num = 0;
    while (this._charIndex < this._strLen)
    {
      char c2 = this._str[this._charIndex];
      if ((int) c2 == (int) separator)
      {
        this.FoundSeparator = true;
        ++num;
        ++this._charIndex;
        if (num > 1)
          throw new InvalidOperationException("TokenizerHelperEmptyToken");
      }
      else if (char.IsWhiteSpace(c2))
        ++this._charIndex;
      else
        break;
    }
    if (num > 0 && this._charIndex >= this._strLen)
      throw new InvalidOperationException("TokenizerHelperEmptyToken");
  }

  internal static char GetNumericListSeparator(IFormatProvider provider)
  {
    char numericListSeparator = ',';
    NumberFormatInfo instance = NumberFormatInfo.GetInstance(provider);
    if (instance.NumberDecimalSeparator.Length > 0 && (int) numericListSeparator == (int) instance.NumberDecimalSeparator[0])
      numericListSeparator = ';';
    return numericListSeparator;
  }
}
