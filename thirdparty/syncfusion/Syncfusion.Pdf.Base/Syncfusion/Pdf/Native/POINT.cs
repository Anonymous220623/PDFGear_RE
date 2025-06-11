// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.POINT
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Native;

internal struct POINT
{
  public int x;
  public int y;

  public POINT(int X, int Y)
  {
    this.x = X;
    this.y = Y;
  }

  public POINT(int lParam)
  {
    this.x = lParam & (int) ushort.MaxValue;
    this.y = lParam >> 16 /*0x10*/;
  }

  public static implicit operator Point(POINT p) => new Point(p.x, p.y);

  public static implicit operator PointF(POINT p) => new PointF((float) p.x, (float) p.y);

  public static implicit operator POINT(Point p) => new POINT(p.X, p.Y);
}
