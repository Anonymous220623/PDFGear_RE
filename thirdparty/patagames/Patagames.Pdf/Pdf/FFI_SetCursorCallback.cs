// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FFI_SetCursorCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Set the Cursor shape</summary>
/// <param name="pThis">Pointer to the interface structure itself.</param>
/// <param name="nCursorType">Cursor type. see Flags for Cursor type for the details.</param>
/// <remarks>Required: Yes.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void FFI_SetCursorCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, CursorTypes nCursorType);
