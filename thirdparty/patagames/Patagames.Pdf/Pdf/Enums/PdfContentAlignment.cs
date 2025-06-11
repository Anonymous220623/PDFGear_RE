// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.PdfContentAlignment
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Specifies alignment of content.</summary>
public enum PdfContentAlignment
{
  /// <summary>
  /// Content is vertically aligned at the top, and horizontally aligned on the left.
  /// </summary>
  TopLeft = 1,
  /// <summary>
  /// Content is vertically aligned at the top, and horizontally aligned at the center.
  /// </summary>
  TopCenter = 2,
  /// <summary>
  /// Content is vertically aligned at the top, and horizontally aligned on the right.
  /// </summary>
  TopRight = 4,
  /// <summary>
  /// Content is vertically aligned in the middle, and horizontally aligned on the left.
  /// </summary>
  MiddleLeft = 16, // 0x00000010
  /// <summary>
  /// Content is vertically aligned in the middle, and horizontally aligned at the center.
  /// </summary>
  MiddleCenter = 32, // 0x00000020
  /// <summary>
  /// Content is vertically aligned in the middle, and horizontally aligned on the right.
  /// </summary>
  MiddleRight = 64, // 0x00000040
  /// <summary>
  /// Content is vertically aligned at the bottom, and horizontally aligned on the left.
  /// </summary>
  BottomLeft = 256, // 0x00000100
  /// <summary>
  /// Content is vertically aligned at the bottom, and horizontally aligned at the center.
  /// </summary>
  BottomCenter = 512, // 0x00000200
  /// <summary>
  /// Content is vertically aligned at the bottom, and horizontally aligned on the right.
  /// </summary>
  BottomRight = 1024, // 0x00000400
}
