// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.ReleaseFontsCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// Give implementation a chance to release any data after the interface is no longer used
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself</param>
/// <remarks>Required: No. Called by Pdfium SDK during the final cleanup process.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void ReleaseFontsCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_SYSFONTINFO pThis);
