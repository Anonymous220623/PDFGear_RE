// Decompiled with JetBrains decompiler
// Type: PDFLauncher.MainWindow
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using CommomLib.AppTheme;
using CommomLib.Commom;
using CommomLib.Views;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using PDFLauncher.CustomControl;
using PDFLauncher.Models;
using PDFLauncher.Utils;
using PDFLauncher.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

#nullable disable
namespace PDFLauncher;

public partial class MainWindow : Window, IComponentConnector, IStyleConnector
{
  private MainViewModel VM = Ioc.Default.GetRequiredService<MainViewModel>();
  internal MainWindow PDFLauncher;
  internal Button supportfeed;
  internal Button openFileBtn1;
  internal Button NewFileBtn;
  internal Button FineFileBtn;
  internal Button ScanDocument;
  internal TextBlock Version;
  internal Border topDecorateBorder;
  internal Grid openFileGrid;
  internal TextBlock tbVersionNum;
  internal TextBlock tbSupport;
  internal Border OpenFileBorder;
  internal Border OpenFileBgBord;
  internal Image openFileImg;
  internal Button openFileBtn;
  internal TextBlock tbSupportType;
  internal Grid OpenButtonTipsGrid;
  internal Border OpenButtonTips;
  internal Border OpenButtonTipsContent;
  internal ScaleTransform OpenButtonTipsGridTrans;
  internal Button openFileTipsCloseBtn;
  internal ListBox lstBoxMenu;
  internal Grid recommendGrid;
  internal StackPanel stpHotTools;
  internal StackPanel stpHotTools0;
  internal LabelButton lbtnHotToolPDF2Word;
  internal LabelButton lbtnHotToolPDF2Excel;
  internal LabelButton lbtnHotToolPDF2Png;
  internal LabelButton lbtnHotToolPDFCompress;
  internal LabelButton lbtnHotToolMergePDF;
  internal StackPanel stpHotTools1;
  internal LabelButton lbtnHotToolConvertWordToPDF;
  internal LabelButton lbtnHotToolConvertImageToPDF;
  internal LabelButton lbtnHotToolFillForm;
  internal LabelButton lbtnHotToolSplitPDF;
  internal StackPanel stpConvert;
  internal StackPanel stpConvert0;
  internal LabelButton lbtnConvertPDF2Word;
  internal LabelButton lbtnConvertPDF2Excel;
  internal LabelButton lbtnConvertPDF2Png;
  internal LabelButton lbtnConvertPDF2JPG;
  internal StackPanel stpConvert1;
  internal LabelButton lbtnConvertPDF2Txt;
  internal LabelButton lbtnConvertPDF2PPT;
  internal LabelButton lbtnConvertPDF2XML;
  internal LabelButton lbtnConvertPDF2RTF;
  internal StackPanel stpConToPDF;
  internal StackPanel stpConToPDF0;
  internal LabelButton lbtnConvertWordToPDF;
  internal LabelButton lbtnConvertExcelToPDF;
  internal LabelButton lbtnConvertPPT;
  internal LabelButton lbtnConvertImgToPDF;
  internal StackPanel stpConToPDF1;
  internal LabelButton lbtnConvertRTFToPDF;
  internal LabelButton lbtnConvertTXTToPDF;
  internal StackPanel stpMergeSplit;
  internal StackPanel stpMergeSplit0;
  internal LabelButton lbtnMergeSplit_Merge;
  internal LabelButton lbtnMergeSplit_Split;
  internal StackPanel stpAllTools;
  internal LabelButton LbtnAllTool_PDF2Word;
  internal LabelButton LbtnAllTool_PDF2Excel;
  internal LabelButton LbtnAllTool_PDF2PNG;
  internal LabelButton LbtnAllTool_PDF2JPG;
  internal LabelButton LbtnAllTool_PDF2PPT;
  internal LabelButton LbtnAllTool_PDF2Txt;
  internal LabelButton LbtnAllTool_PDF2WebPages;
  internal LabelButton LbtnAllTool_PDF2XML;
  internal LabelButton LbtnAllTool_PDF2RTF;
  internal LabelButton LbtnAllTool_Word2PDF;
  internal LabelButton LbtnAllTool_Excel2PDF;
  internal LabelButton LbtnAllTool_PPT2PDF;
  internal LabelButton LbtnAllTool_Image2PDF;
  internal LabelButton LbtnAllTool_RTF2PDF;
  internal LabelButton LbtnAllTool_TXT2PDF;
  internal LabelButton LbtnAllTool_Compress;
  internal LabelButton LbtnAllTool_Merge;
  internal LabelButton LbtnAllTool_Split;
  internal LabelButton LbtnAllTool_FillForm;
  internal Grid historyGrid;
  internal ListView lsvOpenHistory;
  internal GridView gView;
  internal SwitchButton AllToolsSwitch;
  private bool _contentLoaded;

