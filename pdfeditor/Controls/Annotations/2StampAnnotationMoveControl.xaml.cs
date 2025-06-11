// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.ImageStampModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Controls.Annotations;

public class ImageStampModel
{
  public string ImageFilePath { get; set; }

  public BitmapSource StampImageSource { get; set; }

  public double ImageWidth { get; set; }

  public double ImageHeight { get; set; }

  public FS_SIZEF PageSize { get; set; }
}
