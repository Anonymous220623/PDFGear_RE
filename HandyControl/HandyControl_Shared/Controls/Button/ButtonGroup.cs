// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ButtonGroup
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class ButtonGroup : ItemsControl
{
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (ButtonGroup), new PropertyMetadata((object) Orientation.Horizontal));
  public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(nameof (Layout), typeof (LinearLayout), typeof (ButtonGroup), new PropertyMetadata((object) LinearLayout.Uniform));

  protected override bool IsItemItsOwnContainerOverride(object item)
  {
    bool flag;
    switch (item)
    {
      case Button _:
      case RadioButton _:
      case ToggleButton _:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    return flag;
  }

  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(ButtonGroup.OrientationProperty);
    set => this.SetValue(ButtonGroup.OrientationProperty, (object) value);
  }

  public LinearLayout Layout
  {
    get => (LinearLayout) this.GetValue(ButtonGroup.LayoutProperty);
    set => this.SetValue(ButtonGroup.LayoutProperty, (object) value);
  }

  protected override void OnRender(DrawingContext drawingContext)
  {
    int count = this.Items.Count;
    for (int index = 0; index < count; ++index)
    {
      ButtonBase buttonBase = (ButtonBase) this.Items[index];
      buttonBase.Style = this.ItemContainerStyleSelector?.SelectStyle((object) buttonBase, (DependencyObject) this);
    }
  }
}
