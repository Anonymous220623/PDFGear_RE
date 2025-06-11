// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.ProgressiveRenderingStatuses
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Flags for progressive process status.</summary>
/// <remarks>
/// This enumreation is deprecated, please use <see cref="T:Patagames.Pdf.Enums.ProgressiveStatus" /> instead.
/// </remarks>
[Obsolete("This enumreation is deprecated, please use ProgressiveStatus instead", true)]
public enum ProgressiveRenderingStatuses
{
  /// <summary>Render reader</summary>
  RenderReader,
  /// <summary>Render paused</summary>
  RenderTobeContinued,
  /// <summary>Render done</summary>
  RenderDone,
  /// <summary>Render failed</summary>
  RenderFailed,
}
