// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.DataBlockFloat
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class DataBlockFloat : DataBlock
{
  private float[] data;

  public override int DataType => 4;

  public override object Data
  {
    get => (object) this.data;
    set => this.data = (float[]) value;
  }

  public virtual float[] DataFloat
  {
    get => this.data;
    set => this.data = value;
  }

  public DataBlockFloat()
  {
  }

  public DataBlockFloat(int ulx, int uly, int w, int h)
  {
    this.ulx = ulx;
    this.uly = uly;
    this.w = w;
    this.h = h;
    this.offset = 0;
    this.scanw = w;
    this.data = new float[w * h];
  }

  public DataBlockFloat(DataBlockFloat src)
  {
    this.ulx = src.ulx;
    this.uly = src.uly;
    this.w = src.w;
    this.h = src.h;
    this.offset = 0;
    this.scanw = this.w;
    this.data = new float[this.w * this.h];
    for (int index = 0; index < this.h; ++index)
      Array.Copy((Array) src.data, index * src.scanw, (Array) this.data, index * this.scanw, this.w);
  }

  public override string ToString()
  {
    string str = base.ToString();
    if (this.data != null)
      str = $"{str},data={(object) this.data.Length} bytes";
    return str;
  }
}
