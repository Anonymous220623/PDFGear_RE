// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FS_SIZEF
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf;

/// <summary>Represents the size in default user space.</summary>
public struct FS_SIZEF
{
  /// <summary>
  /// Gets or sets the horizontal component of this <see cref="T:Patagames.Pdf.FS_SIZEF" /> structure.
  /// </summary>
  public float Width;
  /// <summary>
  /// Gets or sets the vertical component of this <see cref="T:Patagames.Pdf.FS_SIZEF" /> structure.
  /// </summary>
  public float Height;

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.FS_SIZEF" /> structure.
  /// </summary>
  /// <param name="width">The horizontal component of this <see cref="T:Patagames.Pdf.FS_SIZEF" /> structure.</param>
  /// <param name="height">The vertical component of this <see cref="T:Patagames.Pdf.FS_SIZEF" /> structure.</param>
  public FS_SIZEF(float width, float height)
  {
    this.Width = width;
    this.Height = height;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.FS_SIZEF" /> structure.
  /// </summary>
  /// <param name="width">The horizontal component of this <see cref="T:Patagames.Pdf.FS_SIZEF" /> structure.</param>
  /// <param name="height">The vertical component of this <see cref="T:Patagames.Pdf.FS_SIZEF" /> structure.</param>
  public FS_SIZEF(double width, double height)
  {
    this.Width = (float) width;
    this.Height = (float) height;
  }

  /// <summary>
  /// Specifies whether this <see cref="T:Patagames.Pdf.FS_SIZEF" /> contains the same width and height as the specified object.
  /// </summary>
  /// <param name="obj">The System.Object to test.</param>
  /// <returns>This method returns true if obj is a <see cref="T:Patagames.Pdf.FS_SIZEF" />and has the same width and height as this <see cref="T:Patagames.Pdf.FS_SIZEF" />.</returns>
  public override bool Equals(object obj) => obj is FS_SIZEF fsSizef && this.Equals(fsSizef);

  /// <summary>
  /// Specifies whether this <see cref="T:Patagames.Pdf.FS_SIZEF" /> contains the same width and height as the specified structure.
  /// </summary>
  /// <param name="obj">The <see cref="T:Patagames.Pdf.FS_SIZEF" /> to test.</param>
  /// <returns>This method returns true if obj has the same width and height as this <see cref="T:Patagames.Pdf.FS_SIZEF" />.</returns>
  public bool Equals(FS_SIZEF obj)
  {
    return obj.Width.Equals(this.Width) && obj.Height.Equals(this.Height);
  }

  /// <summary>
  /// Compares two <see cref="T:Patagames.Pdf.FS_SIZEF" /> structures.
  /// </summary>
  /// <param name="left">A <see cref="T:Patagames.Pdf.FS_SIZEF" /> to compare.</param>
  /// <param name="right">A <see cref="T:Patagames.Pdf.FS_SIZEF" /> to compare.</param>
  /// <remarks>
  /// The result specifies whether the
  /// values of the <see cref="F:Patagames.Pdf.FS_SIZEF.Width" /> and <see cref="F:Patagames.Pdf.FS_SIZEF.Height" /> properties
  /// of the two <see cref="T:Patagames.Pdf.FS_SIZEF" /> structures are equal.
  /// </remarks>
  /// <returns>
  /// true if the <see cref="F:Patagames.Pdf.FS_SIZEF.Width" /> and <see cref="F:Patagames.Pdf.FS_SIZEF.Height" /> values of the
  /// left and right <see cref="T:Patagames.Pdf.FS_SIZEF" /> structures are equal; otherwise, false.
  /// </returns>
  public static bool operator ==(FS_SIZEF left, FS_SIZEF right) => left.Equals(right);

  /// <summary>
  /// Determines whether the width and Height of the specified strustures are not equal.
  /// </summary>
  /// <param name="left">A <see cref="T:Patagames.Pdf.FS_SIZEF" /> to compare.</param>
  /// <param name="right">A <see cref="T:Patagames.Pdf.FS_SIZEF" /> to compare.</param>
  /// <returns>true to indicate the <see cref="F:Patagames.Pdf.FS_SIZEF.Width" /> and <see cref="F:Patagames.Pdf.FS_SIZEF.Height" /> values of left and right are not equal; otherwise, false.</returns>
  public static bool operator !=(FS_SIZEF left, FS_SIZEF right) => !(left == right);

  /// <summary>Returns the hash code for this instance.</summary>
  /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
  public override int GetHashCode()
  {
    return (17 * 23 + this.Width.GetHashCode()) * 23 + this.Height.GetHashCode();
  }
}
