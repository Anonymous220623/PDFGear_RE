// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfCode93ExtendedBarcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfCode93ExtendedBarcode : PdfCode93Barcode
{
  private Dictionary<char, char[]> extendedCodes;

  public PdfCode93ExtendedBarcode() => this.InitializeCode93Extended();

  public PdfCode93ExtendedBarcode(string text)
    : this()
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int index1 = 0; index1 < text.Length; ++index1)
    {
      char[] extendedCode = this.extendedCodes[text[index1]];
      if (extendedCode != null)
      {
        for (int index2 = 0; index2 < extendedCode.Length; ++index2)
          stringBuilder.Append(extendedCode[index2]);
      }
    }
    this.ExtendedText = stringBuilder.ToString();
    this.Text = text;
  }

  protected internal override char[] CalculateCheckDigit()
  {
    this.GetExtendedText();
    if (!this.EnableCheckDigit)
      return (char[]) null;
    int num = 0;
    foreach (char key in this.ExtendedText.Equals(string.Empty) ? this.Text : this.ExtendedText)
    {
      BarcodeSymbolTable barcodeSymbol = this.BarcodeSymbols[key];
      num += barcodeSymbol.CheckDigit;
    }
    char[] chArray = new char[1];
    return this.GetCheckSumSymbols();
  }

  protected internal new char[] GetCheckSumSymbols()
  {
    string extendedText = this.ExtendedText;
    char[] checkSumSymbols = new char[2];
    int num1 = 0;
    string str1 = extendedText;
    int length1 = str1.Length;
    for (int index = 0; index < length1; ++index)
    {
      int num2 = (length1 - index) % 20;
      if (num2 == 0)
        num2 = 20;
      int checkDigit = this.BarcodeSymbols[str1[index]].CheckDigit;
      num1 += checkDigit * num2;
    }
    int num3 = num1 % 47;
    char ch1 = ' ';
    foreach (KeyValuePair<char, BarcodeSymbolTable> barcodeSymbol in this.BarcodeSymbols)
    {
      BarcodeSymbolTable barcodeSymbolTable = barcodeSymbol.Value;
      if (barcodeSymbolTable.CheckDigit == num3)
      {
        ch1 = barcodeSymbolTable.Symbol;
        break;
      }
    }
    string str2 = this.ExtendedText + (object) ch1;
    checkSumSymbols[0] = ch1;
    string str3 = str2;
    int num4 = 0;
    string str4 = str3;
    int length2 = str4.Length;
    for (int index = 0; index < length2; ++index)
    {
      int num5 = (length2 - index) % 15;
      if (num5 == 0)
        num5 = 15;
      int checkDigit = this.BarcodeSymbols[str4[index]].CheckDigit;
      num4 += checkDigit * num5;
    }
    int num6 = num4 % 47;
    string str5 = str3 + (object) num6;
    char ch2 = ' ';
    foreach (KeyValuePair<char, BarcodeSymbolTable> barcodeSymbol in this.BarcodeSymbols)
    {
      BarcodeSymbolTable barcodeSymbolTable = barcodeSymbol.Value;
      if (barcodeSymbolTable.CheckDigit == num6)
      {
        ch2 = barcodeSymbolTable.Symbol;
        break;
      }
    }
    string str6 = str2 + (object) ch2;
    checkSumSymbols[1] = ch2;
    return checkSumSymbols;
  }

  private void InitializeCode93Extended()
  {
    this.Initialize();
    this.ValidatorExpression = "^[\\x00-\\x7F\\x00fb\\x00fd\\x00fe\\'þ'\\'ü'\\'ý']+$";
    this.extendedCodes = new Dictionary<char, char[]>();
    this.extendedCodes[char.MinValue] = new char[2]
    {
      'ü',
      'U'
    };
    this.extendedCodes['\u0001'] = new char[2]{ 'û', 'A' };
    this.extendedCodes['\u0002'] = new char[2]{ 'û', 'B' };
    this.extendedCodes['\u0003'] = new char[2]{ 'û', 'C' };
    this.extendedCodes['\u0004'] = new char[2]{ 'û', 'D' };
    this.extendedCodes['\u0005'] = new char[2]{ 'û', 'E' };
    this.extendedCodes['\u0006'] = new char[2]{ 'û', 'F' };
    this.extendedCodes['\a'] = new char[2]{ 'û', 'G' };
    this.extendedCodes['\b'] = new char[2]{ 'û', 'H' };
    this.extendedCodes['\t'] = new char[2]{ 'û', 'I' };
    this.extendedCodes['\n'] = new char[2]{ 'û', 'J' };
    this.extendedCodes['\v'] = new char[2]{ 'û', 'K' };
    this.extendedCodes['\f'] = new char[2]{ 'û', 'L' };
    this.extendedCodes['\r'] = new char[2]{ 'û', 'M' };
    this.extendedCodes['\u000E'] = new char[2]{ 'û', 'N' };
    this.extendedCodes['\u000F'] = new char[2]{ 'û', 'O' };
    this.extendedCodes['\u0010'] = new char[2]{ 'û', 'P' };
    this.extendedCodes['\u0011'] = new char[2]{ 'û', 'Q' };
    this.extendedCodes['\u0012'] = new char[2]{ 'û', 'R' };
    this.extendedCodes['\u0013'] = new char[2]{ 'û', 'S' };
    this.extendedCodes['\u0014'] = new char[2]{ 'û', 'T' };
    this.extendedCodes['\u0015'] = new char[2]{ 'û', 'U' };
    this.extendedCodes['\u0016'] = new char[2]{ 'û', 'V' };
    this.extendedCodes['\u0017'] = new char[2]{ 'û', 'W' };
    this.extendedCodes['\u0018'] = new char[2]{ 'û', 'X' };
    this.extendedCodes['\u0019'] = new char[2]{ 'û', 'Y' };
    this.extendedCodes['\u001A'] = new char[2]{ 'û', 'Z' };
    this.extendedCodes['\u001B'] = new char[2]{ 'ü', 'A' };
    this.extendedCodes['\u001C'] = new char[2]{ 'ü', 'B' };
    this.extendedCodes['\u001D'] = new char[2]{ 'ü', 'C' };
    this.extendedCodes['\u001E'] = new char[2]{ 'ü', 'D' };
    this.extendedCodes['\u001F'] = new char[2]{ 'ü', 'E' };
    this.extendedCodes[' '] = new char[1]{ ' ' };
    this.extendedCodes['!'] = new char[2]{ 'ý', 'A' };
    this.extendedCodes['"'] = new char[2]{ 'ý', 'B' };
    this.extendedCodes['#'] = new char[2]{ 'ý', 'C' };
    this.extendedCodes['$'] = new char[1]{ '$' };
    this.extendedCodes['%'] = new char[1]{ '%' };
    this.extendedCodes['&'] = new char[2]{ 'ý', 'F' };
    this.extendedCodes['\''] = new char[2]{ 'ý', 'G' };
    this.extendedCodes['('] = new char[2]{ 'ý', 'H' };
    this.extendedCodes[')'] = new char[2]{ 'ý', 'I' };
    this.extendedCodes['*'] = new char[2]{ 'ý', 'J' };
    this.extendedCodes['+'] = new char[1]{ '+' };
    this.extendedCodes[','] = new char[2]{ 'ý', 'L' };
    this.extendedCodes['-'] = new char[1]{ '-' };
    this.extendedCodes['.'] = new char[1]{ '.' };
    this.extendedCodes['/'] = new char[1]{ '/' };
    this.extendedCodes['0'] = new char[1]{ '0' };
    this.extendedCodes['1'] = new char[1]{ '1' };
    this.extendedCodes['2'] = new char[1]{ '2' };
    this.extendedCodes['3'] = new char[1]{ '3' };
    this.extendedCodes['4'] = new char[1]{ '4' };
    this.extendedCodes['5'] = new char[1]{ '5' };
    this.extendedCodes['6'] = new char[1]{ '6' };
    this.extendedCodes['7'] = new char[1]{ '7' };
    this.extendedCodes['8'] = new char[1]{ '8' };
    this.extendedCodes['9'] = new char[1]{ '9' };
    this.extendedCodes[':'] = new char[2]{ 'ý', 'Z' };
    this.extendedCodes[';'] = new char[2]{ 'ü', 'F' };
    this.extendedCodes['<'] = new char[2]{ 'ü', 'G' };
    this.extendedCodes['='] = new char[2]{ 'ü', 'H' };
    this.extendedCodes['>'] = new char[2]{ 'ü', 'I' };
    this.extendedCodes['?'] = new char[2]{ 'ü', 'J' };
    this.extendedCodes['@'] = new char[2]{ 'ü', 'V' };
    this.extendedCodes['A'] = new char[1]{ 'A' };
    this.extendedCodes['B'] = new char[1]{ 'B' };
    this.extendedCodes['C'] = new char[1]{ 'C' };
    this.extendedCodes['D'] = new char[1]{ 'D' };
    this.extendedCodes['E'] = new char[1]{ 'E' };
    this.extendedCodes['F'] = new char[1]{ 'F' };
    this.extendedCodes['G'] = new char[1]{ 'G' };
    this.extendedCodes['H'] = new char[1]{ 'H' };
    this.extendedCodes['I'] = new char[1]{ 'I' };
    this.extendedCodes['J'] = new char[1]{ 'J' };
    this.extendedCodes['K'] = new char[1]{ 'K' };
    this.extendedCodes['L'] = new char[1]{ 'L' };
    this.extendedCodes['M'] = new char[1]{ 'M' };
    this.extendedCodes['N'] = new char[1]{ 'N' };
    this.extendedCodes['O'] = new char[1]{ 'O' };
    this.extendedCodes['P'] = new char[1]{ 'P' };
    this.extendedCodes['Q'] = new char[1]{ 'Q' };
    this.extendedCodes['R'] = new char[1]{ 'R' };
    this.extendedCodes['S'] = new char[1]{ 'S' };
    this.extendedCodes['T'] = new char[1]{ 'T' };
    this.extendedCodes['U'] = new char[1]{ 'U' };
    this.extendedCodes['V'] = new char[1]{ 'V' };
    this.extendedCodes['W'] = new char[1]{ 'W' };
    this.extendedCodes['X'] = new char[1]{ 'X' };
    this.extendedCodes['Y'] = new char[1]{ 'Y' };
    this.extendedCodes['Z'] = new char[1]{ 'Z' };
    this.extendedCodes['['] = new char[2]{ 'ü', 'K' };
    this.extendedCodes['\\'] = new char[2]{ 'ü', 'L' };
    this.extendedCodes[']'] = new char[2]{ 'ü', 'M' };
    this.extendedCodes['^'] = new char[2]{ 'ü', 'N' };
    this.extendedCodes['_'] = new char[2]{ 'ü', 'O' };
    this.extendedCodes['`'] = new char[2]{ 'ü', 'W' };
    this.extendedCodes['a'] = new char[2]{ 'þ', 'A' };
    this.extendedCodes['b'] = new char[2]{ 'þ', 'B' };
    this.extendedCodes['c'] = new char[2]{ 'þ', 'C' };
    this.extendedCodes['d'] = new char[2]{ 'þ', 'D' };
    this.extendedCodes['e'] = new char[2]{ 'þ', 'E' };
    this.extendedCodes['f'] = new char[2]{ 'þ', 'F' };
    this.extendedCodes['g'] = new char[2]{ 'þ', 'G' };
    this.extendedCodes['h'] = new char[2]{ 'þ', 'H' };
    this.extendedCodes['i'] = new char[2]{ 'þ', 'I' };
    this.extendedCodes['j'] = new char[2]{ 'þ', 'J' };
    this.extendedCodes['k'] = new char[2]{ 'þ', 'K' };
    this.extendedCodes['l'] = new char[2]{ 'þ', 'L' };
    this.extendedCodes['m'] = new char[2]{ 'þ', 'M' };
    this.extendedCodes['n'] = new char[2]{ 'þ', 'N' };
    this.extendedCodes['o'] = new char[2]{ 'þ', 'O' };
    this.extendedCodes['p'] = new char[2]{ 'þ', 'P' };
    this.extendedCodes['q'] = new char[2]{ 'þ', 'Q' };
    this.extendedCodes['r'] = new char[2]{ 'þ', 'R' };
    this.extendedCodes['s'] = new char[2]{ 'þ', 'S' };
    this.extendedCodes['t'] = new char[2]{ 'þ', 'T' };
    this.extendedCodes['u'] = new char[2]{ 'þ', 'U' };
    this.extendedCodes['v'] = new char[2]{ 'þ', 'V' };
    this.extendedCodes['w'] = new char[2]{ 'þ', 'W' };
    this.extendedCodes['x'] = new char[2]{ 'þ', 'X' };
    this.extendedCodes['y'] = new char[2]{ 'þ', 'Y' };
    this.extendedCodes['z'] = new char[2]{ 'þ', 'Z' };
    this.extendedCodes['{'] = new char[2]{ 'ü', 'P' };
    this.extendedCodes['|'] = new char[2]{ 'ü', 'Q' };
    this.extendedCodes['}'] = new char[2]{ 'ü', 'R' };
    this.extendedCodes['~'] = new char[2]{ 'ü', 'S' };
    this.extendedCodes['\u007F'] = new char[2]{ 'ü', 'T' };
  }

  private void GetExtendedText()
  {
    string text = this.Text;
    string str = "";
    foreach (char key in text)
    {
      foreach (char ch in this.extendedCodes[key])
        str += ch.ToString();
    }
    this.ExtendedText = str;
  }
}
