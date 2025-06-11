// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.Pix
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class Pix
{
  private int m_w;
  private int m_h;
  private int m_d;
  private int m_wpl;
  private int m_xRes;
  private int m_yRes;
  private int m_informat;
  private char m_text;
  private PixColormap m_colormap;
  private uint[] m_data;

  internal int W
  {
    get => this.m_w;
    set => this.m_w = value;
  }

  internal int H
  {
    get => this.m_h;
    set => this.m_h = value;
  }

  internal int D
  {
    get => this.m_d;
    set => this.m_d = value;
  }

  internal int Wpl
  {
    get => this.m_wpl;
    set => this.m_wpl = value;
  }

  internal int XRes
  {
    get => this.m_xRes;
    set => this.m_xRes = value;
  }

  internal int YRes
  {
    get => this.m_yRes;
    set => this.m_yRes = value;
  }

  internal int Informat
  {
    get => this.m_informat;
    set => this.m_informat = value;
  }

  internal char Text
  {
    get => this.m_text;
    set => this.m_text = value;
  }

  internal PixColormap Colormap
  {
    get => this.m_colormap;
    set => this.m_colormap = value;
  }

  internal uint[] Data
  {
    get => this.m_data;
    set => this.m_data = value;
  }
}
