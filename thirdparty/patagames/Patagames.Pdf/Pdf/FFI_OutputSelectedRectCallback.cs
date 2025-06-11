// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FFI_OutputSelectedRectCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// When user is taking the mouse to select texts on a form field, this callback function will keep returning the selected areas to the implementation.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself.</param>
/// <param name="page">Handle to the page. Returned by <see cref="M:Patagames.Pdf.Pdfium.FPDF_LoadPage(System.IntPtr,System.Int32)" /> function.</param>
/// <param name="left">Left position of the client area in PDF page coordinate.</param>
/// <param name="top">Top  position of the client area in PDF page coordinate.</param>
/// <param name="right">Right position of the client area in PDF page  coordinate.</param>
/// <param name="bottom">Bottom position of the client area in PDF page coordinate.</param>
/// <remarks>Required: No. This CALLBACK function is useful for implementing special text selection effect. Implementation should
/// first records the returned rectangles, then draw them one by one at the painting period, last,remove all
/// the recorded rectangles when finish painting.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void FFI_OutputSelectedRectCallback(
  [MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis,
  IntPtr page,
  [MarshalAs(UnmanagedType.R8)] double left,
  [MarshalAs(UnmanagedType.R8)] double top,
  [MarshalAs(UnmanagedType.R8)] double right,
  [MarshalAs(UnmanagedType.R8)] double bottom);
