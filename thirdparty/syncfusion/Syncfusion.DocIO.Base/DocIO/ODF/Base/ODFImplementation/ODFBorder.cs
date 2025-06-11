// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.ODFBorder
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class ODFBorder
{
  private const byte LineColorKey = 0;
  private const byte LineStyleKey = 1;
  private const byte LineWidthKey = 2;
  private Color m_borderColor;
  private BorderLineStyle m_lineStyle;
  private string m_lineWidth;
  internal byte styleFlags;

  internal Color LineColor
  {
    get => this.m_borderColor;
    set
    {
      this.styleFlags = (byte) ((int) this.styleFlags & 254 | 1);
      this.m_borderColor = value;
    }
  }

  internal BorderLineStyle LineStyle
  {
    get => this.m_lineStyle;
    set
    {
      this.styleFlags = (byte) ((int) this.styleFlags & 253 | 2);
      this.m_lineStyle = value;
    }
  }

  internal string LineWidth
  {
    get => this.m_lineWidth;
    set
    {
      this.styleFlags = (byte) ((int) this.styleFlags & 251 | 4);
      this.m_lineWidth = value;
    }
  }

  internal bool HasKey(int propertyKey)
  {
    return ((int) this.styleFlags & (int) (ushort) Math.Pow(2.0, (double) propertyKey)) >> propertyKey != 0;
  }

  public override bool Equals(object obj)
  {
    ODFBorder odfBorder = obj as ODFBorder;
    bool flag = false;
    if (odfBorder == null)
      return false;
    if (this.HasKey(0) && odfBorder.HasKey(0))
    {
      flag = this.LineColor.Equals((object) odfBorder.LineColor);
      if (!flag)
        return flag;
    }
    if (this.HasKey(1) && odfBorder.HasKey(1))
    {
      flag = this.LineStyle.Equals((object) odfBorder.LineStyle);
      if (!flag)
        return flag;
    }
    if (this.HasKey(2) && odfBorder.HasKey(2))
    {
      flag = this.LineWidth.Equals(odfBorder.LineWidth);
      if (!flag)
        return flag;
    }
    return flag;
  }
}
