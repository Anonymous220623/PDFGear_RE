// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.DocumentActionTypes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Additional actions type of document.</summary>
public enum DocumentActionTypes
{
  /// <summary>WC, before closing document, JavaScript action.</summary>
  BeforeClose = 16, // 0x00000010
  /// <summary>WS, before saving document, JavaScript action.</summary>
  BeforeSave = 17, // 0x00000011
  /// <summary>DS, after saving document, JavaScript action.</summary>
  AfterSave = 18, // 0x00000012
  /// <summary>WP, before printing document, JavaScript action.</summary>
  BeforePrint = 19, // 0x00000013
  /// <summary>DP, after printing document, JavaScript action.</summary>
  AfterPrint = 20, // 0x00000014
}
