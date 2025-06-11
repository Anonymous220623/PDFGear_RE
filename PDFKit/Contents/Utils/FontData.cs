// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Utils.FontData
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net;

#nullable disable
namespace PDFKit.Contents.Utils;

public class FontData
{
  public FontData(string fontFamily) => this.FontFamily = fontFamily;

  internal FontData(PdfFont font) => this.PdfFont = font;

  public PdfFont PdfFont { get; }

  public string FontFamily { get; internal set; }

  public BoldItalicFlags FontStyle { get; internal set; }
}
