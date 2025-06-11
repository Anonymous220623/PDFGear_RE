// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Net.PropertyData
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Net;

public class PropertyData : IPropertyData, IComparable
{
  private const int LinkBit = 16777216 /*0x01000000*/;
  private const int NamesDictionaryId = 0;
  private int m_iId;
  private string m_strName;
  public PropertyType PropertyType;
  public object Data;

  public bool IsLinkToSource => (this.Id & 16777216 /*0x01000000*/) != 0;

  public int ParentId => !this.IsLinkToSource ? this.Id : this.Id - 16777216 /*0x01000000*/;

  public int Id
  {
    get => this.m_iId;
    set => this.m_iId = value;
  }

  internal PropertyData()
  {
  }

  public PropertyData(int id) => this.Id = id;

  public void Parse(Stream stream, int roundedSize)
  {
    byte[] buffer = new byte[4];
    this.PropertyType = (PropertyType) StreamHelper.ReadInt32(stream, buffer);
    if ((this.PropertyType & PropertyType.Vector) != PropertyType.Empty)
      this.Data = (object) this.ParseVector(stream, roundedSize);
    else
      this.Data = this.ParseSingleValue(this.PropertyType, stream, roundedSize);
  }

  internal void Parse(Stream stream, int roundedSize, int codePage)
  {
    byte[] buffer = new byte[4];
    this.PropertyType = (PropertyType) StreamHelper.ReadInt32(stream, buffer);
    if ((this.PropertyType & PropertyType.Vector) != PropertyType.Empty)
      this.Data = (object) this.ParseVector(stream, roundedSize, codePage);
    else
      this.Data = this.ParseSingleValue(this.PropertyType, stream, roundedSize, codePage);
  }

  internal bool IsValidProperty()
  {
    switch (this.Id)
    {
      case 1:
        return this.PropertyType == PropertyType.Int16;
      case 2:
      case 3:
      case 14:
      case 15:
      case 26:
      case 27:
      case 28:
      case 29:
        return this.PropertyType == PropertyType.AsciiString;
      case 4:
      case 5:
      case 6:
      case 7:
      case 8:
      case 9:
      case 10:
      case 17:
      case 23:
        return this.PropertyType == PropertyType.Int32;
      case 11:
      case 16 /*0x10*/:
      case 19:
      case 22:
        return this.PropertyType == PropertyType.Bool;
      case 12:
        return this.PropertyType == PropertyType.Vector || this.PropertyType == PropertyType.Object;
      case 13:
        return this.PropertyType == PropertyType.Vector || this.PropertyType == PropertyType.AsciiString;
      case 24:
        return this.PropertyType == PropertyType.Blob;
      default:
        return false;
    }
  }

  private IList ParseVector(Stream stream, int roundedSize)
  {
    byte[] buffer = new byte[4];
    int count = StreamHelper.ReadInt32(stream, buffer);
    PropertyType itemType = this.PropertyType & ~PropertyType.Vector;
    IList array = this.CreateArray(itemType, count);
    for (int index = 0; index < count; ++index)
      array[index] = this.ParseSingleValue(itemType, stream, roundedSize - 4);
    return array;
  }

  internal IList ParseVector(Stream stream, int roundedSize, int codePage)
  {
    byte[] buffer = new byte[4];
    int count = StreamHelper.ReadInt32(stream, buffer);
    PropertyType itemType = this.PropertyType & ~PropertyType.Vector;
    IList array = this.CreateArray(itemType, count);
    for (int index = 0; index < count; ++index)
      array[index] = this.ParseSingleValue(itemType, stream, roundedSize - 4, codePage);
    return array;
  }

  private IList CreateArray(PropertyType itemType, int count)
  {
    switch (itemType)
    {
      case PropertyType.Int32:
      case PropertyType.Int:
        return (IList) new int[count];
      case PropertyType.AsciiString:
      case PropertyType.String:
        return (IList) new string[count];
      default:
        return (IList) new object[count];
    }
  }

  private object ParseSingleValue(PropertyType itemType, Stream stream, int roundedSize)
  {
    byte[] buffer = new byte[8];
    object singleValue;
    switch (itemType)
    {
      case PropertyType.Empty:
      case PropertyType.Null:
        singleValue = (object) null;
        break;
      case PropertyType.Int16:
        singleValue = (object) StreamHelper.ReadInt16(stream, buffer);
        stream.Position += 2L;
        break;
      case PropertyType.Int32:
      case PropertyType.Int:
        singleValue = (object) StreamHelper.ReadInt32(stream, buffer);
        break;
      case PropertyType.Double:
        singleValue = (object) StreamHelper.ReadDouble(stream, buffer);
        break;
      case PropertyType.Bool:
        singleValue = (object) (StreamHelper.ReadInt32(stream, buffer) != 0);
        break;
      case PropertyType.Object:
        singleValue = this.GetObject(stream, roundedSize - 4);
        break;
      case PropertyType.UInt32:
        singleValue = (object) (uint) StreamHelper.ReadInt16(stream, buffer);
        break;
      case PropertyType.AsciiString:
        singleValue = (object) StreamHelper.GetAsciiString(stream, roundedSize - 4);
        break;
      case PropertyType.String:
        singleValue = (object) StreamHelper.GetUnicodeString(stream, roundedSize - 4);
        break;
      case PropertyType.DateTime:
        singleValue = this.GetDateTime(stream, buffer);
        break;
      case PropertyType.Blob:
        singleValue = this.GetBlob(stream, buffer);
        break;
      case PropertyType.ClipboardData:
        singleValue = this.GetClipboardData(stream, buffer);
        break;
      default:
        throw new NotImplementedException();
    }
    return singleValue;
  }

