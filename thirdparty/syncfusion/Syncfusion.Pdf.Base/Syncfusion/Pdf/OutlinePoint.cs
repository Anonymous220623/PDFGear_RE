// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.OutlinePoint
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

internal class OutlinePoint
{
  private PointF m_point;
  private byte m_flags;

  public PointF Point
  {
    get => this.m_point;
    set => this.m_point = value;
  }

  public byte Flags
  {
    get => this.m_flags;
    set => this.m_flags = value;
  }

  public bool IsOnCurve => ((int) this.Flags & 1) != 0;

  public OutlinePoint(double x, double y, byte flags)
  {
    this.m_point = new PointF((float) x, (float) y);
    this.m_flags = flags;
  }

  public OutlinePoint(byte flags) => this.m_flags = flags;
}
