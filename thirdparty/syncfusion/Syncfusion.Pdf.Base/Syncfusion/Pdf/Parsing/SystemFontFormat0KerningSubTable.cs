// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontFormat0KerningSubTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontFormat0KerningSubTable(SystemFontOpenTypeFontSourceBase fontSource) : 
  SystemFontKerningSubTable(fontSource)
{
  private Dictionary<ushort, Dictionary<ushort, short>> values;

  public override short GetValue(ushort leftGlyphIndex, ushort rightGlyphIndex)
  {
    Dictionary<ushort, short> dictionary;
    short num;
    return !this.values.TryGetValue(leftGlyphIndex, out dictionary) || !dictionary.TryGetValue(rightGlyphIndex, out num) ? (short) 0 : num;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    ushort capacity = reader.ReadUShort();
    this.values = new Dictionary<ushort, Dictionary<ushort, short>>((int) capacity);
    int num1 = (int) reader.ReadUShort();
    int num2 = (int) reader.ReadUShort();
    int num3 = (int) reader.ReadUShort();
    for (int index = 0; index < (int) capacity; ++index)
    {
      ushort key1 = reader.ReadUShort();
      ushort key2 = reader.ReadUShort();
      short num4 = reader.ReadShort();
      Dictionary<ushort, short> dictionary;
      if (!this.values.TryGetValue(key1, out dictionary))
      {
        dictionary = new Dictionary<ushort, short>();
        this.values[key1] = dictionary;
      }
      dictionary[key2] = num4;
    }
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    base.Write(writer);
    writer.WriteUShort((ushort) this.values.Count);
    foreach (ushort key1 in this.values.Keys)
    {
      Dictionary<ushort, short> dictionary = this.values[key1];
      writer.WriteUShort(key1);
      writer.WriteUShort((ushort) dictionary.Count);
      foreach (ushort key2 in dictionary.Keys)
      {
        writer.WriteUShort(key2);
        writer.WriteShort(dictionary[key2]);
      }
    }
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    ushort capacity1 = reader.ReadUShort();
    this.values = new Dictionary<ushort, Dictionary<ushort, short>>((int) capacity1);
    for (int index1 = 0; index1 < (int) capacity1; ++index1)
    {
      ushort key1 = reader.ReadUShort();
      ushort capacity2 = reader.ReadUShort();
      Dictionary<ushort, short> dictionary = new Dictionary<ushort, short>((int) capacity2);
      for (int index2 = 0; index2 < (int) capacity2; ++index2)
      {
        ushort key2 = reader.ReadUShort();
        dictionary[key2] = reader.ReadShort();
      }
      this.values[key1] = dictionary;
    }
  }
}
