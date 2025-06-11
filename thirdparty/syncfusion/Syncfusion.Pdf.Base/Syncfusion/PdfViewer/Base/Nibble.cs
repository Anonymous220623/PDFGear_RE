// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Nibble
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class Nibble
{
  private readonly byte value;

  public static Nibble[] GetNibbles(byte b)
  {
    byte[] bits = BitsAssistant.GetBits(b);
    Nibble[] nibbles = new Nibble[2]
    {
      null,
      new Nibble(BitsAssistant.ToByte(bits, 0, 4))
    };
    nibbles[0] = new Nibble(BitsAssistant.ToByte(bits, 4, 4));
    return nibbles;
  }

  public static bool operator ==(Nibble left, Nibble right)
  {
    return object.ReferenceEquals((object) left, (object) null) ? object.ReferenceEquals((object) right, (object) null) : left.Equals((object) right);
  }

  public static bool operator !=(Nibble left, Nibble right) => !(left == right);

  public Nibble(byte value) => this.value = value;

  public override bool Equals(object obj)
  {
    Nibble nibble = obj as Nibble;
    return !(nibble == (Nibble) null) && (int) this.value == (int) nibble.value;
  }

  public override int GetHashCode() => 17 * 23 + this.value.GetHashCode();

  public override string ToString() => $"{this.value:X}";
}
