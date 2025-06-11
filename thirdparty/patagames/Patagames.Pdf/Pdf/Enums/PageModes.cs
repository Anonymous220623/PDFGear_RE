// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.PageModes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Flags for page mode.</summary>
public enum PageModes
{
  /// <summary>Unknown value</summary>
  Unknown = -1, // 0xFFFFFFFF
  /// <summary>Neither document outline nor thumbnail images visible</summary>
  UseNone = 0,
  /// <summary>Document outline visible</summary>
  UseOutlines = 1,
  /// <summary>Thumbnial images visible</summary>
  UseThumbs = 2,
  /// <summary>
  /// Full-screen mode, with no menu bar, window controls, or any other window visible
  /// </summary>
  FullScreen = 3,
  /// <summary>Optional content group panel visible</summary>
  UseOc = 4,
  /// <summary>Attachments panel visible</summary>
  UseAttachments = 5,
}
