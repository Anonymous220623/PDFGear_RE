// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SolidShapeAnnotation
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class SolidShapeAnnotation : ShapeAnnotation
{
  public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(nameof (Angle), typeof (double), typeof (SolidShapeAnnotation), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(SolidShapeAnnotation.OnUpdatePropertyChanged)));
  public static readonly DependencyProperty ResizingModeProperty = DependencyProperty.Register(nameof (ResizingMode), typeof (AxisMode), typeof (SolidShapeAnnotation), new PropertyMetadata((object) AxisMode.All, new PropertyChangedCallback(SolidShapeAnnotation.OnResizingPathChanged)));

  public double Angle
  {
    get => (double) this.GetValue(SolidShapeAnnotation.AngleProperty);
    set => this.SetValue(SolidShapeAnnotation.AngleProperty, (object) value);
  }

  public AxisMode ResizingMode
  {
    get => (AxisMode) this.GetValue(SolidShapeAnnotation.ResizingModeProperty);
    set => this.SetValue(SolidShapeAnnotation.ResizingModeProperty, (object) value);
  }

  private static void OnUpdatePropertyChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(sender is Annotation annotation))
      return;
    annotation.UpdatePropertyChanged(args);
  }

  private static void OnResizingPathChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    SolidShapeAnnotation solidShapeAnnotation = d as SolidShapeAnnotation;
    if (e.NewValue == e.OldValue || solidShapeAnnotation.Chart == null)
      return;
    solidShapeAnnotation.UpdateResizingPath((AxisMode) e.NewValue);
  }

  private void UpdateResizingPath(AxisMode path)
  {
    AnnotationResizer annotationResizer = this.Chart.AnnotationManager.AnnotationResizer;
    if (annotationResizer == null || annotationResizer.ResizingMode == path)
      return;
    annotationResizer.ResizingMode = path;
    annotationResizer.ResizerControl.ChangeView();
  }
}
