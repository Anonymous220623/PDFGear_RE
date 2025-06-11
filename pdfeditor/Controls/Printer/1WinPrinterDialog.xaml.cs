// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Printer.PaperSizeInfo
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Drawing.Printing;

#nullable disable
namespace pdfeditor.Controls.Printer;

public class PaperSizeInfo
{
  public string FriendlyName { get; set; }

  public PaperSize PaperSize { get; set; }

  public override string ToString()
  {
    return this.PaperSize != null ? $"{this.FriendlyName} ({Convert.ToInt32((double) this.PaperSize.Width / (500.0 / (double) sbyte.MaxValue))}*{Convert.ToInt32((double) this.PaperSize.Height / (500.0 / (double) sbyte.MaxValue))}mm)" : this.FriendlyName;
  }
}
