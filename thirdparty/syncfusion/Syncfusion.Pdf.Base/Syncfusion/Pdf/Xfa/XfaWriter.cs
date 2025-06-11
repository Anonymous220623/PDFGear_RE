// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.XfaWriter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

internal class XfaWriter
{
  private PdfXfaForm m_xfaForm;
  private XmlWriter m_writer;
  internal int m_fieldCount = 1;
  internal int m_subFormFieldCount = 1;
  private PdfXfaDocument xfaDocument;

  internal XmlWriter Write
  {
    get => this.m_writer;
    set => this.m_writer = value;
  }

  internal PdfStream WriteDocumentTemplate(PdfXfaForm XfaForm)
  {
    this.m_xfaForm = XfaForm;
    XmlWriterSettings settings = new XmlWriterSettings();
    PdfStream pdfStream = new PdfStream();
    settings.OmitXmlDeclaration = true;
    settings.Encoding = (Encoding) new UTF8Encoding(false);
    this.Write = XmlWriter.Create((Stream) pdfStream.InternalStream, settings);
    this.Write.WriteStartElement("template", "http://www.xfa.org/schema/xfa-template/3.3/");
    this.m_xfaForm.SaveMainForm(this);
    this.Write.WriteEndElement();
    this.Write.Close();
    return pdfStream;
  }

