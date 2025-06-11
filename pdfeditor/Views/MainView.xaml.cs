// Decompiled with JetBrains decompiler
// Type: pdfeditor.Views.MainView
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using CommomLib.Commom;
using CommomLib.Config.ConfigModels;
using HandyControl.Tools;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Controls;
using pdfeditor.Controls.Bookmarks;
using pdfeditor.Controls.Copilot;
using pdfeditor.Controls.Menus;
using pdfeditor.Controls.Menus.ToolbarSettings;
using pdfeditor.Controls.Screenshots;
using pdfeditor.Models.Bookmarks;
using pdfeditor.Models.Menus;
using pdfeditor.Models.Menus.ToolbarSettings;
using pdfeditor.Models.Thumbnails;
using pdfeditor.Models.Viewer;
using pdfeditor.Services;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Views;

public partial class MainView : Window, IComponentConnector, IStyleConnector
{
  private bool isClosing;
  private bool isReadyToClose;
  private bool isClosed;
  private static BackgroundColorSetting[] viewerBackgroundColorValues;
  private static IReadOnlyDictionary<string, BackgroundColorSetting> viewerBackgroundColorDict;
  private MicaHelper micaHelper;
  private BookmarkModel bookmarkContextMenuData;
  private double menuHeaderMaxWidth;
  public static readonly DependencyProperty IsHeaderMenuCompactModeEnabledProperty = DependencyProperty.RegisterAttached("IsHeaderMenuCompactModeEnabled", typeof (bool), typeof (MainView), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.Inherits));
  private System.Timers.Timer selectTimer;
  internal RowDefinition TitlebarRow;
  internal Grid TitlebarPlaceholder;
  internal RowDefinition QuickToolHeaderRow;
  internal RowDefinition MenuRow;
  internal RowDefinition MainRow;
  internal RowDefinition FooterRow;
  internal Grid HeaderContainer;
  internal Grid QuickToolContainer;
  internal Button NewPDFBtn;
  internal Button openBtn;
  internal Button saveBtn;
  internal Button saveAsBtn;
  internal Button printBtn;
  internal Button undoBtn;
  internal Button redoBtn;
  internal Grid MenuHeaderContainer;
  internal ScrollViewer MenuHeaderScrollViewer;
  internal ListBox Menus;
  internal Grid menuFeedBack;
  internal Grid menuBackgroundMode;
  internal Button BackgroundModeButton;
  internal Popup BackgroundModeMenu;
  internal ListBox ThemeListBox;
  internal ListBox PaperColorListBox;
  internal Grid ShareFile;
  internal ToolbarButton ShareButton;
  internal Grid menuFullScreen;
  internal ToolbarToggleButton FullScreenButton;
  internal Grid menuToolbarShow;
  internal Grid MenuContainer;
  internal Grid MenuScrollLeftMask;
  internal Grid MenuScrollRightMask;
  internal ScrollViewer MenuScrollViewer;
  internal Grid menuView;
  internal StackPanel ExitAnnotPanel1;
  internal Grid ZoomSpeace;
  internal ComboBox ZoomCombobox;
  internal ToolbarToggleButton autoScroll;
  internal ToolbarButton presentBtn;
  internal ToolbarRadioButton screenshotBtn;
  internal ToolbarRadioButton ocrBtn;
  internal ToolbarRadioButton cropPageBtn;
  internal Grid menuAnnotation;
  internal StackPanel AnnonationButtons;
  internal StackPanel ExitAnnotPanel2;
  internal ContentPresenter highlight;
  internal ContentPresenter underline;
  internal ContentPresenter strike;
  internal ContentPresenter highlightarea;
  internal ContentPresenter line;
  internal ContentPresenter shape;
  internal ContentPresenter ellipse;
  internal ContentPresenter ink;
  internal ContentPresenter textbox;
  internal ContentPresenter text;
  internal ContentPresenter note;
  internal ToolbarButton stamp;
  internal ToolbarButton HideAnnotationButton;
  internal ToolbarButton ShowAnnotationButton;
  internal ToolbarButton ManageAnnotationButton;
  internal Grid MenuFillForm;
  internal ContentPresenter text2;
  internal ToolbarButton image2;
  internal ToolbarButton signature3;
  internal Grid menuInsert;
  internal ToolbarButton editDocumentBtn;
  internal ToolbarToggleButton editContentBtn;
  internal ContentPresenter text3;
  internal ToolbarButton image;
  internal ToolbarRadioButton link;
  internal ToolbarButton watermark;
  internal ToolbarButton stamp1;
  internal ToolbarButton signature;
  internal Grid menuTools;
  internal ToolbarButton converter;
  internal ToolbarButton CompressBtn;
  internal ToolbarButton signature2;
  internal ToolbarToggleButton Speech;
  internal Grid menuPage;
  internal Grid menuEncrypt;
  internal Grid menuShare;
  internal Grid menuHelp;
  internal Grid PdfContentContainer;
  internal NavigationView LeftNavigationView;
  internal BookmarkControl bookmarkControl;
  internal MenuItem AddChildBookmarkMenuItem;
  internal Separator BookmarkMenuSeparator;
  internal MenuItem DeleteBookmarkMenuItem;
  internal MenuItem EditBookmarkMenuItem;
  internal Separator BookmarkMenuSeparator2;
  internal MenuItem WrapBookmarkMenuItem;
  internal MenuItem ExpandCollapseBookmarkMenuItem;
  internal ToggleButton SidebarInsertBtn;
  internal Button Button_InsertBlank;
  internal Button Button_InsertPDF;
  internal Button Button_InsertWord;
  internal Button Button_InsertImage;
  internal PdfPagePreviewListView ThumbnailList;
  internal StackPanel CommetExpandButtonContainer;
  internal ToggleButton FilterBtn;
  internal Popup filterpop;
  internal ToggleButton UsersFilterbtn;
  internal ToggleButton AnnotationFilterbtn;
  internal Border BatchDeleteArea;
  internal CommetControl CommetMenuControl;
  internal Grid ViewerContainer;
  internal Grid TextEditingBanner;
  internal PDFKit.PdfControl PdfControl;
  internal AnnotationCanvas AnnotationEditorCanvas;
  internal ToolbarSettingPanel AnnotToolbarSettingPanel;
  internal StackPanel ViewerConverterButtonContainer;
  internal AnimationExtentButton MouseOverPdfToWordButton;
  internal AnimationExtentButton MouseOverPdfToExcelButton;
  internal AnimationExtentButton MouseOverPdfToPPTButton;
  internal AnimationExtentButton MouseOverPdfToImageButton;
  internal AnimationExtentButton MouseOverPdfToJpegButton;
  internal DocumentSearchBox SearchBox;
  internal ChatButton ChatButton;
  internal NavigationView RightNavigationView;
  internal ChatPanel ChatPanel;
  internal Grid FooterContainer;
  internal Rectangle FooterContainerHairline;
  internal ProgressBar progressBar;
  internal Label lblsaveTime;
  internal Grid menuFooterShow;
  internal Grid PagesEditorContainer;
  internal PdfPagePreviewGridView PageGridView;
  internal Grid PagesEditorFooterContainer;
  internal Button PagesEditorCheckboxButton;
  internal Slider PagesEditorThumbnailScaleSlider;
  private bool _contentLoaded;

  public MainView()
  {
    this.DataContext = (object) Ioc.Default.GetRequiredService<MainViewModel>();
    this.InitializeComponent();
    this.PdfControl.Viewer.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.Viewer_IsVisibleChanged);
    this.Loaded += new RoutedEventHandler(this.MainView_Loaded);
    WSUtils.LoadWindowInfo();
    this.micaHelper = MicaHelper.Create((Window) this);
    if (this.micaHelper != null)
    {
      this.micaHelper.IsMicaEnabled = true;
      this.TitlebarPlaceholder.Visibility = Visibility.Visible;
      this.micaHelper.TitlebarPlaceholder = (FrameworkElement) this.TitlebarPlaceholder;
    }
    else
      this.Background = (Brush) new SolidColorBrush(Colors.White);
    LaunchUtils.LaunchActionInvoked += new LaunchActionInvokedEventHandler(this.LaunchUtils_LaunchActionInvoked);
    this.InitViewerBackgroundColorValues();
    this.PdfControl.Viewer.IsFillFormHighlighted = ConfigManager.GetIsFillFormHighlightedFlag();
    this.InitViewerThemeValues();
  }

  protected override void OnSourceInitialized(EventArgs e)
  {
    base.OnSourceInitialized(e);
    ((HwndSource) PresentationSource.FromVisual((Visual) this))?.AddHook(new HwndSourceHook(WndProc));

    static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
      if (msg == 17)
      {
        CommomLib.Commom.Log.WriteLog("WM_QUERYENDSESSION");
        pdfeditor.AutoSaveRestore.AutoSaveManager.GetInstance().TrySaveImmediately();
        handled = true;
        return (IntPtr) 1;
      }
      if (msg != 22)
        return IntPtr.Zero;
      CommomLib.Commom.Log.WriteLog("WM_ENDSESSION");
      handled = true;
      Application.Current.Shutdown();
      return (IntPtr) 1;
    }
  }

  private void LaunchUtils_LaunchActionInvoked(
    PdfDocument sender,
    LaunchActionInvokedEventArgs args)
  {
    switch (args.Action)
    {
      case "tab:fillform":
        CommomLib.Commom.GAManager.SendEvent("ToolbarAction", "FillForm", "LaunchAction", 1L);
        this.Menus.SelectedItem = (object) this.VM.Menus.MainMenus.FirstOrDefault<MainMenuGroup>((Func<MainMenuGroup, bool>) (c => c.Tag == "FillForm"));
        break;
      case "new:CreatedFile":
        this.VM.CreatePdfFileAsync();
        break;
      case "open:CreatedFile":
        this.VM.DocumentWrapper.SetUntitledFile();
        this.VM.SetCanSaveFlag("CreateNew", false);
        pdfeditor.AutoSaveRestore.AutoSaveManager.GetInstance().LastOperationVersion = "CreateNew";
        break;
    }
  }

  private void Viewer_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    App.Current.AppHotKeyManager?.UpdateHotKeyEnabledStates();
  }

  public void Menus_SelectItem(string tag)
  {
    if (this.Menus == null || this.Menus.Items.Count <= 0)
      return;
    foreach (MainMenuGroup mainMenuGroup in (IEnumerable) this.Menus.Items)
    {
      if (mainMenuGroup != null && mainMenuGroup.Tag == tag)
      {
        this.Menus.SelectedItem = (object) mainMenuGroup;
        return;
      }
    }
    this.Menus.SelectedIndex = 0;
  }

  protected MainViewModel VM => this.DataContext as MainViewModel;

  private async void MainView_Loaded(object sender, RoutedEventArgs e)
  {
    this.Menus_SelectItem("View");
    this.UpdateBookmarkWrapMode();
    await this.VM.OpenStartUpFileCmd.ExecuteAsync((object) null);
  }

  private void VM_SelectedPageIndexChanged(object sender, EventArgs e)
  {
    this.PdfControl.ScrollToPage(this.VM.SelectedPageIndex);
  }

  private void ThumbnailList_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    object obj = e.AddedItems.OfType<object>().FirstOrDefault<object>();
    if (obj == null)
      return;
    ((PdfPagePreviewListView) sender).ScrollIntoView(obj);
    if (this.PagesEditorContainer.Visibility != Visibility.Collapsed)
      return;
    int selectedIndex = ((Selector) sender).SelectedIndex;
    if (selectedIndex == -1 || this.VM.PageEditors?.PageEditListItemSource == null || selectedIndex >= this.VM.PageEditors.PageEditListItemSource.Count)
      return;
    this.PageGridView.ScrollIntoView((object) this.VM.PageEditors?.PageEditListItemSource[selectedIndex]);
  }

  private void PageNum_GotFocus(object sender, RoutedEventArgs e)
  {
    TextBoxBase tb = sender as TextBoxBase;
    if (tb == null)
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (() =>
    {
      tb.SelectAll();
      if (Keyboard.FocusedElement == tb)
        return;
      Keyboard.Focus((IInputElement) tb);
    }));
  }

  private void ZoomCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    try
    {
      if (!(sender is ComboBox comboBox) || comboBox.SelectedValue == null)
        return;
      this.UpdateDocZoom(comboBox.SelectedValue.ToString());
    }
    catch (Exception ex)
    {
    }
  }

  private void ZoomCB_KeyUp(object sender, KeyEventArgs e)
  {
    if (e.Key != Key.Return)
      return;
    try
    {
      string zoomStr = "";
      if (sender is ComboBox comboBox && !string.IsNullOrWhiteSpace(comboBox.Text))
        zoomStr = comboBox.Text;
      if (string.IsNullOrWhiteSpace(zoomStr) && sender is TextBox textBox && !string.IsNullOrWhiteSpace(textBox.Text))
        zoomStr = textBox.Text;
      if (string.IsNullOrWhiteSpace(zoomStr))
        return;
      this.UpdateDocZoom(zoomStr);
    }
    catch (Exception ex)
    {
    }
  }

  private void ZoomCB_LostFocus(object sender, RoutedEventArgs e)
  {
    BindingOperations.GetBindingExpression((DependencyObject) sender, ComboBox.TextProperty)?.UpdateTarget();
    BindingOperations.GetBindingExpression((DependencyObject) sender, TextBox.TextProperty)?.UpdateTarget();
  }

  private void UpdateDocZoom(string zoomStr)
  {
    try
    {
      if (string.IsNullOrWhiteSpace(zoomStr))
        return;
      string[] strArray = zoomStr.Split('%');
      if (string.IsNullOrWhiteSpace(strArray[0]))
        return;
      int num = 0;
      try
      {
        num = Convert.ToInt32(strArray[0]);
      }
      catch
      {
      }
      if (num <= 0)
        return;
      this.VM.ViewToolbar.UpdateDocToZoom((float) num / 100f);
    }
    catch
    {
    }
  }

  protected override void OnClosed(EventArgs e)
  {
    base.OnClosed(e);
    this.isClosed = true;
    LaunchUtils.LaunchActionInvoked -= new LaunchActionInvokedEventHandler(this.LaunchUtils_LaunchActionInvoked);
    Application.Current.Windows.OfType<ComparisonWindow>().FirstOrDefault<ComparisonWindow>()?.Close();
    AppSettingsViewModel service = Ioc.Default.GetService<AppSettingsViewModel>();
    if ((service != null ? (service.LanguageChangedFlag ? 1 : 0) : 0) == 0)
      return;
    FileWatcherHelper.Instance.TryRestart();
  }

  protected override async void OnClosing(CancelEventArgs e)
  {
    MainView mainView = this;
    // ISSUE: reference to a compiler-generated method
    mainView.\u003C\u003En__0(e);
    if (mainView.isReadyToClose)
      ;
    else
    {
      e.Cancel = true;
      if (mainView.isClosing)
        ;
      else
      {
        mainView.isClosing = true;
        WSUtils.SaveWindowInfo();
        string currentOpenedFile = mainView.VM.DocumentWrapper.DocumentPath;
        await mainView.VM.ReleaseViewerFocusAsync(true);
        if (mainView.VM.CanSave)
        {
          if (mainView.VM.ViewToolbar.IsDocumentEdited)
            CommomLib.Commom.GAManager.SendEvent("TextEditor2", "ExitEditing", "CloseBtnDocEdited", 1L);
          if (await mainView.VM.TrySaveBeforeCloseDocumentAsync())
          {
            RateUtils.CheckAndShowRate(currentOpenedFile);
            mainView.Hide();
            await mainView.VM.AnnotationToolbar.SaveToolbarSettingsConfigAsync();
            await mainView.VM.CloseDocumentAsync();
            await DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (() =>
            {
              this.isReadyToClose = true;
              this.VM.DelAutoSaveFile(currentOpenedFile);
              this.Close();
            }));
          }
          else
            mainView.isClosing = false;
        }
        else
        {
          if (mainView.VM.ViewToolbar.IsDocumentEdited)
            CommomLib.Commom.GAManager.SendEvent("TextEditor2", "ExitEditing", "CloseBtnDocNotEdited", 1L);
          RateUtils.CheckAndShowRate(currentOpenedFile);
          mainView.Hide();
          await mainView.VM.AnnotationToolbar.SaveToolbarSettingsConfigAsync();
          await mainView.VM.CloseDocumentAsync();
          mainView.isReadyToClose = true;
          mainView.isClosing = false;
          mainView.VM.DelAutoSaveFile(currentOpenedFile);
          mainView.Close();
        }
      }
    }
  }

  protected override async void OnPreviewKeyDown(KeyEventArgs e)
  {
    MainView mainView = this;
    // ISSUE: reference to a compiler-generated method
    mainView.\u003C\u003En__1(e);
    if (e.OriginalSource is TextBoxBase)
      return;
    if (e.Key == Key.Escape)
    {
      PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(mainView.VM.Document);
      if (mainView.AnnotationEditorCanvas.TextObjectHolder.SelectedObject != null)
      {
        e.Handled = true;
        mainView.AnnotationEditorCanvas.TextObjectHolder.CancelTextObject();
      }
      else if (mainView.AnnotationEditorCanvas.HolderManager.CurrentHolder != null)
      {
        e.Handled = true;
        if (mainView.VM.AnnotationMode == AnnotationMode.Link)
        {
          if (mainView.VM.AnnotationToolbar.LinkButtonModel.ChildButtonModel is ToolbarChildCheckableButtonModel childButtonModel && childButtonModel.ContextMenu is TypedContextMenuModel contextMenu && contextMenu[0] is ContextMenuItemModel contextMenuItemModel)
            contextMenuItemModel.Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/LinkCE.png"));
          pdfControl.Viewer.IsLinkAnnotationHighlighted = false;
          if ((PdfWrapper) mainView.VM.SelectedAnnotation != (PdfWrapper) null)
            mainView.VM.SelectedAnnotation = (PdfAnnotation) null;
          mainView.VM.AnnotationMode = AnnotationMode.None;
        }
        mainView.AnnotationEditorCanvas.HolderManager.CancelAll();
      }
      else if (mainView.VM.AnnotationMode != AnnotationMode.None)
      {
        e.Handled = true;
        if (mainView.VM.AnnotationMode == AnnotationMode.Link)
        {
          if (mainView.VM.AnnotationToolbar.LinkButtonModel.ChildButtonModel is ToolbarChildCheckableButtonModel childButtonModel && childButtonModel.ContextMenu is TypedContextMenuModel contextMenu && contextMenu[0] is ContextMenuItemModel contextMenuItemModel)
            contextMenuItemModel.Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/LinkCE.png"));
          pdfControl.Viewer.IsLinkAnnotationHighlighted = false;
          if ((PdfWrapper) mainView.VM.SelectedAnnotation != (PdfWrapper) null)
            mainView.VM.SelectedAnnotation = (PdfAnnotation) null;
        }
        else if (mainView.VM.AnnotationMode == AnnotationMode.Ink && mainView.VM.AnnotationToolbar.InkButtonModel.ToolbarSettingModel[1] is ToolbarSettingInkEraserModel settingInkEraserModel && settingInkEraserModel.IsChecked)
          settingInkEraserModel.IsChecked = false;
        mainView.VM.AnnotationMode = AnnotationMode.None;
      }
      else if (FullScreenHelper.GetIsFullScreenEnabled((Window) mainView))
      {
        e.Handled = true;
        FullScreenHelper.SetIsFullScreenEnabled((Window) mainView, false);
        mainView.FullScreenToolbarToggleButton_Click((object) mainView.FullScreenButton, (RoutedEventArgs) null);
      }
      else
      {
        if (mainView.VM.ViewToolbar == null)
          return;
        mainView.VM.ViewToolbar.StopAutoScroll();
      }
    }
    else
    {
      if (e.Key == Key.Delete && Keyboard.Modifiers == ModifierKeys.None)
      {
        if ((PdfWrapper) mainView.VM.SelectedAnnotation != (PdfWrapper) null)
        {
          e.Handled = true;
          await mainView.VM.DeleteSelectedAnnotCmd.ExecuteAsync((object) null);
        }
        else if (mainView.AnnotationEditorCanvas.TextObjectHolder.SelectedObject != null)
        {
          CommomLib.Commom.GAManager.SendEvent("TextEditor", "DeleteSelectedObject", "DeleteKey", 1L);
          e.Handled = true;
          await mainView.AnnotationEditorCanvas.TextObjectHolder.DeleteSelectedObjectAsync();
        }
      }
      if (Keyboard.Modifiers == ModifierKeys.Control)
      {
        if (mainView.PdfControl.IsEditing && (e.Key == Key.O || e.Key == Key.P || e.Key == Key.S))
        {
          e.Handled = true;
          return;
        }
        if (e.Key == Key.C)
        {
          if (!string.IsNullOrEmpty(mainView.PdfControl.Viewer.SelectedText))
          {
            try
            {
              e.Handled = true;
              Clipboard.SetDataObject((object) mainView.PdfControl.Viewer.SelectedText);
            }
            catch
            {
            }
          }
        }
        if (e.Key == Key.A && mainView.PdfControl.Document != null && !mainView.PdfControl.IsEditing)
        {
          e.Handled = true;
          if (mainView.PagesEditorContainer.Visibility == Visibility.Visible)
          {
            if (mainView.VM?.PageEditors?.PageEditListItemSource != null)
              mainView.VM.PageEditors.PageEditListItemSource.AllItemSelected = new bool?(true);
          }
          else
            mainView.PdfControl.Viewer.SelectText(0, 0, mainView.PdfControl.Document.Pages.Count - 1, mainView.PdfControl.Document.Pages[mainView.PdfControl.Document.Pages.Count - 1].Text.CountChars);
        }
        if (e.Key == Key.Up && mainView.PdfControl.Document != null)
        {
          e.Handled = true;
          if (mainView.VM.CurrnetPageIndex > 0)
            --mainView.VM.CurrnetPageIndex;
        }
        if (e.Key == Key.Down && mainView.PdfControl.Document != null)
        {
          e.Handled = true;
          if (mainView.VM.CurrnetPageIndex <= mainView.VM.Document.Pages.Count)
            ++mainView.VM.CurrnetPageIndex;
        }
      }
      if (Keyboard.Modifiers != ModifierKeys.None)
        return;
      if (e.Key == Key.F11 && !e.IsRepeat)
      {
        e.Handled = true;
        ToolbarToggleButton fullScreenButton = mainView.FullScreenButton;
        bool? isChecked = mainView.FullScreenButton.IsChecked;
        bool? nullable = isChecked.HasValue ? new bool?(!isChecked.GetValueOrDefault()) : new bool?();
        fullScreenButton.IsChecked = nullable;
        mainView.EnterOrExitFullScreen();
      }
      if (mainView.PdfControl.IsEditing || !string.IsNullOrEmpty(mainView.PdfControl.Viewer.FillForms.FocusedText))
        return;
      if (e.Key == Key.Left && mainView.PdfControl.Document != null && (mainView.PdfControl.ScrollViewer == null || mainView.PdfControl.ScrollViewer.HorizontalOffset == 0.0))
      {
        e.Handled = true;
        if (mainView.VM.SelectedPageIndex > 0)
          --mainView.VM.SelectedPageIndex;
      }
      if (e.Key == Key.Right && mainView.PdfControl.Document != null && (mainView.PdfControl.ScrollViewer == null || mainView.PdfControl.ScrollViewer.HorizontalOffset == mainView.PdfControl.ScrollViewer.ScrollableWidth))
      {
        e.Handled = true;
        if (mainView.VM.SelectedPageIndex < mainView.VM.Document.Pages.Count - 1)
          ++mainView.VM.SelectedPageIndex;
      }
      if (e.Key == Key.Home && mainView.PdfControl.Document != null)
      {
        e.Handled = true;
        mainView.VM.SelectedPageIndex = 0;
      }
      if (e.Key == Key.End && mainView.PdfControl.Document != null)
      {
        e.Handled = true;
        mainView.VM.SelectedPageIndex = mainView.VM.TotalPagesCount - 1;
      }
      if (e.Key != Key.F5 || mainView.PdfControl.Document == null || !mainView.PdfControl.Viewer.IsVisible)
        return;
      e.Handled = true;
      mainView.VM.ViewToolbar.Present();
    }
  }

  public async void ToolbarScreenShotButton_Click(object sender, RoutedEventArgs e)
  {
    this.SearchBox.CloseSearchBox();
    this.VM.ExitTransientMode(true);
    RadioButton btn = (RadioButton) sender;
    if (this.VM.Document == null)
    {
      btn.IsChecked = new bool?(false);
      btn = (RadioButton) null;
    }
    else if (btn.IsChecked.GetValueOrDefault())
    {
      this.VM.AnnotationMode = AnnotationMode.None;
      await this.VM.ReleaseViewerFocusAsync(true);
      bool flag = false;
      string str = "";
      ScreenshotDialogMode mode = ScreenshotDialogMode.Screenshot;
      if (sender == this.screenshotBtn)
      {
        mode = ScreenshotDialogMode.Screenshot;
        CommomLib.Commom.GAManager.SendEvent("MainWindow", "ScreenshotBtn", "Count", 1L);
        flag = true;
        str = pdfeditor.Properties.Resources.ScreenshotTip;
      }
      else if (sender == this.ocrBtn)
      {
        mode = ScreenshotDialogMode.Ocr;
        CommomLib.Commom.GAManager.SendEvent("MainWindow", "OCRBtn", "Count", 1L);
        flag = true;
        str = pdfeditor.Properties.Resources.OCRTip;
      }
      else if (sender == this.cropPageBtn)
      {
        mode = ScreenshotDialogMode.CropPage;
        CommomLib.Commom.GAManager.SendEvent("PageView", "CropPage", "Main", 1L);
      }
      if (flag && !ConfigManager.GetDoNotShowFlag("NotShowScreenshotTipFlag"))
      {
        bool? checkboxResult = MessageBoxHelper.Show(new MessageBoxHelper.RichMessageBoxContent()
        {
          Content = (object) str,
          ShowLeftBottomCheckbox = true,
          LeftBottomCheckboxContent = pdfeditor.Properties.Resources.WinPwdPasswordSaveTipNotshowagainContent
        }, UtilManager.GetProductName()).CheckboxResult;
        if (checkboxResult.HasValue && checkboxResult.GetValueOrDefault())
          ConfigManager.SetDoNotShowFlag("NotShowScreenshotTipFlag", true);
      }
      this.AnnotationEditorCanvas.CloseScreenShot();
      btn.IsChecked = new bool?(true);
      await this.AnnotationEditorCanvas.StartScreenShotAsync(mode);
      btn = (RadioButton) null;
    }
    else
    {
      CommomLib.Commom.GAManager.SendEvent("CropPage", "CancelCropPageBtn", "Count", 1L);
      this.AnnotationEditorCanvas.CloseScreenShot();
      btn = (RadioButton) null;
    }
  }

  private void AnnotationEditorCanvas_ScreenshotDialogClosed(object sender, EventArgs e)
  {
    this.screenshotBtn.IsChecked = new bool?(false);
    this.ocrBtn.IsChecked = new bool?(false);
    this.cropPageBtn.IsChecked = new bool?(false);
  }

  private void Menus_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.UpdateMenuScrollStates();
    MainMenuGroup mainMenuGroup = e.AddedItems.OfType<MainMenuGroup>().FirstOrDefault<MainMenuGroup>();
    if (mainMenuGroup != null)
    {
      if (this.VM?.Document == null)
      {
        if (!(mainMenuGroup.Tag == "Insert") && !(mainMenuGroup.Tag == "Annotate") && !(mainMenuGroup.Tag == "FillForm") && !(mainMenuGroup.Tag == "Page") && !(mainMenuGroup.Tag == "Protection") && !(mainMenuGroup.Tag == "Share") && !(mainMenuGroup.Tag == "Tools"))
          return;
        ((Selector) sender).SelectedItem = (object) (e.RemovedItems.OfType<MainMenuGroup>().FirstOrDefault<MainMenuGroup>() ?? this.VM.Menus.MainMenus[0]);
      }
      else
      {
        if (mainMenuGroup.Tag == "Page" && this.PagesEditorContainer.Visibility == Visibility.Collapsed)
        {
          this.VM.ExitTransientMode();
          this.AnnotationEditorCanvas.HolderManager.CancelAll();
          this.menuFooterShow.Visibility = Visibility.Collapsed;
          this.PagesEditorContainer.Visibility = Visibility.Visible;
          this.PagesEditorFooterContainer.Visibility = Visibility.Visible;
          this.FooterContainer.Visibility = Visibility.Collapsed;
          this.PdfContentContainer.Visibility = Visibility.Collapsed;
          this.FooterContainer.Visibility = Visibility.Collapsed;
        }
        else if ((mainMenuGroup.Tag == "View" || mainMenuGroup.Tag == "Annotate" || mainMenuGroup.Tag == "FillForm" || mainMenuGroup.Tag == "Insert" || mainMenuGroup.Tag == "Tools") && this.PdfContentContainer.Visibility == Visibility.Collapsed)
        {
          this.menuFooterShow.Visibility = Visibility.Visible;
          this.PagesEditorContainer.Visibility = Visibility.Collapsed;
          this.PagesEditorFooterContainer.Visibility = Visibility.Collapsed;
          this.FooterContainer.Visibility = Visibility.Visible;
          this.PdfContentContainer.Visibility = Visibility.Visible;
          this.FooterContainer.Visibility = Visibility.Visible;
        }
        CommomLib.Commom.GAManager.SendEvent("ToolbarAction", mainMenuGroup.Tag, "SelectionChanged", 1L);
      }
    }
    else
      ((Selector) sender).SelectedItem = (object) (e.RemovedItems.OfType<MainMenuGroup>().FirstOrDefault<MainMenuGroup>() ?? this.VM.Menus.MainMenus[0]);
  }

  private void SearchToolbarButton_Click(object sender, RoutedEventArgs e) => this.ShowSearchBox();

  public bool ShowSearchBox()
  {
    this.VM.ExitTransientMode();
    if (this.VM.ViewToolbar.EditDocumentButtomModel.IsChecked || this.VM.Menus.SearchModel == null || !this.VM.Menus.SearchModel.IsSearchEnabled)
      return false;
    string selectedText = this.PdfControl.Viewer.SelectedText;
    if (!string.IsNullOrEmpty(selectedText) && selectedText.Length < 200)
      this.VM.Menus.SearchModel.SearchText = selectedText;
    if (!this.VM.Menus.SearchModel.IsSearchVisible)
      this.VM.Menus.SearchModel.IsSearchVisible = true;
    else
      this.SearchBox.Focus();
    return true;
  }

  private void MenuScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
  {
    this.UpdateMenuScrollStates();
  }

  private void UpdateMenuScrollStates()
  {
    StackPanel[] source = new StackPanel[2]
    {
      this.ExitAnnotPanel1,
      this.ExitAnnotPanel2
    };
    foreach (StackPanel stackPanel in source)
    {
      if (!(stackPanel.RenderTransform is TranslateTransform translateTransform))
      {
        translateTransform = new TranslateTransform();
        stackPanel.RenderTransform = (Transform) translateTransform;
      }
      translateTransform.X = this.MenuScrollViewer.HorizontalOffset;
    }
    if (this.MenuScrollViewer.HorizontalOffset == 0.0)
    {
      this.MenuScrollLeftMask.Visibility = Visibility.Collapsed;
    }
    else
    {
      double num = 0.0;
      if (this.Menus.SelectedItem is MainMenuGroup selectedItem && (selectedItem.Tag == "View" || selectedItem.Tag == "Annotate"))
        num = ((IEnumerable<StackPanel>) source).Select<StackPanel, double>((Func<StackPanel, double>) (c => c.ActualWidth)).Max();
      this.MenuScrollLeftMask.Margin = new Thickness(num - 2.0, 0.0, 0.0, 0.0);
      this.MenuScrollLeftMask.Visibility = Visibility.Visible;
    }
    if (this.MenuScrollViewer.HorizontalOffset > this.MenuScrollViewer.ScrollableWidth - 5.0)
      this.MenuScrollRightMask.Visibility = Visibility.Collapsed;
    else
      this.MenuScrollRightMask.Visibility = Visibility.Visible;
  }

  private void MenuScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
  {
    if (((ScrollViewer) sender).ScrollableWidth <= 0.0)
      return;
    double horizontalOffset = ((ScrollViewer) sender).HorizontalOffset;
    int num = !(e is MouseTiltEventArgs mouseTiltEventArgs) ? -e.Delta / 2 : mouseTiltEventArgs.Delta / 2;
    ((ScrollViewer) sender).ScrollToHorizontalOffset(horizontalOffset + (double) num);
  }

  private void MenuNavigationButton_Click(object sender, RoutedEventArgs e)
  {
    int num = 40;
    if (((FrameworkElement) sender).HorizontalAlignment == HorizontalAlignment.Left)
      num = -num;
    this.MenuScrollViewer.ScrollToHorizontalOffset(this.MenuScrollViewer.HorizontalOffset + (double) num);
  }

  private async void Viewer_LostFocus(object sender, RoutedEventArgs e)
  {
    IInputElement focusedElement = Keyboard.FocusedElement;
    if (focusedElement == this.PdfControl.Editor || focusedElement == this.PdfControl.Viewer || focusedElement is ScrollViewer scrollViewer && (scrollViewer.Content == this.PdfControl.Editor || scrollViewer.Content == this.PdfControl.Viewer) || focusedElement is FrameworkElement _ele && IsChild<AnnotationCanvas>(_ele))
      return;
    await this.VM.ReleaseViewerFocusAsync(false);

    static bool IsChild<T>(FrameworkElement _ele) where T : DependencyObject
    {
      return (object) GetParent<T>(_ele) != null;
    }

    static T GetParent<T>(FrameworkElement _ele) where T : DependencyObject
    {
      for (; _ele != null; _ele = parent2)
      {
        if (_ele is T parent1)
          return parent1;
        if (!(_ele.Parent is FrameworkElement parent2))
          parent2 = VisualTreeHelper.GetParent((DependencyObject) _ele) as FrameworkElement;
      }
      return default (T);
    }
  }

  private void PagesEditorContainer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
  {
    if (e.Delta == 0 || (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.None)
      return;
    e.Handled = true;
    if (e.Delta < 0)
      this.PagesEditorThumbnailScaleSlider.Value -= 0.1;
    else
      this.PagesEditorThumbnailScaleSlider.Value += 0.1;
  }

  private void PagesEditorCheckboxButton_Click(object sender, RoutedEventArgs e)
  {
    if (this.VM.PageEditors?.PageEditListItemSource == null)
      return;
    bool? nullable = new bool?();
    bool? allItemSelected = (bool?) this.VM.PageEditors?.PageEditListItemSource.AllItemSelected;
    nullable = !allItemSelected.HasValue ? new bool?(true) : (!allItemSelected.GetValueOrDefault() ? new bool?(true) : new bool?(false));
    this.VM.PageEditors.PageEditListItemSource.AllItemSelected = nullable;
  }

  private void CommetMenuContainer_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    if (e.NewSize.Width > 230.0)
      this.CommetExpandButtonContainer.Visibility = Visibility.Visible;
    else
      this.CommetExpandButtonContainer.Visibility = Visibility.Collapsed;
  }

  private void CommetExpandButton_Click(object sender, RoutedEventArgs e)
  {
    this.CommetMenuControl.ExpandAll();
  }

  private void CommetCollapseButton_Click(object sender, RoutedEventArgs e)
  {
    this.CommetMenuControl.CollapseAll();
  }

  private void MenuGroup_MouseDoubleClick(object sender, MouseButtonEventArgs e)
  {
    this.VM.Menus.IsShowToolbar = false;
  }

  private void MenuGroup_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    this.VM.Menus.IsShowToolbar = true;
  }

  private void FooterShowHideButton_Click(object sender, RoutedEventArgs e)
  {
    this.VM.Menus.IsShowFooter = !this.VM.Menus.IsShowFooter;
    if (this.VM.Menus.IsShowFooter)
    {
      Grid.SetRowSpan((UIElement) this.PdfContentContainer, 1);
      Panel.SetZIndex((UIElement) this.FooterContainer, 0);
      this.FooterContainerHairline.Visibility = Visibility.Visible;
    }
    else
    {
      Grid.SetRowSpan((UIElement) this.PdfContentContainer, 2);
      Panel.SetZIndex((UIElement) this.FooterContainer, -1);
      this.FooterContainerHairline.Visibility = Visibility.Collapsed;
    }
  }

  public void SetFooterVisible(bool value)
  {
    if (value == this.VM.Menus.IsShowFooter)
      return;
    this.FooterShowHideButton_Click((object) null, (RoutedEventArgs) null);
  }

  private void FullScreenToolbarToggleButton_Click(object sender, RoutedEventArgs e)
  {
    this.EnterOrExitFullScreen();
  }

  private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
  {
    if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
    {
      e.Handled = true;
      if (e.Delta < 0)
        this.VM.ViewToolbar.DocZoomOut(true, 0.05f);
      else if (e.Delta > 0)
        this.VM.ViewToolbar.DocZoomIn(true, 0.05f);
    }
    if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
    {
      e.Handled = true;
      this.PdfControl.ScrollViewer.ScrollToHorizontalOffset(this.PdfControl.ScrollViewer.HorizontalOffset + (!(e is MouseTiltEventArgs mouseTiltEventArgs) ? (double) (-e.Delta / 2) : (double) (mouseTiltEventArgs.Delta / 2)));
    }
    if (e.Handled || this.PdfControl.ScrollViewer.VerticalOffset >= 1.0)
      return;
    this.VM?.AnnotationToolbar?.UpdateViewerToobarPadding();
  }

  private void PdfViewerScrollViewer_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
  {
    if (e.IsInertial)
      return;
    Vector scale = e.DeltaManipulation.Scale;
    if (scale.X == 1.0 && scale.Y == 1.0)
      return;
    e.Handled = true;
    this.VM.ViewToolbar.UpdateDocToZoom((float) ((double) this.VM.ViewToolbar.DocZoom + scale.Length - 1.4142135623730951), true, new Point?(e.ManipulationOrigin));
  }

  private void PageGridView_ItemDoubleClick(object sender, PdfPagePreviewGridViewItemEventArgs e)
  {
    if (!(e.Item is PdfPageEditListModel pageEditListModel))
      return;
    this.VM.SelectedPageIndex = pageEditListModel.PageIndex;
    this.Menus_SelectItem("View");
  }

  private async void PageGridView_ItemsDragStart(
    object sender,
    PdfPagePreviewGridViewItemDragStartEventArgs e)
  {
    if (!(e.DragContainer.DataContext is PdfPageEditListModel dataContext))
      return;
    WriteableBitmap pdfBitmapAsync = await Ioc.Default.GetRequiredService<PdfThumbnailService>().TryGetPdfBitmapAsync(dataContext.Document.Pages[dataContext.PageIndex], Colors.White, PageRotate.Normal, 0, 100, new CancellationToken());
    double width = pdfBitmapAsync.Width;
    double height = pdfBitmapAsync.Height;
    int num = Math.Min(e.DragItems.Length, 3);
    Grid grid = new Grid();
    for (int index = 0; index < num; ++index)
    {
      Border border = new Border();
      border.Width = width;
      border.Height = height;
      border.Background = (Brush) new SolidColorBrush(Colors.White);
      border.BorderBrush = (Brush) new SolidColorBrush(Colors.Black);
      border.BorderThickness = new Thickness(1.0);
      border.Margin = new Thickness((double) (index * 10), (double) (index * 10), 0.0, 0.0);
      Border element = border;
      if (index == num - 1)
        element.Child = (UIElement) new Image()
        {
          Source = (ImageSource) pdfBitmapAsync
        };
      grid.Children.Add((UIElement) element);
    }
    e.UIOverride = (object) grid;
  }

  private void PageGridView_ItemsDragCompleted(
    object sender,
    PdfPagePreviewGridViewItemDragCompletedEventArgs e)
  {
    if (this.VM?.PageEditors == null || !e.Reordered)
      return;
    MainViewModel vm = this.VM;
    if (vm == null)
      return;
    PageEditorViewModel pageEditors = vm.PageEditors;
    if (pageEditors == null)
      return;
    PdfPageEditListModel beforeItem = e.BeforeItem as PdfPageEditListModel;
    PdfPageEditListModel afterItem = e.AfterItem as PdfPageEditListModel;
    object[] dragItems = e.DragItems;
    PdfPageEditListModel[] array = dragItems != null ? dragItems.OfType<PdfPageEditListModel>().ToArray<PdfPageEditListModel>() : (PdfPageEditListModel[]) null;
    pageEditors.ReorderPages(beforeItem, afterItem, array);
  }

  private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
  {
    this.VM?.AnnotationToolbar?.UpdateViewerToobarPadding();
  }

  private void BookmarkExpandAll_Click(object sender, RoutedEventArgs e)
  {
    this.bookmarkControl.ExpandAll();
  }

  private void BookmarkCollapseAll_Click(object sender, RoutedEventArgs e)
  {
    this.bookmarkControl.CollapseAll();
  }

  private void bookmarkControl_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key == Key.Delete)
    {
      e.Handled = true;
      this.VM.BookmarkRemoveCommand.Execute((BookmarkModel) null);
    }
    else
    {
      if (e.Key != Key.Insert)
        return;
      e.Handled = true;
      this.VM.BookmarkAddCommand.Execute((BookmarkModel) null);
    }
  }

  private void bookmarkControl_ContextMenuOpening(object sender, ContextMenuEventArgs e)
  {
    this.bookmarkContextMenuData = !((e.OriginalSource is FrameworkElement originalSource ? originalSource.DataContext : (object) null) is BookmarkModel dataContext) ? (BookmarkModel) null : dataContext;
    if (this.bookmarkContextMenuData != null)
    {
      this.BookmarkMenuSeparator.Visibility = Visibility.Visible;
      this.AddChildBookmarkMenuItem.Visibility = Visibility.Visible;
      this.EditBookmarkMenuItem.Visibility = Visibility.Visible;
      this.DeleteBookmarkMenuItem.Visibility = Visibility.Visible;
    }
    else
    {
      this.BookmarkMenuSeparator.Visibility = Visibility.Collapsed;
      this.AddChildBookmarkMenuItem.Visibility = Visibility.Collapsed;
      this.EditBookmarkMenuItem.Visibility = Visibility.Collapsed;
      this.DeleteBookmarkMenuItem.Visibility = Visibility.Collapsed;
    }
  }

  private async void BookmarkContextMenuAddBookmark_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("Bookmark", "AddBookmark", "BookmarkContextMenu", 1L);
    AsyncRelayCommand<BookmarkModel> bookmarkAddCommand = this.VM.BookmarkAddCommand;
    BookmarkModel parameter = this.bookmarkContextMenuData;
    if (parameter == null)
    {
      BookmarkModel bookmarks = this.VM.Bookmarks;
      parameter = bookmarks != null ? bookmarks.Children.LastOrDefault<BookmarkModel>() : (BookmarkModel) null;
    }
    await bookmarkAddCommand.ExecuteAsync(parameter);
  }

  private async void BookmarkContextMenuAddChidBookmark_Click(object sender, RoutedEventArgs e)
  {
    BookmarkModel bookmarkContextMenuData = this.bookmarkContextMenuData;
    if (bookmarkContextMenuData == null)
      return;
    await this.VM.BookmarkAddChildCommand.ExecuteAsync(bookmarkContextMenuData);
  }

  private async void BookmarkContextMenuDeleteBookmark_Click(object sender, RoutedEventArgs e)
  {
    BookmarkModel bookmarkContextMenuData = this.bookmarkContextMenuData;
    if (bookmarkContextMenuData == null)
      return;
    await this.VM.BookmarkRemoveCommand.ExecuteAsync(bookmarkContextMenuData);
  }

  private void BookmarkContextMenuRenameBookmark_Click(object sender, RoutedEventArgs e)
  {
    BookmarkModel bookmarkContextMenuData = this.bookmarkContextMenuData;
    if (bookmarkContextMenuData == null)
      return;
    CommomLib.Commom.GAManager.SendEvent("Bookmark", "RenameBookmark", "BookmarkContextMenu", 1L);
    BookmarkRenameDialog.Create(bookmarkContextMenuData).ShowDialog();
  }

  private void WrapBookmarkMenuItem_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("Bookmark", "RenameBookmark", "BookmarkContextMenu", 1L);
    MenuItem menuItem = (MenuItem) sender;
    this.Dispatcher.InvokeAsync((Action) (() =>
    {
      ConfigManager.SetBookmarkWrapFlag(menuItem.IsChecked);
      this.UpdateBookmarkWrapMode();
    }), DispatcherPriority.Loaded);
  }

  private void UpdateBookmarkWrapMode()
  {
    bool bookmarkWrapFlag = ConfigManager.GetBookmarkWrapFlag();
    this.WrapBookmarkMenuItem.IsChecked = bookmarkWrapFlag;
    ScrollViewer.SetHorizontalScrollBarVisibility((DependencyObject) this.bookmarkControl, bookmarkWrapFlag ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto);
  }

  private void PdfControl_EditorUndoStateChanged(object sender, EventArgs e)
  {
    this.VM.UpdateCanSaveFlagState();
  }

  private void ChatButton_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("MainWindowChatButton", "Click", "Count", 1L);
    CommomLib.Commom.GAManager.SendEvent("ChatPdf", "ChatAgainButton", "Count", 1L);
    this.VM.ChatPanelVisible = true;
    this.ChatPanel.FocusUserInputTextBox();
    ConfigManager.SetChatPanelClosed(false);
  }

  private void ChatPanel_CloseButtonClick(object sender, EventArgs e)
  {
    if (!ConfigManager.GetChatPanelFirstClose())
    {
      ConfigManager.SetChatPanelFirstClose(true);
      this.ChatButton.ShowTips();
      this.RightNavigationView.IsAnimationEnabled = false;
    }
    this.VM.ChatPanelVisible = false;
    this.RightNavigationView.IsAnimationEnabled = true;
    ConfigManager.SetChatPanelClosed(true);
  }

  private void BackgroundModeButton_Click(object sender, RoutedEventArgs e)
  {
    BackgroundModel result = ConfigManager.GetBackgroundModelAsync(new CancellationToken()).GetAwaiter().GetResult();
    if (result == null)
    {
      this.PaperColorListBox.SelectedIndex = 0;
      this.BackgroundModeMenu.IsOpen = true;
    }
    else
    {
      for (int index = 0; index < MainView.viewerBackgroundColorValues.Length; ++index)
      {
        if (result.BackgroundName == MainView.viewerBackgroundColorValues[index].Name)
        {
          this.PaperColorListBox.SelectedIndex = index;
          this.BackgroundModeMenu.IsOpen = true;
          return;
        }
      }
      this.PaperColorListBox.SelectedIndex = 0;
      this.BackgroundModeMenu.IsOpen = true;
    }
  }

  private void InitViewerThemeValues()
  {
    ListBoxItem newItem1 = new ListBoxItem();
    newItem1.Content = (object) pdfeditor.Properties.Resources.ToolAppThemeAutoMode;
    newItem1.Tag = (object) "Auto";
    ListBoxItem newItem2 = new ListBoxItem();
    newItem2.Content = (object) pdfeditor.Properties.Resources.ToolAppThemeLightMode;
    newItem2.Tag = (object) "Light";
    ListBoxItem newItem3 = new ListBoxItem();
    newItem3.Content = (object) pdfeditor.Properties.Resources.ToolAppThemeDarkMode;
    newItem3.Tag = (object) "Dark";
    this.ThemeListBox.Items.Add((object) newItem1);
    this.ThemeListBox.Items.Add((object) newItem2);
    this.ThemeListBox.Items.Add((object) newItem3);
    this.UpdateViewerThemeValues();
  }

  internal void UpdateViewerThemeValues()
  {
    switch (ConfigManager.GetCurrentAppTheme())
    {
      case "Auto":
        this.ThemeListBox.SelectedIndex = 0;
        break;
      case "Light":
        this.ThemeListBox.SelectedIndex = 1;
        break;
      default:
        this.ThemeListBox.SelectedIndex = 2;
        break;
    }
  }

  private void InitViewerBackgroundColorValues()
  {
    MainView.viewerBackgroundColorValues = new BackgroundColorSetting[5]
    {
      new BackgroundColorSetting("Default", "", pdfeditor.Properties.Resources.WinViewToolBackgroundDefaultText, App.Current.GetCurrentActualAppTheme() == "Dark" ? "#444444" : "#E2E2E2", "#00FFFFFF"),
      new BackgroundColorSetting("DayMode", "", pdfeditor.Properties.Resources.WinViewToolBackgroundDayModeText, "#FCFCFC", "#00FFFFFF"),
      new BackgroundColorSetting("NightMode", "", pdfeditor.Properties.Resources.WinViewToolBackgroundNightModeText, "#CECECE", "#400F0F0F"),
      new BackgroundColorSetting("EyeProtectionMode", "", pdfeditor.Properties.Resources.WinViewToolBackgroundEyeProtectionModeText, "#D2E2C8", "#404B7430"),
      new BackgroundColorSetting("YellowBackground", "", pdfeditor.Properties.Resources.WinViewToolBackgroundYellowBackgroundText, "#E4DDC4", "#40775F13")
    };
    MainView.viewerBackgroundColorDict = (IReadOnlyDictionary<string, BackgroundColorSetting>) ((IEnumerable<BackgroundColorSetting>) MainView.viewerBackgroundColorValues).ToDictionary<BackgroundColorSetting, string, BackgroundColorSetting>((Func<BackgroundColorSetting, string>) (c => c.Name), (Func<BackgroundColorSetting, BackgroundColorSetting>) (c => c));
    foreach (BackgroundColorSetting backgroundColorValue in MainView.viewerBackgroundColorValues)
    {
      ListBoxItem newItem = new ListBoxItem();
      newItem.Content = (object) backgroundColorValue.DisplayName;
      newItem.Tag = (object) backgroundColorValue.Name;
      this.PaperColorListBox.Items.Add((object) newItem);
    }
    BackgroundModel result = ConfigManager.GetBackgroundModelAsync(new CancellationToken()).GetAwaiter().GetResult();
    if (result == null)
    {
      this.PaperColorListBox.SelectedIndex = 0;
    }
    else
    {
      for (int index = 0; index < MainView.viewerBackgroundColorValues.Length; ++index)
      {
        if (result.BackgroundName == MainView.viewerBackgroundColorValues[index].Name)
        {
          this.PaperColorListBox.SelectedIndex = index;
          return;
        }
      }
      this.PaperColorListBox.SelectedIndex = 0;
    }
  }

  private void DoBackgroundMenuItemCmd()
  {
    if (this.PaperColorListBox == null || this.PaperColorListBox.SelectedIndex == -1)
      return;
    BackgroundColorSetting backgroundColorValue = MainView.viewerBackgroundColorValues[this.PaperColorListBox.SelectedIndex];
    if (backgroundColorValue != null && backgroundColorValue.Name == "Default")
    {
      string str = "#E2E2E2";
      if (App.Current.GetCurrentActualAppTheme() == "Dark")
        str = "#444444";
      backgroundColorValue.BackgroundColor = (Color) ColorConverter.ConvertFromString(str);
    }
    this.TryUpdateViewerBackground();
    BackgroundColorSetting setting = MainView.viewerBackgroundColorValues[this.PaperColorListBox.SelectedIndex];
    if (setting == null)
      return;
    ConfigManager.SetBackgroundAsync(setting.Name, setting.PageMaskColor.ToString(), setting.BackgroundColor.ToString());
    if (this.VM.ViewToolbar == null)
      return;
    IContextMenuModel contextMenuModel = this.VM.ViewToolbar.BackgroundMenuItems.ToList<IContextMenuModel>().Find((Predicate<IContextMenuModel>) (x => x.Name.Equals(setting.Name)));
    if (contextMenuModel == null || !(contextMenuModel is BackgroundContextMenuItemModel contextMenuItemModel))
      return;
    contextMenuItemModel.IsChecked = true;
  }

  public void TryUpdateViewerBackground()
  {
    if (this.VM.Document == null)
      return;
    BackgroundColorSetting backgroundColorValue = MainView.viewerBackgroundColorValues[this.PaperColorListBox.SelectedIndex];
    if (backgroundColorValue == null)
      return;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.VM.Document);
    if (pdfControl == null)
      return;
    pdfControl.PageMaskBrush = (Brush) new SolidColorBrush(backgroundColorValue.PageMaskColor);
    pdfControl.PageBackground = (Brush) new SolidColorBrush(backgroundColorValue.BackgroundColor);
  }

  private void PaperColorListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.DoBackgroundMenuItemCmd();
    this.BackgroundModeMenu.IsOpen = false;
  }

  private void ThemeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (this.ThemeListBox == null || this.ThemeListBox.SelectedIndex == -1)
      return;
    switch ((e.AddedItems[0] as ListBoxItem).Tag.ToString())
    {
      case "Light":
        ConfigManager.SetCurrentAppTheme("Light");
        break;
      case "Dark":
        ConfigManager.SetCurrentAppTheme("Dark");
        break;
      default:
        ConfigManager.SetCurrentAppTheme("Auto");
        SystemThemeListener systemThemeListener = new SystemThemeListener(this.Dispatcher);
        break;
    }
    ThemeResourceDictionary.GetForCurrentApp().Theme = App.Current.GetCurrentActualAppTheme();
    BackgroundColorSetting backgroundColorValue = MainView.viewerBackgroundColorValues[this.PaperColorListBox.SelectedIndex];
    if (backgroundColorValue != null && backgroundColorValue.Name == "Default")
      this.DoBackgroundMenuItemCmd();
    ProcessMessageHelper.SendMessageAsync(0UL, "UpdateTheme");
    this.BackgroundModeMenu.IsOpen = false;
  }

  private void ThemeListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    DependencyObject reference = (DependencyObject) e.OriginalSource;
    while (true)
    {
      switch (reference)
      {
        case null:
        case ListBoxItem _:
          goto label_3;
        default:
          reference = VisualTreeHelper.GetParent(reference);
          continue;
      }
    }
