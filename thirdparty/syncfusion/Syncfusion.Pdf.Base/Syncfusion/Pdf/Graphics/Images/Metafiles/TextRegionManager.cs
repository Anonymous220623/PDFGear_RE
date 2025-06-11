// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Metafiles.TextRegionManager
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Metafiles;

internal class TextRegionManager
{
  private ArrayList m_regions;

  public TextRegionManager() => this.m_regions = new ArrayList();

  internal int Count => this.m_regions.Count;

  public void Add(TextRegion region)
  {
    TextRegion[] regions = region != null ? this.Intersect(region) : throw new ArgumentNullException(nameof (region));
    this.m_regions.Add((object) this.Union(regions, region));
    this.Remove(regions);
  }

  public float GetTopCoordinate(float y)
  {
    float topCoordinate = y;
    TextRegion[] textRegionArray = this.Intersect(new TextRegion(y, 1f));
    if (textRegionArray != null && textRegionArray.Length == 1)
      topCoordinate = textRegionArray[0].Y;
    else if (textRegionArray != null && textRegionArray.Length > 1)
    {
      TextRegion textRegion1 = textRegionArray[0];
      TextRegion textRegion2 = textRegionArray[1];
      topCoordinate = (double) textRegion1.Y >= (double) textRegion2.Y ? textRegion2.Y : textRegion1.Y;
    }
    return topCoordinate;
  }

  public float GetCoordinate(float y)
  {
    float coordinate = y;
    TextRegion[] textRegionArray = this.Intersect(new TextRegion(y, 1f));
    if (textRegionArray != null && textRegionArray.Length == 1)
      coordinate = textRegionArray[0].Y;
    else if (textRegionArray != null && textRegionArray.Length > 1)
    {
      TextRegion textRegion1 = textRegionArray[0];
      TextRegion textRegion2 = textRegionArray[1];
      coordinate = (double) textRegion1.Y >= (double) textRegion2.Y ? textRegion2.Y : textRegion1.Y;
    }
    else if (textRegionArray.Length == 0)
      coordinate = 0.0f;
    return coordinate;
  }

  public void Clear() => this.m_regions.Clear();

  private TextRegion[] Intersect(TextRegion region)
  {
    if (region == null)
      throw new ArgumentNullException(nameof (region));
    ArrayList arrayList = new ArrayList();
    int index = 0;
    for (int count = this.m_regions.Count; index < count; ++index)
    {
      TextRegion region1 = (TextRegion) this.m_regions[index];
      if (region.IntersectsWith(region1))
        arrayList.Add((object) region1);
    }
    return (TextRegion[]) arrayList.ToArray(typeof (TextRegion));
  }

  private void Remove(TextRegion region)
  {
    if (region == null)
      throw new ArgumentNullException(nameof (region));
    this.m_regions.Remove((object) region);
  }

  private void Remove(TextRegion[] regions)
  {
    if (regions == null)
      throw new ArgumentNullException(nameof (regions));
    int index = 0;
    for (int length = regions.Length; index < length; ++index)
      this.m_regions.Remove((object) regions[index]);
  }

  private TextRegion Union(TextRegion[] regions, TextRegion region)
  {
    if (regions == null)
      throw new ArgumentNullException(nameof (regions));
    if (region == null)
      throw new ArgumentNullException(nameof (region));
    int index = 0;
    for (int length = regions.Length; index < length; ++index)
    {
      TextRegion region1 = regions[index];
      if (region.IntersectsWith(region1))
        region = TextRegion.Union(region, region1);
    }
    return region;
  }
}
