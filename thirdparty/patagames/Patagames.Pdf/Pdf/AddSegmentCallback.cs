// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.AddSegmentCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Add a section to be downloaded.</summary>
/// <param name="pThis">Pointer to the interface structure itself.</param>
/// <param name="offset">The offset of the hint reported to be downloaded.</param>
/// <param name="size">The size of the hint reported to be downloaded.</param>
/// <remarks>Required: Yes. Called by Pdfium SDK to report some downloading hints for download manager.
/// The position and size of section may be not accurate, part of the section might be already available.
/// The download manager must deal with that to maximize download efficiency.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void AddSegmentCallback([MarshalAs(UnmanagedType.LPStruct)] FX_DOWNLOADHINTS pThis, IntPtr offset, IntPtr size);
