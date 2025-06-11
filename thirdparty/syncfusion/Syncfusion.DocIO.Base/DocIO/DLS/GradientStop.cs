// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.GradientStop
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class GradientStop
{
  private byte m_position = byte.MaxValue;
  private Color m_color;
  private byte m_opacity = byte.MaxValue;
  private List<DictionaryEntry> m_fillSchemeColor;

  internal byte Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  internal Color Color
  {
    get => this.m_color;
    set => this.m_color = value;
  }

  internal byte Opacity
  {
    get => this.m_opacity;
    set => this.m_opacity = value;
  }

  internal List<DictionaryEntry> FillSchemeColorTransforms
  {
    get
    {
      if (this.m_fillSchemeColor == null)
        this.m_fillSchemeColor = new List<DictionaryEntry>();
      return this.m_fillSchemeColor;
    }
    set => this.m_fillSchemeColor = value;
  }

  internal GradientStop()
  {
  }

  internal GradientStop Clone() => (GradientStop) this.MemberwiseClone();
}
