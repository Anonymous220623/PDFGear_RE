// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.POINTS
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Native;

internal struct POINTS(short X, short Y)
{
  public short x = X;
  public short y = Y;

  public static implicit operator Point(POINTS p) => new Point((int) p.x, (int) p.y);

  public static implicit operator PointF(POINTS p) => new PointF((float) p.x, (float) p.y);

  public static implicit operator POINTS(Point p) => new POINTS((short) p.X, (short) p.Y);

  public static implicit operator POINTS(PointF p) => new POINTS((short) p.X, (short) p.Y);
}
