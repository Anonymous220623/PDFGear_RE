// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.DocumentPasswordRequestedEventArgs
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;

#nullable disable
namespace pdfeditor.Models;

public class DocumentPasswordRequestedEventArgs : EventArgs
{
  public string Password { get; set; }

  public bool Cancel { get; set; }

  public string FileName { get; set; }
}
