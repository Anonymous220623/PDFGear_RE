// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FFI_InvalidateCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// Invalidate the client area within the specified rectangle.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself.</param>
/// <param name="page">Handle to the page. Returned by <see cref="M:Patagames.Pdf.Pdfium.FPDF_LoadPage(System.IntPtr,System.Int32)" /> function.</param>
/// <param name="left">Left position of the client area in PDF page coordinate.</param>
/// <param name="top">Top  position of the client area in PDF page coordinate.</param>
/// <param name="right">Right position of the client area in PDF page  coordinate.</param>
/// <param name="bottom">Bottom position of the client area in PDF page coordinate.</param>
/// <remarks>Required" Yes. All positions are measured in PDF "user space". Implementation should call <see cref="M:Patagames.Pdf.Pdfium.FPDF_RenderPageBitmap(System.IntPtr,System.IntPtr,System.Int32,System.Int32,System.Int32,System.Int32,Patagames.Pdf.Enums.PageRotate,Patagames.Pdf.Enums.RenderFlags)" /> function for repainting a specified page area.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void FFI_InvalidateCallback(
  [MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis,
  IntPtr page,
  [MarshalAs(UnmanagedType.R8)] double left,
  [MarshalAs(UnmanagedType.R8)] double top,
  [MarshalAs(UnmanagedType.R8)] double right,
  [MarshalAs(UnmanagedType.R8)] double bottom);
