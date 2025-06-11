// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.XmlTableFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class XmlTableFormat
{
  private List<Stream> m_nodeArr;
  private string m_styleName;
  private WTable m_ownerTable;

  internal List<Stream> NodeArray
  {
    get
    {
      if (this.m_nodeArr == null)
        this.m_nodeArr = new List<Stream>();
      return this.m_nodeArr;
    }
    set => this.m_nodeArr = value;
  }

  internal string StyleName
  {
    get => this.m_styleName;
    set => this.m_styleName = value;
  }

  internal RowFormat Format => this.m_ownerTable.TableFormat;

  internal bool HasFormat
  {
    get => this.m_styleName != null || this.m_nodeArr != null && this.m_nodeArr.Count > 0;
  }

  internal WTable Owner => this.m_ownerTable;

  internal XmlTableFormat(WTable owner) => this.m_ownerTable = owner;

  internal XmlTableFormat Clone(WTable ownerTable)
  {
    XmlTableFormat xmlTableFormat = new XmlTableFormat(ownerTable);
    xmlTableFormat.StyleName = this.m_styleName;
    xmlTableFormat.Owner.SetOwner((OwnerHolder) ownerTable.OwnerTextBody);
    xmlTableFormat.NodeArray = this.m_nodeArr;
    return xmlTableFormat;
  }

  internal void Close()
  {
    if (this.m_nodeArr == null)
      return;
    this.m_nodeArr.Clear();
    this.m_nodeArr = (List<Stream>) null;
  }
}
