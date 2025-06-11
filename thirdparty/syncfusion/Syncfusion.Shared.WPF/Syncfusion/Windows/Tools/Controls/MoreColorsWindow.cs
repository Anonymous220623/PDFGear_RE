// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.MoreColorsWindow
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

public class MoreColorsWindow : Window, IComponentConnector
{
  private double adj;
  internal double hyp = 10.0;
  private double opp;
  internal double xp;
  internal double yp;
  internal double x = 90.0;
  internal double y = 20.0;
  private PolygonItem polygontemp;
  internal PolygonItem polygonitem;
  internal Binding bind;
  public ObservableCollection<PolygonItem> morecolorcollection;
  public ColorPickerPalette palette;
  private double xprev;
  private double yprev;
  internal Grid WindowGrid;
  internal Grid LayoutRoot;
  internal Button OKButton;
  internal Button CancelButton;
  internal Border New;
  internal Border Current;
  internal TabControl tab;
  internal TabItem standard;
  internal StackPanel standardPanel;
  internal ItemsControl Item;
  internal Path path;
  internal Path path1;
  internal TabItem custom;
  internal StackPanel custompanel;
  internal ColorEdit asd;
  internal Thumb thumb1;
  internal Thumb thumb2;
  internal Thumb thumb3;
  private bool _contentLoaded;

  internal Brush color { get; set; }

  public MoreColorsWindow()
  {
    this.InitializeComponent();
    this.KeyDown += new KeyEventHandler(this.ChildWindow1KeyDown);
    this.Unloaded += new RoutedEventHandler(this.MoreColorsWindow_Unloaded);
    this.LoadPolygonItem();
    this.polygonitem = new PolygonItem();
    this.color = (Brush) new SolidColorBrush(Colors.Transparent);
  }

  private void MoreColorsWindow_Unloaded(object sender, RoutedEventArgs e)
  {
    this.morecolorcollection = (ObservableCollection<PolygonItem>) null;
    this.Item = (ItemsControl) null;
    if (this.palette != null)
      this.palette.child = (MoreColorsWindow) null;
    this.polygonitem = (PolygonItem) null;
  }

