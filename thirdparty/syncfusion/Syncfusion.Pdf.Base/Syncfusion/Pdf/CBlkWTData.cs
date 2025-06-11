// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.CBlkWTData
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class CBlkWTData
{
  public int ulx;
  public int uly;
  public int n;
  public int m;
  internal SubbandAn sb;
  public int w;
  public int h;
  public int offset;
  public int scanw;
  public int magbits;
  public float wmseScaling = 1f;
  public double convertFactor = 1.0;
  public double stepSize = 1.0;
  public int nROIcoeff;
  public int nROIbp;

  public abstract int DataType { get; }

  public abstract object Data { get; set; }

  public override string ToString()
  {
    string str = "";
    switch (this.DataType)
    {
      case 0:
        str = "Unsigned Byte";
        break;
      case 1:
        str = "Short";
        break;
      case 3:
        str = "Integer";
        break;
      case 4:
        str = "Float";
        break;
    }
    return $"ulx={(object) this.ulx}, uly={(object) this.uly}, idx=({(object) this.m},{(object) this.n}), w={(object) this.w}, h={(object) this.h}, off={(object) this.offset}, scanw={(object) this.scanw}, wmseScaling={(object) this.wmseScaling}, convertFactor={(object) this.convertFactor}, stepSize={(object) this.stepSize}, type={str}, magbits={(object) this.magbits}, nROIcoeff={(object) this.nROIcoeff}, nROIbp={(object) this.nROIbp}";
  }
}
