// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfMarkedContent
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Represents the item of the <see cref="T:Patagames.Pdf.Net.PdfMarkedContentCollection" /> collection.
/// </summary>
public class PdfMarkedContent
{
  /// <summary>Gets a tag operand of the marked content</summary>
  /// <remarks>
  /// All marked-content operators except EMC take a tag operand indicating the role
  /// or significance of the marked-content element to the processing application.
  /// </remarks>
  public string Tag { get; private set; }

  /// <summary>
  /// Gets a dictionary containing private information meaningful to the program(application or plugin extension) creating the marked content.
  /// </summary>
  public PdfTypeDictionary Parameters { get; private set; }

  /// <summary>Gets the type of parameters</summary>
  /// <remarks>
  /// If any of the values are indirect references to objects outside the content stream, the Parameters
  /// dictionary must instead be defined as a named resource in the Properties subdictionary of the current resource dictionary.
  /// In this case the ParamType should be <see cref="F:Patagames.Pdf.Enums.PropertyListTypes.PropertiesDict" />.
  /// <para>
  /// If all of the values in a property list dictionary are direct objects, the dictionary
  /// may be written inline in the content stream as a direct object.
  /// In this case the ParamType should be <see cref="F:Patagames.Pdf.Enums.PropertyListTypes.DirectDict" />.
  /// </para>
  /// </remarks>
  public PropertyListTypes ParamType { get; private set; }

  /// <summary>
  /// Gets a value indicating that the <see cref="P:Patagames.Pdf.Net.PdfMarkedContent.Parameters" /> dictionary contains the MCID (marked-content identifier) entry.
  /// </summary>
  /// <remarks>
  /// MCID entry it is an integer marked-content identifier that uniquely identifies the marked-content sequence within its content stream.
  /// </remarks>
  public bool HasMCID { get; private set; }

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> class
  /// </summary>
  /// <param name="tag">The tag operand of the marked content.</param>
  /// <param name="hasMCID">Indicate that the <see cref="P:Patagames.Pdf.Net.PdfMarkedContent.Parameters" /> dictionary contains the MCID (marked-content identifier) entry.</param>
  /// <param name="paramType">The type of the parameters dictionary</param>
  /// <param name="parameters">The parameters dictionary</param>
  public PdfMarkedContent(
    string tag,
    bool hasMCID,
    PropertyListTypes paramType,
    PdfTypeDictionary parameters)
  {
    this.Tag = tag;
    this.ParamType = paramType;
    this.Parameters = parameters;
    this.HasMCID = hasMCID;
  }
}
