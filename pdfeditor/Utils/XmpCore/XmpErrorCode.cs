// Decompiled with JetBrains decompiler
// Type: XmpCore.XmpErrorCode
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace XmpCore;

public enum XmpErrorCode
{
  Unknown = 0,
  BadParam = 4,
  BadValue = 5,
  InternalFailure = 9,
  BadSchema = 101, // 0x00000065
  BadXPath = 102, // 0x00000066
  BadOptions = 103, // 0x00000067
  BadIndex = 104, // 0x00000068
  BadSerialize = 107, // 0x0000006B
  BadXml = 201, // 0x000000C9
  BadRdf = 202, // 0x000000CA
  BadXmp = 203, // 0x000000CB
  BadStream = 204, // 0x000000CC
}
