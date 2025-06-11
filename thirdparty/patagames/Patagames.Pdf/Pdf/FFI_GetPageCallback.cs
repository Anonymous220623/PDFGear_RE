// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FFI_GetPageCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// This method receives the page pointer associated with a specified page index.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself.</param>
/// <param name="document">Handle to document. Returned by <see cref="M:Patagames.Pdf.Pdfium.FPDF_LoadDocument(System.String,System.String)" /> function.</param>
/// <param name="nPageIndex">Index number of the page. 0 for the first page.</param>
/// <returns>Handle to the page. Returned by <see cref="M:Patagames.Pdf.Pdfium.FPDF_LoadPage(System.IntPtr,System.Int32)" /> function.</returns>
/// <remarks>Required: Yes. In some cases, the document-level JavaScript action may refer to a page which hadn't been loaded yet.
/// To successfully run the javascript action, implementation need to load the page for SDK.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate IntPtr FFI_GetPageCallback(
  [MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis,
  IntPtr document,
  int nPageIndex);
