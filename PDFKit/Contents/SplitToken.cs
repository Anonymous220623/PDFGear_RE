// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.SplitToken
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net;

#nullable disable
namespace PDFKit.Contents;

internal class SplitToken
{
  public PdfTextObject TextObject { get; set; }

  public PdfTextObject SplitNewTextObject { get; set; }

  public int ObjectIndex { get; set; }

  public int SplitCharAt { get; set; }
}