label_3:
    if (reference is ListBoxItem listBoxItem1 && listBoxItem1.IsSelected)
      this.BackgroundModeMenu.IsOpen = false;
    if (!(reference is ListBoxItem listBoxItem2))
      return;
    GA4Manager.SendEvent("MainWindow", "UpdateTheme", listBoxItem2.Tag.ToString(), 1L);
  }

  private void MenuHeaderContainer_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    if (e.NewSize.Width > 0.0 && this.menuHeaderMaxWidth == 0.0)
    {
      MainView.SetIsHeaderMenuCompactModeEnabled((DependencyObject) this.MenuHeaderScrollViewer, true);
      this.MenuHeaderScrollViewer.InvalidateMeasure();
      this.MenuHeaderScrollViewer.Measure(new Size(double.MaxValue, e.NewSize.Height));
      this.menuHeaderMaxWidth = this.MenuHeaderScrollViewer.DesiredSize.Width;
    }
    double menuHeaderMaxWidth = this.menuHeaderMaxWidth;
    MainView.SetIsHeaderMenuCompactModeEnabled((DependencyObject) this.MenuHeaderScrollViewer, e.NewSize.Width <= menuHeaderMaxWidth);
  }

  public static bool GetIsHeaderMenuCompactModeEnabled(DependencyObject obj)
  {
    return (bool) obj.GetValue(MainView.IsHeaderMenuCompactModeEnabledProperty);
  }

  public static void SetIsHeaderMenuCompactModeEnabled(DependencyObject obj, bool value)
  {
    obj.SetValue(MainView.IsHeaderMenuCompactModeEnabledProperty, (object) value);
  }

  private void SelectTimer_Elapsed(object sender, ElapsedEventArgs e)
  {
    Application.Current.Dispatcher.Invoke((Action) (() =>
    {
      this.AnnotationFilterbtn.IsChecked = new bool?(false);
      this.UsersFilterbtn.IsChecked = new bool?(true);
      this.selectTimer.Dispose();
    }));
  }

  private void SelectTimer_Elapsed2(object sender, ElapsedEventArgs e)
  {
    Application.Current.Dispatcher.Invoke((Action) (() =>
    {
      this.UsersFilterbtn.IsChecked = new bool?(false);
      this.AnnotationFilterbtn.IsChecked = new bool?(true);
      this.selectTimer.Dispose();
    }));
  }

  private void UsersFilterbtn_MouseEnter(object sender, MouseEventArgs e)
  {
    if ((sender as ToggleButton).IsChecked.GetValueOrDefault())
      return;
    this.selectTimer = new System.Timers.Timer(500.0);
    this.selectTimer.Elapsed += new ElapsedEventHandler(this.SelectTimer_Elapsed);
    this.selectTimer.Start();
  }

  private void UsersFilterbtn_MouseLeave(object sender, MouseEventArgs e)
  {
    this.selectTimer?.Stop();
    this.selectTimer.Dispose();
  }

  private void AnnotationFilterbtn_MouseEnter(object sender, MouseEventArgs e)
  {
    if ((sender as ToggleButton).IsChecked.GetValueOrDefault())
      return;
    this.selectTimer = new System.Timers.Timer(500.0);
    this.selectTimer.Elapsed += new ElapsedEventHandler(this.SelectTimer_Elapsed2);
    this.selectTimer.Start();
  }

  private void UsersFilterbtn_Checked(object sender, RoutedEventArgs e)
  {
    this.AnnotationFilterbtn.IsChecked = new bool?(false);
  }

  private void AnnotationFilterbtn_Checked(object sender, RoutedEventArgs e)
  {
    this.UsersFilterbtn.IsChecked = new bool?(false);
  }

  private void ExpandCollapseBookmarkMenuItem_Click(object sender, RoutedEventArgs e)
  {
    this.Dispatcher.InvokeAsync((Action) (() =>
    {
      if (this.bookmarkControl.CheckIfCollapseAll())
        this.bookmarkControl.CollapseAll();
      else
        this.bookmarkControl.ExpandAll();
    }), DispatcherPriority.Loaded);
  }

  private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    if (e.RightButton != MouseButtonState.Pressed)
      return;
    PdfPagePreviewListViewItem parent = VisualHelper.GetParent<PdfPagePreviewListViewItem>((DependencyObject) sender);
    if (parent == null || !parent.IsSelected)
      return;
    e.Handled = true;
  }

  private void EnterOrExitFullScreen()
  {
    this.Dispatcher.BeginInvoke(DispatcherPriority.Render, (Delegate) (() =>
    {
      bool fullScreenEnabled = FullScreenHelper.GetIsFullScreenEnabled((Window) this);
      this.SetFooterVisible(!fullScreenEnabled);
      this.VM.Menus.IsShowToolbar = !fullScreenEnabled;
      if (this.VM.Menus.IsShowToolbar)
        this.FullScreenButton.ToolTip = (object) pdfeditor.Properties.Resources.HeaderToolFullScreenTips;
      else
        this.FullScreenButton.ToolTip = (object) pdfeditor.Properties.Resources.HeaderToolExitFullScreenTips;
    }));
  }

  private void ZoomCombobox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!(sender is ComboBox comboBox))
      return;
    ComboBoxItem parent = VisualHelper.GetParent<ComboBoxItem>((DependencyObject) e.OriginalSource);
    if (parent == null || parent.Content != comboBox.SelectedValue || !(comboBox.SelectedValue.ToString() != comboBox.Text))
      return;
    this.UpdateDocZoom(comboBox.SelectedValue.ToString());
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/views/mainview.xaml", UriKind.Relative));
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
      case 2:
        this.TitlebarRow = (RowDefinition) target;
        break;
      case 3:
        this.TitlebarPlaceholder = (Grid) target;
        break;
      case 4:
        this.QuickToolHeaderRow = (RowDefinition) target;
        break;
      case 5:
        this.MenuRow = (RowDefinition) target;
        break;
      case 6:
        this.MainRow = (RowDefinition) target;
        break;
      case 7:
        this.FooterRow = (RowDefinition) target;
        break;
      case 8:
        this.HeaderContainer = (Grid) target;
        break;
      case 9:
        this.QuickToolContainer = (Grid) target;
        break;
      case 10:
        this.NewPDFBtn = (Button) target;
        break;
      case 11:
        this.openBtn = (Button) target;
        break;
      case 12:
        this.saveBtn = (Button) target;
        break;
      case 13:
        this.saveAsBtn = (Button) target;
        break;
      case 14:
        this.printBtn = (Button) target;
        break;
      case 15:
        this.undoBtn = (Button) target;
        break;
      case 16 /*0x10*/:
        this.redoBtn = (Button) target;
        break;
      case 17:
        this.MenuHeaderContainer = (Grid) target;
        this.MenuHeaderContainer.SizeChanged += new SizeChangedEventHandler(this.MenuHeaderContainer_SizeChanged);
        break;
      case 18:
        this.MenuHeaderScrollViewer = (ScrollViewer) target;
        this.MenuHeaderScrollViewer.PreviewMouseWheel += new MouseWheelEventHandler(this.MenuScrollViewer_PreviewMouseWheel);
        break;
      case 19:
        this.Menus = (ListBox) target;
        this.Menus.SelectionChanged += new SelectionChangedEventHandler(this.Menus_SelectionChanged);
        break;
      case 20:
        this.menuFeedBack = (Grid) target;
        break;
      case 21:
        this.menuBackgroundMode = (Grid) target;
        break;
      case 22:
        this.BackgroundModeButton = (Button) target;
        this.BackgroundModeButton.Click += new RoutedEventHandler(this.BackgroundModeButton_Click);
        break;
      case 23:
        this.BackgroundModeMenu = (Popup) target;
        break;
      case 24:
        this.ThemeListBox = (ListBox) target;
        this.ThemeListBox.SelectionChanged += new SelectionChangedEventHandler(this.ThemeListBox_SelectionChanged);
        this.ThemeListBox.PreviewMouseDown += new MouseButtonEventHandler(this.ThemeListBox_PreviewMouseDown);
        break;
      case 25:
        this.PaperColorListBox = (ListBox) target;
        this.PaperColorListBox.PreviewMouseDown += new MouseButtonEventHandler(this.ThemeListBox_PreviewMouseDown);
        this.PaperColorListBox.SelectionChanged += new SelectionChangedEventHandler(this.PaperColorListBox_SelectionChanged);
        break;
      case 26:
        this.ShareFile = (Grid) target;
        break;
      case 27:
        this.ShareButton = (ToolbarButton) target;
        break;
      case 28:
        this.menuFullScreen = (Grid) target;
        break;
      case 29:
        this.FullScreenButton = (ToolbarToggleButton) target;
        break;
      case 30:
        this.menuToolbarShow = (Grid) target;
        break;
      case 31 /*0x1F*/:
        this.MenuContainer = (Grid) target;
        break;
      case 32 /*0x20*/:
        this.MenuScrollLeftMask = (Grid) target;
        break;
      case 33:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.MenuNavigationButton_Click);
        break;
      case 34:
        this.MenuScrollRightMask = (Grid) target;
        break;
      case 35:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.MenuNavigationButton_Click);
        break;
      case 36:
        this.MenuScrollViewer = (ScrollViewer) target;
        this.MenuScrollViewer.PreviewMouseWheel += new MouseWheelEventHandler(this.MenuScrollViewer_PreviewMouseWheel);
        this.MenuScrollViewer.ScrollChanged += new ScrollChangedEventHandler(this.MenuScrollViewer_ScrollChanged);
        break;
      case 37:
        this.menuView = (Grid) target;
        break;
      case 38:
        this.ExitAnnotPanel1 = (StackPanel) target;
        break;
      case 39:
        this.ZoomSpeace = (Grid) target;
        break;
      case 40:
        this.ZoomCombobox = (ComboBox) target;
        this.ZoomCombobox.SelectionChanged += new SelectionChangedEventHandler(this.ZoomCB_SelectionChanged);
        this.ZoomCombobox.KeyUp += new KeyEventHandler(this.ZoomCB_KeyUp);
        this.ZoomCombobox.LostFocus += new RoutedEventHandler(this.ZoomCB_LostFocus);
        this.ZoomCombobox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.ZoomCombobox_PreviewMouseLeftButtonDown);
        break;
      case 41:
        this.autoScroll = (ToolbarToggleButton) target;
        break;
      case 42:
        this.presentBtn = (ToolbarButton) target;
        break;
      case 43:
        this.screenshotBtn = (ToolbarRadioButton) target;
        break;
      case 44:
        this.ocrBtn = (ToolbarRadioButton) target;
        break;
      case 45:
        this.cropPageBtn = (ToolbarRadioButton) target;
        break;
      case 46:
        this.menuAnnotation = (Grid) target;
        break;
      case 47:
        this.AnnonationButtons = (StackPanel) target;
        break;
      case 48 /*0x30*/:
        this.ExitAnnotPanel2 = (StackPanel) target;
        break;
      case 49:
        this.highlight = (ContentPresenter) target;
        break;
      case 50:
        this.underline = (ContentPresenter) target;
        break;
      case 51:
        this.strike = (ContentPresenter) target;
        break;
      case 52:
        this.highlightarea = (ContentPresenter) target;
        break;
      case 53:
        this.line = (ContentPresenter) target;
        break;
      case 54:
        this.shape = (ContentPresenter) target;
        break;
      case 55:
        this.ellipse = (ContentPresenter) target;
        break;
      case 56:
        this.ink = (ContentPresenter) target;
        break;
      case 57:
        this.textbox = (ContentPresenter) target;
        break;
      case 58:
        this.text = (ContentPresenter) target;
        break;
      case 59:
        this.note = (ContentPresenter) target;
        break;
      case 60:
        this.stamp = (ToolbarButton) target;
        break;
      case 61:
        this.HideAnnotationButton = (ToolbarButton) target;
        break;
      case 62:
        this.ShowAnnotationButton = (ToolbarButton) target;
        break;
      case 63 /*0x3F*/:
        this.ManageAnnotationButton = (ToolbarButton) target;
        break;
      case 64 /*0x40*/:
        this.MenuFillForm = (Grid) target;
        break;
      case 65:
        this.text2 = (ContentPresenter) target;
        break;
      case 66:
        this.image2 = (ToolbarButton) target;
        break;
      case 67:
        this.signature3 = (ToolbarButton) target;
        break;
      case 68:
        this.menuInsert = (Grid) target;
        break;
      case 69:
        this.editDocumentBtn = (ToolbarButton) target;
        break;
      case 70:
        this.editContentBtn = (ToolbarToggleButton) target;
        break;
      case 71:
        this.text3 = (ContentPresenter) target;
        break;
      case 72:
        this.image = (ToolbarButton) target;
        break;
      case 73:
        this.link = (ToolbarRadioButton) target;
        break;
      case 74:
        this.watermark = (ToolbarButton) target;
        break;
      case 75:
        this.stamp1 = (ToolbarButton) target;
        break;
      case 76:
        this.signature = (ToolbarButton) target;
        break;
      case 77:
        this.menuTools = (Grid) target;
        break;
      case 78:
        this.converter = (ToolbarButton) target;
        break;
      case 79:
        this.CompressBtn = (ToolbarButton) target;
        break;
      case 80 /*0x50*/:
        this.signature2 = (ToolbarButton) target;
        break;
      case 81:
        this.Speech = (ToolbarToggleButton) target;
        break;
      case 82:
        this.menuPage = (Grid) target;
        break;
      case 83:
        this.menuEncrypt = (Grid) target;
        break;
      case 84:
        this.menuShare = (Grid) target;
        break;
      case 85:
        this.menuHelp = (Grid) target;
        break;
      case 86:
        this.PdfContentContainer = (Grid) target;
        break;
      case 87:
        this.LeftNavigationView = (NavigationView) target;
        break;
      case 88:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.BookmarkExpandAll_Click);
        break;
      case 89:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.BookmarkCollapseAll_Click);
        break;
      case 90:
        this.bookmarkControl = (BookmarkControl) target;
        this.bookmarkControl.AddHandler(ContextMenuService.ContextMenuOpeningEvent, (Delegate) new ContextMenuEventHandler(this.bookmarkControl_ContextMenuOpening));
        break;
      case 91:
        ((MenuItem) target).Click += new RoutedEventHandler(this.BookmarkContextMenuAddBookmark_Click);
        break;
      case 92:
        this.AddChildBookmarkMenuItem = (MenuItem) target;
        this.AddChildBookmarkMenuItem.Click += new RoutedEventHandler(this.BookmarkContextMenuAddChidBookmark_Click);
        break;
      case 93:
        this.BookmarkMenuSeparator = (Separator) target;
        break;
      case 94:
        this.DeleteBookmarkMenuItem = (MenuItem) target;
        this.DeleteBookmarkMenuItem.Click += new RoutedEventHandler(this.BookmarkContextMenuDeleteBookmark_Click);
        break;
      case 95:
        this.EditBookmarkMenuItem = (MenuItem) target;
        this.EditBookmarkMenuItem.Click += new RoutedEventHandler(this.BookmarkContextMenuRenameBookmark_Click);
        break;
      case 96 /*0x60*/:
        this.BookmarkMenuSeparator2 = (Separator) target;
        break;
      case 97:
        this.WrapBookmarkMenuItem = (MenuItem) target;
        this.WrapBookmarkMenuItem.Click += new RoutedEventHandler(this.WrapBookmarkMenuItem_Click);
        break;
      case 98:
        this.ExpandCollapseBookmarkMenuItem = (MenuItem) target;
        this.ExpandCollapseBookmarkMenuItem.Click += new RoutedEventHandler(this.ExpandCollapseBookmarkMenuItem_Click);
        break;
      case 99:
        ((UIElement) target).PreviewMouseWheel += new MouseWheelEventHandler(this.MenuScrollViewer_PreviewMouseWheel);
        break;
      case 100:
        this.SidebarInsertBtn = (ToggleButton) target;
        break;
      case 101:
        this.Button_InsertBlank = (Button) target;
        break;
      case 102:
        this.Button_InsertPDF = (Button) target;
        break;
      case 103:
        this.Button_InsertWord = (Button) target;
        break;
      case 104:
        this.Button_InsertImage = (Button) target;
        break;
      case 105:
        this.ThumbnailList = (PdfPagePreviewListView) target;
        break;
      case 106:
        ((FrameworkElement) target).SizeChanged += new SizeChangedEventHandler(this.CommetMenuContainer_SizeChanged);
        break;
      case 107:
        this.CommetExpandButtonContainer = (StackPanel) target;
        break;
      case 108:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.CommetCollapseButton_Click);
        break;
      case 109:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.CommetExpandButton_Click);
        break;
      case 110:
        this.FilterBtn = (ToggleButton) target;
        break;
      case 111:
        this.filterpop = (Popup) target;
        break;
      case 112 /*0x70*/:
        this.UsersFilterbtn = (ToggleButton) target;
        this.UsersFilterbtn.MouseEnter += new MouseEventHandler(this.UsersFilterbtn_MouseEnter);
        this.UsersFilterbtn.MouseLeave += new MouseEventHandler(this.UsersFilterbtn_MouseLeave);
        this.UsersFilterbtn.Checked += new RoutedEventHandler(this.UsersFilterbtn_Checked);
        break;
      case 113:
        this.AnnotationFilterbtn = (ToggleButton) target;
        this.AnnotationFilterbtn.MouseEnter += new MouseEventHandler(this.AnnotationFilterbtn_MouseEnter);
        this.AnnotationFilterbtn.MouseLeave += new MouseEventHandler(this.UsersFilterbtn_MouseLeave);
        this.AnnotationFilterbtn.Checked += new RoutedEventHandler(this.AnnotationFilterbtn_Checked);
        break;
      case 114:
        this.BatchDeleteArea = (Border) target;
        break;
      case 115:
        this.CommetMenuControl = (CommetControl) target;
        break;
      case 116:
        this.ViewerContainer = (Grid) target;
        break;
      case 117:
        this.TextEditingBanner = (Grid) target;
        break;
      case 118:
        this.PdfControl = (PDFKit.PdfControl) target;
        this.PdfControl.PreviewMouseWheel += new MouseWheelEventHandler(this.ScrollViewer_PreviewMouseWheel);
        this.PdfControl.ScrollChanged += new ScrollChangedEventHandler(this.ScrollViewer_ScrollChanged);
        this.PdfControl.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(this.PdfViewerScrollViewer_ManipulationDelta);
        this.PdfControl.LostFocus += new RoutedEventHandler(this.Viewer_LostFocus);
        this.PdfControl.EditorUndoStateChanged += new EventHandler(this.PdfControl_EditorUndoStateChanged);
        break;
      case 119:
        this.AnnotationEditorCanvas = (AnnotationCanvas) target;
        break;
      case 120:
        this.AnnotToolbarSettingPanel = (ToolbarSettingPanel) target;
        break;
      case 121:
        this.ViewerConverterButtonContainer = (StackPanel) target;
        break;
      case 122:
        this.MouseOverPdfToWordButton = (AnimationExtentButton) target;
        break;
      case 123:
        this.MouseOverPdfToExcelButton = (AnimationExtentButton) target;
        break;
      case 124:
        this.MouseOverPdfToPPTButton = (AnimationExtentButton) target;
        break;
      case 125:
        this.MouseOverPdfToImageButton = (AnimationExtentButton) target;
        break;
      case 126:
        this.MouseOverPdfToJpegButton = (AnimationExtentButton) target;
        break;
      case (int) sbyte.MaxValue:
        this.SearchBox = (DocumentSearchBox) target;
        break;
      case 128 /*0x80*/:
        this.ChatButton = (ChatButton) target;
        break;
      case 129:
        this.RightNavigationView = (NavigationView) target;
        break;
      case 130:
        this.ChatPanel = (ChatPanel) target;
        break;
      case 131:
        this.FooterContainer = (Grid) target;
        break;
      case 132:
        this.FooterContainerHairline = (Rectangle) target;
        break;
      case 133:
        ((UIElement) target).KeyUp += new KeyEventHandler(this.ZoomCB_KeyUp);
        ((UIElement) target).GotFocus += new RoutedEventHandler(this.PageNum_GotFocus);
        ((UIElement) target).LostFocus += new RoutedEventHandler(this.ZoomCB_LostFocus);
        break;
      case 134:
        this.progressBar = (ProgressBar) target;
        break;
      case 135:
        this.lblsaveTime = (Label) target;
        break;
      case 136:
        this.menuFooterShow = (Grid) target;
        break;
      case 137:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.FooterShowHideButton_Click);
        break;
      case 138:
        this.PagesEditorContainer = (Grid) target;
        this.PagesEditorContainer.PreviewMouseWheel += new MouseWheelEventHandler(this.PagesEditorContainer_PreviewMouseWheel);
        break;
      case 139:
        this.PageGridView = (PdfPagePreviewGridView) target;
        break;
      case 141:
        this.PagesEditorFooterContainer = (Grid) target;
        break;
      case 142:
        this.PagesEditorCheckboxButton = (Button) target;
        this.PagesEditorCheckboxButton.Click += new RoutedEventHandler(this.PagesEditorCheckboxButton_Click);
        break;
      case 143:
        this.PagesEditorThumbnailScaleSlider = (Slider) target;
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
    if (connectionId != 1)
    {
      if (connectionId != 140)
        return;
      ((UIElement) target).PreviewMouseDown += new MouseButtonEventHandler(this.Border_PreviewMouseDown);
    }
    else
    {
      ((Style) target).Setters.Add((SetterBase) new EventSetter()
      {
        Event = Control.MouseDoubleClickEvent,
        Handler = (Delegate) new MouseButtonEventHandler(this.MenuGroup_MouseDoubleClick)
      });
      ((Style) target).Setters.Add((SetterBase) new EventSetter()
      {
        Event = UIElement.PreviewMouseDownEvent,
        Handler = (Delegate) new MouseButtonEventHandler(this.MenuGroup_PreviewMouseDown)
      });
    }
  }
}
