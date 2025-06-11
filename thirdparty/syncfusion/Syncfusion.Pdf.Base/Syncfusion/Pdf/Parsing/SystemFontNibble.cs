// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontNibble
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontNibble
{
  private readonly byte value;

  public static SystemFontNibble[] GetNibbles(byte b)
  {
    byte[] bits = SystemFontBitsHelper.GetBits(b);
    SystemFontNibble[] nibbles = new SystemFontNibble[2]
    {
      null,
      new SystemFontNibble(SystemFontBitsHelper.ToByte(bits, 0, 4))
    };
    nibbles[0] = new SystemFontNibble(SystemFontBitsHelper.ToByte(bits, 4, 4));
    return nibbles;
  }

  public static bool operator ==(SystemFontNibble left, SystemFontNibble right)
  {
    return object.ReferenceEquals((object) left, (object) null) ? object.ReferenceEquals((object) right, (object) null) : left.Equals((object) right);
  }

  public static bool operator !=(SystemFontNibble left, SystemFontNibble right) => !(left == right);

  public SystemFontNibble(byte value) => this.value = value;

  public override bool Equals(object obj)
  {
    SystemFontNibble systemFontNibble = obj as SystemFontNibble;
    return !(systemFontNibble == (SystemFontNibble) null) && (int) this.value == (int) systemFontNibble.value;
  }

  public override int GetHashCode() => 17 * 23 + this.value.GetHashCode();

  public override string ToString() => $"{this.value:X}";
}
