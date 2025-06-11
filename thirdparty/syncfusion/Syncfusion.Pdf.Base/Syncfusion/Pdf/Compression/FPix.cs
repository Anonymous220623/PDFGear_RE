// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.FPix
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class FPix
{
  private int m_w;
  private int m_h;
  private int m_wpl;
  private int m_refCount;
  private int m_xRes;
  private int m_yRes;
  private float[] m_data;

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

  internal int Wpl
  {
    get => this.m_wpl;
    set => this.m_wpl = value;
  }

  internal int RefCount
  {
    get => this.m_refCount;
    set => this.m_refCount = value;
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

  internal float[] Data
  {
    get => this.m_data;
    set => this.m_data = value;
  }
}
