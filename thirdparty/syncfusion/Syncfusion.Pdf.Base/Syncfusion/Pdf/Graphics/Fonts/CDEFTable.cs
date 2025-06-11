// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.CDEFTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class CDEFTable
{
  internal Dictionary<int, int> m_records = new Dictionary<int, int>();

  internal Dictionary<int, int> Records
  {
    get => this.m_records;
    set => this.m_records = value;
  }

  internal int GetValue(int glyph)
  {
    int num;
    this.Records.TryGetValue(glyph, out num);
    return num;
  }

  internal bool IsMark(int glyph) => this.Records.ContainsKey(glyph) && this.GetValue(glyph) == 3;
}
