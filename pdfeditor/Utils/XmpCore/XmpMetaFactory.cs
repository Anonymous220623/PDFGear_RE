// Decompiled with JetBrains decompiler
// Type: XmpCore.XmpMetaFactory
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.IO;
using System.Xml.Linq;
using XmpCore.Impl;
using XmpCore.Options;

#nullable disable
namespace XmpCore;

public static class XmpMetaFactory
{
  public static IXmpSchemaRegistry SchemaRegistry { get; private set; } = (IXmpSchemaRegistry) new XmpSchemaRegistry();

  public static IXmpMeta Create() => (IXmpMeta) new XmpMeta();

  public static IXmpMeta Parse(Stream stream, ParseOptions options = null)
  {
    return XmpMetaParser.Parse(stream, options);
  }

  public static IXmpMeta ParseFromString(string packet, ParseOptions options = null)
  {
    return XmpMetaParser.Parse(packet, options);
  }

  public static IXmpMeta ParseFromBuffer(byte[] buffer, ParseOptions options = null)
  {
    return XmpMetaParser.Parse(buffer, options);
  }

  public static IXmpMeta ParseFromBuffer(
    byte[] buffer,
    int offset,
    int length,
    ParseOptions options = null)
  {
    return XmpMetaParser.Parse(new ByteBuffer(buffer, offset, length), options);
  }

  public static IXmpMeta ParseFromXDocument(XDocument root, ParseOptions options = null)
  {
    return XmpMetaParser.Parse(root, options);
  }

  public static XDocument ExtractXDocumentFromBuffer(byte[] buffer, ParseOptions options = null)
  {
    return XmpMetaParser.Extract(buffer, options);
  }

  public static void Serialize(IXmpMeta xmp, Stream stream, SerializeOptions options = null)
  {
    XmpMetaFactory.AssertImplementation(xmp);
    XmpSerializerHelper.Serialize((XmpMeta) xmp, stream, options);
  }

  public static byte[] SerializeToBuffer(IXmpMeta xmp, SerializeOptions options)
  {
    XmpMetaFactory.AssertImplementation(xmp);
    return XmpSerializerHelper.SerializeToBuffer((XmpMeta) xmp, options);
  }

  public static string SerializeToString(IXmpMeta xmp, SerializeOptions options)
  {
    XmpMetaFactory.AssertImplementation(xmp);
    return XmpSerializerHelper.SerializeToString((XmpMeta) xmp, options);
  }

  private static void AssertImplementation(IXmpMeta xmp)
  {
    if (!(xmp is XmpMeta))
      throw new NotSupportedException("The serializing service works only with the XmpMeta implementation of this library");
  }

  public static void Reset()
  {
    XmpMetaFactory.SchemaRegistry = (IXmpSchemaRegistry) new XmpSchemaRegistry();
  }

  public static IXmpVersionInfo VersionInfo
  {
    get
    {
      return (IXmpVersionInfo) new XmpMetaFactory.XmpVersionInfo(6, 1, 10, false, 3, "Adobe XMP Core 6.1.10");
    }
  }

  private sealed class XmpVersionInfo : IXmpVersionInfo
  {
    public int Major { get; }

    public int Minor { get; }

    public int Micro { get; }

    public bool IsDebug { get; }

    public int Build { get; }

    public string Message { get; }

    public XmpVersionInfo(
      int major,
      int minor,
      int micro,
      bool debug,
      int engBuild,
      string message)
    {
      this.Major = major;
      this.Minor = minor;
      this.Micro = micro;
      this.IsDebug = debug;
      this.Build = engBuild;
      this.Message = message;
    }

    public override string ToString() => this.Message;
  }
}
