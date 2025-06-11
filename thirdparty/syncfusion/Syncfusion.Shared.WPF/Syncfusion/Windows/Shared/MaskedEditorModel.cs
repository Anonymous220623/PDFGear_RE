// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.MaskedEditorModel
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class MaskedEditorModel
{
  internal MaskedTextBox maskedText = new MaskedTextBox();
  private static readonly string defDigitSymbols = "09#";
  private static readonly string defOtherSymbols = "Ll?&CcAa";
  private static readonly string defSeparatorSymbols = ".,:/$";
  private static readonly string defShiftSymbols = "<>|";

  public MaskedEditorModel()
  {
    this.DateSeparator = "/";
    this.DecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
    this.TimeSeparator = ":";
    if (this.maskedText.GroupSeperatorEnabled)
      this.NumberGroupSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator;
    this.CurrencySymbol = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol;
    this.PromptChar = '_';
    this.Mask = string.Empty;
    this.TextMaskIndexes = new Dictionary<int, int>();
    this.ShiftStatusIndexes = new Dictionary<int, ShiftStatus>();
  }

  public string CurrencySymbol { get; set; }

  public string DateSeparator { get; set; }

  public string DecimalSeparator { get; set; }

  public string Mask { get; set; }

  public string NumberGroupSeparator { get; set; }

  public char PromptChar { get; set; }

  public string Text { get; set; }

  public string TimeSeparator { get; set; }

  private int MaskIndex { get; set; }

  private Dictionary<int, ShiftStatus> ShiftStatusIndexes { get; set; }

  private Dictionary<int, int> TextMaskIndexes { get; set; }

  public static string GetMaskedText(
    string mask,
    string text,
    string dateSeparator,
    string timeSeparator,
    string decimalSeparator,
    string numberGroupSeparator,
    char promptChar,
    string currencySymbol)
  {
    if (mask == null || text == null || dateSeparator == null || timeSeparator == null || decimalSeparator == null || numberGroupSeparator == null || currencySymbol == null)
      return (string) null;
    return new MaskedEditorModel()
    {
      Mask = mask,
      Text = text,
      DateSeparator = dateSeparator,
      TimeSeparator = timeSeparator,
      DecimalSeparator = decimalSeparator,
      NumberGroupSeparator = numberGroupSeparator,
      PromptChar = promptChar,
      CurrencySymbol = currencySymbol
    }.GetMaskedText();
  }

  public string GetMaskedText()
  {
    this.ApplyNewMask();
    return this.Text;
  }

  protected internal void ApplyNewMask()
  {
    string str = string.Empty;
    string empty = string.Empty;
    this.MaskIndex = -1;
    ShiftStatus shiftStatus = ShiftStatus.None;
    if (this.TextMaskIndexes != null && this.TextMaskIndexes.Count > 0)
      this.TextMaskIndexes.Clear();
    if (this.ShiftStatusIndexes != null && this.ShiftStatusIndexes.Count > 0)
      this.ShiftStatusIndexes.Clear();
    while (this.MaskIndex < this.Mask.Length - 1)
    {
      string nextMaskSymbol = this.GetNextMaskSymbol(true);
      if (!string.IsNullOrEmpty(nextMaskSymbol))
      {
        if (this.IsSymbolLiteral(nextMaskSymbol))
          str = nextMaskSymbol[0] != '\\' || nextMaskSymbol.Length != 2 ? str + nextMaskSymbol : str + nextMaskSymbol[1].ToString();
        else if (this.IsSymbolSeparator(nextMaskSymbol))
          str += this.GetSeparatorText(nextMaskSymbol[0]);
        else if (this.IsShiftSymbol(nextMaskSymbol))
        {
          shiftStatus = this.GetShiftStatus(nextMaskSymbol);
        }
        else
        {
          if (this.TextMaskIndexes != null)
            this.TextMaskIndexes.Add(str.Length, this.MaskIndex);
          if (this.ShiftStatusIndexes != null)
            this.ShiftStatusIndexes.Add(str.Length, shiftStatus);
          str += this.PromptChar.ToString();
        }
      }
    }
    string text = this.Text;
    this.MaskIndex = -1;
    this.Text = string.IsNullOrEmpty(this.Mask) ? text : str;
    if (string.IsNullOrEmpty(this.Mask))
      return;
    int index1 = 0;
    for (int index2 = 0; index2 < this.Text.Length && index1 < text.Length; ++index2)
    {
      if (this.TextMaskIndexes != null && this.TextMaskIndexes.ContainsKey(index2))
      {
        if (this.IsAcceptableSymbol(this.Mask[this.TextMaskIndexes[index2]].ToString(), text[index1]))
          this.ReplaceTextSymbol(index2, text[index1++].ToString());
      }
      else if ((int) this.Text[index2] == (int) text[index1])
        this.ReplaceTextSymbol(index2, text[index1++].ToString());
    }
  }

  protected string GetNextMaskSymbol(bool bMoveCursor)
  {
    string empty = string.Empty;
    if (this.MaskIndex < this.Mask.Length - 1)
    {
      empty = this.Mask[bMoveCursor ? ++this.MaskIndex : this.MaskIndex + 1].ToString();
      if (empty == "\\" && this.MaskIndex < this.Mask.Length - 1)
        empty += (string) (object) this.Mask[bMoveCursor ? ++this.MaskIndex : this.MaskIndex + 2];
    }
    return empty;
  }

  protected string GetSeparatorText(char symbol)
  {
    switch (symbol)
    {
      case '$':
        return this.CurrencySymbol;
      case ',':
        return this.NumberGroupSeparator;
      case '.':
        return this.DecimalSeparator;
      case '/':
        return this.DateSeparator;
      case ':':
        return this.TimeSeparator;
      default:
        return string.Empty;
    }
  }

  protected ShiftStatus GetShiftStatus(string maskSymbol)
  {
    switch (maskSymbol)
    {
      case ">":
        return ShiftStatus.Uppercase;
      case "<":
        return ShiftStatus.Lowercase;
      default:
        return ShiftStatus.None;
    }
  }

  protected bool IsAcceptableSymbol(string maskSymbol, char input)
  {
    switch (maskSymbol)
    {
      case "0":
        return char.IsDigit(input);
      case "9":
        return (char.IsDigit(input) || char.IsWhiteSpace(input)) && input != '\t';
      case "#":
        return (char.IsDigit(input) || char.IsWhiteSpace(input) || input == '+' || input == '-') && input != '\t';
      case "l":
      case "L":
        return char.IsLetter(input);
      case "?":
        return (char.IsLetter(input) || char.IsWhiteSpace(input)) && input != '\t';
      case "&":
      case "c":
      case "C":
        return input != '\t';
      case "a":
      case "A":
        return char.IsLetterOrDigit(input);
      default:
        return false;
    }
  }

  protected bool IsInputSymbol(string maskSymbol)
  {
    return MaskedEditorModel.defDigitSymbols.Contains(maskSymbol) || MaskedEditorModel.defOtherSymbols.Contains(maskSymbol);
  }

  protected bool IsShiftSymbol(string maskSymbol)
  {
    return MaskedEditorModel.defShiftSymbols.Contains(maskSymbol);
  }

  protected bool IsSymbolLiteral(string maskSymbol)
  {
    return !this.IsSymbolSeparator(maskSymbol) && !this.IsShiftSymbol(maskSymbol) && !this.IsInputSymbol(maskSymbol);
  }

  protected bool IsSymbolSeparator(string maskSymbol)
  {
    return MaskedEditorModel.defSeparatorSymbols.Contains(maskSymbol);
  }

  protected bool ReplaceTextSymbol(int index, string symbol)
  {
    if (index <= -1 || index >= this.Text.Length)
      return false;
    if (this.ShiftStatusIndexes.ContainsKey(index))
    {
      switch (this.ShiftStatusIndexes[index])
      {
        case ShiftStatus.Uppercase:
          symbol = symbol.ToUpper();
          break;
        case ShiftStatus.Lowercase:
          symbol = symbol.ToLower();
          break;
      }
    }
    this.Text = this.Text.Remove(index, 1).Insert(index, symbol);
    return true;
  }
}
