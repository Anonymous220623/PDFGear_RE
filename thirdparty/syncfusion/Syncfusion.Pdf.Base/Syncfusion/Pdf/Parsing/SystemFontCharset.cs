// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontCharset
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontCharset : SystemFontCFFTable
{
  internal const ushort NotDefIndex = 0;
  private readonly int count;
  private Dictionary<string, ushort> indices;
  private string[] names;

  public ushort this[string name]
  {
    get
    {
      ushort num;
      return this.indices.TryGetValue(name, out num) ? num : (ushort) 0;
    }
  }

  public string this[ushort index] => this.names[(int) index];

  public SystemFontCharset(SystemFontCFFFontFile file, long offset, int count)
    : base(file, offset)
  {
    this.count = count;
  }

  public SystemFontCharset(SystemFontCFFFontFile file, ushort[] glyphs)
    : base(file, -1L)
  {
    this.Initialize(glyphs);
  }

  private List<ushort> ReadFormat0(SystemFontCFFFontReader reader)
  {
    int capacity = this.count - 1;
    List<ushort> ushortList = new List<ushort>(capacity);
    for (int index = 0; index < capacity; ++index)
      ushortList.Add(reader.ReadSID());
    return ushortList;
  }

  private List<ushort> ReadFormat1(SystemFontCFFFontReader reader)
  {
    int capacity = this.count - 1;
    List<ushort> ushortList = new List<ushort>(capacity);
    while (ushortList.Count < capacity)
    {
      ushort num1 = reader.ReadSID();
      byte num2 = reader.ReadCard8();
      ushortList.Add(num1);
      for (int index = 0; index < (int) num2; ++index)
        ushortList.Add((ushort) ((int) num1 + index + 1));
    }
    return ushortList;
  }

  private List<ushort> ReadFormat2(SystemFontCFFFontReader reader)
  {
    int capacity = this.count - 1;
    List<ushort> ushortList = new List<ushort>(capacity);
    while (ushortList.Count < capacity)
    {
      ushort num1 = reader.ReadSID();
      ushort num2 = reader.ReadCard16();
      ushortList.Add(num1);
      for (int index = 0; index < (int) num2; ++index)
        ushortList.Add((ushort) ((int) num1 + index + 1));
    }
    return ushortList;
  }

  private void Initialize(ushort[] glyphs)
  {
    this.indices = new Dictionary<string, ushort>(glyphs.Length);
    this.names = new string[glyphs.Length];
    for (ushort index = 0; (int) index < glyphs.Length; ++index)
    {
      string key = this.File.ReadString(glyphs[(int) index]);
      this.indices[key] = (ushort) ((uint) index + 1U);
      this.names[(int) index] = key;
    }
  }

  public override void Read(SystemFontCFFFontReader reader)
  {
    List<ushort> ushortList;
    switch (reader.ReadCard8())
    {
      case 0:
        ushortList = this.ReadFormat0(reader);
        break;
      case 1:
        ushortList = this.ReadFormat1(reader);
        break;
      case 2:
        ushortList = this.ReadFormat2(reader);
        break;
      default:
        throw new NotSupportedException("Charset format is not supported.");
    }
    this.Initialize(ushortList.ToArray());
  }
}
