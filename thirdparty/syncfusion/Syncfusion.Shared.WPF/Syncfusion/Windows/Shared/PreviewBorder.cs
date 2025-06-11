// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.PreviewBorder
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class PreviewBorder : Border
{
  private readonly Stretch m_Stretch;

  public PreviewBorder()
    : this(Stretch.Uniform)
  {
  }

  public PreviewBorder(Stretch stretch)
  {
    this.m_Stretch = stretch;
    this.DataContextChanged += new DependencyPropertyChangedEventHandler(this.OnDataContextChanged);
  }

  private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue is Visual newValue)
    {
      VisualBrush visualBrush = new VisualBrush();
      visualBrush.Visual = newValue;
      visualBrush.Stretch = this.m_Stretch;
      this.Background = (Brush) visualBrush;
    }
    else
      this.Background = (Brush) Brushes.Transparent;
  }
}
