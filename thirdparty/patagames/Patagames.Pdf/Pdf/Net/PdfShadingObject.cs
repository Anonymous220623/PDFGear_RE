// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfShadingObject
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents a shading object.</summary>
public class PdfShadingObject : PdfPageObject
{
  /// <summary>Get shading pattern.</summary>
  private IntPtr Pattern
  {
    get => Pdfium.FPDFShadingObj_GetShaddingPattern(this.Handle);
    set => Pdfium.FPDFShadingObj_SetShaddingPattern(this.Handle, value);
  }

  internal PdfShadingObject(IntPtr handle)
    : base(handle)
  {
  }

  /// <summary>Create new instance of PdfShadingObject class</summary>
  /// <returns>New instance of PdfShadingObject</returns>
  public static PdfShadingObject Create()
  {
    IntPtr handle = PdfPageObject.CreateObject(PageObjectTypes.PDFPAGE_SHADING);
    return handle == IntPtr.Zero ? (PdfShadingObject) null : new PdfShadingObject(handle);
  }

  /// <summary>Calculate bounding box</summary>
  public void CalcBoundingBox() => Pdfium.FPDFShadingObj_CalcBoundingBox(this.Handle);
}
