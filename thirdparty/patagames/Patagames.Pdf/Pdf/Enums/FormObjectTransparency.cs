// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FormObjectTransparency
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>The transparency type</summary>
[Flags]
public enum FormObjectTransparency
{
  /// <summary>None</summary>
  None = 0,
  /// <summary>Group</summary>
  PDFTRANS_GROUP = 256, // 0x00000100
  /// <summary>Isolated</summary>
  PDFTRANS_ISOLATED = 512, // 0x00000200
  /// <summary>Knockout</summary>
  PDFTRANS_KNOCKOUT = 1024, // 0x00000400
}
