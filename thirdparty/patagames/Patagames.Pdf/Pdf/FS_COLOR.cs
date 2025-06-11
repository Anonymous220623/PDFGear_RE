// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FS_COLOR
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Represents an ARGB (alpha, red, green, blue) color.</summary>
public struct FS_COLOR
{
  /// <summary>Empty color</summary>
  public static readonly FS_COLOR Empty = new FS_COLOR(0);

  /// <summary>
  /// Gets the alpha component value of this <see cref="T:Patagames.Pdf.FS_COLOR" /> structure.
  /// </summary>
  public int A { get; private set; }

  /// <summary>
  /// Gets the red component value of this <see cref="T:Patagames.Pdf.FS_COLOR" /> structure.
  /// </summary>
  public int R { get; private set; }

  /// <summary>
  /// Gets the green component value of this <see cref="T:Patagames.Pdf.FS_COLOR" /> structure.
  /// </summary>
  public int G { get; private set; }

  /// <summary>
  /// Gets the blue component value of this <see cref="T:Patagames.Pdf.FS_COLOR" /> structure.
  /// </summary>
  public int B { get; private set; }

  /// <summary>
  /// Creates a <see cref="T:Patagames.Pdf.FS_COLOR" /> structure from the four ARGB component (alpha, red, green, and blue) values. Although this method allows a 32-bit value to be passed for each component, the value of each component is limited to 8 bits.
  /// </summary>
  /// <param name="a">The alpha component. Valid values are 0 through 255.</param>
  /// <param name="r">The red component. Valid values are 0 through 255.</param>
  /// <param name="g">The green component. Valid values are 0 through 255.</param>
  /// <param name="b">The blue component. Valid values are 0 through 255.</param>
  /// <remarks>To create an opaque color, set alpha to 255. To create a semitransparent color, set alpha to any value from 1 through 254.</remarks>
  public FS_COLOR(int a, int r, int g, int b)
  {
    this.A = Math.Min((int) byte.MaxValue, a < 0 ? 0 : a);
    this.R = Math.Min((int) byte.MaxValue, r < 0 ? 0 : r);
    this.G = Math.Min((int) byte.MaxValue, g < 0 ? 0 : g);
    this.B = Math.Min((int) byte.MaxValue, b < 0 ? 0 : b);
  }

  /// <summary>
  /// Creates a <see cref="T:Patagames.Pdf.FS_COLOR" /> structure from the three RGB component (red, green, and blue) values. Although this method allows a 32-bit value to be passed for each component, the value of each component is limited to 8 bits.
  /// </summary>
  /// <param name="r">The red component. Valid values are 0 through 255.</param>
  /// <param name="g">The green component. Valid values are 0 through 255.</param>
  /// <param name="b">The blue component. Valid values are 0 through 255.</param>
  /// <remarks>The alpha component is equal to 255.</remarks>
  public FS_COLOR(int r, int g, int b)
    : this((int) byte.MaxValue, r, g, b)
  {
  }

  /// <summary>
  /// Creates a <see cref="T:Patagames.Pdf.FS_COLOR" /> structure from a 32-bit ARGB value.
  /// </summary>
  /// <param name="argb">A value specifying the 32-bit ARGB value.</param>
  /// <remarks>The byte-ordering of the 32-bit ARGB value is AARRGGBB. The most significant byte (MSB), represented by AA, is the alpha component value. The second, third, and fourth bytes, represented by RR, GG, and BB, respectively, are the color components red, green, and blue, respectively.</remarks>
  public FS_COLOR(int argb)
    : this(argb >> 24 & (int) byte.MaxValue, argb >> 16 /*0x10*/ & (int) byte.MaxValue, argb >> 8 & (int) byte.MaxValue, argb & (int) byte.MaxValue)
  {
  }

  /// <summary>
  /// Creates a <see cref="T:Patagames.Pdf.FS_COLOR" /> structure from a float array.
  /// </summary>
  /// <param name="arr">An array of numbers in the range 0.0 to 1.0, representing a color.</param>
  /// <remarks>The number of array elements determines the color space in which the color is defined: 0 - No color(transparent); 1 - gray color; 3 - each item in the array represents RGB components respectively; 4 - each item in the array represents CMYK components respectively.</remarks>
  public FS_COLOR(float[] arr)
  {
    if (arr == null)
      throw new PdfParserException(Error.err0039);
    switch (arr.Length)
    {
      case 0:
        this.A = this.R = this.G = this.B = 0;
        break;
      case 1:
        byte num1 = (byte) ((double) arr[0] * (double) byte.MaxValue);
        this.A = (int) byte.MaxValue;
        this.R = this.G = this.B = (int) num1;
        break;
      case 3:
        this.A = (int) byte.MaxValue;
        this.R = (int) (byte) ((double) arr[0] * (double) byte.MaxValue);
        this.G = (int) (byte) ((double) arr[1] * (double) byte.MaxValue);
        this.B = (int) (byte) ((double) arr[2] * (double) byte.MaxValue);
        break;
      case 4:
        float num2 = arr[0];
        float num3 = arr[1];
        float num4 = arr[2];
        float num5 = arr[3];
        this.A = (int) byte.MaxValue;
        this.R = (int) (byte) (int) ((double) byte.MaxValue * (1.0 - (double) num2) * (1.0 - (double) num5));
        this.G = (int) (byte) (int) ((double) byte.MaxValue * (1.0 - (double) num3) * (1.0 - (double) num5));
        this.B = (int) (byte) (int) ((double) byte.MaxValue * (1.0 - (double) num4) * (1.0 - (double) num5));
        break;
      default:
        throw new PdfParserException(Error.err0039);
    }
  }

