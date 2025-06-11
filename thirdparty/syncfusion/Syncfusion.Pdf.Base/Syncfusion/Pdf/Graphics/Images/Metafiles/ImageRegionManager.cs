// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Metafiles.ImageRegionManager
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Metafiles;

internal class ImageRegionManager
{
  private ArrayList m_regions;

  public ImageRegionManager() => this.m_regions = new ArrayList();

  public void Add(ImageRegion region)
  {
    ImageRegion[] regions = region != null ? this.Intersect(region) : throw new ArgumentNullException(nameof (region));
    this.m_regions.Add((object) this.Union(regions, region));
    this.Remove(regions);
  }

  public float GetTopCoordinate(float y)
  {
    float topCoordinate = y;
    ImageRegion[] imageRegionArray = this.Intersect(new ImageRegion(y, 1f));
    if (imageRegionArray != null && imageRegionArray.Length == 1)
      topCoordinate = imageRegionArray[0].Y;
    else if (imageRegionArray != null && imageRegionArray.Length > 1)
    {
      ImageRegion imageRegion1 = imageRegionArray[0];
      ImageRegion imageRegion2 = imageRegionArray[1];
      topCoordinate = (double) imageRegion1.Y >= (double) imageRegion2.Y ? imageRegion2.Y : imageRegion1.Y;
    }
    return topCoordinate;
  }

  public float GetCoordinate(float y)
  {
    float coordinate = y;
    ImageRegion[] imageRegionArray = this.Intersect(new ImageRegion(y, 1f));
    if (imageRegionArray != null && imageRegionArray.Length == 1)
      coordinate = imageRegionArray[0].Y;
    else if (imageRegionArray != null && imageRegionArray.Length > 1)
    {
      ImageRegion imageRegion1 = imageRegionArray[0];
      ImageRegion imageRegion2 = imageRegionArray[1];
      coordinate = (double) imageRegion1.Y >= (double) imageRegion2.Y ? imageRegion2.Y : imageRegion1.Y;
    }
    else if (imageRegionArray == null)
      coordinate = 0.0f;
    return coordinate;
  }

  public void Clear() => this.m_regions.Clear();

  private ImageRegion[] Intersect(ImageRegion region)
  {
    if (region == null)
      throw new ArgumentNullException(nameof (region));
    ArrayList arrayList = new ArrayList();
    int index = 0;
    for (int count = this.m_regions.Count; index < count; ++index)
    {
      ImageRegion region1 = (ImageRegion) this.m_regions[index];
      if (region.IntersectsWith(region1))
        arrayList.Add((object) region1);
    }
    return (ImageRegion[]) arrayList.ToArray(typeof (ImageRegion));
  }

  private void Remove(ImageRegion region)
  {
    if (region == null)
      throw new ArgumentNullException(nameof (region));
    this.m_regions.Remove((object) region);
  }

  private void Remove(ImageRegion[] regions)
  {
    if (regions == null)
      throw new ArgumentNullException(nameof (regions));
    int index = 0;
    for (int length = regions.Length; index < length; ++index)
      this.m_regions.Remove((object) regions[index]);
  }

  private ImageRegion Union(ImageRegion[] regions, ImageRegion region)
  {
    if (regions == null)
      throw new ArgumentNullException(nameof (regions));
    if (region == null)
      throw new ArgumentNullException(nameof (region));
    int index = 0;
    for (int length = regions.Length; index < length; ++index)
    {
      ImageRegion region1 = regions[index];
      if (region.IntersectsWith(region1))
        region = ImageRegion.Union(region, region1);
    }
    return region;
  }
}
