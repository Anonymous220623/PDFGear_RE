// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedWidgetAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedWidgetAnnotation : PdfLoadedStyledAnnotation
{
  private PdfCrossTable m_crossTable;
  private PdfAnnotationFlags m_flags;
  private PdfExtendedAppearance m_extendedAppearance;
  private WidgetBorder m_border = new WidgetBorder();
  private WidgetAppearance m_widgetAppearance = new WidgetAppearance();
  private PdfHighlightMode m_highlightMode = PdfHighlightMode.Invert;
  private PdfDefaultAppearance m_defaultAppearance;
  private PdfAnnotationActions m_actions;
  private PdfAppearance m_appearance;
  private PdfTextAlignment m_alignment;
  private string m_appearanceState;
  internal PdfFont m_font;
  private PdfRecordCollection readTextCollection;
  private ContentParser parser;
  private bool m_isfontAPStream;
  private MemoryStream freeTextStream;

  public PdfExtendedAppearance ExtendedAppearance
  {
    get
    {
      if (this.m_extendedAppearance == null)
        this.m_extendedAppearance = new PdfExtendedAppearance();
      return this.m_extendedAppearance;
    }
    set
    {
      this.m_extendedAppearance = value;
      if (this.m_extendedAppearance != null)
      {
        this.Dictionary.SetProperty("AP", (IPdfWrapper) this.m_extendedAppearance);
        this.Dictionary.SetProperty("MK", (IPdfPrimitive) null);
      }
      else
      {
        if (this.m_appearance != null && this.m_appearance.GetNormalTemplate() != null)
          this.Dictionary.SetProperty("AP", (IPdfWrapper) this.m_appearance);
        else
          this.Dictionary.SetProperty("AP", (IPdfPrimitive) null);
        this.Dictionary.SetProperty("MK", (IPdfWrapper) this.m_widgetAppearance);
        this.Dictionary.SetProperty("AS", (IPdfPrimitive) null);
      }
    }
  }

  public PdfHighlightMode HighlightMode
  {
    get => this.m_highlightMode;
    set
    {
      this.Dictionary.SetName("H", this.HighlightModeToString(this.m_highlightMode));
      this.Dictionary.Modify();
    }
  }

  public PdfTextAlignment TextAlignment
  {
    get
    {
      if (this.Dictionary.ContainsKey("Q") && PdfCrossTable.Dereference(this.Dictionary["Q"]) is PdfNumber pdfNumber)
        this.m_alignment = (PdfTextAlignment) Enum.ToObject(typeof (PdfTextAlignment), pdfNumber.IntValue);
      return this.m_alignment;
    }
    set
    {
      if (this.m_alignment == value)
        return;
      this.m_alignment = value;
      this.Dictionary.SetProperty("Q", (IPdfPrimitive) new PdfNumber((int) this.m_alignment));
      this.Dictionary.Modify();
    }
  }

  public PdfAnnotationActions Actions
  {
    get
    {
      if (this.m_actions == null)
      {
        this.m_actions = new PdfAnnotationActions();
        this.Dictionary.Remove("AA");
        this.Dictionary.SetProperty("AA", (IPdfWrapper) this.m_actions);
        this.Dictionary.Modify();
      }
      return this.m_actions;
    }
  }

  public new PdfAppearance Appearance
  {
    get
    {
      if (this.m_appearance == null)
        this.m_appearance = new PdfAppearance((PdfAnnotation) this);
      return this.m_appearance;
    }
    set
    {
      if (this.m_appearance == value)
        return;
      this.m_appearance = value;
    }
  }

  internal PdfFont Font
  {
    get => this.m_font == null ? this.ObtainFont() : this.m_font;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Font));
      if (this.m_font == value)
        return;
      this.m_font = value;
      this.GetWidgetAnnotation(this.Dictionary, this.CrossTable)["DA"] = (IPdfPrimitive) new PdfString(new PdfDefaultAppearance()
      {
        FontName = this.m_font.Name.Replace(" ", string.Empty),
        FontSize = this.m_font.Size,
        ForeColor = this.ForeColor
      }.ToString());
    }
  }

  internal PdfColor ForeColor
  {
    get
    {
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      PdfColor foreColor = new PdfColor((byte) 0, (byte) 0, (byte) 0);
      if (widgetAnnotation != null && widgetAnnotation.ContainsKey("DA"))
        foreColor = this.GetForeColour((this.CrossTable.GetObject(widgetAnnotation["DA"]) as PdfString).Value);
      else if (widgetAnnotation != null && widgetAnnotation.GetValue(this.CrossTable, "DA", "Parent") is PdfString pdfString)
        foreColor = this.GetForeColour(pdfString.Value);
      return foreColor;
    }
    set
    {
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      float height = 0.0f;
      string str = (string) null;
      if (widgetAnnotation != null && widgetAnnotation.ContainsKey("DA"))
        str = this.FontName((widgetAnnotation["DA"] as PdfString).Value, out height);
      if (!string.IsNullOrEmpty(str))
        widgetAnnotation["DA"] = (IPdfPrimitive) new PdfString(new PdfDefaultAppearance()
        {
          FontName = str,
          FontSize = height,
          ForeColor = value
        }.ToString());
      else
        widgetAnnotation["DA"] = (IPdfPrimitive) new PdfString(new PdfDefaultAppearance()
        {
          FontName = this.Font.Name,
          FontSize = this.Font.Size,
          ForeColor = value
        }.ToString());
    }
  }

  internal string AppearanceState
  {
    get => this.m_appearanceState;
    set
    {
      if (!(this.m_appearanceState != value))
        return;
      this.m_appearanceState = value;
      this.Dictionary.SetName("AS", value);
    }
  }

  internal PdfLoadedWidgetAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rectangle)
    : base(dictionary, crossTable)
  {
    this.Dictionary = dictionary;
    this.m_crossTable = crossTable;
  }

  private string HighlightModeToString(PdfHighlightMode m_highlightingMode)
  {
    switch (m_highlightingMode)
    {
      case PdfHighlightMode.NoHighlighting:
        return "N";
      case PdfHighlightMode.Outline:
        return "O";
      case PdfHighlightMode.Push:
        return "P";
      default:
        return "I";
    }
  }

  internal PdfColor GetForeColour(string defaultAppearance)
  {
    PdfColor foreColour = new PdfColor((byte) 0, (byte) 0, (byte) 0);
    if (defaultAppearance == null || defaultAppearance == string.Empty)
    {
      foreColour = new PdfColor((byte) 0, (byte) 0, (byte) 0);
    }
    else
    {
      PdfReader pdfReader = new PdfReader((Stream) new MemoryStream(Encoding.UTF8.GetBytes(defaultAppearance)));
      pdfReader.Position = 0L;
      bool flag = false;
      Stack<string> stringStack = new Stack<string>();
      string nextToken = pdfReader.GetNextToken();
      if (nextToken == "/")
        flag = true;
      while (nextToken != null && nextToken != string.Empty)
      {
        if (flag)
          nextToken = pdfReader.GetNextToken();
        flag = true;
        switch (nextToken)
        {
          case "g":
            foreColour = new PdfColor(this.ParseFloatColour(stringStack.Pop()));
            continue;
          case "rg":
            byte blue = (byte) ((double) this.ParseFloatColour(stringStack.Pop()) * (double) byte.MaxValue);
            byte green = (byte) ((double) this.ParseFloatColour(stringStack.Pop()) * (double) byte.MaxValue);
            foreColour = new PdfColor((byte) ((double) this.ParseFloatColour(stringStack.Pop()) * (double) byte.MaxValue), green, blue);
            continue;
          case "k":
            float floatColour1 = this.ParseFloatColour(stringStack.Pop());
            float floatColour2 = this.ParseFloatColour(stringStack.Pop());
            float floatColour3 = this.ParseFloatColour(stringStack.Pop());
            foreColour = new PdfColor(this.ParseFloatColour(stringStack.Pop()), floatColour3, floatColour2, floatColour1);
            continue;
          default:
            stringStack.Push(nextToken);
            continue;
        }
      }
    }
    return foreColour;
  }

  private float ParseFloatColour(string text)
  {
    return (float) double.Parse(text, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal string FontName(string fontString, out float height)
  {
    if (fontString.Contains("#2C"))
    {
      StringBuilder stringBuilder = new StringBuilder(fontString);
      stringBuilder.Replace("#2C", ",");
      fontString = stringBuilder.ToString();
    }
    PdfReader pdfReader = new PdfReader((Stream) new MemoryStream(Encoding.UTF8.GetBytes(fontString)));
    pdfReader.Position = 0L;
    string s = pdfReader.GetNextToken();
    string nextToken = pdfReader.GetNextToken();
    string str = (string) null;
    height = 0.0f;
    while (nextToken != null && nextToken != string.Empty)
    {
      str = s;
      s = nextToken;
      nextToken = pdfReader.GetNextToken();
      if (nextToken == "Tf")
      {
        height = (float) double.Parse(s, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture);
        break;
      }
    }
    return str;
  }

  private PdfFont ObtainFont()
  {
    string str1 = "";
    float result = 1f;
    if (this.Dictionary.ContainsKey("DS") || this.Dictionary.ContainsKey("DA"))
    {
      if (this.Dictionary.ContainsKey("DS"))
      {
        string[] strArray1 = (PdfCrossTable.Dereference(this.Dictionary["DS"]) as PdfString).Value.ToString().Split(';');
        for (int index1 = 0; index1 < strArray1.Length; ++index1)
        {
          string[] strArray2 = strArray1[index1].Split(':');
          if (strArray1[index1].Contains("font-family"))
            str1 = strArray2[1];
          else if (strArray1[index1].Contains("font-size"))
          {
            if (strArray2[1].EndsWith("pt"))
              result = float.Parse(strArray2[1].Replace("pt", ""), (IFormatProvider) CultureInfo.InvariantCulture);
          }
          else if (strArray1[index1].Contains("font-style"))
          {
            string str2 = strArray1[1];
          }
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
                result = float.Parse(strArray3[index2].Replace("pt", ""), (IFormatProvider) CultureInfo.InvariantCulture);
            }
            str1 = str4.TrimEnd(' ');
            if (str1.Contains(","))
              str1 = str1.Split(',')[0];
          }
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
            if (strArray[index].Contains("Tf"))
            {
              str1 = strArray[index - 2].TrimStart('/');
              float.TryParse(strArray[index - 1], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result);
            }
          }
        }
        else if (this.Dictionary["AP"] != null)
        {
          this.m_isfontAPStream = true;
          this.ObtainFromAPStream(this.m_isfontAPStream);
          return this.m_font;
        }
      }
      string familyName = str1.Trim();
      if (PdfDocument.ConformanceLevel == PdfConformanceLevel.None)
      {
        switch (familyName)
        {
          case "Helvetica":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, result);
            break;
          case "Courier":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Courier, result);
            break;
          case "Symbol":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Symbol, result);
            break;
          case "TimesRoman":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, result);
            break;
          case "ZapfDingbats":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.ZapfDingbats, result);
            break;
          case "MonotypeSungLight":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.MonotypeSungLight, result);
            break;
          case "SinoTypeSongLight":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.SinoTypeSongLight, result);
            break;
          case "MonotypeHeiMedium":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.MonotypeHeiMedium, result);
            break;
          case "HanyangSystemsGothicMedium":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HanyangSystemsGothicMedium, result);
            break;
          case "HanyangSystemsShinMyeongJoMedium":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HanyangSystemsShinMyeongJoMedium, result);
            break;
          case "HeiseiKakuGothicW5":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HeiseiKakuGothicW5, result);
            break;
          case "HeiseiMinchoW3":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HeiseiMinchoW3, result);
            break;
          default:
            this.m_font = (PdfFont) new PdfTrueTypeFont(new System.Drawing.Font(familyName, result), result);
            break;
        }
      }
      else
        this.m_font = (PdfFont) new PdfTrueTypeFont(new System.Drawing.Font("Arial", result, FontStyle.Regular, GraphicsUnit.Point), true, true, true);
    }
    else if (this.Dictionary["AP"] != null)
    {
      this.m_isfontAPStream = true;
      this.ObtainFromAPStream(this.m_isfontAPStream);
    }
    return this.m_font;
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
    if (this.readTextCollection == null || !isfontStream)
      return;
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
}
