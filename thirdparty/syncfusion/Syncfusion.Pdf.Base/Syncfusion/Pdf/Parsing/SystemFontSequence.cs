// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontSequence
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontSequence
{
  public ushort[] Subsitutes { get; private set; }

  public void Read(SystemFontOpenTypeFontReader reader)
  {
    ushort length = reader.ReadUShort();
    this.Subsitutes = new ushort[(int) length];
    for (int index = 0; index < (int) length; ++index)
      this.Subsitutes[index] = reader.ReadUShort();
  }

  internal void Write(SystemFontFontWriter writer)
  {
    writer.WriteUShort((ushort) this.Subsitutes.Length);
    for (int index = 0; index < this.Subsitutes.Length; ++index)
      writer.WriteUShort(this.Subsitutes[index]);
  }

  internal void Import(SystemFontOpenTypeFontReader reader) => this.Read(reader);
}
