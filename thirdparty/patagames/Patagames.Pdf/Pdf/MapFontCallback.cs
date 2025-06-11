// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.MapFontCallback
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
/// Use the system font mapper to get a font handle from requested parameters
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself</param>
/// <param name="weight">Weight of the requested font. 400 is normal and 700 is bold.</param>
/// <param name="bItalic">Italic option of the requested font, TRUE or FALSE.</param>
/// <param name="charset">Character set identifier for the requested font. See above defined constants.</param>
/// <param name="pitch_family">A combination of flags. See above defined constants.</param>
/// <param name="face">Typeface name. Currently use system local encoding only.</param>
/// <param name="bExact">Pointer to an boolean value receiving the indicator whether mapper found the exact match. If mapper is not sure whether it's exact match, ignore this paramter.</param>
/// <returns>An opaque pointer for font handle, or IntPtr.Zero if system mapping is not supported.</returns>
/// <remarks>
/// Required only if <see cref="F:Patagames.Pdf.FPDF_SYSFONTINFO.GetFont" /> method is not implemented.
/// If the system supports native font mapper (like Windows), implementation can implement this method to get a font handle.
/// Otherwise, Pdfium SDK will do the mapping and then call GetFont method.
/// Only TrueType/OpenType and Type1 fonts are accepted by Pdfium SDK.
/// </remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate IntPtr MapFontCallback(
  [MarshalAs(UnmanagedType.LPStruct)] FPDF_SYSFONTINFO pThis,
  int weight,
  [MarshalAs(UnmanagedType.Bool)] bool bItalic,
  CharacterSetTypes charset,
  PitchFamilyFlags pitch_family,
  [MarshalAs(UnmanagedType.LPStr)] string face,
  [MarshalAs(UnmanagedType.Bool)] ref bool bExact);
