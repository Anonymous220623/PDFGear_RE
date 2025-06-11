// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.StampIconNames
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Represents the name of an icon to be used in displaying the <see cref="T:Patagames.Pdf.Net.Annotations.PdfStampAnnotation" />
/// </summary>
/// <remarks>
/// 
/// Viewer applications should provide predefined icon appearances for at least the following standard names
/// </remarks>
public enum StampIconNames
{
  /// <summary>Draft stamp icon</summary>
  [Description("Draft")] Draft,
  /// <summary>Approved stamp icon</summary>
  [Description("Approved")] Approved,
  /// <summary>Experimental stamp icon</summary>
  [Description("Experimental")] Experimental,
  /// <summary>NotApproved stamp icon</summary>
  [Description("NotApproved")] NotApproved,
  /// <summary>AsIs stamp icon</summary>
  [Description("AsIs")] AsIs,
  /// <summary>Expired stamp icon</summary>
  [Description("Expired")] Expired,
  /// <summary>NotForPublicRelease stamp icon</summary>
  [Description("NotForPublicRelease")] NotForPublicRelease,
  /// <summary>Confidential stamp icon</summary>
  [Description("Confidential")] Confidential,
  /// <summary>Final stamp icon</summary>
  [Description("Final")] Final,
  /// <summary>Sold stamp icon</summary>
  [Description("Sold")] Sold,
  /// <summary>Departmental stamp icon</summary>
  [Description("Departmental")] Departmental,
  /// <summary>ForComment stamp icon</summary>
  [Description("ForComment")] ForComment,
  /// <summary>TopSecret stamp icon</summary>
  [Description("TopSecret")] TopSecret,
  /// <summary>ForPublicRelease stamp icon</summary>
  [Description("ForPublicRelease")] ForPublicRelease,
  /// <summary>
  /// Please see <see cref="P:Patagames.Pdf.Net.Annotations.PdfStampAnnotation.ExtendedIconName" /> property
  /// </summary>
  Extended,
}
