// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Wrappers.PdfWrapper
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Wrappers;

/// <summary>Represents the base class for all dictionary helpers</summary>
public abstract class PdfWrapper : IDisposable
{
  /// <summary>Gets dictionary associated with this object.</summary>
  public PdfTypeDictionary Dictionary { get; private set; }

  /// <summary>Creates empty dictionary.</summary>
  public PdfWrapper() => this.Dictionary = PdfTypeDictionary.Create();

  /// <summary>
  /// Creates a new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfWrapper" /> and initialize it with specified dictionary
  /// </summary>
  /// <param name="dictionary">The dictionary or indirect dictionary</param>
  public PdfWrapper(PdfTypeBase dictionary)
  {
    if (dictionary == null)
      throw new ArgumentNullException();
    if (dictionary is PdfTypeNull)
      return;
    this.Dictionary = dictionary.As<PdfTypeDictionary>();
  }

  /// <summary>Finalise object</summary>
  ~PdfWrapper() => this.Dispose(false);

  /// <summary>
  /// Releases all resources used by the <see cref="T:Patagames.Pdf.Net.Wrappers.PdfWrapper" />.
  /// </summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>
  /// Releases all resources used by the <see cref="T:Patagames.Pdf.Net.Wrappers.PdfWrapper" />.
  /// </summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    this.Dictionary.Dispose();
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>
  /// Determines whether the specified annotations is equal to the current annotation.
  /// </summary>
  /// <param name="dictionary">The dictionary to compare with the current dictionary.</param>
  /// <returns>true if the specified dictionary is equal to the current dictionary; otherwise, false.</returns>
  public override bool Equals(object dictionary)
  {
    return (object) (dictionary as PdfWrapper) != null && this.Dictionary.Handle == (dictionary as PdfWrapper).Dictionary.Handle;
  }

  /// <summary>
  /// Determines whether the specified annotations is equal to the current annotation.
  /// </summary>
  /// <param name="dictionary">The dictionary to compare with the current dictionary.</param>
  /// <returns>true if the specified dictionary is equal to the current dictionary; otherwise, false.</returns>
  public bool Equals(PdfWrapper dictionary)
  {
    return (object) dictionary != null && this.Dictionary.Handle == dictionary.Dictionary.Handle;
  }

  /// <summary>
  /// Compares two <see cref="T:Patagames.Pdf.Net.Wrappers.PdfWrapper" /> classes.
  /// </summary>
  /// <param name="left">A <see cref="T:Patagames.Pdf.Net.Wrappers.PdfWrapper" /> to compare.</param>
  /// <param name="right">A <see cref="T:Patagames.Pdf.Net.Wrappers.PdfWrapper" /> to compare.</param>
  /// <remarks>
  /// The result specifies whether the
  /// values of the <see cref="P:Patagames.Pdf.Net.Wrappers.PdfWrapper.Dictionary" /> properties
  /// of the two <see cref="T:Patagames.Pdf.Net.Wrappers.PdfWrapper" /> are point to the same dictionary.
  /// </remarks>
  /// <returns>
  /// true if the <see cref="P:Patagames.Pdf.Net.Wrappers.PdfWrapper.Dictionary" /> values of the
  /// left and right <see cref="T:Patagames.Pdf.Net.Wrappers.PdfWrapper" /> classes are point to the same dictionary.
  /// </returns>
  public static bool operator ==(PdfWrapper left, PdfWrapper right)
  {
    if ((object) left == (object) right)
      return true;
    return (object) left != null && (object) right != null && left.Dictionary.Handle == right.Dictionary.Handle;
  }

  /// <summary>
  /// Determines whether the Dictionary property of the <see cref="T:Patagames.Pdf.Net.Wrappers.PdfWrapper" /> classes are pointed to the different dictionaries.
  /// </summary>
  /// <param name="left">A <see cref="T:Patagames.Pdf.Net.Wrappers.PdfWrapper" /> to compare.</param>
  /// <param name="right">A <see cref="T:Patagames.Pdf.Net.Wrappers.PdfWrapper" /> to compare.</param>
  /// <returns>Tru or false</returns>
  public static bool operator !=(PdfWrapper left, PdfWrapper right) => !(left == right);

  /// <summary>Serves as a hash function for a particular type.</summary>
  /// <returns>A hash code for the current <see cref="T:Patagames.Pdf.Net.Wrappers.PdfWrapper" />.</returns>
  public override int GetHashCode() => this.Dictionary.Handle.ToInt32();

  /// <summary>
  /// Determines whether the specified key is contained in the current dictionary.
  /// </summary>
  /// <param name="key">The key</param>
  /// <returns>The method return false if key is not contained by dictionary; or if the key is contained, but its value is null, PdfTypeNull or indirect reference to PdfTypeNull or the Direct property is null.</returns>
  public virtual bool IsExists(string key)
  {
    if (!this.Dictionary.ContainsKey(key) || this.Dictionary[key] is PdfTypeNull)
      return false;
    if (this.Dictionary[key] is PdfTypeIndirect)
    {
      switch ((this.Dictionary[key] as PdfTypeIndirect).Direct)
      {
        case null:
        case PdfTypeNull _:
          return false;
      }
    }
    return true;
  }
}
