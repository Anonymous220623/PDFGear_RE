// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.SaveFlags
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Flags for <see cref="M:Patagames.Pdf.Pdfium.FPDF_SaveAsCopy(System.IntPtr,Patagames.Pdf.FPDF_FILEWRITE,Patagames.Pdf.Enums.SaveFlags)" /> and <see cref="M:Patagames.Pdf.Pdfium.FPDF_SaveWithVersion(System.IntPtr,Patagames.Pdf.FPDF_FILEWRITE,Patagames.Pdf.Enums.SaveFlags,System.Int32)" />
/// </summary>
[Flags]
public enum SaveFlags
{
  /// <summary>Incremental save.</summary>
  Incremental = 1,
  /// <summary>Save with no incremental mode.</summary>
  NoIncremental = 2,
  /// <summary>Remove security and save with no incremental mode.</summary>
  RemoveSecurity = NoIncremental | Incremental, // 0x00000003
  /// <summary>Save to object stream.</summary>
  ObjectStream = 8,
  /// <summary>Do not save objects to which there are no references.</summary>
  /// <remarks>
  /// You can use this flag in order to enable the mechanism that removes the orphaned objects.
  /// What is the orphaned objects and where they come from is described in <a href="https://forum.patagames.com/posts/m1361findunread-PDF-documents--Orphaned-objects-and-references">this</a> article.
  /// </remarks>
  /// <seealso href="https://forum.patagames.com/posts/m1361findunread-PDF-documents--Orphaned-objects-and-references">PDF documents. Orphaned objects and references.</seealso>
  RemoveUnusedObjects = 16, // 0x00000010
  /// <summary>Include freed objects into the cross-reference table.</summary>
  /// <remarks>
  /// If this flag is set, then the correspond entries will be added to  the cross-reference table for each missing object.
  /// <note type="note">This flag is incompatible with <see cref="F:Patagames.Pdf.Enums.SaveFlags.Incremental" />.</note>
  /// </remarks>
  GenerateFreeEntries = 32, // 0x00000020
}
