// Decompiled with JetBrains decompiler
// Type: Tesseract.IResultRenderer
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using System;

#nullable disable
namespace Tesseract;

public interface IResultRenderer : IDisposable
{
  IDisposable BeginDocument(string title);

  bool AddPage(Page page);

  int PageNumber { get; }
}
