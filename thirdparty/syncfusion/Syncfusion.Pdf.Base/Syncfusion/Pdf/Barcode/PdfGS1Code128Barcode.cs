// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfGS1Code128Barcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfGS1Code128Barcode : PdfUnidimensionalBarcode
{
  public PdfGS1Code128Barcode() => this.Initialize();

  public PdfGS1Code128Barcode(string text)
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
    string str1 = this.Text.Replace("[FNC1]", "");
    if (!this.Validate(this.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("[FNC1]", "")))
      throw new PdfBarcodeException("Barcode text contains characters that are not accepted by this barcode specification.");
    string str2;
    if (!this.ExtendedText.Equals(string.Empty))
      str2 = this.ExtendedText.Trim('*');
    else
      str2 = this.Text.Trim('*');
    string textToEncode = str2;
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
    this.Text = str1;
    if (this.ShowCheckDigit)
    {
      foreach (char ch in checkDigit)
        this.Text += ch.ToString();
    }
    this.isCheckDigitAdded = true;
    if (this != null)
    {
      string str3 = this.Text;
      string str4 = "";
      for (; str3.Length > 0; str3 = str3.Remove(0, 2))
      {
        int num = int.Parse(str3.Substring(0, 2));
        str4 = num >= 95 ? str4 + (object) (char) (num + 37) : str4 + (object) (char) (num + 32 /*0x20*/);
      }
      textToEncode = ((int) this.StartSymbol + 134).ToString() + str4 + (object) this.StopSymbol;
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
    this.ValidatorExpression = "^[a-zA-Z0-9_]*$";
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
    string str = this.Text.Replace("[FNC1]", "");
    if (!this.Validate(this.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("[FNC1]", "")))
      throw new PdfBarcodeException("Barcode text contains characters that are not accepted by this barcode specification.");
    PdfCode128BBarcode pdfCode128Bbarcode = new PdfCode128BBarcode();
    PdfCode128CBarcode pdfCode128Cbarcode = new PdfCode128CBarcode();
    int num1 = 1;
    int num2 = 0;
    List<byte[]> textToEncodeList = new List<byte[]>();
    textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString["StartCodeC"].Bars);
    int num3 = num2 + num1 * pdfCode128Cbarcode.BarcodeSymbolsString["StartCodeC"].CheckDigit;
    textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString["FNC1"].Bars);
    int num4 = num3;
    int num5 = num1;
    int num6 = num5 + 1;
    int num7 = num5 * pdfCode128Cbarcode.BarcodeSymbolsString["FNC1"].CheckDigit;
    int num8 = num4 + num7;
    string[] strArray1 = str.Split('(');
    bool flag = false;
    for (int index = 0; index < strArray1.Length; ++index)
    {
      if (!string.IsNullOrEmpty(strArray1[index]))
      {
        string[] strArray2 = strArray1[index].Split(')');
        strArray2[0] = strArray2[0].Replace("(", "");
        strArray2[0] = strArray2[0].Replace(")", "");
        int aiLength = this.GetAILength(strArray2[0]);
        if (aiLength != 0 && strArray2[1].Trim().Length != aiLength)
          throw new PdfBarcodeException($"({strArray2[0].ToString()}) AI should have the value of length {(object) aiLength}");
        string s = strArray1[index].Replace("(", "").Replace(")", "");
        while (s.Length > 0)
        {
          if (s.Length == 1)
          {
            char key = s[0];
            textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString["CodeB"].Bars);
            int num9 = num8;
            int num10 = num6;
            int num11 = num10 + 1;
            int num12 = num10 * pdfCode128Cbarcode.BarcodeSymbolsString["CodeB"].CheckDigit;
            int num13 = num9 + num12;
            textToEncodeList.Add(pdfCode128Bbarcode.BarcodeSymbols[key].Bars);
            int num14 = num13;
            int num15 = num11;
            num6 = num15 + 1;
            int num16 = num15 * pdfCode128Bbarcode.BarcodeSymbols[key].CheckDigit;
            num8 = num14 + num16;
            flag = true;
            break;
          }
          string key1 = s.Substring(0, 2);
          long result = 0;
          if (!flag && pdfCode128Cbarcode.BarcodeSymbolsString.ContainsKey(key1))
          {
            textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString[key1].Bars);
            num8 += num6++ * pdfCode128Cbarcode.BarcodeSymbolsString[key1].CheckDigit;
            s = s.Remove(0, 2);
            flag = false;
          }
          else if (flag && long.TryParse(s, out result))
          {
            textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString["CodeC"].Bars);
            int num17 = num8;
            int num18 = num6;
            int num19 = num18 + 1;
            int num20 = num18 * pdfCode128Cbarcode.BarcodeSymbolsString["CodeC"].CheckDigit;
            int num21 = num17 + num20;
            textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString[key1].Bars);
            int num22 = num21;
            int num23 = num19;
            num6 = num23 + 1;
            int num24 = num23 * pdfCode128Cbarcode.BarcodeSymbolsString[key1].CheckDigit;
            num8 = num22 + num24;
            s = s.Remove(0, 2);
            flag = false;
          }
          else
          {
            if (!flag)
            {
              textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString["CodeB"].Bars);
              num8 += num6++ * pdfCode128Cbarcode.BarcodeSymbolsString["CodeB"].CheckDigit;
            }
            textToEncodeList.Add(pdfCode128Bbarcode.BarcodeSymbols[key1[0]].Bars);
            int num25 = num8;
            int num26 = num6;
            int num27 = num26 + 1;
            int num28 = num26 * pdfCode128Bbarcode.BarcodeSymbols[key1[0]].CheckDigit;
            int num29 = num25 + num28;
            textToEncodeList.Add(pdfCode128Bbarcode.BarcodeSymbols[key1[1]].Bars);
            int num30 = num29;
            int num31 = num27;
            num6 = num31 + 1;
            int num32 = num31 * pdfCode128Bbarcode.BarcodeSymbols[key1[1]].CheckDigit;
            num8 = num30 + num32;
            s = s.Remove(0, 2);
            flag = true;
          }
        }
        if (aiLength == 0 && index + 1 < strArray1.Length)
        {
          textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString["FNC1"].Bars);
          num8 += num6++ * pdfCode128Cbarcode.BarcodeSymbolsString["FNC1"].CheckDigit;
        }
      }
    }
    if (flag)
    {
      char symbol = pdfCode128Bbarcode.GetSymbol(num8 % 103);
      textToEncodeList.Add(pdfCode128Bbarcode.BarcodeSymbols[symbol].Bars);
    }
    else
    {
      string key = (num8 % 103).ToString();
      if (key.Length == 1)
        key = "0" + key;
      textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString[key].Bars);
    }
    textToEncodeList.Add(pdfCode128Cbarcode.BarcodeSymbolsString["Stop"].Bars);
    return textToEncodeList;
  }

  private int GetAILength(string ai)
  {
    switch (ai)
    {
      case "00":
        return 18;
      case "01":
        return 14;
      case "02":
        return 14;
      case "03":
        return 14;
      case "04":
        return 16 /*0x10*/;
      case "11":
        return 6;
      case "12":
        return 6;
      case "13":
        return 6;
      case "14":
        return 4;
      case "15":
        return 6;
      case "16":
        return 6;
      case "17":
        return 6;
      case "18":
        return 6;
      case "19":
        return 6;
      case "20":
        return 2;
      case "31":
        return 6;
      case "32":
        return 6;
      case "33":
        return 6;
      case "34":
        return 6;
      case "35":
        return 6;
      case "36":
        return 6;
      case "41":
        return 13;
      default:
        return 0;
    }
  }
}