  /// <summary>
  /// Creates a <see cref="T:Patagames.Pdf.FS_COLOR" /> structure from a <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeArray" /> array.
  /// </summary>
  /// <param name="arr">An array of numbers in the range 0.0 to 1.0, representing a color.</param>
  /// <remarks>The number of array elements determines the color space in which the color is defined: 0 - No color(transparent); 1 - gray color; 3 - each item in the array represents RGB components respectively; 4 - each item in the array represents CMYK components respectively.</remarks>
  public FS_COLOR(PdfTypeArray arr)
    : this(FS_COLOR.ToFloatArray(arr))
  {
  }

  /// <summary>
  /// Initialize a <see cref="T:Patagames.Pdf.FS_COLOR" /> structure from the four ARGB component (alpha, red, green, and blue) values.
  /// </summary>
  /// <param name="a">The alpha component. Valid values are 0 through 1.</param>
  /// <param name="r">The red component. Valid values are 0 through 1.</param>
  /// <param name="g">The green component. Valid values are 0 through 1.</param>
  /// <param name="b">The blue component. Valid values are 0 through 1.</param>
  /// <remarks>To create an opaque color, set alpha to 1. To create a semitransparent color, set alpha to any value from 0 through 1.</remarks>
  public FS_COLOR(float a, float r, float g, float b)
    : this((int) ((double) a * (double) byte.MaxValue), (int) ((double) r * (double) byte.MaxValue), (int) ((double) g * (double) byte.MaxValue), (int) ((double) b * (double) byte.MaxValue))
  {
  }

  internal FS_COLOR(float a, FS_COLOR color)
    : this((int) ((double) a * (double) byte.MaxValue), color.R, color.G, color.B)
  {
  }

  /// <summary>
  /// Gets the 32-bit ARGB value of this <see cref="T:Patagames.Pdf.FS_COLOR" /> structure.
  /// </summary>
  /// <returns>The 32-bit ARGB value of this <see cref="T:Patagames.Pdf.FS_COLOR" />.</returns>
  public int ToArgb() => this.A << 24 | this.R << 16 /*0x10*/ | this.G << 8 | this.B;

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeArray" /> of this <see cref="T:Patagames.Pdf.FS_COLOR" /> structure.
  /// </summary>
  /// <returns><see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeArray" />  of this <see cref="T:Patagames.Pdf.FS_COLOR" />.</returns>
  /// <remarks>The number of array elements determines the color space in which the color is defined: 0 - No color(transparent); 1 - gray color; 3 - each item in the array represents RGB components respectively; 4 - each item in the array represents CMYK components respectively.</remarks>
  public PdfTypeArray ToArray()
  {
    PdfTypeArray array = PdfTypeArray.Create();
    if (this.A != 0)
    {
      array.Add((PdfTypeBase) PdfTypeNumber.Create((float) this.R / (float) byte.MaxValue));
      array.Add((PdfTypeBase) PdfTypeNumber.Create((float) this.G / (float) byte.MaxValue));
      array.Add((PdfTypeBase) PdfTypeNumber.Create((float) this.B / (float) byte.MaxValue));
    }
    return array;
  }

  private static float[] ToFloatArray(PdfTypeArray arr)
  {
    if (arr == null)
      return (float[]) null;
    if (arr.Count < 0)
      return (float[]) null;
    float[] floatArray = new float[arr.Count];
    for (int index = 0; index < floatArray.Length; ++index)
    {
      if (!(arr[index] is PdfTypeNumber))
        throw new PdfParserException(Error.err0039);
      floatArray[index] = (arr[index] as PdfTypeNumber).FloatValue;
    }
    return floatArray;
  }

  /// <summary>
  /// Tests whether the specified object is a <see cref="T:Patagames.Pdf.FS_COLOR" /> structure and is equivalent to this <see cref="T:Patagames.Pdf.FS_COLOR" /> structure.
  /// </summary>
  /// <param name="obj">The object to test.</param>
  /// <returns>true if obj is a <see cref="T:Patagames.Pdf.FS_COLOR" /> structure equivalent to this <see cref="T:Patagames.Pdf.FS_COLOR" /> structure; otherwise, false.</returns>
  public override bool Equals(object obj) => obj is FS_COLOR fsColor && this.Equals(fsColor);

  /// <summary>
  /// Specifies whether this <see cref="T:Patagames.Pdf.FS_COLOR" /> is equivalent to the specified color.
  /// </summary>
  /// <param name="obj">The <see cref="T:Patagames.Pdf.FS_COLOR" /> to test.</param>
  /// <returns>true if the two <see cref="T:Patagames.Pdf.FS_COLOR" /> structures are equal; otherwise, false.</returns>
  public bool Equals(FS_COLOR obj)
  {
    return obj.A.Equals(this.A) && obj.R.Equals(this.R) && obj.G.Equals(this.G) && obj.B.Equals(this.B);
  }

