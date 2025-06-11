// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontGlyphsSequence
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontGlyphsSequence : IEnumerable<SystemFontGlyphInfo>, IEnumerable
{
  private readonly List<SystemFontGlyphInfo> store;

  public SystemFontGlyphInfo this[int index] => this.store[index];

  public int Count => this.store.Count;

  public SystemFontGlyphsSequence() => this.store = new List<SystemFontGlyphInfo>();

  public SystemFontGlyphsSequence(int capacity)
  {
    this.store = new List<SystemFontGlyphInfo>(capacity);
  }

  public void Add(SystemFontGlyphInfo glyphInfo) => this.store.Add(glyphInfo);

  public void Add(ushort glyphID, SystemFontGlyphForm form)
  {
    this.store.Add(new SystemFontGlyphInfo(glyphID, form));
  }

  public void Add(ushort glyphId) => this.store.Add(new SystemFontGlyphInfo(glyphId));

  public void AddRange(IEnumerable<ushort> glyphIDs)
  {
    foreach (ushort glyphId in glyphIDs)
      this.Add(glyphId);
  }

  public void AddRange(IEnumerable<SystemFontGlyphInfo> glyphIDs) => this.store.AddRange(glyphIDs);

  public SystemFontGlyphForm GetGlyphForm(int index) => this.store[index].Form;

  public IEnumerator<SystemFontGlyphInfo> GetEnumerator()
  {
    return (IEnumerator<SystemFontGlyphInfo>) this.store.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.store.GetEnumerator();

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append((object) this.store[0]);
    for (int index = 1; index < this.store.Count; ++index)
    {
      stringBuilder.Append(" ");
      stringBuilder.Append((object) this.store[index]);
    }
    return stringBuilder.ToString();
  }
}
