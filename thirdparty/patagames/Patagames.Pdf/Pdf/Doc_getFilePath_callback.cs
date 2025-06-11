// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Doc_getFilePath_callback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Get the file path of the current document.</summary>
/// <param name="pThis">Pointer to the interface structure itself</param>
/// <param name="filePath">The string buffer to receive the file path. Can be NULL.</param>
/// <param name="length">The length of the buffer, number of bytes. Can be 0.</param>
/// <returns>Number of bytes the filePath consumes, including trailing zeros.</returns>
/// <remarks>
/// Required: Yes.
/// The filePath should be always input in local encoding.
/// The return value always indicated number of bytes required for the buffer, even when there is
/// no buffer specified, or the buffer size is less then required. In this case, the buffer will not be modified.
/// </remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int Doc_getFilePath_callback([MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis, IntPtr filePath, int length);
