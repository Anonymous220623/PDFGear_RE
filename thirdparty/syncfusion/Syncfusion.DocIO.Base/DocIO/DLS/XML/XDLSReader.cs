// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.XML.XDLSReader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.DLS.XML;

public class XDLSReader : IXDLSAttributeReader, IXDLSContentReader
{
  private Dictionary<Type, object> s_enumHashEntryDict = new Dictionary<Type, object>();
  private XmlReader m_reader;
  private XDLSCustomRW m_customRW = new XDLSCustomRW();

  public XDLSReader(XmlReader reader) => this.m_reader = reader;

  public void Deserialize(IXDLSSerializable value)
  {
    while (this.m_reader.NodeType != XmlNodeType.Element)
      this.m_reader.Read();
    this.ReadElement(value);
    value.XDLSHolder.AfterDeserialization(value);
  }

  public bool HasAttribute(string name) => this.m_reader.GetAttribute(name) != null;

  public string ReadString(string name) => this.m_reader.GetAttribute(name);

  public int ReadInt(string name) => XmlConvert.ToInt32(this.m_reader.GetAttribute(name));

  public short ReadShort(string name) => XmlConvert.ToInt16(this.m_reader.GetAttribute(name));

  public double ReadDouble(string name) => XmlConvert.ToDouble(this.m_reader.GetAttribute(name));

  public float ReadFloat(string name) => XmlConvert.ToSingle(this.m_reader.GetAttribute(name));

  public bool ReadBoolean(string name) => XmlConvert.ToBoolean(this.m_reader.GetAttribute(name));

  public byte ReadByte(string name) => XmlConvert.ToByte(this.m_reader.GetAttribute(name));

  public Enum ReadEnum(string name, Type enumType)
  {
    string attribute = this.m_reader.GetAttribute(name);
    return (Enum) Enum.Parse(enumType, attribute);
  }

  public Color ReadColor(string name)
  {
    switch (name)
    {
      case null:
        throw new ArgumentNullException(nameof (name));
      case "":
        throw new ArgumentException("name - string can not be empty");
      default:
        return this.GetHexColor(this.m_reader.GetAttribute(name));
    }
  }

  private Color GetHexColor(string color)
  {
    color = color.Replace("#", string.Empty);
    try
    {
      string s1 = color.Substring(0, 2);
      string s2 = color.Substring(2, 2);
      string s3 = color.Substring(4, 2);
      string s4 = color.Substring(6, 2);
      return Color.FromArgb(int.Parse(s1, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture), int.Parse(s2, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture), int.Parse(s3, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture), int.Parse(s4, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture));
    }
    catch
    {
    }
    return Color.Empty;
  }

  public DateTime ReadDateTime(string name)
  {
    switch (name)
    {
      case null:
        throw new ArgumentNullException(nameof (name));
      case "":
        throw new ArgumentException("name - string can not be empty");
      default:
        return XmlConvert.ToDateTime(this.m_reader.GetAttribute(name), XmlDateTimeSerializationMode.Utc);
    }
  }

  public string TagName => this.m_reader.LocalName;

  public XmlNodeType NodeType => this.m_reader.NodeType;

  public string GetAttributeValue(string name) => this.m_reader.GetAttribute(name);

  public bool ParseElementType(Type enumType, out Enum elementType)
  {
    object obj1 = (object) null;
    if (this.s_enumHashEntryDict.ContainsKey(enumType))
      obj1 = this.s_enumHashEntryDict[enumType];
    string[] names;
    Array values;
    if (obj1 == null)
    {
      names = Enum.GetNames(enumType);
      values = Enum.GetValues(enumType);
      object obj2 = (object) new object[2]
      {
        (object) names,
        (object) values
      };
      this.s_enumHashEntryDict.Add(enumType, obj2);
    }
    else
    {
      names = (string[]) ((object[]) obj1)[0];
      values = (Array) ((object[]) obj1)[1];
    }
    string attributeValue = this.GetAttributeValue("type");
    for (int index = 0; index < names.Length; ++index)
    {
      if (names[index] == attributeValue)
      {
        elementType = (Enum) values.GetValue(index);
        return true;
      }
    }
    elementType = (Enum) values.GetValue(0);
    return false;
  }

  public bool ReadChildElement(object value)
  {
    switch (value)
    {
      case IXDLSSerializable xdlsSerializable:
        this.ReadElement(xdlsSerializable);
        break;
      case IXDLSSerializableCollection coll:
        this.ReadElementCollection(coll);
        break;
      default:
        return false;
    }
    return true;
  }

  public object ReadChildElement(Type type) => this.m_customRW.Read(this.m_reader, type);

  public string ReadChildStringContent() => this.m_reader.ReadElementContentAsString();

