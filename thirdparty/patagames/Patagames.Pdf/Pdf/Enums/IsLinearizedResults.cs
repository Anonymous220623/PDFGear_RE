// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.IsLinearizedResults
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>The result of the process which check linearized PDF.</summary>
public enum IsLinearizedResults
{
  /// <summary>Unknownd linearized state</summary>
  UnknownLinearized = -1, // 0xFFFFFFFF
  /// <summary>Not linearized state</summary>
  NotLinearized = 0,
  /// <summary>Linearized state</summary>
  IsLinearized = 1,
}
