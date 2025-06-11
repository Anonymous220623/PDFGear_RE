// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfCode128ABarcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfCode128ABarcode : PdfUnidimensionalBarcode
{
  public PdfCode128ABarcode()
  {
    this.EnableCheckDigit = true;
    this.Initialize();
  }

  public PdfCode128ABarcode(string text)
    : this()
  {
    this.Text = text;
  }

  protected internal override char[] CalculateCheckDigit()
  {
    if (!this.EnableCheckDigit)
      return (char[]) null;
    int num1 = 0;
    string str = this.ExtendedText.Equals(string.Empty) ? this.Text : this.ExtendedText;
    int num2 = 0;
    foreach (char key in str)
    {
      BarcodeSymbolTable barcodeSymbol = this.BarcodeSymbols[key];
      if (barcodeSymbol == null)
        throw new PdfBarcodeException("Barcode Text contains characters that are not accepted by this barcode specification.");
      num1 += barcodeSymbol.CheckDigit * (num2 + 1);
      ++num2;
    }
    return new char[1]{ this.GetSymbol((num1 + 103) % 103) };
  }

  protected internal override bool Validate(string data)
  {
    return new Regex(this.ValidatorExpression, RegexOptions.Compiled).Matches(data).Count == data.Length;
  }

  protected void Initialize()
  {
    this.StartSymbol = 'ù';
    this.StopSymbol = 'ÿ';
    this.ValidatorExpression = "[\0\u0001\u0002\u0003\u0004\u0005\u0006\a\b\t\n\v\f\r\u000E\u000F\u0010\u0011\u0012\u0013\u0014\u0015\u0016\u0017\u0018\u0019\u001A\u001B\u001C\u001D\u001E\u001F !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_ðñòóô]";
    this.BarcodeSymbols[char.MinValue] = new BarcodeSymbolTable(char.MinValue, 64 /*0x40*/, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['\u0001'] = new BarcodeSymbolTable('\u0001', 65, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 4
    });
    this.BarcodeSymbols['\u0002'] = new BarcodeSymbolTable('\u0002', 66, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 4,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['\u0003'] = new BarcodeSymbolTable('\u0003', 67, new byte[6]
    {
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['\u0004'] = new BarcodeSymbolTable('\u0004', 68, new byte[6]
    {
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['\u0005'] = new BarcodeSymbolTable('\u0005', 69, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 4
    });
    this.BarcodeSymbols['\u0006'] = new BarcodeSymbolTable('\u0006', 70, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['\a'] = new BarcodeSymbolTable('\a', 71, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 4
    });
    this.BarcodeSymbols['\b'] = new BarcodeSymbolTable('\b', 72, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 4,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['\t'] = new BarcodeSymbolTable('\t', 73, new byte[6]
    {
      (byte) 1,
      (byte) 4,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['\n'] = new BarcodeSymbolTable('\n', 74, new byte[6]
    {
      (byte) 1,
      (byte) 4,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['\v'] = new BarcodeSymbolTable('\v', 75, new byte[6]
    {
      (byte) 2,
      (byte) 4,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['\f'] = new BarcodeSymbolTable('\f', 76, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 4
    });
    this.BarcodeSymbols['\r'] = new BarcodeSymbolTable('\r', 77, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['\u000E'] = new BarcodeSymbolTable('\u000E', 78, new byte[6]
    {
      (byte) 2,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['\u000F'] = new BarcodeSymbolTable('\u000F', 79, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['\u0010'] = new BarcodeSymbolTable('\u0010', 80 /*0x50*/, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 2
    });
    this.BarcodeSymbols['\u0011'] = new BarcodeSymbolTable('\u0011', 81, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 2
    });
    this.BarcodeSymbols['\u0012'] = new BarcodeSymbolTable('\u0012', 82, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbols['\u0013'] = new BarcodeSymbolTable('\u0013', 83, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['\u0014'] = new BarcodeSymbolTable('\u0014', 84, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['\u0015'] = new BarcodeSymbolTable('\u0015', 85, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['\u0016'] = new BarcodeSymbolTable('\u0016', 86, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['\u0017'] = new BarcodeSymbolTable('\u0017', 87, new byte[6]
    {
      (byte) 4,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['\u0018'] = new BarcodeSymbolTable('\u0018', 88, new byte[6]
    {
      (byte) 4,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['\u0019'] = new BarcodeSymbolTable('\u0019', 89, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbols['\u001A'] = new BarcodeSymbolTable('\u001A', 90, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['\u001B'] = new BarcodeSymbolTable('\u001B', 91, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['\u001C'] = new BarcodeSymbolTable('\u001C', 92, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 3
    });
    this.BarcodeSymbols['\u001D'] = new BarcodeSymbolTable('\u001D', 93, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbols['\u001E'] = new BarcodeSymbolTable('\u001E', 94, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbols['\u001F'] = new BarcodeSymbolTable('\u001F', 95, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols[' '] = new BarcodeSymbolTable(' ', 0, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['!'] = new BarcodeSymbolTable('!', 1, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['"'] = new BarcodeSymbolTable('"', 2, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['#'] = new BarcodeSymbolTable('#', 3, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbols['$'] = new BarcodeSymbolTable('$', 4, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['%'] = new BarcodeSymbolTable('%', 5, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['&'] = new BarcodeSymbolTable('&', 6, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['\''] = new BarcodeSymbolTable('\'', 7, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['('] = new BarcodeSymbolTable('(', 8, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols[')'] = new BarcodeSymbolTable(')', 9, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['*'] = new BarcodeSymbolTable('*', 10, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['+'] = new BarcodeSymbolTable('+', 11, new byte[6]
    {
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols[','] = new BarcodeSymbolTable(',', 12, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 2
    });
    this.BarcodeSymbols['-'] = new BarcodeSymbolTable('-', 13, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 2
    });
    this.BarcodeSymbols['.'] = new BarcodeSymbolTable('.', 14, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['/'] = new BarcodeSymbolTable('/', 15, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['0'] = new BarcodeSymbolTable('0', 16 /*0x10*/, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['1'] = new BarcodeSymbolTable('1', 17, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 2,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['2'] = new BarcodeSymbolTable('2', 18, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['3'] = new BarcodeSymbolTable('3', 19, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 2
    });
    this.BarcodeSymbols['4'] = new BarcodeSymbolTable('4', 20, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['5'] = new BarcodeSymbolTable('5', 21, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['6'] = new BarcodeSymbolTable('6', 22, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['7'] = new BarcodeSymbolTable('7', 23, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['8'] = new BarcodeSymbolTable('8', 24, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['9'] = new BarcodeSymbolTable('9', 25, new byte[6]
    {
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols[':'] = new BarcodeSymbolTable(':', 26, new byte[6]
    {
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols[';'] = new BarcodeSymbolTable(';', 27, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['<'] = new BarcodeSymbolTable('<', 28, new byte[6]
    {
      (byte) 3,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['='] = new BarcodeSymbolTable('=', 29, new byte[6]
    {
      (byte) 3,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['>'] = new BarcodeSymbolTable('>', 30, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbols['?'] = new BarcodeSymbolTable('?', 31 /*0x1F*/, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['@'] = new BarcodeSymbolTable('@', 32 /*0x20*/, new byte[6]
    {
      (byte) 2,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['A'] = new BarcodeSymbolTable('A', 33, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbols['B'] = new BarcodeSymbolTable('B', 34, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbols['C'] = new BarcodeSymbolTable('C', 35, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['D'] = new BarcodeSymbolTable('D', 36, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['E'] = new BarcodeSymbolTable('E', 37, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['F'] = new BarcodeSymbolTable('F', 38, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['G'] = new BarcodeSymbolTable('G', 39, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['H'] = new BarcodeSymbolTable('H', 40, new byte[6]
    {
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['I'] = new BarcodeSymbolTable('I', 41, new byte[6]
    {
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['J'] = new BarcodeSymbolTable('J', 42, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 3
    });
    this.BarcodeSymbols['K'] = new BarcodeSymbolTable('K', 43, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['L'] = new BarcodeSymbolTable('L', 44, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['M'] = new BarcodeSymbolTable('M', 45, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbols['N'] = new BarcodeSymbolTable('N', 46, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['O'] = new BarcodeSymbolTable('O', 47, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['P'] = new BarcodeSymbolTable('P', 48 /*0x30*/, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['Q'] = new BarcodeSymbolTable('Q', 49, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['R'] = new BarcodeSymbolTable('R', 50, new byte[6]
    {
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['S'] = new BarcodeSymbolTable('S', 51, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['T'] = new BarcodeSymbolTable('T', 52, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['U'] = new BarcodeSymbolTable('U', 53, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['V'] = new BarcodeSymbolTable('V', 54, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbols['W'] = new BarcodeSymbolTable('W', 55, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['X'] = new BarcodeSymbolTable('X', 56, new byte[6]
    {
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['Y'] = new BarcodeSymbolTable('Y', 57, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['Z'] = new BarcodeSymbolTable('Z', 58, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['['] = new BarcodeSymbolTable('[', 59, new byte[6]
    {
      (byte) 3,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['\\'] = new BarcodeSymbolTable('\\', 60, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols[']'] = new BarcodeSymbolTable(']', 61, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['^'] = new BarcodeSymbolTable('^', 62, new byte[6]
    {
      (byte) 4,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['_'] = new BarcodeSymbolTable('_', 63 /*0x3F*/, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 4
    });
    this.BarcodeSymbols['ð'] = new BarcodeSymbolTable('ð', 102, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['ñ'] = new BarcodeSymbolTable('ñ', 97, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['ò'] = new BarcodeSymbolTable('ò', 96 /*0x60*/, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['ó'] = new BarcodeSymbolTable('ó', 101, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbols['ô'] = new BarcodeSymbolTable('ô', 98, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['ü'] = new BarcodeSymbolTable('ü', 99, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbols['û'] = new BarcodeSymbolTable('û', 100, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['ù'] = new BarcodeSymbolTable('ù', 103, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['ÿ'] = new BarcodeSymbolTable('ÿ', -1, new byte[7]
    {
      (byte) 2,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
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
}
