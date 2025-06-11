// Decompiled with JetBrains decompiler
// Type: pdfeditor.AutoSaveRestore.AutoSaveModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace pdfeditor.AutoSaveRestore;

public class AutoSaveModel
{
  public int SpanMinutes { get; set; }

  public bool IsAuto { get; set; }

  public string SourceFileName { get; set; }
}
