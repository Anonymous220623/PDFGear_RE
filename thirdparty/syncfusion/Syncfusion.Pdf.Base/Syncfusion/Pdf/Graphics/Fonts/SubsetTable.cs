// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.SubsetTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal abstract class SubsetTable
{
  internal abstract int Length { get; }

  internal abstract LookupSubTableRecord[] LookupRecord { get; }

  internal virtual int LookupLength => 0;

  internal virtual int BTCLength => 0;

  internal abstract bool Match(int id, int index);

  internal virtual bool IsLookup(int glyphId, int index) => false;

  internal virtual bool IsBackTrack(int glyphId, int index) => false;
}
