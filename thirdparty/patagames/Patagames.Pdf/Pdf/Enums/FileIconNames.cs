// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FileIconNames
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Represents the name of an icon to be used in displaying the file attachment annotation.
/// </summary>
/// <remarks>
/// Viewer applications should provide predefined icon appearances for at least the following standard names.
/// <para>Additional names may be supported as well.</para>
/// </remarks>
public enum FileIconNames
{
  /// <summary>Note icon (Default)</summary>
  [Description("PushPin")] PushPin,
  /// <summary>Graph icon</summary>
  [Description("Graph")] Graph,
  /// <summary>Paper clip icon</summary>
  [Description("Paperclip")] Paperclip,
  /// <summary>Tag icon</summary>
  [Description("Tag")] Tag,
  /// <summary>
  /// Please see <see cref="P:Patagames.Pdf.Net.Annotations.PdfFileAttachmentAnnotation.ExtendedIconName" /> property
  /// </summary>
  Extended,
}
