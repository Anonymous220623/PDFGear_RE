// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.PIGraphicsSurface
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal abstract class PIGraphicsSurface
{
  public abstract void Initialize(float width, float height);

  public abstract PIGraphicsState Save();

  public abstract void Restore();

  public abstract void Restore(PIGraphicsState state);

  public abstract void TranslateTransform(float dx, float dy);

  public abstract void ScaleTransform(float sx, float sy);

  public abstract void RotateTransform(float angle);

  public abstract void DrawPath(PIPen pen, PIPath path);

  public abstract void FillPath(PIBrush brush, PIPath path);

  public abstract void DrawRectangle(PIPen pen, float x, float y, float width, float height);

  public abstract void FillRectangle(PIBrush brush, float x, float y, float width, float height);

  public abstract void DrawLine(PIPen pen, float x1, float y1, float x2, float y2);
}
