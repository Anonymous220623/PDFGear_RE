// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Print.PrintSettings
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Newtonsoft.Json;
using System.Drawing.Printing;

#nullable disable
namespace pdfeditor.Models.Print;

public class PrintSettings
{
  [JsonIgnore]
  public string Device { get; set; }

  public PaperSize Paper { get; set; }

  public bool IsGrayscale { get; set; }

  public Duplex Duplex { get; set; } = Duplex.Simplex;

  public bool Landscape { get; set; }
}
