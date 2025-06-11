// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.XmlMapping
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class XmlMapping
{
  private string m_prefixMapping;
  private string m_XPath;
  private string m_storeItemID;
  private byte m_bFlags;
  private CustomXMLPart m_customXMLPart;
  private CustomXMLNode m_customXMLNode;
  private Entity m_ownerControl;

  public bool IsMapped
  {
    get => ((int) this.m_bFlags & 1) != 0;
    internal set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  public string PrefixMapping
  {
    get => this.m_prefixMapping;
    internal set => this.m_prefixMapping = value;
  }

  public string XPath
  {
    get => this.m_XPath;
    internal set => this.m_XPath = value;
  }

  internal string StoreItemID
  {
    get => this.m_storeItemID;
    set => this.m_storeItemID = value;
  }

  public CustomXMLPart CustomXmlPart
  {
    get
    {
      if (this.m_customXMLPart == null)
        this.m_customXMLPart = new CustomXMLPart();
      return this.m_customXMLPart;
    }
  }

  public CustomXMLNode CustomXmlNode
  {
    get
    {
      if (this.m_customXMLNode == null)
        this.m_customXMLNode = new CustomXMLNode();
      return this.m_customXMLNode;
    }
  }

  internal bool IsWordML
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool IsSupportWordML
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal XmlMapping(Entity ownerControl) => this.m_ownerControl = ownerControl;

  public void SetMapping(string xPath, string prefixMapping, CustomXMLPart customXmlPart)
  {
    this.XPath = xPath;
    this.PrefixMapping = prefixMapping;
    this.StoreItemID = customXmlPart.Id;
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(customXmlPart.XML);
    XmlNode xmlNode = xmlDocument.SelectSingleNode(xPath);
    if (xmlNode == null)
      return;
    this.MapItemsToControl(xmlNode.InnerText);
  }

  public void Delete()
  {
    this.XPath = (string) null;
    this.PrefixMapping = (string) null;
    this.StoreItemID = (string) null;
  }

  public void SetMappingByNode(CustomXMLNode customXmlNode)
  {
    this.XPath = customXmlNode.XPath;
    this.StoreItemID = customXmlNode.OwnerPart.Id;
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(customXmlNode.OwnerPart.XML);
    XmlNode xmlNode = xmlDocument.SelectSingleNode(this.XPath);
    if (xmlNode == null)
      return;
    this.MapItemsToControl(xmlNode.InnerText);
  }

  private void MapItemsToControl(string text)
  {
    if (this.m_ownerControl != null && this.m_ownerControl is InlineContentControl)
      (this.m_ownerControl as InlineContentControl).ParagraphItems.Add((IEntity) new WTextRange((IWordDocument) (this.m_ownerControl as InlineContentControl).Document)
      {
        Text = text
      });
    else if (this.m_ownerControl != null && this.m_ownerControl is BlockContentControl)
    {
      WParagraph wparagraph = new WParagraph((IWordDocument) (this.m_ownerControl as BlockContentControl).Document);
      wparagraph.AppendText(text);
      (this.m_ownerControl as BlockContentControl).MappedParagraph = wparagraph;
      wparagraph.SetOwner((OwnerHolder) (this.m_ownerControl as BlockContentControl));
      (this.m_ownerControl as BlockContentControl).ContentControlProperties.XmlMapping.IsMapped = true;
    }
    else
    {
      if (this.m_ownerControl == null || !(this.m_ownerControl is WTableCell))
        return;
      WTableCell ownerControl = this.m_ownerControl as WTableCell;
      WTableCell wtableCell = new WTableCell((IWordDocument) ownerControl.Document);
      wtableCell.AddParagraph().AppendText(text);
      wtableCell.SetOwner((OwnerHolder) ownerControl.OwnerRow);
      wtableCell.OwnerRow.SetOwner((OwnerHolder) ownerControl.OwnerRow.OwnerTable);
      wtableCell.ContentControl = ownerControl.ContentControl;
      wtableCell.ContentControl.ContentControlProperties.XmlMapping.IsMapped = true;
    }
  }
}
