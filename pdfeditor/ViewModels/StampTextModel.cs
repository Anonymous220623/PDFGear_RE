// Decompiled with JetBrains decompiler
// Type: pdfeditor.ViewModels.StampTextModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;

#nullable disable
namespace pdfeditor.ViewModels;

public class StampTextModel : IStampTextModel
{
  public string TextContent { get; set; }

  public string FontColor { get; set; }

  public string GroupId { get; set; }

  public bool IsPreset => false;

  public DateTime dateTime { get; set; }
}
