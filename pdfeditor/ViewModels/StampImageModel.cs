// Decompiled with JetBrains decompiler
// Type: pdfeditor.ViewModels.StampImageModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Controls.Annotations;

#nullable disable
namespace pdfeditor.ViewModels;

public class StampImageModel
{
  public string ImageFilePath { get; set; }

  public bool RemoveBackground { get; set; }

  public bool IsSignature { get; set; }

  public ImageStampModel ImageStampControlModel { get; set; }
}
