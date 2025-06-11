// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.Sel
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class Sel
{
  private int m_sy;
  private int m_sx;
  private int m_cy;
  private int m_cx;
  private List<int[]> m_data;
  private string m_name;

  internal int SY
  {
    get => this.m_sy;
    set => this.m_sy = value;
  }

  internal int SX
  {
    get => this.m_sx;
    set => this.m_sx = value;
  }

  internal int CY
  {
    get => this.m_cy;
    set => this.m_cy = value;
  }

  internal int CX
  {
    get => this.m_cx;
    set => this.m_cx = value;
  }

  internal List<int[]> Data
  {
    get => this.m_data;
    set => this.m_data = value;
  }

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }
}
