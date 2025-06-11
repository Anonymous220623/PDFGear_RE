// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfCodeUpcBarcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfCodeUpcBarcode : PdfUnidimensionalBarcode
{
  public PdfCodeUpcBarcode()
  {
    this.EnableCheckDigit = true;
    this.Initialize();
  }

  public PdfCodeUpcBarcode(string text)
    : this()
  {
    this.Text = text;
  }

  protected internal override char[] CalculateCheckDigit()
  {
    bool flag = double.TryParse(this.Text, out double _);
    if ((!flag || this.Text.Length != 11) && (!flag || this.Text.Length != 12))
      return (char[]) null;
    string str1 = this.ExtendedText.Equals(string.Empty) ? this.Text : this.ExtendedText;
    int num1 = 0;
    for (int index = 1; index <= str1.Length; ++index)
    {
      if (index <= 11)
      {
        int int32 = Convert.ToInt32(str1.Substring(index - 1, 1));
        if (index % 2 == 0)
          num1 += int32;
        else
          num1 += int32 * 3;
      }
    }
    int num2 = (10 - num1 % 10) % 10;
    char[] charArray = str1.ToCharArray();
    string str2 = string.Empty;
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (index == 0)
        str2 += (string) (object) 6;
      if (index == 6)
        str2 = $"{str2}B{charArray[index].ToString()}";
      else if (index != 11)
        str2 += charArray[index].ToString();
    }
    return (str2 + (object) num2 + (object) 6).ToCharArray();
  }

  private void Initialize()
  {
    this.StartSymbol = 'ý';
    this.StopSymbol = 'ÿ';
    this.ValidatorExpression = "^[\\x00-\\x7F]";
    this.BarcodeSymbols['0'] = new BarcodeSymbolTable('0', 16 /*0x10*/, new byte[4]
    {
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['1'] = new BarcodeSymbolTable('1', 17, new byte[4]
    {
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['2'] = new BarcodeSymbolTable('2', 18, new byte[4]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['3'] = new BarcodeSymbolTable('3', 19, new byte[4]
    {
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['4'] = new BarcodeSymbolTable('4', 20, new byte[4]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 2
    });
    this.BarcodeSymbols['5'] = new BarcodeSymbolTable('5', 21, new byte[4]
    {
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['6'] = new BarcodeSymbolTable('6', 22, new byte[4]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 4
    });
    this.BarcodeSymbols['7'] = new BarcodeSymbolTable('7', 23, new byte[4]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['8'] = new BarcodeSymbolTable('8', 24, new byte[4]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['9'] = new BarcodeSymbolTable('9', 25, new byte[4]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['B'] = new BarcodeSymbolTable('B', 11, new byte[4]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
  }
}
