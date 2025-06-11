// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.AppearanceStreamModes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Represents an annotation appearance mode</summary>
/// <remarks>
/// An annotation can define as many as three separate appearances
/// </remarks>
public enum AppearanceStreamModes
{
  /// <summary>
  /// The normal appearance is used when the annotation is not interacting with the
  /// user. This appearance is also used for printing the annotation.
  /// </summary>
  Normal,
  /// <summary>
  /// The rollover appearance is used when the user moves the cursor into the annotation’s active area without pressing the mouse button.
  /// </summary>
  Rollover,
  /// <summary>
  /// The down appearance is used when the mouse button is pressed or held down within the annotation’s active area.
  /// </summary>
  Down,
}
