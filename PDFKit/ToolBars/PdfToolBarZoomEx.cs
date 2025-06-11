// Decompiled with JetBrains decompiler
// Type: PDFKit.ToolBars.PdfToolBarZoomEx
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace PDFKit.ToolBars;

public class PdfToolBarZoomEx : PdfToolBar
{
  private int _trackBarWidth = 104;
  private int _trackBarHeight = 32 /*0x20*/;
  private int _currentZoomLevel = 0;
  private double[] _zoomLevel = new double[15]
  {
    8.3299999237060547,
    12.5,
    25.0,
    33.330001831054688,
    50.0,
    66.669998168945313,
    75.0,
    100.0,
    125.0,
    150.0,
    200.0,
    300.0,
    400.0,
    600.0,
    800.0
  };

  public double[] ZoomLevel
  {
    get => this._zoomLevel;
    set
    {
      if (value == null || value.Length == 0)
        return;
      this._zoomLevel = value;
      this.Items.Clear();
      this.InitializeButtons();
      this.UpdateButtons();
    }
  }

  public double Zoom
  {
    get
    {
      if (this.PdfViewer == null || this.PdfViewer.Document == null || this.PdfViewer.Document.Pages.Count == 0)
        return 0.0;
      PdfPage currentPage = this.PdfViewer.Document.Pages.CurrentPage;
      return this.PdfViewer.SizeMode == SizeModes.Zoom ? (double) this.PdfViewer.Zoom * 100.0 : this.PdfViewer.CalcActualRect(this.PdfViewer.CurrentIndex).Width * 100.0 / ((double) currentPage.Width * this.PdfViewer._actualSizeFactor());
    }
  }

