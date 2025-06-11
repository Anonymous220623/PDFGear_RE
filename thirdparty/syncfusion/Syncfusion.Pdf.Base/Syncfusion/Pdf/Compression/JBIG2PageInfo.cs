// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2PageInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal struct JBIG2PageInfo
{
  private byte reserved;
  private byte operator_override;
  private byte aux_buffers;
  private byte default_operator;
  private ushort segment_flags;
  private uint m_width;
  private uint m_height;
  private uint m_xRes;
  private uint m_yRes;
  private byte m_defaultPixel;
  private byte m_containsRefinements;
  private byte m_isLossless;

  internal uint Width
  {
    get => this.m_width;
    set => this.m_width = value;
  }

  internal uint Height
  {
    get => this.m_height;
    set => this.m_height = value;
  }

  internal uint XRes
  {
    get => this.m_xRes;
    set => this.m_xRes = value;
  }

  internal uint YRes
  {
    get => this.m_yRes;
    set => this.m_yRes = value;
  }

  internal byte DefaultPixel
  {
    get => this.m_defaultPixel;
    set => this.m_defaultPixel = value;
  }

  internal byte ContainsRefinements
  {
    get => this.m_containsRefinements;
    set => this.m_containsRefinements = value;
  }

  internal byte IsLossless
  {
    get => this.m_isLossless;
    set => this.m_isLossless = value;
  }
}
