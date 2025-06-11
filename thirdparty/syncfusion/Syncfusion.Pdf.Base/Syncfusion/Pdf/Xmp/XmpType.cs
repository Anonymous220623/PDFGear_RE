// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.XmpType
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public abstract class XmpType : XmpEntityBase
{
  private XmpMetadata m_xmp;

  protected internal XmpMetadata Xmp => this.m_xmp;

  internal XmpType(
    XmpMetadata xmp,
    XmlNode parent,
    string prefix,
    string localName,
    string namespaceURI)
    : base(parent, prefix, localName, namespaceURI)
  {
    this.m_xmp = xmp != null ? xmp : throw new ArgumentNullException(nameof (xmp));
    this.Initialize();
  }

  protected override XmlElement GetEntityXml()
  {
    XmlNode entityXml = (XmlNode) null;
    if (this.m_xmp.isLoadedDocument)
    {
      if (!(this.EntityParent.InnerText != ""))
      {
        if (!(this.EntityParent.InnerXml != ""))
          goto label_6;
      }
      try
      {
        entityXml = this.EntityParent.SelectSingleNode($"./{this.EntityPrefix}:{this.EntityName}", this.Xmp.NamespaceManager);
      }
      catch (Exception ex)
      {
        entityXml = this.EntityParent.SelectSingleNode($"./{(this.EntityPrefix == "xap" ? "xmp" : "xap")}:{this.EntityName}", this.Xmp.NamespaceManager);
      }
    }
    else
      entityXml = this.EntityParent.SelectSingleNode($"./{this.EntityPrefix}:{this.EntityName}", this.Xmp.NamespaceManager);
label_6:
    return entityXml as XmlElement;
  }

  protected override void CreateEntity()
  {
    this.EntityParent.AppendChild((XmlNode) this.Xmp.CreateElement(this.EntityPrefix, this.EntityName, this.EntityNamespaceURI));
  }
}
