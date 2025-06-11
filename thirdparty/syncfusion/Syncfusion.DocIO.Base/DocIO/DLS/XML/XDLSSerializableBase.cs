// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.XML.XDLSSerializableBase
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS.XML;

public abstract class XDLSSerializableBase(WordDocument doc, Entity entity) : 
  OwnerHolder(doc, (OwnerHolder) entity),
  IXDLSSerializable
{
  private XDLSHolder m_XDLSHolder;

  void IXDLSSerializable.WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    this.WriteXmlAttributes(writer);
  }

  void IXDLSSerializable.WriteXmlContent(IXDLSContentWriter writer)
  {
    this.XDLSHolder.WriteHolder(writer);
    this.WriteXmlContent(writer);
  }

  void IXDLSSerializable.ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    this.ReadXmlAttributes(reader);
  }

  bool IXDLSSerializable.ReadXmlContent(IXDLSContentReader reader)
  {
    return this.XDLSHolder.ReadHolder(reader) || this.ReadXmlContent(reader);
  }

  XDLSHolder IXDLSSerializable.XDLSHolder
  {
    get
    {
      if (this.m_XDLSHolder == null)
        this.m_XDLSHolder = new XDLSHolder();
      if (this.m_XDLSHolder.Cleared)
      {
        this.m_XDLSHolder.Cleared = false;
        this.InitXDLSHolder();
      }
      return this.m_XDLSHolder;
    }
  }

  void IXDLSSerializable.RestoreReference(string name, int value)
  {
    this.RestoreReference(name, value);
  }

  internal object CloneInt() => this.CloneImpl();

  internal virtual void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
  }

  protected XDLSHolder XDLSHolder => ((IXDLSSerializable) this).XDLSHolder;

  protected virtual object CloneImpl()
  {
    XDLSSerializableBase serializableBase = (XDLSSerializableBase) this.MemberwiseClone();
    serializableBase.m_XDLSHolder = (XDLSHolder) null;
    serializableBase.SetOwner(serializableBase.Document, (OwnerHolder) null);
    return (object) serializableBase;
  }

  protected virtual void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
  }

  protected virtual void WriteXmlContent(IXDLSContentWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
  }

  protected virtual void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
  }

  protected virtual bool ReadXmlContent(IXDLSContentReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    return false;
  }

  protected virtual void InitXDLSHolder()
  {
  }

  protected virtual void RestoreReference(string name, int index)
  {
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_XDLSHolder == null)
      return;
    this.m_XDLSHolder.Close();
    this.m_XDLSHolder = (XDLSHolder) null;
  }
}
