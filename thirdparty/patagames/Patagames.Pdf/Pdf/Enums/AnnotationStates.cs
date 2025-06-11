// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.AnnotationStates
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Represents the annotation states</summary>
/// <remarks>
/// Beginning with PDF 1.5, annotations may have an author-specific state associated
/// with them. The state is not specified in the annotation itself but in a separate text
/// annotation that refers to the original annotation by means of its <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.RelationshipAnnotation" /> property.
/// States are grouped into a number of <see cref="T:Patagames.Pdf.Enums.StateModels" />.
/// <para>
/// Annotations can be thought of as initially being in the default state for each state model.
/// State changes made by a user are indicated in a text annotation with the following entries:
/// <list type="bullet">
/// <item>The <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.Text" /> property specifies the user.</item>
/// <item>The <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.RelationshipAnnotation" /> property refers to the original annotation.</item>
/// <item><see cref="P:Patagames.Pdf.Net.Annotations.PdfTextAnnotation.State" /> and <see cref="P:Patagames.Pdf.Net.Annotations.PdfTextAnnotation.StateModel" /> update the state of the original annotation for the specified user.</item>
/// </list>
/// </para>
/// Additional state changes are made by adding text annotations in reply to the previous reply for a given user.
/// </remarks>
public enum AnnotationStates
{
  /// <summary>
  /// (Marked) The annotation has not been marked by the user (the default for the Marked sate model)
  /// </summary>
  [Description("Unmarked")] Unmarked,
  /// <summary>(Marked) The annotation has been marked by the user.</summary>
  [Description("Marked")] Marked,
  /// <summary>
  /// (Review) The user has indicated nothing about the change (the default for the Review state model).
  /// </summary>
  [Description("None")] None,
  /// <summary>(Review) The user agrees with the change.</summary>
  [Description("Accepted")] Accepted,
  /// <summary>(Review) The user disagrees with the change.</summary>
  [Description("Rejected")] Rejected,
  /// <summary>(Review) The change has been cancelled.</summary>
  [Description("Cancelled")] Cancelled,
  /// <summary>(Review) The change has been completed.</summary>
  [Description("Completed")] Completed,
}
