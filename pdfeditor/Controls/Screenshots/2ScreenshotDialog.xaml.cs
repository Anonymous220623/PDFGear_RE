// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.MarginModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using PDFKit.Utils.PageHeaderFooters;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public class MarginModel : ObservableObject
{
  private double top = 36.0;
  private double bottom = 36.0;
  private double left = 36.0;
  private double right = 36.0;
  private double pagewidth;
  private double pageheight;

  public ScreenshotDialog Screenshot { get; set; }

  public double Top
  {
    get => this.top;
    set
    {
      if (!this.SetProperty<double>(ref this.top, value, nameof (Top)))
        return;
      this.OnPropertyChanged("TopCm");
      if (this.Screenshot == null)
        return;
      this.Screenshot.curPt.X = (float) (this.pagewidth - this.Right);
      this.Screenshot.curPt.Y = (float) this.Bottom;
      this.Screenshot.startPt.Y = (float) (this.pageheight - this.Top);
      this.Screenshot.startPt.X = (float) this.Left;
      this.Screenshot.UpdateBounds(false, true);
    }
  }

  public double Bottom
  {
    get => this.bottom;
    set
    {
      if (!this.SetProperty<double>(ref this.bottom, value, nameof (Bottom)))
        return;
      this.OnPropertyChanged("BottomCm");
      if (this.Screenshot == null)
        return;
      this.Screenshot.curPt.X = (float) (this.pagewidth - this.Right);
      this.Screenshot.curPt.Y = (float) this.Bottom;
      this.Screenshot.startPt.Y = (float) (this.pageheight - this.Top);
      this.Screenshot.startPt.X = (float) this.Left;
      this.Screenshot.UpdateBounds(false, true);
    }
  }

  public double Left
  {
    get => this.left;
    set
    {
      if (!this.SetProperty<double>(ref this.left, value, nameof (Left)))
        return;
      this.OnPropertyChanged("LeftCm");
      if (this.Screenshot == null)
        return;
      this.Screenshot.curPt.X = (float) (this.pagewidth - this.Right);
      this.Screenshot.curPt.Y = (float) this.Bottom;
      this.Screenshot.startPt.Y = (float) (this.pageheight - this.Top);
      this.Screenshot.startPt.X = (float) this.Left;
      this.Screenshot.UpdateBounds(false, true);
    }
  }

  public double Right
  {
    get => this.right;
    set
    {
      if (!this.SetProperty<double>(ref this.right, value, nameof (Right)))
        return;
      this.OnPropertyChanged("RightCm");
      if (this.Screenshot == null)
        return;
      this.Screenshot.curPt.X = (float) (this.pagewidth - this.Right);
      this.Screenshot.curPt.Y = (float) this.Bottom;
      this.Screenshot.startPt.Y = (float) (this.pageheight - this.Top);
      this.Screenshot.startPt.X = (float) this.Left;
      this.Screenshot.UpdateBounds(false, true);
    }
  }

  public double PageWidth
  {
    get => this.pagewidth;
    set => this.SetProperty<double>(ref this.pagewidth, value, nameof (PageWidth));
  }

  public double PageHeight
  {
    get => this.pageheight;
    set => this.SetProperty<double>(ref this.pageheight, value, nameof (PageHeight));
  }

  public double TopCm
  {
    get => PageHeaderFooterUtils.PdfPointToCm(this.Top);
    set
    {
      if (value < 0.0 || value > PageHeaderFooterUtils.PdfPointToCm(this.PageHeight - this.Bottom))
        return;
      this.Top = PageHeaderFooterUtils.CmToPdfPoint(value);
    }
  }

  public double BottomCm
  {
    get => PageHeaderFooterUtils.PdfPointToCm(this.Bottom);
    set
    {
      if (value < 0.0 || value > PageHeaderFooterUtils.PdfPointToCm(this.PageHeight - this.Top))
        return;
      this.Bottom = PageHeaderFooterUtils.CmToPdfPoint(value);
    }
  }

  public double LeftCm
  {
    get => PageHeaderFooterUtils.PdfPointToCm(this.Left);
    set
    {
      if (value < 0.0 || value > PageHeaderFooterUtils.PdfPointToCm(this.PageWidth - this.Right))
        return;
      this.Left = PageHeaderFooterUtils.CmToPdfPoint(value);
    }
  }

  public double RightCm
  {
    get => PageHeaderFooterUtils.PdfPointToCm(this.Right);
    set
    {
      if (value < 0.0 || value > PageHeaderFooterUtils.PdfPointToCm(this.PageWidth - this.Left))
        return;
      this.Right = PageHeaderFooterUtils.CmToPdfPoint(value);
    }
  }
}
