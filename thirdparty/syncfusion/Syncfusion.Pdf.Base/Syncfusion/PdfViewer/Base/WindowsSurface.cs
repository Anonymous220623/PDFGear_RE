// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.WindowsSurface
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class WindowsSurface : PIGraphicsSurface
{
  private Graphics _graphics;

  public override void Initialize(float width, float height)
  {
    this._graphics = Graphics.FromImage((Image) new Bitmap((int) width, (int) height));
    this._graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, (int) width, (int) height));
  }

  public override PIGraphicsState Save() => throw new NotImplementedException();

  public override void Restore() => throw new NotImplementedException();

  public override void Restore(PIGraphicsState state) => throw new NotImplementedException();

  public override void TranslateTransform(float dx, float dy)
  {
    throw new NotImplementedException();
  }

  public override void ScaleTransform(float sx, float sy) => throw new NotImplementedException();

  public override void RotateTransform(float angle) => throw new NotImplementedException();

  public override void DrawPath(PIPen pen, PIPath path) => throw new NotImplementedException();

  public override void FillPath(PIBrush brush, PIPath path) => throw new NotImplementedException();

  public override void DrawRectangle(PIPen pen, float x, float y, float width, float height)
  {
    throw new NotImplementedException();
  }

  public override void FillRectangle(PIBrush brush, float x, float y, float width, float height)
  {
    throw new NotImplementedException();
  }

  public override void DrawLine(PIPen pen, float x1, float y1, float x2, float y2)
  {
    throw new NotImplementedException();
  }
}
