// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.TableCollection
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class TableCollection : ITables, IEnumerable<ITable>, IEnumerable
{
  private BaseSlide _baseSlide;

  internal TableCollection(BaseSlide baseSlide) => this._baseSlide = baseSlide;

  public ITable AddTable(
    int rowCount,
    int columnCount,
    double left,
    double top,
    double width,
    double height)
  {
    return this._baseSlide.Shapes.AddTable(rowCount, columnCount, left, top, width, height);
  }

  public int IndexOf(ITable table) => this.GetList().IndexOf(table);

  public void Remove(ITable table) => ((Shapes) this._baseSlide.Shapes).Remove((ISlideItem) table);

  public void RemoveAt(int index) => this.Remove(this[index]);

  public ITable this[int index] => this.GetList()[index];

  public int Count => this.GetList().Count;

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetList().GetEnumerator();

  public IEnumerator<ITable> GetEnumerator()
  {
    return (IEnumerator<ITable>) this.GetList().GetEnumerator();
  }

  internal List<ITable> GetList()
  {
    List<ITable> list = new List<ITable>();
    foreach (IShape shape in (IEnumerable<ISlideItem>) this._baseSlide.Shapes)
    {
      if (shape.SlideItemType == SlideItemType.Table)
        list.Add((ITable) shape);
    }
    return list;
  }
}
