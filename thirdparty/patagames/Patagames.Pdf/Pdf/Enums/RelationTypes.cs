// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.RelationTypes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Represents the relationship between annotations</summary>
/// <remarks>
/// In PDF 1.6, a set of annotations can be grouped so that they function as a single
/// unit when a user interacts with them.The group consists of a primary annotation,
/// which must not have an IRT entry, and one or more subordinate annotations,
/// which must have an <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.RelationshipAnnotation" /> property that
/// refers to the primary annotation and an <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.Relationship" /> property whose value is Group.
/// <para>
/// Some entries in the primary annotation are treated as “group attributes” that
/// should apply to the group as a whole; the corresponding properties in the subordinate annotations should be ignored.
/// These properties are <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.Contents" /> (or <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.RichText" /> and <see cref="P:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation.DefaultStyle" />),
/// <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.ModificationDate" />, <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.Color" />, <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.Text" />, <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.Popup" />, <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.CreationDate" />, <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.Subject" />, and <see cref="P:Patagames.Pdf.Net.Annotations.PdfPopupAnnotation.IsOpen" />.
/// Operations that manipulate any annotation in a group, such as movement, cut, and copy, should be treated by
/// viewer applications as acting on the entire group.
/// </para>
/// <note type="note"> primary annotation may have replies that are not subordinate annotation
/// that is, that do not have an <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.Relationship" /> value of Group</note>
/// </remarks>
public enum RelationTypes
{
  /// <summary>The relationship is not specified;</summary>
  NonSpecified,
  /// <summary>
  /// The annotation is considered a reply to the annotation specified by <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.RelationshipAnnotation" />.
  /// Viewer applications should not display replies to an annotation individually but together in the form of threaded comments;
  /// see remarks section.
  /// </summary>
  Reply,
  /// <summary>
  /// The annotation is grouped with the annotation specified by <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.RelationshipAnnotation" />
  /// </summary>
  Group,
}
