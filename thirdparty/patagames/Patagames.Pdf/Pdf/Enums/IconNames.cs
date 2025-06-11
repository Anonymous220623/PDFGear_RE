// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.IconNames
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Represents the name of an icon to be used in displaying the annotation.
/// </summary>
/// <remarks>
/// Viewer applications should provide predefined icon appearances for at least the following standard names.
/// <para>Additional names may be supported as well. Default value: Note.</para>
/// </remarks>
public enum IconNames
{
  /// <summary>Note icon (Default)</summary>
  [Description("Note")] Note,
  /// <summary>Comment icon</summary>
  [Description("Comment")] Comment,
  /// <summary>Key icon</summary>
  [Description("Key")] Key,
  /// <summary>Help icon</summary>
  [Description("Help")] Help,
  /// <summary>Mew paragraph icon</summary>
  [Description("NewParagraph")] NewParagraph,
  /// <summary>Paragraph icon</summary>
  [Description("Paragraph")] Paragraph,
  /// <summary>Insert icon</summary>
  [Description("Insert")] Insert,
  /// <summary>
  /// Please see <see cref="P:Patagames.Pdf.Net.Annotations.PdfTextAnnotation.ExtendedIconName" /> property
  /// </summary>
  Extended,
}
