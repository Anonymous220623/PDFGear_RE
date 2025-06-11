// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedFreeTextAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedFreeTextAnnotation : PdfLoadedStyledAnnotation
{
  private PdfLineEndingStyle m_lineEndingStyle;
  private PdfFont m_font;
  private PdfColor m_textMarkupColor;
  private PdfAnnotationIntent m_annotationIntent;
  private PointF[] m_calloutLines = new PointF[0];
  private PdfColor m_borderColor;
  private PdfCrossTable m_crossTable;
  private string m_markUpText;
  private bool m_istextMarkupcolor;
  private PointF[] m_calloutsClone = new PointF[0];
  private bool m_isfontAPStream;
  private bool m_markupTextFromStream;
  private MemoryStream freeTextStream;
  private PdfRecordCollection readTextCollection;
  private ContentParser parser;
  private bool m_complexScript;
  private PdfTextAlignment alignment;
  private PdfTextDirection m_textDirection;
  private float m_lineSpacing;
  private bool isAllRotation = true;

  public float LineSpacing
  {
    get => this.m_lineSpacing;
    set => this.m_lineSpacing = value;
  }

  public PdfLoadedPopupAnnotationCollection ReviewHistory
  {
    get
    {
      if (this.m_reviewHistory == null)
        this.m_reviewHistory = new PdfLoadedPopupAnnotationCollection(this.Page, this.Dictionary, true);
      return this.m_reviewHistory;
    }
  }

  public PdfLoadedPopupAnnotationCollection Comments
  {
    get
    {
      if (this.m_comments == null)
        this.m_comments = new PdfLoadedPopupAnnotationCollection(this.Page, this.Dictionary, false);
      return this.m_comments;
    }
  }

  public PdfLineEndingStyle LineEndingStyle
  {
    get => this.ObtainLineEndingStyle();
    set
    {
      this.m_lineEndingStyle = value;
      this.Dictionary.SetProperty("LE", (IPdfPrimitive) new PdfName(this.m_lineEndingStyle.ToString()));
    }
  }

  public string MarkUpText
  {
    get => string.IsNullOrEmpty(this.m_markUpText) ? this.ObtainText() : this.m_markUpText;
    set
    {
      if (value == null)
        throw new ArgumentNullException("Text");
      if (!(this.m_markUpText != value))
        return;
      this.m_markUpText = value;
      this.Text = value;
    }
  }

  internal bool MarkUpTextFromStream
  {
    get => this.m_markupTextFromStream;
    set => this.m_markupTextFromStream = value;
  }

  public PdfAnnotationIntent AnnotationIntent
  {
    get => this.m_annotationIntent;
    set
    {
      this.m_annotationIntent = value;
      this.Dictionary.SetProperty("IT", (IPdfPrimitive) new PdfName(this.m_annotationIntent.ToString()));
    }
  }

  public PdfFont Font
  {
    get => this.m_font == null ? this.ObtainFont() : this.m_font;
    set
    {
      this.m_font = value != null ? value : throw new ArgumentNullException(nameof (Font));
      this.UpdateTextMarkupColor();
    }
  }

  public PdfColor TextMarkupColor
  {
    get
    {
      this.m_textMarkupColor = this.GetTextMarkUpColor();
      this.UpdateTextMarkupColor();
      return this.m_textMarkupColor;
    }
    set
    {
      this.m_textMarkupColor = value;
      this.m_istextMarkupcolor = true;
      this.UpdateTextMarkupColor();
    }
  }

  public PointF[] CalloutLines
  {
    get => this.GetcalloutLinepoints();
    set
    {
      this.m_calloutLines = value;
      if (this.m_calloutLines.Length < 2)
        return;
      PdfArray primitive = new PdfArray();
      for (int index = 0; index < this.m_calloutLines.Length; ++index)
      {
        primitive.Add((IPdfPrimitive) new PdfNumber(this.m_calloutLines[index].X));
        primitive.Add((IPdfPrimitive) new PdfNumber(this.Page.Size.Height - this.m_calloutLines[index].Y));
      }
      this.Dictionary.SetProperty("CL", (IPdfPrimitive) primitive);
    }
  }

  public PdfColor BorderColor
  {
    get => this.ObtainColor();
    set
    {
      if (!(this.m_borderColor != value))
        return;
      this.m_borderColor = value;
      this.Dictionary.SetProperty("DA", (IPdfPrimitive) new PdfString($"{(ValueType) (float) ((double) this.m_borderColor.R / (double) byte.MaxValue)} {(ValueType) (float) ((double) this.m_borderColor.G / (double) byte.MaxValue)} {(ValueType) (float) ((double) this.m_borderColor.B / (double) byte.MaxValue)} rg "));
    }
  }

  public bool ComplexScript
  {
    get => this.m_complexScript;
    set => this.m_complexScript = value;
  }

  public PdfTextAlignment TextAlignment
  {
    get => this.ObtainTextAlignment();
    set
    {
      if (this.alignment == value)
        return;
      this.alignment = value;
      this.Dictionary.SetProperty("Q", (IPdfPrimitive) new PdfNumber((int) this.alignment));
    }
  }

  public PdfTextDirection TextDirection
  {
    get => this.m_textDirection;
    set => this.m_textDirection = value;
  }

  internal PdfLoadedFreeTextAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect,
    string text)
    : base(dictionary, crossTable)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    this.Dictionary = dictionary;
    this.m_crossTable = crossTable;
  }

  private PdfColor ObtainColor()
  {
    PdfColor color = new PdfColor();
    if (this.Dictionary.ContainsKey("DA"))
    {
      if (this.Dictionary["DA"] is PdfArray)
      {
        if (PdfCrossTable.Dereference(this.Dictionary["DA"]) is PdfArray pdfArray1 && pdfArray1.Count > 0 && pdfArray1[0] is PdfNumber && pdfArray1[1] is PdfNumber && pdfArray1[2] is PdfNumber)
          color = new PdfColor((pdfArray1[0] as PdfNumber).FloatValue, (pdfArray1[1] as PdfNumber).FloatValue, (pdfArray1[2] as PdfNumber).FloatValue);
      }
      else if (this.Dictionary["DA"] is PdfString)
      {
        string[] strArray = (this.Dictionary["DA"] as PdfString).Value.Trim().Split(' ');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (strArray[index] == "rg")
          {
            color = new PdfColor(float.Parse(strArray[index - 3].TrimStart('['), (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), float.Parse(strArray[index - 2], (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), float.Parse(strArray[index - 1].TrimEnd(']'), (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat));
            break;
          }
          if (strArray[index] == "g")
          {
            color = new PdfColor(float.Parse(strArray[index - 1], (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat));
            break;
          }
        }
      }
    }
    else if (this.Dictionary.ContainsKey("MK") && this.Dictionary["MK"] is PdfDictionary pdfDictionary && PdfCrossTable.Dereference(pdfDictionary["BC"]) is PdfArray pdfArray2 && pdfArray2.Elements.Count > 0)
      color = new PdfColor((pdfArray2.Elements[0] as PdfNumber).FloatValue, (pdfArray2.Elements[1] as PdfNumber).FloatValue, (pdfArray2.Elements[2] as PdfNumber).FloatValue);
    return color;
  }

  private PointF[] GetcalloutLinepoints()
  {
    if (this.Dictionary.ContainsKey("CL") && PdfCrossTable.Dereference(this.Dictionary["CL"]) is PdfArray pdfArray)
    {
      this.m_calloutLines = new PointF[pdfArray.Count / 2];
      int index1 = 0;
      int index2 = 0;
      while (index2 < pdfArray.Count)
      {
        float floatValue = (pdfArray[index2] as PdfNumber).FloatValue;
        float y = this.Page.Size.Height - (pdfArray[index2 + 1] as PdfNumber).FloatValue;
        this.m_calloutLines[index1] = new PointF(floatValue, y);
        index2 += 2;
        ++index1;
      }
    }
    return this.m_calloutLines;
  }

  private PdfColor GetTextMarkUpColor()
  {
    string htmlColor = "";
    if (this.Dictionary.ContainsKey("TextColor"))
    {
      PdfColor pdfColor = new PdfColor(System.Drawing.Color.Empty);
      if (PdfCrossTable.Dereference(this.Dictionary["TextColor"]) is PdfArray pdfArray && pdfArray.Elements.Count > 0 && pdfArray[0] is PdfNumber && pdfArray[1] is PdfNumber && pdfArray[2] is PdfNumber)
      {
        pdfColor = new PdfColor((byte) Math.Round((double) (pdfArray[0] as PdfNumber).FloatValue * (double) byte.MaxValue), (byte) Math.Round((double) (pdfArray[1] as PdfNumber).FloatValue * (double) byte.MaxValue), (byte) Math.Round((double) (pdfArray[2] as PdfNumber).FloatValue * (double) byte.MaxValue));
        this.m_textMarkupColor = pdfColor;
        return this.m_textMarkupColor;
      }
    }
    else if (this.Dictionary["AP"] != null && !this.m_istextMarkupcolor)
    {
      this.m_isfontAPStream = false;
      this.ObtainFromAPStream(this.m_isfontAPStream);
    }
    bool flag = false;
    if (this.Dictionary.ContainsKey("DS") && this.m_textMarkupColor.IsEmpty)
    {
      string[] strArray = (this.Dictionary["DS"] as PdfString).Value.ToString().Split(';');
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index].Contains("color"))
          htmlColor = strArray[index].Split(':')[1];
      }
      flag = true;
      this.m_textMarkupColor = (PdfColor) ColorTranslator.FromHtml(htmlColor);
    }
    if (!this.m_istextMarkupcolor && this.Dictionary.ContainsKey("RC"))
    {
      if (!this.m_textMarkupColor.IsEmpty)
      {
        if (!flag)
          goto label_25;
      }
      try
      {
        List<object> xmlData = this.ParseXMLData();
        if (xmlData != null)
        {
          if (xmlData.Count > 0)
          {
            for (int index = 0; index < xmlData.Count; ++index)
            {
              if (xmlData[index] is PdfSolidBrush)
                this.m_textMarkupColor = new PdfColor((xmlData[index] as PdfSolidBrush).Color);
            }
            if (this.m_textMarkupColor.IsEmpty)
              this.m_textMarkupColor = new PdfColor(System.Drawing.Color.Black);
          }
        }
      }
      catch
      {
      }
    }
label_25:
    return this.m_textMarkupColor;
  }

  private void UpdateTextMarkupColor()
  {
    this.Dictionary.SetProperty("DS", (IPdfPrimitive) new PdfString($"font:{this.Font.Name} {this.Font.Size}pt; color:{ColorTranslator.ToHtml(System.Drawing.Color.FromArgb((int) this.m_textMarkupColor.R, (int) this.m_textMarkupColor.G, (int) this.m_textMarkupColor.B))};style:{this.Font.Style}"));
  }

  private string ObtainText()
  {
    string text = (string) null;
    bool flag = this.Dictionary.ContainsKey("Contents");
    if (!this.MarkUpTextFromStream && flag || this.MarkUpTextFromStream && flag && !this.Dictionary.ContainsKey("AP"))
    {
      if (this.Dictionary["Contents"] is PdfString pdfString)
        text = pdfString.Value.ToString();
      if (!string.IsNullOrEmpty(text))
        this.m_markUpText = text;
      return text;
    }
    if (!this.MarkUpTextFromStream || this.Dictionary["AP"] == null)
      return string.Empty;
    this.m_isfontAPStream = true;
    string textFromApStream = this.ObtainTextFromAPStream(this.m_isfontAPStream);
    if (!string.IsNullOrEmpty(textFromApStream))
      this.m_markUpText = textFromApStream;
    return textFromApStream;
  }

  private PdfLineEndingStyle ObtainLineEndingStyle()
  {
    PdfLineEndingStyle lineEndingStyle = PdfLineEndingStyle.Square;
    if (this.Dictionary.ContainsKey("LE"))
    {
      switch ((this.Dictionary["LE"] as PdfName).Value.ToString())
      {
        case "Square":
          lineEndingStyle = PdfLineEndingStyle.Square;
          break;
        case "Butt":
          lineEndingStyle = PdfLineEndingStyle.Butt;
          break;
        case "Circle":
          lineEndingStyle = PdfLineEndingStyle.Circle;
          break;
        case "ClosedArrow":
          lineEndingStyle = PdfLineEndingStyle.ClosedArrow;
          break;
        case "Diamond":
          lineEndingStyle = PdfLineEndingStyle.Diamond;
          break;
        case "None":
          lineEndingStyle = PdfLineEndingStyle.None;
          break;
        case "OpenArrow":
          lineEndingStyle = PdfLineEndingStyle.OpenArrow;
          break;
        case "RClosedArrow":
          lineEndingStyle = PdfLineEndingStyle.RClosedArrow;
          break;
        case "ROpenArrow":
          lineEndingStyle = PdfLineEndingStyle.ROpenArrow;
          break;
        case "Slash":
          lineEndingStyle = PdfLineEndingStyle.Slash;
          break;
      }
    }
    return lineEndingStyle;
  }

  private PdfFont ObtainFont()
  {
    string str1 = "";
    float result = 1f;
    string str2 = "";
    if (this.Dictionary.ContainsKey("DS") || this.Dictionary.ContainsKey("DA"))
    {
      if (this.Dictionary.ContainsKey("DS"))
      {
        string[] strArray1 = (this.Dictionary["DS"] as PdfString).Value.ToString().Split(';');
        for (int index1 = 0; index1 < strArray1.Length; ++index1)
        {
          string[] strArray2 = strArray1[index1].Split(':');
          if (strArray1[index1].Contains("font-family"))
            str1 = strArray2[1];
          else if (strArray1[index1].Contains("font-size"))
          {
            if (strArray2[1].EndsWith("pt"))
            {
              string s = strArray2[1].Replace("pt", "");
              if (s.Contains(","))
                s = s.Replace(",", ".");
              result = float.Parse(s, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
            }
          }
          else if (strArray1[index1].Contains("font-style"))
            str2 = strArray1[1];
          else if (strArray1[index1].Contains("font"))
          {
            string str3 = strArray2[1];
            string str4 = string.Empty;
            string[] strArray3 = str3.Split(' ');
            for (int index2 = 0; index2 < strArray3.Length; ++index2)
            {
              if (strArray3[index2] != "" && !strArray3[index2].EndsWith("pt"))
                str4 = $"{str4}{strArray3[index2]} ";
              if (strArray3[index2].EndsWith("pt"))
              {
                string s = strArray3[index2].Replace("pt", "");
                if (s.Contains(","))
                  s = s.Replace(",", ".");
                result = float.Parse(s, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
              }
            }
            str1 = str4.TrimEnd(' ');
            if (str1.Contains(","))
              str1 = str1.Split(',')[0];
          }
          else if (strArray1[index1].Contains("style"))
            str2 = strArray2[1];
        }
      }
      else if (this.Dictionary.ContainsKey("DA"))
      {
        PdfString pdfString = this.Dictionary["DA"] as PdfString;
        string empty = string.Empty;
        if (pdfString != null)
          empty = pdfString.Value;
        if (empty != string.Empty && empty.Contains("Tf"))
        {
          string[] strArray = empty.ToString().Split(' ');
          for (int index = 0; index < strArray.Length; ++index)
          {
            string str5 = strArray[index];
            if (str5.Contains("Tf"))
            {
              str1 = strArray[index - 2].TrimStart('/');
              float.TryParse(strArray[index - 1], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result);
            }
            if (str5.Contains("rg"))
              this.m_textMarkupColor = new PdfColor(float.Parse(strArray[index - 3].TrimStart('['), (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), float.Parse(strArray[index - 2], (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), float.Parse(strArray[index - 1].TrimEnd(']'), (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat));
          }
        }
        else if (this.Dictionary["AP"] != null)
        {
          this.m_isfontAPStream = true;
          this.ObtainFromAPStream(this.m_isfontAPStream);
          return this.m_font;
        }
      }
      PdfFontStyle style = PdfFontStyle.Regular;
      string str6 = str2;
      char[] chArray = new char[1]{ ',' };
      foreach (string str7 in str6.Split(chArray))
      {
        switch (str7.Trim())
        {
          case "Bold":
            style |= PdfFontStyle.Bold;
            break;
          case "Italic":
            style |= PdfFontStyle.Italic;
            break;
          case "Regular":
            style = style;
            break;
          case "Strikeout":
            style |= PdfFontStyle.Strikeout;
            break;
          case "Underline":
            style |= PdfFontStyle.Underline;
            break;
        }
      }
      if ((double) result == 0.0 && this.Dictionary.ContainsKey("RC"))
      {
        List<object> xmlData = this.ParseXMLData();
        for (int index = 0; index < xmlData.Count; ++index)
        {
          if (xmlData[index] is PdfFont)
          {
            this.m_font = xmlData[index] as PdfFont;
            return this.m_font;
          }
        }
      }
      string familyName = str1.Trim();
      if (PdfDocument.ConformanceLevel == PdfConformanceLevel.None)
      {
        switch (familyName)
        {
          case "Helvetica":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, result, style);
            break;
          case "Courier":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Courier, result, style);
            break;
          case "Symbol":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Symbol, result, style);
            break;
          case "TimesRoman":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, result, style);
            break;
          case "ZapfDingbats":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.ZapfDingbats, result, style);
            break;
          case "MonotypeSungLight":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.MonotypeSungLight, result, style);
            break;
          case "SinoTypeSongLight":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.SinoTypeSongLight, result, style);
            break;
          case "MonotypeHeiMedium":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.MonotypeHeiMedium, result, style);
            break;
          case "HanyangSystemsGothicMedium":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HanyangSystemsGothicMedium, result, style);
            break;
          case "HanyangSystemsShinMyeongJoMedium":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HanyangSystemsShinMyeongJoMedium, result, style);
            break;
          case "HeiseiKakuGothicW5":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HeiseiKakuGothicW5, result, style);
            break;
          case "HeiseiMinchoW3":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HeiseiMinchoW3, result, style);
            break;
          default:
            this.m_font = (PdfFont) new PdfTrueTypeFont(new System.Drawing.Font(familyName, result), result);
            break;
        }
      }
      else
      {
        PdfDocumentBase document = this.m_crossTable.Document;
        this.m_font = (PdfFont) new PdfTrueTypeFont(new System.Drawing.Font("Arial", result, FontStyle.Regular, GraphicsUnit.Point), true, true, true);
      }
    }
    else if (this.Dictionary["AP"] != null)
    {
      this.m_isfontAPStream = true;
      this.ObtainFromAPStream(this.m_isfontAPStream);
    }
    return this.m_font;
  }

  private string ObtainTextFromAPStream(bool isfontStream)
  {
    string textFromApStream = "";
    IPdfPrimitive pdfPrimitive1 = this.Dictionary["AP"];
    if (pdfPrimitive1 != null && isfontStream && PdfCrossTable.Dereference(pdfPrimitive1) is PdfDictionary pdfDictionary)
    {
      IPdfPrimitive pdfPrimitive2 = pdfDictionary["N"];
      PdfDictionary pdfDictionary1 = (PdfDictionary) null;
      if (pdfPrimitive2 != null)
        pdfDictionary1 = PdfCrossTable.Dereference(pdfPrimitive2) as PdfDictionary;
      string key = (string) null;
      if (pdfDictionary1 != null && pdfDictionary1 is PdfStream pdfStream)
      {
        byte[] buffer = PdfString.StringToByte("\r\n");
        pdfStream.Decompress();
        if (this.freeTextStream == null)
          this.freeTextStream = new MemoryStream();
        pdfStream.InternalStream.WriteTo((Stream) this.freeTextStream);
        this.freeTextStream.Write(buffer, 0, buffer.Length);
        if (this.parser == null)
          this.parser = new ContentParser(this.freeTextStream.ToArray());
        this.readTextCollection = this.parser.ReadContent();
        if (this.readTextCollection != null)
        {
          for (int index = 0; index < this.readTextCollection.RecordCollection.Count; ++index)
          {
            string operatorName1 = this.readTextCollection.RecordCollection[index].OperatorName;
            if (operatorName1.Contains("Tf"))
            {
              PdfRecord record = this.readTextCollection.RecordCollection[index];
              string operatorName2 = record.OperatorName;
              key = record.Operands[0].Replace("/", "");
            }
            if (operatorName1.Contains("Tj") || operatorName1.Contains("TJ"))
            {
              string operand = this.readTextCollection.RecordCollection[index].Operands[0];
              if ((operand.Contains("(") || operand.Contains(")")) && pdfDictionary1.ContainsKey("Resources"))
              {
                if (!(pdfDictionary1["Resources"] is PdfDictionary pdfDictionary2))
                {
                  PdfReferenceHolder pdfReferenceHolder = pdfDictionary1["Resources"] as PdfReferenceHolder;
                  if (pdfReferenceHolder != (PdfReferenceHolder) null)
                    pdfDictionary2 = pdfReferenceHolder.Object as PdfDictionary;
                }
                if (pdfDictionary2 != null && pdfDictionary2["Font"] is PdfDictionary pdfDictionary3)
                {
                  if (!(pdfDictionary3[key] is PdfDictionary fontDictionary))
                  {
                    PdfReferenceHolder pdfReferenceHolder = pdfDictionary3[key] as PdfReferenceHolder;
                    if (pdfReferenceHolder != (PdfReferenceHolder) null)
                      fontDictionary = pdfReferenceHolder.Object as PdfDictionary;
                  }
                  if (fontDictionary != null && pdfDictionary3.ContainsKey(key))
                  {
                    FontStructure fontStructure = new FontStructure((IPdfPrimitive) fontDictionary);
                    if (fontStructure != null)
                    {
                      fontStructure.IsTextExtraction = true;
                      string str = fontStructure.DecodeTextExtraction(operand, true);
                      textFromApStream += str;
                    }
                  }
                }
              }
            }
            if (operatorName1.Contains("ET"))
              textFromApStream += "\r\n";
          }
        }
      }
    }
    if (textFromApStream.Contains("\r\n"))
      textFromApStream = textFromApStream.Substring(0, textFromApStream.Length - 2);
    return textFromApStream;
  }

  private void ObtainFromAPStream(bool isfontStream)
  {
    if (!(PdfCrossTable.Dereference(this.Dictionary["AP"]) is PdfDictionary pdfDictionary1) || !(PdfCrossTable.Dereference(pdfDictionary1["N"]) is PdfDictionary pdfDictionary2))
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
    if (this.freeTextStream == null)
      this.freeTextStream = new MemoryStream();
    pdfStream.InternalStream.WriteTo((Stream) this.freeTextStream);
    this.freeTextStream.Write(buffer, 0, buffer.Length);
    if (this.parser == null)
      this.parser = new ContentParser(this.freeTextStream.ToArray());
    this.readTextCollection = this.parser.ReadContent();
    if (this.readTextCollection == null)
      return;
    if (isfontStream)
    {
      string familyName = "";
      float num = 1f;
      foreach (PdfRecord readText in this.readTextCollection)
      {
        string empty = string.Empty;
        string[] operands = readText.Operands;
        if (readText.OperatorName == "Tf")
        {
          string[] strArray = operands;
          string str = strArray[0];
          PdfDictionary pdfDictionary4 = pdfDictionary3["Font"] as PdfDictionary;
          if (str.Contains("/"))
          {
            string key = str.Trim('/');
            familyName = ((PdfCrossTable.Dereference(pdfDictionary4[key]) as PdfDictionary)["BaseFont"] as PdfName).Value;
          }
          num = float.Parse(strArray[1], (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
        }
      }
      if (PdfDocument.ConformanceLevel == PdfConformanceLevel.None)
      {
        switch (familyName)
        {
          case "Helvetica":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, num);
            break;
          case "Courier":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Courier, num);
            break;
          case "Symbol":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Symbol, num);
            break;
          case "TimesRoman":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, num);
            break;
          case "Times-Roman":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, num);
            break;
          case "ZapfDingbats":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.ZapfDingbats, num);
            break;
          case "MonotypeSungLight":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.MonotypeSungLight, num);
            break;
          case "SinoTypeSongLight":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.SinoTypeSongLight, num);
            break;
          case "MonotypeHeiMedium":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.MonotypeHeiMedium, num);
            break;
          case "HanyangSystemsGothicMedium":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HanyangSystemsGothicMedium, num);
            break;
          case "HanyangSystemsShinMyeongJoMedium":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HanyangSystemsShinMyeongJoMedium, num);
            break;
          case "HeiseiKakuGothicW5":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HeiseiKakuGothicW5, num);
            break;
          case "HeiseiMinchoW3":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HeiseiMinchoW3, num);
            break;
          default:
            this.m_font = (PdfFont) new PdfTrueTypeFont(new System.Drawing.Font(familyName, num), num);
            break;
        }
      }
      else
        this.m_font = (PdfFont) new PdfTrueTypeFont(new System.Drawing.Font("Arial", num, FontStyle.Regular, GraphicsUnit.Point), true, true, true);
    }
    if (isfontStream)
      return;
    int textOperatorIndex = this.GetTextOperatorIndex(this.readTextCollection);
    if (textOperatorIndex <= -1)
      return;
    for (int index = textOperatorIndex; index > -1; --index)
    {
      PdfRecord record = this.readTextCollection.RecordCollection[index];
      string empty = string.Empty;
      switch (record.OperatorName)
      {
        case "rg":
          string[] operands1 = record.Operands;
          this.m_textMarkupColor = new PdfColor(float.Parse(operands1[0], (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), float.Parse(operands1[1], (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), float.Parse(operands1[2], (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat));
          return;
        case "k":
          string[] operands2 = record.Operands;
          this.m_textMarkupColor = new PdfColor(float.Parse(operands2[0], (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), float.Parse(operands2[1], (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), float.Parse(operands2[2], (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), float.Parse(operands2[3], (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat));
          return;
        case "g":
          this.m_textMarkupColor = new PdfColor(float.Parse(record.Operands[0], (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat));
          return;
        case "re":
          return;
        default:
          continue;
      }
    }
  }

  private int GetTextOperatorIndex(PdfRecordCollection records)
  {
    int textOperatorIndex = -1;
    int count = records.RecordCollection.Count;
    for (int index = 0; index < count; ++index)
    {
      PdfRecord record = records.RecordCollection[index];
      string empty = string.Empty;
      string operatorName = record.OperatorName;
      if (operatorName == "Tj" || operatorName == "'" || operatorName == "\"" || operatorName == "TJ")
      {
        textOperatorIndex = index;
        break;
      }
    }
    return textOperatorIndex;
  }

  protected override void Save()
  {
    if (this.Dictionary.ContainsKey("RC") && this.isContentUpdated)
      this.Dictionary.SetString("RC", $"<?xml version=\"1.0\"?><body xmlns=\"http://www.w3.org/1999/xhtml\"><p dir=\"ltr\">{this.MarkUpText}</p></body>");
    this.CheckFlatten();
    if (this.Flatten || this.Page.Annotations.Flatten || this.SetAppearanceDictionary)
    {
      PdfTemplate appearance = this.CreateAppearance();
      if (this.Flatten || this.Page.Annotations.Flatten)
        this.FlattenAnnotation((PdfPageBase) this.Page, appearance);
      else if (appearance != null)
      {
        this.Appearance.Normal = appearance;
        this.Dictionary.SetProperty("AP", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.Appearance));
      }
    }
    if (!this.FlattenPopUps)
      return;
    this.FlattenLoadedPopup();
  }

  private void FlattenAnnotation(PdfPageBase page, PdfTemplate appearance)
  {
    if (this.Dictionary.ContainsKey("AP") && appearance == null)
    {
      if (!(PdfCrossTable.Dereference(this.Dictionary["AP"]) is PdfDictionary pdfDictionary))
        return;
      if (PdfCrossTable.Dereference(pdfDictionary["N"]) is PdfDictionary dictionary && dictionary.ContainsKey("Subtype"))
      {
        if (!(dictionary is PdfStream template))
          return;
        appearance = new PdfTemplate(template);
        if (appearance == null)
          return;
        bool isNormalMatrix = this.ValidateTemplateMatrix(dictionary);
        if (isNormalMatrix && this.Page.Rotation != PdfPageRotateAngle.RotateAngle0)
        {
          this.FlattenAnnotationTemplate(appearance, isNormalMatrix);
        }
        else
        {
          if (!isNormalMatrix || !this.IsValidTemplateMatrix(dictionary, this.Bounds.Location, appearance))
            return;
          this.FlattenAnnotationTemplate(appearance, isNormalMatrix);
        }
      }
      else
      {
        this.SetAppearanceDictionary = true;
        appearance = this.CreateAppearance();
        if (appearance == null)
          return;
        bool isNormalMatrix = this.ValidateTemplateMatrix((PdfDictionary) appearance.m_content);
        this.FlattenAnnotationTemplate(appearance, isNormalMatrix);
      }
    }
    else if (!this.Dictionary.ContainsKey("AP") && appearance == null)
    {
      this.SetAppearanceDictionary = true;
      appearance = this.CreateAppearance();
      if (appearance == null)
        return;
      bool isNormalMatrix = this.ValidateTemplateMatrix((PdfDictionary) appearance.m_content);
      this.FlattenAnnotationTemplate(appearance, isNormalMatrix);
    }
    else if (!this.Dictionary.ContainsKey("AP") && appearance != null)
    {
      bool isNormalMatrix = this.ValidateTemplateMatrix((PdfDictionary) appearance.m_content);
      this.FlattenAnnotationTemplate(appearance, isNormalMatrix);
    }
    else
    {
      if (!this.Dictionary.ContainsKey("AP") || appearance == null)
        return;
      bool isNormalMatrix = this.ValidateTemplateMatrix((PdfDictionary) appearance.m_content);
      this.FlattenAnnotationTemplate(appearance, isNormalMatrix);
    }
  }

  private PdfTemplate CreateAppearance()
  {
    if (!this.SetAppearanceDictionary)
      return (PdfTemplate) null;
    float borderWidth = this.Border.Width / 2f;
    RectangleF appearanceBounds = this.ObtainAppearanceBounds();
    if ((double) this.RotateAngle == 90.0 || (double) this.RotateAngle == 180.0 || (double) this.RotateAngle == 270.0 || (double) this.RotateAngle == 0.0)
      this.isAllRotation = false;
    PdfTemplate appearance = (double) this.RotateAngle <= 0.0 || !this.isAllRotation ? new PdfTemplate(appearanceBounds) : new PdfTemplate(appearanceBounds.Size);
    this.SetMatrix((PdfDictionary) appearance.m_content);
    PaintParams paintParams = new PaintParams();
    string text = this.Text;
    appearance.m_writeTransformation = false;
    PdfGraphics graphics = appearance.Graphics;
    this.alignment = this.TextAlignment;
    PdfPen mBorderPen = new PdfPen(this.BorderColor, this.Border.Width);
    if ((double) this.Border.Width > 0.0)
      paintParams.BorderPen = mBorderPen;
    RectangleF rectangleF = this.ObtainStyle(mBorderPen, appearanceBounds, borderWidth, paintParams);
    if (this.Dictionary.ContainsKey("C"))
      paintParams.ForeBrush = (PdfBrush) new PdfSolidBrush(this.Color);
    paintParams.BackBrush = (PdfBrush) new PdfSolidBrush(this.TextMarkupColor);
    paintParams.BorderWidth = (float) (int) this.Border.Width;
    paintParams.m_complexScript = this.ComplexScript;
    paintParams.m_textDirection = this.TextDirection;
    paintParams.m_lineSpacing = this.LineSpacing;
    if (this.CalloutLines.Length >= 2)
    {
      this.DrawCallOuts(graphics, mBorderPen);
      if (this.LineEndingStyle == PdfLineEndingStyle.OpenArrow)
        this.DrawArrow(paintParams, graphics, mBorderPen);
      if (!this.Dictionary.ContainsKey("RD"))
      {
        rectangleF = new RectangleF(this.Bounds.X, (float) -((double) this.Page.Size.Height - ((double) this.Bounds.Y + (double) this.Bounds.Height)), this.Bounds.Width, -this.Bounds.Height);
        this.SetRectangleDifferance(rectangleF);
      }
      else
        rectangleF = new RectangleF(rectangleF.X, -rectangleF.Y, rectangleF.Width, -rectangleF.Height);
      paintParams.Bounds = rectangleF;
    }
    else
    {
      rectangleF = new RectangleF(rectangleF.X, -rectangleF.Y, rectangleF.Width, -rectangleF.Height);
      paintParams.Bounds = rectangleF;
      this.SetRectangleDifferance(rectangleF);
    }
    if ((double) this.Opacity < 1.0)
    {
      graphics.Save();
      graphics.SetTransparency(this.Opacity);
    }
    if (this.Rotate != PdfAnnotationRotateAngle.RotateAngle0)
      graphics.Save();
    this.DrawFreeTextRectangle(graphics, paintParams, rectangleF);
    this.DrawFreeMarkUpText(graphics, paintParams, rectangleF, text);
    if (this.Rotate != PdfAnnotationRotateAngle.RotateAngle0)
      graphics.Restore();
    if ((double) this.Opacity < 1.0)
      graphics.Restore();
    this.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(this.ObtainAppearanceBounds()));
    return appearance;
  }

  private void DrawArrow(PaintParams paintParams, PdfGraphics graphics, PdfPen mBorderPen)
  {
    if (paintParams.BorderPen != null)
      paintParams.BorderPen.LineJoin = PdfLineJoin.Miter;
    if (paintParams.BorderPen == null)
      return;
    PointF[] arrowPoints = this.CalculateArrowPoints(this.m_calloutsClone[1], this.m_calloutsClone[0]);
    PointF[] points = new PointF[3];
    byte[] pathTypes = new byte[3];
    points[0] = new PointF(arrowPoints[0].X, -arrowPoints[0].Y);
    points[1] = new PointF(this.m_calloutsClone[0].X, -this.m_calloutsClone[0].Y);
    points[2] = new PointF(arrowPoints[1].X, -arrowPoints[1].Y);
    pathTypes[0] = (byte) 0;
    pathTypes[1] = (byte) 1;
    pathTypes[2] = (byte) 1;
    PdfPath path = new PdfPath(points, pathTypes);
    graphics.DrawPath(mBorderPen, path);
  }

  private void DrawAppearance(RectangleF rectangle, PdfGraphics graphics, PaintParams paintParams)
  {
    GraphicsPath graphicsPath = new GraphicsPath();
    graphicsPath.AddRectangle(rectangle);
    if (!(this.Dictionary[new PdfName("BE")] is PdfDictionary pdfDictionary))
      return;
    float radius = (double) (pdfDictionary.Items[new PdfName("I")] as PdfNumber).FloatValue == 1.0 ? 4f : 9f;
    this.DrawCloudStyle(graphics, paintParams.ForeBrush, paintParams.BorderPen, radius, 0.833f, graphicsPath.PathPoints, true);
  }

  private RectangleF ObtainStyle(
    PdfPen mBorderPen,
    RectangleF rectangle,
    float borderWidth,
    PaintParams paintParams)
  {
    if (this.Dictionary.ContainsKey("BS"))
    {
      if (PdfCrossTable.Dereference(this.Dictionary["BS"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("D") && PdfCrossTable.Dereference(pdfDictionary.Items[new PdfName("D")]) is PdfArray pdfArray)
      {
        float[] numArray = new float[pdfArray.Count];
        for (int index = 0; index < pdfArray.Count; ++index)
          numArray[index] = (pdfArray.Elements[index] as PdfNumber).FloatValue;
        mBorderPen.DashStyle = PdfDashStyle.Dash;
        mBorderPen.DashPattern = numArray;
      }
      if ((double) this.Border.Width > 0.0)
        paintParams.BorderPen = mBorderPen;
    }
    if (!this.isBounds && this.Dictionary["RD"] != null)
    {
      if (PdfCrossTable.Dereference(this.Dictionary["RD"]) is PdfArray pdfArray1)
      {
        PdfNumber element1 = pdfArray1.Elements[0] as PdfNumber;
        PdfNumber element2 = pdfArray1.Elements[1] as PdfNumber;
        PdfNumber element3 = pdfArray1.Elements[2] as PdfNumber;
        PdfNumber element4 = pdfArray1.Elements[3] as PdfNumber;
        rectangle.X += element1.FloatValue;
        rectangle.Y = rectangle.Y + borderWidth + element2.FloatValue;
        rectangle.Width -= element1.FloatValue + element3.FloatValue;
        rectangle.Height -= element2.FloatValue + element4.FloatValue;
        paintParams.Bounds = new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
      }
    }
    else
    {
      rectangle.X += this.Border.Width / 2f;
      rectangle.Y += this.Border.Width / 2f;
      rectangle.Width -= this.Border.Width;
      rectangle.Height -= this.Border.Width;
      paintParams.Bounds = new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
    }
    return rectangle;
  }

  private PointF[] CalculateArrowPoints(PointF startingPoint, PointF endPoint)
  {
    float num1 = endPoint.X - startingPoint.X;
    float num2 = endPoint.Y - startingPoint.Y;
    float num3 = (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
    float num4 = num1 / num3;
    float num5 = num2 / num3;
    float num6 = (float) (4.0 * (-(double) num5 - (double) num4));
    float num7 = (float) (4.0 * ((double) num4 - (double) num5));
    return new PointF[2]
    {
      new PointF(endPoint.X + num6, endPoint.Y + num7),
      new PointF(endPoint.X - num7, endPoint.Y + num6)
    };
  }

  private RectangleF ObtainAppearanceBounds()
  {
    RectangleF empty = RectangleF.Empty;
    RectangleF bounds;
    if (this.CalloutLines != null && this.CalloutLines.Length > 0)
    {
      PdfPath pdfPath = new PdfPath();
      PointF[] linePoints = this.CalloutLines.Length != 2 ? new PointF[3] : new PointF[this.CalloutLines.Length];
      if (this.CalloutLines.Length >= 2)
      {
        this.ObtainCallOutsNative();
        for (int index = 0; index < this.CalloutLines.Length && index < 3; ++index)
          linePoints[index] = new PointF(this.m_calloutsClone[index].X, this.m_calloutsClone[index].Y);
      }
      if (linePoints.Length > 0)
        pdfPath.AddLines(linePoints);
      pdfPath.AddRectangle(new RectangleF(this.Bounds.X - 2f, (float) ((double) this.Page.Size.Height - ((double) this.Bounds.Y + (double) this.Bounds.Height) - 2.0), this.Bounds.Width + 4f, this.Bounds.Height + 4f));
      bounds = pdfPath.GetBounds();
    }
    else
      bounds = this.GetBounds(this.Dictionary, this.CrossTable);
    return bounds;
  }

  private void DrawFreeTextRectangle(
    PdfGraphics graphics,
    PaintParams paintParams,
    RectangleF rect)
  {
    bool isRotation = false;
    if (this.Dictionary.ContainsKey("BE"))
    {
      float[] numArray = new float[4]
      {
        rect.X,
        rect.Y,
        rect.Width,
        rect.Height
      };
      for (int index = 0; index < 4; ++index)
      {
        if ((double) numArray[index] < 0.0)
          numArray[index] = -numArray[index];
      }
      rect = new RectangleF(numArray[0], numArray[1], numArray[2], numArray[3]);
      this.DrawAppearance(rect, graphics, paintParams);
      if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle90 && !this.isAllRotation)
        graphics.RotateTransform(-90f);
      else if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle180 && !this.isAllRotation)
      {
        graphics.RotateTransform(-180f);
      }
      else
      {
        if (this.Rotate != PdfAnnotationRotateAngle.RotateAngle270 || this.isAllRotation)
          return;
        graphics.RotateTransform(-270f);
      }
    }
    else
    {
      if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle90 && !this.isAllRotation)
      {
        graphics.RotateTransform(-90f);
        paintParams.Bounds = new RectangleF(-rect.Y, rect.Width + rect.X, -rect.Height, -rect.Width);
      }
      else if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle180 && !this.isAllRotation)
      {
        graphics.RotateTransform(-180f);
        paintParams.Bounds = new RectangleF((float) -((double) rect.Width + (double) rect.X), (float) -((double) rect.Height + (double) rect.Y), rect.Width, rect.Height);
      }
      else if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle270 && !this.isAllRotation)
      {
        graphics.RotateTransform(-270f);
        paintParams.Bounds = new RectangleF(rect.Y + rect.Height, -rect.X, -rect.Height, -rect.Width);
      }
      FieldPainter.DrawFreeTextAnnotation(graphics, paintParams, "", this.Font, rect, false, this.alignment, isRotation);
    }
  }

  private void DrawFreeMarkUpText(
    PdfGraphics graphics,
    PaintParams paintParams,
    RectangleF rect,
    string text)
  {
    bool isRotation = false;
    if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle90 && !this.isAllRotation)
      rect = new RectangleF(-rect.Y, rect.X, -rect.Height, rect.Width);
    else if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle180 && !this.isAllRotation)
      rect = new RectangleF((float) -((double) rect.Width + (double) rect.X), -rect.Y, rect.Width, -rect.Height);
    else if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle270 && !this.isAllRotation)
      rect = new RectangleF(rect.Y + rect.Height, (float) -((double) rect.Width + (double) rect.X), -rect.Height, rect.Width);
    else if ((double) this.RotateAngle == 0.0 && !this.isAllRotation)
      rect = new RectangleF(rect.X, rect.Y + rect.Height, rect.Width, rect.Height);
    float rotateAngle = this.RotateAngle;
    if ((double) rotateAngle > 0.0 && this.isAllRotation)
    {
      isRotation = true;
      SizeF sizeF = this.Font.MeasureString(this.Text);
      if ((double) rotateAngle > 0.0 && (double) rotateAngle <= 91.0)
        graphics.TranslateTransform(sizeF.Height, -this.Bounds.Height);
      else if ((double) rotateAngle > 91.0 && (double) rotateAngle <= 181.0)
        graphics.TranslateTransform(this.Bounds.Width - sizeF.Height, (float) -((double) this.Bounds.Height - (double) sizeF.Height));
      else if ((double) rotateAngle > 181.0 && (double) rotateAngle <= 271.0)
        graphics.TranslateTransform(this.Bounds.Width - sizeF.Height, -sizeF.Height);
      else if ((double) rotateAngle > 271.0 && (double) rotateAngle < 360.0)
        graphics.TranslateTransform(sizeF.Height, -sizeF.Height);
      graphics.RotateTransform(this.RotateAngle);
      paintParams.Bounds = new RectangleF(0.0f, 0.0f, paintParams.Bounds.Width, paintParams.Bounds.Height);
    }
    FieldPainter.DrawFreeTextAnnotation(graphics, paintParams, text, this.Font, rect, true, this.alignment, isRotation);
  }

  private void SetRectangleDifferance(RectangleF innerRectangle)
  {
    RectangleF appearanceBounds = this.ObtainAppearanceBounds();
    float[] array = new float[4]
    {
      innerRectangle.X - appearanceBounds.X,
      -innerRectangle.Y - appearanceBounds.Y,
      innerRectangle.Width - appearanceBounds.Width,
      (float) (-(double) innerRectangle.Y - (double) appearanceBounds.Y + -(double) innerRectangle.Height) - appearanceBounds.Height
    };
    for (int index = 0; index < 4; ++index)
    {
      if ((double) array[index] < 0.0)
        array[index] = -array[index];
    }
    this.Dictionary["RD"] = (IPdfPrimitive) new PdfArray(array);
  }

  private void DrawCallOuts(PdfGraphics graphics, PdfPen mBorderPen)
  {
    PdfPath path = new PdfPath();
    PointF[] linePoints = this.CalloutLines.Length != 2 ? new PointF[3] : new PointF[this.CalloutLines.Length];
    for (int index = 0; index < this.CalloutLines.Length && index < 3; ++index)
      linePoints[index] = new PointF(this.m_calloutsClone[index].X, -this.m_calloutsClone[index].Y);
    if (linePoints.Length > 0)
      path.AddLines(linePoints);
    graphics.DrawPath(mBorderPen, path);
  }

  private void ObtainCallOutsNative()
  {
    if (this.CalloutLines.Length <= 0)
      return;
    this.m_calloutsClone = new PointF[this.CalloutLines.Length];
    for (int index = 0; index < this.CalloutLines.Length; ++index)
    {
      float x = this.CalloutLines[index].X;
      float y = this.Page.Size.Height - this.CalloutLines[index].Y;
      this.m_calloutsClone[index] = new PointF(x, y);
    }
  }

  private PdfTextAlignment ObtainTextAlignment()
  {
    bool flag = false;
    PdfTextAlignment textAlignment = PdfTextAlignment.Left;
    if (this.Dictionary.ContainsKey("Q"))
    {
      if (PdfCrossTable.Dereference(this.Dictionary["Q"]) is PdfNumber pdfNumber)
      {
        textAlignment = (PdfTextAlignment) Enum.ToObject(typeof (PdfTextAlignment), pdfNumber.IntValue);
        flag = true;
      }
    }
    else if (this.Dictionary.ContainsKey("RC"))
    {
      List<object> xmlData = this.ParseXMLData();
      if (xmlData != null)
      {
        textAlignment = (PdfTextAlignment) xmlData[1];
        flag = true;
      }
    }
    if (this.Dictionary.ContainsKey("DS") && !flag)
    {
      string[] strArray = (this.Dictionary["DS"] as PdfString).Value.ToString().Split(';');
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index].Contains("text-align"))
        {
          switch (strArray[index].Split(':')[1])
          {
            case "left":
              textAlignment = PdfTextAlignment.Left;
              continue;
            case "right":
              textAlignment = PdfTextAlignment.Right;
              continue;
            case "center":
              textAlignment = PdfTextAlignment.Center;
              continue;
            case "justify":
              textAlignment = PdfTextAlignment.Justify;
              continue;
            default:
              continue;
          }
        }
      }
    }
    return textAlignment;
  }
}