  internal object ParseSingleValue(
    PropertyType itemType,
    Stream stream,
    int roundedSize,
    int codePage)
  {
    byte[] buffer = new byte[8];
    object singleValue;
    switch (itemType)
    {
      case PropertyType.Empty:
      case PropertyType.Null:
        singleValue = (object) null;
        break;
      case PropertyType.Int16:
        singleValue = (object) StreamHelper.ReadInt16(stream, buffer);
        stream.Position += 2L;
        break;
      case PropertyType.Int32:
      case PropertyType.Int:
        singleValue = (object) StreamHelper.ReadInt32(stream, buffer);
        break;
      case PropertyType.Double:
        singleValue = (object) StreamHelper.ReadDouble(stream, buffer);
        break;
      case PropertyType.Bool:
        singleValue = (object) (StreamHelper.ReadInt32(stream, buffer) != 0);
        break;
      case PropertyType.Object:
        singleValue = this.GetObject(stream, roundedSize - 4, codePage);
        break;
      case PropertyType.UInt32:
        singleValue = (object) (uint) StreamHelper.ReadInt16(stream, buffer);
        break;
      case PropertyType.AsciiString:
        singleValue = (object) StreamHelper.GetAsciiString(stream, roundedSize - 4, codePage);
        break;
      case PropertyType.String:
        singleValue = (object) StreamHelper.GetUnicodeString(stream, roundedSize - 4);
        break;
      case PropertyType.DateTime:
        singleValue = this.GetDateTime(stream, buffer);
        break;
      case PropertyType.Blob:
        singleValue = this.GetBlob(stream, buffer);
        break;
      case PropertyType.ClipboardData:
        singleValue = this.GetClipboardData(stream, buffer);
        break;
      default:
        throw new NotImplementedException();
    }
    return singleValue;
  }

  private object GetDateTime(Stream stream, byte[] buffer)
  {
    stream.Read(buffer, 0, 8);
    DateTime dateTime = new DateTime(BitConverter.ToInt64(buffer, 0) + 504911232000000000L);
    if (this.Id != 10)
      dateTime = dateTime.ToLocalTime();
    return (object) dateTime;
  }

  private object GetBlob(Stream stream, byte[] buffer)
  {
    int count = StreamHelper.ReadInt32(stream, buffer);
    byte[] buffer1 = new byte[count];
    if (stream.Read(buffer1, 0, count) != count)
      throw new Exception();
    return (object) buffer1;
  }

  private object GetClipboardData(Stream stream, byte[] buffer)
  {
    ClipboardData clipboardData = new ClipboardData();
    clipboardData.Parse(stream);
    return (object) clipboardData;
  }

  private object GetObject(Stream stream, int roundedSize)
  {
    byte[] buffer = new byte[4];
    return this.ParseSingleValue((PropertyType) StreamHelper.ReadInt32(stream, buffer), stream, roundedSize - 4);
  }

  private object GetObject(Stream stream, int roundedSize, int codePage)
  {
    byte[] buffer = new byte[4];
    return this.ParseSingleValue((PropertyType) StreamHelper.ReadInt32(stream, buffer), stream, roundedSize - 4, codePage);
  }

  private int WriteObject(Stream stream, object value)
  {
    PropertyType valueType;
    switch (value)
    {
      case int _:
        valueType = PropertyType.Int32;
        break;
      case double _:
        valueType = PropertyType.Double;
        break;
      case bool _:
        valueType = PropertyType.Bool;
        break;
      case string _:
        valueType = PropertyType.String;
        break;
      default:
        throw new NotImplementedException();
    }
    StreamHelper.WriteInt32(stream, (int) valueType);
    return this.SerializeSingleValue(stream, value, valueType) + 4;
  }

  public int Serialize(Stream stream)
  {
    int num = StreamHelper.WriteInt32(stream, (int) this.PropertyType);
    int iWrittenSize;
    if ((this.PropertyType & PropertyType.Vector) == PropertyType.Vector)
      iWrittenSize = num + this.SerializeVector(stream, (IList) this.Data);
    else if (this.Id == 0)
    {
      stream.Position -= 4L;
      iWrittenSize = num + this.SerializeDictionary(stream, (Dictionary<int, string>) this.Data);
    }
    else
      iWrittenSize = num + this.SerializeSingleValue(stream, this.Data, this.PropertyType);
    if (this.PropertyType != PropertyType.AsciiString)
      StreamHelper.AddPadding(stream, ref iWrittenSize);
    return iWrittenSize;
  }

