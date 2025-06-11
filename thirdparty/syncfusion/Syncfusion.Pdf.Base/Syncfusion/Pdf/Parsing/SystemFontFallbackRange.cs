// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontFallbackRange
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontFallbackRange
{
  private static List<SystemFontFallbackRange> fallbackRanges;

  public SystemFontRange[] Ranges { get; private set; }

  public string[] FallbackFontFamilies { get; private set; }

  private static void InitializeFallbackRanges()
  {
    SystemFontFallbackRange.fallbackRanges = new List<SystemFontFallbackRange>();
    SystemFontFallbackRange.fallbackRanges.Add(new SystemFontFallbackRange(new SystemFontRange[2]
    {
      new SystemFontRange(0, 591),
      new SystemFontRange(1024 /*0x0400*/, 1327)
    }, new string[1]{ "Times New Roman" }));
  }

  internal static SystemFontFallbackRange GetFallbackRange(char unicode)
  {
    foreach (SystemFontFallbackRange fallbackRange in SystemFontFallbackRange.fallbackRanges)
    {
      if (fallbackRange.FallsInRange(unicode))
        return fallbackRange;
    }
    return (SystemFontFallbackRange) null;
  }

  static SystemFontFallbackRange() => SystemFontFallbackRange.InitializeFallbackRanges();

  public SystemFontFallbackRange(SystemFontRange[] ranges, string[] fallbackFontFamilies)
  {
    this.Ranges = ranges;
    this.FallbackFontFamilies = fallbackFontFamilies;
  }

  public bool FallsInRange(char unicode)
  {
    foreach (SystemFontRange range in this.Ranges)
    {
      if (range.IsInRange((int) unicode))
        return true;
    }
    return false;
  }
}
