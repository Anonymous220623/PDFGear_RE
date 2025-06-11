// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FS_POINTF
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf;

/// <summary>Represents a point.</summary>
public struct FS_POINTF
{
  /// <summary>
  /// Gets or sets the x-coordinate of this <see cref="T:Patagames.Pdf.FS_POINTF" />.
  /// </summary>
  public float X { get; set; }

  /// <summary>
  /// Gets or sets the y-coordinate of this <see cref="T:Patagames.Pdf.FS_POINTF" />.
  /// </summary>
  public float Y { get; set; }

  /// <summary>
  /// Specifies whether this <see cref="T:Patagames.Pdf.FS_POINTF" /> contains the same coordinates as the specified object.
  /// </summary>
  /// <param name="obj">The System.Object to test.</param>
  /// <returns>This method returns true if obj is a <see cref="T:Patagames.Pdf.FS_POINTF" /> and has the same coordinates as this <see cref="T:Patagames.Pdf.FS_POINTF" />.</returns>
  public override bool Equals(object obj) => obj is FS_POINTF fsPointf && this.Equals(fsPointf);

  /// <summary>
  /// Specifies whether this <see cref="T:Patagames.Pdf.FS_POINTF" /> contains the same coordinates as the specified point.
  /// </summary>
  /// <param name="obj">The <see cref="T:Patagames.Pdf.FS_POINTF" /> to test.</param>
  /// <returns>This method returns true if obj has the same coordinates as this <see cref="T:Patagames.Pdf.FS_POINTF" />.</returns>
  public bool Equals(FS_POINTF obj) => obj.X.Equals(this.X) && obj.Y.Equals(this.Y);

  /// <summary>
  /// Compares two <see cref="T:Patagames.Pdf.FS_POINTF" /> structures.
  /// </summary>
  /// <param name="left">A <see cref="T:Patagames.Pdf.FS_POINTF" /> to compare.</param>
  /// <param name="right">A <see cref="T:Patagames.Pdf.FS_POINTF" /> to compare.</param>
  /// <remarks>
  /// The result specifies whether the
  /// values of the <see cref="P:Patagames.Pdf.FS_POINTF.X" /> and <see cref="P:Patagames.Pdf.FS_POINTF.Y" /> properties
  /// of the two <see cref="T:Patagames.Pdf.FS_POINTF" /> structures are equal.
  /// </remarks>
  /// <returns>
  /// true if the <see cref="P:Patagames.Pdf.FS_POINTF.X" /> and <see cref="P:Patagames.Pdf.FS_POINTF.Y" /> values of the
  /// left and right <see cref="T:Patagames.Pdf.FS_POINTF" /> structures are equal; otherwise, false.
  /// </returns>
  public static bool operator ==(FS_POINTF left, FS_POINTF right)
  {
    return left.X.Equals(right.X) && left.Y.Equals(right.Y);
  }

  /// <summary>
  /// Determines whether the coordinates of the specified points are not equal.
  /// </summary>
  /// <param name="left">A <see cref="T:Patagames.Pdf.FS_POINTF" /> to compare.</param>
  /// <param name="right">A <see cref="T:Patagames.Pdf.FS_POINTF" /> to compare.</param>
  /// <returns>true to indicate the <see cref="P:Patagames.Pdf.FS_POINTF.X" /> and <see cref="P:Patagames.Pdf.FS_POINTF.Y" /> values of left and right are not equal; otherwise, false.</returns>
  public static bool operator !=(FS_POINTF left, FS_POINTF right) => !(left == right);

  /// <summary>Returns the hash code for this instance.</summary>
  /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
  public override int GetHashCode() => (17 * 23 + this.X.GetHashCode()) * 23 + this.Y.GetHashCode();

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.FS_POINTF" /> structure
  /// </summary>
  /// <param name="x">The x-coordinate of this <see cref="T:Patagames.Pdf.FS_POINTF" />.</param>
  /// <param name="y">The y-coordinate of this <see cref="T:Patagames.Pdf.FS_POINTF" />.</param>
  public FS_POINTF(float x, float y)
  {
    this.X = x;
    this.Y = y;
  }

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.FS_POINTF" /> structure
  /// </summary>
  /// <param name="x">The x-coordinate of this <see cref="T:Patagames.Pdf.FS_POINTF" />.</param>
  /// <param name="y">The y-coordinate of this <see cref="T:Patagames.Pdf.FS_POINTF" />.</param>
  public FS_POINTF(double x, double y)
  {
    this.X = (float) x;
    this.Y = (float) y;
  }
}