  public MainWindow()
  {
    this.InitializeComponent();
    WSUtils.LoadWindowInfo("Launcher");
    this.DataContext = (object) Ioc.Default.GetRequiredService<MainViewModel>();
    GAManager.SendEvent("WelcomeWindow", "Show", "Count", 1L);
    this.SizeChanged += new SizeChangedEventHandler(this.MainWindow_SizeChanged);
    this.Version.Text = UtilManager.GetAppVersion();
    if (!ConfigManager.GetWelcomeOpenBtnTipsFlag())
    {
      int btnShowTipsCount = ConfigManager.GetWelcomeOpenBtnShowTipsCount();
      if (btnShowTipsCount <= 2)
      {
        ConfigManager.SetWelcomeOpenBtnShowTipsCount(btnShowTipsCount + 1);
        this.ShowAllTips();
      }
      else
        this.CloseAllTips();
    }
    else
      this.CloseAllTips();
    this.Activated += new EventHandler(this.MainWindow_Activated);
  }

  private void MainWindow_Deactivated(object sender, EventArgs e)
  {
    if (this.OpenButtonTipsGrid.Visibility != Visibility.Visible)
      return;
    ((Storyboard) this.OpenButtonTipsGrid.Resources[(object) "OpenButtonTipsAnimation"]).Stop();
  }

  private void MainWindow_Activated(object sender, EventArgs e)
  {
    if (this.OpenButtonTipsGrid.Visibility != Visibility.Visible)
      return;
    ((Storyboard) this.OpenButtonTipsGrid.Resources[(object) "OpenButtonTipsAnimation"]).Begin();
  }

  private void openFileBtn_Click(object sender, RoutedEventArgs e)
  {
    string file = FileManager.SelectFileForOpen();
    if (string.IsNullOrWhiteSpace(file))
      return;
    DocsPathUtils.WriteFilesPathJson("unknow", (object) file);
    FileManager.OpenOneFile(file);
  }

  private void openFileGrid_MouseEnter(object sender, MouseEventArgs e)
  {
    this.openFileGrid.Background = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFDEDE"));
  }

  private void openFileGrid_MouseLeave(object sender, MouseEventArgs e)
  {
    this.openFileGrid.Background = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFF0F0"));
  }

  private void FastStartClick(object sender, RoutedEventArgs e)
  {
  }

  private void Image_MouseDown(object sender, MouseButtonEventArgs e)
  {
    this.VM.OpenOneFileCMD.Execute((object) null);
  }

  private void fileItemListSelectAll(object sender, RoutedEventArgs e)
  {
    object dataContext = this.lsvOpenHistory.DataContext;
    this.VM.SelectAll();
  }

  private void fileItemListSelectNone(object sender, RoutedEventArgs e) => this.VM.SelectNone();

  private void fileItemChecked(object sender, RoutedEventArgs e)
  {
    this.VM.SelectItemsPropertyChange();
  }

