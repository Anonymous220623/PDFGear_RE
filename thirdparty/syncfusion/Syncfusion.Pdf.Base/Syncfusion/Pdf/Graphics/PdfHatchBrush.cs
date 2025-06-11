// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfHatchBrush
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class PdfHatchBrush : PdfTilingBrush
{
  private PdfHatchStyle m_hatchStyle;
  private PdfColor m_foreColor = PdfColor.Empty;
  private PdfColor m_backColor = PdfColor.Empty;

  internal PdfColor BackColor => this.m_backColor;

  public PdfHatchBrush(PdfHatchStyle hatchstyle, PdfColor foreColor)
    : base(new SizeF(8f, 8f))
  {
    this.m_hatchStyle = hatchstyle;
    this.m_foreColor = foreColor;
    this.CreateHatchBrush();
  }

  public PdfHatchBrush(PdfHatchStyle hatchstyle, PdfColor foreColor, PdfColor backColor)
    : base(new SizeF(8f, 8f))
  {
    this.m_hatchStyle = hatchstyle;
    this.m_foreColor = foreColor;
    this.m_backColor = backColor;
    this.CreateHatchBrush();
  }

  private void CreateHatchBrush()
  {
    PdfGraphics graphics = this.Graphics;
    PdfPen pen = new PdfPen(this.m_foreColor, 1f);
    SizeF sizeF = new SizeF(8f, 8f);
    switch (this.m_hatchStyle)
    {
      case PdfHatchStyle.Horizontal:
        PdfHatchBrush.DrawHorizontal(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.ForwardDiagonal:
        PdfHatchBrush.DrawForwardDiagonal(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.BackwardDiagonal:
        PdfHatchBrush.DrawBackwardDiagonal(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.Cross:
        PdfHatchBrush.DrawCross(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.DiagonalCross:
        PdfHatchBrush.DrawForwardDiagonal(graphics, pen, sizeF);
        PdfHatchBrush.DrawBackwardDiagonal(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.LightDownwardDiagonal:
        PdfHatchBrush.DrawDownwardDiagonal(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.LightUpwardDiagonal:
        PdfHatchBrush.DrawUpwardDiagonal(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.DarkDownwardDiagonal:
        pen.Width = 2f;
        PdfHatchBrush.DrawDownwardDiagonal(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.DarkUpwardDiagonal:
        pen.Width = 2f;
        PdfHatchBrush.DrawUpwardDiagonal(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.LightVertical:
        PdfHatchBrush.DrawVertical(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.LightHorizontal:
        PdfHatchBrush.DrawHorizontal(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.DarkVertical:
        pen.Width = 2f;
        PdfHatchBrush.DrawVertical(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.DarkHorizontal:
        pen.Width = 2f;
        PdfHatchBrush.DrawHorizontal(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.DashedDownwardDiagonal:
        pen.DashStyle = PdfDashStyle.Dash;
        PdfHatchBrush.DrawDownwardDiagonal(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.DashedUpwardDiagonal:
        pen.DashStyle = PdfDashStyle.Dash;
        PdfHatchBrush.DrawUpwardDiagonal(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.DashedHorizontal:
        pen.DashStyle = PdfDashStyle.Dash;
        PdfHatchBrush.DrawHorizontal(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.DashedVertical:
        pen.DashStyle = PdfDashStyle.Dash;
        PdfHatchBrush.DrawVertical(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.LargeConfetti:
        break;
      case PdfHatchStyle.DiagonalBrick:
        PdfHatchBrush.DrawForwardDiagonal(graphics, pen, sizeF);
        PdfHatchBrush.DrawBrickTails(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.HorizontalBrick:
        PdfHatchBrush.DrawHorizontalBrick(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.Weave:
        this.DrawWeave(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.Divot:
        break;
      case PdfHatchStyle.DottedGrid:
        pen.DashStyle = PdfDashStyle.Dot;
        PdfHatchBrush.DrawCross(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.DottedDiamond:
        pen.DashStyle = PdfDashStyle.Dot;
        PdfHatchBrush.DrawForwardDiagonal(graphics, pen, sizeF);
        PdfHatchBrush.DrawBackwardDiagonal(graphics, pen, sizeF);
        break;
      case PdfHatchStyle.LargeCheckerBoard:
        PdfHatchBrush.DrawCheckerBoard(graphics, pen, sizeF, 4);
        break;
      default:
        PdfGraphicsState state = graphics.Save();
        graphics.SetTransparency(0.5f);
        graphics.DrawRectangle((PdfBrush) new PdfSolidBrush(this.m_foreColor), new RectangleF(PointF.Empty, sizeF));
        graphics.Restore(state);
        break;
    }
  }

  private static void DrawCross(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    float num1 = brushSize.Width / 2f;
    float num2 = brushSize.Height / 2f;
    graphics.DrawLine(pen, num1, 0.0f, num1, brushSize.Height);
    graphics.DrawLine(pen, 0.0f, num2, brushSize.Width, num2);
  }

  private static void DrawBackwardDiagonal(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    graphics.DrawLine(pen, brushSize.Width, 0.0f, 0.0f, brushSize.Height);
    graphics.DrawLine(pen, -1f, 1f, 1f, -1f);
    graphics.DrawLine(pen, brushSize.Width - 1f, brushSize.Height + 1f, brushSize.Width + 1f, brushSize.Height - 1f);
  }

  private static void DrawForwardDiagonal(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    graphics.DrawLine(pen, 0.0f, 0.0f, brushSize.Width, brushSize.Height);
    graphics.DrawLine(pen, -1f, -1f, 1f, 1f);
    graphics.DrawLine(pen, brushSize.Width - 1f, brushSize.Height - 1f, brushSize.Width + 1f, brushSize.Height + 1f);
  }

  private static void DrawHorizontal(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    float num1 = 0.0f;
    float num2 = brushSize.Height / 2f;
    float height = brushSize.Height;
    graphics.DrawLine(pen, 0.0f, num1, brushSize.Width, num1);
    graphics.DrawLine(pen, 0.0f, num2, brushSize.Width, num2);
    graphics.DrawLine(pen, 0.0f, height, brushSize.Width, height);
  }

  private static void DrawVertical(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    float num1 = 0.0f;
    float num2 = brushSize.Height / 2f;
    float height = brushSize.Height;
    graphics.DrawLine(pen, num1, 0.0f, num1, brushSize.Height);
    graphics.DrawLine(pen, num2, 0.0f, num2, brushSize.Height);
    graphics.DrawLine(pen, height, 0.0f, height, brushSize.Height);
  }

  private static void DrawDownwardDiagonal(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    float num1 = brushSize.Height / 2f;
    float num2 = brushSize.Width / 2f;
    graphics.DrawLine(pen, 0.0f, 0.0f, brushSize.Width, brushSize.Height);
    graphics.DrawLine(pen, 0.0f, num1, num2, brushSize.Height);
    graphics.DrawLine(pen, num2, 0.0f, brushSize.Width, num1);
    graphics.DrawLine(pen, -1f, -1f, 1f, 1f);
    graphics.DrawLine(pen, brushSize.Width - 1f, brushSize.Height - 1f, brushSize.Width + 1f, brushSize.Height + 1f);
  }

  private void DrawWeave(PdfGraphics g, PdfPen pen, SizeF brushSize)
  {
    g.TranslateTransform(-0.5f, -0.5f);
    g.DrawLine(pen, new PointF(0.0f, 0.0f), new PointF(0.5f, 0.5f));
    g.DrawLine(pen, new PointF(0.0f, 1f), new PointF(1f, 0.0f));
    g.DrawLine(pen, new PointF(0.0f, 5f), new PointF(5f, 0.0f));
    g.DrawLine(pen, new PointF(0.0f, 4f), new PointF(5f, 9f));
    g.DrawLine(pen, new PointF(2.5f, 2.5f), new PointF(9f, 9f));
    g.DrawLine(pen, new PointF(4f, 0.0f), new PointF(6.5f, 2.5f));
    g.DrawLine(pen, new PointF((float) (6.5 - Math.Sqrt(0.125)), (float) (2.5 + Math.Sqrt(0.125))), new PointF(9f, 0.0f));
    g.DrawLine(pen, new PointF(6.5f, 6.5f), new PointF(9f, 4f));
    g.DrawLine(pen, new PointF(2.5f, 6.5f), new PointF(0.5f, 8.5f));
  }

  private static void DrawUpwardDiagonal(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    float num1 = brushSize.Height / 2f;
    float num2 = brushSize.Width / 2f;
    graphics.DrawLine(pen, brushSize.Width, 0.0f, 0.0f, brushSize.Height);
    graphics.DrawLine(pen, 0.0f, num1, num2, 0.0f);
    graphics.DrawLine(pen, num2, brushSize.Height, brushSize.Width, num1);
    graphics.DrawLine(pen, -1f, 1f, 1f, -1f);
    graphics.DrawLine(pen, brushSize.Width - 1f, brushSize.Height + 1f, brushSize.Width + 1f, brushSize.Height - 1f);
  }

  private static void DrawBrickTails(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    float x1 = brushSize.Width / 2f;
    float y1 = brushSize.Height / 2f;
    graphics.DrawLine(pen, x1, y1, brushSize.Width, brushSize.Height);
  }

  private static void DrawHorizontalBrick(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    float num1 = brushSize.Width / 2f;
    float num2 = brushSize.Height / 2f;
    graphics.DrawLine(pen, 0.0f, 0.0f, brushSize.Width, 0.0f);
    graphics.DrawLine(pen, 0.0f, brushSize.Height, brushSize.Width, brushSize.Height);
    graphics.DrawLine(pen, 0.0f, num2, brushSize.Width, num2);
    graphics.DrawLine(pen, num1, 0.0f, num1, num2);
    graphics.DrawLine(pen, 0.0f, num2, 0.0f, brushSize.Height);
    graphics.DrawLine(pen, brushSize.Width, num2, brushSize.Width, brushSize.Height);
  }

  private static void DrawCheckerBoard(
    PdfGraphics graphics,
    PdfPen pen,
    SizeF brushSize,
    int cellSize)
  {
    int num1 = (int) ((double) brushSize.Width / (double) cellSize);
    int num2 = (int) ((double) brushSize.Height / (double) cellSize);
    PdfSolidBrush brush = new PdfSolidBrush(pen.Color);
    for (int index1 = 0; index1 < num2; ++index1)
    {
      float y = (float) (index1 * cellSize);
      for (int index2 = 0; index2 < num1; ++index2)
      {
        float x = (float) (index2 * cellSize);
        graphics.DrawRectangle((PdfBrush) brush, x, y, (float) cellSize, (float) cellSize);
      }
    }
  }
}
