// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfCode128CBarcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfCode128CBarcode : PdfUnidimensionalBarcode
{
  public PdfCode128CBarcode()
  {
    this.Initialize();
    this.EnableCheckDigit = true;
  }

  public PdfCode128CBarcode(string text)
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

  protected internal override bool Validate(string data)
  {
    return new Regex(this.ValidatorExpression, RegexOptions.Compiled).Matches(data).Count == data.Length;
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
    if (this.EnableCheckDigit && !this.isCheckDigitAdded)
    {
      foreach (char ch in checkDigit)
        textToEncode += ch.ToString();
    }
    if (!this.isCheckDigitAdded)
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
    return textToEncode;
  }

  protected override List<byte[]> GetTextToEncodeList()
  {
    string str = this.Text;
    if (!this.Validate(this.Text))
      throw new PdfBarcodeException("Barcode text contains characters that are not accepted by this barcode specification.");
    if (this.Text.Length % 2 != 0 && this.Text.Length % 2 != 0)
    {
      this.Text = "0" + this.Text;
      str = this.Text;
    }
    int num1 = 1;
    int num2 = 0;
    List<byte[]> textToEncodeList = new List<byte[]>();
    textToEncodeList.Add(this.BarcodeSymbolsString["StartCodeC"].Bars);
    int num3 = num2 + num1 * this.BarcodeSymbolsString["StartCodeC"].CheckDigit;
    for (; str != string.Empty; str = str.Remove(0, 2))
    {
      string key = str.Substring(0, 2);
      textToEncodeList.Add(this.BarcodeSymbolsString[key].Bars);
      num3 += num1++ * this.BarcodeSymbolsString[key].CheckDigit;
    }
    string key1 = (num3 % 103).ToString();
    if (key1.Length == 1)
      key1 = "0" + key1;
    textToEncodeList.Add(this.BarcodeSymbolsString[key1].Bars);
    textToEncodeList.Add(this.BarcodeSymbolsString["Stop"].Bars);
    return textToEncodeList;
  }

  protected string GetDataToEncode(string originalData)
  {
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
    this.StartSymbol = 'þ';
    this.StopSymbol = 'ÿ';
    this.ValidatorExpression = "[0-9]";
    this.BarcodeSymbolsString["00"] = new BarcodeSymbolTable(0, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbolsString["01"] = new BarcodeSymbolTable(1, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbolsString["02"] = new BarcodeSymbolTable(2, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbolsString["03"] = new BarcodeSymbolTable(3, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbolsString["04"] = new BarcodeSymbolTable(4, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbolsString["05"] = new BarcodeSymbolTable(5, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbolsString["06"] = new BarcodeSymbolTable(6, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbolsString["07"] = new BarcodeSymbolTable(7, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["08"] = new BarcodeSymbolTable(8, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["09"] = new BarcodeSymbolTable(9, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbolsString["10"] = new BarcodeSymbolTable(10, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["11"] = new BarcodeSymbolTable(11, new byte[6]
    {
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["12"] = new BarcodeSymbolTable(12, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 2
    });
    this.BarcodeSymbolsString["13"] = new BarcodeSymbolTable(13, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 2
    });
    this.BarcodeSymbolsString["14"] = new BarcodeSymbolTable(14, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbolsString["15"] = new BarcodeSymbolTable(15, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbolsString["16"] = new BarcodeSymbolTable(16 /*0x10*/, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbolsString["17"] = new BarcodeSymbolTable(17, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 2,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbolsString["18"] = new BarcodeSymbolTable(18, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["19"] = new BarcodeSymbolTable(19, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 2
    });
    this.BarcodeSymbolsString["20"] = new BarcodeSymbolTable(20, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbolsString["21"] = new BarcodeSymbolTable(21, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["22"] = new BarcodeSymbolTable(22, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["23"] = new BarcodeSymbolTable(23, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbolsString["24"] = new BarcodeSymbolTable(24, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbolsString["25"] = new BarcodeSymbolTable(25, new byte[6]
    {
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbolsString["26"] = new BarcodeSymbolTable(26, new byte[6]
    {
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbolsString["27"] = new BarcodeSymbolTable(27, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["28"] = new BarcodeSymbolTable(28, new byte[6]
    {
      (byte) 3,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["29"] = new BarcodeSymbolTable(29, new byte[6]
    {
      (byte) 3,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["30"] = new BarcodeSymbolTable(30, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbolsString["31"] = new BarcodeSymbolTable(31 /*0x1F*/, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbolsString["32"] = new BarcodeSymbolTable(32 /*0x20*/, new byte[6]
    {
      (byte) 2,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbolsString["33"] = new BarcodeSymbolTable(33, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbolsString["34"] = new BarcodeSymbolTable(34, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbolsString["35"] = new BarcodeSymbolTable(35, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbolsString["36"] = new BarcodeSymbolTable(36, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbolsString["37"] = new BarcodeSymbolTable(37, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbolsString["38"] = new BarcodeSymbolTable(38, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["39"] = new BarcodeSymbolTable(39, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbolsString["40"] = new BarcodeSymbolTable(40, new byte[6]
    {
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbolsString["41"] = new BarcodeSymbolTable(41, new byte[6]
    {
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["42"] = new BarcodeSymbolTable(42, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 3
    });
    this.BarcodeSymbolsString["43"] = new BarcodeSymbolTable(43, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbolsString["44"] = new BarcodeSymbolTable(44, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbolsString["45"] = new BarcodeSymbolTable(45, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbolsString["46"] = new BarcodeSymbolTable(46, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbolsString["47"] = new BarcodeSymbolTable(47, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbolsString["48"] = new BarcodeSymbolTable(48 /*0x30*/, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbolsString["49"] = new BarcodeSymbolTable(49, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbolsString["50"] = new BarcodeSymbolTable(50, new byte[6]
    {
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbolsString["51"] = new BarcodeSymbolTable(51, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbolsString["52"] = new BarcodeSymbolTable(52, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["53"] = new BarcodeSymbolTable(53, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbolsString["54"] = new BarcodeSymbolTable(54, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbolsString["55"] = new BarcodeSymbolTable(55, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbolsString["56"] = new BarcodeSymbolTable(56, new byte[6]
    {
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbolsString["57"] = new BarcodeSymbolTable(57, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbolsString["58"] = new BarcodeSymbolTable(58, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["59"] = new BarcodeSymbolTable(59, new byte[6]
    {
      (byte) 3,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["60"] = new BarcodeSymbolTable(60, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["61"] = new BarcodeSymbolTable(61, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["62"] = new BarcodeSymbolTable(62, new byte[6]
    {
      (byte) 4,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["63"] = new BarcodeSymbolTable(63 /*0x3F*/, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 4
    });
    this.BarcodeSymbolsString["64"] = new BarcodeSymbolTable(64 /*0x40*/, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbolsString["65"] = new BarcodeSymbolTable(65, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 4
    });
    this.BarcodeSymbolsString["66"] = new BarcodeSymbolTable(66, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 4,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbolsString["67"] = new BarcodeSymbolTable(67, new byte[6]
    {
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbolsString["68"] = new BarcodeSymbolTable(68, new byte[6]
    {
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbolsString["69"] = new BarcodeSymbolTable(69, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 4
    });
    this.BarcodeSymbolsString["70"] = new BarcodeSymbolTable(70, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["71"] = new BarcodeSymbolTable(71, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 4
    });
    this.BarcodeSymbolsString["72"] = new BarcodeSymbolTable(72, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 4,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["73"] = new BarcodeSymbolTable(73, new byte[6]
    {
      (byte) 1,
      (byte) 4,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["74"] = new BarcodeSymbolTable(74, new byte[6]
    {
      (byte) 1,
      (byte) 4,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["75"] = new BarcodeSymbolTable(75, new byte[6]
    {
      (byte) 2,
      (byte) 4,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["76"] = new BarcodeSymbolTable(76, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 4
    });
    this.BarcodeSymbolsString["77"] = new BarcodeSymbolTable(77, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["78"] = new BarcodeSymbolTable(78, new byte[6]
    {
      (byte) 2,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["79"] = new BarcodeSymbolTable(79, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["80"] = new BarcodeSymbolTable(80 /*0x50*/, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 2
    });
    this.BarcodeSymbolsString["81"] = new BarcodeSymbolTable(81, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 2
    });
    this.BarcodeSymbolsString["82"] = new BarcodeSymbolTable(82, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbolsString["83"] = new BarcodeSymbolTable(83, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["84"] = new BarcodeSymbolTable(84, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["85"] = new BarcodeSymbolTable(85, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["86"] = new BarcodeSymbolTable(86, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["87"] = new BarcodeSymbolTable(87, new byte[6]
    {
      (byte) 4,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["88"] = new BarcodeSymbolTable(88, new byte[6]
    {
      (byte) 4,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["89"] = new BarcodeSymbolTable(89, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbolsString["90"] = new BarcodeSymbolTable(90, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbolsString["91"] = new BarcodeSymbolTable(91, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbolsString["92"] = new BarcodeSymbolTable(92, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 3
    });
    this.BarcodeSymbolsString["93"] = new BarcodeSymbolTable(93, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbolsString["94"] = new BarcodeSymbolTable(94, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbolsString["95"] = new BarcodeSymbolTable(95, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbolsString["96"] = new BarcodeSymbolTable(96 /*0x60*/, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["97"] = new BarcodeSymbolTable(97, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbolsString["98"] = new BarcodeSymbolTable(98, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbolsString["99"] = new BarcodeSymbolTable(99, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbolsString["100"] = new BarcodeSymbolTable(100, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbolsString["101"] = new BarcodeSymbolTable(101, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbolsString["102"] = new BarcodeSymbolTable(102, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbolsString["CodeB"] = new BarcodeSymbolTable(100, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbolsString["CodeA"] = new BarcodeSymbolTable(101, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbolsString["FNC1"] = new BarcodeSymbolTable(102, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbolsString["StartCodeA"] = new BarcodeSymbolTable(103, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["StartCodeB"] = new BarcodeSymbolTable(104, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 4
    });
    this.BarcodeSymbolsString["StartCodeC"] = new BarcodeSymbolTable(105, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 2
    });
    this.BarcodeSymbolsString["Stop"] = new BarcodeSymbolTable(106, new byte[7]
    {
      (byte) 2,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbolsString["CodeC"] = new BarcodeSymbolTable(99, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbols[char.MinValue] = new BarcodeSymbolTable(char.MinValue, 0, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['\u0001'] = new BarcodeSymbolTable('\u0001', 1, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['\u0002'] = new BarcodeSymbolTable('\u0002', 2, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['\u0003'] = new BarcodeSymbolTable('\u0003', 3, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbols['\u0004'] = new BarcodeSymbolTable('\u0004', 4, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['\u0005'] = new BarcodeSymbolTable('\u0005', 5, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['\u0006'] = new BarcodeSymbolTable('\u0006', 6, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['\a'] = new BarcodeSymbolTable('\a', 7, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['\b'] = new BarcodeSymbolTable('\b', 8, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['\t'] = new BarcodeSymbolTable('\t', 9, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['\n'] = new BarcodeSymbolTable('\n', 10, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['\v'] = new BarcodeSymbolTable('\v', 11, new byte[6]
    {
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['\f'] = new BarcodeSymbolTable('\f', 12, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 2
    });
    this.BarcodeSymbols['\r'] = new BarcodeSymbolTable('\r', 13, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 2
    });
    this.BarcodeSymbols['\u000E'] = new BarcodeSymbolTable('\u000E', 14, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['\u000F'] = new BarcodeSymbolTable('\u000F', 15, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['\u0010'] = new BarcodeSymbolTable('\u0010', 16 /*0x10*/, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['\u0011'] = new BarcodeSymbolTable('\u0011', 17, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 2,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['\u0012'] = new BarcodeSymbolTable('\u0012', 18, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['\u0013'] = new BarcodeSymbolTable('\u0013', 19, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 2
    });
    this.BarcodeSymbols['\u0014'] = new BarcodeSymbolTable('\u0014', 20, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['\u0015'] = new BarcodeSymbolTable('\u0015', 21, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['\u0016'] = new BarcodeSymbolTable('\u0016', 22, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['\u0017'] = new BarcodeSymbolTable('\u0017', 23, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['\u0018'] = new BarcodeSymbolTable('\u0018', 24, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['\u0019'] = new BarcodeSymbolTable('\u0019', 25, new byte[6]
    {
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['\u001A'] = new BarcodeSymbolTable('\u001A', 26, new byte[6]
    {
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['\u001B'] = new BarcodeSymbolTable('\u001B', 27, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['\u001C'] = new BarcodeSymbolTable('\u001C', 28, new byte[6]
    {
      (byte) 3,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['\u001D'] = new BarcodeSymbolTable('\u001D', 29, new byte[6]
    {
      (byte) 3,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['\u001E'] = new BarcodeSymbolTable('\u001E', 30, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbols['\u001F'] = new BarcodeSymbolTable('\u001F', 31 /*0x1F*/, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols[' '] = new BarcodeSymbolTable(' ', 32 /*0x20*/, new byte[6]
    {
      (byte) 2,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['!'] = new BarcodeSymbolTable('!', 33, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbols['"'] = new BarcodeSymbolTable('"', 34, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbols['#'] = new BarcodeSymbolTable('#', 35, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['$'] = new BarcodeSymbolTable('$', 36, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['%'] = new BarcodeSymbolTable('%', 37, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['&'] = new BarcodeSymbolTable('&', 38, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['\''] = new BarcodeSymbolTable('\'', 39, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['('] = new BarcodeSymbolTable('(', 40, new byte[6]
    {
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols[')'] = new BarcodeSymbolTable(')', 41, new byte[6]
    {
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['*'] = new BarcodeSymbolTable('*', 42, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 3
    });
    this.BarcodeSymbols['+'] = new BarcodeSymbolTable('+', 43, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols[','] = new BarcodeSymbolTable(',', 44, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['-'] = new BarcodeSymbolTable('-', 45, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbols['.'] = new BarcodeSymbolTable('.', 46, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['/'] = new BarcodeSymbolTable('/', 47, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['0'] = new BarcodeSymbolTable('0', 48 /*0x30*/, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['1'] = new BarcodeSymbolTable('1', 49, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['2'] = new BarcodeSymbolTable('2', 50, new byte[6]
    {
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['3'] = new BarcodeSymbolTable('3', 51, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['4'] = new BarcodeSymbolTable('4', 52, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['5'] = new BarcodeSymbolTable('5', 53, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1
    });
    this.BarcodeSymbols['6'] = new BarcodeSymbolTable('6', 54, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 3
    });
    this.BarcodeSymbols['7'] = new BarcodeSymbolTable('7', 55, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['8'] = new BarcodeSymbolTable('8', 56, new byte[6]
    {
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['9'] = new BarcodeSymbolTable('9', 57, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols[':'] = new BarcodeSymbolTable(':', 58, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols[';'] = new BarcodeSymbolTable(';', 59, new byte[6]
    {
      (byte) 3,
      (byte) 3,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['<'] = new BarcodeSymbolTable('<', 60, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['='] = new BarcodeSymbolTable('=', 61, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['>'] = new BarcodeSymbolTable('>', 62, new byte[6]
    {
      (byte) 4,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['?'] = new BarcodeSymbolTable('?', 63 /*0x3F*/, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 4
    });
    this.BarcodeSymbols['@'] = new BarcodeSymbolTable('@', 64 /*0x40*/, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['A'] = new BarcodeSymbolTable('A', 65, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 4
    });
    this.BarcodeSymbols['B'] = new BarcodeSymbolTable('B', 66, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 4,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['C'] = new BarcodeSymbolTable('C', 67, new byte[6]
    {
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['D'] = new BarcodeSymbolTable('D', 68, new byte[6]
    {
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['E'] = new BarcodeSymbolTable('E', 69, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 4
    });
    this.BarcodeSymbols['F'] = new BarcodeSymbolTable('F', 70, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['G'] = new BarcodeSymbolTable('G', 71, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 4
    });
    this.BarcodeSymbols['H'] = new BarcodeSymbolTable('H', 72, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 4,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['I'] = new BarcodeSymbolTable('I', 73, new byte[6]
    {
      (byte) 1,
      (byte) 4,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['J'] = new BarcodeSymbolTable('J', 74, new byte[6]
    {
      (byte) 1,
      (byte) 4,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['K'] = new BarcodeSymbolTable('K', 75, new byte[6]
    {
      (byte) 2,
      (byte) 4,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['L'] = new BarcodeSymbolTable('L', 76, new byte[6]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 4
    });
    this.BarcodeSymbols['M'] = new BarcodeSymbolTable('M', 77, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['N'] = new BarcodeSymbolTable('N', 78, new byte[6]
    {
      (byte) 2,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['O'] = new BarcodeSymbolTable('O', 79, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['P'] = new BarcodeSymbolTable('P', 80 /*0x50*/, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 2
    });
    this.BarcodeSymbols['Q'] = new BarcodeSymbolTable('Q', 81, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 2
    });
    this.BarcodeSymbols['R'] = new BarcodeSymbolTable('R', 82, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbols['S'] = new BarcodeSymbolTable('S', 83, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['T'] = new BarcodeSymbolTable('T', 84, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['U'] = new BarcodeSymbolTable('U', 85, new byte[6]
    {
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['V'] = new BarcodeSymbolTable('V', 86, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['W'] = new BarcodeSymbolTable('W', 87, new byte[6]
    {
      (byte) 4,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['X'] = new BarcodeSymbolTable('X', 88, new byte[6]
    {
      (byte) 4,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['Y'] = new BarcodeSymbolTable('Y', 89, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbols['Z'] = new BarcodeSymbolTable('Z', 90, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['['] = new BarcodeSymbolTable('[', 91, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['\\'] = new BarcodeSymbolTable('\\', 92, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 3
    });
    this.BarcodeSymbols[']'] = new BarcodeSymbolTable(']', 93, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbols['^'] = new BarcodeSymbolTable('^', 94, new byte[6]
    {
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 1
    });
    this.BarcodeSymbols['_'] = new BarcodeSymbolTable('_', 95, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['`'] = new BarcodeSymbolTable('`', 96 /*0x60*/, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['a'] = new BarcodeSymbolTable('a', 97, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3
    });
    this.BarcodeSymbols['b'] = new BarcodeSymbolTable('b', 98, new byte[6]
    {
      (byte) 4,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['c'] = new BarcodeSymbolTable('c', 99, new byte[6]
    {
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 4,
      (byte) 1
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
    this.BarcodeSymbols['ú'] = new BarcodeSymbolTable('ú', 101, new byte[6]
    {
      (byte) 3,
      (byte) 1,
      (byte) 1,
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
    this.BarcodeSymbols['þ'] = new BarcodeSymbolTable('þ', 105, new byte[6]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 3,
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
