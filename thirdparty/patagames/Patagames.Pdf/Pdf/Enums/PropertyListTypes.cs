// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.PropertyListTypes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Represents the type of the property list of marked content (<see cref="P:Patagames.Pdf.Net.PdfMarkedContent.Parameters" />)
/// </summary>
public enum PropertyListTypes
{
  /// <summary>Has no any parameters</summary>
  None,
  /// <summary>
  /// Indicates that the values are indirect references to objects outside the content stream, the <see cref="P:Patagames.Pdf.Net.PdfMarkedContent.Parameters" />
  /// dictionary must be defined as a named resource in the Properties subdictionary of the current resource dictionary.
  /// </summary>
  PropertiesDict,
  /// <summary>
  /// If all of the values in a property list dictionary are direct objects, the dictionary
  /// may be written inline in the content stream as a direct object.
  /// </summary>
  DirectDict,
}
