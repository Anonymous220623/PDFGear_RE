// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.FieldPainter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class FieldPainter
{
  private static PdfBrush s_whiteBrush = (PdfBrush) null;
  private static PdfBrush s_blackBrush = (PdfBrush) null;
  private static PdfBrush s_silverBrush = (PdfBrush) null;
  private static PdfBrush s_grayBrush = (PdfBrush) null;
  private static Dictionary<string, PdfPen> s_pens = new Dictionary<string, PdfPen>();
  internal static bool isAutoFontSize = false;
  private static PdfStringFormat s_checkFieldFormat = (PdfStringFormat) null;

  private static PdfBrush WhiteBrush
  {
    get
    {
      lock (FieldPainter.s_pens)
      {
        if (FieldPainter.s_whiteBrush == null)
          FieldPainter.s_whiteBrush = PdfBrushes.White;
        return FieldPainter.s_whiteBrush;
      }
    }
  }

  private static PdfBrush BlackBrush
  {
    get
    {
      lock (FieldPainter.s_pens)
      {
        if (FieldPainter.s_blackBrush == null)
          FieldPainter.s_blackBrush = PdfBrushes.Black;
        return FieldPainter.s_blackBrush;
      }
    }
  }

  private static PdfBrush GrayBrush
  {
    get
    {
      lock (FieldPainter.s_pens)
      {
        if (FieldPainter.s_grayBrush == null)
          FieldPainter.s_grayBrush = PdfBrushes.Gray;
        return FieldPainter.s_grayBrush;
      }
    }
  }

  private static PdfBrush SilverBrush
  {
    get
    {
      lock (FieldPainter.s_pens)
      {
        if (FieldPainter.s_silverBrush == null)
          FieldPainter.s_silverBrush = PdfBrushes.Silver;
        return FieldPainter.s_silverBrush;
      }
    }
  }

  private static PdfStringFormat CheckFieldFormat
  {
    get
    {
      lock (FieldPainter.s_pens)
      {
        if (FieldPainter.s_checkFieldFormat == null)
          FieldPainter.s_checkFieldFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        return FieldPainter.s_checkFieldFormat;
      }
    }
  }

  public static void DrawButton(
    PdfGraphics g,
    PaintParams paintParams,
    string text,
    PdfFont font,
    PdfStringFormat format)
  {
    if (g == null)
      throw new ArgumentNullException(nameof (g));
    if (paintParams == null)
      throw new ArgumentNullException(nameof (paintParams));
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    FieldPainter.DrawRectangularControl(g, paintParams);
    RectangleF layoutRectangle = paintParams.Bounds;
    PdfDictionary pdfDictionary1 = (PdfDictionary) null;
    if (g.Layer != null && g.Page != null && !g.Page.Dictionary.ContainsKey("Rotate"))
    {
      pdfDictionary1 = new PdfDictionary();
      if ((g.Page.Dictionary["Parent"] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary2)
        pdfDictionary2.ContainsKey("Rotate");
    }
    int rotationAngle = paintParams.RotationAngle;
    if (g.Layer != null && g.Page.Rotation != PdfPageRotateAngle.RotateAngle0 || paintParams.RotationAngle > 0)
    {
      PdfGraphicsState state = g.Save();
      if (g.Layer != null && g.Page.Rotation != PdfPageRotateAngle.RotateAngle0)
      {
        if (g.Page.Rotation == PdfPageRotateAngle.RotateAngle90)
        {
          g.TranslateTransform(g.Size.Height, 0.0f);
          g.RotateTransform(90f);
          float y = g.Page.Size.Height - (layoutRectangle.X + layoutRectangle.Width);
          layoutRectangle = new RectangleF(layoutRectangle.Y, y, layoutRectangle.Height, layoutRectangle.Width);
        }
        else if (g.Page.Rotation == PdfPageRotateAngle.RotateAngle180)
        {
          g.TranslateTransform(g.Size.Width, g.Size.Height);
          g.RotateTransform(-180f);
          SizeF size = g.Size;
          layoutRectangle = new RectangleF(size.Width - (layoutRectangle.X + layoutRectangle.Width), size.Height - (layoutRectangle.Y + layoutRectangle.Height), layoutRectangle.Width, layoutRectangle.Height);
        }
        else if (g.Page.Rotation == PdfPageRotateAngle.RotateAngle270)
        {
          g.TranslateTransform(0.0f, g.Size.Width);
          g.RotateTransform(270f);
          layoutRectangle = new RectangleF(g.Size.Width - (layoutRectangle.Y + layoutRectangle.Height), layoutRectangle.X, layoutRectangle.Height, layoutRectangle.Width);
        }
      }
      if (paintParams.RotationAngle > 0)
      {
        if (paintParams.RotationAngle == 90)
        {
          g.TranslateTransform(0.0f, g.Size.Height);
          g.RotateTransform(-90f);
          layoutRectangle = new RectangleF(g.Size.Height - (layoutRectangle.Y + layoutRectangle.Height), layoutRectangle.X, layoutRectangle.Height, layoutRectangle.Width);
        }
        else if (paintParams.RotationAngle == 270)
        {
          g.TranslateTransform(g.Size.Width, 0.0f);
          g.RotateTransform(-270f);
          layoutRectangle = new RectangleF(layoutRectangle.Y, g.Size.Width - (layoutRectangle.X + layoutRectangle.Width), layoutRectangle.Height, layoutRectangle.Width);
        }
        else if (paintParams.RotationAngle == 180)
        {
          g.TranslateTransform(g.Size.Width, g.Size.Height);
          g.RotateTransform(-180f);
          layoutRectangle = new RectangleF(g.Size.Width - (layoutRectangle.X + layoutRectangle.Width), g.Size.Height - (layoutRectangle.Y + layoutRectangle.Height), layoutRectangle.Width, layoutRectangle.Height);
        }
      }
      g.DrawString(text, font, paintParams.ForeBrush, layoutRectangle, format);
      g.Restore(state);
    }
    else
      g.DrawString(text, font, paintParams.ForeBrush, layoutRectangle, format);
  }

  public static void DrawPressedButton(
    PdfGraphics g,
    PaintParams paintParams,
    string text,
    PdfFont font,
    PdfStringFormat format)
  {
    if (g == null)
      throw new ArgumentNullException(nameof (g));
    if (paintParams == null)
      throw new ArgumentNullException(nameof (paintParams));
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (paintParams.BorderStyle == PdfBorderStyle.Inset)
      g.DrawRectangle(paintParams.ShadowBrush, paintParams.Bounds);
    else
      g.DrawRectangle(paintParams.BackBrush, paintParams.Bounds);
    FieldPainter.DrawBorder(g, paintParams.Bounds, paintParams.BorderPen, paintParams.BorderStyle, paintParams.BorderWidth);
    RectangleF layoutRectangle = new RectangleF(paintParams.BorderWidth, paintParams.BorderWidth, paintParams.Bounds.Size.Width - paintParams.BorderWidth, paintParams.Bounds.Size.Height - paintParams.BorderWidth);
    g.DrawString(text, font, paintParams.ForeBrush, layoutRectangle, format);
    switch (paintParams.BorderStyle)
    {
      case PdfBorderStyle.Beveled:
        FieldPainter.DrawLeftTopShadow(g, paintParams.Bounds, paintParams.BorderWidth, paintParams.ShadowBrush);
        FieldPainter.DrawRightBottomShadow(g, paintParams.Bounds, paintParams.BorderWidth, FieldPainter.WhiteBrush);
        break;
      case PdfBorderStyle.Inset:
        FieldPainter.DrawLeftTopShadow(g, paintParams.Bounds, paintParams.BorderWidth, FieldPainter.GrayBrush);
        FieldPainter.DrawRightBottomShadow(g, paintParams.Bounds, paintParams.BorderWidth, FieldPainter.SilverBrush);
        break;
      default:
        FieldPainter.DrawLeftTopShadow(g, paintParams.Bounds, paintParams.BorderWidth, paintParams.ShadowBrush);
        break;
    }
  }

  internal static void DrawTextBox(
    PdfGraphics g,
    PaintParams paintParams,
    string text,
    PdfFont font,
    PdfStringFormat format,
    bool multiLine,
    bool Scroll,
    int maxLength)
  {
    if (paintParams.InsertSpace)
    {
      float width = 0.0f;
      char[] charArray = text.ToCharArray();
      if (maxLength > 0)
        width = paintParams.Bounds.Width / (float) maxLength;
      g.DrawRectangle(paintParams.BorderPen, paintParams.Bounds);
      for (int index = 0; index < maxLength; ++index)
      {
        if (format.Alignment != PdfTextAlignment.Right)
        {
          if (format.Alignment == PdfTextAlignment.Center && charArray.Length < maxLength)
          {
            int num = maxLength / 2 - (int) Math.Ceiling((double) charArray.Length / 2.0);
            text = index < num || index >= num + charArray.Length ? "" : charArray[index - num].ToString();
          }
          else
            text = charArray.Length <= index ? "" : charArray[index].ToString();
        }
        else
          text = maxLength - charArray.Length > index ? "" : charArray[index - (maxLength - charArray.Length)].ToString();
        paintParams.Bounds = new RectangleF(paintParams.Bounds.X, paintParams.Bounds.Y, width, paintParams.Bounds.Height);
        PdfStringFormat format1 = (PdfStringFormat) format.Clone();
        format1.Alignment = PdfTextAlignment.Center;
        FieldPainter.DrawTextBox(g, paintParams, text, font, format1, multiLine, Scroll);
        paintParams.Bounds = new RectangleF(paintParams.Bounds.X + width, paintParams.Bounds.Y, width, paintParams.Bounds.Height);
        if ((double) paintParams.BorderWidth != 0.0)
          g.DrawLine(paintParams.BorderPen, paintParams.Bounds.Location, new PointF(paintParams.Bounds.X, paintParams.Bounds.Y + paintParams.Bounds.Height));
      }
    }
    else
      FieldPainter.DrawTextBox(g, paintParams, text, font, format, multiLine, Scroll);
  }

  public static void DrawTextBox(
    PdfGraphics g,
    PaintParams paintParams,
    string text,
    PdfFont font,
    PdfStringFormat format,
    bool multiLine,
    bool scroll)
  {
    if (g == null)
      throw new ArgumentNullException(nameof (g));
    if (paintParams == null)
      throw new ArgumentNullException(nameof (paintParams));
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (!paintParams.InsertSpace)
      FieldPainter.DrawRectangularControl(g, paintParams);
    RectangleF layoutRectangle = paintParams.Bounds;
    if (paintParams.BorderStyle == PdfBorderStyle.Beveled || paintParams.BorderStyle == PdfBorderStyle.Inset)
    {
      layoutRectangle.X += 4f * paintParams.BorderWidth;
      layoutRectangle.Width -= 8f * paintParams.BorderWidth;
    }
    else
    {
      layoutRectangle.X += 2f * paintParams.BorderWidth;
      layoutRectangle.Width -= 4f * paintParams.BorderWidth;
    }
    if (multiLine)
    {
      float num1 = format == null || (double) format.LineSpacing == 0.0 ? font.Height : format.LineSpacing;
      bool flag = format != null && format.SubSuperScript == PdfSubSuperScript.SubScript;
      float ascent = font.Metrics.GetAscent(format);
      float descent = font.Metrics.GetDescent(format);
      float num2 = flag ? num1 - (font.Height + descent) : num1 - ascent;
      if (text.Contains("\n"))
      {
        if (layoutRectangle.Location == PointF.Empty)
          layoutRectangle.Y = (float) -((double) layoutRectangle.Y - (double) num2);
      }
      else if (layoutRectangle.Location == PointF.Empty)
        layoutRectangle.Y = (float) -((double) layoutRectangle.Y - (double) num2);
      if (FieldPainter.isAutoFontSize && (double) paintParams.BorderWidth != 0.0)
        layoutRectangle.Y += 2.5f * paintParams.BorderWidth;
    }
    PdfDictionary pdfDictionary1 = (PdfDictionary) null;
    if (g.Layer != null && g.Page != null && !g.Page.Dictionary.ContainsKey("Rotate"))
    {
      pdfDictionary1 = new PdfDictionary();
      if ((g.Page.Dictionary["Parent"] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary2)
        pdfDictionary2.ContainsKey("Rotate");
    }
    if (g.Layer != null && g.Page.Rotation != PdfPageRotateAngle.RotateAngle0 || paintParams.RotationAngle > 0)
    {
      PdfGraphicsState state = g.Save();
      if (paintParams.PageRotationAngle != PdfPageRotateAngle.RotateAngle0)
      {
        if (paintParams.PageRotationAngle == PdfPageRotateAngle.RotateAngle90)
        {
          g.TranslateTransform(g.Size.Height, 0.0f);
          g.RotateTransform(90f);
          float y = g.Size.Height - (layoutRectangle.X + layoutRectangle.Width);
          layoutRectangle = new RectangleF(layoutRectangle.Y, y, layoutRectangle.Height, layoutRectangle.Width);
        }
        else if (paintParams.PageRotationAngle == PdfPageRotateAngle.RotateAngle180)
        {
          g.TranslateTransform(g.Size.Width, g.Size.Height);
          g.RotateTransform(-180f);
          SizeF size = g.Size;
          layoutRectangle = new RectangleF(size.Width - (layoutRectangle.X + layoutRectangle.Width), size.Height - (layoutRectangle.Y + layoutRectangle.Height), layoutRectangle.Width, layoutRectangle.Height);
        }
        else if (paintParams.PageRotationAngle == PdfPageRotateAngle.RotateAngle270)
        {
          g.TranslateTransform(0.0f, g.Size.Width);
          g.RotateTransform(270f);
          layoutRectangle = new RectangleF(g.Size.Width - (layoutRectangle.Y + layoutRectangle.Height), layoutRectangle.X, layoutRectangle.Height, layoutRectangle.Width);
        }
      }
      if (paintParams.RotationAngle > 0)
      {
        if (paintParams.RotationAngle == 90)
        {
          if (paintParams.PageRotationAngle == PdfPageRotateAngle.RotateAngle90)
          {
            g.TranslateTransform(0.0f, g.Size.Height);
            g.RotateTransform(-90f);
            layoutRectangle = new RectangleF(g.Size.Height - (layoutRectangle.Y + layoutRectangle.Height), layoutRectangle.X, layoutRectangle.Height, layoutRectangle.Width);
          }
          else if ((double) layoutRectangle.Width > (double) layoutRectangle.Height)
          {
            g.TranslateTransform(0.0f, g.Size.Height);
            g.RotateTransform(-90f);
            layoutRectangle = new RectangleF(paintParams.Bounds.X, paintParams.Bounds.Y, paintParams.Bounds.Width, paintParams.Bounds.Height);
          }
          else
          {
            float x = layoutRectangle.X;
            layoutRectangle.X = (float) -((double) layoutRectangle.Y + (double) layoutRectangle.Height);
            layoutRectangle.Y = x;
            float height = layoutRectangle.Height;
            layoutRectangle.Height = (double) layoutRectangle.Width > (double) font.Height ? layoutRectangle.Width : font.Height;
            layoutRectangle.Width = height;
            g.RotateTransform(-90f);
            layoutRectangle = new RectangleF(layoutRectangle.X, layoutRectangle.Y, layoutRectangle.Width, layoutRectangle.Height);
          }
        }
        else if (paintParams.RotationAngle == 270)
        {
          g.TranslateTransform(g.Size.Width, 0.0f);
          g.RotateTransform(-270f);
          layoutRectangle = new RectangleF(layoutRectangle.Y, g.Size.Width - (layoutRectangle.X + layoutRectangle.Width), layoutRectangle.Height, layoutRectangle.Width);
        }
        else if (paintParams.RotationAngle == 180)
        {
          g.TranslateTransform(g.Size.Width, g.Size.Height);
          g.RotateTransform(-180f);
          layoutRectangle = new RectangleF(g.Size.Width - (layoutRectangle.X + layoutRectangle.Width), g.Size.Height - (layoutRectangle.Y + layoutRectangle.Height), layoutRectangle.Width, layoutRectangle.Height);
        }
      }
      g.DrawString(text, font, paintParams.ForeBrush, layoutRectangle, format);
      g.Restore(state);
    }
    else
      g.DrawString(text, font, paintParams.ForeBrush, layoutRectangle, format);
  }

  public static void DrawListBox(
    PdfGraphics g,
    PaintParams paintParams,
    PdfListFieldItemCollection items,
    int[] selectedItem,
    PdfFont font,
    PdfStringFormat stringFormat)
  {
    if (g == null)
      throw new ArgumentNullException(nameof (g));
    if (paintParams == null)
      throw new ArgumentNullException(nameof (paintParams));
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    FieldPainter.DrawRectangularControl(g, paintParams);
    int index = 0;
    for (int count = items.Count; index < count; ++index)
    {
      PdfListFieldItem pdfListFieldItem = items[index];
      PointF empty = PointF.Empty;
      float borderWidth = paintParams.BorderWidth;
      float num1 = 2f * borderWidth;
      bool flag1 = paintParams.BorderStyle == PdfBorderStyle.Inset || paintParams.BorderStyle == PdfBorderStyle.Beveled;
      if (flag1)
      {
        empty.X = 2f * num1;
        empty.Y = (float) ((double) (index + 2) * (double) borderWidth + (double) font.Size * (double) index);
      }
      else
      {
        empty.X = num1;
        empty.Y = (float) ((double) (index + 1) * (double) borderWidth + (double) font.Size * (double) index);
      }
      PdfBrush brush1 = paintParams.ForeBrush;
      RectangleF bounds = paintParams.Bounds;
      float width = bounds.Width - num1;
      RectangleF rectangle = bounds;
      if (flag1)
        rectangle.Height -= num1;
      else
        rectangle.Height -= borderWidth;
      g.SetClip(rectangle, PdfFillMode.Winding);
      bool flag2 = false;
      foreach (int num2 in selectedItem)
      {
        if (num2 == index)
          flag2 = true;
      }
      if (paintParams.RotationAngle == 0 && flag2)
      {
        float x = bounds.X + borderWidth;
        if (flag1)
        {
          x += borderWidth;
          width -= num1;
        }
        PdfBrush brush2 = (PdfBrush) new PdfSolidBrush(new PdfColor(byte.MaxValue, (byte) 51, (byte) 153, byte.MaxValue));
        g.DrawRectangle(brush2, x, empty.Y, width, font.Height);
        brush1 = (PdfBrush) new PdfSolidBrush(new PdfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
      }
      string s = pdfListFieldItem.Text != null ? pdfListFieldItem.Text : pdfListFieldItem.Value;
      RectangleF layoutRectangle = new RectangleF(empty.X, empty.Y, width - empty.X, font.Height);
      PdfDictionary pdfDictionary1 = (PdfDictionary) null;
      if (g.Layer != null && g.Page != null && !g.Page.Dictionary.ContainsKey("Rotate"))
      {
        pdfDictionary1 = new PdfDictionary();
        if ((g.Page.Dictionary["Parent"] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary2)
          pdfDictionary2.ContainsKey("Rotate");
      }
      int rotationAngle = paintParams.RotationAngle;
      if (g.Layer != null && g.Page.Rotation != PdfPageRotateAngle.RotateAngle0 || paintParams.RotationAngle > 0)
      {
        PdfGraphicsState state = g.Save();
        if (g.Layer != null && g.Page.Rotation != PdfPageRotateAngle.RotateAngle0)
        {
          if (g.Page.Rotation == PdfPageRotateAngle.RotateAngle90)
          {
            g.TranslateTransform(g.Size.Height, 0.0f);
            g.RotateTransform(90f);
            float y = g.Page.Size.Height - (rectangle.X + rectangle.Width);
            rectangle = new RectangleF(rectangle.Y, y, rectangle.Height + rectangle.Width, rectangle.Width + rectangle.Height);
          }
          else if (g.Page.Rotation == PdfPageRotateAngle.RotateAngle180)
          {
            g.TranslateTransform(g.Size.Width, g.Size.Height);
            g.RotateTransform(-180f);
            SizeF size = g.Page.Size;
            rectangle = new RectangleF(size.Width - (rectangle.X + rectangle.Width), size.Height - (rectangle.Y + rectangle.Height), rectangle.Width, rectangle.Height);
          }
          else if (g.Page.Rotation == PdfPageRotateAngle.RotateAngle270)
          {
            g.TranslateTransform(0.0f, g.Size.Width);
            g.RotateTransform(270f);
            rectangle = new RectangleF(g.Page.Size.Width - (rectangle.Y + rectangle.Height), rectangle.X, rectangle.Height + rectangle.Width, rectangle.Width + rectangle.Height);
          }
        }
        if (paintParams.RotationAngle > 0)
        {
          if (paintParams.RotationAngle == 90)
          {
            g.TranslateTransform(0.0f, g.Size.Height);
            g.RotateTransform(-90f);
            rectangle = new RectangleF(g.Size.Height - (rectangle.Y + rectangle.Height), rectangle.X, rectangle.Height + rectangle.Width, rectangle.Width);
          }
          else if (paintParams.RotationAngle == 270)
          {
            g.TranslateTransform(g.Size.Width, 0.0f);
            g.RotateTransform(-270f);
            rectangle = new RectangleF(rectangle.Y, g.Size.Width - (rectangle.X + rectangle.Width), rectangle.Height + rectangle.Width, rectangle.Width);
          }
          else if (paintParams.RotationAngle == 180)
          {
            g.TranslateTransform(g.Size.Width, g.Size.Height);
            g.RotateTransform(-180f);
            rectangle = new RectangleF(g.Size.Width - (rectangle.X + rectangle.Width), g.Size.Height - (rectangle.Y + rectangle.Height), rectangle.Width, rectangle.Height);
          }
        }
        if (flag2)
        {
          float x = bounds.X + borderWidth;
          if (flag1)
          {
            x += borderWidth;
            width -= num1;
          }
          PdfBrush brush3 = (PdfBrush) new PdfSolidBrush(new PdfColor(byte.MaxValue, (byte) 51, (byte) 153, byte.MaxValue));
          g.DrawRectangle(brush3, x, empty.Y, width, font.Height);
          brush1 = (PdfBrush) new PdfSolidBrush(new PdfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
        }
        g.DrawString(s, font, brush1, layoutRectangle, stringFormat);
        g.Restore(state);
      }
      else
        g.DrawString(s, font, brush1, layoutRectangle, stringFormat);
    }
  }

  public static void DrawCheckBox(
    PdfGraphics g,
    PaintParams paintParams,
    string checkSymbol,
    PdfCheckFieldState state)
  {
    FieldPainter.DrawCheckBox(g, paintParams, checkSymbol, state, (PdfFont) null);
  }

  public static void DrawCheckBox(
    PdfGraphics g,
    PaintParams paintParams,
    string checkSymbol,
    PdfCheckFieldState state,
    PdfFont font)
  {
    if (g == null)
      throw new ArgumentNullException(nameof (g));
    if (paintParams == null)
      throw new ArgumentNullException(nameof (paintParams));
    if (checkSymbol == null)
      throw new ArgumentNullException(nameof (checkSymbol));
    switch (state)
    {
      case PdfCheckFieldState.Unchecked:
      case PdfCheckFieldState.Checked:
        if (paintParams.BorderPen != null && paintParams.BorderPen.Color.A != (byte) 0)
        {
          g.DrawRectangle(paintParams.BackBrush, paintParams.Bounds);
          break;
        }
        break;
      case PdfCheckFieldState.PressedUnchecked:
      case PdfCheckFieldState.PressedChecked:
        if (paintParams.BorderStyle == PdfBorderStyle.Beveled || paintParams.BorderStyle == PdfBorderStyle.Underline)
        {
          if (paintParams.BorderPen != null && paintParams.BorderPen.Color.A != (byte) 0)
          {
            g.DrawRectangle(paintParams.BackBrush, paintParams.Bounds);
            break;
          }
          break;
        }
        if (paintParams.BorderPen != null && paintParams.BorderPen.Color.A != (byte) 0)
        {
          g.DrawRectangle(paintParams.ShadowBrush, paintParams.Bounds);
          break;
        }
        break;
    }
    RectangleF rectangleF = paintParams.Bounds;
    FieldPainter.DrawBorder(g, paintParams.Bounds, paintParams.BorderPen, paintParams.BorderStyle, paintParams.BorderWidth);
    if (state == PdfCheckFieldState.PressedChecked || state == PdfCheckFieldState.PressedUnchecked)
    {
      switch (paintParams.BorderStyle)
      {
        case PdfBorderStyle.Beveled:
          FieldPainter.DrawLeftTopShadow(g, paintParams.Bounds, paintParams.BorderWidth, paintParams.ShadowBrush);
          FieldPainter.DrawRightBottomShadow(g, paintParams.Bounds, paintParams.BorderWidth, FieldPainter.WhiteBrush);
          break;
        case PdfBorderStyle.Inset:
          FieldPainter.DrawLeftTopShadow(g, paintParams.Bounds, paintParams.BorderWidth, FieldPainter.BlackBrush);
          FieldPainter.DrawRightBottomShadow(g, paintParams.Bounds, paintParams.BorderWidth, FieldPainter.WhiteBrush);
          break;
      }
    }
    else
    {
      switch (paintParams.BorderStyle)
      {
        case PdfBorderStyle.Beveled:
          FieldPainter.DrawLeftTopShadow(g, paintParams.Bounds, paintParams.BorderWidth, FieldPainter.WhiteBrush);
          FieldPainter.DrawRightBottomShadow(g, paintParams.Bounds, paintParams.BorderWidth, paintParams.ShadowBrush);
          break;
        case PdfBorderStyle.Inset:
          FieldPainter.DrawLeftTopShadow(g, paintParams.Bounds, paintParams.BorderWidth, FieldPainter.GrayBrush);
          FieldPainter.DrawRightBottomShadow(g, paintParams.Bounds, paintParams.BorderWidth, FieldPainter.SilverBrush);
          break;
      }
    }
    float num1 = 0.0f;
    float num2 = 0.0f;
    switch (state)
    {
      case PdfCheckFieldState.Checked:
      case PdfCheckFieldState.PressedChecked:
        if (font == null)
        {
          bool flag = paintParams.BorderStyle == PdfBorderStyle.Beveled || paintParams.BorderStyle == PdfBorderStyle.Inset;
          float borderWidth = paintParams.BorderWidth;
          if (flag)
            borderWidth *= 2f;
          float val2 = Math.Max(flag ? 2f * paintParams.BorderWidth : paintParams.BorderWidth, 1f);
          float num3 = Math.Min(borderWidth, val2);
          num2 = (double) paintParams.Bounds.Width > (double) paintParams.Bounds.Height ? paintParams.Bounds.Height : paintParams.Bounds.Width;
          font = (PdfFont) new PdfStandardFont(PdfFontFamily.ZapfDingbats, num2 - 2f * num3);
          if ((double) paintParams.Bounds.Width > (double) paintParams.Bounds.Height)
            num1 = (float) (((double) paintParams.Bounds.Height - (double) font.Height) / 2.0);
        }
        else
          font = (PdfFont) new PdfStandardFont(PdfFontFamily.ZapfDingbats, font.Size);
        if ((double) num2 == 0.0)
          num2 = paintParams.Bounds.Height;
        if ((double) num2 < (double) font.Size)
          throw new Exception("Font size cannot be greater than CheckBox height");
        PdfDictionary pdfDictionary1 = (PdfDictionary) null;
        if (g.Layer != null && g.Page != null && !g.Page.Dictionary.ContainsKey("Rotate"))
        {
          pdfDictionary1 = new PdfDictionary();
          if ((g.Page.Dictionary["Parent"] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary2)
            pdfDictionary2.ContainsKey("Rotate");
        }
        if (paintParams.PageRotationAngle != PdfPageRotateAngle.RotateAngle0 || paintParams.RotationAngle > 0)
        {
          PdfGraphicsState state1 = g.Save();
          if (paintParams.PageRotationAngle != PdfPageRotateAngle.RotateAngle0)
          {
            if (paintParams.PageRotationAngle == PdfPageRotateAngle.RotateAngle90)
            {
              g.TranslateTransform(g.Size.Height, 0.0f);
              g.RotateTransform(90f);
              float y = g.Size.Height - (rectangleF.X + rectangleF.Width);
              rectangleF = new RectangleF(rectangleF.Y, y, rectangleF.Height, rectangleF.Width);
            }
            else if (paintParams.PageRotationAngle == PdfPageRotateAngle.RotateAngle180)
            {
              g.TranslateTransform(g.Size.Width, g.Size.Height);
              g.RotateTransform(-180f);
              SizeF size = g.Size;
              rectangleF = new RectangleF(size.Width - (rectangleF.X + rectangleF.Width), size.Height - (rectangleF.Y + rectangleF.Height), rectangleF.Width, rectangleF.Height);
            }
            else if (paintParams.PageRotationAngle == PdfPageRotateAngle.RotateAngle270)
            {
              g.TranslateTransform(0.0f, g.Size.Width);
              g.RotateTransform(270f);
              rectangleF = new RectangleF(g.Size.Width - (rectangleF.Y + rectangleF.Height), rectangleF.X, rectangleF.Height, rectangleF.Width);
            }
          }
          if (paintParams.RotationAngle > 0)
          {
            if (paintParams.RotationAngle == 90)
            {
              if (paintParams.PageRotationAngle == PdfPageRotateAngle.RotateAngle90)
              {
                g.TranslateTransform(0.0f, g.Size.Height);
                g.RotateTransform(-90f);
                rectangleF = new RectangleF(g.Size.Height - (rectangleF.Y + rectangleF.Height), rectangleF.X, rectangleF.Height, rectangleF.Width);
              }
              else if ((double) rectangleF.Width > (double) rectangleF.Height)
              {
                g.TranslateTransform(0.0f, g.Size.Height);
                g.RotateTransform(-90f);
                rectangleF = new RectangleF(paintParams.Bounds.X, paintParams.Bounds.Y, paintParams.Bounds.Width, paintParams.Bounds.Height);
              }
              else
              {
                float x = rectangleF.X;
                rectangleF.X = (float) -((double) rectangleF.Y + (double) rectangleF.Height);
                rectangleF.Y = x;
                float height = rectangleF.Height;
                rectangleF.Height = (double) rectangleF.Width > (double) font.Height ? rectangleF.Width : font.Height;
                rectangleF.Width = height;
                g.RotateTransform(-90f);
                rectangleF = new RectangleF(rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
              }
            }
            else if (paintParams.RotationAngle == 270)
            {
              g.TranslateTransform(g.Size.Width, 0.0f);
              g.RotateTransform(-270f);
              rectangleF = new RectangleF(rectangleF.Y, g.Size.Width - (rectangleF.X + rectangleF.Width), rectangleF.Height, rectangleF.Width);
            }
            else if (paintParams.RotationAngle == 180)
            {
              g.TranslateTransform(g.Size.Width, g.Size.Height);
              g.RotateTransform(-180f);
              rectangleF = new RectangleF(g.Size.Width - (rectangleF.X + rectangleF.Width), g.Size.Height - (rectangleF.Y + rectangleF.Height), rectangleF.Width, rectangleF.Height);
            }
          }
          g.DrawString(checkSymbol, font, paintParams.ForeBrush, new RectangleF(rectangleF.X, rectangleF.Y - num1, rectangleF.Width, rectangleF.Height), FieldPainter.CheckFieldFormat);
          g.Restore(state1);
          break;
        }
        g.DrawString(checkSymbol, font, paintParams.ForeBrush, new RectangleF(rectangleF.X, rectangleF.Y - num1, rectangleF.Width, rectangleF.Height), FieldPainter.CheckFieldFormat);
        break;
    }
  }

  public static void DrawComboBox(PdfGraphics g, PaintParams paintParams)
  {
    if (g == null)
      throw new ArgumentNullException(nameof (g));
    if (paintParams == null)
      throw new ArgumentNullException(nameof (paintParams));
    FieldPainter.DrawRectangularControl(g, paintParams);
  }

  public static void DrawComboBox(
    PdfGraphics g,
    PaintParams paintParams,
    string text,
    PdfFont font,
    PdfStringFormat format)
  {
    if (g == null)
      throw new ArgumentNullException(nameof (g));
    if (paintParams == null)
      throw new ArgumentNullException(nameof (paintParams));
    FieldPainter.DrawRectangularControl(g, paintParams);
    RectangleF layoutRectangle = paintParams.Bounds;
    PdfDictionary pdfDictionary1 = (PdfDictionary) null;
    if (g.Layer != null && g.Page != null && !g.Page.Dictionary.ContainsKey("Rotate"))
    {
      pdfDictionary1 = new PdfDictionary();
      if ((g.Page.Dictionary["Parent"] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary2)
        pdfDictionary2.ContainsKey("Rotate");
    }
    int rotationAngle = paintParams.RotationAngle;
    if (g.Layer != null && g.Page.Rotation != PdfPageRotateAngle.RotateAngle0 || paintParams.RotationAngle > 0)
    {
      PdfGraphicsState state = g.Save();
      if (g.Layer != null && g.Page.Rotation != PdfPageRotateAngle.RotateAngle0)
      {
        if (g.Page.Rotation == PdfPageRotateAngle.RotateAngle90)
        {
          g.TranslateTransform(g.Size.Height, 0.0f);
          g.RotateTransform(90f);
          float y = g.Page.Size.Height - (layoutRectangle.X + layoutRectangle.Width);
          layoutRectangle = new RectangleF(layoutRectangle.Y, y, layoutRectangle.Height, layoutRectangle.Width);
        }
        else if (g.Page.Rotation == PdfPageRotateAngle.RotateAngle180)
        {
          g.TranslateTransform(g.Page.Size.Width, g.Page.Size.Height);
          g.RotateTransform(-180f);
          SizeF size = g.Page.Size;
          layoutRectangle = new RectangleF(size.Width - (layoutRectangle.X + layoutRectangle.Width), size.Height - (layoutRectangle.Y + layoutRectangle.Height), layoutRectangle.Width, layoutRectangle.Height);
        }
        else if (g.Page.Rotation == PdfPageRotateAngle.RotateAngle270)
        {
          g.TranslateTransform(0.0f, g.Size.Width);
          g.RotateTransform(270f);
          layoutRectangle = new RectangleF(g.Page.Size.Width - (layoutRectangle.Y + layoutRectangle.Height), layoutRectangle.X, layoutRectangle.Height, layoutRectangle.Width);
        }
      }
      if (paintParams.RotationAngle > 0)
      {
        if (paintParams.RotationAngle == 90)
        {
          g.TranslateTransform(0.0f, g.Size.Height);
          g.RotateTransform(-90f);
          layoutRectangle = new RectangleF(g.Size.Height - (layoutRectangle.Y + layoutRectangle.Height), layoutRectangle.X, layoutRectangle.Height, layoutRectangle.Width);
        }
        else if (paintParams.RotationAngle == 270)
        {
          g.TranslateTransform(g.Size.Width, 0.0f);
          g.RotateTransform(-270f);
          layoutRectangle = new RectangleF(layoutRectangle.Y, g.Size.Width - (layoutRectangle.X + layoutRectangle.Width), layoutRectangle.Height, layoutRectangle.Width);
        }
        else if (paintParams.RotationAngle == 180)
        {
          g.TranslateTransform(g.Page.Size.Width, g.Page.Size.Height);
          g.RotateTransform(-180f);
          layoutRectangle = new RectangleF(g.Size.Width - (layoutRectangle.X + layoutRectangle.Width), g.Size.Height - (layoutRectangle.Y + layoutRectangle.Height), layoutRectangle.Width, layoutRectangle.Height);
        }
      }
      g.DrawString(text, font, paintParams.ForeBrush, layoutRectangle, format);
      g.Restore(state);
    }
    else
      g.DrawString(text, font, paintParams.ForeBrush, layoutRectangle, format);
  }

  public static void DrawRadioButton(
    PdfGraphics g,
    PaintParams paintParams,
    string checkSymbol,
    PdfCheckFieldState state)
  {
    if (checkSymbol != "l")
    {
      FieldPainter.DrawCheckBox(g, paintParams, checkSymbol, state, (PdfFont) null);
    }
    else
    {
      switch (state)
      {
        case PdfCheckFieldState.Unchecked:
        case PdfCheckFieldState.Checked:
          g.DrawEllipse(paintParams.BackBrush, paintParams.Bounds);
          break;
        case PdfCheckFieldState.PressedUnchecked:
        case PdfCheckFieldState.PressedChecked:
          if (paintParams.BorderStyle == PdfBorderStyle.Beveled || paintParams.BorderStyle == PdfBorderStyle.Underline)
          {
            g.DrawEllipse(paintParams.BackBrush, paintParams.Bounds);
            break;
          }
          g.DrawEllipse(paintParams.ShadowBrush, paintParams.Bounds);
          break;
      }
      FieldPainter.DrawRoundBorder(g, paintParams.Bounds, paintParams.BorderPen, paintParams.BorderWidth);
      FieldPainter.DrawRoundShadow(g, paintParams, state);
      switch (state - 1)
      {
        case PdfCheckFieldState.Unchecked:
        case PdfCheckFieldState.PressedUnchecked:
          RectangleF rectangle = new RectangleF(paintParams.Bounds.X + paintParams.BorderWidth / 2f, paintParams.Bounds.Y + paintParams.BorderWidth / 2f, paintParams.Bounds.Width - paintParams.BorderWidth, paintParams.Bounds.Height - paintParams.BorderWidth);
          rectangle.X += rectangle.Width / 4f;
          rectangle.Y += rectangle.Width / 4f;
          rectangle.Height -= rectangle.Width / 2f;
          rectangle.Width -= rectangle.Width / 2f;
          g.DrawEllipse(paintParams.ForeBrush, rectangle);
          break;
      }
    }
  }

  public static void DrawSignature(PdfGraphics g, PaintParams paintParams)
  {
    if (g == null)
      throw new ArgumentNullException(nameof (g));
    if (paintParams == null)
      throw new ArgumentNullException(nameof (paintParams));
    FieldPainter.DrawRectangularControl(g, paintParams);
    RectangleF bounds = paintParams.Bounds;
    if (paintParams.BorderStyle == PdfBorderStyle.Beveled || paintParams.BorderStyle == PdfBorderStyle.Inset)
    {
      bounds.X += 4f * paintParams.BorderWidth;
      bounds.Width -= 8f * paintParams.BorderWidth;
    }
    else
    {
      bounds.X += 2f * paintParams.BorderWidth;
      bounds.Width -= 4f * paintParams.BorderWidth;
    }
  }

  public static void DrawEllipseAnnotation(
    PdfGraphics g,
    PaintParams paintParams,
    float x,
    float y,
    float width,
    float height)
  {
    g.DrawEllipse(paintParams.BorderPen, paintParams.BackBrush, x, y, width, height);
  }

  public static void DrawRectangleAnnotation(
    PdfGraphics g,
    PaintParams paintParams,
    float x,
    float y,
    float width,
    float height)
  {
    g.DrawRectangle(paintParams.BorderPen, paintParams.BackBrush, x, y, width, height);
  }

  internal static void DrawPolygonCloud(
    PdfGraphics g,
    PdfPen borderPen,
    PdfBrush backBrush,
    float intensity,
    PointF[] points,
    float borderWidth)
  {
    float radius = (float) ((double) intensity * 4.0 + 0.5 * (double) borderWidth);
    FieldPainter.DrawCloudStyle(g, backBrush, borderPen, radius, 0.833f, points, false);
  }

  internal static void DrawRectanglecloud(
    PdfGraphics g,
    PaintParams paintparams,
    RectangleF rectangle,
    float intensity,
    float borderWidth)
  {
    float radius = (float) ((double) intensity * 4.0 + 0.5 * (double) borderWidth);
    GraphicsPath graphicsPath = new GraphicsPath();
    graphicsPath.AddRectangle(rectangle);
    graphicsPath.CloseFigure();
    FieldPainter.DrawCloudStyle(g, paintparams.BackBrush, paintparams.BorderPen, radius, 0.833f, graphicsPath.PathPoints, false);
  }

  private static void DrawCloudStyle(
    PdfGraphics g,
    PdfBrush brush,
    PdfPen pen,
    float radius,
    float overlap,
    PointF[] points,
    bool isAppearance)
  {
    if (FieldPainter.IsClockWise(points))
    {
      PointF[] pointFArray = new PointF[points.Length];
      int index1 = points.Length - 1;
      int index2 = 0;
      while (index1 >= 0)
      {
        pointFArray[index2] = points[index1];
        --index1;
        ++index2;
      }
      points = pointFArray;
    }
    List<FieldPainter.CloudStyleArc> cloudStyleArcList = new List<FieldPainter.CloudStyleArc>();
    float num1 = 2f * radius * overlap;
    PointF pointF = points[points.Length - 1];
    for (int index = 0; index < points.Length; ++index)
    {
      PointF point = points[index];
      float num2 = point.X - pointF.X;
      float num3 = point.Y - pointF.Y;
      float num4 = (float) Math.Sqrt((double) num2 * (double) num2 + (double) num3 * (double) num3);
      float num5 = num2 / num4;
      float num6 = num3 / num4;
      float num7 = num1;
      for (float num8 = 0.0f; (double) num8 + 0.1 * (double) num7 < (double) num4; num8 += num7)
        cloudStyleArcList.Add(new FieldPainter.CloudStyleArc()
        {
          point = new PointF(pointF.X + num8 * num5, pointF.Y + num8 * num6)
        });
      pointF = point;
    }
    new GraphicsPath().AddPolygon(points);
    FieldPainter.CloudStyleArc cloudStyleArc1 = cloudStyleArcList[cloudStyleArcList.Count - 1];
    for (int index = 0; index < cloudStyleArcList.Count; ++index)
    {
      FieldPainter.CloudStyleArc cloudStyleArc2 = cloudStyleArcList[index];
      PointF intersectionDegrees = FieldPainter.GetIntersectionDegrees(cloudStyleArc1.point, cloudStyleArc2.point, radius);
      cloudStyleArc1.endAngle = intersectionDegrees.X;
      cloudStyleArc2.startAngle = intersectionDegrees.Y;
      cloudStyleArc1 = cloudStyleArc2;
    }
    GraphicsPath graphicsPath1 = new GraphicsPath();
    for (int index = 0; index < cloudStyleArcList.Count; ++index)
    {
      FieldPainter.CloudStyleArc cloudStyleArc3 = cloudStyleArcList[index];
      float startAngle = cloudStyleArc3.startAngle % 360f;
      float num9 = cloudStyleArc3.endAngle % 360f;
      float sweepAngle = 0.0f;
      if ((double) startAngle > 0.0 && (double) num9 < 0.0)
        sweepAngle = (float) (180.0 - (double) startAngle + (180.0 - ((double) num9 < 0.0 ? -(double) num9 : (double) num9)));
      else if ((double) startAngle < 0.0 && (double) num9 > 0.0)
        sweepAngle = -startAngle + num9;
      else if ((double) startAngle > 0.0 && (double) num9 > 0.0)
        sweepAngle = (double) startAngle <= (double) num9 ? num9 - startAngle : 360f - (startAngle - num9);
      else if ((double) startAngle < 0.0 && (double) num9 < 0.0)
        sweepAngle = (double) startAngle <= (double) num9 ? (float) -((double) startAngle + -(double) num9) : 360f - (startAngle - num9);
      if ((double) sweepAngle < 0.0)
        sweepAngle = -sweepAngle;
      cloudStyleArc3.endAngle = sweepAngle;
      graphicsPath1.AddArc(new RectangleF(cloudStyleArc3.point.X - radius, cloudStyleArc3.point.Y - radius, 2f * radius, 2f * radius), startAngle, sweepAngle);
    }
    graphicsPath1.CloseFigure();
    PointF[] points1 = new PointF[graphicsPath1.PathPoints.Length];
    if (isAppearance)
    {
      for (int index = 0; index < graphicsPath1.PathPoints.Length; ++index)
        points1[index] = new PointF(graphicsPath1.PathPoints[index].X, -graphicsPath1.PathPoints[index].Y);
    }
    PdfPath path1 = !isAppearance ? new PdfPath(graphicsPath1.PathPoints, graphicsPath1.PathTypes) : new PdfPath(points1, graphicsPath1.PathTypes);
    if (brush != null)
      g.DrawPath(brush, path1);
    float num10 = 19.0985928f;
    GraphicsPath graphicsPath2 = new GraphicsPath();
    for (int index = 0; index < cloudStyleArcList.Count; ++index)
    {
      FieldPainter.CloudStyleArc cloudStyleArc4 = cloudStyleArcList[index];
      graphicsPath2.AddArc(new RectangleF(cloudStyleArc4.point.X - radius, cloudStyleArc4.point.Y - radius, 2f * radius, 2f * radius), cloudStyleArc4.startAngle, cloudStyleArc4.endAngle + num10);
    }
    graphicsPath2.CloseFigure();
    PointF[] points2 = new PointF[graphicsPath2.PathPoints.Length];
    if (isAppearance)
    {
      for (int index = 0; index < graphicsPath2.PathPoints.Length; ++index)
        points2[index] = new PointF(graphicsPath2.PathPoints[index].X, -graphicsPath2.PathPoints[index].Y);
    }
    PdfPath path2 = !isAppearance ? new PdfPath(graphicsPath2.PathPoints, graphicsPath2.PathTypes) : new PdfPath(points2, graphicsPath2.PathTypes);
    g.DrawPath(pen, path2);
  }

  private static bool IsClockWise(PointF[] points)
  {
    double num = 0.0;
    for (int index = 0; index < points.Length; ++index)
    {
      PointF point1 = points[index];
      PointF point2 = points[(index + 1) % points.Length];
      num += ((double) point2.X - (double) point1.X) * ((double) point2.Y + (double) point1.Y);
    }
    return num > 0.0;
  }

  private static PointF GetIntersectionDegrees(PointF point1, PointF point2, float radius)
  {
    float x = point2.X - point1.X;
    float y = point2.Y - point1.Y;
    float d = 0.5f * (float) Math.Sqrt((double) x * (double) x + (double) y * (double) y) / radius;
    if ((double) d < -1.0)
      d = -1f;
    if ((double) d > 1.0)
      d = 1f;
    float num1 = (float) Math.Atan2((double) y, (double) x);
    float num2 = (float) Math.Acos((double) d);
    return new PointF((float) (((double) num1 - (double) num2) * (180.0 / Math.PI)), (float) ((Math.PI + (double) num1 + (double) num2) * (180.0 / Math.PI)));
  }

  public static void DrawFreeTextAnnotation(
    PdfGraphics g,
    PaintParams paintParams,
    string text,
    PdfFont font,
    RectangleF rect,
    bool isSkipDrawRectangle,
    PdfTextAlignment alignment,
    bool isRotation)
  {
    if (!isSkipDrawRectangle)
    {
      if (isRotation)
        g.DrawRectangle(paintParams.BorderPen, paintParams.ForeBrush, rect);
      else
        g.DrawRectangle(paintParams.BorderPen, paintParams.ForeBrush, paintParams.Bounds.X, paintParams.Bounds.Y, paintParams.Bounds.Width, paintParams.Bounds.Height);
    }
    PdfStringFormat format = new PdfStringFormat();
    format.WordWrap = PdfWordWrapType.Word;
    format.LineAlignment = PdfVerticalAlignment.Top;
    format.ComplexScript = paintParams.m_complexScript;
    format.TextDirection = paintParams.m_textDirection;
    format.Alignment = alignment;
    format.LineSpacing = paintParams.m_lineSpacing;
    if (isRotation)
      g.DrawString(text, font, paintParams.BackBrush, paintParams.Bounds.Location, format);
    else
      g.DrawString(text, font, paintParams.BackBrush, rect, format);
  }

  private static void DrawBorder(
    PdfGraphics g,
    RectangleF bounds,
    PdfPen borderPen,
    PdfBorderStyle style,
    float borderWidth)
  {
    if (borderPen == null || (double) borderWidth <= 0.0 || borderPen.Color.IsEmpty)
      return;
    if (style == PdfBorderStyle.Underline)
    {
      g.DrawLine(borderPen, bounds.X, (float) ((double) bounds.Y + (double) bounds.Height - (double) borderWidth / 2.0), bounds.X + bounds.Width, (float) ((double) bounds.Y + (double) bounds.Height - (double) borderWidth / 2.0));
    }
    else
    {
      RectangleF rectangle = new RectangleF(bounds.X + borderWidth / 2f, bounds.Y + borderWidth / 2f, bounds.Width - borderWidth, bounds.Height - borderWidth);
      g.DrawRectangle(borderPen, rectangle);
    }
  }

  private static void DrawRoundBorder(
    PdfGraphics g,
    RectangleF bounds,
    PdfPen borderPen,
    float borderWidth)
  {
    RectangleF rectangle = bounds;
    if (!(rectangle != RectangleF.Empty))
      return;
    rectangle = new RectangleF(bounds.X + borderWidth / 2f, bounds.Y + borderWidth / 2f, bounds.Width - borderWidth, bounds.Height - borderWidth);
    g.DrawEllipse(borderPen, rectangle);
  }

  private static void DrawRectangularControl(PdfGraphics g, PaintParams paintParams)
  {
    g.DrawRectangle(paintParams.BackBrush, paintParams.Bounds);
    FieldPainter.DrawBorder(g, paintParams.Bounds, paintParams.BorderPen, paintParams.BorderStyle, paintParams.BorderWidth);
    switch (paintParams.BorderStyle)
    {
      case PdfBorderStyle.Beveled:
        FieldPainter.DrawLeftTopShadow(g, paintParams.Bounds, paintParams.BorderWidth, FieldPainter.WhiteBrush);
        FieldPainter.DrawRightBottomShadow(g, paintParams.Bounds, paintParams.BorderWidth, paintParams.ShadowBrush);
        break;
      case PdfBorderStyle.Inset:
        FieldPainter.DrawLeftTopShadow(g, paintParams.Bounds, paintParams.BorderWidth, FieldPainter.GrayBrush);
        FieldPainter.DrawRightBottomShadow(g, paintParams.Bounds, paintParams.BorderWidth, FieldPainter.SilverBrush);
        break;
    }
  }

  private static void DrawLeftTopShadow(
    PdfGraphics g,
    RectangleF bounds,
    float width,
    PdfBrush brush)
  {
    PdfPath path = new PdfPath();
    PointF[] points = new PointF[6]
    {
      new PointF(bounds.X + width, bounds.Y + width),
      new PointF(bounds.X + width, bounds.Bottom - width),
      new PointF(bounds.X + 2f * width, bounds.Bottom - 2f * width),
      new PointF(bounds.X + 2f * width, bounds.Y + 2f * width),
      new PointF(bounds.Right - 2f * width, bounds.Y + 2f * width),
      new PointF(bounds.Right - width, bounds.Y + width)
    };
    path.AddPolygon(points);
    g.DrawPath(brush, path);
  }

  private static void DrawRightBottomShadow(
    PdfGraphics g,
    RectangleF bounds,
    float width,
    PdfBrush brush)
  {
    PdfPath path = new PdfPath();
    PointF[] points = new PointF[6]
    {
      new PointF(bounds.X + width, bounds.Bottom - width),
      new PointF(bounds.X + 2f * width, bounds.Bottom - 2f * width),
      new PointF(bounds.Right - 2f * width, bounds.Bottom - 2f * width),
      new PointF(bounds.Right - 2f * width, bounds.Y + 2f * width),
      new PointF(bounds.X + bounds.Width - width, bounds.Y + width),
      new PointF(bounds.Right - width, bounds.Bottom - width)
    };
    path.AddPolygon(points);
    g.DrawPath(brush, path);
  }

  private static void DrawRoundShadow(
    PdfGraphics g,
    PaintParams paintParams,
    PdfCheckFieldState state)
  {
    float borderWidth = paintParams.BorderWidth;
    RectangleF bounds = paintParams.Bounds;
    bounds.Inflate(-1.5f * borderWidth, -1.5f * borderWidth);
    PdfPen pen1 = (PdfPen) null;
    PdfPen pen2 = (PdfPen) null;
    PdfColor color = ((PdfSolidBrush) paintParams.ShadowBrush).Color;
    switch (paintParams.BorderStyle)
    {
      case PdfBorderStyle.Beveled:
        switch (state)
        {
          case PdfCheckFieldState.Unchecked:
          case PdfCheckFieldState.Checked:
            pen1 = FieldPainter.GetPen(new PdfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue), borderWidth);
            pen2 = FieldPainter.GetPen(color, borderWidth);
            break;
          case PdfCheckFieldState.PressedUnchecked:
          case PdfCheckFieldState.PressedChecked:
            pen1 = FieldPainter.GetPen(color, borderWidth);
            pen2 = FieldPainter.GetPen(new PdfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue), borderWidth);
            break;
        }
        break;
      case PdfBorderStyle.Inset:
        switch (state)
        {
          case PdfCheckFieldState.Unchecked:
          case PdfCheckFieldState.Checked:
            pen1 = FieldPainter.GetPen(new PdfColor(byte.MaxValue, (byte) 128 /*0x80*/, (byte) 128 /*0x80*/, (byte) 128 /*0x80*/), borderWidth);
            pen2 = FieldPainter.GetPen(new PdfColor(byte.MaxValue, (byte) 192 /*0xC0*/, (byte) 192 /*0xC0*/, (byte) 192 /*0xC0*/), borderWidth);
            break;
          case PdfCheckFieldState.PressedUnchecked:
          case PdfCheckFieldState.PressedChecked:
            pen1 = FieldPainter.GetPen(new PdfColor((byte) 0, (byte) 0, (byte) 0), borderWidth);
            pen2 = FieldPainter.GetPen(new PdfColor((byte) 0, (byte) 0, (byte) 0), borderWidth);
            break;
        }
        break;
    }
    if (pen1 == null || pen2 == null)
      return;
    g.DrawArc(pen1, bounds, 135f, 180f);
    g.DrawArc(pen2, bounds, -45f, 180f);
  }

  private static PdfPen GetPen(PdfColor color, float width)
  {
    lock (FieldPainter.s_pens)
    {
      string key = $"{color}{width}";
      PdfPen pen = FieldPainter.s_pens.ContainsKey(key) ? FieldPainter.s_pens[key] : (PdfPen) null;
      if (pen == null)
      {
        pen = new PdfPen(color, width);
        FieldPainter.s_pens[key] = pen;
      }
      return pen;
    }
  }

  internal class CloudStyleArc
  {
    internal PointF point;
    internal float endAngle;
    internal float startAngle;
  }
}
