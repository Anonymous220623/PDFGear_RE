// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.BorderProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class BorderProperties
{
  internal const ushort BorderKey = 0;
  internal const ushort BorderTopKey = 1;
  internal const ushort BorderBottomKey = 2;
  internal const ushort BorderLeftKey = 3;
  internal const ushort BorderRightKey = 4;
  private ODFBorder m_border;
  private ODFBorder m_borderTop;
  private ODFBorder m_borderBottom;
  private ODFBorder m_borderLeft;
  private ODFBorder m_borderRight;
  private ODFBorder m_diagonalLeft;
  private ODFBorder m_diagonalRight;
  internal byte borderFlags;

  internal ODFBorder Border
  {
    get => this.m_border;
    set
    {
      this.borderFlags = (byte) ((int) this.borderFlags & 254 | 1);
      this.m_border = value;
    }
  }

  internal ODFBorder BorderTop
  {
    get => this.m_borderTop;
    set
    {
      this.borderFlags = (byte) ((int) this.borderFlags & 253 | 2);
      this.m_borderTop = value;
    }
  }

  internal ODFBorder BorderBottom
  {
    get => this.m_borderBottom;
    set
    {
      this.borderFlags = (byte) ((int) this.borderFlags & 251 | 4);
      this.m_borderBottom = value;
    }
  }

  internal ODFBorder BorderLeft
  {
    get => this.m_borderLeft;
    set
    {
      this.borderFlags = (byte) ((int) this.borderFlags & 247 | 8);
      this.m_borderLeft = value;
    }
  }

  internal ODFBorder BorderRight
  {
    get => this.m_borderRight;
    set
    {
      this.borderFlags = (byte) ((int) this.borderFlags & 239 | 16 /*0x10*/);
      this.m_borderRight = value;
    }
  }

  internal ODFBorder DiagonalLeft
  {
    get => this.m_diagonalLeft;
    set => this.m_diagonalLeft = value;
  }

  internal ODFBorder DiagonalRight
  {
    get => this.m_diagonalRight;
    set => this.m_diagonalRight = value;
  }

  internal void Dispose()
  {
    if (this.m_border != null)
      this.m_border = (ODFBorder) null;
    if (this.m_borderBottom != null)
      this.m_borderBottom = (ODFBorder) null;
    if (this.m_borderLeft != null)
      this.m_borderLeft = (ODFBorder) null;
    if (this.m_borderRight != null)
      this.m_borderRight = (ODFBorder) null;
    if (this.m_borderTop != null)
      this.m_borderTop = (ODFBorder) null;
    if (this.m_diagonalLeft != null)
      this.m_diagonalLeft = (ODFBorder) null;
    if (this.m_diagonalRight == null)
      return;
    this.m_diagonalRight = (ODFBorder) null;
  }
}
