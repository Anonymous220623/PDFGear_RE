// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.UtilityMethods
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation;

public class UtilityMethods
{
  internal static XmlReader CreateReader(Stream data) => UtilityMethods.CreateReader(data, true);

  internal static XmlReader CreateReader(Stream data, bool skipToElement)
  {
    if (data.CanSeek && data.Position != 0L)
      data.Position = 0L;
    XmlReaderSettings settings = new XmlReaderSettings();
    XmlReader reader = XmlReader.Create(data, settings);
    if (skipToElement)
    {
      while (reader.NodeType != XmlNodeType.Element)
        reader.Read();
    }
    return reader;
  }

  internal static bool IsValidXMLDocument(Stream stream)
  {
    try
    {
      new XmlDocument().Load(stream);
    }
    catch
    {
      return false;
    }
    return true;
  }

  internal static XmlWriter CreateWriter(TextWriter data) => XmlWriter.Create(data);

  internal static XmlWriter CreateWriter(MemoryStream stream) => XmlWriter.Create((Stream) stream);

  internal static Stream ReadSingleNodeIntoStream(XmlReader reader)
  {
    MemoryStream stream = new MemoryStream();
    using (XmlWriter writer = UtilityMethods.CreateWriter(stream))
    {
      writer.WriteStartDocument();
      writer.WriteStartElement("sld");
      writer.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("xmlns", "p", (string) null, "http://schemas.openxmlformats.org/presentationml/2006/main");
      writer.WriteNode(reader, true);
      writer.WriteEndElement();
      writer.WriteEndDocument();
      writer.Flush();
    }
    return (Stream) stream;
  }

  public static double EmuToPoint(int emu) => Convert.ToDouble((double) emu / 12700.0);

  public static double EmuToInch(int emu) => Convert.ToDouble((double) emu / 914400.0);

  public static int InchToEmu(double inch) => Convert.ToInt32(inch * 914400.0);

  public static int PointToEmu(double point) => (int) (point * 12700.0);

  public static float InchToPoint(double inch) => (float) (inch * 72.0);

  public static float InchToPixel(double inch) => (float) (inch * 96.0);

  public static double EmuToPoint(long emu) => Convert.ToDouble((double) emu / 12700.0);

  public static int PointToPixel(double value) => Convert.ToInt32(value * 96.0 / 72.0);
}