  internal PdfStream WritePreamble()
  {
    PdfStream pdfStream = new PdfStream();
    pdfStream.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?><xdp:xdp xmlns:xdp=\"http://ns.adobe.com/xdp/\">");
    return pdfStream;
  }

  internal void StartDataSets(XmlWriter dataWriter)
  {
    dataWriter.WriteStartElement("xfa", "datasets", "http://www.xfa.org/schema/xfa-data/1.0/");
    dataWriter.WriteStartElement("xfa", "data", (string) null);
  }

  internal void EndDataSets(XmlWriter dataWriter)
  {
    dataWriter.WriteEndElement();
    dataWriter.WriteEndElement();
  }

  internal PdfStream WritePostable()
  {
    PdfStream pdfStream = new PdfStream();
    pdfStream.Write("</xdp:xdp>");
    return pdfStream;
  }

  internal PdfStream WriteConfig()
  {
    PdfStream pdfStream = new PdfStream();
    MemoryStream output = new MemoryStream();
    this.Write = XmlWriter.Create((Stream) output, new XmlWriterSettings()
    {
      Encoding = (Encoding) new UTF8Encoding(false),
      OmitXmlDeclaration = true
    });
    this.Write.WriteStartElement("config", "http://www.xfa.org/schema/xci/1.0/");
    this.Write.WriteStartElement("present");
    this.Write.WriteStartElement("pdf");
    this.Write.WriteStartElement("fontInfo");
    this.Write.WriteStartElement("embed");
    this.Write.WriteString("1");
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("version");
    this.Write.WriteString("1.65");
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("creator");
    this.Write.WriteString("Syncfusion");
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("producer");
    this.Write.WriteString("Syncfusion");
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("scriptModel");
    this.Write.WriteString("XFA");
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("interactive");
    this.Write.WriteString("1");
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("tagged");
    this.Write.WriteString("1");
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("encryption");
    this.Write.WriteStartElement("permissions");
    this.Write.WriteStartElement("accessibleContent");
    this.Write.WriteString("1");
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("contentCopy");
    this.Write.WriteString("1");
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("documentAssembly");
    this.Write.WriteString("1");
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("formFieldFilling");
    this.Write.WriteString("1");
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("modifyAnnots");
    this.Write.WriteString("1");
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("print");
    this.Write.WriteString("1");
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("printHighQuality");
    this.Write.WriteString("1");
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("change");
    this.Write.WriteString("1");
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("plaintextMetadata");
    this.Write.WriteString("1");
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("compression");
    this.Write.WriteStartElement("level");
    this.Write.WriteString("6");
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("compressLogicalStructure");
    this.Write.WriteString("1");
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("linearized");
    this.Write.WriteString("1");
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
    this.Write.WriteStartElement("acrobat");
    this.Write.WriteStartElement("acrobat7");
    this.Write.WriteStartElement("dynamicRender");
    this.Write.WriteString("required");
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
    this.Write.Close();
    pdfStream.Write(output.GetBuffer());
    output.Dispose();
    return pdfStream;
  }

  internal void WriteUI(string name, Dictionary<string, string> values, PdfXfaBorder border)
  {
    this.WriteUI(name, values, border, 0);
  }

  internal void WriteUI(
    string name,
    Dictionary<string, string> values,
    PdfXfaBorder border,
    PdfPaddings padding)
  {
    this.WriteUI(name, values, border, 0, padding);
  }

  internal void WriteUI(
    string name,
    Dictionary<string, string> values,
    PdfXfaBorder border,
    int comb)
  {
    this.WriteUI(name, values, border, comb, (PdfPaddings) null);
  }

  internal void WriteUI(
    string name,
    Dictionary<string, string> values,
    PdfXfaBorder border,
    int comb,
    PdfPaddings padding)
  {
    this.Write.WriteStartElement("ui");
    this.Write.WriteStartElement(name);
    if (values != null)
    {
      foreach (KeyValuePair<string, string> keyValuePair in values)
      {
        if (keyValuePair.Value != null && keyValuePair.Value != "\0")
          this.Write.WriteAttributeString(keyValuePair.Key, keyValuePair.Value);
      }
    }
    if (comb > 0)
    {
      this.Write.WriteAttributeString("hScrollPolicy", "off");
      this.Write.WriteStartElement(nameof (comb));
      this.Write.WriteAttributeString("numberOfCells", comb.ToString());
      this.Write.WriteEndElement();
    }
    this.DrawBorder(border);
    if (padding != null)
      this.WriteMargins(padding.Left, padding.Right, padding.Bottom, padding.Top);
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
  }

  internal void WriteValue(string text, int maxChar)
  {
    this.Write.WriteStartElement("value");
    this.Write.WriteStartElement(nameof (text));
    if (maxChar > 0)
      this.Write.WriteAttributeString("maxChars", maxChar.ToString());
    this.Write.WriteString(text);
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
  }

  internal void WriteValue(string text, string value, int maxChar)
  {
    this.Write.WriteStartElement(nameof (value));
    this.Write.WriteStartElement(value);
    if (maxChar > 0)
      this.Write.WriteAttributeString("maxChars", maxChar.ToString());
    this.Write.WriteString(text);
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
  }

  internal void WriteMargins(float l, float r, float b, float t)
  {
    this.Write.WriteStartElement("margin");
    if ((double) b != 0.0)
      this.Write.WriteAttributeString("bottomInset", b.ToString() + "pt");
    if ((double) t != 0.0)
      this.Write.WriteAttributeString("topInset", t.ToString() + "pt");
    if ((double) l != 0.0)
      this.Write.WriteAttributeString("leftInset", l.ToString() + "pt");
    if ((double) r != 0.0)
      this.Write.WriteAttributeString("rightInset", r.ToString() + "pt");
    this.Write.WriteEndElement();
  }

  internal void WriteMargins(PdfMargins margins)
  {
    this.WriteMargins(margins.Left, margins.Right, margins.Bottom, margins.Top);
  }

  internal void WriteFontInfo(PdfFont font, PdfColor foreColor)
  {
    if (font != null)
      this.WriteFontInfo(font.Name.ToString(), font.Size, font.Style, foreColor);
    else
      this.WriteFontInfo("Times New Roman", 0.0f, PdfFontStyle.Regular, foreColor);
  }

  private void WriteFontInfo(string name, float size, PdfFontStyle style, PdfColor fillColor)
  {
    this.Write.WriteStartElement("font");
    this.Write.WriteAttributeString("typeface", name);
    if ((double) size > 0.0)
      this.Write.WriteAttributeString(nameof (size), size.ToString() + "pt");
    switch (style)
    {
      case PdfFontStyle.Bold:
        this.Write.WriteAttributeString("weight", style.ToString().ToLower());
        break;
      case PdfFontStyle.Italic:
        this.Write.WriteAttributeString("posture", style.ToString().ToLower());
        break;
      case PdfFontStyle.Underline:
        this.Write.WriteAttributeString("underline", "1");
        break;
      case PdfFontStyle.Strikeout:
        this.Write.WriteAttributeString("lineThrough", "1");
        break;
    }
    this.DrawFillColor(fillColor);
    this.Write.WriteEndElement();
  }

  internal void SetSize(float fixedHeight, float fixedWidth, float minHeight, float minWidth)
  {
    this.SetSize(fixedHeight, fixedWidth, minHeight, minWidth, 0.0f, 0.0f);
  }

  internal void SetSize(
    float fixedHeight,
    float fixedWidth,
    float minHeight,
    float minWidth,
    float maxHeight,
    float maxWidth)
  {
    if ((double) fixedHeight != 0.0)
      this.Write.WriteAttributeString("h", fixedHeight.ToString() + "pt");
    if ((double) fixedWidth != 0.0)
      this.Write.WriteAttributeString("w", fixedWidth.ToString() + "pt");
    if ((double) minHeight != 0.0)
      this.Write.WriteAttributeString("minH", minHeight.ToString() + "pt");
    if ((double) minWidth != 0.0)
      this.Write.WriteAttributeString("minW", minWidth.ToString() + "pt");
    if ((double) maxHeight != 0.0)
      this.Write.WriteAttributeString("maxH", maxHeight.ToString() + "pt");
    if ((double) maxWidth == 0.0)
      return;
    this.Write.WriteAttributeString("maxW", maxWidth.ToString() + "pt");
  }

  internal void DrawLine(float thickness, string slope, string color)
  {
    this.Write.WriteStartElement("value");
    this.Write.WriteStartElement("line");
    if (slope != "")
      this.Write.WriteAttributeString(nameof (slope), slope);
    this.Write.WriteStartElement("edge");
    this.Write.WriteAttributeString(nameof (thickness), thickness.ToString() + "pt");
    this.Write.WriteStartElement(nameof (color));
    this.Write.WriteAttributeString("value", color);
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
  }

  internal void WriteCaption(
    string text,
    float reserve,
    PdfXfaHorizontalAlignment hAlign,
    PdfXfaVerticalAlignment vAligh)
  {
    this.Write.WriteStartElement("caption");
    if ((double) reserve > 0.0)
      this.Write.WriteAttributeString(nameof (reserve), reserve.ToString() + "pt");
    this.WriteValue(text, 0);
    this.WritePragraph(vAligh, hAlign);
    this.Write.WriteEndElement();
  }

  internal void WriteItems(string text)
  {
    this.Write.WriteStartElement("items");
    this.Write.WriteStartElement("integer");
    this.Write.WriteString(text);
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
  }

  internal void WriteItems(string rollOver, string down)
  {
    this.Write.WriteStartElement("items");
    if (rollOver != null && rollOver != "")
    {
      this.Write.WriteStartElement("text");
      this.Write.WriteAttributeString("name", "rollover");
      this.Write.WriteString(rollOver);
      this.Write.WriteEndElement();
    }
    if (down != null && down != "")
    {
      this.Write.WriteStartElement("text");
      this.Write.WriteAttributeString("name", nameof (down));
      this.Write.WriteString(down);
      this.Write.WriteEndElement();
    }
    this.Write.WriteEndElement();
  }

  internal void WriteListItems(List<string> list, string saveString)
  {
    this.Write.WriteStartElement("items");
    if (saveString != null)
      this.Write.WriteAttributeString("save", saveString);
    if (list.Count > 0)
    {
      foreach (string text in list)
      {
        this.Write.WriteStartElement("text");
        this.Write.WriteString(text);
        this.Write.WriteEndElement();
      }
    }
    this.Write.WriteEndElement();
  }

  internal void WriteToolTip(string text)
  {
    this.Write.WriteStartElement("assist");
    this.Write.WriteStartElement("toolTip");
    this.Write.WriteString(text);
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
  }

  internal void DrawFillColor(PdfColor fillColor)
  {
    if (fillColor.R == (byte) 0 && fillColor.G == (byte) 0 && fillColor.B == (byte) 0)
      return;
    this.Write.WriteStartElement("fill");
    this.DrawColor(fillColor);
    this.Write.WriteEndElement();
  }

  internal void WriteLocation(PointF location)
  {
    this.Write.WriteAttributeString("x", location.X.ToString() + "pt");
    this.Write.WriteAttributeString("y", location.Y.ToString() + "pt");
  }

  internal void WritePragraph(PdfXfaVerticalAlignment vAlign, PdfXfaHorizontalAlignment hAlign)
  {
    this.Write.WriteStartElement("para");
    string str = hAlign.ToString().ToLower();
    if (hAlign == PdfXfaHorizontalAlignment.JustifyAll)
      str = "justifyAll";
    this.Write.WriteAttributeString(nameof (hAlign), str);
    this.Write.WriteAttributeString(nameof (vAlign), vAlign.ToString().ToLower());
    this.Write.WriteEndElement();
  }

  internal void WritePattern(string value, bool isvalidate)
  {
    if (value == null)
      return;
    if (isvalidate)
    {
      this.Write.WriteStartElement("validate");
      this.Write.WriteStartElement("picture");
      this.Write.WriteString(value);
      this.Write.WriteEndElement();
      this.Write.WriteEndElement();
    }
    this.Write.WriteStartElement("format");
    this.Write.WriteStartElement("picture");
    this.Write.WriteString(value);
    this.Write.WriteEndElement();
    this.Write.WriteEndElement();
  }

  internal void DrawBorder(PdfXfaBorder border, bool isSkip)
  {
    if (border == null)
      return;
    if (border.LeftEdge != null || border.RightEdge != null || border.TopEdge != null || border.BottomEdge != null)
    {
      this.DrawEdge(border.TopEdge);
      this.DrawEdge(border.RightEdge);
      this.DrawEdge(border.BottomEdge);
      this.DrawEdge(border.LeftEdge);
    }
    else
      this.DrawEdge(new PdfXfaEdge()
      {
        BorderStyle = border.Style,
        Color = border.Color,
        Thickness = border.Width,
        Visibility = border.Visibility
      });
  }

  internal void DrawBorder(PdfXfaBorder border)
  {
    if (border == null)
      return;
    this.Write.WriteStartElement(nameof (border));
    int handedness = (int) border.Handedness;
    this.Write.WriteAttributeString("hand", border.Handedness.ToString().ToLower());
    if (border.Visibility != PdfXfaVisibility.Visible)
      this.Write.WriteAttributeString("presence", border.Visibility.ToString().ToLower());
    this.DrawBorder(border, true);
    if (border.FillColor != null)
      this.DrawFillColor(border.FillColor);
    this.Write.WriteEndElement();
  }

  internal void DrawBorder(PdfXfaBorder border, PdfXfaBrush fillColor)
  {
    if (border == null)
      return;
    this.Write.WriteStartElement(nameof (border));
    int handedness = (int) border.Handedness;
    this.Write.WriteAttributeString("hand", border.Handedness.ToString().ToLower());
    if (border.Visibility != PdfXfaVisibility.Visible)
      this.Write.WriteAttributeString("presence", border.Visibility.ToString().ToLower());
    this.DrawBorder(border, true);
    this.DrawFillColor(fillColor);
    this.Write.WriteEndElement();
  }

  internal void DrawEdge(PdfXfaEdge edge)
  {
    if (edge != null)
    {
      this.Write.WriteStartElement(nameof (edge));
      this.DrawStroke(edge.BorderStyle);
      if ((double) edge.Thickness >= 0.0)
        this.Write.WriteAttributeString("thickness", edge.Thickness.ToString() + "pt");
      if (edge.Visibility != PdfXfaVisibility.Visible)
        this.Write.WriteAttributeString("presence", edge.Visibility.ToString().ToLower());
      if (edge.Color.R != (byte) 0 || edge.Color.G != (byte) 0 || edge.Color.B != (byte) 0)
        this.DrawColor(edge.Color);
      this.Write.WriteEndElement();
    }
    else
    {
      this.Write.WriteStartElement(nameof (edge));
      this.Write.WriteEndElement();
    }
  }

  internal void DrawCorner(PdfXfaCorner corner)
  {
    if (corner == null)
      return;
    this.Write.WriteStartElement(nameof (corner));
    this.DrawStroke(corner.BorderStyle);
    if ((double) corner.Thickness > 0.0)
      this.Write.WriteAttributeString("thickness", corner.Thickness.ToString() + "pt");
    if ((double) corner.Radius > 0.0)
    {
      this.Write.WriteAttributeString("radius", corner.Radius.ToString() + "pt");
      this.Write.WriteAttributeString("join", corner.Shape.ToString().ToLower());
    }
    if (corner.Visibility != PdfXfaVisibility.Visible)
      this.Write.WriteAttributeString("presence", corner.Visibility.ToString().ToLower());
    if (corner.BorderColor.R != (byte) 0 || corner.BorderColor.G != (byte) 0 || corner.BorderColor.B != (byte) 0)
      this.DrawColor(corner.BorderColor);
    this.Write.WriteEndElement();
  }

  internal void DrawColor(PdfColor color)
  {
    this.Write.WriteStartElement(nameof (color));
    this.Write.WriteAttributeString("value", $"{color.R.ToString()},{color.G.ToString()},{color.B.ToString()}");
    this.Write.WriteEndElement();
  }

  private void DrawStroke(PdfXfaBorderStyle style)
  {
    if (!(style.ToString().ToLower() != "none"))
      return;
    string str = style.ToString();
    this.Write.WriteAttributeString("stroke", char.ToLower(str[0]).ToString() + str.Substring(1));
  }

  internal void DrawFillColor(PdfXfaBrush fill)
  {
    if (fill == null)
      return;
    this.Write.WriteStartElement(nameof (fill));
    if (fill != null)
    {
      if (fill is PdfXfaSolidBrush)
      {
        PdfXfaSolidBrush pdfXfaSolidBrush = fill as PdfXfaSolidBrush;
        if (pdfXfaSolidBrush.Color.R != (byte) 0 || pdfXfaSolidBrush.Color.G != (byte) 0 || pdfXfaSolidBrush.Color.B != (byte) 0)
          this.DrawColor(pdfXfaSolidBrush.Color);
      }
      else if (fill is PdfXfaLinearBrush)
        this.DrawLinearBrush(fill as PdfXfaLinearBrush);
      else if (fill is PdfXfaRadialBrush)
        this.DrawRadialBrush(fill as PdfXfaRadialBrush);
    }
    this.Write.WriteEndElement();
  }

  internal void DrawRadialBrush(PdfXfaRadialBrush rBrush)
  {
    if (rBrush == null)
      return;
    PdfColor startColor = rBrush.StartColor;
    this.DrawColor(rBrush.StartColor);
    this.Write.WriteStartElement("radial");
    string str = (string) null;
    switch (rBrush.Type)
    {
      case PdfXfaRadialType.CenterToEdge:
        str = "toEdge";
        break;
      case PdfXfaRadialType.EdgeToCenter:
        str = "toCenter";
        break;
    }
    this.Write.WriteAttributeString("type", str);
    this.DrawColor(rBrush.EndColor);
    this.Write.WriteEndElement();
  }

  internal void DrawLinearBrush(PdfXfaLinearBrush lBrush)
  {
    if (lBrush == null)
      return;
    PdfColor startColor = lBrush.StartColor;
    this.DrawColor(lBrush.StartColor);
    this.Write.WriteStartElement("linear");
    string str = (string) null;
    switch (lBrush.Type)
    {
      case PdfXfaLinearType.LeftToRight:
        str = "toRight";
        break;
      case PdfXfaLinearType.RightToLeft:
        str = "toLeft";
        break;
      case PdfXfaLinearType.BottomToTop:
        str = "toTop";
        break;
      case PdfXfaLinearType.TopToBottom:
        str = "toBottom";
        break;
    }
    this.Write.WriteAttributeString("type", str);
    this.DrawColor(lBrush.EndColor);
    this.Write.WriteEndElement();
  }

  internal void SetRPR(PdfXfaRotateAngle rotation, PdfXfaVisibility presence, bool isReadOnly)
  {
    if (rotation != PdfXfaRotateAngle.RotateAngle0)
    {
      string str = "0";
      switch (rotation - 1)
      {
        case PdfXfaRotateAngle.RotateAngle0:
          str = "90";
          break;
        case PdfXfaRotateAngle.RotateAngle90:
          str = "180";
          break;
        case PdfXfaRotateAngle.RotateAngle180:
          str = "270";
          break;
      }
      this.Write.WriteAttributeString("rotate", str);
    }
    if (isReadOnly)
      this.Write.WriteAttributeString("access", "readOnly");
    if (presence == PdfXfaVisibility.Visible)
      return;
    this.Write.WriteAttributeString(nameof (presence), presence.ToString().ToLower());
  }

  internal string GetDatePattern(PdfXfaDatePattern pattern)
  {
    string datePattern = string.Empty;
    switch (pattern)
    {
      case PdfXfaDatePattern.Default:
        datePattern = "MMM d, yyyy";
        break;
      case PdfXfaDatePattern.Short:
        datePattern = "M/d/yyyy";
        break;
      case PdfXfaDatePattern.Medium:
        datePattern = "MMM d, yyyy";
        break;
      case PdfXfaDatePattern.Long:
        datePattern = "MMMM d, yyyy";
        break;
      case PdfXfaDatePattern.Full:
        datePattern = "dddd, MMMM dd, yyyy";
        break;
      case PdfXfaDatePattern.MDYY:
        datePattern = "M/d/yy";
        break;
      case PdfXfaDatePattern.MMMD_YYYY:
        datePattern = "MMM d, yyyy";
        break;
      case PdfXfaDatePattern.MMMMD_YYYY:
        datePattern = "MMMM d, yyyy";
        break;
      case PdfXfaDatePattern.EEEE_MMMMD_YYYY:
        datePattern = "dddd, MMMM d, yyyy";
        break;
      case PdfXfaDatePattern.MDYYYY:
        datePattern = "M/d/yyyy";
        break;
      case PdfXfaDatePattern.MMDDYY:
        datePattern = "MM/dd/yy";
        break;
      case PdfXfaDatePattern.MMDDYYYY:
        datePattern = "MM/dd/yyyy";
        break;
      case PdfXfaDatePattern.YYMMDD:
        datePattern = "yy/MM/dd";
        break;
      case PdfXfaDatePattern.YYYYMMDD:
        datePattern = "yyyy-MM-dd";
        break;
      case PdfXfaDatePattern.DDMMMYY:
        datePattern = "dd-MMM-yy";
        break;
      case PdfXfaDatePattern.EEEEMMMMDDYYYY:
        datePattern = "dddd, MMMM dd, yyyy";
        break;
      case PdfXfaDatePattern.MMMMDDYYYY:
        datePattern = "MMMM dd, yyyy}";
        break;
      case PdfXfaDatePattern.EEEEDDMMMMYYYY:
        datePattern = "dddd, dd MMMM, yyyy";
        break;
      case PdfXfaDatePattern.DDMMMMYYYY:
        datePattern = "dd MMMM, yyyy";
        break;
      case PdfXfaDatePattern.MMMMYYYY:
        datePattern = "MMMM, yyyy";
        break;
      case PdfXfaDatePattern.DDMMYYYY:
        datePattern = "dd/MM/yyyy";
        break;
    }
    return datePattern;
  }

  internal string GetTimePattern(PdfXfaTimePattern pattern)
  {
    string timePattern = string.Empty;
    switch (pattern)
    {
      case PdfXfaTimePattern.Default:
        timePattern = "h:mm:ss tt";
        break;
      case PdfXfaTimePattern.Short:
        timePattern = "t";
        break;
      case PdfXfaTimePattern.Medium:
        timePattern = "h:mm:ss tt";
        break;
      case PdfXfaTimePattern.Long:
        timePattern = "T";
        break;
      case PdfXfaTimePattern.Full:
        timePattern = "hh:mm:ss tt \"GMT\" zzz";
        break;
      case PdfXfaTimePattern.H_MM_A:
        timePattern = "h:mm tt";
        break;
      case PdfXfaTimePattern.H_MM_SS_A:
        timePattern = "h:mm:ss tt";
        break;
      case PdfXfaTimePattern.H_MM_SS_A_Z:
        timePattern = "h:mm:ss tt \"GMT\" zzz";
        break;
      case PdfXfaTimePattern.HH_MM_SS_A:
        timePattern = "hh:mm:ss tt";
        break;
      case PdfXfaTimePattern.H_MM_SS:
        timePattern = "H:mm:ss";
        break;
      case PdfXfaTimePattern.HH_MM_SS:
        timePattern = "HH:mm:ss";
        break;
    }
    return timePattern;
  }

  internal string GetDateTimePattern(PdfXfaDatePattern d, PdfXfaTimePattern t)
  {
    return $"{this.GetDatePattern(d)} {this.GetTimePattern(t)}";
  }

  internal string GetDatePattern(string pattern)
  {
    string datePattern = this.GetDatePattern(PdfXfaDatePattern.Default);
    switch (pattern)
    {
      case "date.short{}":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.Short);
        break;
      case "date.medium{}":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.Medium);
        break;
      case "date.long{}":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.Long);
        break;
      case "date.full{}":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.Full);
        break;
      case "date{DD MMMM, YYYY}":
      case "DD MMMM, YYYY":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.DDMMMMYYYY);
        break;
      case "date{DD-MMM-YY}":
      case "DD-MMM-YY":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.DDMMMYY);
        break;
      case "date{EEEE, DD MMMM, YYYY}":
      case "EEEE, DD MMMM, YYYY":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.EEEEMMMMDDYYYY);
        break;
      case "date{EEEE, MMMM D, YYYY}":
      case "EEEEE, MMMM D, YYYY":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.EEEE_MMMMD_YYYY);
        break;
      case "date{EEEE, MMMM DD, YYYY}":
      case "EEEE, MMMM DD, YYYY":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.EEEEMMMMDDYYYY);
        break;
      case "date{M/D/YY}":
      case "M/D/YY":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.MDYY);
        break;
      case "date{M/D/YYYY}":
      case "M/D/YYYY":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.MDYYYY);
        break;
      case "date{MM/DD/YY}":
      case "MM/DD/YY":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.MMDDYY);
        break;
      case "date{MM/DD/YYYY}":
      case "MM/DD/YYYY":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.MMDDYYYY);
        break;
      case "date{MMM D, YYYY}":
      case "MMM D, YYYY":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.MMMD_YYYY);
        break;
      case "date{MMMM D, YYYY}":
      case "MMMM D, YYYY":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.MMMMD_YYYY);
        break;
      case "date{MMMM DD, YYYY}":
      case "MMMM DD, YYYY":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.MMMMDDYYYY);
        break;
      case "date{MMMM, YYYY}":
      case "MMMM, YYYY":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.MMMMYYYY);
        break;
      case "date{YY/MM/DD}":
      case "YY/MM/DD":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.YYMMDD);
        break;
      case "date{YYYY-MM-DD}":
      case "YYYY-MM-DD":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.YYYYMMDD);
        break;
      case "date{DD/MM/YYYY}":
      case "DD/MM/YYYY":
        datePattern = this.GetDatePattern(PdfXfaDatePattern.DDMMYYYY);
        break;
    }
    return datePattern;
  }

  internal string GetTimePattern(string pattern)
  {
    string timePattern = this.GetTimePattern(PdfXfaTimePattern.Default);
    switch (pattern)
    {
      case "time.short{}":
        timePattern = this.GetTimePattern(PdfXfaTimePattern.Short);
        break;
      case "time.medium{}":
        timePattern = this.GetTimePattern(PdfXfaTimePattern.Medium);
        break;
      case "time.long{}":
        timePattern = this.GetTimePattern(PdfXfaTimePattern.Long);
        break;
      case "time.full{}":
        timePattern = this.GetTimePattern(PdfXfaTimePattern.Full);
        break;
      case "time{h:MM A}":
      case "h:MM A":
        timePattern = this.GetTimePattern(PdfXfaTimePattern.H_MM_A);
        break;
      case "time{H:MM:SS}":
      case "H:MM:SS":
        timePattern = this.GetTimePattern(PdfXfaTimePattern.H_MM_SS);
        break;
      case "time{H:MM:SS A}":
      case "H:MM:SS A":
        timePattern = this.GetTimePattern(PdfXfaTimePattern.H_MM_SS_A);
        break;
      case "time{H:MM:SS A Z}":
      case "H:MM:SS A Z":
        timePattern = this.GetTimePattern(PdfXfaTimePattern.H_MM_SS_A_Z);
        break;
      case "time{HH:MM:SS}":
      case "HH:MM:SS":
        timePattern = this.GetTimePattern(PdfXfaTimePattern.HH_MM_SS);
        break;
      case "time{hh:MM:SS A}":
      case "hh:MM:SS A":
        timePattern = this.GetTimePattern(PdfXfaTimePattern.HH_MM_SS_A);
        break;
    }
    return timePattern;
  }
}
