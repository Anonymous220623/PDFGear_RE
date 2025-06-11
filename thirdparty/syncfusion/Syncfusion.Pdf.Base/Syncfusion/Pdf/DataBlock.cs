// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.DataBlock
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class DataBlock
{
  public const int TYPE_BYTE = 0;
  public const int TYPE_SHORT = 1;
  public const int TYPE_INT = 3;
  public const int TYPE_FLOAT = 4;
  public int ulx;
  public int uly;
  public int w;
  public int h;
  public int offset;
  public int scanw;
  public bool progressive;

  public abstract int DataType { get; }

  public abstract object Data { get; set; }

  public static int getSize(int type)
  {
    switch (type)
    {
      case 0:
        return 8;
      case 1:
        return 16 /*0x10*/;
      case 3:
      case 4:
        return 32 /*0x20*/;
      default:
        throw new ArgumentException();
    }
  }

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
    return $"DataBlk: upper-left({(object) this.ulx},{(object) this.uly}), width={(object) this.w}, height={(object) this.h}, progressive={(object) this.progressive}, offset={(object) this.offset}, scanw={(object) this.scanw}, type={str}";
  }
}
