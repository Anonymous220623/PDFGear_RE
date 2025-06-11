// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.PrintPreviewControl
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

public class PrintPreviewControl : UserControl, IComponentConnector
{
  private const double ZOOM_STARTING_SIZE = 12.5;
  private bool isLoaded;
  private int currentpage = 1;
  private double zoomFactor;
  private PageSetupUI PageSetup;
  private PageInformation pageInfo;
  private PrintPageSettings pageSettings = new PrintPageSettings();
  public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (PrintPreviewControl), new PropertyMetadata((object) null, new PropertyChangedCallback(PrintPreviewControl.OnHeaderTemplatePropertyChanged)));
  public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register(nameof (FooterTemplate), typeof (DataTemplate), typeof (PrintPreviewControl), new PropertyMetadata((object) null, new PropertyChangedCallback(PrintPreviewControl.OnFooterTemplatePropertyChanged)));
  public static readonly DependencyProperty ZoomFactorProperty = DependencyProperty.Register(nameof (ZoomFactor), typeof (double), typeof (PrintPreviewControl), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(PrintPreviewControl.OnZoomFactorPropertyChanged)));
  internal Grid MainGrid;
  internal RowDefinition toolBarGridRow;
  internal RowDefinition parameterGridRow;
  internal RowDefinition viewerContentRow;
  internal StackPanel toolBar;
  internal Button buttonPrint;
  internal Button buttonFirst;
  internal Button buttonPrevious;
  internal TextBox textBoxCurrentPage;
  internal TextBlock labelOf;
  internal TextBlock textBoxTotalPages;
  internal Button buttonNext;
  internal Button buttonLast;
  internal Border comboBoxExternalBorder;
  internal ComboBox comboBoxPageZoom;
  internal Button PageLayout;
  internal Grid gridRenderingRegion;
  internal ScrollViewer scrollViewer;
  internal StackPanel PageView;
  internal ScaleTransform Zoom;
  internal Grid renderArea;
  internal ContentControl renderHeader;
  internal ContentControl renderCanvas;
  internal ContentControl renderFooter;
  private bool _contentLoaded;

  public bool CanMoveNext => this.buttonNext.IsEnabled;

  public bool CanMovePrevious => this.buttonPrevious.IsEnabled;

  public bool CanMoveFirst => this.buttonFirst.IsEnabled;

  public bool CanMoveLast => this.buttonLast.IsEnabled;

  public DataTemplate HeaderTemplate
  {
    set => this.SetValue(PrintPreviewControl.HeaderTemplateProperty, (object) value);
    internal get => (DataTemplate) this.GetValue(PrintPreviewControl.HeaderTemplateProperty);
  }

  public DataTemplate FooterTemplate
  {
    set => this.SetValue(PrintPreviewControl.FooterTemplateProperty, (object) value);
    internal get => (DataTemplate) this.GetValue(PrintPreviewControl.FooterTemplateProperty);
  }

  public int CurrentPage
  {
    get => this.currentpage;
    set => this.currentpage = value;
  }

  public int TotalPage => this.PrintDocument.TotalPages;

  public IPrintDocument PrintDocument { get; set; }

  public double ZoomFactor
  {
    get => this.zoomFactor;
    set
    {
      if (value >= 0.25 && value <= 5.0)
      {
        this.Zoom.ScaleX = value;
        this.Zoom.ScaleY = value;
      }
      else if (value >= 0.25)
      {
        this.Zoom.ScaleX = 0.25;
        this.Zoom.ScaleY = 0.25;
      }
      else
      {
        if (value > 5.0)
          return;
        this.Zoom.ScaleX = 5.0;
        this.Zoom.ScaleY = 5.0;
      }
    }
  }

  private PrintPageSettings PageSetupSettings
  {
    get => this.pageSettings;
    set => this.pageSettings = value;
  }

  public static void OnHeaderTemplatePropertyChanged(
    DependencyObject dependencyObject,
    DependencyPropertyChangedEventArgs e)
  {
    (dependencyObject as PrintPreviewControl).renderHeader.ContentTemplate = e.NewValue as DataTemplate;
  }

  public static void OnFooterTemplatePropertyChanged(
    DependencyObject dependencyObject,
    DependencyPropertyChangedEventArgs e)
  {
    (dependencyObject as PrintPreviewControl).renderFooter.ContentTemplate = e.NewValue as DataTemplate;
  }

  public static void OnZoomFactorPropertyChanged(
    DependencyObject dependencyObject,
    DependencyPropertyChangedEventArgs e)
  {
    (dependencyObject as PrintPreviewControl).ZoomFactor = (double) (int) e.NewValue;
  }

  public PrintPreviewControl()
  {
    this.InitializeComponent();
    this.WireEvents();
    this.UpdateToolbarCulture();
    this.pageInfo = new PageInformation();
  }

  public PrintPreviewControl(IPrintDocument printDocument)
  {
    this.InitializeComponent();
    this.WireEvents();
    this.UpdateToolbarCulture();
    this.pageInfo = new PageInformation();
    this.PrintDocument = printDocument;
  }

  private void WireEvents()
  {
    this.Loaded += new RoutedEventHandler(this.PrintNavigationControl_Loaded);
    this.comboBoxPageZoom.SelectionChanged += new SelectionChangedEventHandler(this.comboBoxPageZoom_SelectionChanged);
  }

  public void UpdateToolbarCulture()
  {
    this.buttonPrevious.ToolTip = (object) SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Previous");
    this.buttonFirst.ToolTip = (object) SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "First");
    this.buttonLast.ToolTip = (object) SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Last");
    this.buttonNext.ToolTip = (object) SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Next");
    this.comboBoxPageZoom.ToolTip = (object) SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Zoom");
    this.buttonPrint.ToolTip = (object) SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "Print");
    this.PageLayout.ToolTip = (object) SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "PrintLayout");
    this.buttonPrevious.SetValue(ToolTipService.ShowOnDisabledProperty, (object) true);
    this.buttonFirst.SetValue(ToolTipService.ShowOnDisabledProperty, (object) true);
    this.buttonLast.SetValue(ToolTipService.ShowOnDisabledProperty, (object) true);
    this.buttonNext.SetValue(ToolTipService.ShowOnDisabledProperty, (object) true);
    this.comboBoxPageZoom.SetValue(ToolTipService.ShowOnDisabledProperty, (object) true);
    this.buttonPrint.SetValue(ToolTipService.ShowOnDisabledProperty, (object) true);
    this.PageLayout.SetValue(ToolTipService.ShowOnDisabledProperty, (object) true);
  }

  private void PrintNavigationControl_Loaded(object sender, RoutedEventArgs e)
  {
    this.isLoaded = true;
    this.SetPageSize();
    this.IntializePrintDialog();
    this.DataContext = (object) this.pageInfo;
  }

  private void IntializePrintDialog()
  {
    this.renderArea.Margin = this.PrintDocument.Margin;
    this.PageView.InvalidateArrange();
    if (this.PrintDocument == null)
      return;
    this.PrintDocument.PrintablePageSize = new Size(this.PrintDocument.PageSize.Width - (this.PrintDocument.Margin.Left + this.PrintDocument.Margin.Right), this.PrintDocument.PageSize.Height - (this.GetHeight(this.HeaderTemplate) + this.GetHeight(this.FooterTemplate)) - (this.PrintDocument.Margin.Top + this.PrintDocument.Margin.Bottom));
    this.PrintDocument.OnSetPageSize();
    this.PageView.Width = this.PrintDocument.PageSize.Width;
    this.PageView.Height = this.PrintDocument.PageSize.Height;
    this.pageInfo.TotalPages = this.PrintDocument.TotalPages;
    if (this.PrintDocument.TotalPages <= 0)
      return;
    this.textBoxTotalPages.Text = this.PrintDocument.TotalPages.ToString();
    if (this.currentpage > this.PrintDocument.TotalPages && this.currentpage < 0)
      this.currentpage = 1;
    this.textBoxCurrentPage.Text = this.currentpage.ToString();
    this.SetPageContent(this.currentpage);
    this.UpdatePageDetails(this.currentpage);
    this.UpdateImageContent();
  }

  private double GetHeight(DataTemplate dataTemplate)
  {
    if (dataTemplate == null)
      return 0.0;
    Border border = new Border();
    ContentControl contentControl = new ContentControl();
    contentControl.ContentTemplate = dataTemplate;
    border.Child = (UIElement) contentControl;
    contentControl.UpdateLayout();
    contentControl.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    border.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    border.Arrange(new Rect(0.0, 0.0, contentControl.ActualWidth, contentControl.ActualHeight));
    return contentControl.ActualHeight;
  }

  private void comboBoxPageZoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.zoomFactor = double.Parse(new Regex("[0-9]*").Match((this.comboBoxPageZoom.SelectedItem as ComboBoxItem).Content.ToString()).Value) / 100.0;
    this.Zoom.ScaleX = this.zoomFactor;
    this.Zoom.ScaleY = this.zoomFactor;
  }

  private void SetPageContent(int pageNo)
  {
    this.pageInfo.PageNumber = pageNo;
    this.renderCanvas.Content = (object) this.PrintDocument.GetPage(pageNo - 1);
  }

  private void UpdatePageDetails(int pageno)
  {
    if (pageno == 1)
    {
      this.buttonFirst.IsEnabled = false;
      this.buttonPrevious.IsEnabled = false;
    }
    else
    {
      this.buttonFirst.IsEnabled = true;
      this.buttonPrevious.IsEnabled = true;
    }
    if (pageno != this.TotalPage)
    {
      this.buttonNext.IsEnabled = true;
      this.buttonLast.IsEnabled = true;
    }
    else
    {
      this.buttonNext.IsEnabled = false;
      this.buttonLast.IsEnabled = false;
    }
    this.textBoxCurrentPage.Text = pageno.ToString();
    if (this.PrintDocument.TotalPages > 1)
      this.textBoxCurrentPage.IsEnabled = true;
    else
      this.textBoxCurrentPage.IsEnabled = false;
  }

  private void SetPageSize()
  {
    Size pageSize = this.PrintDocument.PageSize;
    if (this.PrintDocument.PageSize.Height == 0.0 || this.PrintDocument.PageSize.Width == 0.0)
      this.PrintDocument.PageSize = new Size(816.0, 1056.0);
    Thickness margin = this.PrintDocument.Margin;
  }

  public void UpdatePrintDialog()
  {
    if (!this.isLoaded)
      return;
    this.IntializePrintDialog();
  }

  private void UpdateImageContent()
  {
    if (this.buttonFirst.IsEnabled)
      (this.buttonFirst.Content as Image).Source = (ImageSource) this.Resources[(object) "First_Nav"];
    else
      (this.buttonFirst.Content as Image).Source = (ImageSource) this.Resources[(object) "First_NavDisabled"];
    if (this.buttonPrevious.IsEnabled)
      (this.buttonPrevious.Content as Image).Source = (ImageSource) this.Resources[(object) "Previous_Nav"];
    else
      (this.buttonPrevious.Content as Image).Source = (ImageSource) this.Resources[(object) "Previous_NavDisabled"];
    if (this.buttonNext.IsEnabled)
      (this.buttonNext.Content as Image).Source = (ImageSource) this.Resources[(object) "Next_Nav"];
    else
      (this.buttonNext.Content as Image).Source = (ImageSource) this.Resources[(object) "Next_NavDisabled"];
    if (this.buttonLast.IsEnabled)
      (this.buttonLast.Content as Image).Source = (ImageSource) this.Resources[(object) "Last_Nav"];
    else
      (this.buttonLast.Content as Image).Source = (ImageSource) this.Resources[(object) "Last_NavDisabled"];
  }

  private void buttonFirst_Click(object sender, RoutedEventArgs e)
  {
    this.currentpage = 1;
    this.SetPageContent(this.currentpage);
    this.textBoxCurrentPage.Text = "1";
    this.buttonPrevious.IsEnabled = false;
    this.buttonFirst.IsEnabled = false;
    this.buttonNext.IsEnabled = true;
    this.buttonLast.IsEnabled = true;
    this.UpdateImageContent();
  }

  private void buttonPrevious_Click(object sender, RoutedEventArgs e)
  {
    --this.currentpage;
    this.SetPageContent(this.currentpage);
    this.textBoxCurrentPage.Text = this.currentpage.ToString();
    this.buttonLast.IsEnabled = true;
    this.buttonNext.IsEnabled = true;
    if (this.currentpage == 1)
    {
      this.buttonFirst.IsEnabled = false;
      this.buttonPrevious.IsEnabled = false;
    }
    this.UpdateImageContent();
  }

  private void buttonNext_Click(object sender, RoutedEventArgs e)
  {
    ++this.currentpage;
    this.textBoxCurrentPage.Text = this.currentpage.ToString();
    this.SetPageContent(this.currentpage);
    if (this.currentpage == this.PrintDocument.TotalPages)
    {
      this.buttonNext.IsEnabled = false;
      this.buttonLast.IsEnabled = false;
    }
    this.buttonFirst.IsEnabled = true;
    this.buttonPrevious.IsEnabled = true;
    this.UpdateImageContent();
  }

  private void buttonLast_Click(object sender, RoutedEventArgs e)
  {
    this.currentpage = this.PrintDocument.TotalPages;
    this.SetPageContent(this.currentpage);
    this.textBoxCurrentPage.Text = this.PrintDocument.TotalPages.ToString();
    this.buttonFirst.IsEnabled = true;
    this.buttonPrevious.IsEnabled = true;
    this.buttonNext.IsEnabled = false;
    this.buttonLast.IsEnabled = false;
    this.UpdateImageContent();
  }

  private void buttonprint_Click(object sender, RoutedEventArgs e) => this.Print();

  private StackPanel GetPageVisual(int pageNo)
  {
    PageInformation pageInformation = new PageInformation()
    {
      TotalPages = this.TotalPage,
      PageNumber = pageNo
    };
    StackPanel stackPanel = new StackPanel();
    stackPanel.DataContext = (object) (stackPanel.Orientation = System.Windows.Controls.Orientation.Vertical);
    Grid element1 = new Grid();
    element1.Margin = this.PrintDocument.Margin;
    stackPanel.Width = this.PrintDocument.PageSize.Width;
    stackPanel.Height = this.PrintDocument.PageSize.Height;
    stackPanel.Children.Add((UIElement) element1);
    element1.RowDefinitions.Clear();
    for (int index = 0; index < 3; ++index)
      element1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(0.0, GridUnitType.Auto)
      });
    int num1 = 0;
    ContentControl element2 = new ContentControl();
    ContentControl element3 = element2;
    int num2 = num1;
    int num3 = num2 + 1;
    Grid.SetRow((UIElement) element3, num2);
    element1.Children.Add((UIElement) element2);
    if (this.HeaderTemplate != null)
      element2.ContentTemplate = this.HeaderTemplate;
    ContentControl element4 = new ContentControl();
    ContentControl element5 = element4;
    int num4 = num3;
    int num5 = num4 + 1;
    Grid.SetRow((UIElement) element5, num4);
    element1.Children.Add((UIElement) element4);
    element4.Content = (object) this.PrintDocument.GetPage(pageNo - 1);
    ContentControl element6 = new ContentControl();
    Grid.SetRow((UIElement) element6, num5);
    element1.Children.Add((UIElement) element6);
    if (this.FooterTemplate != null)
      element6.ContentTemplate = this.FooterTemplate;
    this.SetPageContent(pageNo);
    stackPanel.UpdateLayout();
    return this.PageView;
  }

  public void Print()
  {
    PrintDialog printDialog = new PrintDialog();
    bool? nullable = printDialog.ShowDialog();
    if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
      return;
    printDialog.PrintDocument(this.GetFixedDocument().DocumentPaginator, "Print Document");
  }

  private FixedDocument GetFixedDocument()
  {
    this.comboBoxPageZoom.SelectedIndex = 3;
    FixedDocument fixedDocument = new FixedDocument();
    PrintDialog printDialog = new PrintDialog();
    int selectedIndex = this.comboBoxPageZoom.SelectedIndex;
    this.comboBoxPageZoom.SelectedIndex = 3;
    StackPanel stackPanel = new StackPanel();
    for (int pageNo = 1; pageNo <= this.PrintDocument.TotalPages; ++pageNo)
    {
      StackPanel pageVisual = this.GetPageVisual(pageNo);
      Size size = new Size(this.PrintDocument.PageSize.Width, this.PrintDocument.PageSize.Height);
      pageVisual.Measure(size);
      pageVisual.Arrange(new Rect(new Point(0.0, 0.0), size));
      RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int) pageVisual.ActualWidth, (int) pageVisual.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
      renderTargetBitmap.Render((Visual) pageVisual);
      DrawingVisual v = new DrawingVisual();
      DrawingContext drawingContext = v.RenderOpen();
      drawingContext.PushTransform((Transform) new TranslateTransform(0.0, 0.0));
      drawingContext.DrawImage((ImageSource) renderTargetBitmap, new Rect(new Size(this.PageView.Width, this.PageView.Height)));
      drawingContext.Close();
      PageContent newPageContent = new PageContent();
      FixedPage fixedPage = new FixedPage();
      fixedPage.Width = this.PrintDocument.PageSize.Width;
      fixedPage.Height = this.PrintDocument.PageSize.Height;
      PrintVisualContainer element = new PrintVisualContainer();
      element.AddVisual((Visual) v);
      fixedPage.Children.Add((UIElement) element);
      ((IAddChild) newPageContent).AddChild((object) fixedPage);
      fixedDocument.Pages.Add(newPageContent);
    }
    this.comboBoxPageZoom.SelectedIndex = selectedIndex;
    return fixedDocument;
  }

  private void PageLayout_Click(object sender, RoutedEventArgs e)
  {
    PageSetupUI pageSetupUi = new PageSetupUI(this.PageSetupSettings);
    this.PageSetup = pageSetupUi;
    pageSetupUi.IsInternalChange = true;
    pageSetupUi.Owner = Window.GetWindow((DependencyObject) this);
    pageSetupUi.top.Value = new double?(this.PrintDocument.Margin.Top / 96.0);
    pageSetupUi.left.Value = new double?(this.PrintDocument.Margin.Left / 96.0);
    pageSetupUi.right.Value = new double?(this.PrintDocument.Margin.Right / 96.0);
    pageSetupUi.bottom.Value = new double?(this.PrintDocument.Margin.Bottom / 96.0);
    pageSetupUi.pageWidth.Value = new double?(this.PrintDocument.PageSize.Width / 96.0);
    pageSetupUi.pageHeight.Value = new double?(this.PrintDocument.PageSize.Height / 96.0);
    pageSetupUi.IsInternalChange = false;
    bool? nullable = pageSetupUi.ShowDialog();
    if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
      return;
    this.PrintDocument.PageSize = new Size(this.PageSetupSettings.PageWidth, this.PageSetupSettings.PageHeight);
    this.PrintDocument.Margin = this.PageSetupSettings.PageMargin;
    this.UpdatePrintDialog();
  }

  private void Ok_button_Click(object sender, RoutedEventArgs e)
  {
    bool? dialogResult = this.PageSetup.DialogResult;
    if ((!dialogResult.GetValueOrDefault() ? 0 : (dialogResult.HasValue ? 1 : 0)) == 0)
      return;
    this.PrintDocument.PageSize = new Size(this.PageSetup.PageWidth, this.PageSetup.PageHeight);
    this.PrintDocument.Margin = new Thickness(this.PageSetup.LeftMargin, this.PageSetup.TopMargin, this.PageSetup.RightMargin, this.PageSetup.BottomMargin);
    this.UpdatePrintDialog();
    this.PageSetup.DialogResult = new bool?(false);
  }

  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/Syncfusion.Shared.Wpf;component/controls/printpreview/printpreviewcontrol.xaml", UriKind.Relative));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [DebuggerNonUserCode]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.MainGrid = (Grid) target;
        break;
      case 2:
        this.toolBarGridRow = (RowDefinition) target;
        break;
      case 3:
        this.parameterGridRow = (RowDefinition) target;
        break;
      case 4:
        this.viewerContentRow = (RowDefinition) target;
        break;
      case 5:
        this.toolBar = (StackPanel) target;
        break;
      case 6:
        this.buttonPrint = (Button) target;
        this.buttonPrint.Click += new RoutedEventHandler(this.buttonprint_Click);
        break;
      case 7:
        this.buttonFirst = (Button) target;
        this.buttonFirst.Click += new RoutedEventHandler(this.buttonFirst_Click);
        break;
      case 8:
        this.buttonPrevious = (Button) target;
        this.buttonPrevious.Click += new RoutedEventHandler(this.buttonPrevious_Click);
        break;
      case 9:
        this.textBoxCurrentPage = (TextBox) target;
        break;
      case 10:
        this.labelOf = (TextBlock) target;
        break;
      case 11:
        this.textBoxTotalPages = (TextBlock) target;
        break;
      case 12:
        this.buttonNext = (Button) target;
        this.buttonNext.Click += new RoutedEventHandler(this.buttonNext_Click);
        break;
      case 13:
        this.buttonLast = (Button) target;
        this.buttonLast.Click += new RoutedEventHandler(this.buttonLast_Click);
        break;
      case 14:
        this.comboBoxExternalBorder = (Border) target;
        break;
      case 15:
        this.comboBoxPageZoom = (ComboBox) target;
        break;
      case 16 /*0x10*/:
        this.PageLayout = (Button) target;
        this.PageLayout.Click += new RoutedEventHandler(this.PageLayout_Click);
        break;
      case 17:
        this.gridRenderingRegion = (Grid) target;
        break;
      case 18:
        this.scrollViewer = (ScrollViewer) target;
        break;
      case 19:
        this.PageView = (StackPanel) target;
        break;
      case 20:
        this.Zoom = (ScaleTransform) target;
        break;
      case 21:
        this.renderArea = (Grid) target;
        break;
      case 22:
        this.renderHeader = (ContentControl) target;
        break;
      case 23:
        this.renderCanvas = (ContentControl) target;
        break;
      case 24:
        this.renderFooter = (ContentControl) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
