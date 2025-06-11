// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.CustomSchema
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public class CustomSchema : XmpSchema
{
  private string m_namespace;
  private string m_namespaceUri;
  private Dictionary<string, string> m_customdata = new Dictionary<string, string>();
  internal XmpMetadata m_Xmp;

  public string this[string name]
  {
    get => this.GetSimpleProperty(name).Value;
    set
    {
      this.GetSimpleProperty(name).Value = value;
      this.m_customdata[name] = value;
      if (this.m_Xmp.DocumentInfo == null)
        return;
      this.m_Xmp.DocumentInfo.AddCustomMetaDataInfo(name, value);
    }
  }

  internal Dictionary<string, string> CustomData
  {
    get => this.m_customdata;
    set => this.m_customdata = value;
  }

  internal bool ContainsKey(string key)
  {
    return key != null ? this.m_customdata.ContainsValue(key) : throw new ArgumentNullException("key value should not be null");
  }

  public override XmpSchemaType SchemaType => XmpSchemaType.Custom;

  protected override string Prefix => this.m_namespace;

  protected override string Name => this.m_namespaceUri;

  public CustomSchema(XmpMetadata xmp, string xmlNamespace, string namespaceUri)
    : base(xmp)
  {
    this.m_Xmp = xmp;
    if (xmlNamespace == null)
      throw new ArgumentNullException(nameof (xmlNamespace));
    if (namespaceUri == null)
      throw new ArgumentNullException(nameof (namespaceUri));
    this.m_namespace = xmlNamespace;
    this.m_namespaceUri = namespaceUri;
    this.Initialize();
  }

  protected override XmlElement GetEntityXml()
  {
    XmlElement entityXml = (XmlElement) null;
    if (this.m_namespace != null)
      entityXml = base.GetEntityXml();
    return entityXml;
  }

  internal void Remove(string key)
  {
    if (key == null)
      throw new ArgumentNullException("key value should not be  null");
    if (!this.GetSimpleProperty(key).RemoveNode)
      return;
    this.m_customdata.Remove(key);
    this.RemoveSimplePropertity(key);
    if (this.Xmp.DocumentInfo == null || this.Xmp.DocumentInfo.CustomMetadata == null || !this.Xmp.DocumentInfo.CustomMetadata.ContainsKey(key))
      return;
    this.Xmp.DocumentInfo.CustomMetadata.Remove(key);
  }

  internal void GetCustomPrefix(string key) => this.GetCustomPefixNode(key);

  internal void SetCustomPrefix() => this.SetCustomPrefixNode();
}