  private Button CreateZoomDropDown()
  {
    Button button = this.CreateButton("btnDropDownZoomEx", PDFKit.Properties.Resources.btnZoomComboText, PDFKit.Properties.Resources.btnZoomComboToolTipText, (Uri) null, new RoutedEventHandler(this.ZoomLevel_ButtonClick), 16 /*0x10*/, 16 /*0x10*/, PdfToolBar.ImageTextType.TextOnly);
    button.MinWidth = 80.0;
    double x = 0.0;
    double y = 0.0;
    PathFigure pathFigure = new PathFigure();
    pathFigure.StartPoint = new Point(x, y);
    pathFigure.Segments.Add((PathSegment) new LineSegment(new Point(x + 10.43, y), true));
    pathFigure.Segments.Add((PathSegment) new LineSegment(new Point(x + 5.215, y + 6.099), true));
    pathFigure.Segments.Add((PathSegment) new LineSegment(new Point(x, y), true));
    pathFigure.IsClosed = true;
    Path element = new Path();
    element.Margin = new Thickness(4.0);
    element.VerticalAlignment = VerticalAlignment.Center;
    element.HorizontalAlignment = HorizontalAlignment.Right;
    element.Width = 6.0;
    element.Fill = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 82, (byte) 125, (byte) 181));
    element.Stretch = Stretch.Uniform;
    element.Data = (Geometry) new PathGeometry();
    (element.Data as PathGeometry).Figures.Add(pathFigure);
    (button.Content as StackPanel).Orientation = Orientation.Horizontal;
    (button.Content as StackPanel).Children.Add((UIElement) element);
    button.ContextMenu = new ContextMenu();
    button.ContextMenu.Opened += new RoutedEventHandler(this.ZoomLevel_DropDownOpening);
    for (int index = this.ZoomLevel.Length - 1; index >= 0; --index)
    {
      MenuItem newItem = new MenuItem();
      newItem.Header = (object) $"{this.ZoomLevel[index]:0.00}%";
      newItem.Name = "btnZoomLevel_" + this.ZoomLevel[index].ToString().Replace(".", "_").Replace(",", "_");
      newItem.Tag = (object) index;
      newItem.Click += new RoutedEventHandler(this.ZoomLevel_Click);
      button.ContextMenu.Items.Add((object) newItem);
    }
    button.ContextMenu.Items.Add((object) new Separator());
    MenuItem newItem1 = new MenuItem();
    newItem1.Header = (object) PDFKit.Properties.Resources.btnActualSizeText;
    newItem1.Click += new RoutedEventHandler(this.btn_ActualSizeClick);
    newItem1.Name = "btnActualSizeEx";
    MenuItem menuItem1 = newItem1;
    System.Windows.Controls.ToolTip toolTip1 = new System.Windows.Controls.ToolTip();
    toolTip1.Content = (object) PDFKit.Properties.Resources.btnActualSizeToolTipText;
    menuItem1.ToolTip = (object) toolTip1;
    button.ContextMenu.Items.Add((object) newItem1);
    MenuItem newItem2 = new MenuItem();
    newItem2.Header = (object) PDFKit.Properties.Resources.btnFitPageText;
    newItem2.Click += new RoutedEventHandler(this.btn_FitPageClick);
    newItem2.Name = "btnFitPageEx";
    MenuItem menuItem2 = newItem2;
    System.Windows.Controls.ToolTip toolTip2 = new System.Windows.Controls.ToolTip();
    toolTip2.Content = (object) PDFKit.Properties.Resources.btnFitPageToolTipText;
    menuItem2.ToolTip = (object) toolTip2;
    button.ContextMenu.Items.Add((object) newItem2);
    MenuItem newItem3 = new MenuItem();
    newItem3.Header = (object) PDFKit.Properties.Resources.btnFitWidthText;
    newItem3.Click += new RoutedEventHandler(this.btn_FitWidthClick);
    newItem3.Name = "btnFitWidthEx";
    MenuItem menuItem3 = newItem3;
    System.Windows.Controls.ToolTip toolTip3 = new System.Windows.Controls.ToolTip();
    toolTip3.Content = (object) PDFKit.Properties.Resources.btnFitWidthToolTipText;
    menuItem3.ToolTip = (object) toolTip3;
    button.ContextMenu.Items.Add((object) newItem3);
    MenuItem newItem4 = new MenuItem();
    newItem4.Header = (object) PDFKit.Properties.Resources.btnFitHeightText;
    newItem4.Click += new RoutedEventHandler(this.btn_FitHeightClick);
    newItem4.Name = "btnFitHeightEx";
    MenuItem menuItem4 = newItem4;
    System.Windows.Controls.ToolTip toolTip4 = new System.Windows.Controls.ToolTip();
    toolTip4.Content = (object) PDFKit.Properties.Resources.btnFitHeightToolTipText;
    menuItem4.ToolTip = (object) toolTip4;
    button.ContextMenu.Items.Add((object) newItem4);
    return button;
  }

  private Slider CreateTrackBar()
  {
    Slider trackBar = new Slider();
    trackBar.Name = "btnTrackBar";
    trackBar.Width = (double) this._trackBarWidth;
    trackBar.Height = (double) this._trackBarHeight;
    trackBar.TickPlacement = TickPlacement.None;
    trackBar.Maximum = (double) (this.ZoomLevel.Length - 1);
    trackBar.Minimum = 0.0;
    trackBar.LargeChange = 1.0;
    trackBar.SmallChange = 1.0;
    trackBar.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.TrackBar_ValueChanged);
    trackBar.Margin = new Thickness(0.0, 13.0, 0.0, 0.0);
    return trackBar;
  }

  protected override void InitializeButtons()
  {
    this.Items.Add((object) this.CreateZoomDropDown());
    Button button1 = this.CreateButton("btnZoomExOut", PDFKit.Properties.Resources.btnZoomOutText, PDFKit.Properties.Resources.btnZoomOutToolTipText, this.CreateUriToResource("zoomExOut.png"), new RoutedEventHandler(this.btn_ZoomExOutClick), 16 /*0x10*/, 16 /*0x10*/, PdfToolBar.ImageTextType.ImageOnly);
    button1.Padding = new Thickness(0.0);
    this.Items.Add((object) button1);
    this.Items.Add((object) this.CreateTrackBar());
    Button button2 = this.CreateButton("btnZoomExIn", PDFKit.Properties.Resources.btnZoomInText, PDFKit.Properties.Resources.btnZoomInToolTipText, this.CreateUriToResource("zoomExIn.png"), new RoutedEventHandler(this.btn_ZoomExInClick), 16 /*0x10*/, 16 /*0x10*/, PdfToolBar.ImageTextType.ImageOnly);
    button2.Padding = new Thickness(0.0);
    this.Items.Add((object) button2);
  }

  protected override void UpdateButtons()
  {
    if (this.Items[0] is Button button1)
    {
      int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
      button1.IsEnabled = num != 0;
    }
    if (this.Items[2] is Slider slider1)
    {
      int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
      slider1.IsEnabled = num != 0;
    }
    if (this.Items[1] is Button button2)
    {
      int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
      button2.IsEnabled = num != 0;
    }
    if (this.Items[3] is Button button3)
    {
      int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
      button3.IsEnabled = num != 0;
    }
    if (this.PdfViewer == null || this.PdfViewer.Document == null)
      return;
    double zoom = this.Zoom;
    if (this.Items[0] is Button button4)
      ((button4.Content as StackPanel).Children[0] as TextBlock).Text = $"{zoom:.00}%";
    this.CalcCurrentZoomLevel();
    if (!(this.Items[2] is Slider slider2))
      return;
    slider2.ValueChanged -= new RoutedPropertyChangedEventHandler<double>(this.TrackBar_ValueChanged);
    slider2.Value = this.Orientation == Orientation.Vertical ? (double) (this._currentZoomLevel * -1) : (double) this._currentZoomLevel;
    slider2.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.TrackBar_ValueChanged);
  }

  protected override void OnPdfViewerChanging(PdfViewer oldValue, PdfViewer newValue)
  {
    base.OnPdfViewerChanging(oldValue, newValue);
    if (oldValue != null)
      this.UnsubscribePdfViewEvents(oldValue);
    if (newValue == null)
      return;
    this.SubscribePdfViewEvents(newValue);
  }

  protected virtual void OnToolBarOrientationChanged()
  {
    if (this.Orientation == Orientation.Vertical)
    {
      if (this.Items[2] is Slider slider1)
      {
        slider1.Orientation = Orientation.Vertical;
        slider1.Width = (double) this._trackBarHeight;
        slider1.Height = (double) this._trackBarWidth;
        slider1.Minimum = (double) ((this.ZoomLevel.Length - 1) * -1);
        slider1.Maximum = 0.0;
      }
    }
    else if (this.Items[2] is Slider slider2)
    {
      slider2.Orientation = Orientation.Horizontal;
      slider2.Width = (double) this._trackBarWidth;
      slider2.Height = (double) this._trackBarHeight;
      slider2.Maximum = (double) (this.ZoomLevel.Length - 1);
      slider2.Minimum = 0.0;
    }
    this.UpdateButtons();
  }

  protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    base.OnPropertyChanged(e);
    if (!(e.Property.Name == "Orientation"))
      return;
    this.OnToolBarOrientationChanged();
  }

  private void PdfViewer_SomethingChanged(object sender, EventArgs e) => this.UpdateButtons();

  private void ZoomLevel_ButtonClick(object sender, EventArgs e)
  {
    (sender as Button).ContextMenu.IsEnabled = true;
    (sender as Button).ContextMenu.PlacementTarget = (UIElement) (sender as Button);
    (sender as Button).ContextMenu.Placement = PlacementMode.Bottom;
    (sender as Button).ContextMenu.IsOpen = true;
  }

  private void ZoomLevel_DropDownOpening(object sender, EventArgs e)
  {
    if (!(this.Items[0] is Button button))
      return;
    ContextMenu contextMenu = button.ContextMenu;
    MenuItem menuItem1 = contextMenu.Items[contextMenu.Items.Count - 1] as MenuItem;
    MenuItem menuItem2 = contextMenu.Items[contextMenu.Items.Count - 2] as MenuItem;
    MenuItem menuItem3 = contextMenu.Items[contextMenu.Items.Count - 3] as MenuItem;
    MenuItem menuItem4 = contextMenu.Items[contextMenu.Items.Count - 4] as MenuItem;
    if (menuItem1 != null)
      menuItem1.IsChecked = this.PdfViewer.SizeMode == SizeModes.FitToHeight;
    if (menuItem2 != null)
      menuItem2.IsChecked = this.PdfViewer.SizeMode == SizeModes.FitToWidth;
    if (menuItem3 != null)
      menuItem3.IsChecked = this.PdfViewer.SizeMode == SizeModes.FitToSize;
    if (menuItem4 == null)
      return;
    menuItem4.IsChecked = this.PdfViewer.SizeMode == SizeModes.Zoom && (double) this.PdfViewer.Zoom >= 0.99996 && (double) this.PdfViewer.Zoom <= 1.00004;
  }

  private void TrackBar_ValueChanged(object sender, EventArgs e)
  {
    this.OnTrackBarValueChanged(this.Items[2] as Slider);
  }

  private void ZoomLevel_Click(object sender, EventArgs e)
  {
    this.OnZoomLevelClick(sender as Button, this.ZoomLevel[(int) (sender as MenuItem).Tag]);
  }

  private void btn_ZoomExOutClick(object sender, EventArgs e)
  {
    this.OnZoomExOutClick(sender as Button);
  }

  private void btn_ZoomExInClick(object sender, EventArgs e)
  {
    this.OnZoomExInClick(sender as Button);
  }

  private void btn_ActualSizeClick(object sender, EventArgs e)
  {
    this.OnActualSizeClick(sender as MenuItem);
  }

  private void btn_FitPageClick(object sender, EventArgs e)
  {
    this.OnFitPageClick(sender as MenuItem);
  }

  private void btn_FitWidthClick(object sender, EventArgs e)
  {
    this.OnFitWidthClick(sender as MenuItem);
  }

  private void btn_FitHeightClick(object sender, EventArgs e)
  {
    this.OnFitHeightClick(sender as MenuItem);
  }

  protected void SetZoom(int zoomIndex) => this.SetZoom(this.ZoomLevel[zoomIndex] / 100.0);

  protected virtual void SetZoom(double zoom)
  {
    this.UnsubscribePdfViewEvents(this.PdfViewer);
    this.PdfViewer.SizeMode = SizeModes.Zoom;
    this.PdfViewer.Zoom = (float) zoom;
    this.SubscribePdfViewEvents(this.PdfViewer);
    this.CalcCurrentZoomLevel();
  }

  protected void CalcCurrentZoomLevel()
  {
    double zoom = this.Zoom;
    double num1 = double.MaxValue;
    this._currentZoomLevel = 0;
    for (int index = 0; index < this.ZoomLevel.Length; ++index)
    {
      double num2 = this.ZoomLevel[index] - zoom;
      if (num2 < 0.0)
        num2 = -num2;
      if (num1 > num2)
      {
        num1 = num2;
        this._currentZoomLevel = index;
      }
    }
  }

  protected virtual void OnTrackBarValueChanged(Slider item)
  {
    int num = (int) item.Value;
    this.SetZoom(this.Orientation == Orientation.Vertical ? num * -1 : num);
    this.UpdateButtons();
  }

  protected virtual void OnZoomLevelClick(Button item, double zoom)
  {
    this.SetZoom(zoom / 100.0);
    this.UpdateButtons();
  }

  protected virtual void OnZoomExInClick(Button item)
  {
    if (this._currentZoomLevel >= this.ZoomLevel.Length - 1)
      return;
    ++this._currentZoomLevel;
    this.SetZoom(this._currentZoomLevel);
    this.UpdateButtons();
  }

  protected virtual void OnZoomExOutClick(Button item)
  {
    if (this._currentZoomLevel <= 0)
      return;
    --this._currentZoomLevel;
    this.SetZoom(this._currentZoomLevel);
    this.UpdateButtons();
  }

  protected virtual void OnActualSizeClick(MenuItem item)
  {
    this.SetZoom(1.0);
    this.UpdateButtons();
  }

  protected virtual void OnFitPageClick(MenuItem item)
  {
    this.PdfViewer.SizeMode = SizeModes.FitToSize;
  }

  protected virtual void OnFitWidthClick(MenuItem item)
  {
    this.PdfViewer.SizeMode = SizeModes.FitToWidth;
  }

  protected virtual void OnFitHeightClick(MenuItem item)
  {
    this.PdfViewer.SizeMode = SizeModes.FitToHeight;
  }

  private void UnsubscribePdfViewEvents(PdfViewer oldValue)
  {
    oldValue.AfterDocumentChanged -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.DocumentLoaded -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.DocumentClosed -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.ViewModeChanged -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.SizeModeChanged -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.ZoomChanged -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.CurrentPageChanged -= new EventHandler(this.PdfViewer_SomethingChanged);
  }

  private void SubscribePdfViewEvents(PdfViewer newValue)
  {
    newValue.AfterDocumentChanged += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.DocumentLoaded += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.DocumentClosed += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.ViewModeChanged += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.SizeModeChanged += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.ZoomChanged += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.CurrentPageChanged += new EventHandler(this.PdfViewer_SomethingChanged);
  }
}
