// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.IsDataAvailableCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// Report whether the specified data section is available. A section is available only if all bytes in the section is available.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself.</param>
/// <param name="offset">The offset of the data section in the file.</param>
/// <param name="size">The size of the data section</param>
/// <returns>true means the specified data section is available.</returns>
/// <remarks>Required: Yes. Called by Pdfium SDK to check whether the data section is ready.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.I1)]
public delegate bool IsDataAvailableCallback([MarshalAs(UnmanagedType.LPStruct)] FX_FILEAVAIL pThis, IntPtr offset, IntPtr size);
