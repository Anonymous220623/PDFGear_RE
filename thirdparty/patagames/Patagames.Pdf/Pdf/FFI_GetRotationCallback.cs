// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FFI_GetRotationCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// This method receives currently rotation of the page view.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself.</param>
/// <param name="page">Handle to page. Returned by <see cref="M:Patagames.Pdf.Pdfium.FPDF_LoadPage(System.IntPtr,System.Int32)" /> function.</param>
/// <returns>The page rotation. Should be 0(0 degree),1(90 degree),2(180 degree),3(270 degree), in a clockwise direction.</returns>
/// <remarks>Required: Yes.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate PageRotate FFI_GetRotationCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, IntPtr page);
