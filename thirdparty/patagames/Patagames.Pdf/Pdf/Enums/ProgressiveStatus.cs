// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.ProgressiveStatus
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Flags for progressive process status.</summary>
public enum ProgressiveStatus
{
  /// <summary>Ready for start progressive process.</summary>
  Ready,
  /// <summary>The process should be continued.</summary>
  ToBeContinued,
  /// <summary>The process is done.</summary>
  Done,
  /// <summary>The process is failed.</summary>
  Failed,
}
