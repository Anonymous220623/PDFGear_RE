// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.SectionProperties
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class SectionProperties
{
  private string m_backgroundColor;
  private int m_marginLeft;
  private int m_marginRight;
  private ODFColumns m_columns;

  internal ODFColumns Columns
  {
    get => this.m_columns;
    set => this.m_columns = value;
  }

  internal string BackgroundColor
  {
    get => this.m_backgroundColor;
    set => this.m_backgroundColor = value;
  }

  internal int MarginLeft
  {
    get => this.m_marginLeft;
    set => this.m_marginLeft = value;
  }

  internal int MarginRight
  {
    get => this.m_marginRight;
    set => this.m_marginRight = value;
  }

  internal void Close()
  {
    if (this.m_columns == null)
      return;
    this.m_columns = (ODFColumns) null;
  }
}
