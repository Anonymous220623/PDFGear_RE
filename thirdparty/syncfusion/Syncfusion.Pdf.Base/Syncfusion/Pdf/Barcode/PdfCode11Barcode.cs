// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfCode11Barcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfCode11Barcode : PdfUnidimensionalBarcode
{
  public PdfCode11Barcode() => this.Initialize();

  public PdfCode11Barcode(string text)
    : this()
  {
    this.Text = text;
  }

  protected internal override char[] CalculateCheckDigit()
  {
    string text = this.Text;
    int num1 = 0;
    int num2 = 1;
    while (num1 != -1)
    {
      int[] numArray = new int[text.Length];
      for (int index = text.Length - 1; index >= 0; --index)
      {
        numArray[index] = num2;
        ++num2;
        if (num2 == 11 && num1 == 0)
          num2 = 1;
        if (num2 == 10 && num1 == 1)
          num2 = 1;
      }
      int num3 = 0;
      for (int index = text.Length - 1; index >= 0; --index)
      {
        if (text[index] == '-')
        {
          num3 += 10 * numArray[index];
        }
        else
        {
          int num4 = int.Parse(text[index].ToString());
          num3 += num4 * numArray[index];
        }
      }
      char symbol = this.GetSymbol(num3 % 11);
      text += symbol.ToString();
      if (text.Length >= 10 && text.Length - this.Text.Length <= 2 && num1 != 1)
        ++num1;
      else
        num1 = -1;
    }
    return text.ToCharArray(this.Text.Length, text.Length - this.Text.Length);
  }

  private void Initialize()
  {
    this.StartSymbol = '*';
    this.StopSymbol = '*';
    this.ValidatorExpression = "^[0-9\\-]*$";
    this.BarcodeSymbols['0'] = new BarcodeSymbolTable('0', 0, new byte[5]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['1'] = new BarcodeSymbolTable('1', 1, new byte[5]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['2'] = new BarcodeSymbolTable('2', 2, new byte[5]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['3'] = new BarcodeSymbolTable('3', 3, new byte[5]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['4'] = new BarcodeSymbolTable('4', 4, new byte[5]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['5'] = new BarcodeSymbolTable('5', 5, new byte[5]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['6'] = new BarcodeSymbolTable('6', 6, new byte[5]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['7'] = new BarcodeSymbolTable('7', 7, new byte[5]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['8'] = new BarcodeSymbolTable('8', 8, new byte[5]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['9'] = new BarcodeSymbolTable('9', 9, new byte[5]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['-'] = new BarcodeSymbolTable('-', 10, new byte[5]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['*'] = new BarcodeSymbolTable('*', 0, new byte[5]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1
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
