// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedEQFields
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedEQFields
{
  private List<LayoutedEQFields> m_childEQFileds;
  private RectangleF m_bounds;
  private LayoutedEQFields.EQSwitchType m_switchType;
  private StringAlignment m_alignment;

  internal RectangleF Bounds
  {
    get => this.m_bounds;
    set => this.m_bounds = value;
  }

  internal List<LayoutedEQFields> ChildEQFileds
  {
    get
    {
      if (this.m_childEQFileds == null)
        this.m_childEQFileds = new List<LayoutedEQFields>();
      return this.m_childEQFileds;
    }
    set => this.m_childEQFileds = value;
  }

  internal LayoutedEQFields.EQSwitchType SwitchType
  {
    get => this.m_switchType;
    set => this.m_switchType = value;
  }

  internal StringAlignment Alignment
  {
    get => this.m_alignment;
    set => this.m_alignment = value;
  }

  internal enum EQSwitchType
  {
    Array = 1,
    Bracket = 2,
    Displace = 3,
    Fraction = 4,
    Integral = 5,
    List = 6,
    Overstrike = 7,
    Radical = 8,
    Superscript = 9,
    Subscript = 10, // 0x0000000A
    Box = 11, // 0x0000000B
  }
}