  private void fileItemUnchecked(object sender, RoutedEventArgs e)
  {
    this.VM.SelectItemsPropertyChange();
  }

  private void lstBoxMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.stpHotTools.Visibility = Visibility.Collapsed;
    this.stpConvert.Visibility = Visibility.Collapsed;
    this.stpConToPDF.Visibility = Visibility.Collapsed;
    this.stpMergeSplit.Visibility = Visibility.Collapsed;
    this.stpAllTools.Visibility = Visibility.Collapsed;
    switch (this.lstBoxMenu.SelectedIndex)
    {
      case 0:
        this.stpHotTools.Visibility = Visibility.Visible;
        break;
      case 1:
        this.stpConvert.Visibility = Visibility.Visible;
        break;
      case 2:
        this.stpConToPDF.Visibility = Visibility.Visible;
        break;
      case 3:
        this.stpMergeSplit.Visibility = Visibility.Visible;
        break;
      case 4:
        this.stpAllTools.Visibility = Visibility.Visible;
        break;
    }
  }

  private void Window_Closing(object sender, CancelEventArgs e)
  {
  }

  private void Button_Click(object sender, RoutedEventArgs e)
  {
    this.VM.SelectFilesSupportPropertChange((sender as Button).DataContext as OpenHistoryModel);
  }

  private void pdfto_Click(object sender, RoutedEventArgs e)
  {
  }

  private void singleFileOperateSub_Closed(object sender, EventArgs e)
  {
  }

  private void tbFilePath_MouseDown(object sender, MouseButtonEventArgs e)
  {
    Process.Start("explorer.exe", "/select," + ((sender as TextBlock).DataContext as OpenHistoryModel).FilePath);
  }

  private void LBtnRemoveClick(object sender, RoutedEventArgs e)
  {
  }

  private void Grid_MouseMove(object sender, MouseEventArgs e)
  {
    if (e.LeftButton != MouseButtonState.Pressed)
      return;
    this.DragMove();
  }

  private void OnListViewItemDoubleClick(object sender, MouseButtonEventArgs e)
  {
    if (e.Source is CheckBox)
      return;
    try
    {
      OpenHistoryModel dataContext = (sender as ListBoxItem).DataContext as OpenHistoryModel;
      if (string.IsNullOrWhiteSpace(dataContext.FilePath))
        return;
      GAManager.SendEvent("WelcomeWindow", "RecentOpenFile", "Count", 1L);
      DocsPathUtils.WriteFilesPathJson("unknow", (object) dataContext.FilePath);
      FileManager.OpenOneFile(dataContext.FilePath);
    }
    catch
    {
    }
  }

  private void AllToolsSwitch_Click(object sender, RoutedEventArgs e)
  {
    SwitchButton switchButton = sender as SwitchButton;
    switchButton.ContextMenu.DataContext = switchButton.DataContext;
    if (!switchButton.ContextMenu.IsOpen)
      return;
    switchButton.ContextMenu.PlacementTarget = (UIElement) switchButton;
    switchButton.ContextMenu.Placement = PlacementMode.Bottom;
  }

  private void ContextMenu_Closed(object sender, RoutedEventArgs e)
  {
    this.VM.AllToolsSwitchIsChecked = false;
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    base.OnClosing(e);
    WSUtils.SaveWindowInfo("Launcher");
  }

  private void fileItemCB_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
  {
    e.Handled = true;
  }

  private void Button_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
  {
    e.Handled = true;
  }

  private void openFileTipsCloseBtn_Click(object sender, RoutedEventArgs e)
  {
    ConfigManager.SetWelcomeOpenBtnTipsFlag(true);
    this.CloseAllTips();
  }

  private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    if (e.NewSize.Width <= 0.0 || e.NewSize.Height <= 0.0)
      return;
    Rect rect = this.openFileBtn.TransformToVisual((Visual) this.OpenFileBorder).TransformBounds(new Rect(0.0, 0.0, this.openFileBtn.ActualWidth, this.openFileBtn.ActualHeight));
    Canvas.SetLeft((UIElement) this.OpenButtonTips, rect.Right);
    Canvas.SetTop((UIElement) this.OpenButtonTips, rect.Top + (rect.Height - this.OpenButtonTips.ActualHeight) / 2.0);
  }

  private void CloseAllTips() => this.VM.OpenBtnTipsVisibility = Visibility.Collapsed;

  private void ShowAllTips() => this.VM.OpenBtnTipsVisibility = Visibility.Visible;

  private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (async () =>
    {
      FileWatcherHelper.Instance.UpdateState();
      int num = await UpdateHelper.UpdateAndExit(true) ? 1 : 0;
    }));
  }

  private void supportfeed_Click(object sender, RoutedEventArgs e)
  {
    FeedbackWindow feedbackWindow = new FeedbackWindow();
    feedbackWindow.HideFaq();
    feedbackWindow.Owner = Application.Current.MainWindow;
    feedbackWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    feedbackWindow.ShowDialog();
  }

  private void Button_Click_1(object sender, RoutedEventArgs e)
  {
    if (ThemeResourceDictionary.GetForCurrentApp().Theme == "Light")
      ThemeResourceDictionary.GetForCurrentApp().Theme = "Dark";
    else
      ThemeResourceDictionary.GetForCurrentApp().Theme = "Light";
  }

  private void ContextMenu_Opened(object sender, RoutedEventArgs e)
  {
  }

  private void Window_Drop(object sender, DragEventArgs e)
  {
    if (e.Data.GetDataPresent(DataFormats.FileDrop))
    {
      foreach (string file in (string[]) e.Data.GetData(DataFormats.FileDrop))
      {
        if (!string.IsNullOrWhiteSpace(file))
        {
          DocsPathUtils.WriteFilesPathJson("unknow", (object) file);
          GAManager.SendEvent("WelcomeWindow", "OpenOneFileBtnClick", "Count", 1L);
          FileManager.OpenOneFile(file);
        }
        else
          break;
      }
    }
    e.Handled = true;
  }

  private void ScanDocument_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("Ads", "ScanPDF", "BtnClick", 1L);
    GearForPDFScan gearForPdfScan = new GearForPDFScan();
    gearForPdfScan.Owner = Application.Current.MainWindow;
    gearForPdfScan.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    gearForPdfScan.ShowDialog();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/PDFLauncher;component/mainwindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.PDFLauncher = (MainWindow) target;
        this.PDFLauncher.Closing += new CancelEventHandler(this.Window_Closing);
        this.PDFLauncher.Drop += new DragEventHandler(this.Window_Drop);
        break;
      case 2:
        this.supportfeed = (Button) target;
        this.supportfeed.Click += new RoutedEventHandler(this.supportfeed_Click);
        break;
      case 3:
        this.openFileBtn1 = (Button) target;
        break;
      case 4:
        this.NewFileBtn = (Button) target;
        break;
      case 5:
        this.FineFileBtn = (Button) target;
        break;
      case 6:
        this.ScanDocument = (Button) target;
        this.ScanDocument.Click += new RoutedEventHandler(this.ScanDocument_Click);
        break;
      case 7:
        this.Version = (TextBlock) target;
        break;
      case 8:
        ((UIElement) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.TextBlock_MouseLeftButtonDown);
        break;
      case 9:
        this.topDecorateBorder = (Border) target;
        break;
      case 10:
        this.openFileGrid = (Grid) target;
        break;
      case 11:
        this.tbVersionNum = (TextBlock) target;
        break;
      case 12:
        this.tbSupport = (TextBlock) target;
        break;
      case 13:
        this.OpenFileBorder = (Border) target;
        break;
      case 14:
        this.OpenFileBgBord = (Border) target;
        this.OpenFileBgBord.MouseDown += new MouseButtonEventHandler(this.Image_MouseDown);
        break;
      case 15:
        this.openFileImg = (Image) target;
        break;
      case 16 /*0x10*/:
        this.openFileBtn = (Button) target;
        break;
      case 17:
        this.tbSupportType = (TextBlock) target;
        break;
      case 18:
        this.OpenButtonTipsGrid = (Grid) target;
        break;
      case 19:
        this.OpenButtonTips = (Border) target;
        break;
      case 20:
        this.OpenButtonTipsContent = (Border) target;
        break;
      case 21:
        this.OpenButtonTipsGridTrans = (ScaleTransform) target;
        break;
      case 22:
        this.openFileTipsCloseBtn = (Button) target;
        this.openFileTipsCloseBtn.Click += new RoutedEventHandler(this.openFileTipsCloseBtn_Click);
        break;
      case 23:
        this.lstBoxMenu = (ListBox) target;
        this.lstBoxMenu.SelectionChanged += new SelectionChangedEventHandler(this.lstBoxMenu_SelectionChanged);
        break;
      case 24:
        this.recommendGrid = (Grid) target;
        break;
      case 25:
        this.stpHotTools = (StackPanel) target;
        break;
      case 26:
        this.stpHotTools0 = (StackPanel) target;
        break;
      case 27:
        this.lbtnHotToolPDF2Word = (LabelButton) target;
        break;
      case 28:
        this.lbtnHotToolPDF2Excel = (LabelButton) target;
        break;
      case 29:
        this.lbtnHotToolPDF2Png = (LabelButton) target;
        break;
      case 30:
        this.lbtnHotToolPDFCompress = (LabelButton) target;
        break;
      case 31 /*0x1F*/:
        this.lbtnHotToolMergePDF = (LabelButton) target;
        break;
      case 32 /*0x20*/:
        this.stpHotTools1 = (StackPanel) target;
        break;
      case 33:
        this.lbtnHotToolConvertWordToPDF = (LabelButton) target;
        break;
      case 34:
        this.lbtnHotToolConvertImageToPDF = (LabelButton) target;
        break;
      case 35:
        this.lbtnHotToolFillForm = (LabelButton) target;
        break;
      case 36:
        this.lbtnHotToolSplitPDF = (LabelButton) target;
        break;
      case 37:
        this.stpConvert = (StackPanel) target;
        break;
      case 38:
        this.stpConvert0 = (StackPanel) target;
        break;
      case 39:
        this.lbtnConvertPDF2Word = (LabelButton) target;
        break;
      case 40:
        this.lbtnConvertPDF2Excel = (LabelButton) target;
        break;
      case 41:
        this.lbtnConvertPDF2Png = (LabelButton) target;
        break;
      case 42:
        this.lbtnConvertPDF2JPG = (LabelButton) target;
        break;
      case 43:
        this.stpConvert1 = (StackPanel) target;
        break;
      case 44:
        this.lbtnConvertPDF2Txt = (LabelButton) target;
        break;
      case 45:
        this.lbtnConvertPDF2PPT = (LabelButton) target;
        break;
      case 46:
        this.lbtnConvertPDF2XML = (LabelButton) target;
        break;
      case 47:
        this.lbtnConvertPDF2RTF = (LabelButton) target;
        break;
      case 48 /*0x30*/:
        this.stpConToPDF = (StackPanel) target;
        break;
      case 49:
        this.stpConToPDF0 = (StackPanel) target;
        break;
      case 50:
        this.lbtnConvertWordToPDF = (LabelButton) target;
        break;
      case 51:
        this.lbtnConvertExcelToPDF = (LabelButton) target;
        break;
      case 52:
        this.lbtnConvertPPT = (LabelButton) target;
        break;
      case 53:
        this.lbtnConvertImgToPDF = (LabelButton) target;
        break;
      case 54:
        this.stpConToPDF1 = (StackPanel) target;
        break;
      case 55:
        this.lbtnConvertRTFToPDF = (LabelButton) target;
        break;
      case 56:
        this.lbtnConvertTXTToPDF = (LabelButton) target;
        break;
      case 57:
        this.stpMergeSplit = (StackPanel) target;
        break;
      case 58:
        this.stpMergeSplit0 = (StackPanel) target;
        break;
      case 59:
        this.lbtnMergeSplit_Merge = (LabelButton) target;
        break;
      case 60:
        this.lbtnMergeSplit_Split = (LabelButton) target;
        break;
      case 61:
        this.stpAllTools = (StackPanel) target;
        break;
      case 62:
        this.LbtnAllTool_PDF2Word = (LabelButton) target;
        break;
      case 63 /*0x3F*/:
        this.LbtnAllTool_PDF2Excel = (LabelButton) target;
        break;
      case 64 /*0x40*/:
        this.LbtnAllTool_PDF2PNG = (LabelButton) target;
        break;
      case 65:
        this.LbtnAllTool_PDF2JPG = (LabelButton) target;
        break;
      case 66:
        this.LbtnAllTool_PDF2PPT = (LabelButton) target;
        break;
      case 67:
        this.LbtnAllTool_PDF2Txt = (LabelButton) target;
        break;
      case 68:
        this.LbtnAllTool_PDF2WebPages = (LabelButton) target;
        break;
      case 69:
        this.LbtnAllTool_PDF2XML = (LabelButton) target;
        break;
      case 70:
        this.LbtnAllTool_PDF2RTF = (LabelButton) target;
        break;
      case 71:
        this.LbtnAllTool_Word2PDF = (LabelButton) target;
        break;
      case 72:
        this.LbtnAllTool_Excel2PDF = (LabelButton) target;
        break;
      case 73:
        this.LbtnAllTool_PPT2PDF = (LabelButton) target;
        break;
      case 74:
        this.LbtnAllTool_Image2PDF = (LabelButton) target;
        break;
      case 75:
        this.LbtnAllTool_RTF2PDF = (LabelButton) target;
        break;
      case 76:
        this.LbtnAllTool_TXT2PDF = (LabelButton) target;
        break;
      case 77:
        this.LbtnAllTool_Compress = (LabelButton) target;
        break;
      case 78:
        this.LbtnAllTool_Merge = (LabelButton) target;
        break;
      case 79:
        this.LbtnAllTool_Split = (LabelButton) target;
        break;
      case 80 /*0x50*/:
        this.LbtnAllTool_FillForm = (LabelButton) target;
        break;
      case 81:
        this.historyGrid = (Grid) target;
        break;
      case 82:
        this.lsvOpenHistory = (ListView) target;
        break;
      case 84:
        this.gView = (GridView) target;
        break;
      case 88:
        this.AllToolsSwitch = (SwitchButton) target;
        break;
      case 89:
        ((ContextMenu) target).Closed += new RoutedEventHandler(this.ContextMenu_Closed);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IStyleConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 83:
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = Control.MouseDoubleClickEvent,
          Handler = (Delegate) new MouseButtonEventHandler(this.OnListViewItemDoubleClick)
        });
        break;
      case 85:
        ((ToggleButton) target).Checked += new RoutedEventHandler(this.fileItemListSelectAll);
        ((ToggleButton) target).Unchecked += new RoutedEventHandler(this.fileItemListSelectNone);
        break;
      case 86:
        ((ToggleButton) target).Checked += new RoutedEventHandler(this.fileItemChecked);
        ((ToggleButton) target).Unchecked += new RoutedEventHandler(this.fileItemUnchecked);
        ((Control) target).PreviewMouseDoubleClick += new MouseButtonEventHandler(this.fileItemCB_PreviewMouseDoubleClick);
        break;
      case 87:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.Button_Click);
        ((Control) target).PreviewMouseDoubleClick += new MouseButtonEventHandler(this.Button_PreviewMouseDoubleClick);
        break;
    }
  }
}
