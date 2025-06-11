// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Cell
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public abstract class Cell : ContentControl
{
  public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (Cell), (PropertyMetadata) new UIPropertyMetadata((object) false));
  public static readonly DependencyProperty IsInvalidDateProperty = DependencyProperty.Register(nameof (IsInvalidDate), typeof (bool), typeof (Cell), (PropertyMetadata) new UIPropertyMetadata((object) false));
  private CornerRadius mcornerRadius;

  static Cell()
  {
    UIElement.FocusableProperty.OverrideMetadata(typeof (Cell), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
    Border.CornerRadiusProperty.AddOwner(typeof (Cell));
  }

  public CornerRadius CornerRadius
  {
    get => this.mcornerRadius;
    set => this.mcornerRadius = value;
  }

  public bool IsSelected
  {
    get => (bool) this.GetValue(Cell.IsSelectedProperty);
    set => this.SetValue(Cell.IsSelectedProperty, (object) value);
  }

  public bool IsInvalidDate
  {
    get => (bool) this.GetValue(Cell.IsInvalidDateProperty);
    set => this.SetValue(Cell.IsInvalidDateProperty, (object) value);
  }
}
