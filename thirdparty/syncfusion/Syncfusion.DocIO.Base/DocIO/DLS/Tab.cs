// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Tab
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Tab : XDLSSerializableBase
{
  internal const byte DeletePositionKey = 1;
  private TabJustification m_jc;
  private TabLeader m_tlc;
  private float m_tabPosition;
  private float m_tabDeletePosition;
  internal Dictionary<int, object> m_propertiesHash;

  public TabJustification Justification
  {
    get => this.m_jc;
    set
    {
      if (value != this.m_jc)
        this.m_jc = value;
      this.OnChange();
    }
  }

  public TabLeader TabLeader
  {
    get => this.m_tlc;
    set
    {
      if (value != this.m_tlc)
        this.m_tlc = value;
      this.OnChange();
    }
  }

  public float Position
  {
    get => this.m_tabPosition;
    set
    {
      if ((double) value != (double) this.m_tabPosition)
        this.m_tabPosition = value;
      this.OnChange();
    }
  }

  public float DeletePosition
  {
    get => this.HasKey(1) ? (float) this.PropertiesHash[1] : this.m_tabDeletePosition;
    set
    {
      this.m_tabDeletePosition = value;
      this.OnChange();
      this.SetKeyValue(1, (object) value);
    }
  }

  internal Dictionary<int, object> PropertiesHash
  {
    get
    {
      if (this.m_propertiesHash == null)
        this.m_propertiesHash = new Dictionary<int, object>();
      return this.m_propertiesHash;
    }
  }

  protected object this[int key]
  {
    get => (object) key;
    set => this.PropertiesHash[key] = value;
  }

  internal Tab(IWordDocument doc)
    : base((WordDocument) doc, (Entity) null)
  {
  }

  internal Tab(
    IWordDocument doc,
    float position,
    TabJustification justification,
    TabLeader leader)
    : this(doc, position, 0.0f, justification, leader)
  {
  }

  internal Tab(
    IWordDocument doc,
    float position,
    float deletePosition,
    TabJustification justification,
    TabLeader leader)
    : this(doc)
  {
    this.m_tabPosition = position;
    this.m_jc = justification;
    this.m_tlc = leader;
    this.m_tabDeletePosition = deletePosition;
  }

  internal bool HasKey(int Key)
  {
    return this.m_propertiesHash != null && this.m_propertiesHash.ContainsKey(Key);
  }

  internal void SetKeyValue(int propKey, object value) => this[propKey] = value;

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    writer.WriteValue("Position", this.Position);
    writer.WriteValue("Justification", (Enum) this.Justification);
    writer.WriteValue("Leader", (Enum) this.TabLeader);
    writer.WriteValue("Delete", this.DeletePosition);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    if (reader.HasAttribute("Position"))
      this.m_tabPosition = reader.ReadFloat("Position");
    if (reader.HasAttribute("Justification"))
      this.m_jc = (TabJustification) reader.ReadEnum("Justification", typeof (TabJustification));
    if (reader.HasAttribute("Leader"))
      this.m_tlc = (TabLeader) reader.ReadEnum("Leader", typeof (TabLeader));
    if (!reader.HasAttribute("Delete"))
      return;
    this.m_tabDeletePosition = reader.ReadFloat("Delete");
  }

  internal Tab Clone() => (Tab) this.CloneImpl();

  private void OnChange()
  {
    if (this.OwnerBase == null)
      return;
    (this.OwnerBase as TabCollection).OnChange();
  }

  internal bool Compare(Tab tab)
  {
    return (double) this.DeletePosition == (double) tab.DeletePosition && this.Justification == tab.Justification && (double) this.Position == (double) tab.Position && this.TabLeader == tab.TabLeader;
  }
}
