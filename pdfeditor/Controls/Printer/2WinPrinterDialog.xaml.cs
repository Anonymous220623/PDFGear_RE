// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Printer.PrintArgs
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Net;
using pdfeditor.Utils.Printer;
using PDFKit;
using System.Drawing.Printing;

#nullable disable
namespace pdfeditor.Controls.Printer;

public class PrintArgs
{
  public bool Grayscale { get; set; }

  public bool AutoCenter { get; set; }

  public bool AutoRotate { get; set; }

  public int Copies { get; set; } = 1;

  public int AllCount { get; set; } = 1;

  public PrintSizeMode PrintSizeMode { get; set; }

  public int Scale { get; set; } = 100;

  public string PrinterName { get; set; }

  public string DocumentPath { get; set; }

  public bool Collate { get; set; }

  public Duplex Duplex { get; set; }

  public PaperSize PaperSize { get; set; }

  public bool Landscape { get; set; }

  public PdfDocument Document { get; set; }

  public PrintDevModeHandle PrintDevMode { get; set; }

  public bool IsTempDocument { get; set; }

  public string DocumentTitle { get; set; }
}
