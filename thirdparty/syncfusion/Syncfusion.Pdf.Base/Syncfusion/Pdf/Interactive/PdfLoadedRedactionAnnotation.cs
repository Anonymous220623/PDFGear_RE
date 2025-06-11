// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedRedactionAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Redaction;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedRedactionAnnotation : PdfLoadedStyledAnnotation
{
  private PdfColor borderColor;
  private LineBorder border = new LineBorder();
  private string overlayText = string.Empty;
  private PdfColor textColor;
  private bool repeat;
  private PdfFont font;
  private PdfTextAlignment alignment;
  private PdfCrossTable crossTable;
  private bool isfontAPStream;
  private bool flatten;
  private MemoryStream redactTextStream;
  private PdfRecordCollection readTextCollection;
  private ContentParser parser;
  private PdfDictionary dictionary = new PdfDictionary();
  internal bool AppearanceEnabled;

  public PdfColor TextColor
  {
    get => this.ObtainTextColor();
    set
    {
      this.textColor = value;
      if (this.textColor.A == (byte) 0)
        return;
      this.Dictionary.SetProperty("C", (IPdfPrimitive) this.textColor.ToArray());
    }
  }

  public PdfTextAlignment TextAlignment
  {
    get => this.ObtainTextAlignment();
    set
    {
      this.alignment = value;
      this.Dictionary.SetProperty("Q", (IPdfPrimitive) new PdfNumber((int) this.alignment));
    }
  }

  public LineBorder Border
  {
    get => this.border;
    set => this.border = value;
  }

  public string OverlayText
  {
    get => this.ObtainOverlayText();
    set
    {
      this.overlayText = value;
      this.Dictionary.SetString(nameof (OverlayText), this.overlayText);
    }
  }

  public PdfFont Font
  {
    get => this.font == null ? this.ObtainFont() : this.font;
    set => this.font = value;
  }

  public PdfColor BorderColor
  {
    get => this.ObtainBorderColor();
    set
    {
      this.borderColor = value;
      if (this.borderColor.A == (byte) 0)
        return;
      this.Dictionary.SetProperty("OC", (IPdfPrimitive) this.borderColor.ToArray());
    }
  }

  public bool RepeatText
  {
    get => this.ObtainTextRepeat();
    set
    {
      this.repeat = value;
      this.Dictionary.SetBoolean("Repeat", this.repeat);
    }
  }

  public new bool Flatten
  {
    get => this.flatten;
    set
    {
      this.flatten = value;
      if (!this.flatten)
        return;
      this.ApplyRedaction();
    }
  }

  internal PdfLoadedRedactionAnnotation(PdfDictionary dictionary, PdfCrossTable crossTable)
    : base(dictionary, crossTable)
  {
    this.crossTable = crossTable;
    this.Dictionary = dictionary;
  }

  protected override void Save()
  {
    this.CheckFlatten();
    PdfTemplate normalAppearance = this.CreateNormalAppearance(this.OverlayText, this.Font, this.RepeatText, this.TextColor, this.TextAlignment, this.Border);
    if (this.Flatten || this.Page.Annotations.Flatten)
    {
      this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
    }
    else
    {
      this.Appearance.Normal = this.CreateBorderAppearance(this.BorderColor, this.Border);
      this.Appearance.MouseHover = normalAppearance;
      this.Dictionary.SetProperty("AP", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.Appearance));
    }
  }

  private void ApplyRedaction()
  {
    PdfTemplate normalAppearance = this.CreateNormalAppearance(this.OverlayText, this.Font, this.RepeatText, this.TextColor, this.TextAlignment, this.Border);
    this.Page.Redactions.Add(new PdfRedaction(this.Bounds)
    {
      m_success = true,
      Appearance = normalAppearance
    });
  }

  private string ObtainOverlayText()
  {
    string empty = string.Empty;
    if (this.Dictionary.ContainsKey("OverlayText") && PdfCrossTable.Dereference(this.Dictionary["OverlayText"]) is PdfString pdfString)
      empty = pdfString.Value.ToString();
    return empty;
  }

  private bool ObtainTextRepeat()
  {
    bool textRepeat = false;
    if (this.Dictionary.ContainsKey("Repeat") && PdfCrossTable.Dereference(this.Dictionary["Repeat"]) is PdfBoolean pdfBoolean)
      textRepeat = pdfBoolean.Value;
    return textRepeat;
  }

  private PdfColor ObtainBorderColor()
  {
    PdfColor borderColor = PdfColor.Empty;
    if (this.Dictionary.ContainsKey("OC"))
      borderColor = this.GetColorFromArray(PdfCrossTable.Dereference(this.Dictionary["OC"]) as PdfArray);
    return borderColor;
  }

  private PdfColor ObtainTextColor()
  {
    PdfColor textColor = PdfColor.Empty;
    if (this.Dictionary.ContainsKey("C"))
      textColor = this.GetColorFromArray(PdfCrossTable.Dereference(this.Dictionary["C"]) as PdfArray);
    return textColor;
  }

  private PdfColor GetColorFromArray(PdfArray colours)
  {
    PdfColor colorFromArray = PdfColor.Empty;
    if (colours != null)
    {
      if (colours.Count == 3 && colours[0] is PdfNumber && colours[1] is PdfNumber && colours[2] is PdfNumber)
        colorFromArray = new PdfColor((byte) Math.Round((double) (colours[0] as PdfNumber).FloatValue * (double) byte.MaxValue), (byte) Math.Round((double) (colours[1] as PdfNumber).FloatValue * (double) byte.MaxValue), (byte) Math.Round((double) (colours[2] as PdfNumber).FloatValue * (double) byte.MaxValue));
      if (colours.Count == 4 && colours[0] is PdfNumber && colours[1] is PdfNumber && colours[2] is PdfNumber && colours[3] is PdfNumber)
        colorFromArray = new PdfColor((byte) Math.Round((double) (colours[0] as PdfNumber).FloatValue * (double) byte.MaxValue), (byte) Math.Round((double) (colours[1] as PdfNumber).FloatValue * (double) byte.MaxValue), (byte) Math.Round((double) (colours[2] as PdfNumber).FloatValue * (double) byte.MaxValue), (byte) Math.Round((double) (colours[3] as PdfNumber).FloatValue * (double) byte.MaxValue));
      if (colours.Count == 1 && colours[0] is PdfNumber)
        colorFromArray = new PdfColor((float) Math.Round((double) (colours[0] as PdfNumber).FloatValue * (double) byte.MaxValue));
    }
    return colorFromArray;
  }

  private PdfTextAlignment ObtainTextAlignment()
  {
    PdfTextAlignment textAlignment = PdfTextAlignment.Left;
    if (this.Dictionary.ContainsKey("Q") && PdfCrossTable.Dereference(this.Dictionary["Q"]) is PdfNumber pdfNumber)
      textAlignment = (PdfTextAlignment) Enum.ToObject(typeof (PdfTextAlignment), pdfNumber.IntValue);
    return textAlignment;
  }

  private PdfFont ObtainFont()
  {
    if (this.Dictionary["AP"] != null)
    {
      this.isfontAPStream = true;
      this.ObtainFromAPStream(this.isfontAPStream);
    }
    return this.font;
  }

  private void ObtainFromAPStream(bool isfontStream)
  {
    if (!(PdfCrossTable.Dereference(this.Dictionary["AP"]) is PdfDictionary pdfDictionary1) || !(PdfCrossTable.Dereference(pdfDictionary1["R"]) is PdfDictionary pdfDictionary2))
      return;
    if (!(pdfDictionary2["Resources"] is PdfDictionary pdfDictionary3))
    {
      PdfReferenceHolder pdfReferenceHolder = pdfDictionary2["Resources"] as PdfReferenceHolder;
      if (pdfReferenceHolder != (PdfReferenceHolder) null)
        pdfDictionary3 = pdfReferenceHolder.Object as PdfDictionary;
    }
    if (pdfDictionary3 == null || !(pdfDictionary2 is PdfStream pdfStream))
      return;
    byte[] buffer = PdfString.StringToByte("\r\n");
    pdfStream.Decompress();
    if (this.redactTextStream == null)
      this.redactTextStream = new MemoryStream();
    pdfStream.InternalStream.WriteTo((Stream) this.redactTextStream);
    this.redactTextStream.Write(buffer, 0, buffer.Length);
    if (this.parser == null)
      this.parser = new ContentParser(this.redactTextStream.ToArray());
    this.readTextCollection = this.parser.ReadContent();
    if (this.readTextCollection == null || !isfontStream)
      return;
    string str1 = "";
    float num = 1f;
    foreach (PdfRecord readText in this.readTextCollection)
    {
      string empty = string.Empty;
      string[] operands = readText.Operands;
      if (readText.OperatorName == "Tf")
      {
        string[] strArray = operands;
        string str2 = strArray[0];
        PdfDictionary pdfDictionary4 = pdfDictionary3["Font"] as PdfDictionary;
        if (str2.Contains("/"))
        {
          string key = str2.Trim('/');
          str1 = ((PdfCrossTable.Dereference(pdfDictionary4[key]) as PdfDictionary)["BaseFont"] as PdfName).Value;
        }
        num = float.Parse(strArray[1], (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
      }
    }
    if (PdfDocument.ConformanceLevel == PdfConformanceLevel.None)
    {
      switch (str1)
      {
        case "Helvetica":
          this.font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, num);
          break;
        case "Courier":
          this.font = (PdfFont) new PdfStandardFont(PdfFontFamily.Courier, num);
          break;
        case "Symbol":
          this.font = (PdfFont) new PdfStandardFont(PdfFontFamily.Symbol, num);
          break;
        case "TimesRoman":
          this.font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, num);
          break;
        case "ZapfDingbats":
          this.font = (PdfFont) new PdfStandardFont(PdfFontFamily.ZapfDingbats, num);
          break;
        case "MonotypeSungLight":
          this.font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.MonotypeSungLight, num);
          break;
        case "SinoTypeSongLight":
          this.font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.SinoTypeSongLight, num);
          break;
        case "MonotypeHeiMedium":
          this.font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.MonotypeHeiMedium, num);
          break;
        case "HanyangSystemsGothicMedium":
          this.font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HanyangSystemsGothicMedium, num);
          break;
        case "HanyangSystemsShinMyeongJoMedium":
          this.font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HanyangSystemsShinMyeongJoMedium, num);
          break;
        case "HeiseiKakuGothicW5":
          this.font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HeiseiKakuGothicW5, num);
          break;
        case "HeiseiMinchoW3":
          this.font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HeiseiMinchoW3, num);
          break;
        default:
          this.font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, num);
          break;
      }
    }
    else
    {
      PdfDocumentBase document = this.crossTable.Document;
      this.font = (PdfFont) new PdfTrueTypeFont(new System.Drawing.Font("Arial", num, FontStyle.Regular, GraphicsUnit.Point), true, true, true);
    }
  }
}
