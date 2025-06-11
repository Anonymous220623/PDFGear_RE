// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Convertors.UtilityMethods
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.DLS.Convertors;

internal class UtilityMethods
{
  public static DateTime ConvertNumberToDateTime(double dNumber)
  {
    if (dNumber < 61.0)
      ++dNumber;
    return DateTime.FromOADate(dNumber);
  }

  public static void CopyStreamTo(Stream source, Stream destination)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    byte[] buffer = new byte[32768 /*0x8000*/];
    int count;
    while ((count = source.Read(buffer, 0, 32768 /*0x8000*/)) > 0)
      destination.Write(buffer, 0, count);
  }

  public static Stream CloneStream(Stream source)
  {
    Stream destination = (Stream) new MemoryStream((int) source.Length);
    long position = source.Position;
    source.Position = 0L;
    UtilityMethods.CopyStreamTo(source, destination);
    destination.Position = source.Position = position;
    return destination;
  }

  public static XmlReader CreateReader(Stream data, bool skipToElement)
  {
    data.Position = 0L;
    XmlReader reader = XmlReader.Create(data);
    if (skipToElement)
    {
      while (reader.NodeType != XmlNodeType.Element)
        reader.Read();
    }
    return reader;
  }

  public static XmlReader CreateReader(Stream data) => UtilityMethods.CreateReader(data, true);

  public static XmlWriter CreateWriter(Stream data, Encoding encoding)
  {
    return XmlWriter.Create(data, new XmlWriterSettings()
    {
      Encoding = encoding
    });
  }

  public static XmlWriter CreateWriter(TextWriter data)
  {
    XmlWriterSettings settings = new XmlWriterSettings();
    return XmlWriter.Create(data, settings);
  }

  public static XmlWriter CreateWriter(TextWriter data, bool indent)
  {
    return XmlWriter.Create(data, new XmlWriterSettings()
    {
      Indent = indent
    });
  }
}
