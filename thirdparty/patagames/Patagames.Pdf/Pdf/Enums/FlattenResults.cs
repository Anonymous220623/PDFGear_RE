// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FlattenResults
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// The result flag of the <see cref="M:Patagames.Pdf.Pdfium.FPDFPage_Flatten(System.IntPtr,Patagames.Pdf.Enums.FlattenFlags)" /> function
/// </summary>
public enum FlattenResults
{
  /// <summary>Flatten operation failed</summary>
  Fail,
  /// <summary>Flatten operation succeed.</summary>
  Success,
  /// <summary>There is nothing can be flatten</summary>
  NothingTodo,
}
