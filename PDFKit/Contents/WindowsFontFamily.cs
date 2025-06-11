// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.WindowsFontFamily
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace PDFKit.Contents;

public class WindowsFontFamily
{
  internal WindowsFontFamily(string name, IEnumerable<Patagames.Pdf.LOGFONT> logfonts)
  {
    this.Name = name;
    this.LOGFONT = (IReadOnlyDictionary<FontCharSet, Patagames.Pdf.LOGFONT>) logfonts.GroupBy<Patagames.Pdf.LOGFONT, FontCharSet>((Func<Patagames.Pdf.LOGFONT, FontCharSet>) (c => c.lfCharSet)).Select<IGrouping<FontCharSet, Patagames.Pdf.LOGFONT>, (FontCharSet, Patagames.Pdf.LOGFONT)>((Func<IGrouping<FontCharSet, Patagames.Pdf.LOGFONT>, (FontCharSet, Patagames.Pdf.LOGFONT)>) (c => (c.Key, c.FirstOrDefault<Patagames.Pdf.LOGFONT>()))).ToDictionary<(FontCharSet, Patagames.Pdf.LOGFONT), FontCharSet, Patagames.Pdf.LOGFONT>((Func<(FontCharSet, Patagames.Pdf.LOGFONT), FontCharSet>) (c => c.Key), (Func<(FontCharSet, Patagames.Pdf.LOGFONT), Patagames.Pdf.LOGFONT>) (c => c.Item2));
  }

  public string Name { get; }

  public IReadOnlyDictionary<FontCharSet, Patagames.Pdf.LOGFONT> LOGFONT { get; }

  public override string ToString() => this.Name;
}
