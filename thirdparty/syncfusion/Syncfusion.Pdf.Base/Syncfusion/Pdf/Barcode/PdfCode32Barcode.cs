// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfCode32Barcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfCode32Barcode : PdfCode39Barcode
{
  private const int m_encodedtextLength = 6;
  private char[] checkSumSymbols;
  private bool isTextEncoded;
  private string encodedText = "";

  public PdfCode32Barcode() => this.InitializeCode32();

  public PdfCode32Barcode(string text)
    : this()
  {
    this.Text = text;
  }

  protected internal override char[] CalculateCheckDigit()
  {
    int num1 = (int) this.Text[0] - 48 /*0x30*/;
    int num2 = 2 * ((int) this.Text[1] - 48 /*0x30*/);
    int num3 = (int) this.Text[2] - 48 /*0x30*/;
    int num4 = 2 * ((int) this.Text[3] - 48 /*0x30*/);
    int num5 = (int) this.Text[4] - 48 /*0x30*/;
    int num6 = 2 * ((int) this.Text[5] - 48 /*0x30*/);
    int num7 = (int) this.Text[6] - 48 /*0x30*/;
    int num8 = 2 * ((int) this.Text[7] - 48 /*0x30*/);
    return new char[1]
    {
      (char) ((num2 / 10 + num4 / 10 + num6 / 10 + num8 / 10 + num2 % 10 + num4 % 10 + num6 % 10 + num8 % 10 + (num1 + num3 + num5 + num7)) % 10 + 48 /*0x30*/)
    };
  }

  protected string ObtainBarcodeSymbols()
  {
    string str = "";
    this.checkSumSymbols = this.CalculateCheckDigit();
    if (this.checkSumSymbols != null)
    {
      for (int index = 0; index < this.checkSumSymbols.Length; ++index)
      {
        if (this.EnableCheckDigit)
          str += (string) (object) this.checkSumSymbols[index];
      }
    }
    return this.GetDataToEncode(this.Text + str);
  }

  protected string GetDataToEncode(string originalData)
  {
    string dataToEncode = string.Empty;
    int num1 = int.Parse(originalData);
    while (num1 != 0)
    {
      int num2 = num1 % 32 /*0x20*/;
      num1 /= 32 /*0x20*/;
      foreach (KeyValuePair<char, BarcodeSymbolTable> barcodeSymbol in this.BarcodeSymbols)
      {
        BarcodeSymbolTable barcodeSymbolTable = barcodeSymbol.Value;
        if (barcodeSymbolTable.CheckDigit == num2)
          dataToEncode = barcodeSymbolTable.Symbol.ToString() + dataToEncode;
      }
    }
    if (dataToEncode.Length < 6)
      dataToEncode = new string('0', 6 - dataToEncode.Length) + dataToEncode;
    return dataToEncode;
  }

  internal override string GetTextToEncode()
  {
    if (this.isTextEncoded && this.encodedText != string.Empty)
      return this.encodedText;
    this.Text = this.Text.TrimStart('A');
    if (this.Text.Length != 8)
      throw new PdfBarcodeException("Barcode Text Length that are not accepted by this barcode specification.");
    if (!this.Validate(this.Text))
      throw new PdfBarcodeException("Barcode text contains characters that are not accepted by this barcode specification.");
    string str;
    if (!this.ExtendedText.Equals(string.Empty))
      str = this.ExtendedText.Trim('*');
    else
      str = this.Text.Trim('*');
    string barcodeSymbols = this.ObtainBarcodeSymbols();
    this.isTextEncoded = true;
    this.encodedText = barcodeSymbols;
    if (this.EnableCheckDigit)
      this.Text += (string) (object) this.checkSumSymbols[0];
    this.Text = "A" + this.Text;
    return this.encodedText;
  }

  private void InitializeCode32()
  {
    this.StartSymbol = '*';
    this.StopSymbol = '*';
    this.ValidatorExpression = "^[\\x41-\\x5A\\x30-\\x39\\x20\\-\\*\\.\\/\\+\\%]+$";
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
    this.BarcodeSymbols['B'] = new BarcodeSymbolTable('B', 10, new byte[9]
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
    this.BarcodeSymbols['C'] = new BarcodeSymbolTable('C', 11, new byte[9]
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
    this.BarcodeSymbols['D'] = new BarcodeSymbolTable('D', 12, new byte[9]
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
    this.BarcodeSymbols['F'] = new BarcodeSymbolTable('F', 13, new byte[9]
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
    this.BarcodeSymbols['G'] = new BarcodeSymbolTable('G', 14, new byte[9]
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
    this.BarcodeSymbols['H'] = new BarcodeSymbolTable('H', 15, new byte[9]
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
    this.BarcodeSymbols['J'] = new BarcodeSymbolTable('J', 16 /*0x10*/, new byte[9]
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
    this.BarcodeSymbols['K'] = new BarcodeSymbolTable('K', 17, new byte[9]
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
    this.BarcodeSymbols['L'] = new BarcodeSymbolTable('L', 18, new byte[9]
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
    this.BarcodeSymbols['M'] = new BarcodeSymbolTable('M', 19, new byte[9]
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
    this.BarcodeSymbols['N'] = new BarcodeSymbolTable('N', 20, new byte[9]
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
    this.BarcodeSymbols['P'] = new BarcodeSymbolTable('P', 21, new byte[9]
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
    this.BarcodeSymbols['Q'] = new BarcodeSymbolTable('Q', 22, new byte[9]
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
    this.BarcodeSymbols['R'] = new BarcodeSymbolTable('R', 23, new byte[9]
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
    this.BarcodeSymbols['S'] = new BarcodeSymbolTable('S', 24, new byte[9]
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
    this.BarcodeSymbols['T'] = new BarcodeSymbolTable('T', 25, new byte[9]
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
    this.BarcodeSymbols['U'] = new BarcodeSymbolTable('U', 26, new byte[9]
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
    this.BarcodeSymbols['V'] = new BarcodeSymbolTable('V', 27, new byte[9]
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
    this.BarcodeSymbols['W'] = new BarcodeSymbolTable('W', 28, new byte[9]
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
    this.BarcodeSymbols['X'] = new BarcodeSymbolTable('X', 29, new byte[9]
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
    this.BarcodeSymbols['Y'] = new BarcodeSymbolTable('Y', 30, new byte[9]
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
    this.BarcodeSymbols['Z'] = new BarcodeSymbolTable('Z', 31 /*0x1F*/, new byte[9]
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
}
