// Decompiled with JetBrains decompiler
// Type: XmpCore.IXmpProperty
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using XmpCore.Options;

#nullable disable
namespace XmpCore;

public interface IXmpProperty
{
  string Value { get; }

  PropertyOptions Options { get; }

  string Language { get; }
}
