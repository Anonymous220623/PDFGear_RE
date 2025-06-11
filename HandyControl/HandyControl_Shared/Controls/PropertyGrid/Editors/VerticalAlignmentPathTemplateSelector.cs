// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.VerticalAlignmentPathTemplateSelector
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

public class VerticalAlignmentPathTemplateSelector : DataTemplateSelector
{
  public override DataTemplate SelectTemplate(object item, DependencyObject container)
  {
    if (!(item is VerticalAlignment verticalAlignment))
      return (DataTemplate) null;
    DataTemplate dataTemplate = new DataTemplate()
    {
      DataType = (object) typeof (System.Windows.Controls.ComboBox)
    };
    FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof (Path));
    frameworkElementFactory.SetValue(FrameworkElement.WidthProperty, ValueBoxes.Double10Box);
    frameworkElementFactory.SetValue(FrameworkElement.HeightProperty, (object) 12.0);
    frameworkElementFactory.SetBinding(Shape.FillProperty, (BindingBase) new Binding(Control.ForegroundProperty.Name)
    {
      RelativeSource = new RelativeSource()
      {
        AncestorType = typeof (ComboBoxItem)
      }
    });
    switch (verticalAlignment)
    {
      case VerticalAlignment.Top:
        frameworkElementFactory.SetValue(Path.DataProperty, (object) ResourceHelper.GetResourceInternal<Geometry>("AlignTopGeometry"));
        break;
      case VerticalAlignment.Center:
        frameworkElementFactory.SetValue(Path.DataProperty, (object) ResourceHelper.GetResourceInternal<Geometry>("AlignVCenterGeometry"));
        break;
      case VerticalAlignment.Bottom:
        frameworkElementFactory.SetValue(Path.DataProperty, (object) ResourceHelper.GetResourceInternal<Geometry>("AlignBottomGeometry"));
        break;
      case VerticalAlignment.Stretch:
        frameworkElementFactory.SetValue(Path.DataProperty, (object) ResourceHelper.GetResourceInternal<Geometry>("AlignVStretchGeometry"));
        break;
    }
    dataTemplate.VisualTree = frameworkElementFactory;
    return dataTemplate;
  }
}
