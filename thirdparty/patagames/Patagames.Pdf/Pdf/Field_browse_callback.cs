// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Field_browse_callback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// Show a file selection dialog, and return the selected file path.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself.</param>
/// <param name="filePath">Pointer to the data buffer to receive the file path.Can be NULL.</param>
/// <param name="length">The length of the buffer, number of bytes. Can be 0.</param>
/// <returns>Number of bytes the filePath consumes, including trailing zeros.</returns>
/// <remarks>Required: Yes. The filePath shoule be always input in local encoding.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int Field_browse_callback([MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis, IntPtr filePath, int length);