  private int SerializeDictionary(Stream stream, Dictionary<int, string> dictionary)
  {
    int num1 = 0;
    int count = dictionary.Count;
    int num2 = num1 + StreamHelper.WriteInt32(stream, count);
    foreach (KeyValuePair<int, string> keyValuePair in dictionary)
    {
      num2 += StreamHelper.WriteInt32(stream, keyValuePair.Key);
      num2 += StreamHelper.WriteAsciiString(stream, keyValuePair.Value, false);
    }
    return num2;
  }

  private int SerializeVector(Stream stream, IList data)
  {
    int count = data.Count;
    StreamHelper.WriteInt32(stream, count);
    int num = 4;
    PropertyType valueType = this.PropertyType & ~PropertyType.Vector;
    for (int index = 0; index < count; ++index)
      num += this.SerializeSingleValue(stream, data[index], valueType);
    return num;
  }

  private int SerializeSingleValue(Stream stream, object value, PropertyType valueType)
  {
    int num = 0;
    switch (valueType)
    {
      case PropertyType.Empty:
      case PropertyType.Null:
        return num;
      case PropertyType.Int16:
        if (this.Id == 1)
        {
          num += StreamHelper.WriteInt32(stream, Convert.ToInt32(value));
          goto case PropertyType.Empty;
        }
        num += StreamHelper.WriteInt16(stream, (short) value);
        goto case PropertyType.Empty;
      case PropertyType.Int32:
      case PropertyType.Int:
        num += StreamHelper.WriteInt32(stream, (int) value);
        goto case PropertyType.Empty;
      case PropertyType.Double:
        num += StreamHelper.WriteDouble(stream, (double) value);
        goto case PropertyType.Empty;
      case PropertyType.Bool:
        bool flag = (bool) value;
        num += StreamHelper.WriteInt32(stream, flag ? 1 : 0);
        goto case PropertyType.Empty;
      case PropertyType.Object:
        num += this.WriteObject(stream, value);
        goto case PropertyType.Empty;
      case PropertyType.UInt32:
        num += StreamHelper.WriteInt32(stream, (int) (uint) value);
        goto case PropertyType.Empty;
      case PropertyType.AsciiString:
        num += StreamHelper.WriteAsciiString(stream, (string) value, false);
        goto case PropertyType.Empty;
      case PropertyType.String:
        num += StreamHelper.WriteUnicodeString(stream, (string) value);
        goto case PropertyType.Empty;
      case PropertyType.DateTime:
        DateTime dateTime = !(value is TimeSpan timeSpan) ? (DateTime) value : DateTime.FromBinary(timeSpan.Ticks);
        if (this.Id != 10)
          dateTime = dateTime.ToUniversalTime();
        byte[] bytes = BitConverter.GetBytes((ulong) (dateTime.Ticks - 504911232000000000L));
        stream.Write(bytes, 0, bytes.Length);
        num += bytes.Length;
        goto case PropertyType.Empty;
      case PropertyType.Blob:
        byte[] numArray = (byte[]) value;
        num += this.SerializeBlob(stream, numArray);
        goto case PropertyType.Empty;
      case PropertyType.ClipboardData:
        ClipboardData data = (ClipboardData) value;
        num += this.SerializeClipboardData(stream, data);
        goto case PropertyType.Empty;
      default:
        throw new NotImplementedException();
    }
  }

  private int SerializeClipboardData(Stream stream, ClipboardData data) => data.Serialize(stream);

  private int SerializeBlob(Stream stream, byte[] value)
  {
    int num1 = 0;
    int length = value.Length;
    int num2 = num1 + StreamHelper.WriteInt32(stream, length);
    stream.Write(value, 0, length);
    return num2 + length;
  }

  public bool SetValue(object value, PropertyType type)
  {
    bool flag = false;
    switch (type)
    {
      case PropertyType.Empty:
      case PropertyType.Null:
      case PropertyType.Int16:
      case PropertyType.Int32:
      case PropertyType.Double:
      case PropertyType.Bool:
      case PropertyType.Object:
      case PropertyType.UInt32:
      case PropertyType.Int:
      case PropertyType.AsciiString:
      case PropertyType.String:
      case PropertyType.DateTime:
      case PropertyType.Blob:
      case PropertyType.ClipboardData:
      case PropertyType.Vector:
      case PropertyType.ObjectArray:
      case PropertyType.AsciiStringArray:
      case PropertyType.StringArray:
        this.Value = value;
        this.Type = (VarEnum) type;
        flag = true;
        break;
    }
    return flag;
  }

  public object Value
  {
    get => this.Data;
    set => this.Data = value;
  }

  public VarEnum Type
  {
    get => (VarEnum) this.PropertyType;
    set => this.PropertyType = (PropertyType) value;
  }

  public string Name
  {
    get => this.m_strName;
    set => this.m_strName = value;
  }

  public int CompareTo(object obj) => this.Id - ((PropertyData) obj).Id;
}
