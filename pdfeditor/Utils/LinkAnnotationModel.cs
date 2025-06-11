// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.LinkAnnotationModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using pdfeditor.ViewModels;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Utils;

public class LinkAnnotationModel
{
  public string Title = "";

  public LinkSelect Action { get; set; }

  public BorderStyles BorderStyle { get; set; }

  public float Width { get; set; }

  public Color BorderColor { get; set; }

  public string Uri { get; set; }

  public int Page { get; set; }

  public PdfDocument PdfDocument { get; set; }

  public string FileName { get; set; }
}
