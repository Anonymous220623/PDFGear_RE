// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfCrossRefItem
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Net.BasicTypes;

/// <summary>
/// Represenmts the information about item in cross-reference table
/// </summary>
public struct PdfCrossRefItem
{
  /// <summary>Gets object's number</summary>
  public int ObjectNumber;
  /// <summary>
  /// Gets the number of bytes from the beginning of the file to the beginning of the object. See remarks for details.
  /// </summary>
  /// <remarks>
  /// <list type="table">
  /// <listheader>
  /// <term>Object Type</term><description>Position means</description>
  /// </listheader>
  /// <item><term>0</term><description>Nothing means. Always 0.</description></item>
  /// <item><term>1</term><description>The byte offset of the object, starting from the beginning of the file.</description></item>
  /// <item><term>2</term><description>The object number of the object stream in which this object is stored.</description></item>
  /// <item><term>255</term><description>The byte offset of the StreamObject, starting from the beginning of the file.</description></item>
  /// </list>
  /// </remarks>
  public int Position;
  /// <summary>Gets type of object.</summary>
  /// <remarks>
  /// <list type="table">
  /// <listheader>
  /// <term>Value</term><term>Name</term><description>Description</description>
  /// </listheader>
  /// <item><term>0</term><term>free entry</term><description>Object not represented in file.</description></item>
  /// <item><term>1</term><term>in-use entry</term><description>Object represented in file.</description></item>
  /// <item><term>2</term><term>in-objstm entry</term><description>The object is located in the stream of objects.</description></item>
  /// <item><term>255</term><term>object stream marker</term><description>The object is a stream of objects.</description></item>
  /// </list>
  /// </remarks>
  public int ObjectType;
  /// <summary>Generation number</summary>
  /// <remarks>
  /// Except for object number 0, all objects in the cross-reference table initially have
  /// generation numbers of 0. When an indirect object is deleted, its cross-reference
  /// entry is marked free and it is added to the linked list of free entries.The entry’s
  /// generation number is incremented by 1 to indicate the generation number to be
  /// used the next time an object with that object number is created.Thus, each time
  /// the entry is reused, it is given a new generation number.The maximum
  /// generation number is 65,535; when a cross-reference entry reaches this value, it is
  /// never reused.
  /// </remarks>
  public int GenerationNumber;
}
