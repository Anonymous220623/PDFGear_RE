// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.OTextBodyItem
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class OTextBodyItem
{
  private byte m_flag;
  private string m_sectionStyleName;

  internal bool IsFirstItemOfSection
  {
    get => ((int) this.m_flag & 1) != 0;
    set => this.m_flag = (byte) ((int) this.m_flag & 254 | (value ? 1 : 0));
  }

  internal bool IsLastItemOfSection
  {
    get => ((int) this.m_flag & 2) >> 1 != 0;
    set => this.m_flag = (byte) ((int) this.m_flag & 253 | (value ? 1 : 0) << 1);
  }

  internal string SectionStyleName
  {
    get => this.m_sectionStyleName;
    set
    {
      if (string.IsNullOrEmpty(value))
        return;
      this.m_sectionStyleName = value;
    }
  }
}
