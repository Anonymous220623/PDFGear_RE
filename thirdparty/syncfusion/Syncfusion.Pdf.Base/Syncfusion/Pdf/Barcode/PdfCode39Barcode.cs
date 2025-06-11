// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfCode39Barcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfCode39Barcode : PdfUnidimensionalBarcode
{
  public PdfCode39Barcode()
  {
    if (this is PdfCode32Barcode)
      return;
    this.Initialize();
  }

  public PdfCode39Barcode(string text)
    : this()
  {
    this.Text = text;
  }

  internal void Initialize()
  {
    this.StartSymbol = '*';
    this.StopSymbol = '*';
    this.ValidatorExpression = "^[\\x41-\\x5A\\x30-\\x39\\x20\\-\\.\\$\\/\\+\\%\\ ]+$";
    this.BarcodeSymbols['0'] = new BarcodeSymbolTable('0', 0, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['1'] = new BarcodeSymbolTable('1', 1, new byte[9]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['2'] = new BarcodeSymbolTable('2', 2, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['3'] = new BarcodeSymbolTable('3', 3, new byte[9]
    {
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['4'] = new BarcodeSymbolTable('4', 4, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['5'] = new BarcodeSymbolTable('5', 5, new byte[9]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['6'] = new BarcodeSymbolTable('6', 6, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['7'] = new BarcodeSymbolTable('7', 7, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['8'] = new BarcodeSymbolTable('8', 8, new byte[9]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['9'] = new BarcodeSymbolTable('9', 9, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['A'] = new BarcodeSymbolTable('A', 10, new byte[9]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['B'] = new BarcodeSymbolTable('B', 11, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['C'] = new BarcodeSymbolTable('C', 12, new byte[9]
    {
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['D'] = new BarcodeSymbolTable('D', 13, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['E'] = new BarcodeSymbolTable('E', 14, new byte[9]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['F'] = new BarcodeSymbolTable('F', 15, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['G'] = new BarcodeSymbolTable('G', 16 /*0x10*/, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['H'] = new BarcodeSymbolTable('H', 17, new byte[9]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['I'] = new BarcodeSymbolTable('I', 18, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['J'] = new BarcodeSymbolTable('J', 19, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['K'] = new BarcodeSymbolTable('K', 20, new byte[9]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3
    });
    this.BarcodeSymbols['L'] = new BarcodeSymbolTable('L', 21, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3
    });
    this.BarcodeSymbols['M'] = new BarcodeSymbolTable('M', 22, new byte[9]
    {
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['N'] = new BarcodeSymbolTable('N', 23, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3
    });
    this.BarcodeSymbols['O'] = new BarcodeSymbolTable('O', 24, new byte[9]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['P'] = new BarcodeSymbolTable('P', 25, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['Q'] = new BarcodeSymbolTable('Q', 26, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 3
    });
    this.BarcodeSymbols['R'] = new BarcodeSymbolTable('R', 27, new byte[9]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['S'] = new BarcodeSymbolTable('S', 28, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['T'] = new BarcodeSymbolTable('T', 29, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['U'] = new BarcodeSymbolTable('U', 30, new byte[9]
    {
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['V'] = new BarcodeSymbolTable('V', 31 /*0x1F*/, new byte[9]
    {
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['W'] = new BarcodeSymbolTable('W', 32 /*0x20*/, new byte[9]
    {
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['X'] = new BarcodeSymbolTable('X', 33, new byte[9]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['Y'] = new BarcodeSymbolTable('Y', 34, new byte[9]
    {
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['Z'] = new BarcodeSymbolTable('Z', 35, new byte[9]
    {
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['-'] = new BarcodeSymbolTable('-', 36, new byte[9]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['.'] = new BarcodeSymbolTable('.', 37, new byte[9]
    {
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols[' '] = new BarcodeSymbolTable(' ', 38, new byte[9]
    {
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['$'] = new BarcodeSymbolTable('$', 39, new byte[9]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['/'] = new BarcodeSymbolTable('/', 40, new byte[9]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['+'] = new BarcodeSymbolTable('+', 41, new byte[9]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['%'] = new BarcodeSymbolTable('%', 42, new byte[9]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['*'] = new BarcodeSymbolTable('*', 0, new byte[9]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
  }

  protected internal override char[] CalculateCheckDigit()
  {
    if (!this.EnableCheckDigit)
      return (char[]) null;
    int num = 0;
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
