// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.PicturesCollection
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class PicturesCollection : IPictures, IEnumerable<IPicture>, IEnumerable
{
  private BaseSlide _baseSlide;

  internal PicturesCollection(BaseSlide baseSlide) => this._baseSlide = baseSlide;

  public IPicture AddPicture(Stream stream, double left, double top, double width, double height)
  {
    return this._baseSlide.Shapes.AddPicture(stream, left, top, width, height);
  }

  public IPicture AddPicture(
    Stream svgStream,
    Stream rasterImgStream,
    double left,
    double top,
    double width,
    double height)
  {
    return this._baseSlide.Shapes.AddPicture(svgStream, rasterImgStream, left, top, width, height);
  }

  public int IndexOf(IPicture picture) => this.GetList().IndexOf(picture);

  public void Remove(IPicture picture)
  {
    ((Shapes) this._baseSlide.Shapes).Remove((ISlideItem) picture);
  }

  public void RemoveAt(int index) => this.Remove(this[index]);

  public IPicture this[int index] => this.GetList()[index];

  public int Count => this.GetList().Count;

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetList().GetEnumerator();

  public IEnumerator<IPicture> GetEnumerator()
  {
    return (IEnumerator<IPicture>) this.GetList().GetEnumerator();
  }

  internal List<IPicture> GetList()
  {
    List<IPicture> list = new List<IPicture>();
    foreach (IShape shape in (IEnumerable<ISlideItem>) this._baseSlide.Shapes)
    {
      if (shape.SlideItemType == SlideItemType.Picture)
        list.Add((IPicture) shape);
    }
    return list;
  }
}
