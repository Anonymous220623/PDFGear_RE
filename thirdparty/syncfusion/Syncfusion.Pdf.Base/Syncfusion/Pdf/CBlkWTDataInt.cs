// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.CBlkWTDataInt
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class CBlkWTDataInt : CBlkWTData
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
}
