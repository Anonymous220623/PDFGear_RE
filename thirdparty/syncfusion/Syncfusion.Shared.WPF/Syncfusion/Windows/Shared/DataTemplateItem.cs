// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DataTemplateItem
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class DataTemplateItem
{
  private DateTime m_date;
  private DataTemplate m_template;

  public DataTemplateItem()
  {
  }

  public DataTemplateItem(DateTime date, DataTemplate template)
  {
    this.m_date = date;
    this.m_template = template;
  }

  public DateTime Date
  {
    get => this.m_date;
    set => this.m_date = value;
  }

  public DataTemplate Template
  {
    get => this.m_template;
    set => this.m_template = value;
  }
}
