// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FFI_GetCurrentPageCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>This method receives the current page pointer.</summary>
/// <param name="pThis">Pointer to the interface structure itself.</param>
/// <param name="document">Handle to document. Returned by <see cref="M:Patagames.Pdf.Pdfium.FPDF_LoadDocument(System.String,System.String)" /> function.</param>
/// <returns>Handle to the page. Returned by <see cref="M:Patagames.Pdf.Pdfium.FPDF_LoadPage(System.IntPtr,System.Int32)" /> function.</returns>
/// <remarks>Required: Yes.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate IntPtr FFI_GetCurrentPageCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, IntPtr document);
