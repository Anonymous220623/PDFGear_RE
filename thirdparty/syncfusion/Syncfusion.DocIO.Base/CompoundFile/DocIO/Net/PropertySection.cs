// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Net.PropertySection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Net;

public class PropertySection
{
  private const int PropertyNamesId = -2147483648 /*0x80000000*/;
  private const short UnicodeCodePage = 1200;
  private int m_iOffset;
  private Guid m_id;
  private int m_iLength;
  private List<PropertyData> m_lstProperties = new List<PropertyData>();
  private int m_sCodePage = -1;
  private PropertySection.DictionaryInfo m_dictionaryInfo;

  public int Offset
  {
    get => this.m_iOffset;
    set => this.m_iOffset = value;
  }

  public Guid Id
  {
    get => this.m_id;
    set => this.m_id = value;
  }

  public int Length
  {
    get => this.m_iLength;
    set => this.m_iLength = value;
  }

  public int Count => this.m_lstProperties.Count;

  public List<PropertyData> Properties => this.m_lstProperties;

  public PropertySection(Guid guid, int sectionOffset)
  {
    this.m_id = guid;
    this.m_iOffset = sectionOffset;
  }

  public void Parse(Stream stream)
  {
    byte[] buffer = new byte[4];
    stream.Position = (long) this.m_iOffset;
    this.m_iLength = StreamHelper.ReadInt32(stream, buffer);
    int num1 = StreamHelper.ReadInt32(stream, buffer);
    List<int> intList = new List<int>();
    for (int index = 0; index < num1; ++index)
    {
      int id = StreamHelper.ReadInt32(stream, buffer);
      int num2 = StreamHelper.ReadInt32(stream, buffer);
      this.m_lstProperties.Add(new PropertyData(id));
      intList.Add(num2);
    }
    intList.Add((int) stream.Length);
    Dictionary<int, string> dictNames = (Dictionary<int, string>) null;
    for (int index = 0; index < num1; ++index)
    {
      PropertyData lstProperty = this.m_lstProperties[index];
      int num3 = intList[index];
      int num4 = intList[index + 1];
      stream.Position = (long) (this.m_iOffset + intList[index]);
      int num5 = num4 - num3;
      if (lstProperty.Id < 2)
      {
        this.ParseSpecialProperties(lstProperty, stream, num5, ref dictNames);
      }
      else
      {
        this.ParseDictionary(stream, ref dictNames);
        lstProperty.Parse(stream, num5, this.m_sCodePage);
        string str;
        if (dictNames != null && dictNames.TryGetValue(lstProperty.Id, out str))
          lstProperty.Name = str;
      }
    }
  }

  private void ParseDictionary(Stream stream, ref Dictionary<int, string> dictNames)
  {
    if (this.m_dictionaryInfo == null)
      return;
    dictNames = this.ParsePropertyNames(stream, this.m_dictionaryInfo);
    this.m_dictionaryInfo = (PropertySection.DictionaryInfo) null;
  }

  private Dictionary<int, string> ParsePropertyNames(
    Stream stream,
    PropertySection.DictionaryInfo dictionaryInfo)
  {
    long position = stream.Position;
    stream.Position = dictionaryInfo.StreamOffset;
    Dictionary<int, string> propertyNames = this.ParsePropertyNames(stream);
    stream.Position = position;
    return propertyNames;
  }

  private void ParseSpecialProperties(
    PropertyData property,
    Stream stream,
    int reservedSize,
    ref Dictionary<int, string> dictNames)
  {
    if (property.Id == 0)
    {
      this.m_dictionaryInfo = new PropertySection.DictionaryInfo();
      this.m_dictionaryInfo.StreamOffset = stream.Position;
      this.m_dictionaryInfo.DataSize = reservedSize;
      stream.Position += (long) reservedSize;
    }
    else if (property.Id == 1)
    {
      byte[] buffer = new byte[4];
      property.PropertyType = (PropertyType) StreamHelper.ReadInt32(stream, buffer);
      if ((property.PropertyType & PropertyType.Vector) != PropertyType.Empty)
      {
        property.Value = (object) property.ParseVector(stream, reservedSize, this.m_sCodePage);
      }
      else
      {
        PropertyType itemType = property.PropertyType == PropertyType.Int16 ? PropertyType.Int32 : property.PropertyType;
        property.Value = property.ParseSingleValue(itemType, stream, reservedSize, this.m_sCodePage);
      }
      this.m_sCodePage = (int) property.Value;
      this.ParseDictionary(stream, ref dictNames);
    }
    else
      property.Parse(stream, reservedSize, this.m_sCodePage);
  }

  private Dictionary<int, string> ParsePropertyNames(Stream stream)
  {
    byte[] buffer = new byte[4];
    int num = StreamHelper.ReadInt32(stream, buffer);
    Dictionary<int, string> propertyNames = new Dictionary<int, string>();
    for (int index = 0; index < num; ++index)
    {
      int key = StreamHelper.ReadInt32(stream, buffer);
      string str = (short) this.m_sCodePage != (short) 1200 ? StreamHelper.GetAsciiString(stream, -1, this.m_sCodePage) : StreamHelper.GetUnicodeString(stream, -1);
      propertyNames.Add(key, str);
    }
    return propertyNames;
  }

  public void Serialize(Stream stream)
  {
    this.m_iOffset = (int) stream.Position;
    StreamHelper.WriteInt32(stream, 0);
    this.PrepareNames();
    int count1 = this.m_lstProperties.Count;
    StreamHelper.WriteInt32(stream, count1);
    stream.Position += (long) (count1 * 8);
    List<int> intList = new List<int>();
    for (int index = 0; index < count1; ++index)
    {
      PropertyData lstProperty = this.m_lstProperties[index];
      intList.Add((int) stream.Position);
      lstProperty.Serialize(stream);
    }
    long position = stream.Position;
    stream.Position = (long) (this.m_iOffset + 8);
    int index1 = 0;
    for (int count2 = intList.Count; index1 < count2; ++index1)
    {
      int num = intList[index1] - this.m_iOffset;
      StreamHelper.WriteInt32(stream, this.m_lstProperties[index1].Id);
      StreamHelper.WriteInt32(stream, num);
    }
    this.m_iLength = (int) (position - (long) this.m_iOffset);
    stream.Position = (long) this.m_iOffset;
    StreamHelper.WriteInt32(stream, this.m_iLength);
    stream.Position = position;
  }

  private Dictionary<int, string> PrepareNames()
  {
    Dictionary<int, string> dictionary = new Dictionary<int, string>();
    int index = 0;
    for (int count = this.m_lstProperties.Count; index < count; ++index)
    {
      PropertyData lstProperty = this.m_lstProperties[index];
      if (lstProperty.Name != null)
        dictionary.Add(lstProperty.Id, lstProperty.Name);
    }
    if (dictionary.Count > 0)
      this.m_lstProperties.Insert(0, new PropertyData(0)
      {
        Value = (object) dictionary
      });
    return dictionary;
  }

  private class DictionaryInfo
  {
    public long StreamOffset;
    public int DataSize;
  }
}
