// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.XML.XDLSHolder
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.DLS.XML;

public class XDLSHolder
{
  private int m_id = -1;
  private Dictionary<string, object> m_hashElements;
  private Dictionary<string, object> m_hashRefElements;
  private byte m_bFlags = 1;

  public int ID
  {
    get => this.m_id;
    set => this.m_id = value;
  }

  public bool Cleared
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set
    {
      if (value == (((int) this.m_bFlags & 1) != 0))
        return;
      if (value)
        this.Clear();
      else
        this.m_bFlags &= (byte) 254;
    }
  }

  public bool EnableID
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  public bool SkipMe
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  public void AddElement(string tagName, object value)
  {
    if (this.m_hashElements == null)
      this.m_hashElements = new Dictionary<string, object>();
    this.m_hashElements[tagName] = value;
  }

  public void AddRefElement(string tagName, object value)
  {
    if (this.m_hashRefElements == null)
      this.m_hashRefElements = new Dictionary<string, object>();
    this.m_hashRefElements[tagName] = value;
  }

  public void WriteHolder(IXDLSContentWriter writer)
  {
    if (this.m_hashElements != null)
    {
      foreach (string key in this.m_hashElements.Keys)
        writer.WriteChildElement(key, this.m_hashElements[key]);
    }
    if (this.m_hashRefElements == null)
      return;
    foreach (string key in this.m_hashRefElements.Keys)
    {
      if (this.m_hashRefElements[key] is IXDLSSerializable hashRefElement)
        writer.WriteChildRefElement(key, hashRefElement.XDLSHolder.ID);
    }
  }

  public bool ReadHolder(IXDLSContentReader reader)
  {
    if (reader.NodeType == XmlNodeType.Element)
    {
      string tagName = reader.TagName;
      if (this.m_hashElements != null && this.m_hashElements.ContainsKey(tagName))
      {
        object hashElement = this.m_hashElements[tagName];
        if (hashElement != null)
        {
          if (hashElement is IXDLSFactory xdlsFactory)
          {
            hashElement = (object) xdlsFactory.Create(reader);
            this.m_hashElements[tagName] = hashElement;
          }
          return reader.ReadChildElement(hashElement);
        }
      }
      if (this.m_hashRefElements != null && this.m_hashRefElements.ContainsKey(tagName))
      {
        string attributeValue = reader.GetAttributeValue("ref");
        this.m_hashRefElements[reader.TagName] = attributeValue != null ? (object) XmlConvert.ToInt32(attributeValue) : (object) -1;
        return false;
      }
    }
    return false;
  }

  public void AfterDeserialization(IXDLSSerializable owner)
  {
    if (this.m_hashElements != null)
    {
      foreach (string key in this.m_hashElements.Keys)
      {
        if (this.m_hashElements[key] is IXDLSSerializable hashElement2)
          hashElement2.XDLSHolder.AfterDeserialization(hashElement2);
        else if (this.m_hashElements[key] is IXDLSSerializableCollection hashElement1)
        {
          foreach (IXDLSSerializable owner1 in (IEnumerable) hashElement1)
            owner1?.XDLSHolder.AfterDeserialization(owner1);
        }
      }
    }
    if (this.m_hashRefElements != null)
    {
      foreach (string key in this.m_hashRefElements.Keys)
      {
        int num = -1;
        if (this.m_hashRefElements[key] != null)
          num = (int) this.m_hashRefElements[key];
        owner.RestoreReference(key, num);
      }
    }
    this.Clear();
  }

  public void BeforeSerialization()
  {
    if (this.m_hashElements == null)
      return;
    foreach (string key in this.m_hashElements.Keys)
    {
      if (this.m_hashElements[key] is IXDLSSerializable hashElement2)
      {
        hashElement2.XDLSHolder.Cleared = true;
        hashElement2.XDLSHolder.BeforeSerialization();
      }
      else if (this.m_hashElements[key] is IXDLSSerializableCollection hashElement1)
      {
        int num = 0;
        foreach (IXDLSSerializable xdlsSerializable in (IEnumerable) hashElement1)
        {
          if (xdlsSerializable != null)
          {
            xdlsSerializable.XDLSHolder.Cleared = true;
            xdlsSerializable.XDLSHolder.ID = num;
            xdlsSerializable.XDLSHolder.BeforeSerialization();
            ++num;
          }
        }
      }
    }
  }

  private void Clear()
  {
    if (this.m_hashElements != null)
      this.m_hashElements.Clear();
    if (this.m_hashRefElements != null)
      this.m_hashRefElements.Clear();
    this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | 1);
  }

  internal void Close()
  {
    if (this.m_hashElements != null)
    {
      this.m_hashElements.Clear();
      this.m_hashElements = (Dictionary<string, object>) null;
    }
    if (this.m_hashRefElements == null)
      return;
    this.m_hashRefElements.Clear();
    this.m_hashRefElements = (Dictionary<string, object>) null;
  }
}
