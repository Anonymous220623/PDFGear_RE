// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.TextPositions
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Indicating where to position the text of the control caption relative to its icon
/// </summary>
public enum TextPositions
{
  /// <summary>No icon; caption only</summary>
  TEXTPOS_CAPTION,
  /// <summary>No caption; icon only</summary>
  TEXTPOS_ICON,
  /// <summary>Caption below the icon</summary>
  TEXTPOS_BELOW,
  /// <summary>Caption above the icon</summary>
  TEXTPOS_ABOVE,
  /// <summary>Caption to the right of the icon</summary>
  TEXTPOS_RIGHT,
  /// <summary>Caption to the left of the icon</summary>
  TEXTPOS_LEFT,
  /// <summary>Caption overlaid directly on the icon</summary>
  TEXTPOS_OVERLAID,
}
