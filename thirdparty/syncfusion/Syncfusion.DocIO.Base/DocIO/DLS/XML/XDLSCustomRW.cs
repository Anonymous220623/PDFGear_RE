// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.XML.XDLSCustomRW
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.DLS.XML;

public class XDLSCustomRW
{
  private XmlReader m_reader;
  private XmlWriter m_writer;

  public bool Write(XmlWriter writer, string tagName, object value)
  {
    this.m_writer = writer;
    if (value is Matrix)
      this.WriteMatrix(tagName, value as Matrix);
    if (value is Color color)
    {
      this.WriteColor(tagName, color);
    }
    else
    {
      if (!(value is Font))
        return false;
      this.WriteFont(tagName, (Font) value);
    }
    return true;
  }

  public object Read(XmlReader reader, Type type)
  {
    this.m_reader = reader;
    if (type.Equals(typeof (Matrix)))
      return (object) this.ReadMatrix();
    if (type.Equals(typeof (Color)))
      return (object) this.ReadColor();
    return type.Equals(typeof (Font)) ? (object) this.ReadFont() : (object) null;
  }

  private void WriteMatrix(string name, Matrix matrix)
  {
    switch (name)
    {
      case null:
        throw new ArgumentNullException(nameof (name));
      case "":
        throw new ArgumentException("name - string can not be empty");
      default:
        if (matrix == null)
          throw new ArgumentNullException(nameof (matrix));
        this.m_writer.WriteStartElement(name);
        float[] elements = matrix.Elements;
        this.m_writer.WriteAttributeString("m11", XmlConvert.ToString(elements[0]));
        this.m_writer.WriteAttributeString("m12", XmlConvert.ToString(elements[1]));
        this.m_writer.WriteAttributeString("m21", XmlConvert.ToString(elements[2]));
        this.m_writer.WriteAttributeString("m22", XmlConvert.ToString(elements[3]));
        this.m_writer.WriteAttributeString("d1", XmlConvert.ToString(elements[4]));
        this.m_writer.WriteAttributeString("d2", XmlConvert.ToString(elements[5]));
        this.m_writer.WriteEndElement();
        break;
    }
  }

  private void WriteColor(string name, Color color)
  {
    this.m_writer.WriteStartElement(name);
    this.m_writer.WriteAttributeString("type", "Color");
    this.m_writer.WriteAttributeString("argb", XmlConvert.ToString(color.ToArgb()));
    this.m_writer.WriteEndElement();
  }

  private void WriteFont(string name, Font font)
  {
    this.m_writer.WriteStartElement(name);
    this.m_writer.WriteAttributeString("type", "Font");
    this.m_writer.WriteAttributeString("fontName", font.Name);
    this.m_writer.WriteAttributeString("size", font.SizeInPoints.ToString());
    this.m_writer.WriteEndElement();
  }

  private Font ReadFont()
  {
    string attribute1 = this.m_reader.GetAttribute("fontName");
    string attribute2 = this.m_reader.GetAttribute("size");
    this.m_reader.GetAttribute("style");
    this.m_reader.Read();
    return new Font(attribute1, (float) int.Parse(attribute2));
  }

  private Color ReadColor()
  {
    string attribute = this.m_reader.GetAttribute("argb");
    this.m_reader.Read();
    return Color.FromArgb(int.Parse(attribute));
  }

  private Matrix ReadMatrix()
  {
    Matrix matrix = new Matrix(XmlConvert.ToSingle(this.m_reader.GetAttribute("m11")), XmlConvert.ToSingle(this.m_reader.GetAttribute("m12")), XmlConvert.ToSingle(this.m_reader.GetAttribute("m21")), XmlConvert.ToSingle(this.m_reader.GetAttribute("m22")), XmlConvert.ToSingle(this.m_reader.GetAttribute("d1")), XmlConvert.ToSingle(this.m_reader.GetAttribute("d2")));
    this.m_reader.Read();
    return matrix;
  }
}
