// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.VisualToDrawingConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[ValueConversion(typeof (Visual), typeof (Drawing))]
internal class VisualToDrawingConverter : IValueConverter
{
  private readonly DrawingHelper m_helper = new DrawingHelper();
  private Visual m_topMostVisual;
  private int m_convertionChainCount;

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    DrawingGroup drawingGroup = (DrawingGroup) null;
    if (value is Visual current)
    {
      if (this.m_topMostVisual == null || this.m_convertionChainCount == 0)
        this.m_topMostVisual = current;
      ++this.m_convertionChainCount;
      drawingGroup = VisualTreeHelper.GetDrawing(value as Visual);
      if (drawingGroup != null)
      {
        IEnumerable children = LogicalTreeHelper.GetChildren((DependencyObject) current);
        if (children != null)
        {
          foreach (object obj in children)
          {
            if (obj is Visual visual1 && this.Convert((object) visual1, typeof (Drawing), parameter, culture) is Drawing drawing1)
            {
              GeneralTransform visual = visual1.TransformToVisual(this.m_topMostVisual);
              Drawing drawing = DrawingHelper.ApplyTransformToDrawing(drawing1, visual);
              drawingGroup.Children.Add(drawing);
            }
          }
        }
      }
      --this.m_convertionChainCount;
    }
    return (object) drawingGroup;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
