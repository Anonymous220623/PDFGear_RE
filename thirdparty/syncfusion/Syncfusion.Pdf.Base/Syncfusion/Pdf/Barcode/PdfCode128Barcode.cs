// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfCode128Barcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfCode128Barcode : PdfUnidimensionalBarcode
{
  private static bool isnumb;

  public PdfCode128Barcode() => this.Initialize();

  public PdfCode128Barcode(string text)
    : this()
  {
    this.Text = text;
  }

  protected internal override char[] CalculateCheckDigit()
  {
    string str1 = (string) null;
    if (!this.EnableCheckDigit)
      return (char[]) null;
    int num1 = 0;
    string str2 = this.Text;
    if (str2.Length % 2 == 1)
      str2 = "0" + str2;
    for (int startIndex = 0; startIndex < str2.Length; startIndex += 2)
    {
      int num2 = int.Parse(str2.Substring(startIndex, 2));
      foreach (KeyValuePair<char, BarcodeSymbolTable> barcodeSymbol in this.BarcodeSymbols)
      {
        BarcodeSymbolTable barcodeSymbolTable = barcodeSymbol.Value;
        if ((int) barcodeSymbolTable.Symbol == (int) (ushort) num2)
        {
          if (barcodeSymbolTable != null)
            num1 += barcodeSymbolTable.CheckDigit * (startIndex / 2 + 1);
          str1 += (string) (object) barcodeSymbolTable.Symbol;
          break;
        }
      }
    }
    char symbol = this.GetSymbol((num1 + 105) % 103);
    this.Text = str1;
    return new char[1]{ symbol };
  }

  internal override string GetTextToEncode()
  {
    string text = this.Text;
    if (!this.Validate(this.Text))
      throw new PdfBarcodeException("Barcode text contains characters that are not accepted by this barcode specification.");
    string str;
    if (!this.ExtendedText.Equals(string.Empty))
      str = this.ExtendedText.Trim('*');
    else
      str = this.Text.Trim('*');
    string textToEncode = str;
    if (this.isCheckDigitAdded || !this.EnableCheckDigit)
      return textToEncode;
    char[] checkDigit = this.CalculateCheckDigit();
    if (checkDigit == null || checkDigit.Length == 0)
      return textToEncode;
    if (this.ShowCheckDigit && !this.isCheckDigitAdded)
    {
      if ((int) textToEncode[textToEncode.Length - 1] != (int) checkDigit[checkDigit.Length - 1])
      {
        foreach (char ch in checkDigit)
          textToEncode += ch.ToString();
      }
      this.isCheckDigitAdded = true;
      if (this.ExtendedText.Equals(string.Empty))
      {
        textToEncode = this.Text;
        foreach (char ch in checkDigit)
          textToEncode += ch.ToString();
      }
    }
    this.Text = text;
    if (this.ShowCheckDigit)
    {
      foreach (char ch in checkDigit)
        this.Text += ch.ToString();
    }
    this.isCheckDigitAdded = true;
    if (this.ExtendedText.Equals(string.Empty))
    {
      textToEncode = this.Text;
      foreach (char ch in checkDigit)
        textToEncode += ch.ToString();
    }
    return textToEncode;
  }

  protected string GetDataToEncode(string originalData)
  {
    if (originalData.Length % 2 == 0)
      originalData.Split('(');
    StringBuilder stringBuilder = new StringBuilder();
    string str = originalData;
    if (originalData.Length % 2 == 1)
      str = "0" + str;
    for (int startIndex = 0; startIndex < str.Length; startIndex += 2)
    {
      char ch = (char) int.Parse(str.Substring(startIndex, 2));
      stringBuilder.Append(ch);
    }
    return stringBuilder.ToString();
  }

  private void Initialize()
  {
    this.StartSymbol = '\u0087';
    this.StopSymbol = '\u0088';
    this.ValidatorExpression = "^[\\x00-\\x7F]";
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

  protected override List<byte[]> GetTextToEncodeList()
  {
    string text = this.Text;
    PdfCode128BBarcode pdfCode128Bbarcode = new PdfCode128BBarcode();
    PdfCode128CBarcode pdfCode128Cbarcode = new PdfCode128CBarcode();
    int num1 = 1;
    int num2 = 0;
    List<byte[]> textToEncodeList = new List<byte[]>();
    int num3;
    if (text.Length >= 2)
    {
      string key = text.Substring(0, 2);
      if (pdfCode128Cbarcode.BarcodeSymbolsString.ContainsKey(key))
      {
        textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString["StartCodeC"].Bars);
        num3 = num2 + num1 * pdfCode128Cbarcode.BarcodeSymbolsString["StartCodeC"].CheckDigit;
      }
      else
      {
        textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString["StartCodeB"].Bars);
        num3 = num2 + num1 * pdfCode128Cbarcode.BarcodeSymbolsString["StartCodeB"].CheckDigit;
      }
    }
    else
    {
      textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString["StartCodeB"].Bars);
      num3 = num2 + num1 * pdfCode128Cbarcode.BarcodeSymbolsString["StartCodeB"].CheckDigit;
    }
    bool flag = false;
    string str = text;
    while (str.Length > 0)
    {
      if (str.Length == 1)
      {
        char key = str[0];
        if (PdfCode128Barcode.isnumb)
        {
          textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString["CodeB"].Bars);
          num3 += num1++ * pdfCode128Cbarcode.BarcodeSymbolsString["CodeB"].CheckDigit;
        }
        textToEncodeList.Add(pdfCode128Bbarcode.BarcodeSymbols[key].Bars);
        int num4 = num3;
        int num5 = num1;
        int num6 = num5 + 1;
        int num7 = num5 * pdfCode128Bbarcode.BarcodeSymbols[key].CheckDigit;
        num3 = num4 + num7;
        flag = true;
        break;
      }
      string key1 = str.Substring(0, 2);
      if (!flag && pdfCode128Cbarcode.BarcodeSymbolsString.ContainsKey(key1))
      {
        textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString[key1].Bars);
        num3 += num1++ * pdfCode128Cbarcode.BarcodeSymbolsString[key1].CheckDigit;
        str = str.Remove(0, 2);
        PdfCode128Barcode.isnumb = true;
        flag = false;
      }
      else
      {
        if (!flag && PdfCode128Barcode.isnumb)
        {
          textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString["CodeB"].Bars);
          num3 += num1++ * pdfCode128Cbarcode.BarcodeSymbolsString["CodeB"].CheckDigit;
        }
        textToEncodeList.Add(pdfCode128Bbarcode.BarcodeSymbols[key1[0]].Bars);
        int num8 = num3;
        int num9 = num1;
        int num10 = num9 + 1;
        int num11 = num9 * pdfCode128Bbarcode.BarcodeSymbols[key1[0]].CheckDigit;
        int num12 = num8 + num11;
        textToEncodeList.Add(pdfCode128Bbarcode.BarcodeSymbols[key1[1]].Bars);
        int num13 = num12;
        int num14 = num10;
        num1 = num14 + 1;
        int num15 = num14 * pdfCode128Bbarcode.BarcodeSymbols[key1[1]].CheckDigit;
        num3 = num13 + num15;
        str = str.Remove(0, 2);
        flag = true;
      }
    }
    if (flag)
    {
      char symbol = pdfCode128Bbarcode.GetSymbol(num3 % 103);
      textToEncodeList.Add(pdfCode128Bbarcode.BarcodeSymbols[symbol].Bars);
    }
    else
    {
      string key = (num3 % 103).ToString();
      if (key.Length == 1)
        key = "0" + key;
      textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString[key].Bars);
    }
    PdfCode128Barcode.isnumb = false;
    textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString["Stop"].Bars);
    return textToEncodeList;
  }
}
