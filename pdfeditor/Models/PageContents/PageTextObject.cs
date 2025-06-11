// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.PageContents.PageTextObject
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using System;

#nullable disable
namespace pdfeditor.Models.PageContents;

public class PageTextObject : PageBaseObject
{
  public override PageObjectTypes ObjectType => PageObjectTypes.PDFPAGE_TEXT;

  public float CharSpacing { get; protected set; }

  public float WordSpacing { get; protected set; }

  public PdfFont Font { get; protected set; }

  public float FontSize { get; protected set; }

  public TextRenderingModes RenderMode { get; protected set; }

  public FS_POINTF Location { get; protected set; }

  public bool TextKnockout { get; protected set; }

  public string TextUnicode { get; protected set; }

  protected override void ApplyCore(PdfPageObject pageObject)
  {
    base.ApplyCore(pageObject);
    if (!(pageObject is PdfTextObject pdfTextObject))
      return;
    pdfTextObject.CharSpacing = this.CharSpacing;
    pdfTextObject.WordSpacing = this.WordSpacing;
    pdfTextObject.Font = this.Font;
    pdfTextObject.FontSize = this.FontSize;
    pdfTextObject.RenderMode = this.RenderMode;
    pdfTextObject.Location = this.Location;
    pdfTextObject.TextKnockout = this.TextKnockout;
    pdfTextObject.TextUnicode = this.TextUnicode;
  }

  protected override void Init(PdfPageObject pageObject)
  {
    base.Init(pageObject);
    PdfTextObject textObject = pageObject as PdfTextObject;
    if (textObject == null)
      return;
    this.CharSpacing = PageBaseObject.ReturnValueOrDefault<float>((Func<float>) (() => textObject.CharSpacing));
    this.WordSpacing = PageBaseObject.ReturnValueOrDefault<float>((Func<float>) (() => textObject.WordSpacing));
    this.Font = PageBaseObject.ReturnValueOrDefault<PdfFont>((Func<PdfFont>) (() => textObject.Font));
    this.FontSize = PageBaseObject.ReturnValueOrDefault<float>((Func<float>) (() => textObject.FontSize));
    this.RenderMode = PageBaseObject.ReturnValueOrDefault<TextRenderingModes>((Func<TextRenderingModes>) (() => textObject.RenderMode));
    this.Location = PageBaseObject.ReturnValueOrDefault<FS_POINTF>((Func<FS_POINTF>) (() => textObject.Location));
    this.TextKnockout = PageBaseObject.ReturnValueOrDefault<bool>((Func<bool>) (() => textObject.TextKnockout));
    this.TextUnicode = PageBaseObject.ReturnValueOrDefault<string>((Func<string>) (() => textObject.TextUnicode));
  }

  protected override bool EqualsCore(PageBaseObject other)
  {
    return base.EqualsCore(other) && other is PageTextObject pageTextObject && (double) this.CharSpacing == (double) pageTextObject.CharSpacing && (double) this.WordSpacing == (double) pageTextObject.WordSpacing && this.Font == pageTextObject.Font && (double) this.FontSize == (double) pageTextObject.FontSize && this.RenderMode == pageTextObject.RenderMode && this.Location == pageTextObject.Location && this.TextKnockout == pageTextObject.TextKnockout && this.TextUnicode == pageTextObject.TextUnicode;
  }
}
