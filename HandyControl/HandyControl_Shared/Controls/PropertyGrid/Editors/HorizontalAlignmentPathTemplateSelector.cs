// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.HorizontalAlignmentPathTemplateSelector
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace HandyControl.Controls;

public class HorizontalAlignmentPathTemplateSelector : DataTemplateSelector
{
  public override DataTemplate SelectTemplate(object item, DependencyObject container)
  {
    if (!(item is HorizontalAlignment horizontalAlignment))
      return (DataTemplate) null;
    DataTemplate dataTemplate = new DataTemplate()
    {
      DataType = (object) typeof (System.Windows.Controls.ComboBox)
    };
    FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof (Path));
    frameworkElementFactory.SetValue(FrameworkElement.WidthProperty, (object) 12.0);
    frameworkElementFactory.SetValue(FrameworkElement.HeightProperty, ValueBoxes.Double10Box);
    frameworkElementFactory.SetBinding(Shape.FillProperty, (BindingBase) new Binding(Control.ForegroundProperty.Name)
    {
      RelativeSource = new RelativeSource()
      {
        AncestorType = typeof (ComboBoxItem)
      }
    });
    switch (horizontalAlignment)
    {
      case HorizontalAlignment.Left:
        frameworkElementFactory.SetValue(Path.DataProperty, (object) ResourceHelper.GetResourceInternal<Geometry>("AlignLeftGeometry"));
        break;
      case HorizontalAlignment.Center:
        frameworkElementFactory.SetValue(Path.DataProperty, (object) ResourceHelper.GetResourceInternal<Geometry>("AlignHCenterGeometry"));
        break;
      case HorizontalAlignment.Right:
        frameworkElementFactory.SetValue(Path.DataProperty, (object) ResourceHelper.GetResourceInternal<Geometry>("AlignRightGeometry"));
        break;
      case HorizontalAlignment.Stretch:
        frameworkElementFactory.SetValue(Path.DataProperty, (object) ResourceHelper.GetResourceInternal<Geometry>("AlignHStretchGeometry"));
        break;
    }
    dataTemplate.VisualTree = frameworkElementFactory;
    return dataTemplate;
  }
}
