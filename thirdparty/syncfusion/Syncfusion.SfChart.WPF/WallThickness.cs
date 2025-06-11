// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.WallThickness
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.ComponentModel;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

[TypeConverter(typeof (WallThicknessConverter))]
[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
public struct WallThickness : IEquatable<WallThickness>
{
  private double _left;
  private double _bottom;
  private double _back;

  public double Left
  {
    get => this._left;
    set => this._left = value;
  }

  public double Bottom
  {
    get => this._bottom;
    set => this._bottom = value;
  }

  public double Back
  {
    get => this._back;
    set => this._back = value;
  }

  public WallThickness(double uniformValue)
    : this()
  {
    this.Left = this.Bottom = this.Back = uniformValue;
  }

  public WallThickness(double left, double bottom, double back)
    : this()
  {
    this.Left = left;
    this.Bottom = bottom;
    this.Back = back;
  }

  public override int GetHashCode() => base.GetHashCode();

  public override bool Equals(object obj)
  {
    return obj is WallThickness wallThickness && this.Equals(wallThickness);
  }

  public bool Equals(WallThickness wallThickness)
  {
    return this.Left == wallThickness.Left && this.Bottom == wallThickness.Bottom;
  }

  public static bool operator ==(WallThickness point1, WallThickness point2)
  {
    return point1.Equals(point2);
  }

  public static bool operator !=(WallThickness point1, WallThickness point2)
  {
    return !point1.Equals(point2);
  }
}
