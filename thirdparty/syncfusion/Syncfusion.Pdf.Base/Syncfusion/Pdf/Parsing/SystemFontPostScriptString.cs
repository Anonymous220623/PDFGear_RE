// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontPostScriptString
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontPostScriptString
{
  private readonly char[] value;

  public string Value => this.value.ToString();

  public int Capacity => this.value.Length;

  public char this[int index]
  {
    get => this.value[index];
    set => this.value[index] = value;
  }

  public SystemFontPostScriptString(string str) => this.value = str.ToCharArray();

  public SystemFontPostScriptString(int capacity) => this.value = new char[capacity];

  public byte[] ToByteArray()
  {
    byte[] byteArray = new byte[this.value.Length];
    for (int index = 0; index < this.value.Length; ++index)
      byteArray[index] = (byte) this.value[index];
    return byteArray;
  }

  public override string ToString() => new string(this.value);
}
