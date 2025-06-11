// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.XPSRenderer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Graphics.Fonts;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Native;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

#nullable disable
namespace Syncfusion.XPS;

internal class XPSRenderer : IDisposable
{
  private const float m_decrementFactor = 0.275f;
  private PdfGraphics m_graphics;
  private PdfPage m_page;
  private PdfUnitConvertor m_unitConvertor;
  private XPSDocumentReader m_reader;
  private char[] m_commaSeparator = new char[1]{ ',' };
  private bool m_bStateChanged;
  private PdfTransformationMatrix currentMatrix;
  private Canvas m_canvas;
  private float m_locationY;
  private bool m_isLineClip;
  private XPSToPdfConverterSettings m_embedFont = new XPSToPdfConverterSettings();
  private TtfReader m_tffReader;
  private RectangleF v_viewport;
  private PdfGraphicsState v_graphicsState;
  private string m_hashValue;
  private PdfDocument m_document;
  private int count;
  private List<float> list = new List<float>();

  public XPSRenderer(PdfPage page, XPSDocumentReader reader)
  {
    this.m_page = page != null ? page : throw new ArgumentNullException(nameof (page));
    this.m_graphics = page.Graphics;
    this.m_unitConvertor = new PdfUnitConvertor(96f);
    this.m_reader = reader;
  }

  internal PdfGraphics Graphics => this.m_graphics;

  internal PdfDocument Document
  {
    get => this.m_document;
    set => this.m_document = value;
  }

  internal PrivateFontCollection PrivateFonts => PdfDocument.PrivateFonts;

  internal XPSToPdfConverterSettings EmbedFont
  {
    get => this.m_embedFont;
    set => this.m_embedFont = value;
  }

  public void DrawGlyphs(Glyphs glyphs, PdfGraphics m_graphics)
  {
    PdfGraphicsState state1 = m_graphics.Save();
    if (glyphs.Opacity < 1.0)
      this.Graphics.SetTransparency((float) glyphs.Opacity);
    if (glyphs.RenderTransform != null)
    {
      string renderTransform = glyphs.RenderTransform;
      float[] elements = (!renderTransform.Contains("StaticResource") ? this.ReadMatrix(renderTransform) : this.ReadMatrix((this.ReadStaticResource(renderTransform) as MatrixTransform).Matrix)).Elements;
      if (this.count == 0 || glyphs.OriginX == 0.0 && glyphs.OriginY == 0.0 || (double) elements[0] == 0.0)
      {
        this.ApplyRenderTransform(renderTransform);
      }
      else
      {
        glyphs.OriginX *= (double) elements[0];
        glyphs.OriginY *= (double) elements[3];
        glyphs.FontRenderingEmSize *= (double) elements[0];
      }
    }
    if (glyphs.GlyphsRenderTransform != null)
    {
      state1 = m_graphics.Save();
      this.ApplyRenderTransform(glyphs.GlyphsRenderTransform.MatrixTransform.Matrix);
    }
    m_graphics.m_isNormalRender = false;
    if ((glyphs.UnicodeString != null || glyphs.Indices != null) && glyphs.FontRenderingEmSize > 0.0)
    {
      PointF pointF = new PointF(this.ConvertToPoints(glyphs.OriginX), this.ConvertToPoints(glyphs.OriginY));
      this.m_locationY = pointF.Y;
      PdfFont font1 = this.GetFont(glyphs);
      int result = 0;
      int.TryParse(glyphs.BidiLevel, out result);
      PdfStringFormat pdfStringFormat = new PdfStringFormat();
      pdfStringFormat.MeasureTrailingSpaces = true;
      if (glyphs.UnicodeString != null && PdfString.IsUnicode(glyphs.UnicodeString))
      {
        string unicodeString = glyphs.UnicodeString;
        ushort[] lpCharType = new ushort[unicodeString.Length];
        KernelApi.GetStringTypeExW(2048U /*0x0800*/, StringInfoType.CT_TYPE2, unicodeString, unicodeString.Length, lpCharType);
        if (result % 2 != 0)
        {
          pdfStringFormat.RightToLeft = true;
          pdfStringFormat.Alignment = PdfTextAlignment.Right;
        }
      }
      else if (result % 2 != 0)
      {
        pdfStringFormat.RightToLeft = true;
        pdfStringFormat.Alignment = PdfTextAlignment.Right;
      }
      float ascent = font1.Metrics.GetAscent(pdfStringFormat);
      float lineGap = font1.Metrics.GetLineGap(pdfStringFormat);
      if (!glyphs.IsSideways)
        pointF.Y -= ascent + lineGap;
      PdfBrush brush = this.GetSolidBrush(glyphs.Fill);
      if (glyphs.Fill != null && !glyphs.Fill.Contains("StaticResource") && !glyphs.Fill.Contains("icc"))
      {
        PdfColor pdfColor = new PdfColor(ColorTranslator.FromHtml(glyphs.Fill));
        if (pdfColor.A != byte.MaxValue)
          this.Graphics.SetTransparency((float) pdfColor.A / (float) byte.MaxValue);
      }
      PdfPen pen = (PdfPen) null;
      bool boldStyle = false;
      if (glyphs.Fill == null && glyphs.GlyphsFill != null)
      {
        if (glyphs.GlyphsFill.Item is SolidColorBrush)
          brush = (PdfBrush) new PdfSolidBrush((PdfColor) this.FromHtml((glyphs.GlyphsFill.Item as SolidColorBrush).Color));
        else if (glyphs.GlyphsFill.Item is LinearGradientBrush)
          brush = (PdfBrush) this.ReadLinearGradientBrush(glyphs.GlyphsFill.Item as LinearGradientBrush);
        else if (glyphs.GlyphsFill.Item is RadialGradientBrush)
          brush = (PdfBrush) this.ReadRadialGradientBrush(glyphs.GlyphsFill.Item as RadialGradientBrush);
      }
      if ((glyphs.StyleSimulations == StyleSimulations.BoldSimulation || glyphs.StyleSimulations == StyleSimulations.BoldItalicSimulation) && !font1.Metrics.PostScriptName.Contains("Bold"))
      {
        boldStyle = true;
        pen = new PdfPen(brush);
        pen.Width = 0.3f;
      }
      if (glyphs.Clip != null)
      {
        PdfPath path = this.GetPathFromGeometry(glyphs.Clip);
        PointF[] points = new PointF[path.PathPoints.Length];
        float num1 = 0.0f;
        float num2 = 0.0f;
        float num3 = 0.0f;
        Matrix matrix = (Matrix) null;
        if (glyphs.RenderTransform != null)
        {
          string renderTransform = glyphs.RenderTransform;
          if (!renderTransform.Contains("StaticResource"))
            matrix = this.ReadMatrix(renderTransform);
        }
        for (int index = 0; index < path.PathPoints.Length; ++index)
        {
          if (matrix != null && (double) matrix.OffsetX == 0.0 && (double) matrix.OffsetY == 0.0)
          {
            points[index].X = path.PathPoints[index].X * matrix.Elements[0];
            points[index].Y = path.PathPoints[index].Y * matrix.Elements[3];
          }
          switch (index)
          {
            case 1:
              num1 = path.PathPoints[index].X;
              break;
            case 2:
              num2 = path.PathPoints[index].X;
              break;
            case 3:
              num3 = path.PathPoints[index].X;
              break;
          }
        }
        if (matrix != null)
          path = new PdfPath(points, path.PathTypes);
        if ((double) path.PathPoints[0].X != (double) num1 && (double) num1 == (double) num2 && (double) num3 == (double) path.PathPoints[0].X || !this.m_isLineClip)
          m_graphics.SetClip(path);
      }
      PdfGraphicsState state2 = (PdfGraphicsState) null;
      if ((glyphs.StyleSimulations == StyleSimulations.ItalicSimulation || glyphs.StyleSimulations == StyleSimulations.BoldItalicSimulation) && !font1.Metrics.PostScriptName.Contains("Italic"))
      {
        state2 = m_graphics.Save();
        if ((double) pointF.X == 0.0)
          pointF.X = (float) -((double) font1.Height * Math.Cos(80.0));
        m_graphics.TranslateTransform(pointF.X, pointF.Y);
        m_graphics.SkewTransform(0.0f, -10f);
        pointF = PointF.Empty;
      }
      if (glyphs.Indices != null && (pdfStringFormat.RightToLeft || glyphs.Indices.Contains("(") && glyphs.Indices.Contains(")")))
      {
        string[] glyphIndices = glyphs.Indices.Split(';');
        if (glyphIndices.Length != glyphs.UnicodeString.Length && glyphs.Indices.Contains("(") && glyphs.Indices.Contains(")"))
        {
          char[] charArray1 = glyphs.UnicodeString.ToCharArray();
          List<string> uniCodeString = new List<string>();
          for (int index = 0; index < charArray1.Length; ++index)
            uniCodeString.Insert(index, charArray1[index].ToString());
          string str1 = "";
          List<string> ligatureIndices = new List<string>();
          List<int> intList1 = new List<int>();
          List<int> intList2 = new List<int>();
          int num4 = 1;
          int num5 = 0;
          int num6 = 1;
          bool flag1 = false;
          int index1 = 0;
          for (int length = glyphIndices.Length; index1 < length; ++index1)
          {
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            bool flag5 = false;
            char[] chArray;
            if (glyphIndices[index1].Contains("(") && glyphIndices[index1].Contains(")"))
            {
              if (glyphIndices[index1].Contains(","))
              {
                char[] charArray2 = glyphIndices[index1].ToCharArray();
                for (int index2 = 0; index2 < charArray2.Length; ++index2)
                {
                  int num7 = (int) charArray2[index2];
                  if (charArray2[index2] == '(')
                  {
                    flag2 = true;
                    num4 = index1;
                    num6 = int.Parse(charArray2[index2 + 1].ToString(), (IFormatProvider) CultureInfo.InvariantCulture);
                  }
                  if (charArray2[index2] == ':')
                  {
                    flag5 = true;
                    num5 = int.Parse(charArray2[index2 + 1].ToString(), (IFormatProvider) CultureInfo.InvariantCulture);
                  }
                  if (charArray2[index2] == ',')
                    flag4 = true;
                  if (flag2 && flag3 && !flag4)
                    str1 += charArray2[index2].ToString();
                  if (charArray2[index2] == ')')
                    flag3 = true;
                  if (flag4 && flag5 && str1 != null)
                  {
                    ligatureIndices.Add(str1);
                    str1 = (string) null;
                    intList1.Add(num4);
                    intList2.Add(num6);
                  }
                  if (num5 > 1 && flag4)
                    break;
                }
                if (!flag5 && str1 != null)
                {
                  ligatureIndices.Add(str1);
                  num5 = 1;
                  intList1.Add(num4);
                  intList2.Add(num6);
                }
                chArray = (char[]) null;
                str1 = (string) null;
              }
              else
              {
                char[] charArray3 = glyphIndices[index1].ToCharArray();
                for (int index3 = 0; index3 < charArray3.Length; ++index3)
                {
                  int num8 = (int) charArray3[index3];
                  if (charArray3[index3] == '(')
                  {
                    flag2 = true;
                    num4 = index1;
                    num6 = int.Parse(charArray3[index3 + 1].ToString(), (IFormatProvider) CultureInfo.InvariantCulture);
                  }
                  if (charArray3[index3] == ':')
                    num5 = int.Parse(charArray3[index3 + 1].ToString(), (IFormatProvider) CultureInfo.InvariantCulture);
                  if (flag2 && flag3)
                    str1 += charArray3[index3].ToString();
                  if (charArray3[index3] == ')')
                    flag3 = true;
                  if (num5 > 1)
                    break;
                }
                if (str1 != null && str1 != string.Empty)
                {
                  ligatureIndices.Add(str1);
                  intList1.Add(num4);
                  intList2.Add(num6);
                }
                chArray = (char[]) null;
                str1 = (string) null;
              }
            }
            else if (glyphIndices[index1].Length > 0 && glyphIndices[index1].Contains(","))
            {
              if (glyphIndices[index1].Split(',')[1] == string.Empty)
                flag1 = true;
            }
          }
          StringBuilder stringBuilder = new StringBuilder();
          string empty1 = string.Empty;
          string empty2 = string.Empty;
          bool flag6 = false;
          for (int index4 = 0; index4 < ligatureIndices.Count; ++index4)
          {
            int glyphIndex = int.Parse(ligatureIndices[index4], (IFormatProvider) CultureInfo.InvariantCulture);
            TtfGlyphInfo glyph = this.m_tffReader.GetGlyph(glyphIndex);
            if (glyph.CharCode == 32 /*0x20*/ || glyph.CharCode == -1)
            {
              string key = Convert.ToChar(glyphIndex).ToString();
              m_graphics.XPSToken = true;
              if (!m_graphics.XPSReplaceCharacter.ContainsKey(key))
                m_graphics.XPSReplaceCharacter.Add(key, key);
              uniCodeString.RemoveRange(intList1[index4], intList2[index4]);
              uniCodeString.Insert(intList1[index4], key);
              flag6 = true;
            }
            else if (!glyph.Empty)
            {
              string key = Convert.ToChar(glyph.CharCode).ToString();
              if (glyph.CharCode >= 64256 && glyph.CharCode <= 64335 || glyphIndex == glyph.CharCode)
              {
                if (glyphIndex == glyph.CharCode)
                {
                  m_graphics.XPSToken = true;
                  if (!m_graphics.XPSReplaceCharacter.ContainsKey(key))
                    m_graphics.XPSReplaceCharacter.Add(key, key);
                }
                uniCodeString.RemoveRange(intList1[index4], intList2[index4]);
                uniCodeString.Insert(intList1[index4], key);
                flag6 = true;
              }
            }
          }
          if (!pdfStringFormat.RightToLeft && !flag1 && flag6)
          {
            if (font1 is PdfTrueTypeFont pdfTrueTypeFont && pdfTrueTypeFont.InternalFont is UnicodeTrueTypeFont internalFont)
              internalFont.m_isXPSFontStream = true;
            this.DrawGlyphTextElement(glyphs, pointF, glyphIndices, uniCodeString, font1, brush, pen, pdfStringFormat, boldStyle, ligatureIndices, ascent);
          }
          else
          {
            foreach (string str2 in uniCodeString)
              stringBuilder.Append(str2);
            string s = stringBuilder.ToString();
            m_graphics.DrawString(s, font1, brush, pointF, pdfStringFormat);
          }
          ligatureIndices.Clear();
          intList1.Clear();
          intList2.Clear();
          m_graphics.XPSToken = false;
          m_graphics.XPSReplaceCharacter.Clear();
          if (state1 == null)
            return;
          m_graphics.Restore(state1);
          return;
        }
      }
      SizeF sizeF;
      PdfGraphicsState pdfGraphicsState;
      if (glyphs.UnicodeString != null && glyphs.Indices != null && glyphs.Indices.Contains(",") && !glyphs.Indices.Contains("(") && !glyphs.Indices.Contains(")") && !glyphs.Indices.Contains(".") && !glyphs.Indices.Contains("-") && !glyphs.Indices.Contains("E") && !glyphs.Indices.Contains("e"))
      {
        double locationY = (double) this.m_locationY;
        sizeF = this.m_page.Size;
        double height = (double) sizeF.Height;
        if (locationY < height || glyphs.RenderTransform != null)
        {
          string[] glyphIndices = glyphs.Indices.Split(';');
          if (glyphIndices.Length > 0 && glyphIndices.Length <= glyphs.UnicodeString.Length && !pdfStringFormat.RightToLeft)
          {
            char[] charArray = glyphs.UnicodeString.ToCharArray();
            List<string> uniCodeString = new List<string>();
            for (int index = 0; index < charArray.Length; ++index)
              uniCodeString.Insert(index, charArray[index].ToString());
            this.DrawGlyphTextElement(glyphs, pointF, glyphIndices, uniCodeString, font1, brush, pen, pdfStringFormat, boldStyle, (List<string>) null, ascent);
            if (state2 != null)
            {
              m_graphics.Restore(state2);
              pdfGraphicsState = (PdfGraphicsState) null;
            }
            if (state1 == null)
              return;
            m_graphics.Restore(state1);
            return;
          }
          m_graphics.DrawString(glyphs.UnicodeString, font1, brush, pointF, pdfStringFormat);
          return;
        }
      }
      if (glyphs.UnicodeString == null && glyphs.Indices != null && !glyphs.Indices.Contains(";"))
      {
        string s = glyphs.Indices.Split(',')[0];
        if (s != string.Empty)
        {
          TtfGlyphInfo glyph = this.m_tffReader.GetGlyph(int.Parse(s));
          if (!glyph.Empty)
            glyphs.UnicodeString = Convert.ToChar(glyph.CharCode).ToString();
        }
      }
      if (glyphs.IsSideways)
      {
        m_graphics.RotateTransform(-90f);
        foreach (char ch in glyphs.UnicodeString.ToCharArray())
        {
          PdfStringLayouter pdfStringLayouter = new PdfStringLayouter();
          string text = ch.ToString();
          PdfFont font2 = font1;
          PdfStringFormat format = pdfStringFormat;
          sizeF = new SizeF();
          SizeF size = sizeF;
          PdfStringLayoutResult stringLayoutResult = pdfStringLayouter.Layout(text, font2, format, size);
          PointF point = pointF;
          m_graphics.m_isNormalRender = false;
          ref PointF local1 = ref point;
          double num9 = -(double) pointF.Y;
          sizeF = stringLayoutResult.ActualSize;
          double num10 = (double) sizeF.Width / 2.0;
          double num11 = num9 - num10;
          local1.X = (float) num11;
          point.Y = pointF.X;
          if (!boldStyle)
            m_graphics.DrawString(ch.ToString(), font1, brush, point, pdfStringFormat);
          else
            m_graphics.DrawString(ch.ToString(), font1, pen, brush, point, pdfStringFormat);
          ref PointF local2 = ref pointF;
          double x = (double) local2.X;
          sizeF = stringLayoutResult.ActualSize;
          double points = (double) this.ConvertToPoints((double) sizeF.Height);
          local2.X = (float) (x + points);
        }
        m_graphics.RotateTransform(90f);
      }
      else if (glyphs.UnicodeString != null)
      {
        if (!boldStyle)
        {
          if (glyphs.UnicodeString.StartsWith("{}"))
          {
            string s = glyphs.UnicodeString.Substring(2);
            m_graphics.DrawString(s, font1, brush, pointF, pdfStringFormat);
          }
          else
            m_graphics.DrawString(glyphs.UnicodeString, font1, brush, pointF, pdfStringFormat);
        }
        else if (glyphs.UnicodeString.StartsWith("{}"))
        {
          string s = glyphs.UnicodeString.Substring(2);
          m_graphics.DrawString(s, font1, pen, brush, pointF, pdfStringFormat);
        }
        else
          m_graphics.DrawString(glyphs.UnicodeString, font1, pen, brush, pointF, pdfStringFormat);
      }
      if (state2 != null)
      {
        m_graphics.Restore(state2);
        pdfGraphicsState = (PdfGraphicsState) null;
      }
    }
    if (state1 == null)
      return;
    m_graphics.Restore(state1);
  }

