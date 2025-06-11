// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.XmpSerializerHelper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.IO;
using XmpCore.Options;

#nullable disable
namespace XmpCore.Impl;

public static class XmpSerializerHelper
{
  public static void Serialize(XmpMeta xmp, Stream stream, SerializeOptions options)
  {
    options = options ?? new SerializeOptions();
    if (options.Sort)
      xmp.Sort();
    new XmpSerializerRdf().Serialize((IXmpMeta) xmp, stream, options);
  }

  public static string SerializeToString(XmpMeta xmp, SerializeOptions options)
  {
    options = options ?? new SerializeOptions();
    MemoryStream memoryStream = new MemoryStream(2048 /*0x0800*/);
    XmpSerializerHelper.Serialize(xmp, (Stream) memoryStream, options);
    try
    {
      return options.GetEncoding().GetString(memoryStream.ToArray(), 0, (int) memoryStream.Length);
    }
    catch
    {
      return memoryStream.ToString();
    }
  }

  public static byte[] SerializeToBuffer(XmpMeta xmp, SerializeOptions options)
  {
    MemoryStream memoryStream = new MemoryStream(2048 /*0x0800*/);
    XmpSerializerHelper.Serialize(xmp, (Stream) memoryStream, options);
    return memoryStream.ToArray();
  }
}
