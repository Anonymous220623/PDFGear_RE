// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfPageLabel
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfPageLabel : IPdfWrapper
{
  private PdfDictionary m_dictionary = new PdfDictionary();
  private int m_startIndex = -1;

  public PdfNumberStyle NumberStyle
  {
    get
    {
      PdfNumberStyle numberStyle = PdfNumberStyle.None;
      PdfName pdfName = this.m_dictionary["S"] as PdfName;
      if (pdfName != (PdfName) null)
        numberStyle = PdfPageLabel.FromStringToStyle(pdfName.Value);
      return numberStyle;
    }
    set
    {
      string name = PdfPageLabel.FromStyleToString(value);
      if (name == null || name == string.Empty)
        this.m_dictionary.Remove("S");
      else
        this.m_dictionary.SetName("S", name);
    }
  }

  public string Prefix
  {
    get
    {
      string prefix = (string) null;
      if (this.m_dictionary["P"] is PdfString pdfString)
        prefix = pdfString.Value;
      return prefix;
    }
    set
    {
      if (value == null || value == string.Empty)
        this.m_dictionary.Remove("P");
      else
        this.m_dictionary.SetString("P", value);
    }
  }

  public int StartNumber
  {
    get
    {
      int startNumber = -1;
      if (this.m_dictionary["St"] is PdfNumber pdfNumber)
        startNumber = pdfNumber.IntValue;
      return startNumber;
    }
    set
    {
      if (value < 0)
        this.m_dictionary.Remove("St");
      else
        this.m_dictionary.SetNumber("St", value);
    }
  }

  public int StartPageIndex
  {
    get => this.m_startIndex;
    set
    {
      this.m_startIndex = value >= 0 ? value : throw new ArgumentException(" Start index not less than zero");
    }
  }

  public PdfPageLabel()
  {
    this.m_dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("PageLabel"));
    this.m_dictionary.SetProperty("S", (IPdfPrimitive) new PdfName("D"));
  }

  private static string FromStyleToString(PdfNumberStyle style)
  {
    string str = (string) null;
    switch (style)
    {
      case PdfNumberStyle.None:
        return str;
      case PdfNumberStyle.Numeric:
        str = "D";
        goto case PdfNumberStyle.None;
      case PdfNumberStyle.LowerLatin:
        str = "a";
        goto case PdfNumberStyle.None;
      case PdfNumberStyle.LowerRoman:
        str = "r";
        goto case PdfNumberStyle.None;
      case PdfNumberStyle.UpperLatin:
        str = "A";
        goto case PdfNumberStyle.None;
      case PdfNumberStyle.UpperRoman:
        str = "R";
        goto case PdfNumberStyle.None;
      default:
        throw new ArgumentException("Unsupported style.", nameof (style));
    }
  }

  private static PdfNumberStyle FromStringToStyle(string name)
  {
    PdfNumberStyle style = PdfNumberStyle.None;
    if (name != null && name != string.Empty)
    {
      switch (name)
      {
        case "D":
          style = PdfNumberStyle.Numeric;
          break;
        case "A":
          style = PdfNumberStyle.UpperLatin;
          break;
        case "a":
          style = PdfNumberStyle.LowerLatin;
          break;
        case "R":
          style = PdfNumberStyle.UpperRoman;
          break;
        case "r":
          style = PdfNumberStyle.LowerRoman;
          break;
        default:
          throw new ArgumentException("Unsupported style name.", nameof (name));
      }
    }
    return style;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
