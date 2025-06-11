// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.XML.XDLSWriter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace Syncfusion.DocIO.DLS.XML;

public class XDLSWriter : IXDLSAttributeWriter, IXDLSContentWriter
{
  private const string DEF_SHARP = "#";
  private const string DEF_HEX_FORMAT = "X2";
  private readonly XmlWriter m_writer;
  private string m_rootTagName = "DLS";
  private XDLSCustomRW m_customRW = new XDLSCustomRW();
  private Metafile m_srcMetafile;

  public XDLSWriter(XmlWriter writer) => this.m_writer = writer;

  public void Serialize(IXDLSSerializable value)
  {
    value.XDLSHolder.BeforeSerialization();
    this.WriteElement(this.m_rootTagName, value, false);
  }

  private void WriteElement(string tagName, IXDLSSerializable value, bool isWriteID)
  {
    if (value.XDLSHolder.SkipMe)
      return;
    this.m_writer.WriteStartElement(tagName);
    if (isWriteID && value.XDLSHolder.EnableID)
      this.WriteValue("id", value.XDLSHolder.ID);
    value.WriteXmlAttributes((IXDLSAttributeWriter) this);
    value.WriteXmlContent((IXDLSContentWriter) this);
    this.m_writer.WriteEndElement();
  }

  private void WriteCollectionElement(string tagName, IXDLSSerializableCollection value)
  {
    if (value.Count <= 0)
      return;
    this.m_writer.WriteStartElement(tagName);
    foreach (IXDLSSerializable xdlsSerializable in (IEnumerable) value)
    {
      if (xdlsSerializable != null)
        this.WriteElement(value.TagItemName, xdlsSerializable, true);
    }
    this.m_writer.WriteEndElement();
  }

  protected virtual void WriteCustomElement(string tagName, object value)
  {
    if (this.m_customRW.Write(this.m_writer, tagName, value))
      return;
    this.WriteDefElement(tagName, value);
  }

  private void WriteDefElement(string tagName, object value)
  {
    if (!(value is IXmlSerializable xmlSerializable))
      return;
    xmlSerializable.WriteXml(this.m_writer);
  }

  public void WriteValue(string name, float value)
  {
    this.m_writer.WriteAttributeString(name, XmlConvert.ToString(value));
  }

  public void WriteValue(string name, double value)
  {
    this.m_writer.WriteAttributeString(name, XmlConvert.ToString(value));
  }

  public void WriteValue(string name, int value)
  {
    this.m_writer.WriteAttributeString(name, XmlConvert.ToString(value));
  }

  public void WriteValue(string name, string value)
  {
    this.m_writer.WriteAttributeString(name, value);
  }

  public void WriteValue(string name, Enum value)
  {
    this.m_writer.WriteAttributeString(name, value.ToString());
  }

  public void WriteValue(string name, bool value)
  {
    this.m_writer.WriteAttributeString(name, XmlConvert.ToString(value));
  }

  public void WriteValue(string name, Color value)
  {
    switch (name)
    {
      case null:
        throw new ArgumentNullException(nameof (name));
      case "":
        throw new ArgumentException("name - string can not be empty");
      default:
        StringBuilder stringBuilder = new StringBuilder();
        if (!value.IsEmpty)
        {
          stringBuilder.Append("#");
          stringBuilder.Append(value.A.ToString("X2"));
          stringBuilder.Append(value.R.ToString("X2"));
          stringBuilder.Append(value.G.ToString("X2"));
          stringBuilder.Append(value.B.ToString("X2"));
        }
        this.m_writer.WriteAttributeString(name, stringBuilder.ToString());
        break;
    }
  }

  public void WriteValue(string name, DateTime value)
  {
    this.m_writer.WriteAttributeString(name, XmlConvert.ToString(value, XmlDateTimeSerializationMode.Utc));
  }

  public void WriteChildStringElement(string name, string value)
  {
    this.m_writer.WriteStartElement(name);
    this.m_writer.WriteString(value);
    this.m_writer.WriteEndElement();
  }