  /// <summary>
  /// Tests whether two specified <see cref="T:Patagames.Pdf.FS_COLOR" /> structures are equivalent.
  /// </summary>
  /// <param name="left">The <see cref="T:Patagames.Pdf.FS_COLOR" /> that is to the left of the equality operator.</param>
  /// <param name="right">The <see cref="T:Patagames.Pdf.FS_COLOR" /> that is to the right of the equality operator.</param>
  /// <returns>true if the two <see cref="T:Patagames.Pdf.FS_COLOR" /> structures are equal; otherwise, false.</returns>
  public static bool operator ==(FS_COLOR left, FS_COLOR right) => left.Equals(right);

  /// <summary>
  /// Tests whether two specified <see cref="T:Patagames.Pdf.FS_COLOR" /> structures are different.
  /// </summary>
  /// <param name="left">The <see cref="T:Patagames.Pdf.FS_COLOR" /> that is to the left of the inequality operator.</param>
  /// <param name="right">The <see cref="T:Patagames.Pdf.FS_COLOR" /> that is to the right of the inequality operator.</param>
  /// <returns>true if the two <see cref="T:Patagames.Pdf.FS_COLOR" /> structures are different; otherwise, false.</returns>
  public static bool operator !=(FS_COLOR left, FS_COLOR right) => !(left == right);

  /// <summary>
  /// Returns the hash code for this <see cref="T:Patagames.Pdf.FS_COLOR" /> structure.
  /// </summary>
  /// <returns>A 32-bit signed integer that is the hash code for this <see cref="T:Patagames.Pdf.FS_COLOR" />.</returns>
  public override int GetHashCode()
  {
    return (((17 * 23 + this.A.GetHashCode()) * 23 + this.R.GetHashCode()) * 23 + this.G.GetHashCode()) * 23 + this.B.GetHashCode();
  }

