// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.FeatureRecord
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal struct FeatureRecord
{
  private string m_tag;
  private int[] m_indexes;

  internal string Tag
  {
    get => this.m_tag;
    set => this.m_tag = value;
  }

  internal int[] Indexes
  {
    get => this.m_indexes;
    set => this.m_indexes = value;
  }
}
