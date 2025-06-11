// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.PageContents.PageContentFactory
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Net;

#nullable disable
namespace pdfeditor.Models.PageContents;

public static class PageContentFactory
{
  public static PageBaseObject Create(PdfPageObject pageObject)
  {
    if (pageObject == null)
      return (PageBaseObject) null;
    PageBaseObject model = (PageBaseObject) null;
    if (pageObject is PdfTextObject)
      model = (PageBaseObject) new PageTextObject();
    if (model != null)
      PageBaseObject.InitModelProperties(pageObject, model);
    return model;
  }
}