  public void WriteChildBinaryElement(string name, byte[] value)
  {
    switch (name)
    {
      case null:
        throw new ArgumentNullException(nameof (name));
      case "":
        throw new ArgumentException("name - string can not be empty");
      default:
        this.InnerWriter.WriteStartElement(name);
        this.InnerWriter.WriteBase64(value, 0, value.Length);
        this.InnerWriter.WriteEndElement();
        break;
    }
  }

  public void WriteChildElement(string name, object value)
  {
    switch (value)
    {
      case IXDLSSerializable xdlsSerializable:
        this.WriteElement(name, xdlsSerializable, false);
        break;
      case IXDLSSerializableCollection serializableCollection:
        this.WriteCollectionElement(name, serializableCollection);
        break;
      case string _:
        this.m_writer.WriteStartElement(name);
        this.WriteValue("type", "String");
        this.WriteValue(nameof (value), (string) value);
        this.m_writer.WriteEndElement();
        break;
      case int num1:
        this.m_writer.WriteStartElement(name);
        this.WriteValue("type", "Int32");
        this.WriteValue(nameof (value), num1);
        this.m_writer.WriteEndElement();
        break;
      case float num2:
        this.m_writer.WriteStartElement(name);
        this.WriteValue("type", "Single");
        this.WriteValue(nameof (value), num2);
        this.m_writer.WriteEndElement();
        break;
      case bool _:
        this.m_writer.WriteStartElement(name);
        this.WriteValue("type", "Boolean");
        this.WriteValue(nameof (value), value.ToString());
        this.m_writer.WriteEndElement();
        break;
      case Enum _:
        this.m_writer.WriteStartElement(name);
        this.WriteValue("type", value.GetType().ToString());
        this.WriteValue(nameof (value), value.ToString());
        this.m_writer.WriteEndElement();
        break;
      default:
        this.WriteCustomElement(name, value);
        break;
    }
  }

  public void WriteChildRefElement(string name, int refToElement)
  {
    this.m_writer.WriteStartElement(name);
    this.WriteValue("ref", refToElement);
    this.m_writer.WriteEndElement();
  }

  internal void WriteImage(Image image)
  {
    if (image == null)
      return;
    MemoryStream streamFromImage = this.CreateStreamFromImage(image);
    byte[] buffer = new byte[streamFromImage.Length];
    streamFromImage.Position = 0L;
    streamFromImage.Read(buffer, 0, buffer.Length);
    this.WriteChildBinaryElement(nameof (image), buffer);
  }

  public XmlWriter InnerWriter => this.m_writer;

  private bool PlayInMeta(
    EmfPlusRecordType recordType,
    int flags,
    int dataSize,
    IntPtr data,
    PlayRecordCallback callbackData)
  {
    byte[] numArray = new byte[dataSize];
    if (data != IntPtr.Zero)
      Marshal.Copy(data, numArray, 0, dataSize);
    this.m_srcMetafile.PlayRecord(recordType, flags, dataSize, numArray);
    return true;
  }

  private MemoryStream CreateStreamFromImage(Image image)
  {
    MemoryStream streamFromImage = new MemoryStream();
    if (image is Metafile)
    {
      this.m_srcMetafile = image as Metafile;
      Rectangle bounds = this.m_srcMetafile.GetMetafileHeader().Bounds;
      Graphics graphics1 = Graphics.FromImage((Image) new Bitmap(bounds.Width, bounds.Height, this.m_srcMetafile.PixelFormat));
      IntPtr hdc = graphics1.GetHdc();
      Metafile metafile = new Metafile((Stream) streamFromImage, hdc, EmfType.EmfOnly);
      graphics1.ReleaseHdc(hdc);
      using (Graphics graphics2 = Graphics.FromImage((Image) metafile))
        graphics2.EnumerateMetafile(this.m_srcMetafile, bounds.Location, new Graphics.EnumerateMetafileProc(this.PlayInMeta));
    }
    else
    {
      try
      {
        image.Save((Stream) streamFromImage, image.RawFormat);
      }
      catch
      {
        image.Save((Stream) streamFromImage, ImageFormat.Png);
      }
    }
    return streamFromImage;
  }
}
