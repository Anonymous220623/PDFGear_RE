// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfCode39ExtendedBarcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfCode39ExtendedBarcode : PdfCode39Barcode
{
  private Dictionary<char, char[]> extendedCodes;

  public PdfCode39ExtendedBarcode() => this.InitializeCode39Extended();

  public PdfCode39ExtendedBarcode(string text)
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
    if (!this.EnableCheckDigit)
      return (char[]) null;
    int num = 0;
    this.GetExtendedTextValue();
    foreach (char key in this.ExtendedText.Equals(string.Empty) ? this.Text : this.ExtendedText)
    {
      BarcodeSymbolTable barcodeSymbol = this.BarcodeSymbols[key];
      num += barcodeSymbol.CheckDigit;
    }
    return new char[1]
    {
      this.GetSymbol(num % (this.BarcodeSymbols.Count - 1))
    };
  }

  private void InitializeCode39Extended()
  {
    this.Initialize();
    this.ValidatorExpression = "^[\\x00-\\x7F]+$";
    this.extendedCodes = new Dictionary<char, char[]>();
    this.extendedCodes[char.MinValue] = new char[2]
    {
      '%',
      'U'
    };
    this.extendedCodes['\u0001'] = new char[2]{ '$', 'A' };
    this.extendedCodes['\u0002'] = new char[2]{ '$', 'B' };
    this.extendedCodes['\u0003'] = new char[2]{ '$', 'C' };
    this.extendedCodes['\u0004'] = new char[2]{ '$', 'D' };
    this.extendedCodes['\u0005'] = new char[2]{ '$', 'E' };
    this.extendedCodes['\u0006'] = new char[2]{ '$', 'F' };
    this.extendedCodes['\a'] = new char[2]{ '$', 'G' };
    this.extendedCodes['\b'] = new char[2]{ '$', 'H' };
    this.extendedCodes['\t'] = new char[2]{ '$', 'I' };
    this.extendedCodes['\n'] = new char[2]{ '$', 'J' };
    this.extendedCodes['\v'] = new char[2]{ '$', 'K' };
    this.extendedCodes['\f'] = new char[2]{ '$', 'L' };
    this.extendedCodes['\r'] = new char[2]{ '$', 'M' };
    this.extendedCodes['\u000E'] = new char[2]{ '$', 'N' };
    this.extendedCodes['\u000F'] = new char[2]{ '$', 'O' };
    this.extendedCodes['\u0010'] = new char[2]{ '$', 'P' };
    this.extendedCodes['\u0011'] = new char[2]{ '$', 'Q' };
    this.extendedCodes['\u0012'] = new char[2]{ '$', 'R' };
    this.extendedCodes['\u0013'] = new char[2]{ '$', 'S' };
    this.extendedCodes['\u0014'] = new char[2]{ '$', 'T' };
    this.extendedCodes['\u0015'] = new char[2]{ '$', 'U' };
    this.extendedCodes['\u0016'] = new char[2]{ '$', 'V' };
    this.extendedCodes['\u0017'] = new char[2]{ '$', 'W' };
    this.extendedCodes['\u0018'] = new char[2]{ '$', 'X' };
    this.extendedCodes['\u0019'] = new char[2]{ '$', 'Y' };
    this.extendedCodes['\u001A'] = new char[2]{ '$', 'Z' };
    this.extendedCodes['\u001B'] = new char[2]{ '%', 'A' };
    this.extendedCodes['\u001C'] = new char[2]{ '%', 'B' };
    this.extendedCodes['\u001D'] = new char[2]{ '%', 'C' };
    this.extendedCodes['\u001E'] = new char[2]{ '%', 'D' };
    this.extendedCodes['\u001F'] = new char[2]{ '%', 'E' };
    this.extendedCodes[' '] = new char[1]{ ' ' };
    this.extendedCodes['!'] = new char[2]{ '/', 'A' };
    this.extendedCodes['"'] = new char[2]{ '/', 'B' };
    this.extendedCodes['#'] = new char[2]{ '/', 'C' };
    this.extendedCodes['$'] = new char[2]{ '/', 'D' };
    this.extendedCodes['%'] = new char[2]{ '/', 'E' };
    this.extendedCodes['&'] = new char[2]{ '/', 'F' };
    this.extendedCodes['\''] = new char[2]{ '/', 'G' };
    this.extendedCodes['('] = new char[2]{ '/', 'H' };
    this.extendedCodes[')'] = new char[2]{ '/', 'I' };
    this.extendedCodes['*'] = new char[2]{ '/', 'J' };
    this.extendedCodes['+'] = new char[2]{ '/', 'K' };
    this.extendedCodes[','] = new char[2]{ '/', 'L' };
    this.extendedCodes['-'] = new char[1]{ '-' };
    this.extendedCodes['.'] = new char[1]{ '.' };
    this.extendedCodes['/'] = new char[2]{ '/', 'O' };
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
    this.extendedCodes[':'] = new char[2]{ '/', 'Z' };
    this.extendedCodes[';'] = new char[2]{ '%', 'F' };
    this.extendedCodes['<'] = new char[2]{ '%', 'G' };
    this.extendedCodes['='] = new char[2]{ '%', 'H' };
    this.extendedCodes['>'] = new char[2]{ '%', 'I' };
    this.extendedCodes['?'] = new char[2]{ '%', 'J' };
    this.extendedCodes['@'] = new char[2]{ '%', 'V' };
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
    this.extendedCodes['['] = new char[2]{ '%', 'K' };
    this.extendedCodes['\\'] = new char[2]{ '%', 'L' };
    this.extendedCodes[']'] = new char[2]{ '%', 'M' };
    this.extendedCodes['^'] = new char[2]{ '%', 'N' };
    this.extendedCodes['_'] = new char[2]{ '%', 'O' };
    this.extendedCodes['`'] = new char[2]{ '%', 'W' };
    this.extendedCodes['a'] = new char[2]{ '+', 'A' };
    this.extendedCodes['b'] = new char[2]{ '+', 'B' };
    this.extendedCodes['c'] = new char[2]{ '+', 'C' };
    this.extendedCodes['d'] = new char[2]{ '+', 'D' };
    this.extendedCodes['e'] = new char[2]{ '+', 'E' };
    this.extendedCodes['f'] = new char[2]{ '+', 'F' };
    this.extendedCodes['g'] = new char[2]{ '+', 'G' };
    this.extendedCodes['h'] = new char[2]{ '+', 'H' };
    this.extendedCodes['i'] = new char[2]{ '+', 'I' };
    this.extendedCodes['j'] = new char[2]{ '+', 'J' };
    this.extendedCodes['k'] = new char[2]{ '+', 'K' };
    this.extendedCodes['l'] = new char[2]{ '+', 'L' };
    this.extendedCodes['m'] = new char[2]{ '+', 'M' };
    this.extendedCodes['n'] = new char[2]{ '+', 'N' };
    this.extendedCodes['o'] = new char[2]{ '+', 'O' };
    this.extendedCodes['p'] = new char[2]{ '+', 'P' };
    this.extendedCodes['q'] = new char[2]{ '+', 'Q' };
    this.extendedCodes['r'] = new char[2]{ '+', 'R' };
    this.extendedCodes['s'] = new char[2]{ '+', 'S' };
    this.extendedCodes['t'] = new char[2]{ '+', 'T' };
    this.extendedCodes['u'] = new char[2]{ '+', 'U' };
    this.extendedCodes['v'] = new char[2]{ '+', 'V' };
    this.extendedCodes['w'] = new char[2]{ '+', 'W' };
    this.extendedCodes['x'] = new char[2]{ '+', 'X' };
    this.extendedCodes['y'] = new char[2]{ '+', 'Y' };
    this.extendedCodes['z'] = new char[2]{ '+', 'Z' };
    this.extendedCodes['{'] = new char[2]{ '%', 'P' };
    this.extendedCodes['|'] = new char[2]{ '%', 'Q' };
    this.extendedCodes['}'] = new char[2]{ '%', 'R' };
    this.extendedCodes['~'] = new char[2]{ '%', 'S' };
    this.extendedCodes['\u007F'] = new char[2]{ '%', 'T' };
  }

  private char GetSymbol(int checkValue)
  {
    foreach (KeyValuePair<char, BarcodeSymbolTable> barcodeSymbol in this.BarcodeSymbols)
    {
      BarcodeSymbolTable barcodeSymbolTable = barcodeSymbol.Value;
      if (barcodeSymbolTable.CheckDigit == checkValue)
        return barcodeSymbolTable.Symbol;
    }
    return char.MinValue;
  }

  protected internal override void GetExtendedTextValue()
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
