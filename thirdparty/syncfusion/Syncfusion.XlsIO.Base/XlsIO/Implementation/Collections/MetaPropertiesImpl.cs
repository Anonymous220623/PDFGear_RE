// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.MetaPropertiesImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class MetaPropertiesImpl(IApplication application, object parent) : 
  CollectionBaseEx<IMetaProperty>(application, parent),
  IMetaProperties,
  IEnumerable
{
  private Dictionary<string, IMetaProperty> m_hashNameToIMetaProperty = new Dictionary<string, IMetaProperty>();
  private Dictionary<string, IMetaProperty> m_displayNameToIMetaProperty = new Dictionary<string, IMetaProperty>();
  private WorkbookImpl m_book;
  private WorksheetImpl m_worksheet;
  private string m_SchemaXml;
  private string m_ItemName;
  private bool m_isValid = true;

  internal string ItemName
  {
    get => this.m_ItemName;
    set => this.m_ItemName = value;
  }

  internal bool IsValid
  {
    get => this.m_isValid;
    set => this.m_isValid = value;
  }

  public string SchemaXml
  {
    get => this.m_SchemaXml;
    set => this.m_SchemaXml = value;
  }

  protected override void OnInsertComplete(int index, IMetaProperty value)
  {
    MetaPropertyImpl metaPropertyImpl = (MetaPropertyImpl) value;
    base.OnInsertComplete(index, value);
    this.m_hashNameToIMetaProperty[metaPropertyImpl.InternalName] = value;
    if (metaPropertyImpl.ElementName == null)
      return;
    this.m_displayNameToIMetaProperty[metaPropertyImpl.ElementName] = value;
  }

  public IMetaProperty GetItemByInternalName(string name)
  {
    return this.m_hashNameToIMetaProperty.ContainsKey(name) ? this.m_hashNameToIMetaProperty[name] : (IMetaProperty) null;
  }

  internal IMetaProperty GetItemByDisplayName(string name)
  {
    return this.m_displayNameToIMetaProperty.ContainsKey(name) ? this.m_displayNameToIMetaProperty[name] : (IMetaProperty) null;
  }
}
