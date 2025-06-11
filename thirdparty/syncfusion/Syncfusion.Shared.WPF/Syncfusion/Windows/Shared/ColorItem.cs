// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ColorItem
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class ColorItem : IDisposable
{
  private string m_name;
  private SolidColorBrush m_brush;

  public string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  public SolidColorBrush Brush => this.m_brush;

  public ColorItem()
  {
  }

  public ColorItem(string name, SolidColorBrush brush)
  {
    this.m_name = name;
    this.m_brush = brush;
    Color color = brush.Color;
  }

  internal void Dispose(bool disposing)
  {
    this.m_brush = (SolidColorBrush) null;
    this.m_name = (string) null;
  }

  public void Dispose() => this.Dispose(true);
}
