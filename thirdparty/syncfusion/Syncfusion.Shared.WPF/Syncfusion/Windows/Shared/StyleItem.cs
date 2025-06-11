// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.StyleItem
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class StyleItem
{
  private DateTime m_date;
  private Style m_style;

  public StyleItem()
  {
  }

  public StyleItem(DateTime date, Style style)
  {
    this.m_date = date;
    this.m_style = style;
  }

  public DateTime Date
  {
    get => this.m_date;
    set => this.m_date = value;
  }

  public Style Style
  {
    get => this.m_style;
    set => this.m_style = value;
  }
}
