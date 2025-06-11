// Decompiled with JetBrains decompiler
// Type: XmpCore.XmpException
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;

#nullable disable
namespace XmpCore;

public sealed class XmpException : Exception
{
  public XmpErrorCode ErrorCode { get; }

  public XmpException(string message, XmpErrorCode errorCode)
    : base(message)
  {
    this.ErrorCode = errorCode;
  }

  public XmpException(string message, XmpErrorCode errorCode, Exception innerException)
    : base(message, innerException)
  {
    this.ErrorCode = errorCode;
  }
}
