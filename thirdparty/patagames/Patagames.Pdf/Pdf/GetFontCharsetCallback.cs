// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.GetFontCharsetCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Get character set information for a font handle</summary>
/// <param name="pThis">Pointer to the interface structure itself</param>
/// <param name="hFont">Font handle returned by <see cref="F:Patagames.Pdf.FPDF_SYSFONTINFO.MapFont" /> or <see cref="F:Patagames.Pdf.FPDF_SYSFONTINFO.GetFont" /> method</param>
/// <returns>Character set identifier. See defined constants above.</returns>
/// <remarks>Required: No.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate CharacterSetTypes GetFontCharsetCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_SYSFONTINFO pThis, IntPtr hFont);
