// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Primitives;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public abstract class PdfXfaField
{
  private string m_name = string.Empty;
  private PdfMargins m_margins = new PdfMargins();
  private PdfXfaVisibility m_visibility;
  internal string LSFN;
  internal bool m_isRendered;

  public string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  public PdfMargins Margins
  {
    get => this.m_margins;
    set => this.m_margins = value;
  }

  public PdfXfaVisibility Visibility
  {
    get => this.m_visibility;
    set => this.m_visibility = value;
  }

  internal PdfTextAlignment ConvertToPdfTextAlignment(PdfXfaHorizontalAlignment align)
  {
    PdfTextAlignment pdfTextAlignment = PdfTextAlignment.Center;
    switch (align)
    {
      case PdfXfaHorizontalAlignment.Left:
        pdfTextAlignment = PdfTextAlignment.Left;
        break;
      case PdfXfaHorizontalAlignment.Center:
        pdfTextAlignment = PdfTextAlignment.Center;
        break;
      case PdfXfaHorizontalAlignment.Right:
        pdfTextAlignment = PdfTextAlignment.Right;
        break;
      case PdfXfaHorizontalAlignment.Justify:
      case PdfXfaHorizontalAlignment.JustifyAll:
        pdfTextAlignment = PdfTextAlignment.Justify;
        break;
    }
    return pdfTextAlignment;
  }

  internal void SetFont(PdfDocumentBase doc, PdfFont font)
  {
    if (font == null || !(font is PdfTrueTypeFont))
      return;
    PdfForm form = doc.ObtainForm();
    if (form.Dictionary == null)
      return;
    PdfDictionary dictionary = form.Dictionary;
    if (dictionary.ContainsKey("DR"))
    {
      if (!(dictionary.Items[new PdfName("DR")] is PdfResources pdfResources))
        return;
      if (pdfResources.Items[new PdfName("Font")] is PdfDictionary pdfDictionary1)
      {
        if (pdfDictionary1.ContainsKey(new PdfName(font.Metrics.PostScriptName)))
          return;
        pdfDictionary1.Items.Add(new PdfName(font.Metrics.PostScriptName), (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) font));
      }
      else
      {
        PdfDictionary pdfDictionary = new PdfDictionary();
        if (!pdfDictionary.ContainsKey(new PdfName(font.Metrics.PostScriptName)))
          pdfDictionary.Items.Add(new PdfName(font.Metrics.PostScriptName), (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) font));
        pdfResources.Items.Add(new PdfName("Font"), (IPdfPrimitive) pdfDictionary);
      }
    }
    else
    {
      PdfResources pdfResources = new PdfResources();
      pdfResources.SetProperty(new PdfName("Font"), (IPdfPrimitive) new PdfDictionary()
      {
        Items = {
          {
            new PdfName(font.Metrics.PostScriptName),
            (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) font)
          }
        }
      });
      dictionary.Items.Add(new PdfName("DR"), (IPdfPrimitive) pdfResources);
    }
  }

  internal RectangleF GetBounds(RectangleF bounds, PdfXfaRotateAngle rotate, PdfXfaCaption caption)
  {
    RectangleF bounds1 = bounds;
    if (caption != null)
    {
      switch (rotate)
      {
        case PdfXfaRotateAngle.RotateAngle0:
          bounds1 = caption.Position != PdfXfaPosition.Top ? (caption.Position != PdfXfaPosition.Bottom ? (caption.Position != PdfXfaPosition.Left ? new RectangleF(new PointF(bounds1.Location.X, bounds1.Location.Y), new SizeF(bounds1.Size.Width - caption.Width, bounds1.Size.Height)) : new RectangleF(new PointF(bounds1.Location.X + caption.Width, bounds1.Location.Y), new SizeF(bounds1.Size.Width - caption.Width, bounds1.Size.Height))) : new RectangleF(new PointF(bounds1.Location.X, bounds1.Location.Y), new SizeF(bounds1.Size.Width, bounds1.Size.Height - caption.Width))) : new RectangleF(new PointF(bounds1.Location.X, bounds1.Location.Y + caption.Width), new SizeF(bounds1.Size.Width, bounds1.Size.Height - caption.Width));
          break;
        case PdfXfaRotateAngle.RotateAngle90:
          bounds1 = caption.Position != PdfXfaPosition.Top ? (caption.Position != PdfXfaPosition.Bottom ? (caption.Position != PdfXfaPosition.Left ? new RectangleF(new PointF(bounds1.Location.X, bounds1.Location.Y + caption.Width), new SizeF(bounds1.Size.Width, bounds1.Size.Height - caption.Width)) : new RectangleF(new PointF(bounds1.Location.X, bounds1.Location.Y), new SizeF(bounds1.Size.Width, bounds1.Size.Height - caption.Width))) : new RectangleF(new PointF(bounds1.Location.X, bounds1.Location.Y), new SizeF(bounds1.Size.Width - caption.Width, bounds1.Size.Height))) : new RectangleF(new PointF(bounds1.Location.X + caption.Width, bounds1.Location.Y), new SizeF(bounds1.Size.Width - caption.Width, bounds1.Size.Height));
          break;
        case PdfXfaRotateAngle.RotateAngle180:
          bounds1 = caption.Position != PdfXfaPosition.Top ? (caption.Position != PdfXfaPosition.Bottom ? (caption.Position != PdfXfaPosition.Left ? new RectangleF(new PointF(bounds1.Location.X + caption.Width, bounds1.Location.Y), new SizeF(bounds1.Size.Width - caption.Width, bounds1.Size.Height)) : new RectangleF(new PointF(bounds1.Location.X, bounds1.Location.Y), new SizeF(bounds1.Size.Width - caption.Width, bounds1.Size.Height))) : new RectangleF(new PointF(bounds1.Location.X, bounds1.Location.Y + caption.Width), new SizeF(bounds1.Size.Width, bounds1.Size.Height - caption.Width))) : new RectangleF(new PointF(bounds1.Location.X, bounds1.Location.Y), new SizeF(bounds1.Size.Width, bounds1.Size.Height - caption.Width));
          break;
        case PdfXfaRotateAngle.RotateAngle270:
          if (caption.Position == PdfXfaPosition.Top)
          {
            bounds1 = new RectangleF(new PointF(bounds1.Location.X, bounds1.Location.Y), new SizeF(bounds1.Size.Width - caption.Width, bounds1.Size.Height));
            break;
          }
          if (caption.Position == PdfXfaPosition.Bottom)
          {
            bounds1 = new RectangleF(new PointF(bounds1.Location.X + caption.Width, bounds1.Location.Y), new SizeF(bounds1.Size.Width - caption.Width, bounds1.Size.Height));
            break;
          }
          if (caption.Position == PdfXfaPosition.Left)
          {
            bounds1 = new RectangleF(new PointF(bounds1.Location.X, bounds1.Location.Y + caption.Width), new SizeF(bounds1.Size.Width, bounds1.Size.Height - caption.Width));
            break;
          }
          ref RectangleF local = ref bounds1;
          PointF location = new PointF(bounds1.Location.X, bounds1.Location.Y);
          SizeF size1 = bounds1.Size;
          double width = (double) size1.Width;
          size1 = bounds1.Size;
          double height = (double) size1.Height - (double) caption.Width;
          SizeF size2 = new SizeF((float) width, (float) height);
          local = new RectangleF(location, size2);
          break;
      }
    }
    return bounds1;
  }

  internal string GetPattern(string patternText)
  {
    string str = patternText;
    if (patternText.Contains("|"))
      str = patternText.Substring(0, patternText.IndexOf("|"));
    return str.Replace("date{", string.Empty).Replace("}", string.Empty).Replace('Y', 'y').Replace('D', 'd').Replace("time{", string.Empty).Replace("}", string.Empty);
  }

  internal string TrimDatePattern(string patternText)
  {
    string str1 = patternText;
    if (patternText.Contains("|"))
      str1 = patternText.Substring(0, patternText.IndexOf("|"));
    string str2 = str1.TrimEnd().TrimStart();
    if (str2.Contains("date.short{}"))
      return "date.short{}";
    if (str2.Contains("date.medium{}"))
      return "date.medium{}";
    if (str2.Contains("date.long{}"))
      return "date.long{}";
    if (str2.Contains("date.full{}"))
      return "date.full{}";
    if (str2.Contains("date{"))
    {
      int startIndex = str2.IndexOf("date{");
      int length = str2.IndexOf("}", startIndex);
      str2 = str2.Substring(startIndex, length);
    }
    else if (str2.Contains("h"))
      str2 = str2.Substring(0, str2.IndexOf("h"));
    else if (str2.Contains("H"))
      str2 = str2.Substring(0, str2.IndexOf("H"));
    return str2.Replace("date{", string.Empty).Replace("}", string.Empty).TrimEnd();
  }

  internal string TrimTimePattern(string patternText)
  {
    string str1 = patternText;
    if (patternText.Contains("|"))
      str1 = patternText.Substring(0, patternText.IndexOf("|"));
    string str2 = str1.TrimEnd().TrimStart();
    if (str2.Contains("time.short{}"))
      return "time.short{}";
    if (str2.Contains("time.medium{}"))
      return "time.medium{}";
    if (str2.Contains("time.long{}"))
      return "time.long{}";
    if (str2.Contains("time.full{}"))
      return "time.full{}";
    if (str2.Contains("time{"))
    {
      int startIndex = str2.IndexOf("time{");
      int num = str2.IndexOf("}", startIndex);
      str2 = str2.Substring(startIndex, num - startIndex);
    }
    else if (str2.Contains("h"))
    {
      int startIndex = str2.IndexOf("h");
      int length = str2.Length - startIndex;
      str2 = str2.Substring(startIndex, length);
    }
    else if (str2.Contains("H"))
    {
      int startIndex = str2.IndexOf("H");
      int length = str2.Length - startIndex;
      str2 = str2.Substring(startIndex, length);
    }
    return str2.Replace("time{", string.Empty).Replace("}", string.Empty).TrimEnd();
  }
}
