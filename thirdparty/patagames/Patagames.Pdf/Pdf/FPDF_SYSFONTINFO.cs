// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FPDF_SYSFONTINFO
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// Interface for getting system font information and font mapping
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public class FPDF_SYSFONTINFO : IStaticCallbacks
{
  /// <summary>Version number of the interface. Currently must be 1.</summary>
  public int Version = 1;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.ReleaseCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: No.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public ReleaseFontsCallback Release;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.EnumFontsCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: No.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public EnumFontsCallback EnumFonts;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.MapFontCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required only if <see cref="F:Patagames.Pdf.FPDF_SYSFONTINFO.GetFont" /> method is not implemented.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public MapFontCallback MapFont;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.GetFontCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required only if <see cref="F:Patagames.Pdf.FPDF_SYSFONTINFO.MapFont" /> method is not implemented</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public GetFontCallback GetFont;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.GetFontDataCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public GetFontDataCallback GetFontData;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.GetFaceNameCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: No.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public GetFaceNameCallback GetFaceName;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.GetFontCharsetCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: No.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public GetFontCharsetCallback GetFontCharset;
  /// <summary>
  /// Application defined callback function. See <see cref="F:Patagames.Pdf.FPDF_SYSFONTINFO.DeleteFont" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public DeleteFontCallback DeleteFont;
}
