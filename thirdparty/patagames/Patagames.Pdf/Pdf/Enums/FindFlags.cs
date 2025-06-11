// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FindFlags
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Flags used by <see cref="M:Patagames.Pdf.Pdfium.FPDFText_FindStart(System.IntPtr,System.String,Patagames.Pdf.Enums.FindFlags,System.Int32)" /> function.
/// </summary>
[Flags]
public enum FindFlags
{
  /// <summary>Not set any flags</summary>
  None = 0,
  /// <summary>If not set, it will not match case by default</summary>
  MatchCase = 1,
  /// <summary>
  /// If not set, it will not match the whole word by default.
  /// </summary>
  MatchWholeWord = 2,
}
