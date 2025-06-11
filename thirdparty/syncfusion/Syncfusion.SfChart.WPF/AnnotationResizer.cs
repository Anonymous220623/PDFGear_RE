// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.AnnotationResizer
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal class AnnotationResizer : SolidShapeAnnotation
{
  public override UIElement GetRenderedAnnotation() => (UIElement) this.ResizerControl;

  internal override UIElement CreateAnnotation()
  {
    this.ResizerControl = new Resizer();
    this.ResizerControl.AnnotationResizer = this;
    this.SetBindings();
    return (UIElement) this.ResizerControl;
  }

  internal void MapActualValueToPixels() => this.ResizerControl.MapActualValueToPixels();

  protected override void SetBindings()
  {
    if (this.ResizerControl == null || this.Chart.AnnotationManager.SelectedAnnotation == null)
      return;
    Binding binding = new Binding()
    {
      Path = new PropertyPath("Cursor", new object[0]),
      Source = (object) this.Chart.AnnotationManager.SelectedAnnotation
    };
    this.ResizerControl.SetBinding(FrameworkElement.CursorProperty, (BindingBase) binding);
  }
}
