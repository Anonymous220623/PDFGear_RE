// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FillModes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Fill modes</summary>
[Flags]
public enum FillModes
{
  /// <summary>None</summary>
  None = 0,
  /// <summary>Specifies the alternate fill mode.</summary>
  Alternate = 1,
  /// <summary>Specifies the winding fill mode.</summary>
  Winding = 2,
  FullCover = 4,
  RectangleAA = 8,
  Stroke = 16, // 0x00000010
  Adjust = 32, // 0x00000020
  StrokeTextMode = 64, // 0x00000040
  FillTextMode = 128, // 0x00000080
  ZeroArea = 256, // 0x00000100
  NoPathSmooth = 512, // 0x00000200
}
