// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.CustomDocumentProperties
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.CompoundFile.XlsIO.Native;
using Syncfusion.CompoundFile.XlsIO.Net;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class CustomDocumentProperties(IApplication application, object parent) : 
  CollectionBaseEx<DocumentPropertyImpl>(application, parent),
  ICustomDocumentProperties
{
  public const string CustomGuidString = "D5CDD505-2E9C-101B-9397-08002B2CF9AE";
  public static readonly Guid GuidCustom = new Guid("D5CDD505-2E9C-101B-9397-08002B2CF9AE");
  private Dictionary<string, DocumentPropertyImpl> m_propertiesHash = new Dictionary<string, DocumentPropertyImpl>();

  public IDocumentProperty this[string strName] => this.GetProperty(strName) ?? this.Add(strName);

  IDocumentProperty ICustomDocumentProperties.this[int iIndex]
  {
    get
    {
      return iIndex >= 0 && iIndex <= this.Count - 1 ? (IDocumentProperty) this.InnerList[iIndex] : throw new ArgumentOutOfRangeException(nameof (iIndex), "Value cannot be less than 0 and greater than than Count - 1.");
    }
  }

  public IDocumentProperty GetProperty(string strName)
  {
    DocumentPropertyImpl property;
    this.m_propertiesHash.TryGetValue(strName, out property);
    return (IDocumentProperty) property;
  }

  public void Remove(string strName)
  {
    DocumentPropertyImpl documentPropertyImpl;
    if (!this.m_propertiesHash.TryGetValue(strName, out documentPropertyImpl))
      return;
    this.m_propertiesHash.Remove(strName);
    this.Remove(documentPropertyImpl);
  }

  public IDocumentProperty Add(string strName)
  {
    DocumentPropertyImpl documentPropertyImpl = new DocumentPropertyImpl(strName, (object) null);
    this.m_propertiesHash.Add(strName, documentPropertyImpl);
    this.Add(documentPropertyImpl);
    return (IDocumentProperty) documentPropertyImpl;
  }

  public bool Contains(string strName) => this.m_propertiesHash.ContainsKey(strName);

  [CLSCompliant(false)]
  public void Serialize(PropertySection section)
  {
    BuiltInDocumentProperties.WriteProperties(section, (ICollection) this.m_propertiesHash.Values);
  }

  [CLSCompliant(false)]
  public void Serialize(IPropertySetStorage setProp)
  {
    BuiltInDocumentProperties.WriteProperties(setProp, CustomDocumentProperties.GuidCustom, (ICollection) this.m_propertiesHash.Values);
  }

  [CLSCompliant(false)]
  public void Parse(IPropertySetStorage setProp)
  {
    BuiltInDocumentProperties.ReadProperties(setProp, CustomDocumentProperties.GuidCustom, (IDictionary) this.m_propertiesHash, this.List, true, false);
  }

  [CLSCompliant(false)]
  public void Parse(DocumentPropertyCollection properties)
  {
    System.Collections.Generic.List<PropertySection> sections = properties.Sections;
    int index = 0;
    for (int count = sections.Count; index < count; ++index)
    {
      PropertySection section = sections[index];
      if (section.Id == CustomDocumentProperties.GuidCustom)
        BuiltInDocumentProperties.ReadProperties(section, (IDictionary) this.m_propertiesHash, this.InnerList, true, false);
    }
  }

  protected override void OnClearComplete() => this.m_propertiesHash.Clear();
}
