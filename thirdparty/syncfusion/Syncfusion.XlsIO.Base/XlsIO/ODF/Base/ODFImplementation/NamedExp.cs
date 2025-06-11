// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.NamedExp
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class NamedExp
{
  private string m_name;
  private string m_baseCellAddress;

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal string BaseCellAddress
  {
    get => this.m_baseCellAddress;
    set => this.m_baseCellAddress = value;
  }
}