  /// <summary>Gets a color that has an ARGB value of #00FFFFFF</summary>
  public static FS_COLOR Transparent
  {
    get => new FS_COLOR(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
  }

  /// <summary>Gets a color that has an ARGB value of #FFF0F8FF</summary>
  public static FS_COLOR AliceBlue
  {
    get => new FS_COLOR((int) byte.MaxValue, 240 /*0xF0*/, 248, (int) byte.MaxValue);
  }

  /// <summary>Gets a color that has an ARGB value of #FFFAEBD7</summary>
  public static FS_COLOR AntiqueWhite => new FS_COLOR((int) byte.MaxValue, 250, 235, 215);

  /// <summary>Gets a color that has an ARGB value of #FF00FFFF</summary>
  public static FS_COLOR Aqua
  {
    get => new FS_COLOR((int) byte.MaxValue, 0, (int) byte.MaxValue, (int) byte.MaxValue);
  }

  /// <summary>Gets a color that has an ARGB value of #FF7FFFD4</summary>
  public static FS_COLOR Aquamarine
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) sbyte.MaxValue, (int) byte.MaxValue, 212);
  }

  /// <summary>Gets a color that has an ARGB value of #FFF0FFFF</summary>
  public static FS_COLOR Azure
  {
    get
    {
      return new FS_COLOR((int) byte.MaxValue, 240 /*0xF0*/, (int) byte.MaxValue, (int) byte.MaxValue);
    }
  }

  /// <summary>Gets a color that has an ARGB value of #FFF5F5DC</summary>
  public static FS_COLOR Beige => new FS_COLOR((int) byte.MaxValue, 245, 245, 220);

  /// <summary>Gets a color that has an ARGB value of #FFFFE4C4</summary>
  public static FS_COLOR Bisque => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 228, 196);

  /// <summary>Gets a color that has an ARGB value of #FF000000</summary>
  public static FS_COLOR Black => new FS_COLOR((int) byte.MaxValue, 0, 0, 0);

  /// <summary>Gets a color that has an ARGB value of #FFFFEBCD</summary>
  public static FS_COLOR BlanchedAlmond
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 235, 205);
  }

  /// <summary>Gets a color that has an ARGB value of #FF0000FF</summary>
  public static FS_COLOR Blue => new FS_COLOR((int) byte.MaxValue, 0, 0, (int) byte.MaxValue);

  /// <summary>Gets a color that has an ARGB value of #FF8A2BE2</summary>
  public static FS_COLOR BlueViolet => new FS_COLOR((int) byte.MaxValue, 138, 43, 226);

  /// <summary>Gets a color that has an ARGB value of #FFA52A2A</summary>
  public static FS_COLOR Brown => new FS_COLOR((int) byte.MaxValue, 165, 42, 42);

  /// <summary>Gets a color that has an ARGB value of #FFDEB887</summary>
  public static FS_COLOR BurlyWood => new FS_COLOR((int) byte.MaxValue, 222, 184, 135);

  /// <summary>Gets a color that has an ARGB value of #FF5F9EA0</summary>
  public static FS_COLOR CadetBlue => new FS_COLOR((int) byte.MaxValue, 95, 158, 160 /*0xA0*/);

  /// <summary>Gets a color that has an ARGB value of #FF7FFF00</summary>
  public static FS_COLOR Chartreuse
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) sbyte.MaxValue, (int) byte.MaxValue, 0);
  }

  /// <summary>Gets a color that has an ARGB value of #FFD2691E</summary>
  public static FS_COLOR Chocolate => new FS_COLOR((int) byte.MaxValue, 210, 105, 30);

  /// <summary>Gets a color that has an ARGB value of #FFFF7F50</summary>
  public static FS_COLOR Coral
  {
    get
    {
      return new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, (int) sbyte.MaxValue, 80 /*0x50*/);
    }
  }

  /// <summary>Gets a color that has an ARGB value of #FF6495ED</summary>
  public static FS_COLOR CornflowerBlue => new FS_COLOR((int) byte.MaxValue, 100, 149, 237);

  /// <summary>Gets a color that has an ARGB value of #FFFFF8DC</summary>
  public static FS_COLOR Cornsilk
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 248, 220);
  }

  /// <summary>Gets a color that has an ARGB value of #FFDC143C</summary>
  public static FS_COLOR Crimson => new FS_COLOR((int) byte.MaxValue, 220, 20, 60);

  /// <summary>Gets a color that has an ARGB value of #FF00FFFF</summary>
  public static FS_COLOR Cyan
  {
    get => new FS_COLOR((int) byte.MaxValue, 0, (int) byte.MaxValue, (int) byte.MaxValue);
  }

  /// <summary>Gets a color that has an ARGB value of #FF00008B</summary>
  public static FS_COLOR DarkBlue => new FS_COLOR((int) byte.MaxValue, 0, 0, 139);

  /// <summary>Gets a color that has an ARGB value of #FF008B8B</summary>
  public static FS_COLOR DarkCyan => new FS_COLOR((int) byte.MaxValue, 0, 139, 139);

  /// <summary>Gets a color that has an ARGB value of #FFB8860B</summary>
  public static FS_COLOR DarkGoldenrod => new FS_COLOR((int) byte.MaxValue, 184, 134, 11);

  /// <summary>Gets a color that has an ARGB value of #FFA9A9A9</summary>
  public static FS_COLOR DarkGray => new FS_COLOR((int) byte.MaxValue, 169, 169, 169);

  /// <summary>Gets a color that has an ARGB value of #FF006400</summary>
  public static FS_COLOR DarkGreen => new FS_COLOR((int) byte.MaxValue, 0, 100, 0);

  /// <summary>Gets a color that has an ARGB value of #FFBDB76B</summary>
  public static FS_COLOR DarkKhaki => new FS_COLOR((int) byte.MaxValue, 189, 183, 107);

  /// <summary>Gets a color that has an ARGB value of #FF8B008B</summary>
  public static FS_COLOR DarkMagenta => new FS_COLOR((int) byte.MaxValue, 139, 0, 139);

  /// <summary>Gets a color that has an ARGB value of #FF556B2F</summary>
  public static FS_COLOR DarkOliveGreen => new FS_COLOR((int) byte.MaxValue, 85, 107, 47);

  /// <summary>Gets a color that has an ARGB value of #FFFF8C00</summary>
  public static FS_COLOR DarkOrange
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 140, 0);
  }

  /// <summary>Gets a color that has an ARGB value of #FF9932CC</summary>
  public static FS_COLOR DarkOrchid => new FS_COLOR((int) byte.MaxValue, 153, 50, 204);

  /// <summary>Gets a color that has an ARGB value of #FF8B0000</summary>
  public static FS_COLOR DarkRed => new FS_COLOR((int) byte.MaxValue, 139, 0, 0);

  /// <summary>Gets a color that has an ARGB value of #FFE9967A</summary>
  public static FS_COLOR DarkSalmon => new FS_COLOR((int) byte.MaxValue, 233, 150, 122);

  /// <summary>Gets a color that has an ARGB value of #FF8FBC8B</summary>
  public static FS_COLOR DarkSeaGreen => new FS_COLOR((int) byte.MaxValue, 143, 188, 139);

  /// <summary>Gets a color that has an ARGB value of #FF483D8B</summary>
  public static FS_COLOR DarkSlateBlue => new FS_COLOR((int) byte.MaxValue, 72, 61, 139);

  /// <summary>Gets a color that has an ARGB value of #FF2F4F4F</summary>
  public static FS_COLOR DarkSlateGray => new FS_COLOR((int) byte.MaxValue, 47, 79, 79);

  /// <summary>Gets a color that has an ARGB value of #FF00CED1</summary>
  public static FS_COLOR DarkTurquoise => new FS_COLOR((int) byte.MaxValue, 0, 206, 209);

  /// <summary>Gets a color that has an ARGB value of #FF9400D3</summary>
  public static FS_COLOR DarkViolet => new FS_COLOR((int) byte.MaxValue, 148, 0, 211);

  /// <summary>Gets a color that has an ARGB value of #FFFF1493</summary>
  public static FS_COLOR DeepPink
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 20, 147);
  }

  /// <summary>Gets a color that has an ARGB value of #FF00BFFF</summary>
  public static FS_COLOR DeepSkyBlue
  {
    get => new FS_COLOR((int) byte.MaxValue, 0, 191, (int) byte.MaxValue);
  }

  /// <summary>Gets a color that has an ARGB value of #FF696969</summary>
  public static FS_COLOR DimGray => new FS_COLOR((int) byte.MaxValue, 105, 105, 105);

  /// <summary>Gets a color that has an ARGB value of #FF1E90FF</summary>
  public static FS_COLOR DodgerBlue
  {
    get => new FS_COLOR((int) byte.MaxValue, 30, 144 /*0x90*/, (int) byte.MaxValue);
  }

  /// <summary>Gets a color that has an ARGB value of #FFB22222</summary>
  public static FS_COLOR Firebrick => new FS_COLOR((int) byte.MaxValue, 178, 34, 34);

  /// <summary>Gets a color that has an ARGB value of #FFFFFAF0</summary>
  public static FS_COLOR FloralWhite
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 250, 240 /*0xF0*/);
  }

  /// <summary>Gets a color that has an ARGB value of #FF228B22</summary>
  public static FS_COLOR ForestGreen => new FS_COLOR((int) byte.MaxValue, 34, 139, 34);

  /// <summary>Gets a color that has an ARGB value of #FFFF00FF</summary>
  public static FS_COLOR Fuchsia
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 0, (int) byte.MaxValue);
  }

  /// <summary>Gets a color that has an ARGB value of #FFDCDCDC</summary>
  public static FS_COLOR Gainsboro => new FS_COLOR((int) byte.MaxValue, 220, 220, 220);

  /// <summary>Gets a color that has an ARGB value of #FFF8F8FF</summary>
  public static FS_COLOR GhostWhite
  {
    get => new FS_COLOR((int) byte.MaxValue, 248, 248, (int) byte.MaxValue);
  }

  /// <summary>Gets a color that has an ARGB value of #FFFFD700</summary>
  public static FS_COLOR Gold => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 215, 0);

  /// <summary>Gets a color that has an ARGB value of #FFDAA520</summary>
  public static FS_COLOR Goldenrod => new FS_COLOR((int) byte.MaxValue, 218, 165, 32 /*0x20*/);

  /// <summary>Gets a color that has an ARGB value of #FF808080</summary>
  public static FS_COLOR Gray
  {
    get => new FS_COLOR((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
  }

  /// <summary>Gets a color that has an ARGB value of #FF008000</summary>
  public static FS_COLOR Green => new FS_COLOR((int) byte.MaxValue, 0, 128 /*0x80*/, 0);

  /// <summary>Gets a color that has an ARGB value of #FFADFF2F</summary>
  public static FS_COLOR GreenYellow
  {
    get => new FS_COLOR((int) byte.MaxValue, 173, (int) byte.MaxValue, 47);
  }

  /// <summary>Gets a color that has an ARGB value of #FFF0FFF0</summary>
  public static FS_COLOR Honeydew
  {
    get => new FS_COLOR((int) byte.MaxValue, 240 /*0xF0*/, (int) byte.MaxValue, 240 /*0xF0*/);
  }

  /// <summary>Gets a color that has an ARGB value of #FFFF69B4</summary>
  public static FS_COLOR HotPink
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 105, 180);
  }

  /// <summary>Gets a color that has an ARGB value of #FFCD5C5C</summary>
  public static FS_COLOR IndianRed => new FS_COLOR((int) byte.MaxValue, 205, 92, 92);

  /// <summary>Gets a color that has an ARGB value of #FF4B0082</summary>
  public static FS_COLOR Indigo => new FS_COLOR((int) byte.MaxValue, 75, 0, 130);

  /// <summary>Gets a color that has an ARGB value of #FFFFFFF0</summary>
  public static FS_COLOR Ivory
  {
    get
    {
      return new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 240 /*0xF0*/);
    }
  }

  /// <summary>Gets a color that has an ARGB value of #FFF0E68C</summary>
  public static FS_COLOR Khaki => new FS_COLOR((int) byte.MaxValue, 240 /*0xF0*/, 230, 140);

  /// <summary>Gets a color that has an ARGB value of #FFE6E6FA</summary>
  public static FS_COLOR Lavender => new FS_COLOR((int) byte.MaxValue, 230, 230, 250);

  /// <summary>Gets a color that has an ARGB value of #FFFFF0F5</summary>
  public static FS_COLOR LavenderBlush
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 240 /*0xF0*/, 245);
  }

  /// <summary>Gets a color that has an ARGB value of #FF7CFC00</summary>
  public static FS_COLOR LawnGreen => new FS_COLOR((int) byte.MaxValue, 124, 252, 0);

  /// <summary>Gets a color that has an ARGB value of #FFFFFACD</summary>
  public static FS_COLOR LemonChiffon
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 250, 205);
  }

  /// <summary>Gets a color that has an ARGB value of #FFADD8E6</summary>
  public static FS_COLOR LightBlue => new FS_COLOR((int) byte.MaxValue, 173, 216, 230);

  /// <summary>Gets a color that has an ARGB value of #FFF08080</summary>
  public static FS_COLOR LightCoral
  {
    get => new FS_COLOR((int) byte.MaxValue, 240 /*0xF0*/, 128 /*0x80*/, 128 /*0x80*/);
  }

  /// <summary>Gets a color that has an ARGB value of #FFE0FFFF</summary>
  public static FS_COLOR LightCyan
  {
    get
    {
      return new FS_COLOR((int) byte.MaxValue, 224 /*0xE0*/, (int) byte.MaxValue, (int) byte.MaxValue);
    }
  }

  /// <summary>Gets a color that has an ARGB value of #FFFAFAD2</summary>
  public static FS_COLOR LightGoldenrodYellow => new FS_COLOR((int) byte.MaxValue, 250, 250, 210);

  /// <summary>Gets a color that has an ARGB value of #FFD3D3D3</summary>
  public static FS_COLOR LightGray => new FS_COLOR((int) byte.MaxValue, 211, 211, 211);

  /// <summary>Gets a color that has an ARGB value of #FF90EE90</summary>
  public static FS_COLOR LightGreen
  {
    get => new FS_COLOR((int) byte.MaxValue, 144 /*0x90*/, 238, 144 /*0x90*/);
  }

  /// <summary>Gets a color that has an ARGB value of #FFFFB6C1</summary>
  public static FS_COLOR LightPink
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 182, 193);
  }

  /// <summary>Gets a color that has an ARGB value of #FFFFA07A</summary>
  public static FS_COLOR LightSalmon
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 160 /*0xA0*/, 122);
  }

  /// <summary>Gets a color that has an ARGB value of #FF20B2AA</summary>
  public static FS_COLOR LightSeaGreen => new FS_COLOR((int) byte.MaxValue, 32 /*0x20*/, 178, 170);

  /// <summary>Gets a color that has an ARGB value of #FF87CEFA</summary>
  public static FS_COLOR LightSkyBlue => new FS_COLOR((int) byte.MaxValue, 135, 206, 250);

  /// <summary>Gets a color that has an ARGB value of #FF778899</summary>
  public static FS_COLOR LightSlateGray => new FS_COLOR((int) byte.MaxValue, 119, 136, 153);

  /// <summary>Gets a color that has an ARGB value of #FFB0C4DE</summary>
  public static FS_COLOR LightSteelBlue
  {
    get => new FS_COLOR((int) byte.MaxValue, 176 /*0xB0*/, 196, 222);
  }

  /// <summary>Gets a color that has an ARGB value of #FFFFFFE0</summary>
  public static FS_COLOR LightYellow
  {
    get
    {
      return new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 224 /*0xE0*/);
    }
  }

  /// <summary>Gets a color that has an ARGB value of #FF00FF00</summary>
  public static FS_COLOR Lime => new FS_COLOR((int) byte.MaxValue, 0, (int) byte.MaxValue, 0);

  /// <summary>Gets a color that has an ARGB value of #FF32CD32</summary>
  public static FS_COLOR LimeGreen => new FS_COLOR((int) byte.MaxValue, 50, 205, 50);

  /// <summary>Gets a color that has an ARGB value of #FFFAF0E6</summary>
  public static FS_COLOR Linen => new FS_COLOR((int) byte.MaxValue, 250, 240 /*0xF0*/, 230);

  /// <summary>Gets a color that has an ARGB value of #FFFF00FF</summary>
  public static FS_COLOR Magenta
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 0, (int) byte.MaxValue);
  }

  /// <summary>Gets a color that has an ARGB value of #FF800000</summary>
  public static FS_COLOR Maroon => new FS_COLOR((int) byte.MaxValue, 128 /*0x80*/, 0, 0);

  /// <summary>Gets a color that has an ARGB value of #FF66CDAA</summary>
  public static FS_COLOR MediumAquamarine => new FS_COLOR((int) byte.MaxValue, 102, 205, 170);

  /// <summary>Gets a color that has an ARGB value of #FF0000CD</summary>
  public static FS_COLOR MediumBlue => new FS_COLOR((int) byte.MaxValue, 0, 0, 205);

  /// <summary>Gets a color that has an ARGB value of #FFBA55D3</summary>
  public static FS_COLOR MediumOrchid => new FS_COLOR((int) byte.MaxValue, 186, 85, 211);

  /// <summary>Gets a color that has an ARGB value of #FF9370DB</summary>
  public static FS_COLOR MediumPurple => new FS_COLOR((int) byte.MaxValue, 147, 112 /*0x70*/, 219);

  /// <summary>Gets a color that has an ARGB value of #FF3CB371</summary>
  public static FS_COLOR MediumSeaGreen => new FS_COLOR((int) byte.MaxValue, 60, 179, 113);

  /// <summary>Gets a color that has an ARGB value of #FF7B68EE</summary>
  public static FS_COLOR MediumSlateBlue => new FS_COLOR((int) byte.MaxValue, 123, 104, 238);

  /// <summary>Gets a color that has an ARGB value of #FF00FA9A</summary>
  public static FS_COLOR MediumSpringGreen => new FS_COLOR((int) byte.MaxValue, 0, 250, 154);

  /// <summary>Gets a color that has an ARGB value of #FF48D1CC</summary>
  public static FS_COLOR MediumTurquoise => new FS_COLOR((int) byte.MaxValue, 72, 209, 204);

  /// <summary>Gets a color that has an ARGB value of #FFC71585</summary>
  public static FS_COLOR MediumVioletRed => new FS_COLOR((int) byte.MaxValue, 199, 21, 133);

  /// <summary>Gets a color that has an ARGB value of #FF191970</summary>
  public static FS_COLOR MidnightBlue => new FS_COLOR((int) byte.MaxValue, 25, 25, 112 /*0x70*/);

  /// <summary>Gets a color that has an ARGB value of #FFF5FFFA</summary>
  public static FS_COLOR MintCream
  {
    get => new FS_COLOR((int) byte.MaxValue, 245, (int) byte.MaxValue, 250);
  }

  /// <summary>Gets a color that has an ARGB value of #FFFFE4E1</summary>
  public static FS_COLOR MistyRose
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 228, 225);
  }

  /// <summary>Gets a color that has an ARGB value of #FFFFE4B5</summary>
  public static FS_COLOR Moccasin
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 228, 181);
  }

  /// <summary>Gets a color that has an ARGB value of #FFFFDEAD</summary>
  public static FS_COLOR NavajoWhite
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 222, 173);
  }

  /// <summary>Gets a color that has an ARGB value of #FF000080</summary>
  public static FS_COLOR Navy => new FS_COLOR((int) byte.MaxValue, 0, 0, 128 /*0x80*/);

  /// <summary>Gets a color that has an ARGB value of #FFFDF5E6</summary>
  public static FS_COLOR OldLace => new FS_COLOR((int) byte.MaxValue, 253, 245, 230);

  /// <summary>Gets a color that has an ARGB value of #FF808000</summary>
  public static FS_COLOR Olive => new FS_COLOR((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 0);

  /// <summary>Gets a color that has an ARGB value of #FF6B8E23</summary>
  public static FS_COLOR OliveDrab => new FS_COLOR((int) byte.MaxValue, 107, 142, 35);

  /// <summary>Gets a color that has an ARGB value of #FFFFA500</summary>
  public static FS_COLOR Orange => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 165, 0);

  /// <summary>Gets a color that has an ARGB value of #FFFF4500</summary>
  public static FS_COLOR OrangeRed => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 69, 0);

  /// <summary>Gets a color that has an ARGB value of #FFDA70D6</summary>
  public static FS_COLOR Orchid => new FS_COLOR((int) byte.MaxValue, 218, 112 /*0x70*/, 214);

  /// <summary>Gets a color that has an ARGB value of #FFEEE8AA</summary>
  public static FS_COLOR PaleGoldenrod => new FS_COLOR((int) byte.MaxValue, 238, 232, 170);

  /// <summary>Gets a color that has an ARGB value of #FF98FB98</summary>
  public static FS_COLOR PaleGreen => new FS_COLOR((int) byte.MaxValue, 152, 251, 152);

  /// <summary>Gets a color that has an ARGB value of #FFAFEEEE</summary>
  public static FS_COLOR PaleTurquoise => new FS_COLOR((int) byte.MaxValue, 175, 238, 238);

  /// <summary>Gets a color that has an ARGB value of #FFDB7093</summary>
  public static FS_COLOR PaleVioletRed => new FS_COLOR((int) byte.MaxValue, 219, 112 /*0x70*/, 147);

  /// <summary>Gets a color that has an ARGB value of #FFFFEFD5</summary>
  public static FS_COLOR PapayaWhip
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 239, 213);
  }

  /// <summary>Gets a color that has an ARGB value of #FFFFDAB9</summary>
  public static FS_COLOR PeachPuff
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 218, 185);
  }

  /// <summary>Gets a color that has an ARGB value of #FFCD853F</summary>
  public static FS_COLOR Peru => new FS_COLOR((int) byte.MaxValue, 205, 133, 63 /*0x3F*/);

  /// <summary>Gets a color that has an ARGB value of #FFFFC0CB</summary>
  public static FS_COLOR Pink
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/, 203);
  }

  /// <summary>Gets a color that has an ARGB value of #FFDDA0DD</summary>
  public static FS_COLOR Plum => new FS_COLOR((int) byte.MaxValue, 221, 160 /*0xA0*/, 221);

  /// <summary>Gets a color that has an ARGB value of #FFB0E0E6</summary>
  public static FS_COLOR PowderBlue
  {
    get => new FS_COLOR((int) byte.MaxValue, 176 /*0xB0*/, 224 /*0xE0*/, 230);
  }

  /// <summary>Gets a color that has an ARGB value of #FF800080</summary>
  public static FS_COLOR Purple => new FS_COLOR((int) byte.MaxValue, 128 /*0x80*/, 0, 128 /*0x80*/);

  /// <summary>Gets a color that has an ARGB value of #FFFF0000</summary>
  public static FS_COLOR Red => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 0, 0);

  /// <summary>Gets a color that has an ARGB value of #FFBC8F8F</summary>
  public static FS_COLOR RosyBrown => new FS_COLOR((int) byte.MaxValue, 188, 143, 143);

  /// <summary>Gets a color that has an ARGB value of #FF4169E1</summary>
  public static FS_COLOR RoyalBlue => new FS_COLOR((int) byte.MaxValue, 65, 105, 225);

  /// <summary>Gets a color that has an ARGB value of #FF8B4513</summary>
  public static FS_COLOR SaddleBrown => new FS_COLOR((int) byte.MaxValue, 139, 69, 19);

  /// <summary>Gets a color that has an ARGB value of #FFFA8072</summary>
  public static FS_COLOR Salmon => new FS_COLOR((int) byte.MaxValue, 250, 128 /*0x80*/, 114);

  /// <summary>Gets a color that has an ARGB value of #FFF4A460</summary>
  public static FS_COLOR SandyBrown => new FS_COLOR((int) byte.MaxValue, 244, 164, 96 /*0x60*/);

  /// <summary>Gets a color that has an ARGB value of #FF2E8B57</summary>
  public static FS_COLOR SeaGreen => new FS_COLOR((int) byte.MaxValue, 46, 139, 87);

  /// <summary>Gets a color that has an ARGB value of #FFFFF5EE</summary>
  public static FS_COLOR SeaShell
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 245, 238);
  }

  /// <summary>Gets a color that has an ARGB value of #FFA0522D</summary>
  public static FS_COLOR Sienna => new FS_COLOR((int) byte.MaxValue, 160 /*0xA0*/, 82, 45);

  /// <summary>Gets a color that has an ARGB value of #FFC0C0C0</summary>
  public static FS_COLOR Silver
  {
    get => new FS_COLOR((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
  }

  /// <summary>Gets a color that has an ARGB value of #FF87CEEB</summary>
  public static FS_COLOR SkyBlue => new FS_COLOR((int) byte.MaxValue, 135, 206, 235);

  /// <summary>Gets a color that has an ARGB value of #FF6A5ACD</summary>
  public static FS_COLOR SlateBlue => new FS_COLOR((int) byte.MaxValue, 106, 90, 205);

  /// <summary>Gets a color that has an ARGB value of #FF708090</summary>
  public static FS_COLOR SlateGray
  {
    get => new FS_COLOR((int) byte.MaxValue, 112 /*0x70*/, 128 /*0x80*/, 144 /*0x90*/);
  }

  /// <summary>Gets a color that has an ARGB value of #FFFFFAFA</summary>
  public static FS_COLOR Snow => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 250, 250);

  /// <summary>Gets a color that has an ARGB value of #FF00FF7F</summary>
  public static FS_COLOR SpringGreen
  {
    get => new FS_COLOR((int) byte.MaxValue, 0, (int) byte.MaxValue, (int) sbyte.MaxValue);
  }

  /// <summary>Gets a color that has an ARGB value of #FF4682B4</summary>
  public static FS_COLOR SteelBlue => new FS_COLOR((int) byte.MaxValue, 70, 130, 180);

  /// <summary>Gets a color that has an ARGB value of #FFD2B48C</summary>
  public static FS_COLOR Tan => new FS_COLOR((int) byte.MaxValue, 210, 180, 140);

  /// <summary>Gets a color that has an ARGB value of #FF008080</summary>
  public static FS_COLOR Teal => new FS_COLOR((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/);

  /// <summary>Gets a color that has an ARGB value of #FFD8BFD8</summary>
  public static FS_COLOR Thistle => new FS_COLOR((int) byte.MaxValue, 216, 191, 216);

  /// <summary>Gets a color that has an ARGB value of #FFFF6347</summary>
  public static FS_COLOR Tomato => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 99, 71);

  /// <summary>Gets a color that has an ARGB value of #FF40E0D0</summary>
  public static FS_COLOR Turquoise
  {
    get => new FS_COLOR((int) byte.MaxValue, 64 /*0x40*/, 224 /*0xE0*/, 208 /*0xD0*/);
  }

  /// <summary>Gets a color that has an ARGB value of #FFEE82EE</summary>
  public static FS_COLOR Violet => new FS_COLOR((int) byte.MaxValue, 238, 130, 238);

  /// <summary>Gets a color that has an ARGB value of #FFF5DEB3</summary>
  public static FS_COLOR Wheat => new FS_COLOR((int) byte.MaxValue, 245, 222, 179);

  /// <summary>Gets a color that has an ARGB value of #FFFFFFFF</summary>
  public static FS_COLOR White
  {
    get
    {
      return new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    }
  }

  /// <summary>Gets a color that has an ARGB value of #FFF5F5F5</summary>
  public static FS_COLOR WhiteSmoke => new FS_COLOR((int) byte.MaxValue, 245, 245, 245);

  /// <summary>Gets a color that has an ARGB value of #FFFFFF00</summary>
  public static FS_COLOR Yellow
  {
    get => new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
  }

  /// <summary>Gets a color that has an ARGB value of #FF9ACD32</summary>
  public static FS_COLOR YellowGreen => new FS_COLOR((int) byte.MaxValue, 154, 205, 50);
}
