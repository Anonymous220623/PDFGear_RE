// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.OverprintModes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Represents overprint modes.</summary>
public enum OverprintModes
{
  /// <summary>
  /// When the overprint mode is Zero, each source color component value replaces the value previously painted for the corresponding device colorant, no matter what the new value is.
  /// </summary>
  Zero,
  /// <summary>
  /// When the overprint mode is NonZero, a tint value of 0.0 for a source color component leaves the corresponding component of the previously painted color unchanged.
  /// </summary>
  /// <remarks>
  /// <para>The effect is equivalent to painting in a DeviceN color space that includes only those components whose values are nonzero.</para>
  /// <para>Nonzero overprint mode applies only to painting operations that use the current color in the graphics state when the current color space is DeviceCMYK (or is implicitly converted to DeviceCMYK.
  ///  It does not apply to the painting of images or to any colors that are the result of a computation, such as those in a shading pattern or conversions from some other color space.
  ///  It also does not apply if the device’s native color space is not DeviceCMYK; in that case, source colors must be converted to the
  ///  device’s native color space, and all components participate in the conversion, whatever their values.
  /// </para>
  /// </remarks>
  NonZero,
}
