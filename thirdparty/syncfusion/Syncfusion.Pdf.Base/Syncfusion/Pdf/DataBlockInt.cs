// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.DataBlockInt
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class DataBlockInt : DataBlock
{
  public int[] data_array;

  public override int DataType => 3;

  public override object Data
  {
    get => (object) this.data_array;
    set => this.data_array = (int[]) value;
  }

  public virtual int[] DataInt
  {
    get => this.data_array;
    set => this.data_array = value;
  }

  public DataBlockInt()
  {
  }

  public DataBlockInt(int ulx, int uly, int w, int h)
  {
    this.ulx = ulx;
    this.uly = uly;
    this.w = w;
    this.h = h;
    this.offset = 0;
    this.scanw = w;
    this.data_array = new int[w * h];
  }

  public DataBlockInt(DataBlockInt src)
  {
    this.ulx = src.ulx;
    this.uly = src.uly;
    this.w = src.w;
    this.h = src.h;
    this.offset = 0;
    this.scanw = this.w;
    this.data_array = new int[this.w * this.h];
    for (int index = 0; index < this.h; ++index)
      Array.Copy((Array) src.data_array, index * src.scanw, (Array) this.data_array, index * this.scanw, this.w);
  }

  public override string ToString()
  {
    string str = base.ToString();
    if (this.data_array != null)
      str = $"{str},data={(object) this.data_array.Length} bytes";
    return str;
  }
}
