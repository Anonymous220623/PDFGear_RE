// Decompiled with JetBrains decompiler
// Type: XmpCore.IXmpVersionInfo
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace XmpCore;

public interface IXmpVersionInfo
{
  int Major { get; }

  int Minor { get; }

  int Micro { get; }

  int Build { get; }

  bool IsDebug { get; }

  string Message { get; }
}