  private bool IsRTLText(ushort[] characterCodes)
  {
    bool flag = false;
    int index = 0;
    for (int length = characterCodes.Length; index < length; ++index)
    {
      if (characterCodes[index] == (ushort) 2)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private PdfImageMask CheckAlpha(Bitmap bitmap)
  {
    PdfImageMask pdfImageMask = (PdfImageMask) null;
    switch (bitmap.PixelFormat)
    {
      case PixelFormat.Format1bppIndexed:
      case PixelFormat.Format4bppIndexed:
      case PixelFormat.Format8bppIndexed:
        Color[] entries = bitmap.Palette.Entries;
        pdfImageMask = this.CheckAlpha(bitmap.Palette.Flags, (Image) bitmap, entries);
        break;
      case PixelFormat.Format32bppPArgb:
      case PixelFormat.Format32bppArgb:
        pdfImageMask = new PdfImageMask(new PdfBitmap((Image) PdfBitmap.CreateMaskFromARGBImage((Image) bitmap)));
        break;
    }
    return pdfImageMask;
  }

  private Bitmap CompressImage(Bitmap image)
  {
    Bitmap bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) bitmap))
    {
      graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
      graphics.DrawImage((Image) image, new Rectangle(0, 0, image.Width, image.Height));
    }
    MemoryStream memoryStream = new MemoryStream();
    bitmap.Save((Stream) memoryStream, ImageFormat.Png);
    return Image.FromStream((Stream) memoryStream) as Bitmap;
  }

  private PdfImageMask CheckAlpha(int flags, Image bitmap, Color[] array)
  {
    PdfImageMask pdfImageMask = (PdfImageMask) null;
    bool flag = false;
    int index = 0;
    for (int length = array.Length; index < length; ++index)
    {
      if (flags == 1 && array[index].A < byte.MaxValue)
        flag = true;
    }
    if (flag)
      pdfImageMask = new PdfImageMask(new PdfBitmap(PdfBitmap.CreateMaskFromIndexedImage(bitmap)));
    return pdfImageMask;
  }

  private Bitmap clipXpsImage(Bitmap sourceImage, Rectangle Bounds)
  {
    Bitmap bitmap = new Bitmap(Bounds.Width, Bounds.Height);
    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) bitmap))
      graphics.DrawImage((Image) sourceImage, new Rectangle(0, 0, bitmap.Width, bitmap.Height), Bounds, GraphicsUnit.Pixel);
    return bitmap;
  }

  public PdfPath GetPathFromGeometry(string pathData)
  {
    this.m_isLineClip = false;
    if (pathData == null)
      return (PdfPath) null;
    if (pathData.Contains("StaticResource"))
    {
      PathGeometry xpsPathGeometry = this.ReadStaticResource(pathData) as PathGeometry;
      if (xpsPathGeometry.Figures == null)
        return this.GetPathFromPathGeometry(xpsPathGeometry);
      pathData = xpsPathGeometry.Figures;
    }
    pathData = pathData.Trim();
    XPSPathDataReader xpsPathDataReader = new XPSPathDataReader(pathData);
    xpsPathDataReader.Position = 0;
    PointF pointF1 = PointF.Empty;
    PointF pointF2 = PointF.Empty;
    bool flag = false;
    GraphicsPath graphicsPath = new GraphicsPath();
    char ch;
    PointF val1;
    float num1;
    float num2;
    while ((ch = xpsPathDataReader.ReadSymbol()) != char.MinValue)
    {
      switch (ch)
      {
        case 'A':
          if (xpsPathDataReader.TryReadPoint(out val1))
          {
            float rotationAngle;
            xpsPathDataReader.TryReadFloat(out rotationAngle);
            float num3;
            xpsPathDataReader.TryReadFloat(out num3);
            float num4;
            xpsPathDataReader.TryReadFloat(out num4);
            PointF val2;
            xpsPathDataReader.TryReadPoint(out val2);
            val1 = this.ConvertToPoints(val1);
            val2 = this.ConvertToPoints(val2);
            if (pointF2 != PointF.Empty)
              pointF1 = pointF2;
            List<PointF> arc = this.ComputeArc(pointF1, val2, val1.X, val1.Y, (double) rotationAngle, (double) num3 == 1.0, (double) num4 != 1.0);
            if (arc.Count == 0)
            {
              graphicsPath.AddLine(pointF1, val2);
              pointF1 = val2;
            }
            else if (arc.Count < 0)
              continue;
            graphicsPath.AddBeziers(arc.ToArray());
            pointF2 = pointF1 = arc[arc.Count - 1];
            arc.Clear();
            continue;
          }
          continue;
        case 'C':
          List<PointF> pointFList1 = new List<PointF>();
          if (pointF2 != PointF.Empty)
            pointFList1.Add(pointF2);
          else
            pointFList1.Add(pointF1);
          PointF[] val3 = (PointF[]) null;
          while (xpsPathDataReader.TryReadPointM3(out val3))
          {
            foreach (PointF point in val3)
              pointFList1.Add(this.ConvertToPoints(point));
          }
          if ((pointFList1.Count - 1) % 3 == 0)
            graphicsPath.AddBeziers(pointFList1.ToArray());
          else
            graphicsPath.AddLines(pointFList1.ToArray());
          pointF2 = pointFList1[pointFList1.Count - 1];
          pointFList1.Clear();
          continue;
        case 'F':
          float num5;
          if (xpsPathDataReader.TryReadFloat(out num5))
          {
            graphicsPath.FillMode = (double) num5 == 0.0 ? FillMode.Alternate : FillMode.Winding;
            continue;
          }
          continue;
        case 'H':
          if (xpsPathDataReader.TryReadFloat(out num1))
          {
            float points = this.ConvertToPoints((double) num1);
            graphicsPath.AddLine(pointF1, new PointF(points, pointF1.Y));
            pointF1 = new PointF(points, pointF1.Y);
            continue;
          }
          continue;
        case 'L':
        case 'l':
          this.m_isLineClip = true;
          List<PointF> pointFList2 = new List<PointF>();
          PointF pt1 = !(pointF2 != PointF.Empty) ? pointF1 : pointF2;
          while (xpsPathDataReader.TryReadPoint(out val1))
          {
            graphicsPath.AddLine(pt1, this.ConvertToPoints(val1));
            pt1 = pointF2 = this.ConvertToPoints(val1);
          }
          continue;
        case 'M':
          if (xpsPathDataReader.TryReadPoint(out val1))
          {
            graphicsPath.StartFigure();
            pointF1 = this.ConvertToPoints(val1);
            pointF2 = PointF.Empty;
            continue;
          }
          continue;
        case 'V':
          if (xpsPathDataReader.TryReadFloat(out num2))
          {
            float points = this.ConvertToPoints((double) num2);
            graphicsPath.AddLine(pointF1, new PointF(pointF1.X, points));
            pointF1 = new PointF(pointF1.X, points);
            continue;
          }
          continue;
        case 'Z':
        case 'z':
          if (xpsPathDataReader.EOF)
          {
            graphicsPath.CloseAllFigures();
            flag = true;
            continue;
          }
          graphicsPath.CloseFigure();
          continue;
        case 'h':
          if (xpsPathDataReader.TryReadFloat(out num1))
          {
            float points = this.ConvertToPoints((double) num1);
            graphicsPath.AddLine(pointF1, new PointF(pointF1.X + points, pointF1.Y));
            pointF1 = new PointF(pointF1.X + points, pointF1.Y);
            continue;
          }
          continue;
        case 'v':
          if (xpsPathDataReader.TryReadFloat(out num2))
          {
            float points = this.ConvertToPoints((double) num2);
            graphicsPath.AddLine(pointF1, new PointF(pointF1.X, pointF1.Y + points));
            pointF1 = new PointF(pointF1.X, pointF1.Y + points);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
    if (graphicsPath.PathData.Points.LongLength <= 0L)
      return new PdfPath();
    PdfPath pathFromGeometry = new PdfPath(graphicsPath.PathPoints, graphicsPath.PathTypes);
    pathFromGeometry.FillMode = graphicsPath.FillMode == FillMode.Alternate ? PdfFillMode.Alternate : PdfFillMode.Winding;
    if (flag)
      pathFromGeometry.CloseAllFigures();
    return pathFromGeometry;
  }

  private List<PointF> ComputeArc(
    PointF startPoint,
    PointF endPoint,
    float radiusX,
    float radiusY,
    double rotationAngle,
    bool isLargeArc,
    bool isCounterClockwise)
  {
    List<PointF> arc = new List<PointF>();
    arc.Add(startPoint);
    bool flag = false;
    Matrix matrix1 = new Matrix();
    double num1 = ((double) endPoint.X - (double) startPoint.X) / 2.0;
    double num2 = ((double) endPoint.Y - (double) startPoint.Y) / 2.0;
    PointF pointF1 = new PointF(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
    double num3 = Math.Sqrt((double) pointF1.X * (double) pointF1.X + (double) pointF1.Y * (double) pointF1.Y) / 2.0;
    rotationAngle = -rotationAngle * (Math.PI / 180.0);
    double num4 = Math.Cos(rotationAngle);
    double num5 = Math.Sin(rotationAngle);
    double num6 = num1 * num4 - num2 * num5;
    double num7 = num1 * num5 + num2 * num4;
    double num8 = num6 / (double) radiusX;
    double num9 = num7 / (double) radiusY;
    double d = num8 * num8 + num9 * num9;
    double num10;
    double num11;
    if (d > 1.0)
    {
      double num12 = Math.Sqrt(d);
      radiusX *= (float) num12;
      radiusY *= (float) num12;
      num11 = num10 = 0.0;
      flag = true;
      num8 /= num12;
      num9 /= num12;
    }
    else
    {
      double num13 = d != 0.0 ? Math.Sqrt((1.0 - d) / d) : d;
      if (isLargeArc == isCounterClockwise)
      {
        num11 = -num13 * num9;
        num10 = num13 * num8;
      }
      else
      {
        num11 = num13 * num9;
        num10 = -num13 * num8;
      }
    }
    PointF startPoint1 = new PointF((float) (-num8 - num11), (float) (-num9 - num10));
    PointF endPoint1 = new PointF((float) (num8 - num11), (float) (num9 - num10));
    double num14 = !flag ? num4 * (double) radiusX * num11 + num5 * (double) radiusX * num10 : 0.0;
    double num15 = !flag ? -num5 * (double) radiusY * num11 + num4 * (double) radiusY * num10 : 0.0;
    Matrix matrix2 = new Matrix((float) num4 * radiusX, (float) -num5 * radiusX, (float) num5 * radiusY, (float) num4 * radiusY, (float) num14 + (float) (((double) endPoint.X + (double) startPoint.X) / 2.0), (float) num15 + (float) (((double) endPoint.Y + (double) startPoint.Y) / 2.0));
    double cosArcAngle;
    double sinArcAngle;
    int lines;
    this.GetArcAngle(startPoint1, endPoint1, isLargeArc, isCounterClockwise, out cosArcAngle, out sinArcAngle, out lines);
    double num16 = this.GetBezierDistance(cosArcAngle, 1.0);
    if (isCounterClockwise)
      num16 = -num16;
    PointF pointF2 = new PointF((float) -num16 * startPoint1.Y, (float) num16 * startPoint1.X);
    PointF pointF3 = PointF.Empty;
    PointF pointF4 = PointF.Empty;
    PointF pointF5;
    for (int index = 1; index < lines; ++index)
    {
      PointF pointF6 = new PointF((float) ((double) startPoint1.X * cosArcAngle - (double) startPoint1.Y * sinArcAngle), (float) ((double) startPoint1.X * sinArcAngle + (double) startPoint1.Y * cosArcAngle));
      pointF5 = new PointF((float) -num16 * pointF6.Y, (float) num16 * pointF6.X);
      pointF3 = new PointF(startPoint1.X + pointF2.X, startPoint1.Y + pointF2.Y);
      pointF4 = new PointF(pointF6.X - pointF5.X, pointF6.Y - pointF5.Y);
      PointF[] pointFArray = new PointF[3]
      {
        pointF3,
        pointF4,
        pointF6
      };
      matrix2.TransformPoints(pointFArray);
      arc.AddRange((IEnumerable<PointF>) pointFArray);
      startPoint1 = pointF6;
      pointF2 = pointF5;
    }
    pointF5 = new PointF((float) -num16 * endPoint1.Y, (float) num16 * endPoint1.X);
    pointF3 = new PointF(startPoint1.X + pointF2.X, startPoint1.Y + pointF2.Y);
    pointF4 = new PointF(endPoint1.X - pointF5.X, endPoint1.Y - pointF5.Y);
    PointF[] pointFArray1 = new PointF[2]
    {
      pointF3,
      pointF4
    };
    matrix2.TransformPoints(pointFArray1);
    arc.AddRange((IEnumerable<PointF>) pointFArray1);
    arc.Add(new PointF(endPoint.X, endPoint.Y));
    return arc;
  }

  private void GetArcAngle(
    PointF startPoint,
    PointF endPoint,
    bool isLargeArc,
    bool isCounterClockwise,
    out double cosArcAngle,
    out double sinArcAngle,
    out int lines)
  {
    cosArcAngle = (double) startPoint.X * (double) endPoint.X + (double) startPoint.Y * (double) endPoint.Y;
    sinArcAngle = (double) startPoint.X * (double) endPoint.Y - (double) startPoint.Y * (double) endPoint.X;
    if (cosArcAngle >= 0.0)
    {
      if (isLargeArc)
      {
        lines = 4;
      }
      else
      {
        lines = 1;
        return;
      }
    }
    else
      lines = !isLargeArc ? 2 : 3;
    double num1 = Math.Atan2(sinArcAngle, cosArcAngle);
    if (!isCounterClockwise)
    {
      if (num1 < 0.0)
        num1 += 2.0 * Math.PI;
    }
    else if (num1 > 0.0)
      num1 -= 2.0 * Math.PI;
    double num2 = num1 / (double) lines;
    cosArcAngle = Math.Cos(num2);
    sinArcAngle = Math.Sin(num2);
  }

  private double GetBezierDistance(double dot, double radius)
  {
    double num1 = radius * radius;
    double bezierDistance = 0.0;
    double d1 = (num1 + dot) / 2.0;
    if (d1 < 0.0)
      return bezierDistance;
    double d2 = num1 - d1;
    if (d2 <= 0.0)
      return bezierDistance;
    double num2 = Math.Sqrt(d2);
    double num3 = Math.Sqrt(d1);
    return 4.0 * (radius - num3) / 3.0 > num2 * 1E-06 ? 4.0 * (radius - num3) / num2 / 3.0 : 0.0;
  }

  private void ThrowNotImplementedException()
  {
  }

  public void DrawPath(Path path, PdfGraphics Graphics)
  {
    float[] numArray1 = (float[]) null;
    PdfPen pen1 = (PdfPen) null;
    PdfBrush brush1 = (PdfBrush) null;
    SizeF empty1 = SizeF.Empty;
    bool flag1 = false;
    Image image1 = (Image) null;
    PdfImageMask pdfImageMask = (PdfImageMask) null;
    SizeF size1 = SizeF.Empty;
    PdfGraphicsState state1 = Graphics.Save();
    PdfPath path1 = this.GetPathFromGeometry(path.Data);
    if (path1 == null && path.PathData != null)
    {
      path1 = this.GetPathFromPathGeometry(path.PathData.PathGeometry);
      if (path.PathData.PathGeometry != null && path.PathData.PathGeometry.PathFigure != null)
      {
        if (path.PathData.PathGeometry.PathFigure.Length > 1)
        {
          string fill = path.Fill;
          foreach (PathFigure pathFigure in path.PathData.PathGeometry.PathFigure)
          {
            Path path2 = new Path();
            Path path3 = path;
            path3.Fill = fill;
            path3.PathData.PathGeometry.PathFigure = new PathFigure[1];
            path3.PathData.PathGeometry.PathFigure[0] = pathFigure;
            this.DrawPath(path3, Graphics);
          }
          return;
        }
        if (!path.PathData.PathGeometry.PathFigure[0].IsFilled)
          path.Fill = (string) null;
      }
    }
    if (path.RenderTransform != null)
    {
      string str = path.RenderTransform;
      if (str.Contains("StaticResource"))
        str = (this.ReadStaticResource(str) as MatrixTransform).Matrix;
      this.ApplyRenderTransform(str);
    }
    if (path.StrokeDashArray != null)
    {
      string[] val;
      new XPSPathDataReader(path.StrokeDashArray).TryReadPositionArray(out val);
      numArray1 = new float[val.Length];
      for (int index = 0; index < val.Length; ++index)
        numArray1[index] = this.ConvertToPoints((double) XPSRenderer.ParseFloat(val[index]));
    }
    if (path.Stroke != null)
    {
      if (path.Stroke.Contains("StaticResource"))
        path.PathStroke = this.ReadStaticResource(path.Stroke) as Brush;
      else if (path.StrokeThickness != 0.0)
      {
        pen1 = new PdfPen(new PdfColor(this.FromHtml(path.Stroke)), this.ConvertToPoints(path.StrokeThickness));
        pen1.LineJoin = path.StrokeLineJoin == LineJoin.Bevel ? PdfLineJoin.Bevel : (path.StrokeLineJoin == LineJoin.Miter ? PdfLineJoin.Miter : PdfLineJoin.Round);
        if (numArray1 != null)
        {
          pen1.DashStyle = PdfDashStyle.Dash;
          pen1.DashPattern = numArray1;
        }
      }
    }
    PointF pointF;
    if (path.PathStroke != null)
    {
      if (path.PathStroke.Item is LinearGradientBrush)
        pen1 = new PdfPen((PdfBrush) this.ReadLinearGradientBrush(path.PathStroke.Item as LinearGradientBrush), (float) path.StrokeThickness);
      else if (path.PathStroke.Item is ImageBrush)
      {
        ImageBrush imageBrush = path.PathStroke.Item as ImageBrush;
        if (!string.IsNullOrEmpty(imageBrush.Transform))
          this.ApplyRenderTransform(imageBrush.Transform);
        try
        {
          image1 = Image.FromStream(this.m_reader.ReadImage(imageBrush.ImageSource));
          SizeF size2 = new SizeF(this.ConvertToPoints((double) image1.Size.Width), this.ConvertToPoints((double) image1.Size.Height));
          if (path1 != null)
          {
            PointF point = path1.Points[1];
            double x1 = (double) point.X;
            point = path1.Points[0];
            double x2 = (double) point.X;
            double width = x1 - x2;
            point = path1.Points[2];
            double y1 = (double) point.Y;
            point = path1.Points[0];
            double y2 = (double) point.Y;
            double height = y1 - y2;
            size1 = new SizeF((float) width, (float) height);
          }
          PdfTilingBrush pdfTilingBrush = new PdfTilingBrush(size2);
          if (imageBrush.TileMode != TileMode.None)
          {
            RectangleF rectangleF = RectangleF.Empty;
            RectangleF rectangle = RectangleF.Empty;
            PdfImage image2 = PdfImage.FromImage(image1);
            if (!string.IsNullOrEmpty(imageBrush.Viewport))
              rectangleF = this.StringToRectangleF(imageBrush.Viewport);
            if (!string.IsNullOrEmpty(imageBrush.Viewbox))
              rectangle = this.StringToRectangleF(imageBrush.Viewbox);
            if ((double) rectangleF.Width > 0.75 && (double) rectangleF.Height > 0.75)
            {
              PdfTilingBrush brush2 = new PdfTilingBrush(rectangle, this.m_page);
              brush2.Graphics.ScaleTransform(rectangleF.Width / rectangle.Width, rectangleF.Height / rectangle.Height);
              rectangleF.X -= rectangle.X;
              rectangleF.Y -= rectangle.Y;
              rectangleF.Width = (float) image2.Width;
              rectangleF.Height = (float) image2.Height;
              brush2.Graphics.DrawImage(image2, rectangle);
              PdfPen pen2 = new PdfPen((PdfBrush) brush2, (float) path.StrokeThickness);
              Graphics.DrawRectangle(pen2, new RectangleF(path1.Points[0], size1));
            }
            else
              Graphics.DrawImage(image2, new RectangleF(path1.Points[0], size1));
            image1 = (Image) null;
          }
        }
        catch (Exception ex)
        {
        }
      }
      else if (path.PathStroke.Item is SolidColorBrush)
        pen1 = new PdfPen(this.GetSolidBrush((path.PathStroke.Item as SolidColorBrush).Color), (float) path.StrokeThickness);
      else if (path.PathStroke.Item is RadialGradientBrush)
        pen1 = new PdfPen((PdfBrush) this.ReadRadialGradientBrush(path.PathStroke.Item as RadialGradientBrush), (float) path.StrokeThickness);
      else if (path.PathStroke.Item is VisualBrush)
      {
        VisualBrush xpsVisualBrush = path.PathStroke.Item as VisualBrush;
        if (xpsVisualBrush.VisualBrushVisual.Item is Glyphs)
        {
          Glyphs glyphs = xpsVisualBrush.VisualBrushVisual.Item as Glyphs;
          ref SizeF local = ref size1;
          pointF = path1.Points[1];
          double x3 = (double) pointF.X;
          pointF = path1.Points[0];
          double x4 = (double) pointF.X;
          double width = x3 - x4;
          pointF = path1.Points[2];
          double y3 = (double) pointF.Y;
          pointF = path1.Points[0];
          double y4 = (double) pointF.Y;
          double height = y3 - y4;
          local = new SizeF((float) width, (float) height);
          RectangleF rectangleF1 = this.StringToRectangleF(xpsVisualBrush.Viewport);
          RectangleF rectangleF2 = this.StringToRectangleF(xpsVisualBrush.Viewbox);
          PdfFont font1 = this.GetFont(glyphs);
          PdfTilingBrush brush3 = new PdfTilingBrush(new SizeF(rectangleF1.Width, rectangleF1.Height));
          PdfBrush solidBrush = this.GetSolidBrush(glyphs.Fill);
          brush3.Graphics.ScaleTransform(rectangleF1.Width / rectangleF2.Width, rectangleF1.Height / rectangleF2.Height);
          PdfGraphics graphics = brush3.Graphics;
          string unicodeString = glyphs.UnicodeString;
          PdfFont font2 = font1;
          PdfBrush brush4 = solidBrush;
          pointF = new PointF();
          PointF point = pointF;
          graphics.DrawString(unicodeString, font2, brush4, point);
          PdfPen pen3 = new PdfPen((PdfBrush) brush3, (float) path.StrokeThickness);
          Graphics.DrawRectangle(pen3, new RectangleF(path1.Points[0], size1));
        }
        else
          this.ReadVisualBrush(xpsVisualBrush, Graphics);
      }
    }
    if (path.Fill != null)
    {
      if (path.Fill.Contains("StaticResource"))
        path.PathFill = this.ReadStaticResource(path.Fill) as Brush;
      else if (path.Fill.Contains("icc"))
      {
        string[] strArray = Regex.Split(Regex.Split(path.Fill, ".icc")[1], ",");
        float[] numArray2 = new float[strArray.Length];
        if (numArray2.Length == 5)
        {
          this.m_page.Graphics.Save();
          this.m_page.Graphics.ColorSpace = PdfColorSpace.CMYK;
          for (int index = 0; index < strArray.Length; ++index)
            numArray2[index] = Convert.ToSingle(strArray[index]);
          brush1 = (PdfBrush) new PdfSolidBrush(new PdfColor(numArray2[1], numArray2[2], numArray2[3], numArray2[4]));
        }
        else
          brush1 = (PdfBrush) null;
      }
      else
      {
        Color color = this.FromHtml(path.Fill);
        if (path.Fill.Contains("sc#"))
        {
          double alpha = color.A < byte.MaxValue ? (double) color.A / 256.0 : 1.0;
          if (alpha < 1.0)
            Graphics.SetTransparency((float) alpha);
        }
        if (color.A != (byte) 0)
          brush1 = (PdfBrush) new PdfSolidBrush((PdfColor) color);
        if (color.A < byte.MaxValue && color.A > (byte) 0)
        {
          float alpha = (float) color.A / (float) byte.MaxValue;
          Graphics.SetTransparency(alpha);
        }
      }
    }
    SizeF physicalDimension;
    if (path.PathOpacityMask != null && path.Fill == null && path.PathOpacityMask.Item is ImageBrush)
    {
      pdfImageMask = new PdfImageMask(new PdfBitmap((Image) PdfBitmap.CreateMaskFromARGBImage(Image.FromStream(this.m_reader.ReadImage((path.PathOpacityMask.Item as ImageBrush).ImageSource)))));
      ref SizeF local = ref empty1;
      double width = (double) pdfImageMask.Mask.PhysicalDimension.Width;
      physicalDimension = pdfImageMask.Mask.PhysicalDimension;
      double height = (double) physicalDimension.Height;
      local = new SizeF((float) width, (float) height);
    }
    if (path.PathFill != null)
    {
      if (path.PathFill.Item is ImageBrush)
      {
        PdfGraphicsState state2 = Graphics.Save();
        ImageBrush imageBrush = path.PathFill.Item as ImageBrush;
        if (!string.IsNullOrEmpty(imageBrush.Transform))
          this.ApplyRenderTransform(imageBrush.Transform);
        try
        {
          image1 = Image.FromStream(this.m_reader.ReadImage(imageBrush.ImageSource));
          SizeF size3 = new SizeF(this.ConvertToPoints((double) image1.Size.Width), this.ConvertToPoints((double) image1.Size.Height));
          if (path1 != null)
          {
            pointF = path1.Points[1];
            double x5 = (double) pointF.X;
            pointF = path1.Points[0];
            double x6 = (double) pointF.X;
            double width = x5 - x6;
            pointF = path1.Points[2];
            double y5 = (double) pointF.Y;
            pointF = path1.Points[0];
            double y6 = (double) pointF.Y;
            double height = y5 - y6;
            size1 = new SizeF((float) width, (float) height);
          }
          PdfTilingBrush pdfTilingBrush = new PdfTilingBrush(size3);
          RectangleF rectangle = RectangleF.Empty;
          RectangleF rect = RectangleF.Empty;
          if (!string.IsNullOrEmpty(imageBrush.Viewport))
            rectangle = this.StringToRectangleF(imageBrush.Viewport);
          if (!string.IsNullOrEmpty(imageBrush.Viewbox))
          {
            rect = this.StringToRectangleF(imageBrush.Viewbox);
            int height1 = (int) rect.Height;
            Size size4 = image1.Size;
            int height2 = size4.Height;
            if (height1 != height2 || (double) rect.Y != 0.0)
              flag1 = true;
            int width1 = (int) rect.Width;
            size4 = image1.Size;
            int width2 = size4.Width;
            if (width1 != width2 || (double) rect.X != 0.0)
              flag1 = true;
          }
          if (imageBrush.TileMode != TileMode.None)
          {
            ++this.count;
            PdfBitmap image3 = PdfImage.FromImage(image1) as PdfBitmap;
            if (pdfImageMask != null)
            {
              SizeF sizeF1 = empty1;
              physicalDimension = image3.PhysicalDimension;
              double width = (double) physicalDimension.Width;
              physicalDimension = image3.PhysicalDimension;
              double height = (double) physicalDimension.Height;
              SizeF sizeF2 = new SizeF((float) width, (float) height);
              if (sizeF1 == sizeF2)
                image3.Mask = (PdfMask) pdfImageMask;
            }
            if ((double) Graphics.Matrix.Matrix.Elements[0] < 1.0)
              rectangle.Size = new SizeF(rectangle.Width * Graphics.Matrix.Matrix.Elements[0], rectangle.Height * Graphics.Matrix.Matrix.Elements[3]);
            if ((double) rectangle.Width > 0.75 && (double) rectangle.Height > 0.75)
            {
              PdfTilingBrush brush5;
              if (imageBrush.ImageBrushTransform != null)
              {
                float[] numArray3 = new float[6];
                string[] strArray = imageBrush.ImageBrushTransform.MatrixTransform.Matrix.Split(this.m_commaSeparator);
                for (int index = 0; index < numArray3.Length; ++index)
                  numArray3[index] = XPSRenderer.ParseFloat(strArray[index]);
                if ((double) numArray3[0] > 0.0 && (double) numArray3[3] > 0.0)
                {
                  rectangle.Size = new SizeF(rectangle.Width * Graphics.Matrix.Matrix.Elements[0], rectangle.Height * Graphics.Matrix.Matrix.Elements[3]);
                  rect.Size = new SizeF(rect.Width * Graphics.Matrix.Matrix.Elements[0], rect.Height * Graphics.Matrix.Matrix.Elements[3]);
                  brush5 = new PdfTilingBrush(rectangle, this.m_page);
                  Matrix matrix = new Matrix(numArray3[0], numArray3[1], numArray3[2], numArray3[3], numArray3[4], numArray3[5]);
                  brush5.TransformationMatrix = this.PrepareMatrix(matrix);
                }
                else
                  brush5 = new PdfTilingBrush(rectangle, this.m_page);
              }
              else
                brush5 = new PdfTilingBrush(rectangle, this.m_page);
              brush5.Graphics.ScaleTransform(rectangle.Width / rect.Width, rectangle.Height / rect.Height);
              rectangle.X -= rect.X;
              rectangle.Y -= rect.Y;
              rectangle.Width = (float) image3.Width;
              rectangle.Height = (float) image3.Height;
              brush5.Graphics.DrawImage((PdfImage) image3, rectangle);
              Graphics.DrawRectangle((PdfBrush) brush5, new RectangleF(path1.Points[0], size1));
            }
            else
              Graphics.DrawImage((PdfImage) image3, new RectangleF(path1.Points[0], size1));
            image1 = (Image) null;
          }
          else if (imageBrush.TileMode == TileMode.None)
          {
            if (imageBrush.Viewbox != null)
            {
              if (imageBrush.Viewport != null)
              {
                if ((double) size1.Width == 0.0 || (double) size1.Height == 0.0)
                  size1 = new SizeF(rectangle.Width, rectangle.Height);
                if ((double) PdfUnitConvertor.HorizontalResolution == 144.0)
                {
                  if (!flag1)
                    goto label_114;
                }
                if ((double) image1.HorizontalResolution != 96.0)
                {
                  PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor(image1.HorizontalResolution);
                  int num = 3;
                  RectangleF pixels = pdfUnitConvertor.ConvertToPixels(rect, PdfGraphicsUnit.Point);
                  bool flag2 = (double) pixels.Width >= (double) (image1.Width - num) && (double) pixels.Width <= (double) (image1.Width + num);
                  bool flag3 = (double) pixels.Height >= (double) (image1.Height - num) && (double) pixels.Height <= (double) (image1.Height + num);
                  bool flag4 = (double) rect.Y != 0.0 || (double) rect.X != 0.0;
                  if (flag2 && flag3)
                  {
                    if (!flag4)
                      goto label_114;
                  }
                  MemoryStream memoryStream = new MemoryStream();
                  Bitmap bitmap = this.clipXpsImage(image1 as Bitmap, new Rectangle((int) pixels.X, (int) pixels.Y, (int) pixels.Width, (int) pixels.Height));
                  pdfImageMask = this.CheckAlpha(bitmap);
                  if (PdfImage.FromImage((Image) this.CompressImage(bitmap)) is PdfBitmap image4)
                  {
                    image4.Mask = (PdfMask) pdfImageMask;
                    Graphics.DrawImage((PdfImage) image4, rectangle);
                    image1 = (Image) null;
                  }
                  memoryStream.Dispose();
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          if (ex is OutOfMemoryException)
            throw new OutOfMemoryException(ex.Message);
        }
label_114:
        Graphics.Restore(state2);
      }
      else if (path.PathFill.Item is SolidColorBrush)
        brush1 = (PdfBrush) new PdfSolidBrush((PdfColor) ColorTranslator.FromHtml((path.PathFill.Item as SolidColorBrush).Color));
      else if (path.PathFill.Item is LinearGradientBrush)
        brush1 = (PdfBrush) this.ReadLinearGradientBrush(path.PathFill.Item as LinearGradientBrush);
      else if (path.PathFill.Item is RadialGradientBrush)
        brush1 = (PdfBrush) this.ReadRadialGradientBrush(path.PathFill.Item as RadialGradientBrush);
      else if (path.PathFill.Item is VisualBrush)
      {
        VisualBrush xpsVisualBrush = path.PathFill.Item as VisualBrush;
        this.v_graphicsState = Graphics.Save();
        RectangleF rectangleF3 = RectangleF.Empty;
        RectangleF rectangleF4 = RectangleF.Empty;
        if (!string.IsNullOrEmpty(xpsVisualBrush.Viewport))
          rectangleF3 = this.StringToRectangleF(xpsVisualBrush.Viewport);
        if (!string.IsNullOrEmpty(xpsVisualBrush.Viewbox))
          rectangleF4 = this.StringToRectangleF(xpsVisualBrush.Viewbox);
        this.v_viewport = rectangleF3;
        if (xpsVisualBrush.TileMode == TileMode.None && xpsVisualBrush.Viewbox != null && xpsVisualBrush.Viewport != null)
        {
          Graphics.ScaleTransform(rectangleF3.Width / rectangleF4.Width, rectangleF3.Height / rectangleF4.Height);
          rectangleF3.X -= rectangleF4.X;
          rectangleF3.Y -= rectangleF4.Y;
          Graphics.TranslateTransform(rectangleF3.X, rectangleF3.Y);
          this.ReadVisualBrush(xpsVisualBrush, Graphics);
          Graphics.Restore(this.v_graphicsState);
        }
        else
        {
          PdfTilingBrush brush6 = new PdfTilingBrush(new SizeF(rectangleF3.Width, rectangleF3.Height));
          brush6.Graphics.ScaleTransform(rectangleF3.Width / rectangleF4.Width, rectangleF3.Height / rectangleF4.Height);
          this.ReadVisualBrush(xpsVisualBrush, brush6.Graphics);
          if (xpsVisualBrush.Transform != null)
          {
            brush6.isXPSBrush = true;
            brush6.TransformationMatrix = this.GetTransformationMatrix(xpsVisualBrush.Transform);
          }
          if (Graphics == this.v_graphicsState.Graphics)
            Graphics.Restore(this.v_graphicsState);
          Graphics.DrawPath((PdfBrush) brush6, path1);
        }
      }
    }
    if (path.Opacity < 1.0)
      Graphics.SetTransparency((float) path.Opacity);
    if (path.Clip != null)
    {
      PdfPath pathFromGeometry = this.GetPathFromGeometry(path.Clip);
      Graphics.SetClip(pathFromGeometry);
    }
    int strokeLineJoin = (int) path.StrokeLineJoin;
    if (pen1 != null)
      pen1.LineJoin = (PdfLineJoin) Enum.Parse(typeof (PdfLineJoin), Enum.GetName(typeof (LineJoin), (object) path.StrokeLineJoin));
    int strokeStartLineCap = (int) path.StrokeStartLineCap;
    if (pen1 != null && path.StrokeStartLineCap != LineCap.Triangle)
      pen1.LineCap = (PdfLineCap) Enum.Parse(typeof (PdfLineCap), Enum.GetName(typeof (LineCap), (object) path.StrokeStartLineCap));
    if (path.FixedPageNavigateUri != null && this.currentMatrix != null)
    {
      PdfUriAnnotation annotation;
      if (path1 != null)
      {
        ref SizeF local = ref size1;
        pointF = path1.Points[2];
        double x7 = (double) pointF.X;
        pointF = path1.Points[0];
        double x8 = (double) pointF.X;
        double width = (x7 - x8) * (double) this.currentMatrix.Matrix.Elements[0];
        pointF = path1.Points[2];
        double y7 = (double) pointF.Y;
        pointF = path1.Points[0];
        double y8 = (double) pointF.Y;
        double height = (y7 - y8) * (double) this.currentMatrix.Matrix.Elements[3];
        local = new SizeF((float) width, (float) height);
        pointF = path1.Points[0];
        double x9 = (double) pointF.X * (double) this.currentMatrix.Matrix.Elements[0];
        pointF = path1.Points[0];
        double y9 = (double) pointF.Y * (double) this.currentMatrix.Matrix.Elements[3];
        annotation = new PdfUriAnnotation(new RectangleF(new PointF((float) x9, (float) y9), size1));
      }
      else
        annotation = new PdfUriAnnotation(new RectangleF());
      annotation.Uri = path.FixedPageNavigateUri;
      annotation.Color = new PdfColor(Color.Transparent);
      this.m_page.Annotations.Add((PdfAnnotation) annotation);
    }
    else if (path.PathFill != null && path.PathFill.Item is ImageBrush && image1 != null && path1.PointCount > 0)
    {
      ImageBrush imageBrush = path.PathFill.Item as ImageBrush;
      float num = 0.0f;
      if (imageBrush.ImageBrushTransform != null)
        num = XPSRenderer.ParseFloat(imageBrush.ImageBrushTransform.MatrixTransform.Matrix.Split(this.m_commaSeparator)[3]);
      PdfBitmap pdfBitmap;
      if (this.CompareImage(image1))
      {
        pdfBitmap = this.Document.ImageCollection[this.m_hashValue] as PdfBitmap;
        this.Graphics.isImageOptimized = true;
      }
      else
      {
        pdfBitmap = PdfImage.FromImage(image1) as PdfBitmap;
        this.Document.ImageCollection.Add(this.m_hashValue, (PdfImage) pdfBitmap);
      }
      if (pdfImageMask != null)
      {
        SizeF sizeF3 = empty1;
        physicalDimension = pdfBitmap.PhysicalDimension;
        double width = (double) physicalDimension.Width;
        physicalDimension = pdfBitmap.PhysicalDimension;
        double height = (double) physicalDimension.Height;
        SizeF sizeF4 = new SizeF((float) width, (float) height);
        if (sizeF3 == sizeF4)
          pdfBitmap.Mask = (PdfMask) pdfImageMask;
      }
      if ((double) size1.Height < 0.0 && (double) num >= 0.0)
      {
        size1.Height = -size1.Height;
        PdfGraphics pdfGraphics = Graphics;
        PdfBitmap image5 = pdfBitmap;
        pointF = path1.Points[0];
        double x = (double) pointF.X;
        pointF = path1.Points[0];
        double y = (double) pointF.Y - (double) size1.Height;
        RectangleF rectangle = new RectangleF(new PointF((float) x, (float) y), size1);
        pdfGraphics.DrawImage((PdfImage) image5, rectangle);
      }
      else
      {
        PdfGraphics pdfGraphics = Graphics;
        PdfBitmap image6 = pdfBitmap;
        pointF = path1.Points[0];
        double x = (double) pointF.X;
        pointF = path1.Points[0];
        double y = (double) pointF.Y;
        RectangleF rectangle = new RectangleF(new PointF((float) x, (float) y), size1);
        pdfGraphics.DrawImage((PdfImage) image6, rectangle);
      }
      if (pen1 != null)
        Graphics.DrawPath(pen1, path1);
    }
    else if (path.PathOpacityMask != null && path.Fill != null && path.Data == null)
    {
      if (path.PathOpacityMask.Item is ImageBrush)
      {
        PdfGraphicsState state3 = Graphics.Save();
        ImageBrush imageBrush = path.PathOpacityMask.Item as ImageBrush;
        if (!string.IsNullOrEmpty(imageBrush.Transform))
          this.ApplyRenderTransform(imageBrush.Transform);
        Image original = Image.FromStream(this.m_reader.ReadImage(imageBrush.ImageSource));
        Size size5 = original.Size;
        double points1 = (double) this.ConvertToPoints((double) size5.Width);
        size5 = original.Size;
        double points2 = (double) this.ConvertToPoints((double) size5.Height);
        SizeF sizeF = new SizeF((float) points1, (float) points2);
        RectangleF rectangle = RectangleF.Empty;
        RectangleF empty2 = RectangleF.Empty;
        if (!string.IsNullOrEmpty(imageBrush.Viewport))
          rectangle = this.StringToRectangleF(imageBrush.Viewport);
        if (!string.IsNullOrEmpty(imageBrush.Viewbox))
          this.StringToRectangleF(imageBrush.Viewbox);
        if ((double) Graphics.Matrix.Matrix.Elements[0] < 1.0)
          rectangle.Size = new SizeF(rectangle.Width * Graphics.Matrix.Matrix.Elements[0], rectangle.Height * Graphics.Matrix.Matrix.Elements[3]);
        if (imageBrush.TileMode != TileMode.None)
        {
          Bitmap bitmap = this.setFillColor(new Bitmap(original), this.FromHtml(path.Fill));
          MemoryStream memoryStream = new MemoryStream();
          bitmap.Save((Stream) memoryStream, ImageFormat.Png);
          PdfBitmap image7 = PdfImage.FromImage((Image) (Image.FromStream((Stream) memoryStream) as Bitmap)) as PdfBitmap;
          Graphics.DrawImage((PdfImage) image7, rectangle);
          memoryStream.Dispose();
        }
        else if (path.PathOpacityMask.Item is LinearGradientBrush)
        {
          PdfBrush pdfBrush = (PdfBrush) this.ReadLinearGradientBrush(path.PathOpacityMask.Item as LinearGradientBrush);
        }
        Graphics.Restore(state3);
      }
    }
    else
    {
      if (brush1 is PdfSolidBrush)
      {
        PdfSolidBrush pdfSolidBrush = brush1 as PdfSolidBrush;
        if (pdfSolidBrush.Color.A < byte.MaxValue)
        {
          PdfColor color = pdfSolidBrush.Color;
          if (color.A > (byte) 0)
          {
            color = pdfSolidBrush.Color;
            float alpha = (float) color.A / (float) byte.MaxValue;
            Graphics.SetTransparency(alpha);
          }
        }
      }
      bool flag5 = false;
      if (path1.PointCount == 4 && path1.Points[0] == path1.Points[3] && path1.Points[1] == path1.Points[2])
        flag5 = true;
      if (flag5 && this.v_graphicsState != null)
      {
        Graphics.Restore(this.v_graphicsState);
        Graphics.DrawLine(pen1, this.v_viewport.X, this.v_viewport.Height, this.v_viewport.Width, this.v_viewport.Height);
      }
      else if (path.PathFill == null || !(path.PathFill.Item is VisualBrush))
        Graphics.DrawPath(pen1, brush1, path1);
    }
    Graphics.Restore(state1);
  }

  private bool CompareImage(Image img)
  {
    this.m_hashValue = this.Document.GetImageHash(img);
    return this.Document.ImageCollection.ContainsKey(this.m_hashValue);
  }

  private Bitmap setFillColor(Bitmap bitmap, Color color)
  {
    Bitmap bitmap1 = new Bitmap(bitmap.Width, bitmap.Height);
    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) bitmap1))
    {
      ColorMatrix newColorMatrix = new ColorMatrix(new float[5][]
      {
        new float[5],
        new float[5],
        new float[5],
        new float[5]{ 0.0f, 0.0f, 0.0f, 1f, 0.0f },
        new float[5]
        {
          (float) color.R / (float) byte.MaxValue,
          (float) color.G / (float) byte.MaxValue,
          (float) color.B / (float) byte.MaxValue,
          0.0f,
          1f
        }
      });
      ImageAttributes imageAttr = new ImageAttributes();
      imageAttr.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
      graphics.SmoothingMode = SmoothingMode.Default;
      graphics.DrawImage((Image) bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, imageAttr);
    }
    return bitmap1;
  }

  private void ReadVisualBrush(VisualBrush xpsVisualBrush, PdfGraphics Graphics)
  {
    if (xpsVisualBrush.Key != null && xpsVisualBrush.Key == "EmptyBrush")
      return;
    if (xpsVisualBrush.Transform != null)
      this.ApplyRenderTransform(xpsVisualBrush.Transform);
    object obj = (object) null;
    if (xpsVisualBrush.VisualBrushVisual != null)
      obj = xpsVisualBrush.VisualBrushVisual.Item;
    else if (!string.IsNullOrEmpty(xpsVisualBrush.Visual) && xpsVisualBrush.Visual.Contains("StaticResource"))
      obj = this.ReadStaticResource(xpsVisualBrush.Visual);
    switch (obj)
    {
      case Canvas _:
        this.DrawCanvas(obj as Canvas, Graphics);
        break;
      case Glyphs _:
        this.DrawGlyphs(obj as Glyphs, Graphics);
        break;
      case Path _:
        this.DrawPath(obj as Path, Graphics);
        break;
    }
  }

  private PdfLinearGradientBrush ReadLinearGradientBrush(LinearGradientBrush gradientBrush)
  {
    PointF val;
    new XPSPathDataReader(gradientBrush.StartPoint).TryReadPoint(out val);
    PointF point1 = new PointF(this.ConvertToPoints((double) val.X), this.ConvertToPoints((double) val.Y));
    new XPSPathDataReader(gradientBrush.EndPoint).TryReadPoint(out val);
    PointF point2 = new PointF(this.ConvertToPoints((double) val.X), this.ConvertToPoints((double) val.Y));
    List<object> offSet;
    List<PdfColor> colors;
    this.PreProcessGradientStops(gradientBrush.LinearGradientBrushGradientStops, out offSet, out colors);
    float[] numArray = new float[offSet.Count];
    int index = 0;
    foreach (object obj in offSet)
    {
      numArray[index] = XPSRenderer.ParseFloat(obj.ToString());
      ++index;
    }
    PdfLinearGradientBrush brush = new PdfLinearGradientBrush(point1, point2, colors[0], colors[colors.Count - 1]);
    if (gradientBrush.SpreadMethod == SpreadMethod.Pad)
      brush.Extend = PdfExtend.Both;
    else if (gradientBrush.SpreadMethod == SpreadMethod.Repeat)
      brush.Extend = PdfExtend.None;
    else if (gradientBrush.SpreadMethod == SpreadMethod.Reflect)
      brush.Extend = PdfExtend.None;
    PdfColorBlend pdfColorBlend = new PdfColorBlend((PdfBrush) brush);
    pdfColorBlend.Positions = numArray;
    pdfColorBlend.Colors = colors.ToArray();
    brush.InterpolationColors = pdfColorBlend;
    return brush;
  }

  private PdfRadialGradientBrush ReadRadialGradientBrush(RadialGradientBrush xpsRadialBrush)
  {
    float radiusX = (float) xpsRadialBrush.RadiusX;
    float radiusY = (float) xpsRadialBrush.RadiusY;
    PointF val;
    new XPSPathDataReader(xpsRadialBrush.GradientOrigin).TryReadPoint(out val);
    PointF centreEnd = new PointF(this.ConvertToPoints((double) val.X), this.ConvertToPoints((double) val.Y));
    new XPSPathDataReader(xpsRadialBrush.Center).TryReadPoint(out val);
    PointF centreStart = new PointF(this.ConvertToPoints((double) val.X), this.ConvertToPoints((double) val.Y));
    List<object> offSet;
    List<PdfColor> colors;
    this.PreProcessGradientStops(xpsRadialBrush.RadialGradientBrushGradientStops, out offSet, out colors);
    float[] numArray = new float[offSet.Count];
    int index = 0;
    foreach (object obj in offSet)
    {
      numArray[index] = XPSRenderer.ParseFloat(obj.ToString());
      ++index;
    }
    PdfRadialGradientBrush brush = new PdfRadialGradientBrush(centreStart, radiusX, centreEnd, radiusY, colors[0], colors[colors.Count - 1]);
    if (xpsRadialBrush.SpreadMethod == SpreadMethod.Pad)
      brush.Extend = PdfExtend.Both;
    else if (xpsRadialBrush.SpreadMethod == SpreadMethod.Repeat)
      brush.Extend = PdfExtend.None;
    else if (xpsRadialBrush.SpreadMethod == SpreadMethod.Reflect)
      brush.Extend = PdfExtend.None;
    PdfColorBlend pdfColorBlend = new PdfColorBlend((PdfBrush) brush);
    pdfColorBlend.Positions = numArray;
    pdfColorBlend.Colors = colors.ToArray();
    brush.InterpolationColors = pdfColorBlend;
    return brush;
  }

  private PdfPath GetPathFromPathGeometry(PathGeometry xpsPathGeometry)
  {
    PdfPath fromPathGeometry = (PdfPath) null;
    if (xpsPathGeometry == null)
      return fromPathGeometry;
    if (xpsPathGeometry.Transform != null)
      this.ApplyRenderTransform(xpsPathGeometry.Transform);
    if (xpsPathGeometry.Figures != null)
      fromPathGeometry = this.GetPathFromGeometry(xpsPathGeometry.Figures);
    else if (xpsPathGeometry.PathFigure != null)
    {
      PathFigure[] pathFigure1 = xpsPathGeometry.PathFigure;
      GraphicsPath graphicsPath = new GraphicsPath();
      for (int index = 0; index < pathFigure1.Length; ++index)
      {
        PathFigure pathFigure2 = pathFigure1[index];
        PointF pt1 = this.StringToPointF(pathFigure2.StartPoint);
        PointF pointF1 = pt1;
        foreach (object obj in pathFigure2.Items)
        {
          switch (obj)
          {
            case PolyLineSegment _:
              string points = (obj as PolyLineSegment).Points;
              char[] chArray = new char[1]{ ' ' };
              foreach (string point in points.Split(chArray))
              {
                PointF pointF2 = this.StringToPointF(point);
                graphicsPath.AddLine(pointF1, pointF2);
                pointF1 = pointF2;
              }
              break;
            case ArcSegment _:
              ArcSegment arcSegment = obj as ArcSegment;
              PointF pointF3 = this.StringToPointF(arcSegment.Point);
              PointF pointF4 = this.StringToPointF(arcSegment.Size);
              bool isCounterClockwise = false;
              if (arcSegment.SweepDirection == SweepDirection.Clockwise)
                isCounterClockwise = false;
              else if (arcSegment.SweepDirection == SweepDirection.Counterclockwise)
                isCounterClockwise = true;
              List<PointF> arc = this.ComputeArc(pointF1, pointF3, pointF4.X, pointF4.Y, arcSegment.RotationAngle, arcSegment.IsLargeArc, isCounterClockwise);
              if (arc.Count == 0)
              {
                if (arcSegment.IsStroked)
                  graphicsPath.AddLine(pt1, pointF3);
                pt1 = pointF3;
                break;
              }
              if (arc.Count > 0)
              {
                if (arcSegment.IsStroked)
                  graphicsPath.AddBeziers(arc.ToArray());
                pointF1 = arc[arc.Count - 1];
                arc.Clear();
                break;
              }
              break;
            case PolyBezierSegment _:
              PolyBezierSegment polyBezierSegment = obj as PolyBezierSegment;
              List<PointF> pointFList = new List<PointF>();
              string[] strArray = polyBezierSegment.Points.Split(' ');
              if (pointF1 != PointF.Empty)
                pointFList.Add(pointF1);
              else
                pointFList.Add(pt1);
              foreach (string point in strArray)
                pointFList.Add(this.StringToPointF(point));
              if (polyBezierSegment.IsStroked)
                graphicsPath.AddBeziers(pointFList.ToArray());
              pointF1 = pointFList[pointFList.Count - 1];
              pointFList.Clear();
              break;
          }
        }
        if (pathFigure2.IsClosed)
        {
          graphicsPath.CloseFigure();
          graphicsPath.StartFigure();
          PointF empty = PointF.Empty;
        }
      }
      if (graphicsPath.PointCount > 0)
      {
        fromPathGeometry = new PdfPath(graphicsPath.PathPoints, graphicsPath.PathTypes);
        fromPathGeometry.FillMode = graphicsPath.FillMode == FillMode.Alternate ? PdfFillMode.Alternate : PdfFillMode.Winding;
        fromPathGeometry.CloseAllFigures();
      }
      else if (fromPathGeometry == null)
        fromPathGeometry = new PdfPath();
    }
    return fromPathGeometry;
  }

  private PointF StringToPointF(string point)
  {
    string[] strArray = point.Split(',');
    return this.ConvertToPoints(new PointF(XPSRenderer.ParseFloat(strArray[0]), XPSRenderer.ParseFloat(strArray[1])));
  }

  private RectangleF StringToRectangleF(string rect)
  {
    string[] strArray = rect.Split(',');
    PointF point1 = new PointF(XPSRenderer.ParseFloat(strArray[0]), XPSRenderer.ParseFloat(strArray[1]));
    PointF point2 = new PointF(XPSRenderer.ParseFloat(strArray[2]), XPSRenderer.ParseFloat(strArray[3]));
    PointF points1 = this.ConvertToPoints(point1);
    PointF points2 = this.ConvertToPoints(point2);
    SizeF size = new SizeF(points2.X, points2.Y);
    return new RectangleF(points1, size);
  }

  private void PreProcessGradientStops(
    GradientStop[] stops,
    out List<object> offSet,
    out List<PdfColor> colors)
  {
    offSet = new List<object>();
    colors = new List<PdfColor>();
    PdfColor empty = PdfColor.Empty;
    foreach (GradientStop stop in stops)
    {
      float offset = (float) stop.Offset;
      PdfColor pdfColor = new PdfColor(this.FromHtml(stop.Color));
      int num1 = offSet.IndexOf((object) offset);
      int num2 = offSet.LastIndexOf((object) offset);
      if (num1 == -1 && num1 == num2)
      {
        offSet.Add((object) offset);
        offSet.Sort();
        int index = offSet.IndexOf((object) offset);
        colors.Insert(index, pdfColor);
      }
      else if (num1 > -1 && num1 == num2)
      {
        offSet.Add((object) offset);
        offSet.Sort();
        int index = offSet.LastIndexOf((object) offset);
        colors.Insert(index, pdfColor);
      }
      else if (num1 > -1 && num1 != num2)
      {
        int index = offSet.LastIndexOf((object) offset);
        offSet.RemoveAt(index);
        colors.RemoveAt(index);
        offSet.Insert(index, (object) offset);
        colors.Insert(index, pdfColor);
      }
    }
    if (!offSet.Contains((object) 0.0f))
    {
      offSet.FindLast(new Predicate<object>(XPSRenderer.LowerLeastGradient));
      offSet.Find(new Predicate<object>(XPSRenderer.LowerMostGradient));
      if ((double) (float) offSet[0] > 0.0)
      {
        offSet.Insert(0, (object) 0.0f);
        PdfColor pdfColor = colors[0];
        colors.Insert(0, pdfColor);
      }
      object last1 = offSet.FindLast(new Predicate<object>(XPSRenderer.LowerLeastGradient));
      object obj1 = offSet.Find(new Predicate<object>(XPSRenderer.LowerMostGradient));
      if (last1 != null && obj1 != null)
      {
        PdfColor color1 = colors[offSet.IndexOf(last1)];
        PdfColor color2 = colors[offSet.IndexOf(obj1)];
        PdfColor pdfColor1 = colors[offSet.IndexOf(last1)];
        PdfColor pdfColor2 = PdfBlendBase.Interpolate(0.5, color1, color2, PdfColorSpace.RGB);
        for (; last1 != null; last1 = offSet.Find(new Predicate<object>(XPSRenderer.LowerLeastGradient)))
        {
          int index = offSet.IndexOf(last1);
          offSet.RemoveAt(index);
          colors.RemoveAt(index);
        }
        offSet.Insert(0, (object) 0.0f);
        colors.Insert(0, pdfColor2);
      }
      object last2 = offSet.FindLast(new Predicate<object>(XPSRenderer.LowerLeastGradient));
      object obj2 = offSet.Find(new Predicate<object>(XPSRenderer.LowerMostGradient));
      if (last2 != null && obj2 == null)
      {
        PdfColor pdfColor = colors[colors.Count - 1];
        for (; last2 != null; last2 = offSet.Find(new Predicate<object>(XPSRenderer.LowerLeastGradient)))
        {
          int index = offSet.IndexOf(last2);
          offSet.RemoveAt(index);
          colors.RemoveAt(index);
        }
        offSet.Insert(0, (object) 0.0f);
        colors.Insert(0, pdfColor);
      }
    }
    if (offSet.Contains((object) 1f))
      return;
    offSet.FindLast(new Predicate<object>(XPSRenderer.HigherLeastGradient));
    offSet.Find(new Predicate<object>(XPSRenderer.HigherMostGradient));
    if ((double) (float) offSet[offSet.Count - 1] < 1.0)
    {
      int count = offSet.Count;
      offSet.Add((object) 1f);
      PdfColor pdfColor = colors[count - 1];
      colors.Add(pdfColor);
    }
    object last3 = offSet.FindLast(new Predicate<object>(XPSRenderer.HigherLeastGradient));
    object obj3 = offSet.Find(new Predicate<object>(XPSRenderer.HigherMostGradient));
    if (obj3 != null && last3 != null)
    {
      PdfColor color1 = colors[offSet.IndexOf(last3)];
      PdfColor color2 = colors[offSet.IndexOf(obj3)];
      PdfColor pdfColor3 = colors[offSet.IndexOf(obj3)];
      PdfColor pdfColor4 = PdfBlendBase.Interpolate(0.5, color1, color2, PdfColorSpace.RGB);
      for (; obj3 != null; obj3 = offSet.Find(new Predicate<object>(XPSRenderer.HigherMostGradient)))
      {
        int index = offSet.IndexOf(obj3);
        offSet.RemoveAt(index);
        colors.RemoveAt(index);
      }
      offSet.Add((object) 1f);
      colors.Add(pdfColor4);
    }
    object last4 = offSet.FindLast(new Predicate<object>(XPSRenderer.HigherLeastGradient));
    object obj4 = offSet.Find(new Predicate<object>(XPSRenderer.HigherMostGradient));
    if (obj4 == null || last4 != null)
      return;
    PdfColor pdfColor5 = colors[0];
    for (; obj4 != null; obj4 = offSet.Find(new Predicate<object>(XPSRenderer.HigherMostGradient)))
    {
      int index = offSet.IndexOf(obj4);
      offSet.RemoveAt(index);
      colors.RemoveAt(index);
    }
    offSet.Add((object) 1f);
    colors.Add(pdfColor5);
  }

  private static bool LowerLeastGradient(object p) => (double) (float) p < 0.0;

  private static bool LowerMostGradient(object p) => (double) (float) p > 0.0;

  private static bool HigherLeastGradient(object p) => (double) (float) p < 1.0;

  private static bool HigherMostGradient(object p) => (double) (float) p > 1.0;

  private object ReadStaticResource(string resourceName)
  {
    resourceName = resourceName.Trim('{', '}').Replace("StaticResource ", string.Empty);
    object obj = (object) null;
    int index = this.m_page.Section.Parent.IndexOf(this.m_page.Section);
    object[] collection = (object[]) null;
    if (this.m_canvas != null)
    {
      collection = this.GetResourceCollection(this.m_canvas.CanvasResources);
      obj = this.GetResource(collection, resourceName);
    }
    if (this.m_canvas != null && obj == null)
    {
      Canvas canvas = this.m_canvas;
      while (canvas.m_parent != null)
      {
        canvas = canvas.m_parent as Canvas;
        collection = this.GetResourceCollection(canvas.CanvasResources);
        obj = this.GetResource(collection, resourceName);
        if (obj != null)
          break;
      }
    }
    if (collection == null && this.m_reader.Pages[index].FixedPageResources != null)
      obj = this.GetResource(this.GetResourceCollection(this.m_reader.Pages[index].FixedPageResources), resourceName);
    return obj;
  }

  private object[] GetResourceCollection(Resources resources)
  {
    if (resources != null)
    {
      ResourceDictionary resourceDictionary1 = resources.ResourceDictionary;
      if (resourceDictionary1 != null && resourceDictionary1.Items == null && resourceDictionary1.Source != null && resourceDictionary1.Source != string.Empty)
      {
        ResourceDictionary resourceDictionary2 = (ResourceDictionary) new XmlSerializer(typeof (ResourceDictionary)).Deserialize((TextReader) new StreamReader(this.m_reader.ReadResource(resourceDictionary1.Source)));
        resources.ResourceDictionary = resourceDictionary2;
      }
    }
    return resources == null || resources.ResourceDictionary == null ? (object[]) null : resources.ResourceDictionary.Items;
  }

  private object GetResource(object[] collection, string resourceName)
  {
    object resource1 = (object) null;
    Brush resource2 = new Brush();
    PathGeometry pathGeometry = new PathGeometry();
    if (collection != null && collection.Length > 0)
    {
      for (int index = 0; index < collection.Length; ++index)
      {
        resource1 = collection[index];
        switch (resource1)
        {
          case SolidColorBrush _ when (resource1 as SolidColorBrush).Key == resourceName:
            resource2.Item = resource1;
            return (object) resource2;
          case ImageBrush _ when (resource1 as ImageBrush).Key == resourceName:
            resource2.Item = resource1;
            return (object) resource2;
          case VisualBrush _ when (resource1 as VisualBrush).Key == resourceName:
            resource2.Item = resource1;
            return (object) resource2;
          case LinearGradientBrush _ when (resource1 as LinearGradientBrush).Key == resourceName:
            resource2.Item = resource1;
            return (object) resource2;
          case RadialGradientBrush _ when (resource1 as RadialGradientBrush).Key == resourceName:
            resource2.Item = resource1;
            return (object) resource2;
          case PathGeometry _ when (resource1 as PathGeometry).Key == resourceName:
            return (object) (resource1 as PathGeometry);
          case MatrixTransform _ when (resource1 as MatrixTransform).Key == resourceName:
            return (object) (resource1 as MatrixTransform);
          case VisualBrush _ when (resource1 as VisualBrush).Key == resourceName:
            return (object) (resource1 as VisualBrush);
          default:
            continue;
        }
      }
    }
    return resource1;
  }

  public void DrawCanvas(Canvas canvas, PdfGraphics Grapics)
  {
    PdfGraphicsState state = this.Graphics.Save();
    this.InitializeCanvas(canvas);
    if (this.m_canvas != null)
      canvas.m_parent = (object) this.m_canvas;
    this.m_canvas = canvas;
    if (canvas.Opacity < 1.0)
      this.Graphics.SetTransparency((float) canvas.Opacity);
    if (canvas.Items != null)
    {
      float opacity = (float) canvas.Opacity;
      foreach (object obj in canvas.Items)
      {
        switch (obj)
        {
          case Canvas _:
            this.list.Add(opacity);
            this.DrawCanvas((Canvas) obj, Grapics);
            this.list.Remove(opacity);
            break;
          case Path _:
            float num1 = 1f;
            for (int index = 0; index < this.list.Count; ++index)
              num1 = this.list[index] * num1;
            this.m_canvas.Opacity = (double) num1 * (double) opacity;
            this.Graphics.SetTransparency((float) canvas.Opacity);
            this.DrawPath((Path) obj, Grapics);
            break;
          case Glyphs _:
            float num2 = 1f;
            for (int index = 0; index < this.list.Count; ++index)
              num2 = this.list[index] * num2;
            this.m_canvas.Opacity = (double) num2 * (double) opacity;
            this.Graphics.SetTransparency((float) canvas.Opacity);
            this.DrawGlyphs((Glyphs) obj, Grapics);
            break;
          default:
            throw new NotImplementedException(obj.GetType().ToString());
        }
      }
    }
    this.m_canvas = (Canvas) this.m_canvas.m_parent;
    this.Graphics.Restore(state);
  }

  public void InitializeCanvas(Canvas canvas)
  {
    if (canvas == null)
      throw new ArgumentNullException("Canvas");
    if (canvas.CanvasRenderTransform != null)
      this.ApplyRenderTransform(canvas.CanvasRenderTransform.MatrixTransform.Matrix);
    if (canvas.RenderTransform != null)
      this.ApplyRenderTransform(canvas.RenderTransform);
    if (canvas.Clip == null)
      return;
    this.Graphics.SetClip(this.GetPathFromGeometry(canvas.Clip));
  }

  public void ApplyRenderTransform(string data, PdfGraphics graphics)
  {
    this.Graphics.MultiplyTransform(this.PrepareMatrix(this.ReadMatrix(data)));
  }

  public void ApplyRenderTransform(string data)
  {
    PdfTransformationMatrix transformationMatrix = this.GetTransformationMatrix(data);
    this.Graphics.Matrix.Multiply(transformationMatrix);
    this.Graphics.MultiplyTransform(transformationMatrix);
    this.currentMatrix = transformationMatrix;
  }

  private PdfTransformationMatrix GetTransformationMatrix(string data)
  {
    return this.PrepareMatrix(!data.Contains("StaticResource") ? this.ReadMatrix(data) : this.ReadMatrix((this.ReadStaticResource(data) as MatrixTransform).Matrix));
  }

  private PdfTransformationMatrix PrepareMatrix(Matrix matrix)
  {
    PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
    PdfTransformationMatrix matrix1 = new PdfTransformationMatrix();
    matrix1.Matrix = matrix;
    transformationMatrix.Scale(1f, -1f);
    transformationMatrix.Multiply(matrix1);
    transformationMatrix.Scale(1f, -1f);
    return transformationMatrix;
  }

  private Matrix ReadMatrix(string data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    float[] numArray = new float[6];
    string[] strArray = data.Split(this.m_commaSeparator);
    for (int index = 0; index < numArray.Length; ++index)
      numArray[index] = XPSRenderer.ParseFloat(strArray[index]);
    return new Matrix(numArray[0], numArray[1], numArray[2], numArray[3], this.ConvertToPoints((double) numArray[4]), this.ConvertToPoints((double) numArray[5]));
  }

  private PdfFont GetFont(Glyphs glyphs)
  {
    PdfTrueTypeFont font1 = (PdfTrueTypeFont) null;
    float points = this.ConvertToPoints(glyphs.FontRenderingEmSize);
    int deviceFontStyle = (int) this.GetDeviceFontStyle(glyphs.StyleSimulations);
    if (glyphs.UnicodeString != null)
      PdfString.IsUnicode(glyphs.UnicodeString);
    List<FontFamily> fontFamilyList = new List<FontFamily>((IEnumerable<FontFamily>) FontFamily.Families);
    if (glyphs.FontUri != null)
    {
      if (this.m_reader.Fonts.ContainsKey(glyphs.FontUri))
      {
        PdfTrueTypeFont font2 = this.m_reader.Fonts[glyphs.FontUri];
        if (font2.Name.Contains("Segoe") && glyphs.BidiLevel == "1")
        {
          float size = points - points * 0.275f;
          font1 = new PdfTrueTypeFont(font2, size, true);
        }
        else
          font1 = new PdfTrueTypeFont(font2, points, true);
        this.m_tffReader = font1.TtfReader;
      }
      else if (glyphs.UnicodeString != null)
      {
        Stream stream = this.m_reader.ReadFont(glyphs.FontUri);
        stream.Position = 0L;
        BinaryReader reader = new BinaryReader(stream);
        TtfReader ttfReader = new TtfReader(reader);
        string unicodeString = glyphs.UnicodeString;
        ushort[] numArray1 = new ushort[unicodeString.Length];
        KernelApi.GetStringTypeExW(2048U /*0x0800*/, StringInfoType.CT_TYPE2, unicodeString, unicodeString.Length, numArray1);
        if (this.IsRTLText(numArray1))
        {
          bool isBold = ttfReader.Metrics.IsBold;
          bool isItalic = ttfReader.Metrics.IsItalic;
          stream.Position = 0L;
          int length = (int) stream.Length;
          IntPtr num = Marshal.AllocCoTaskMem(length);
          byte[] numArray2 = new byte[length];
          stream.Read(numArray2, 0, length);
          Marshal.Copy(numArray2, 0, num, length);
          this.PrivateFonts.AddMemoryFont(num, length);
          Marshal.FreeCoTaskMem(num);
          FontStyle style = FontStyle.Regular;
          if (isBold)
            style |= FontStyle.Bold;
          if (isItalic)
            style |= FontStyle.Italic;
          Font font3 = new Font(this.PrivateFonts.Families[this.Find(ttfReader.Metrics.FontFamily)], 1f, style);
          if (font3.Name.Contains("Segoe") && glyphs.BidiLevel == "1")
          {
            float size = points - points * 0.275f;
            font1 = new PdfTrueTypeFont(font3, size, true);
          }
          else
            font1 = new PdfTrueTypeFont(font3, points, true);
          this.m_tffReader = font1.TtfReader;
          this.m_reader.Fonts.Add(glyphs.FontUri, font1);
          stream.Dispose();
          reader.Close();
          ttfReader.Close();
        }
        else
        {
          stream.Position = 0L;
          PdfTrueTypeFont font4;
          if (this.m_reader.fontNames.Count > 0)
          {
            bool flag = true;
            if (this.m_reader.fontNames.Contains(ttfReader.Metrics.FontFamily))
            {
              PdfTrueTypeFont pdfTrueTypeFont1 = new PdfTrueTypeFont(stream, points, this.m_reader.count++.ToString(), this.m_embedFont.EmbedCompleteFont);
              foreach (KeyValuePair<string, PdfTrueTypeFont> font5 in this.m_reader.Fonts)
              {
                if (font5.Value.Metrics.PostScriptName == ttfReader.Metrics.PostScriptName)
                {
                  PdfTrueTypeFont pdfTrueTypeFont2 = font5.Value;
                  if (pdfTrueTypeFont1.Metrics.WidthTable.ToArray().Count == pdfTrueTypeFont2.Metrics.WidthTable.ToArray().Count)
                  {
                    for (int index = 0; index < pdfTrueTypeFont1.Metrics.WidthTable.ToArray().Count; ++index)
                    {
                      if (pdfTrueTypeFont1.Metrics.WidthTable[index] != pdfTrueTypeFont2.Metrics.WidthTable[index])
                      {
                        flag = false;
                        break;
                      }
                    }
                  }
                  else
                    flag = false;
                }
              }
              stream.Position = 0L;
              font4 = flag ? new PdfTrueTypeFont(stream, points, (this.m_reader.count - 1).ToString(), this.m_embedFont.EmbedCompleteFont) : pdfTrueTypeFont1;
              this.m_tffReader = font4.TtfReader;
            }
            else
              font4 = new PdfTrueTypeFont(stream, points, this.m_embedFont.EmbedCompleteFont);
            this.m_tffReader = font4.TtfReader;
          }
          else
            font4 = new PdfTrueTypeFont(stream, points, this.m_embedFont.EmbedCompleteFont);
          this.m_tffReader = font4.TtfReader;
          this.m_reader.fontNames.Add(font4.Metrics.Name);
          this.m_reader.Fonts.Add(glyphs.FontUri, font4);
          return (PdfFont) font4;
        }
      }
      else if (glyphs.UnicodeString == null)
      {
        Stream fontStream = this.m_reader.ReadFont(glyphs.FontUri);
        fontStream.Position = 0L;
        font1 = new PdfTrueTypeFont(fontStream, points, this.m_embedFont.EmbedCompleteFont);
        this.m_tffReader = font1.TtfReader;
      }
    }
    else if (glyphs.DeviceFontName != null)
    {
      font1 = new PdfTrueTypeFont(new Font(glyphs.DeviceFontName, points, this.GetDeviceFontStyle(glyphs.StyleSimulations)), true, false, this.m_embedFont.EmbedCompleteFont);
      this.m_tffReader = font1.TtfReader;
    }
    return (PdfFont) font1;
  }

  private PdfBrush GetSolidBrush(string color)
  {
    if (color == null)
      return (PdfBrush) null;
    if (color.Contains("StaticResource"))
    {
      PdfBrush solidBrush = PdfBrushes.Transparent;
      Brush brush = this.ReadStaticResource(color) as Brush;
      if (brush.Item is SolidColorBrush)
        solidBrush = (PdfBrush) new PdfSolidBrush((PdfColor) this.FromHtml((brush.Item as SolidColorBrush).Color));
      else if (brush.Item is LinearGradientBrush)
        solidBrush = (PdfBrush) this.ReadLinearGradientBrush(brush.Item as LinearGradientBrush);
      else if (brush.Item is RadialGradientBrush)
        solidBrush = (PdfBrush) this.ReadRadialGradientBrush(brush.Item as RadialGradientBrush);
      return solidBrush;
    }
    if (!color.Contains("icc"))
      return (PdfBrush) new PdfSolidBrush(new PdfColor(ColorTranslator.FromHtml(color)));
    PdfBrush transparent = PdfBrushes.Transparent;
    string[] strArray = Regex.Split(Regex.Split(color, ".icc")[1], ",");
    float[] numArray = new float[strArray.Length];
    PdfBrush solidBrush1;
    if (numArray.Length == 5)
    {
      this.m_page.Graphics.Save();
      this.m_page.Graphics.ColorSpace = PdfColorSpace.CMYK;
      for (int index = 0; index < strArray.Length; ++index)
        numArray[index] = Convert.ToSingle(strArray[index]);
      solidBrush1 = (PdfBrush) new PdfSolidBrush(new PdfColor(numArray[1], numArray[2], numArray[3], numArray[4]));
    }
    else
      solidBrush1 = (PdfBrush) null;
    return solidBrush1;
  }

  private FontStyle GetDeviceFontStyle(StyleSimulations style)
  {
    FontStyle deviceFontStyle = FontStyle.Regular;
    switch (style)
    {
      case StyleSimulations.ItalicSimulation:
        deviceFontStyle = FontStyle.Italic;
        break;
      case StyleSimulations.BoldSimulation:
        deviceFontStyle = FontStyle.Bold;
        break;
      case StyleSimulations.BoldItalicSimulation:
        deviceFontStyle = FontStyle.Bold | FontStyle.Italic;
        break;
    }
    return deviceFontStyle;
  }

  private FontStyle GetFontStyle(PdfFontStyle style)
  {
    FontStyle fontStyle = FontStyle.Regular;
    if ((style & PdfFontStyle.Bold) == PdfFontStyle.Bold)
      fontStyle |= FontStyle.Bold;
    if ((style & PdfFontStyle.Italic) == PdfFontStyle.Italic)
      fontStyle |= FontStyle.Italic;
    if ((style & PdfFontStyle.Strikeout) == PdfFontStyle.Strikeout)
      fontStyle |= FontStyle.Strikeout;
    if ((style & PdfFontStyle.Underline) == PdfFontStyle.Underline)
      fontStyle |= FontStyle.Underline;
    return fontStyle;
  }

  private float ConvertToPoints(double value)
  {
    return this.m_unitConvertor.ConvertFromPixels((float) value, PdfGraphicsUnit.Point);
  }

  private PointF ConvertToPoints(PointF point)
  {
    return new PointF(this.ConvertToPoints((double) point.X), this.ConvertToPoints((double) point.Y));
  }

  private int Find(string fontName)
  {
    for (int index = 0; index < this.PrivateFonts.Families.Length; ++index)
    {
      if (this.PrivateFonts.Families[index].Name == fontName)
        return index;
    }
    return 0;
  }

  private Color FromHtml(string colorString)
  {
    string[] strArray1 = colorString.Replace("sc#", string.Empty).Split(',');
    Color empty = Color.Empty;
    Color color;
    if (strArray1 != null && strArray1.Length > 1)
    {
      int num1 = 0;
      double[] numArray1 = new double[4]
      {
        strArray1.Length == 4 ? XPSRenderer.ParseDouble(strArray1[num1++]) : 1.0,
        0.0,
        0.0,
        0.0
      };
      double[] numArray2 = numArray1;
      string[] strArray2 = strArray1;
      int index1 = num1;
      int num2 = index1 + 1;
      double num3 = XPSRenderer.ParseDouble(strArray2[index1]);
      numArray2[1] = num3;
      double[] numArray3 = numArray1;
      string[] strArray3 = strArray1;
      int index2 = num2;
      int num4 = index2 + 1;
      double num5 = XPSRenderer.ParseDouble(strArray3[index2]);
      numArray3[2] = num5;
      double[] numArray4 = numArray1;
      string[] strArray4 = strArray1;
      int index3 = num4;
      int num6 = index3 + 1;
      double num7 = XPSRenderer.ParseDouble(strArray4[index3]);
      numArray4[3] = num7;
      color = Color.FromArgb(numArray1[0] == 1.0 ? (int) byte.MaxValue : (int) (numArray1[0] * 256.0), numArray1[1] == 1.0 ? (int) byte.MaxValue : (int) (numArray1[1] * 256.0), numArray1[2] == 1.0 ? (int) byte.MaxValue : (int) (numArray1[2] * 256.0), numArray1[3] == 1.0 ? (int) byte.MaxValue : (int) (numArray1[3] * 256.0));
    }
    else
      color = ColorTranslator.FromHtml(colorString);
    return color;
  }

  internal static float ParseFloat(string f)
  {
    float result = 0.0f;
    if (CultureInfo.CurrentCulture.ToString() == "pt-BR")
      f = f.Replace(',', '.');
    else if (CultureInfo.CurrentCulture.ToString() == "fi-FI")
      f = f.Replace(',', '.');
    if (float.TryParse(f, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result))
      result = float.Parse(f, (IFormatProvider) CultureInfo.InvariantCulture);
    return result;
  }

  internal static double ParseDouble(string f)
  {
    double result = 0.0;
    if (double.TryParse(f, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result))
      result = double.Parse(f, (IFormatProvider) CultureInfo.InvariantCulture);
    return result;
  }

  private void DrawGlyphTextElement(
    Glyphs glyphs,
    PointF location,
    string[] glyphIndices,
    List<string> uniCodeString,
    PdfFont font,
    PdfBrush brush,
    PdfPen pen,
    PdfStringFormat stringFormat,
    bool boldStyle,
    List<string> ligatureIndices,
    float ascent)
  {
    PdfTextElement pdfTextElement = new PdfTextElement();
    PdfLayoutResult pdfLayoutResult = (PdfLayoutResult) null;
    float width = 0.0f;
    int index1 = 0;
    int num1 = uniCodeString.Count - glyphIndices.Length;
    int num2 = num1 >= 0 ? glyphIndices.Length : uniCodeString.Count;
    for (int index2 = 0; index2 < num2; ++index2)
    {
      pdfTextElement.Text = uniCodeString[index2];
      pdfTextElement.Font = font;
      pdfTextElement.Brush = brush;
      if (boldStyle)
        pdfTextElement.Pen = pen;
      pdfTextElement.StringFormat = stringFormat;
      if (pdfLayoutResult != null && (double) width != 0.0)
        location.X = pdfLayoutResult.Bounds.Left + width;
      else if (pdfLayoutResult != null)
        location.X = pdfLayoutResult.Bounds.Right;
      this.m_graphics.m_isNormalRender = false;
      bool flag = false;
      if (ligatureIndices != null && ligatureIndices.Count > index1 && glyphIndices[index2] != null && glyphIndices[index2].Contains(ligatureIndices[index1]) && glyphIndices[index2].Length > 0 && glyphIndices[index2].Contains("("))
      {
        int num3 = int.Parse(ligatureIndices[index1], (IFormatProvider) CultureInfo.InvariantCulture);
        TtfGlyphInfo glyph = this.m_tffReader.GetGlyph(num3);
        if ((glyph.CharCode == 32 /*0x20*/ || glyph.CharCode == -1 || num3 == glyph.CharCode) && this.m_tffReader.CompleteGlyph.ContainsKey(num3))
        {
          glyph = this.m_tffReader.CompleteGlyph[num3];
          flag = true;
          width = (float) glyph.Width * (1f / 1000f * font.Size);
        }
        ++index1;
      }
      if (glyphIndices[index2].Length > 0 && glyphIndices[index2].Contains(","))
      {
        glyphIndices[index2] = glyphIndices[index2].Split(',')[1];
        if (glyphIndices[index2] != "")
          width = (float) ((double) (int) float.Parse(glyphIndices[index2], (IFormatProvider) CultureInfo.InvariantCulture) * glyphs.FontRenderingEmSize / 100.0);
        width = this.ConvertToPoints((double) width);
      }
      else if (glyphIndices[index2] != null && !flag && (glyphIndices[index2].Length == 0 || !glyphIndices[index2].Contains(",")))
        width = font.GetCharWidth(uniCodeString[index2].ToCharArray()[0], stringFormat);
      if ((double) width > 0.0 && (double) width > (double) font.GetCharWidth(uniCodeString[index2].ToCharArray()[0], stringFormat) && (double) this.m_locationY < (double) this.m_page.Size.Height - (double) ascent)
        pdfLayoutResult = (PdfLayoutResult) pdfTextElement.Draw(this.m_page, location, width, new PdfLayoutFormat());
      else if ((double) this.m_locationY < (double) this.m_page.Size.Height - (double) ascent)
      {
        pdfLayoutResult = pdfTextElement.Draw(this.m_page, location);
      }
      else
      {
        this.m_graphics.DrawString(pdfTextElement.Text, font, pen, brush, location, stringFormat);
        location.X += width;
      }
    }
    if ((double) width > 0.0 && pdfLayoutResult != null)
      location.X = pdfLayoutResult.Bounds.X + width;
    else if (pdfLayoutResult != null)
      location.X = pdfLayoutResult.Bounds.Right;
    string empty = string.Empty;
    if (num1 <= 0)
      return;
    int length = glyphIndices.Length;
    for (int index3 = 0; index3 < num1; ++index3)
      empty += uniCodeString[length + index3];
    if (!boldStyle)
      this.m_graphics.DrawString(empty, font, brush, location, stringFormat);
    else
      this.m_graphics.DrawString(empty, font, pen, brush, location, stringFormat);
  }

  public void Dispose()
  {
  }
}