  public byte[] ReadChildBinaryElement()
  {
    XmlReader reader = this.m_reader;
    byte[] numArray1 = new byte[0];
    byte[] numArray2 = new byte[1000];
    do
    {
      int length = reader.ReadElementContentAsBase64(numArray2, 0, numArray2.Length);
      byte[] destinationArray = new byte[numArray1.Length + length];
      numArray1.CopyTo((Array) destinationArray, 0);
      Array.Copy((Array) numArray2, 0, (Array) destinationArray, numArray1.Length, length);
      numArray1 = destinationArray;
      if (length >= numArray2.Length)
        numArray2 = new byte[numArray1.Length * 2];
      else
        break;
    }
    while (!reader.EOF);
    return numArray1;
  }

  internal Image ReadImage() => this.ReadImage(false);

  internal Image ReadImage(bool isMetafile)
  {
    Image image = (Image) null;
    byte[] buffer = this.ReadChildBinaryElement();
    if (buffer.Length > 0)
    {
      MemoryStream memoryStream = new MemoryStream(buffer);
      image = !isMetafile ? (Image) new Bitmap((Stream) memoryStream) : (Image) new Metafile((Stream) memoryStream);
    }
    return image;
  }

  public XmlReader InnerReader => this.m_reader;

  public IXDLSAttributeReader AttributeReader => (IXDLSAttributeReader) this;

  private void ReadElement(IXDLSSerializable value)
  {
    if (value == null)
    {
      this.m_reader.Skip();
    }
    else
    {
      if (this.m_reader.HasAttributes)
      {
        if (this.m_reader.MoveToAttribute("id"))
          value.XDLSHolder.ID = XmlConvert.ToInt32(this.m_reader.GetAttribute("id"));
        value.ReadXmlAttributes((IXDLSAttributeReader) this);
        this.m_reader.MoveToElement();
      }
      bool flag1 = false;
      string localName1 = this.m_reader.LocalName;
      bool flag2 = true;
      if (!this.m_reader.IsEmptyElement)
      {
        string localName2 = this.m_reader.LocalName;
        this.m_reader.Read();
        flag2 = false;
        if (!(localName2 == this.m_reader.LocalName) || this.m_reader.NodeType != XmlNodeType.EndElement)
          flag1 = true;
        else
          this.m_reader.ReadStartElement();
      }
      if (flag2)
        this.m_reader.ReadStartElement();
      int num = 0;
      if (!flag1)
        return;
      while ((this.m_reader.NodeType != XmlNodeType.EndElement || !(this.m_reader.LocalName == localName1) || num != 0) && !this.m_reader.EOF)
      {
        if (this.m_reader.NodeType != XmlNodeType.Whitespace && this.m_reader.IsStartElement() && this.m_reader.LocalName == localName1)
          ++num;
        else if (this.m_reader.NodeType == XmlNodeType.EndElement && this.m_reader.LocalName == localName1)
          --num;
        if (this.m_reader.NodeType != XmlNodeType.Element)
          this.m_reader.Read();
        else if (!value.ReadXmlContent((IXDLSContentReader) this))
          this.m_reader.Skip();
      }
      if (this.m_reader.NodeType != XmlNodeType.EndElement)
        return;
      this.m_reader.ReadEndElement();
    }
  }

  private void ReadElementCollection(IXDLSSerializableCollection coll)
  {
    bool flag1 = false;
    string localName1 = this.m_reader.LocalName;
    bool flag2 = true;
    if (!this.m_reader.IsEmptyElement)
    {
      string localName2 = this.m_reader.LocalName;
      this.m_reader.Read();
      flag2 = false;
      if (!(localName2 == this.m_reader.LocalName) || this.m_reader.NodeType != XmlNodeType.EndElement)
        flag1 = true;
      else
        this.m_reader.ReadStartElement();
    }
    if (flag2)
      this.m_reader.ReadStartElement();
    int num = 0;
    if (!flag1)
      return;
    while ((this.m_reader.NodeType != XmlNodeType.EndElement || !(this.m_reader.LocalName == localName1) || num != 0) && !this.m_reader.EOF)
    {
      if (this.m_reader.NodeType != XmlNodeType.Whitespace && this.m_reader.IsStartElement() && this.m_reader.LocalName == localName1)
        ++num;
      else if (this.m_reader.NodeType == XmlNodeType.EndElement && this.m_reader.LocalName == localName1)
        --num;
      if (this.m_reader.NodeType != XmlNodeType.Element)
        this.m_reader.Read();
      else if (this.m_reader.LocalName == coll.TagItemName)
        this.ReadElement(coll.AddNewItem((IXDLSContentReader) this));
    }
    if (this.m_reader.NodeType != XmlNodeType.EndElement)
      return;
    this.m_reader.ReadEndElement();
  }
}
