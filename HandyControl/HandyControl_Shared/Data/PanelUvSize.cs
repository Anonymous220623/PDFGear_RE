// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.PanelUvSize
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Data;

internal struct PanelUvSize
{
  private readonly Orientation _orientation;

  public Size ScreenSize => new Size(this.U, this.V);

  public double U { get; set; }

  public double V { get; set; }

  public double Width
  {
    get => this._orientation != Orientation.Horizontal ? this.V : this.U;
    private set
    {
      if (this._orientation == Orientation.Horizontal)
        this.U = value;
      else
        this.V = value;
    }
  }

  public double Height
  {
    get => this._orientation != Orientation.Horizontal ? this.U : this.V;
    private set
    {
      if (this._orientation == Orientation.Horizontal)
        this.V = value;
      else
        this.U = value;
    }
  }

  public PanelUvSize(Orientation orientation, double width, double height)
  {
    this.U = this.V = 0.0;
    this._orientation = orientation;
    this.Width = width;
    this.Height = height;
  }

  public PanelUvSize(Orientation orientation, Size size)
  {
    this.U = this.V = 0.0;
    this._orientation = orientation;
    this.Width = size.Width;
    this.Height = size.Height;
  }

  public PanelUvSize(Orientation orientation)
  {
    this.U = this.V = 0.0;
    this._orientation = orientation;
  }
}
