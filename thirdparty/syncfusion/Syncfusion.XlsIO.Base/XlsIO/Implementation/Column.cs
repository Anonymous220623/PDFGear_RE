// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Column
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class Column
{
  private byte binrayInfo;
  public double defaultWidth;
  private int styleIndex;
  private short minCol;
  private WorksheetImpl worksheet;

  public int Index => (int) this.minCol;

  public bool IsHidden
  {
    get => this.GetHiddenInfo();
    set
    {
      this.SetHiddenInfo(value);
      this.SetBestFitInfo(false);
      if (value || this.defaultWidth != 0.0)
        return;
      this.defaultWidth = this.worksheet.Columnss.Width;
    }
  }

  public double Width
  {
    get => this.defaultWidth;
    set
    {
      if (value < double.Epsilon)
      {
        this.SetHiddenInfo(true);
      }
      else
      {
        this.defaultWidth = value;
        this.SetHiddenInfo(false);
      }
      this.SetBestFitInfo(false);
    }
  }

  internal Column(int minCol, WorksheetImpl worksheet, double defaultWidth)
  {
    this.styleIndex = -1;
    this.minCol = (short) minCol;
    this.worksheet = worksheet;
    this.defaultWidth = defaultWidth;
  }

  internal void SetMinColumnIndex(int minCol) => this.minCol = (short) minCol;

  internal void SetCollapsedInfo(bool isCollapsed)
  {
    if (isCollapsed)
      this.binrayInfo |= (byte) 16 /*0x10*/;
    else
      this.binrayInfo &= (byte) 239;
  }

  internal void SetBestFitInfo(bool isBestFit)
  {
    if (isBestFit)
      this.binrayInfo |= (byte) 64 /*0x40*/;
    else
      this.binrayInfo &= (byte) 191;
  }

  internal void SetWidth(int width)
  {
    int fontCalc2 = this.worksheet.GetAppImpl().GetFontCalc2();
    int fontCalc1 = this.worksheet.GetAppImpl().GetFontCalc1();
    int fontCalc3 = this.worksheet.GetAppImpl().GetFontCalc3();
    int num = width;
    this.defaultWidth = num >= fontCalc2 + fontCalc3 ? (double) (int) ((double) (num - (int) ((double) (fontCalc2 * fontCalc1) / 256.0 + 0.5)) * 100.0 / (double) fontCalc2 + 0.5) / 100.0 : 1.0 * (double) num / (double) (fontCalc2 + fontCalc3);
    this.SetBestFitInfo(false);
  }

  internal void SetOutLineLevel(byte outLineLevel)
  {
    this.binrayInfo &= (byte) 240 /*0xF0*/;
    this.binrayInfo |= outLineLevel;
  }

  internal void SetStyleIndex(int styleIndex) => this.styleIndex = styleIndex;

  internal bool GetHiddenInfo() => ((int) this.binrayInfo & 32 /*0x20*/) != 0;

  internal void SetHiddenInfo(bool isHidden)
  {
    if (!isHidden)
      this.binrayInfo &= (byte) 223;
    else
      this.binrayInfo |= (byte) 32 /*0x20*/;
  }
}