  private void LoadPolygonItem()
  {
    this.opp = Math.Sin(Math.PI / 6.0) * this.hyp;
    this.adj = Math.Cos(Math.PI / 6.0) * this.hyp;
    this.bind = new Binding();
    this.morecolorcollection = new ObservableCollection<PolygonItem>();
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 51, (byte) 102)),
      RowIndex = 1,
      ColumnIndex = 1,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 2.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 102, (byte) 153)),
      RowIndex = 1,
      ColumnIndex = 2,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 4.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 102, (byte) 204)),
      RowIndex = 1,
      ColumnIndex = 3,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 6.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 51, (byte) 153)),
      RowIndex = 1,
      ColumnIndex = 4,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 8.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 153)),
      RowIndex = 1,
      ColumnIndex = 5,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 10.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 204)),
      RowIndex = 1,
      ColumnIndex = 6,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 12.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 102)),
      RowIndex = 1,
      ColumnIndex = 7,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")
    });
    this.x -= this.adj;
    this.y = this.y + this.opp + this.hyp;
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 102, (byte) 102)),
      RowIndex = 2,
      ColumnIndex = 1,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Teal")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 2.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 102, (byte) 153)),
      RowIndex = 2,
      ColumnIndex = 2,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Teal")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 4.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 153, (byte) 204)),
      RowIndex = 2,
      ColumnIndex = 3,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Turquoise")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 6.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 102, (byte) 204)),
      RowIndex = 2,
      ColumnIndex = 4,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 8.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 51, (byte) 204)),
      RowIndex = 2,
      ColumnIndex = 5,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 10.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, byte.MaxValue)),
      RowIndex = 2,
      ColumnIndex = 6,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 12.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 51, byte.MaxValue)),
      RowIndex = 2,
      ColumnIndex = 7,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 14.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 51, (byte) 153)),
      RowIndex = 2,
      ColumnIndex = 8,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Indigo")
    });
    this.x -= this.adj;
    this.y = this.y + this.opp + this.hyp;
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 128 /*0x80*/, (byte) 128 /*0x80*/)),
      RowIndex = 3,
      ColumnIndex = 1,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Teal")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 2.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 153, (byte) 153)),
      RowIndex = 3,
      ColumnIndex = 2,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Teal")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 4.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 204, (byte) 204)),
      RowIndex = 3,
      ColumnIndex = 3,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Teal")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 6.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 204, byte.MaxValue)),
      RowIndex = 3,
      ColumnIndex = 4,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Turquoise")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 8.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 153, byte.MaxValue)),
      RowIndex = 3,
      ColumnIndex = 5,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 10.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 102, byte.MaxValue)),
      RowIndex = 3,
      ColumnIndex = 6,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 12.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 102, byte.MaxValue)),
      RowIndex = 3,
      ColumnIndex = 7,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 14.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 51, (byte) 204)),
      RowIndex = 3,
      ColumnIndex = 8,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 16.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, (byte) 102, (byte) 153)),
      RowIndex = 3,
      ColumnIndex = 9,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Indigo")
    });
    this.x -= this.adj;
    this.y = this.y + this.opp + this.hyp;
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 153, (byte) 102)),
      RowIndex = 4,
      ColumnIndex = 1,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 2.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 204, (byte) 153)),
      RowIndex = 4,
      ColumnIndex = 2,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 4.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, byte.MaxValue, (byte) 204)),
      RowIndex = 4,
      ColumnIndex = 3,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Teal")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 6.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, byte.MaxValue, byte.MaxValue)),
      RowIndex = 4,
      ColumnIndex = 4,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Aqua")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 8.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 204, byte.MaxValue)),
      RowIndex = 4,
      ColumnIndex = 5,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Turquoise")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 10.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 153, byte.MaxValue)),
      RowIndex = 4,
      ColumnIndex = 6,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 12.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, (byte) 153, byte.MaxValue)),
      RowIndex = 4,
      ColumnIndex = 7,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 14.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, (byte) 102, byte.MaxValue)),
      RowIndex = 4,
      ColumnIndex = 8,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 16.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, (byte) 0, byte.MaxValue)),
      RowIndex = 4,
      ColumnIndex = 9,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Purple")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 18.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, (byte) 0, (byte) 204)),
      RowIndex = 4,
      ColumnIndex = 10,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Purple")
    });
    this.x -= this.adj;
    this.y = this.y + this.opp + this.hyp;
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 153, (byte) 51)),
      RowIndex = 5,
      ColumnIndex = 1,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 2.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 204, (byte) 102)),
      RowIndex = 5,
      ColumnIndex = 2,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 4.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, byte.MaxValue, (byte) 153)),
      RowIndex = 5,
      ColumnIndex = 3,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 6.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, byte.MaxValue, (byte) 204)),
      RowIndex = 5,
      ColumnIndex = 4,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 8.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, byte.MaxValue, byte.MaxValue)),
      RowIndex = 5,
      ColumnIndex = 5,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Sky")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 10.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, (byte) 204, byte.MaxValue)),
      RowIndex = 5,
      ColumnIndex = 6,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Turquoise")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 12.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, (byte) 204, byte.MaxValue)),
      RowIndex = 5,
      ColumnIndex = 7,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 14.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, (byte) 153, byte.MaxValue)),
      RowIndex = 5,
      ColumnIndex = 8,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 16.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, (byte) 102, byte.MaxValue)),
      RowIndex = 5,
      ColumnIndex = 9,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 18.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, (byte) 51, byte.MaxValue)),
      RowIndex = 5,
      ColumnIndex = 10,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Lavender")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 20.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, (byte) 0, byte.MaxValue)),
      RowIndex = 5,
      ColumnIndex = 11,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Purple")
    });
    this.x -= this.adj;
    this.y = this.y + this.opp + this.hyp;
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 102, (byte) 0)),
      RowIndex = 6,
      ColumnIndex = 1,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 2.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 204, (byte) 0)),
      RowIndex = 6,
      ColumnIndex = 2,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 4.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, byte.MaxValue, (byte) 0)),
      RowIndex = 6,
      ColumnIndex = 3,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 6.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, byte.MaxValue, (byte) 153)),
      RowIndex = 6,
      ColumnIndex = 4,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 8.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, byte.MaxValue, (byte) 204)),
      RowIndex = 6,
      ColumnIndex = 5,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 10.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, byte.MaxValue, byte.MaxValue)),
      RowIndex = 6,
      ColumnIndex = 6,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Sky")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 12.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, (byte) 236, byte.MaxValue)),
      RowIndex = 6,
      ColumnIndex = 7,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 14.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, (byte) 204, byte.MaxValue)),
      RowIndex = 6,
      ColumnIndex = 8,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Blue")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 16.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, (byte) 153, byte.MaxValue)),
      RowIndex = 6,
      ColumnIndex = 9,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Lavender")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 18.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, (byte) 102, byte.MaxValue)),
      RowIndex = 6,
      ColumnIndex = 10,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Lavender")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 20.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, (byte) 0, byte.MaxValue)),
      RowIndex = 6,
      ColumnIndex = 11,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Purple")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 22.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, (byte) 0, (byte) 204)),
      RowIndex = 6,
      ColumnIndex = 12,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Purple")
    });
    this.x -= this.adj;
    this.y = this.y + this.opp + this.hyp;
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 51, (byte) 0)),
      RowIndex = 7,
      ColumnIndex = 1,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 2.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 128 /*0x80*/, (byte) 0)),
      RowIndex = 7,
      ColumnIndex = 2,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 4.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 204, (byte) 51)),
      RowIndex = 7,
      ColumnIndex = 3,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 6.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, byte.MaxValue, (byte) 102)),
      RowIndex = 7,
      ColumnIndex = 4,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 8.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, byte.MaxValue, (byte) 153)),
      RowIndex = 7,
      ColumnIndex = 5,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 10.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, byte.MaxValue, (byte) 204)),
      RowIndex = 7,
      ColumnIndex = 6,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 12.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)),
      RowIndex = 7,
      ColumnIndex = 7,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "White")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 14.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 204, byte.MaxValue)),
      RowIndex = 7,
      ColumnIndex = 8,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Lavender")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 16.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 153, byte.MaxValue)),
      RowIndex = 7,
      ColumnIndex = 9,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Lavender")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 18.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 102, byte.MaxValue)),
      RowIndex = 7,
      ColumnIndex = 10,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Lavender")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 20.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 0, byte.MaxValue)),
      RowIndex = 7,
      ColumnIndex = 11,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Purple")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 22.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, (byte) 0, (byte) 204)),
      RowIndex = 7,
      ColumnIndex = 12,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Purple")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 24.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, (byte) 0, (byte) 102)),
      RowIndex = 7,
      ColumnIndex = 13,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Purple")}"
    });
    this.x += this.adj;
    this.y = this.y + this.opp + this.hyp;
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 102, (byte) 0)),
      RowIndex = 8,
      ColumnIndex = 1,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 2.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 153, (byte) 0)),
      RowIndex = 8,
      ColumnIndex = 2,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 4.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, byte.MaxValue, (byte) 51)),
      RowIndex = 8,
      ColumnIndex = 3,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 6.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, byte.MaxValue, (byte) 102)),
      RowIndex = 8,
      ColumnIndex = 4,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 8.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, byte.MaxValue, (byte) 153)),
      RowIndex = 8,
      ColumnIndex = 5,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 10.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 204)),
      RowIndex = 8,
      ColumnIndex = 6,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Yellow")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 12.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 204, (byte) 204)),
      RowIndex = 8,
      ColumnIndex = 7,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Rose")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 14.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 153, (byte) 204)),
      RowIndex = 8,
      ColumnIndex = 8,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Pink")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 16.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 102, (byte) 204)),
      RowIndex = 8,
      ColumnIndex = 9,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Pink")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 18.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 51, (byte) 204)),
      RowIndex = 8,
      ColumnIndex = 10,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Pink")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 20.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, (byte) 0, (byte) 153)),
      RowIndex = 8,
      ColumnIndex = 11,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Pink")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 22.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 128 /*0x80*/, (byte) 0, (byte) 128 /*0x80*/)),
      RowIndex = 8,
      ColumnIndex = 12,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Purple")}"
    });
    this.x += this.adj;
    this.y = this.y + this.opp + this.hyp;
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 51, (byte) 0)),
      RowIndex = 9,
      ColumnIndex = 1,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Yellow")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 2.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, (byte) 153, (byte) 0)),
      RowIndex = 9,
      ColumnIndex = 2,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 4.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, byte.MaxValue, (byte) 51)),
      RowIndex = 9,
      ColumnIndex = 3,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 6.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, byte.MaxValue, (byte) 102)),
      RowIndex = 9,
      ColumnIndex = 4,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Lime")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 8.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 153)),
      RowIndex = 9,
      ColumnIndex = 5,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Yellow")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 10.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 204, (byte) 153)),
      RowIndex = 9,
      ColumnIndex = 6,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Orange")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 12.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 153, (byte) 153)),
      RowIndex = 9,
      ColumnIndex = 7,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Rose")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 14.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 102, (byte) 153)),
      RowIndex = 9,
      ColumnIndex = 8,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Rose")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 16.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 51, (byte) 153)),
      RowIndex = 9,
      ColumnIndex = 9,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Pink")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 18.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, (byte) 51, (byte) 153)),
      RowIndex = 9,
      ColumnIndex = 10,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Pink")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 20.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, (byte) 0, (byte) 153)),
      RowIndex = 9,
      ColumnIndex = 11,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Purple")}"
    });
    this.x += this.adj;
    this.y = this.y + this.opp + this.hyp;
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, (byte) 102, (byte) 51)),
      RowIndex = 10,
      ColumnIndex = 1,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Brown")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 2.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, (byte) 204, (byte) 0)),
      RowIndex = 10,
      ColumnIndex = 2,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Green")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 4.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, byte.MaxValue, (byte) 51)),
      RowIndex = 10,
      ColumnIndex = 3,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Lime")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 6.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 102)),
      RowIndex = 10,
      ColumnIndex = 4,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Yellow")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 8.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 204, (byte) 102)),
      RowIndex = 10,
      ColumnIndex = 5,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Yellow")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 10.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 153, (byte) 102)),
      RowIndex = 10,
      ColumnIndex = 6,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Orange")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 12.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 124, (byte) 128 /*0x80*/)),
      RowIndex = 10,
      ColumnIndex = 7,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Rose")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 14.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 0, (byte) 102)),
      RowIndex = 10,
      ColumnIndex = 8,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Pink")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 16.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 214, (byte) 0, (byte) 147)),
      RowIndex = 10,
      ColumnIndex = 9,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Pink")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 18.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, (byte) 51, (byte) 102)),
      RowIndex = 10,
      ColumnIndex = 10,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Pink")
    });
    this.x += this.adj;
    this.y = this.y + this.opp + this.hyp;
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 128 /*0x80*/, (byte) 128 /*0x80*/, (byte) 0)),
      RowIndex = 11,
      ColumnIndex = 1,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Yellow")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 2.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, (byte) 204, (byte) 0)),
      RowIndex = 11,
      ColumnIndex = 2,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Yellow")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 4.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 0)),
      RowIndex = 11,
      ColumnIndex = 3,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Yellow")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 6.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 204, (byte) 0)),
      RowIndex = 11,
      ColumnIndex = 4,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gold")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 8.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 153, (byte) 51)),
      RowIndex = 11,
      ColumnIndex = 5,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Orange")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 10.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 102, (byte) 0)),
      RowIndex = 11,
      ColumnIndex = 6,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Orange")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 12.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 80 /*0x50*/, (byte) 80 /*0x50*/)),
      RowIndex = 11,
      ColumnIndex = 7,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Red")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 14.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, (byte) 0, (byte) 102)),
      RowIndex = 11,
      ColumnIndex = 8,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Pink")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 16.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, (byte) 0, (byte) 51)),
      RowIndex = 11,
      ColumnIndex = 9,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Plum")
    });
    this.x += this.adj;
    this.y = this.y + this.opp + this.hyp;
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, (byte) 102, (byte) 51)),
      RowIndex = 12,
      ColumnIndex = 1,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Tan")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 2.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, (byte) 153, (byte) 0)),
      RowIndex = 12,
      ColumnIndex = 2,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gold")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 4.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 153, (byte) 0)),
      RowIndex = 12,
      ColumnIndex = 3,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Orange")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 6.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, (byte) 102, (byte) 0)),
      RowIndex = 12,
      ColumnIndex = 4,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Orange")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 8.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 51, (byte) 0)),
      RowIndex = 12,
      ColumnIndex = 5,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Red")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 10.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 0, (byte) 0)),
      RowIndex = 12,
      ColumnIndex = 6,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Red")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 12.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, (byte) 0, (byte) 0)),
      RowIndex = 12,
      ColumnIndex = 7,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Red")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 14.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, (byte) 0, (byte) 51)),
      RowIndex = 12,
      ColumnIndex = 8,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Red")}"
    });
    this.x += this.adj;
    this.y = this.y + this.opp + this.hyp;
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, (byte) 51, (byte) 0)),
      RowIndex = 13,
      ColumnIndex = 1,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Brown")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 2.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, (byte) 102, (byte) 0)),
      RowIndex = 13,
      ColumnIndex = 2,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Yellow")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 4.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 204, (byte) 51, (byte) 0)),
      RowIndex = 13,
      ColumnIndex = 3,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Red")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 6.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, (byte) 51, (byte) 0)),
      RowIndex = 13,
      ColumnIndex = 4,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Brown")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 8.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 153, (byte) 0, (byte) 0)),
      RowIndex = 13,
      ColumnIndex = 5,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Red")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 10.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 128 /*0x80*/, (byte) 0, (byte) 0)),
      RowIndex = 13,
      ColumnIndex = 6,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Red")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 12.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 165, (byte) 0, (byte) 33)),
      RowIndex = 13,
      ColumnIndex = 7,
      ColorName = $"{SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Red")}"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x - 7.0 * this.adj, this.y + (5.0 * this.opp + this.hyp), 10.0),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)),
      RowIndex = 14,
      ColumnIndex = 1,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "White")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 19.0 * this.adj, this.y + (5.0 * this.opp + this.hyp), 10.0),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0)),
      RowIndex = 15,
      ColumnIndex = 8,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Black")
    });
    this.x -= this.adj;
    this.y += 2.0 * (2.0 * this.opp + this.hyp);
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 248, (byte) 248, (byte) 248)),
      RowIndex = 14,
      ColumnIndex = 2,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "White")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 2.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 221, (byte) 221, (byte) 221)),
      RowIndex = 14,
      ColumnIndex = 3,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray") + "-25%"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 4.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 178, (byte) 178, (byte) 178)),
      RowIndex = 14,
      ColumnIndex = 4,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray") + "-25%"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 10.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 51, (byte) 51)),
      RowIndex = 14,
      ColumnIndex = 6,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray") + "-50%"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 6.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 128 /*0x80*/, (byte) 128 /*0x80*/, (byte) 128 /*0x80*/)),
      RowIndex = 14,
      ColumnIndex = 7,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray") + "-80%"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 8.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 95, (byte) 95, (byte) 95)),
      RowIndex = 14,
      ColumnIndex = 8,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray") + "-80%"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 10.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 51, (byte) 51)),
      RowIndex = 14,
      ColumnIndex = 9,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray") + "-80%"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 12.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 28, (byte) 28, (byte) 28)),
      RowIndex = 14,
      ColumnIndex = 10,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Black")
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 14.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 8, (byte) 8, (byte) 8)),
      RowIndex = 14,
      ColumnIndex = 11,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Black")
    });
    this.x += this.adj;
    this.y = this.y + this.opp + this.hyp;
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 234, (byte) 234, (byte) 234)),
      RowIndex = 15,
      ColumnIndex = 1,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray") + "-25%"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 2.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 192 /*0xC0*/, (byte) 192 /*0xC0*/, (byte) 192 /*0xC0*/)),
      RowIndex = 15,
      ColumnIndex = 2,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray") + "-25%"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 4.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 150, (byte) 150, (byte) 150)),
      RowIndex = 15,
      ColumnIndex = 3,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray") + "-50%"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 6.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 119, (byte) 119, (byte) 119)),
      RowIndex = 15,
      ColumnIndex = 4,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray") + "-50%"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 8.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 77, (byte) 77, (byte) 77)),
      RowIndex = 15,
      ColumnIndex = 5,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray") + "-80%"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 10.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 41, (byte) 41, (byte) 41)),
      RowIndex = 15,
      ColumnIndex = 6,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Gray") + "-80%"
    });
    this.morecolorcollection.Add(new PolygonItem()
    {
      Points = this.CalculatePoints(this.x + 12.0 * this.adj, this.y),
      color = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 17, (byte) 17, (byte) 17)),
      RowIndex = 15,
      ColumnIndex = 7,
      ColorName = SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Black")
    });
    this.Item.ItemsSource = (IEnumerable) this.morecolorcollection;
  }

  private void drawPath1()
  {
    foreach (PolygonItem polygonItem in this.palette.child.morecolorcollection.Where<PolygonItem>((Func<PolygonItem, bool>) (more => more.RowIndex == 14 && more.ColumnIndex == 1)))
    {
      this.palette.child.path1.Stroke = (Brush) new SolidColorBrush(Colors.Black);
      this.palette.child.path1.Fill = (Brush) new SolidColorBrush(Colors.White);
      this.palette.child.path1.Data = (Geometry) this.palette.child.polygonitem.DrawPath(polygonItem.Points);
    }
  }

  private void drawSmallPath1()
  {
    foreach (PolygonItem polygonItem in this.palette.child.morecolorcollection.Where<PolygonItem>((Func<PolygonItem, bool>) (more => more.RowIndex == 7 && more.ColumnIndex == 7)))
    {
      this.palette.child.path1.Stroke = (Brush) new SolidColorBrush(Colors.Black);
      this.palette.child.path1.Fill = (Brush) new SolidColorBrush(Colors.White);
      this.palette.child.path1.Data = (Geometry) this.palette.child.polygonitem.DrawPath(polygonItem.Points);
    }
  }

  private void ChildWindow1KeyDown(object sender, KeyEventArgs e)
  {
    PointCollection pointCollection = new PointCollection();
    PointCollection points1 = new PointCollection();
    this.opp = Math.Sin(Math.PI / 6.0) * this.hyp;
    this.adj = Math.Cos(Math.PI / 6.0) * this.hyp;
    int num = 0;
    int rindex = 0;
    int colindex = 0;
    if (this.palette == null)
      return;
    if (e.Key == Key.Down)
    {
      if (this.palette.child.polygonitem == null)
        return;
      PointCollection points2 = this.palette.child.polygonitem.Points;
      if (this.palette.child.polygonitem.RowIndex + 1 == 7 && this.palette.child.polygonitem.ColumnIndex + 1 == 7)
        this.drawPath1();
      else
        this.palette.child.path1.Data = (Geometry) null;
      this.xp = points2[1].X + this.adj;
      this.yp = points2[1].Y + this.opp + this.hyp;
      points1 = this.CalculatePoints(this.xp, this.yp);
      num = this.GetPolygonItem(points1[1].X, points1[1].Y, out this.polygontemp);
      if (num == 1)
      {
        this.palette.child.polygonitem = this.polygontemp;
        this.palette.child.path.Stroke = (Brush) new SolidColorBrush(Colors.Black);
        this.palette.child.path.Fill = (Brush) new SolidColorBrush(Colors.White);
        this.palette.child.path.Data = (Geometry) this.palette.child.polygonitem.DrawPath(points1);
        this.New.Background = this.palette.child.polygonitem.color;
      }
      else
      {
        rindex = this.palette.child.polygonitem.RowIndex;
        colindex = this.palette.child.polygonitem.ColumnIndex;
        switch (rindex)
        {
          case 7:
            colindex = 12;
            break;
          case 8:
            colindex = 11;
            break;
          case 9:
            colindex = 10;
            break;
          case 10:
            colindex = 9;
            break;
          case 11:
            colindex = 8;
            break;
          case 12:
            colindex = 7;
            break;
          case 13:
            rindex = 12;
            ++colindex;
            break;
        }
        foreach (PolygonItem polygonItem in this.palette.child.morecolorcollection.Where<PolygonItem>((Func<PolygonItem, bool>) (more => more.RowIndex == rindex + 1 && more.ColumnIndex == colindex)))
        {
          this.palette.child.polygonitem = polygonItem;
          this.xp = polygonItem.Points[1].X;
          this.yp = polygonItem.Points[1].Y;
          points1 = this.CalculatePoints(this.xp, this.yp);
          this.palette.child.path.Stroke = (Brush) new SolidColorBrush(Colors.Black);
          this.palette.child.path.Fill = (Brush) new SolidColorBrush(Colors.White);
          this.palette.child.path.Data = (Geometry) this.palette.child.polygonitem.DrawPath(points1);
          this.New.Background = this.palette.child.polygonitem.color;
        }
      }
    }
    if (e.Key == Key.Up)
    {
      if (this.palette.child.polygonitem == null)
        return;
      PointCollection points3 = this.palette.child.polygonitem.Points;
      if (this.palette.child.polygonitem.RowIndex - 1 == 7 && this.palette.child.polygonitem.ColumnIndex == 7)
        this.drawPath1();
      else
        this.palette.child.path1.Data = (Geometry) null;
      this.xp = points3[1].X - this.adj;
      this.yp = points3[1].Y - this.opp - this.hyp;
      points1 = this.CalculatePoints(this.xp, this.yp);
      num = this.GetPolygonItem(points1[1].X, points1[1].Y, out this.polygontemp);
      if (num == 1)
      {
        this.palette.child.polygonitem = this.polygontemp;
        this.palette.child.path.Stroke = (Brush) new SolidColorBrush(Colors.Black);
        this.palette.child.path.Fill = (Brush) new SolidColorBrush(Colors.White);
        this.palette.child.path.Data = (Geometry) this.palette.child.polygonitem.DrawPath(points1);
        this.New.Background = this.palette.child.polygonitem.color;
      }
      else
      {
        rindex = this.palette.child.polygonitem.RowIndex;
        colindex = this.palette.child.polygonitem.ColumnIndex;
        switch (rindex)
        {
          case 1:
            rindex = 2;
            --colindex;
            break;
          case 15:
            rindex = 14;
            colindex = 7;
            break;
          default:
            colindex = 1;
            break;
        }
        foreach (PolygonItem polygonItem in this.palette.child.morecolorcollection.Where<PolygonItem>((Func<PolygonItem, bool>) (more => more.RowIndex == rindex - 1 && more.ColumnIndex == colindex)))
        {
          this.palette.child.polygonitem = polygonItem;
          this.xp = polygonItem.Points[1].X;
          this.yp = polygonItem.Points[1].Y;
          points1 = this.CalculatePoints(this.xp, this.yp);
          this.palette.child.path.Stroke = (Brush) new SolidColorBrush(Colors.Black);
          this.palette.child.path.Fill = (Brush) new SolidColorBrush(Colors.White);
          this.palette.child.path.Data = (Geometry) this.palette.child.polygonitem.DrawPath(points1);
          this.New.Background = this.palette.child.polygonitem.color;
        }
      }
    }
    if (e.Key == Key.Left)
    {
      if (this.palette.child.polygonitem == null)
        return;
      PointCollection points4 = this.palette.child.polygonitem.Points;
      if (this.palette.child.polygonitem.RowIndex == 7 && this.palette.child.polygonitem.ColumnIndex - 1 == 7)
        this.drawPath1();
      else
        this.palette.child.path1.Data = (Geometry) null;
      if (this.palette.child.polygonitem.RowIndex == 14 && this.palette.child.polygonitem.ColumnIndex - 1 == 1)
      {
        using (IEnumerator<PolygonItem> enumerator = this.palette.child.morecolorcollection.Where<PolygonItem>((Func<PolygonItem, bool>) (more => more.RowIndex == 14 && more.ColumnIndex == 1)).GetEnumerator())
        {
          if (enumerator.MoveNext())
          {
            this.polygontemp = enumerator.Current;
            num = 1;
            points1 = this.polygontemp.Points;
          }
        }
        this.drawSmallPath1();
      }
      else
      {
        this.xp = points4[1].X - 2.0 * this.adj;
        this.yp = points4[1].Y;
        points1 = this.CalculatePoints(this.xp, this.yp);
        num = this.GetPolygonItem(points1[1].X, points1[1].Y, out this.polygontemp);
      }
      if (num == 1)
      {
        this.palette.child.polygonitem = this.polygontemp;
        this.palette.child.path.Stroke = (Brush) new SolidColorBrush(Colors.Black);
        this.palette.child.path.Fill = (Brush) new SolidColorBrush(Colors.White);
        this.palette.child.path.Data = (Geometry) this.palette.child.polygonitem.DrawPath(points1);
        this.New.Background = this.palette.child.polygonitem.color;
      }
      else
      {
        rindex = this.palette.child.polygonitem.RowIndex;
        colindex = this.palette.child.polygonitem.ColumnIndex;
        switch (rindex)
        {
          case 1:
            colindex = 0;
            break;
          case 2:
            colindex = 7;
            break;
          case 3:
            colindex = 8;
            break;
          case 4:
            colindex = 9;
            break;
          case 5:
            colindex = 10;
            break;
          case 6:
            colindex = 11;
            break;
          case 7:
            colindex = 12;
            break;
          case 8:
            colindex = 13;
            break;
          case 9:
            colindex = 12;
            break;
          case 10:
            colindex = 11;
            break;
          case 11:
            colindex = 10;
            break;
          case 12:
            colindex = 9;
            break;
          case 13:
            colindex = 8;
            break;
          case 14:
            colindex = 7;
            break;
          case 15:
            if (colindex == 1)
            {
              colindex = 11;
              break;
            }
            rindex = 16 /*0x10*/;
            colindex = 7;
            break;
        }
        foreach (PolygonItem polygonItem in this.palette.child.morecolorcollection.Where<PolygonItem>((Func<PolygonItem, bool>) (more => more.RowIndex == rindex - 1 && more.ColumnIndex == colindex)))
        {
          this.palette.child.polygonitem = polygonItem;
          this.xp = polygonItem.Points[1].X;
          this.yp = polygonItem.Points[1].Y;
          points1 = this.CalculatePoints(this.xp, this.yp);
          this.palette.child.path.Stroke = (Brush) new SolidColorBrush(Colors.Black);
          this.palette.child.path.Fill = (Brush) new SolidColorBrush(Colors.White);
          this.palette.child.path.Data = (Geometry) this.palette.child.polygonitem.DrawPath(points1);
          this.New.Background = this.palette.child.polygonitem.color;
        }
      }
    }
    if (e.Key == Key.Right)
    {
      if (this.palette.child.polygonitem == null)
        return;
      PointCollection points5 = this.palette.child.polygonitem.Points;
      if (this.palette.child.polygonitem.RowIndex == 7 && this.palette.child.polygonitem.ColumnIndex + 1 == 7)
        this.drawPath1();
      else if (this.palette.child.polygonitem.RowIndex + 1 == 14 && this.palette.child.polygonitem.ColumnIndex - 6 == 1)
        this.drawSmallPath1();
      else
        this.palette.child.path1.Data = (Geometry) null;
      if (this.palette.child.polygonitem.RowIndex == 14 && this.palette.child.polygonitem.ColumnIndex == 1)
      {
        this.xp = points5[1].X + 7.0 * this.adj - this.adj;
        this.yp = points5[1].Y - (5.0 * this.opp + this.hyp) + 2.0 * (2.0 * this.opp + this.hyp);
        points1 = this.CalculatePoints(this.xp, this.yp);
        num = this.GetPolygonItem(points1[1].X, points1[1].Y, out this.polygontemp);
        this.palette.child.path1.Data = (Geometry) null;
      }
      else if (this.palette.child.polygonitem.RowIndex == 15 && this.palette.child.polygonitem.ColumnIndex + 1 == 8)
      {
        using (IEnumerator<PolygonItem> enumerator = this.palette.child.morecolorcollection.Where<PolygonItem>((Func<PolygonItem, bool>) (more => more.RowIndex == 15 && more.ColumnIndex == 8)).GetEnumerator())
        {
          if (enumerator.MoveNext())
          {
            this.polygontemp = enumerator.Current;
            num = 1;
            points1 = this.polygontemp.Points;
          }
        }
        this.palette.child.path1.Data = (Geometry) null;
      }
      else
      {
        this.xp = points5[1].X + 2.0 * this.adj;
        this.yp = points5[1].Y;
        points1 = this.CalculatePoints(this.xp, this.yp);
        num = this.GetPolygonItem(points1[1].X, points1[1].Y, out this.polygontemp);
      }
      if (num == 1)
      {
        this.palette.child.polygonitem = this.polygontemp;
        this.palette.child.path.Stroke = (Brush) new SolidColorBrush(Colors.Black);
        this.palette.child.path.Fill = (Brush) new SolidColorBrush(Colors.White);
        this.palette.child.path.Data = (Geometry) this.palette.child.polygonitem.DrawPath(points1);
        this.New.Background = this.palette.child.polygonitem.color;
      }
      else
      {
        rindex = this.palette.child.polygonitem.RowIndex;
        colindex = this.palette.child.polygonitem.ColumnIndex;
        foreach (PolygonItem polygonItem in this.palette.child.morecolorcollection.Where<PolygonItem>((Func<PolygonItem, bool>) (more => more.RowIndex == rindex + 1 && more.ColumnIndex == 1)))
        {
          this.palette.child.polygonitem = polygonItem;
          this.xp = polygonItem.Points[1].X;
          this.yp = polygonItem.Points[1].Y;
          PointCollection points6 = polygonItem.RowIndex != 14 || polygonItem.ColumnIndex != 1 ? this.CalculatePoints(this.xp, this.yp) : this.CalculatePoints(this.xp, this.yp, 20.0);
          this.palette.child.path.Stroke = (Brush) new SolidColorBrush(Colors.Black);
          this.palette.child.path.Fill = (Brush) new SolidColorBrush(Colors.White);
          this.palette.child.path.Data = (Geometry) this.palette.child.polygonitem.DrawPath(points6);
          this.New.Background = this.palette.child.polygonitem.color;
        }
      }
    }
    if (e.Key == Key.Tab && (this.tab.SelectedItem as TabItem).Header.ToString() == "Standard" && this.palette.child.polygonitem == null)
    {
      this.palette.child.path.Data = (Geometry) null;
      this.palette.child.polygonitem = this.palette.child.morecolorcollection[0];
      this.palette.child.path.Stroke = (Brush) new SolidColorBrush(Colors.Black);
      this.palette.child.path.Fill = (Brush) new SolidColorBrush(Colors.White);
      this.palette.child.path.Data = (Geometry) this.palette.child.polygonitem.DrawPath(this.palette.child.polygonitem.Points);
      this.New.Background = this.palette.child.polygonitem.color;
    }
    if (e.Key == Key.Return)
      this.SelectAndClose();
    if (e.Key != Key.Escape)
      return;
    this.close();
  }

  private int GetPolygonItem(double x, double y, out PolygonItem polygon)
  {
    int num = 0;
    PolygonItem polygonItem1 = (PolygonItem) null;
    foreach (PolygonItem polygonItem2 in (Collection<PolygonItem>) this.palette.child.morecolorcollection)
    {
      if (Math.Floor(polygonItem2.Points[1].Y) == Math.Floor(y) && Math.Floor(polygonItem2.Points[1].X) == Math.Floor(x))
      {
        polygonItem1 = polygonItem2;
        num = 1;
      }
    }
    if (num == 1)
    {
      polygon = polygonItem1;
      return 1;
    }
    polygon = (PolygonItem) null;
    return 0;
  }

  private void OKButton_Click(object sender, RoutedEventArgs e) => this.SelectAndClose();

  private void SelectAndClose()
  {
    if (this.palette.SelectedItem != null)
      this.palette.SelectedItem.IsSelected = false;
    if (this.tab.SelectedIndex == 0)
    {
      if (this.color != null)
      {
        this.palette.IsSelected = false;
        if (this.palette.child.polygonitem != null)
        {
          this.palette.ColorName = this.polygonitem.ColorName;
          this.palette.Color = ((SolidColorBrush) this.palette.child.polygonitem.color).Color;
          this.Current.Background = this.palette.child.polygonitem.color;
          this.palette.SelectedMoreColor = this.palette.child.polygonitem;
        }
      }
    }
    else
    {
      this.palette.IsSelected = false;
      this.palette.SelectedMoreColor = (PolygonItem) null;
      this.polygonitem = (PolygonItem) null;
      this.palette.ColorName = "Unknown";
    }
    this.Current.Background = this.asd.Brush;
    this.palette.Color = ((SolidColorBrush) this.asd.Brush).Color;
    this.palette.RaiseCommand();
    this.palette.IsChecked = false;
    if (this.palette.RecentlyUsedCollection != null && this.palette.RecentlyUsedCollection.Count > 0)
    {
      this.palette.RecentlyUsedCollection[this.palette.RecentlyUsedCollection.Count - 1].IsSelected = true;
      this.palette.SelectedItem = this.palette.RecentlyUsedCollection[this.palette.RecentlyUsedCollection.Count - 1];
    }
    this.Close();
  }

  private void close()
  {
    this.palette.IsChecked = false;
    this.Close();
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e) => this.close();

  public PointCollection CalculatePoints(double x, double y, double side)
  {
    double num1 = side + this.hyp;
    double num2 = Math.Sin(Math.PI / 6.0) * num1;
    double num3 = Math.Cos(Math.PI / 6.0) * num1;
    return new PointCollection()
    {
      new Point(x - num3, y + num2),
      new Point(x, y),
      new Point(x + num3, y + num2),
      new Point(x + num3, y + num2 + num1),
      new Point(x, y + 2.0 * num2 + num1),
      new Point(x - num3, y + num2 + num1)
    };
  }

  public PointCollection CalculatePoints(double x, double y)
  {
    double num1 = Math.Sin(Math.PI / 6.0) * this.hyp;
    double num2 = Math.Cos(Math.PI / 6.0) * this.hyp;
    return new PointCollection()
    {
      new Point(x - num2, y + num1),
      new Point(x, y),
      new Point(x + num2, y + num1),
      new Point(x + num2, y + num1 + this.hyp),
      new Point(x, y + 2.0 * num1 + this.hyp),
      new Point(x - num2, y + num1 + this.hyp)
    };
  }

  private void asdSelectedBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    this.New.Background = this.asd.Brush;
  }

  private void RadioButtonClick(object sender, RoutedEventArgs e)
  {
    this.asd.VisualizationStyle = ColorSelectionMode.RGB;
  }

  private void HSVRadioButtonClick(object sender, RoutedEventArgs e)
  {
    this.asd.VisualizationStyle = ColorSelectionMode.HSV;
  }

  private void tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    int num = 0;
    if ((sender as TabControl).SelectedIndex != 0 || this.palette == null || this.palette.child == null)
      return;
    this.palette.child.path.Data = (Geometry) null;
    this.palette.child.path1.Data = (Geometry) null;
    if (this.palette.child.morecolorcollection == null)
      return;
    foreach (PolygonItem polygonItem in this.palette.child.morecolorcollection.Where<PolygonItem>((Func<PolygonItem, bool>) (more => ((SolidColorBrush) more.color).Color.Equals(((SolidColorBrush) this.New.Background).Color))))
    {
      if (num == 0)
      {
        this.palette.child.path.Stroke = (Brush) new SolidColorBrush(Colors.Black);
        this.palette.child.path.Fill = (Brush) new SolidColorBrush(Colors.White);
        this.palette.child.path.Data = (Geometry) polygonItem.DrawPath(polygonItem.Points);
        num = 1;
      }
      else
      {
        this.palette.child.path1.Stroke = (Brush) new SolidColorBrush(Colors.Black);
        this.palette.child.path1.Fill = (Brush) new SolidColorBrush(Colors.White);
        this.palette.child.path1.Data = (Geometry) polygonItem.DrawPath(polygonItem.Points);
      }
    }
    if (this.palette.child.polygonitem != null)
      return;
    ((sender as TabControl).SelectedItem as TabItem).Focus();
  }

  private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
  {
    if ((sender as Thumb).Tag.ToString().Equals("right") && ((sender as Thumb).Parent as Grid).Width + e.HorizontalChange > 0.0)
      ((sender as Thumb).Parent as Grid).Width += e.HorizontalChange;
    if ((sender as Thumb).Tag.ToString().Equals("left") && ((sender as Thumb).Parent as Grid).Width + e.HorizontalChange * -1.0 > 0.0)
    {
      ((sender as Thumb).Parent as Grid).Width += e.HorizontalChange * -1.0;
      (sender as Thumb).SetValue(Canvas.LeftProperty, (object) -20.0);
      this.Margin = new Thickness(-10.0, 0.0, 10.0, 0.0);
    }
    if (!(sender as Thumb).Tag.ToString().Equals("bottom") || (((sender as Thumb).Parent as Grid).Parent as Grid).Height + e.VerticalChange <= 0.0)
      return;
    (((sender as Thumb).Parent as Grid).Parent as Grid).Height += e.VerticalChange;
  }

  private void tab_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    double actualWidth = (sender as TabControl).ActualWidth;
    double actualHeight = (sender as TabControl).ActualHeight;
    if (this.WindowGrid.Width <= 424.0)
      this.WindowGrid.Width = 424.0;
    else if (175.0 / 212.0 * this.WindowGrid.Width <= this.WindowGrid.Height)
    {
      this.hyp = 1.0 / 32.0 * (sender as TabControl).ActualWidth;
      this.xprev = 9.0 / 32.0 * (sender as TabControl).ActualWidth;
      this.x = this.xprev;
      if (this.yprev != 0.0)
        this.y = this.yprev;
      this.LoadPolygonItem();
      IEnumerable<PolygonItem> polygonItems = this.palette.child.morecolorcollection.Where<PolygonItem>((Func<PolygonItem, bool>) (more => ((SolidColorBrush) more.color).Color.Equals(((SolidColorBrush) this.palette.child.New.Background).Color)));
      int num = 0;
      foreach (PolygonItem polygonItem in polygonItems)
      {
        this.palette.SelectedMoreColor = polygonItem;
        this.palette.child.polygonitem = polygonItem;
        if (num == 0)
        {
          this.palette.child.path.Stroke = (Brush) new SolidColorBrush(Colors.Black);
          this.palette.child.path.Fill = (Brush) new SolidColorBrush(Colors.White);
          this.palette.child.path.Data = (Geometry) this.palette.child.polygonitem.DrawPath(this.palette.child.polygonitem.Points);
          num = 1;
        }
        else
        {
          this.palette.child.path1.Stroke = (Brush) new SolidColorBrush(Colors.Black);
          this.palette.child.path1.Fill = (Brush) new SolidColorBrush(Colors.White);
          this.palette.child.path1.Data = (Geometry) this.palette.child.polygonitem.DrawPath(this.palette.child.polygonitem.Points);
        }
        Binding binding = new Binding();
        binding.Source = (object) this.palette.SelectedMoreColor.color;
        binding.Mode = BindingMode.OneWay;
        this.palette.child.Current.SetBinding(Border.BackgroundProperty, (BindingBase) binding);
        this.palette.child.New.SetBinding(Border.BackgroundProperty, (BindingBase) binding);
      }
    }
    if (this.asd.Width >= 300.0)
      this.asd.Width = 15.0 / 16.0 * (sender as TabControl).ActualWidth;
    else
      this.asd.Width = 300.0;
    if (this.asd.Height >= 200.0)
      this.asd.Height = 4.0 / 7.0 * (sender as TabControl).ActualHeight;
    else
      this.asd.Height = 200.0;
    if (this.WindowGrid.Height <= 346.0)
    {
      this.WindowGrid.Height = 346.0;
    }
    else
    {
      if (212.0 / 175.0 * this.WindowGrid.Height > this.WindowGrid.Width)
        return;
      this.hyp = 5.0 / 173.0 * (sender as TabControl).ActualHeight;
      this.yprev = 10.0 / 173.0 * (sender as TabControl).ActualHeight;
      this.y = this.yprev;
      if (this.xprev != 0.0)
        this.x = this.xprev;
      this.LoadPolygonItem();
      IEnumerable<PolygonItem> polygonItems = this.palette.child.morecolorcollection.Where<PolygonItem>((Func<PolygonItem, bool>) (more => ((SolidColorBrush) more.color).Color.Equals(((SolidColorBrush) this.palette.child.New.Background).Color)));
      int num = 0;
      foreach (PolygonItem polygonItem in polygonItems)
      {
        this.palette.SelectedMoreColor = polygonItem;
        this.palette.child.polygonitem = polygonItem;
        if (num == 0)
        {
          this.palette.child.path.Stroke = (Brush) new SolidColorBrush(Colors.Black);
          this.palette.child.path.Fill = (Brush) new SolidColorBrush(Colors.White);
          this.palette.child.path.Data = (Geometry) this.palette.child.polygonitem.DrawPath(this.palette.child.polygonitem.Points);
          num = 1;
        }
        else
        {
          this.palette.child.path1.Stroke = (Brush) new SolidColorBrush(Colors.Black);
          this.palette.child.path1.Fill = (Brush) new SolidColorBrush(Colors.White);
          this.palette.child.path1.Data = (Geometry) this.palette.child.polygonitem.DrawPath(this.palette.child.polygonitem.Points);
        }
        Binding binding = new Binding();
        binding.Source = (object) this.palette.SelectedMoreColor.color;
        binding.Mode = BindingMode.OneWay;
        this.palette.child.Current.SetBinding(Border.BackgroundProperty, (BindingBase) binding);
        this.palette.child.New.SetBinding(Border.BackgroundProperty, (BindingBase) binding);
      }
    }
  }

  private void Thumb_DragDelta_1(object sender, DragDeltaEventArgs e)
  {
    if ((((sender as Thumb).Parent as Grid).Parent as Grid).Width + e.HorizontalChange > 0.0)
      (((sender as Thumb).Parent as Grid).Parent as Grid).Width += e.HorizontalChange;
    if ((((sender as Thumb).Parent as Grid).Parent as Grid).Height + e.VerticalChange <= 0.0)
      return;
    (((sender as Thumb).Parent as Grid).Parent as Grid).Height += e.VerticalChange;
  }

  protected override void OnClosed(EventArgs e)
  {
    if (this.palette.Mode == PickerMode.DropDown)
      this.palette.Focus();
    this.Dispose();
    base.OnClosed(e);
    this.Close();
  }

  internal void Dispose()
  {
    this.KeyDown -= new KeyEventHandler(this.ChildWindow1KeyDown);
    this.Unloaded -= new RoutedEventHandler(this.MoreColorsWindow_Unloaded);
    if (this.morecolorcollection != null)
    {
      for (int index = 0; index < this.morecolorcollection.Count; ++index)
        this.morecolorcollection[index].Dispose();
      this.morecolorcollection.Clear();
      this.morecolorcollection = (ObservableCollection<PolygonItem>) null;
    }
    if (this.palette != null)
    {
      if (this.palette.child != null)
        this.palette.child = (MoreColorsWindow) null;
      this.palette = (ColorPickerPalette) null;
    }
    if (this.polygonitem != null)
    {
      this.polygonitem.Dispose();
      this.polygonitem = (PolygonItem) null;
    }
    if (this.polygontemp != null)
    {
      this.polygontemp.Dispose();
      this.polygontemp = (PolygonItem) null;
    }
    if (this.bind != null)
      this.bind = (Binding) null;
    if (this.color != null)
      this.color = (Brush) null;
    if (this.tab != null)
    {
      this.tab.SizeChanged -= new SizeChangedEventHandler(this.tab_SizeChanged);
      this.tab.SelectionChanged -= new SelectionChangedEventHandler(this.tab_SelectionChanged);
      if (this.tab.Items != null && this.tab.Items.Count > 0)
      {
        foreach (Control control in (IEnumerable) this.tab.Items)
          control.Template = (ControlTemplate) null;
      }
      this.tab.ItemsSource = (IEnumerable) null;
      if (this.tab.ItemsSource == null)
        this.tab.Items.Clear();
      this.tab = (TabControl) null;
    }
    if (this.asd != null)
    {
      BindingOperations.ClearAllBindings((DependencyObject) this.asd);
      this.asd.Template = (ControlTemplate) null;
      this.asd.Dispose();
      this.asd = (ColorEdit) null;
    }
    if (this.custom != null)
      this.custom = (TabItem) null;
    if (this.custompanel != null)
      this.custompanel = (StackPanel) null;
    if (this.WindowGrid != null)
      this.WindowGrid = (Grid) null;
    if (this.standard != null)
      this.standard = (TabItem) null;
    if (this.standardPanel != null)
      this.standardPanel = (StackPanel) null;
    if (this.Current != null)
      this.Current = (Border) null;
    if (this.CancelButton != null)
    {
      this.CancelButton.Click -= new RoutedEventHandler(this.CancelButton_Click);
      this.CancelButton = (Button) null;
    }
    if (this.OKButton != null)
    {
      this.OKButton.Click -= new RoutedEventHandler(this.OKButton_Click);
      this.OKButton = (Button) null;
    }
    if (this.LayoutRoot != null)
      this.LayoutRoot = (Grid) null;
    if (this.New != null)
      this.New = (Border) null;
    if (this.path != null)
      this.path = (Path) null;
    if (this.path1 != null)
      this.path1 = (Path) null;
    if (this.palette != null)
      this.palette = (ColorPickerPalette) null;
    if (this.Item != null)
    {
      if (this.Item.ItemsSource != null)
        this.Item.ItemsSource = (IEnumerable) null;
      if (this.Item.ItemsSource == null)
        this.Item.Items.Clear();
      this.Item = (ItemsControl) null;
    }
    if (this.thumb1 != null)
    {
      this.thumb1.DragDelta -= new DragDeltaEventHandler(this.Thumb_DragDelta);
      this.thumb1 = (Thumb) null;
    }
    if (this.thumb2 != null)
    {
      this.thumb2.DragDelta -= new DragDeltaEventHandler(this.Thumb_DragDelta);
      this.thumb2 = (Thumb) null;
    }
    if (this.thumb3 != null)
    {
      this.thumb3.DragDelta -= new DragDeltaEventHandler(this.Thumb_DragDelta_1);
      this.thumb3 = (Thumb) null;
    }
    if (this.Resources == null)
      return;
    this.Resources.Clear();
    this.Resources = (ResourceDictionary) null;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/Syncfusion.Shared.Wpf;component/controls/colorpickerpalette/themes/morecolorwindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.WindowGrid = (Grid) target;
        break;
      case 2:
        this.LayoutRoot = (Grid) target;
        break;
      case 3:
        this.OKButton = (Button) target;
        this.OKButton.Click += new RoutedEventHandler(this.OKButton_Click);
        break;
      case 4:
        this.CancelButton = (Button) target;
        this.CancelButton.Click += new RoutedEventHandler(this.CancelButton_Click);
        break;
      case 5:
        this.New = (Border) target;
        break;
      case 6:
        this.Current = (Border) target;
        break;
      case 7:
        this.tab = (TabControl) target;
        this.tab.SizeChanged += new SizeChangedEventHandler(this.tab_SizeChanged);
        this.tab.SelectionChanged += new SelectionChangedEventHandler(this.tab_SelectionChanged);
        break;
      case 8:
        this.standard = (TabItem) target;
        break;
      case 9:
        this.standardPanel = (StackPanel) target;
        break;
      case 10:
        this.Item = (ItemsControl) target;
        break;
      case 11:
        this.path = (Path) target;
        break;
      case 12:
        this.path1 = (Path) target;
        break;
      case 13:
        this.custom = (TabItem) target;
        break;
      case 14:
        this.custompanel = (StackPanel) target;
        break;
      case 15:
        this.asd = (ColorEdit) target;
        break;
      case 16 /*0x10*/:
        this.thumb1 = (Thumb) target;
        this.thumb1.DragDelta += new DragDeltaEventHandler(this.Thumb_DragDelta);
        break;
      case 17:
        this.thumb2 = (Thumb) target;
        this.thumb2.DragDelta += new DragDeltaEventHandler(this.Thumb_DragDelta);
        break;
      case 18:
        this.thumb3 = (Thumb) target;
        this.thumb3.DragDelta += new DragDeltaEventHandler(this.Thumb_DragDelta_1);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
