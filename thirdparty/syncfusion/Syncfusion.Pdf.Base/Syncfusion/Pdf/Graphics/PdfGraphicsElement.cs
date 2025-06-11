// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfGraphicsElement
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public abstract class PdfGraphicsElement
{
  public void Draw(PdfGraphics graphics)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    this.Draw(graphics, PointF.Empty);
  }

  public void Draw(PdfGraphics graphics, PointF location)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    this.Draw(graphics, location.X, location.Y);
  }

  public virtual void Draw(PdfGraphics graphics, float x, float y)
  {
    bool flag = (double) x != 0.0 || (double) y != 0.0;
    PdfGraphicsState state = (PdfGraphicsState) null;
    if (flag)
    {
      state = graphics.Save();
      graphics.TranslateTransform(x, y);
    }
    this.DrawInternal(graphics);
    if (!flag)
      return;
    graphics.Restore(state);
  }

  protected abstract void DrawInternal(PdfGraphics graphics);
}
