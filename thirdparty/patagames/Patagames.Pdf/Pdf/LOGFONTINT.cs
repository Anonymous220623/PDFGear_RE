// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.LOGFONTINT
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// The LOGFONT structure defines the attributes of a font.
/// </summary>
/// <remarks>
/// Please refer <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd145037(v=vs.85).aspx">MSDN</a> for more details
/// </remarks>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal class LOGFONTINT
{
  /// <summary>
  /// The height, in logical units, of the font's character cell or character. The character height value (also known as the em height) is the character cell height value minus the internal-leading value. The font mapper interprets the value specified in lfHeight in the following manner.
  /// </summary>
  public int lfHeight;
  /// <summary>
  /// The average width, in logical units, of characters in the font. If lfWidth is zero, the aspect ratio of the device is matched against the digitization aspect ratio of the available fonts to find the closest match, determined by the absolute value of the difference.
  /// </summary>
  public int lfWidth;
  /// <summary>
  /// The angle, in tenths of degrees, between the escapement vector and the x-axis of the device. The escapement vector is parallel to the base line of a row of text.
  /// </summary>
  public int lfEscapement;
  /// <summary>
  /// The angle, in tenths of degrees, between each character's base line and the x-axis of the device.
  /// </summary>
  public int lfOrientation;
  /// <summary>
  /// The weight of the font in the range 0 through 1000. For example, 400 is normal and 700 is bold. If this value is zero, a default weight is used.
  /// </summary>
  public FontWeight lfWeight;
  /// <summary>An italic font if set to TRUE.</summary>
  [MarshalAs(UnmanagedType.U1)]
  public bool lfItalic;
  /// <summary>An underlined font if set to TRUE.</summary>
  [MarshalAs(UnmanagedType.U1)]
  public bool lfUnderline;
  /// <summary>A strikeout font if set to TRUE.</summary>
  [MarshalAs(UnmanagedType.U1)]
  public bool lfStrikeOut;
  /// <summary>The character set.</summary>
  public FontCharSet lfCharSet;
  /// <summary>
  /// The output precision. The output precision defines how closely the output must match the requested font's height, width, character orientation, escapement, pitch, and font type.
  /// </summary>
  public LOGFONT.FontPrecision lfOutPrecision;
  /// <summary>
  /// The clipping precision. The clipping precision defines how to clip characters that are partially outside the clipping region.
  /// </summary>
  public LOGFONT.FontClipPrecision lfClipPrecision;
  /// <summary>
  /// The output quality. The output quality defines how carefully the graphics device interface (GDI) must attempt to match the logical-font attributes to those of an actual physical font.
  /// </summary>
  public LOGFONT.FontQuality lfQuality;
  /// <summary>
  /// The pitch and family of the font. The two low-order bits specify the pitch of the font.
  /// </summary>
  public LOGFONT.FontPitchAndFamily lfPitchAndFamily;
  /// <summary>
  /// A string that specifies the typeface name of the font. The length of this string must not exceed 32 TCHAR values, including the terminating NULL.  If lfFaceName is an empty string, GDI uses the first font that matches the other specified attributes.
  /// </summary>
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32 /*0x20*/)]
  public string lfFaceName;

  /// <summary>Returns a string that represents the LOGFONT object.</summary>
  /// <returns></returns>
  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("LOGFONT\n");
    stringBuilder.AppendFormat("   lfHeight: {0}\n", (object) this.lfHeight);
    stringBuilder.AppendFormat("   lfWidth: {0}\n", (object) this.lfWidth);
    stringBuilder.AppendFormat("   lfEscapement: {0}\n", (object) this.lfEscapement);
    stringBuilder.AppendFormat("   lfOrientation: {0}\n", (object) this.lfOrientation);
    stringBuilder.AppendFormat("   lfWeight: {0}\n", (object) this.lfWeight);
    stringBuilder.AppendFormat("   lfItalic: {0}\n", (object) this.lfItalic);
    stringBuilder.AppendFormat("   lfUnderline: {0}\n", (object) this.lfUnderline);
    stringBuilder.AppendFormat("   lfStrikeOut: {0}\n", (object) this.lfStrikeOut);
    stringBuilder.AppendFormat("   lfCharSet: {0}\n", (object) this.lfCharSet);
    stringBuilder.AppendFormat("   lfOutPrecision: {0}\n", (object) this.lfOutPrecision);
    stringBuilder.AppendFormat("   lfClipPrecision: {0}\n", (object) this.lfClipPrecision);
    stringBuilder.AppendFormat("   lfQuality: {0}\n", (object) this.lfQuality);
    stringBuilder.AppendFormat("   lfPitchAndFamily: {0}\n", (object) this.lfPitchAndFamily);
    stringBuilder.AppendFormat("   lfFaceName: {0}\n", (object) this.lfFaceName);
    return stringBuilder.ToString();
  }

  public LOGFONTINT(LOGFONT font)
  {
    this.lfCharSet = font.lfCharSet;
    this.lfFaceName = font.lfFaceName;
    this.lfItalic = font.lfItalic;
    this.lfPitchAndFamily = font.lfPitchAndFamily;
    this.lfWeight = font.lfWeight;
    this.lfOutPrecision = font.lfOutPrecision;
    this.lfClipPrecision = font.lfClipPrecision;
    this.lfQuality = font.lfQuality;
  }
}
