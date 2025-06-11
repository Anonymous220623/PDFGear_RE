// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.MetaPropertyImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class MetaPropertyImpl : IMetaProperty
{
  private string m_value;
  private string m_elementName;
  private string m_displayName;
  private string m_internalName;
  private string m_nameSpaceURI;

  public string Value
  {
    get => this.m_value;
    set => this.m_value = value;
  }

  public string Name
  {
    get => this.m_displayName;
    set => this.m_displayName = value;
  }

  internal string ElementName
  {
    get => this.m_elementName;
    set => this.m_elementName = value;
  }

  internal string InternalName
  {
    get => this.m_internalName;
    set => this.m_internalName = value;
  }

  internal string NameSpaceURI
  {
    get => this.m_nameSpaceURI;
    set => this.m_nameSpaceURI = value;
  }
}
