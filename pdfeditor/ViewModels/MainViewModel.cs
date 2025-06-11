// Decompiled with JetBrains decompiler
// Type: pdfeditor.ViewModels.MainViewModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using CommomLib.Config.ConfigModels;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Microsoft.Win32;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Controls;
using pdfeditor.Controls.Annotations.Holders;
using pdfeditor.Controls.Printer;
using pdfeditor.Controls.Signature;
using pdfeditor.Controls.Speech;
using pdfeditor.Models;
using pdfeditor.Models.Bookmarks;
using pdfeditor.Models.Commets;
using pdfeditor.Models.LeftNavigations;
using pdfeditor.Models.Menus;
using pdfeditor.Models.Menus.ToolbarSettings;
using pdfeditor.Models.Operations;
using pdfeditor.Models.Thumbnails;
using pdfeditor.Models.Viewer;
using pdfeditor.Properties;
using pdfeditor.Utils;
using pdfeditor.Utils.Copilot;
using pdfeditor.Utils.Enums;
using pdfeditor.Views;
using PDFKit;
using PDFKit.Contents.Controls;
using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Media;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

#nullable enable
namespace pdfeditor.ViewModels;

public class MainViewModel : ObservableObject
{
  private 
  #nullable disable
  DocumentWrapper documentWrapper;
  private List<PdfThumbnailModel> thumbnailItemSource;
  private BookmarkModel bookmarks;
  private ObservableCollection<NavigationModel> leftNavList;
  private int selectedPageIndex;
  private DateTime time = DateTime.Now;
  public SpeechUtils speechUtils;
  public bool IsReading;
  public SpeechControl speechControl;
  public int ReadCulIndex = -1;
  public PdfPage LastViewPage;
  public SoundPlayer m_SoundPlayer;
  public pdfeditor.AutoSaveRestore.AutoSaveModel AutoSaveModel;
  public bool IsSaveBySignature;
  public int SignaturesCount;
  public const string AppTitle = "PDFgear";
  public string CurrentFileName = string.Empty;
  public bool Jumping;
  private QuickToolModel quickToolOpenModel;
  private QuickToolModel quickToolSaveModel;
  private QuickToolModel quickToolSaveAsModel;
  private QuickToolModel quickToolPrintModel;
  private QuickToolModel quickToolUndoModel;
  private QuickToolModel quickToolRedoModel;
  private DataOperationModel viewerOperationModel;
  private PdfViewJumpManager viewJumpManager = new PdfViewJumpManager();
  private ConverterCommands converterCmds;
  private ViewToolbarViewModel viewToolbarViewModel;
  private AnnotationToolbarViewModel annotationToolbarViewModel;
  private PageEditorViewModel pageEditorViewModel;
  private MenuViewModel menus;
  private ShareTabViewModel shareTabViewModel;
  private PageObjectType editingPageObjectType;
  private PdfAnnotation selectedAnnotation;
  private EnumBindingObject<MouseModes> viewerMouseMode = new EnumBindingObject<MouseModes>(MouseModes.Default);
  private EnumBindingObject<EditorMouseModes> editorMouseMode = new EnumBindingObject<EditorMouseModes>(EditorMouseModes.SelectParagraph);
  private AsyncRelayCommand openStartUpFileCmd;
  private string extraSaveOperationName;
  private OperationManager operationManager;
  private string version;
  private bool canSave;
  private string _password = "";
  private AllPageCommetCollectionView pageCommetSource;
  private CopilotHelper copilotHelper;
  private Thickness pdfToWordMargin = new Thickness(0.0, 0.0, 12.0, 12.0);
  private bool isDocumentOpened;
  private bool fillForm;
  private AllPageCommetCollectionView allpageCommetSource;
  private bool? isUserFilterAllChecked = new bool?(true);
  private int allCount;
  private bool? isFilterAllChecked = new bool?(true);
  private bool? isKindFilterAllChecked = new bool?(true);
  private RelayCommand userMannulCmd;
  private RelayCommand userGuideCmd;
  private RelayCommand getPhoneStoreCmd;
  private RelayCommand feedBackCmd;
  private RelayCommand autoSaveSettingCmd;
  private RelayCommand upgradeCmd;
  private RelayCommand aboutCmd;
  private RelayCommand propertiesCmd;
  private RelayCommand updateCmd;
  private AsyncRelayCommand settingsCmd;
  private AsyncRelayCommand printDocCmd;
  private AsyncRelayCommand batchPrintCmd;
  private AsyncRelayCommand openDocCmd;
  private AsyncRelayCommand saveCmd;
  private RelayCommand encryptCMD;
  private RelayCommand removePasswordCMD;
  private AsyncRelayCommand saveAsCmd;
  private AsyncRelayCommand undoCmd;
  private AsyncRelayCommand redoCmd;
  private bool isAnnotationVisible = true;
  private RelayCommand exitAnnotationCmd;
  private AsyncRelayCommand showHideAnnotationCmd;
  private AsyncRelayCommand mannageAnnotationCmd;
  private AsyncRelayCommand deleteSelectedAnnotCmd;
  private bool isDeleteAreaVisible;
  private bool? isSelectedAll = new bool?(false);
  private AsyncRelayCommand canceldeleteAnnotCmd;
  private AsyncRelayCommand selectAllAnnotCmd;
  private AsyncRelayCommand batchdeleteAnnotCmd;
  private AsyncRelayCommand deleteAnnotCmd;
  private bool documentClosing;
  private RelayCommand openImgCmd;
  private AsyncRelayCommand<BookmarkModel> bookmarkAddCommand;
  private AsyncRelayCommand<BookmarkModel> bookmarkAddCommand2;
  private AsyncRelayCommand<BookmarkModel> bookmarkAddChildCommand;
  private AsyncRelayCommand<BookmarkModel> bookmarkRemoveCommand;
  private BookmarkModel selectedBookmark;
  private bool chatButtonVisible;
  private bool chatPanelVisible;

  public QuickToolModel QuickToolOpenModel
  {
    get
    {
      QuickToolModel quickToolOpenModel1 = this.quickToolOpenModel;
      if (quickToolOpenModel1 != null)
        return quickToolOpenModel1;
      QuickToolModel quickToolModel = new QuickToolModel();
      quickToolModel.IsVisible = true;
      quickToolModel.Command = (ICommand) this.OpenDocCmd;
      QuickToolModel quickToolOpenModel2 = quickToolModel;
      this.quickToolOpenModel = quickToolModel;
      return quickToolOpenModel2;
    }
  }

  public QuickToolModel QuickToolSaveModel
  {
    get
    {
      QuickToolModel quickToolSaveModel1 = this.quickToolSaveModel;
      if (quickToolSaveModel1 != null)
        return quickToolSaveModel1;
      QuickToolModel quickToolModel = new QuickToolModel();
      quickToolModel.IsVisible = true;
      quickToolModel.Command = (ICommand) this.SaveCmd;
      QuickToolModel quickToolSaveModel2 = quickToolModel;
      this.quickToolSaveModel = quickToolModel;
      return quickToolSaveModel2;
    }
  }

  public QuickToolModel QuickToolSaveAsModel
  {
    get
    {
      QuickToolModel quickToolSaveAsModel1 = this.quickToolSaveAsModel;
      if (quickToolSaveAsModel1 != null)
        return quickToolSaveAsModel1;
      QuickToolModel quickToolModel = new QuickToolModel();
      quickToolModel.IsVisible = true;
      quickToolModel.Command = (ICommand) this.SaveAsCmd;
      QuickToolModel quickToolSaveAsModel2 = quickToolModel;
      this.quickToolSaveAsModel = quickToolModel;
      return quickToolSaveAsModel2;
    }
  }

  public QuickToolModel QuickToolPrintModel
  {
    get
    {
      QuickToolModel quickToolPrintModel1 = this.quickToolPrintModel;
      if (quickToolPrintModel1 != null)
        return quickToolPrintModel1;
      QuickToolModel quickToolModel = new QuickToolModel();
      quickToolModel.IsVisible = true;
      quickToolModel.Command = (ICommand) this.PrintDocCmd;
      QuickToolModel quickToolPrintModel2 = quickToolModel;
      this.quickToolPrintModel = quickToolModel;
      return quickToolPrintModel2;
    }
  }

  public QuickToolModel QuickToolUndoModel
  {
    get
    {
      QuickToolModel quickToolUndoModel1 = this.quickToolUndoModel;
      if (quickToolUndoModel1 != null)
        return quickToolUndoModel1;
      QuickToolModel quickToolModel = new QuickToolModel();
      quickToolModel.IsVisible = true;
      quickToolModel.Command = (ICommand) this.UndoCmd;
      QuickToolModel quickToolUndoModel2 = quickToolModel;
      this.quickToolUndoModel = quickToolModel;
      return quickToolUndoModel2;
    }
  }

  public QuickToolModel QuickToolRedoModel
  {
    get
    {
      QuickToolModel quickToolRedoModel1 = this.quickToolRedoModel;
      if (quickToolRedoModel1 != null)
        return quickToolRedoModel1;
      QuickToolModel quickToolModel = new QuickToolModel();
      quickToolModel.IsVisible = true;
      quickToolModel.Command = (ICommand) this.RedoCmd;
      QuickToolModel quickToolRedoModel2 = quickToolModel;
      this.quickToolRedoModel = quickToolModel;
      return quickToolRedoModel2;
    }
  }

  internal DataOperationModel ViewerOperationModel
  {
    get => this.viewerOperationModel;
    set
    {
      DataOperationModel viewerOperationModel = this.viewerOperationModel;
      if (!this.SetProperty<DataOperationModel>(ref this.viewerOperationModel, value, nameof (ViewerOperationModel)) || viewerOperationModel == null)
        return;
      viewerOperationModel.Dispose();
    }
  }

  public PdfViewJumpManager ViewJumpManager => this.viewJumpManager;

  public ConverterCommands ConverterCommands
  {
    get => this.converterCmds ?? (this.converterCmds = new ConverterCommands(this));
  }

  public ViewToolbarViewModel ViewToolbar
  {
    get => this.viewToolbarViewModel;
    private set
    {
      this.SetProperty<ViewToolbarViewModel>(ref this.viewToolbarViewModel, value, nameof (ViewToolbar));
    }
  }

  public AnnotationToolbarViewModel AnnotationToolbar
  {
    get => this.annotationToolbarViewModel;
    private set
    {
      this.SetProperty<AnnotationToolbarViewModel>(ref this.annotationToolbarViewModel, value, nameof (AnnotationToolbar));
    }
  }

  public MenuViewModel Menus => this.menus ?? (this.menus = new MenuViewModel(this));

  public PageEditorViewModel PageEditors
  {
    get => this.pageEditorViewModel;
    private set
    {
      this.SetProperty<PageEditorViewModel>(ref this.pageEditorViewModel, value, nameof (PageEditors));
    }
  }

  public ShareTabViewModel ShareTab
  {
    get => this.shareTabViewModel;
    private set
    {
      this.SetProperty<ShareTabViewModel>(ref this.shareTabViewModel, value, nameof (ShareTab));
    }
  }

  public AnnotationMode AnnotationMode
  {
    get
    {
      AnnotationToolbarViewModel annotationToolbar = this.AnnotationToolbar;
      AnnotationMode? nullable;
      if (annotationToolbar == null)
      {
        nullable = new AnnotationMode?();
      }
      else
      {
        System.Collections.Generic.IReadOnlyList<ToolbarAnnotationButtonModel> annotationButton = annotationToolbar.AllAnnotationButton;
        nullable = annotationButton != null ? annotationButton.FirstOrDefault<ToolbarAnnotationButtonModel>((Func<ToolbarAnnotationButtonModel, bool>) (c => c.IsChecked))?.Mode : new AnnotationMode?();
      }
      return nullable ?? AnnotationMode.None;
    }
    set
    {
      if (this.AnnotationToolbar == null)
        return;
      int annotationMode = (int) this.AnnotationMode;
      if (value == AnnotationMode.None || value == AnnotationMode.Stamp || value == AnnotationMode.Signature)
      {
        foreach (ToolbarAnnotationButtonModel annotationButtonModel in (IEnumerable<ToolbarAnnotationButtonModel>) this.AnnotationToolbar.AllAnnotationButton)
        {
          if (annotationButtonModel.ChildButtonModel is ToolbarChildCheckableButtonModel childButtonModel && childButtonModel.IsChecked)
            childButtonModel.IsChecked = false;
          if (annotationButtonModel.IsChecked)
            annotationButtonModel.IsChecked = false;
        }
      }
      if (value != AnnotationMode.None)
      {
        this.ExitTransientMode();
        ToolbarAnnotationButtonModel annotationButtonModel = this.AnnotationToolbar.AllAnnotationButton.FirstOrDefault<ToolbarAnnotationButtonModel>((Func<ToolbarAnnotationButtonModel, bool>) (c => c.Mode == value));
        if (annotationButtonModel != null && annotationButtonModel.IsCheckable)
        {
          if (!annotationButtonModel.IsChecked)
            annotationButtonModel.IsChecked = true;
        }
        else
          this.AnnotationMode = AnnotationMode.None;
        if (MainViewModel.\u003C\u003Eo__70.\u003C\u003Ep__2 == null)
          MainViewModel.\u003C\u003Eo__70.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MainViewModel), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        Func<CallSite, object, bool> target1 = MainViewModel.\u003C\u003Eo__70.\u003C\u003Ep__2.Target;
        CallSite<Func<CallSite, object, bool>> p2 = MainViewModel.\u003C\u003Eo__70.\u003C\u003Ep__2;
        if (MainViewModel.\u003C\u003Eo__70.\u003C\u003Ep__1 == null)
          MainViewModel.\u003C\u003Eo__70.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, MouseModes, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (MainViewModel), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        Func<CallSite, object, MouseModes, object> target2 = MainViewModel.\u003C\u003Eo__70.\u003C\u003Ep__1.Target;
        CallSite<Func<CallSite, object, MouseModes, object>> p1 = MainViewModel.\u003C\u003Eo__70.\u003C\u003Ep__1;
        if (MainViewModel.\u003C\u003Eo__70.\u003C\u003Ep__0 == null)
          MainViewModel.\u003C\u003Eo__70.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof (MainViewModel), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        object obj1 = MainViewModel.\u003C\u003Eo__70.\u003C\u003Ep__0.Target((CallSite) MainViewModel.\u003C\u003Eo__70.\u003C\u003Ep__0, this.ViewerMouseMode);
        object obj2 = target2((CallSite) p1, obj1, MouseModes.Default);
        if (target1((CallSite) p2, obj2))
        {
          if (MainViewModel.\u003C\u003Eo__70.\u003C\u003Ep__3 == null)
            MainViewModel.\u003C\u003Eo__70.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, MouseModes, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Value", typeof (MainViewModel), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          object obj3 = MainViewModel.\u003C\u003Eo__70.\u003C\u003Ep__3.Target((CallSite) MainViewModel.\u003C\u003Eo__70.\u003C\u003Ep__3, this.ViewerMouseMode, MouseModes.Default);
        }
      }
      this.OnPropertyChanged(nameof (AnnotationMode));
    }
  }

  public PageObjectType EditingPageObjectType
  {
    get => this.editingPageObjectType;
    set
    {
      if (!this.SetProperty<PageObjectType>(ref this.editingPageObjectType, value, nameof (EditingPageObjectType)))
        return;
      this.viewToolbarViewModel.EditPageTextObjectButtonModel.IsChecked = value == PageObjectType.Text;
    }
  }

  public void RaiseAnnotationModePropertyChanged() => this.OnPropertyChanged("AnnotationMode");

  public object ViewerMouseMode => (object) this.viewerMouseMode;

  public object EditorMouseMode => (object) this.editorMouseMode;

  public MainViewModel()
  {
  }

  public MainViewModel(string startUpFilePath)
  {
    this.StartUpFilePath = startUpFilePath;
    this.IsDocumentOpened = !string.IsNullOrEmpty(this.StartUpFilePath);
    this.documentWrapper = new DocumentWrapper();
    this.documentWrapper.PasswordRequested += new EventHandler<DocumentPasswordRequestedEventArgs>(this.DocumentWrapper_PasswordRequested);
    this.documentWrapper.FileError += new EventHandler(this.DocumentWrapper_FileError);
    DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() =>
    {
      this.ViewToolbar = new ViewToolbarViewModel(this);
      this.AnnotationToolbar = new AnnotationToolbarViewModel(this);
      this.PageEditors = new PageEditorViewModel(this);
      this.ShareTab = new ShareTabViewModel(this);
      this.InitDefaultSettings();
      this.InitAutoSave();
      this.AnnotationToolbar.LoadToolbarSettingsConfigAsync();
      this.Menus.ToolbarInited = true;
      // ISSUE: reference to a compiler-generated field
      if (MainViewModel.\u003C\u003Eo__82.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MainViewModel.\u003C\u003Eo__82.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof (MainViewModel), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = MainViewModel.\u003C\u003Eo__82.\u003C\u003Ep__0.Target((CallSite) MainViewModel.\u003C\u003Eo__82.\u003C\u003Ep__0, this.ViewerMouseMode);
    }));
  }

  private void InitAutoSave()
  {
    ConfigManager.GetAutoSaveAsync(new CancellationToken()).GetAwaiter().GetResult();
    this.AutoSaveModel = new pdfeditor.AutoSaveRestore.AutoSaveModel()
    {
      IsAuto = true,
      SpanMinutes = 5
    };
    pdfeditor.AutoSaveRestore.AutoSaveManager.GetInstance().SaveStarted += new EventHandler(this.AutoSaveStarted);
    pdfeditor.AutoSaveRestore.AutoSaveManager.GetInstance().SaveCompleted += new EventHandler(this.AutoSaveCompleted);
  }

  private void AutoSaveStarted(object sender, EventArgs e)
  {
    DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (() =>
    {
      MainView mainWindow = (MainView) App.Current.MainWindow;
      mainWindow.progressBar.Visibility = Visibility.Visible;
      mainWindow.progressBar.Value = 0.0;
    }));
  }

  private void AutoSaveCompleted(object sender, EventArgs e)
  {
    DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (async () =>
    {
      MainView mainView = (MainView) App.Current.MainWindow;
      mainView.progressBar.Value = 1.0;
      await Task.Delay(500);
      mainView.progressBar.Value = 0.0;
      mainView.progressBar.Visibility = Visibility.Collapsed;
      mainView.lblsaveTime.Content = (object) $"{DateTime.Now.ToString("HH:mm:ss")} {Resources.WinStatuBarPDFAutoSaveText}";
      mainView = (MainView) null;
    }));
  }

  private async void InitDefaultSettings()
  {
    PageSizeModel result1 = ConfigManager.GetPageSizeModelAsync(new CancellationToken()).GetAwaiter().GetResult();
    if (result1 != null)
    {
      this.ViewToolbar.DocSizeModeWrap = (SizeModesWrap) Enum.Parse(typeof (SizeModesWrap), result1.SizeMode);
    }
    else
    {
      string pageDefaultSize = ConfigManager.GetPageDefaultSize();
      this.ViewToolbar.DocSizeModeWrap = !(pageDefaultSize != "FitToWidth") ? SizeModesWrap.FitToWidth : (SizeModesWrap) Enum.Parse(typeof (SizeModesWrap), pageDefaultSize);
    }
    PageDisplayModel result2 = ConfigManager.GetPageDisplayModelAsync(new CancellationToken()).GetAwaiter().GetResult();
    if (result2 != null)
    {
      this.ViewToolbar.SubViewModePage = (SubViewModePage) Enum.Parse(typeof (SubViewModePage), result2.DisplayMode);
      this.ViewToolbar.SubViewModeContinuous = (SubViewModeContinuous) Enum.Parse(typeof (SubViewModeContinuous), result2.ContinuousDisplayMode);
    }
    else
    {
      this.ViewToolbar.SubViewModePage = SubViewModePage.SinglePage;
      this.ViewToolbar.SubViewModeContinuous = SubViewModeContinuous.Verticalcontinuous;
    }
    BackgroundModel backGround = ConfigManager.GetBackgroundModelAsync(new CancellationToken()).GetAwaiter().GetResult();
    if (backGround != null)
    {
      IContextMenuModel contextMenuModel = this.ViewToolbar.BackgroundMenuItems.ToList<IContextMenuModel>().Find((Predicate<IContextMenuModel>) (x => x.Name.Equals(backGround.BackgroundName)));
      if (contextMenuModel != null && contextMenuModel is BackgroundContextMenuItemModel contextMenuItemModel)
        contextMenuItemModel.IsChecked = true;
    }
    int autoScrollSpeed = await ConfigManager.GetAutoScrollSpeedAsync(1);
    ContextMenuItemModel contextMenuItemModel1 = this.ViewToolbar.AutoScrollMenuItems.OfType<ContextMenuItemModel>().FirstOrDefault<ContextMenuItemModel>((Func<ContextMenuItemModel, bool>) (c => c.TagData.MenuItemValue is int menuItemValue && menuItemValue == autoScrollSpeed));
    if (contextMenuItemModel1 == null)
    {
      contextMenuItemModel1 = this.ViewToolbar.AutoScrollMenuItems.OfType<ContextMenuItemModel>().FirstOrDefault<ContextMenuItemModel>((Func<ContextMenuItemModel, bool>) (c => c.TagData.MenuItemValue is 1));
      autoScrollSpeed = (int) contextMenuItemModel1.TagData.MenuItemValue;
    }
    contextMenuItemModel1.IsChecked = true;
    this.ViewToolbar.AutoScrollSpeed = autoScrollSpeed;
  }

  private void DocumentWrapper_PasswordRequested(
    object sender,
    DocumentPasswordRequestedEventArgs e)
  {
    EnterPasswordDialog enterPasswordDialog = new EnterPasswordDialog(Path.GetFileName(e.FileName));
    enterPasswordDialog.Owner = (Window) Application.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
    enterPasswordDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    bool? nullable = enterPasswordDialog.ShowDialog();
    e.Cancel = !nullable.GetValueOrDefault();
    e.Password = enterPasswordDialog.Password;
  }

  private async void DocumentWrapper_FileError(object sender, EventArgs e)
  {
    await this.CloseDocumentAsync();
  }

  public DocumentWrapper DocumentWrapper => this.documentWrapper;

  public string StartUpFilePath { get; }

  public AsyncRelayCommand OpenStartUpFileCmd
  {
    get
    {
      return this.openStartUpFileCmd ?? (this.openStartUpFileCmd = new AsyncRelayCommand((Func<Task>) (async () =>
      {
        if (string.IsNullOrEmpty(this.StartUpFilePath) || !this.StartUpFilePath.ToLowerInvariant().EndsWith(".pdf"))
        {
          this.IsDocumentOpened = false;
          LaunchUtils.DoLaunchAction();
        }
        int num = await this.OpenDocumentCoreAsync(this.StartUpFilePath) ? 1 : 0;
        this.OpenDocCmd.NotifyCanExecuteChanged();
      }), (Func<bool>) (() => !this.OpenStartUpFileCmd.IsRunning)));
    }
  }

  public string ExtraSaveOperationName => this.extraSaveOperationName;

  public bool HasExtraSaveOperation => !string.IsNullOrEmpty(this.extraSaveOperationName);

  public OperationManager OperationManager
  {
    get => this.operationManager;
    private set
    {
      this.SetProperty<OperationManager>(ref this.operationManager, value, nameof (OperationManager));
    }
  }

  public void SetCanSaveFlag() => this.SetCanSaveFlag("Unknown", false);

  public void SetCanSaveFlag(string operation, bool clearOperationStack)
  {
    if (clearOperationStack)
      this.OperationManager?.ClearAsync()?.GetAwaiter().GetResult();
    if (string.IsNullOrEmpty(operation))
      operation = "Unknown";
    this.extraSaveOperationName = operation;
    this.CanSave = true;
    pdfeditor.AutoSaveRestore.AutoSaveManager.GetInstance().CanSaveByOperationManager = true;
    pdfeditor.AutoSaveRestore.AutoSaveManager.GetInstance().LastOperationVersion = pdfeditor.AutoSaveRestore.AutoSaveManager.MutexOperationID;
  }

  public void RemoveCanSaveFlag(bool clearOperationStack)
  {
    this.SetCanSaveFlag("Unknown", clearOperationStack);
    this.extraSaveOperationName = (string) null;
    this.UpdateCanSaveFlagState();
  }

  public void UpdateCanSaveFlagState()
  {
    this.RedoCmd.NotifyCanExecuteChanged();
    this.UndoCmd.NotifyCanExecuteChanged();
    bool flag = this.HasExtraSaveOperation || this.version != this.OperationManager?.Version;
    if (!flag && this.Document != null)
    {
      PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.Document);
      flag = flag || pdfControl.CanEditorUndo;
    }
    this.CanSave = flag;
  }

  public bool CanSave
  {
    get => this.canSave;
    set
    {
      if (!this.SetProperty<bool>(ref this.canSave, value, nameof (CanSave)))
        return;
      string str = this.CurrentFileName + " - PDFgear";
      if (value)
        str += " *";
      App.Current.MainWindow.Title = str;
      this.SaveCmd.NotifyCanExecuteChanged();
    }
  }

  public string Password
  {
    get
    {
      if (this.DocumentWrapper?.Document != null)
      {
        if (this.DocumentWrapper.EncryptManage.IsHaveOwerPassword)
          return this.DocumentWrapper.EncryptManage.OwerPassword;
        if (this.DocumentWrapper.EncryptManage.IsHaveUserPassword)
          return this.DocumentWrapper.EncryptManage.UserPassword;
      }
      return (string) null;
    }
  }

  private void UpdateDocument(bool closing)
  {
    this.OperationManager?.Dispose();
    this.OperationManager = (OperationManager) null;
    this.version = string.Empty;
    this.extraSaveOperationName = (string) null;
    this.pageEditorViewModel.FlushViewerAndThumbnail();
    if (this.DocumentWrapper?.Document != null)
    {
      this.OperationManager = new OperationManager(this.Document);
      this.OperationManager.StateChanged += (EventHandler) ((s, a) => this.UpdateCanSaveFlagState());
      if (this.DocumentWrapper.Document.FormFill != null)
        this.DocumentWrapper.Document.FormFill.FieldChanged += new EventHandler(this.DocumentFormFill_FieldChanged);
      if (this.DocumentWrapper.Document.Pages.Count > 0)
        PageDisposeHelper.TryFixPageAnnotations(this.DocumentWrapper.Document, 0);
    }
    this.RedoCmd.NotifyCanExecuteChanged();
    this.UndoCmd.NotifyCanExecuteChanged();
    this.CanSave = this.version != (this.OperationManager?.Version ?? string.Empty);
    if (closing)
    {
      this.IsDocumentOpened = true;
      this.ChatButtonVisible = false;
      this.ChatPanelVisible = false;
    }
    else
    {
      this.IsDocumentOpened = this.DocumentWrapper?.Document != null;
      this.IsSaveBySignature = false;
      this.SignaturesCount = 0;
      this.ChatButtonVisible = this.DocumentWrapper?.Document != null && ConfigManager.GetShowcaseChatButtonFlag();
      this.ChatPanelVisible = !ConfigManager.GetChatPanelClosed() && this.ChatButtonVisible;
    }
    this.OnPropertyChanged("Document");
    this.UpdateDocumentCore();
    this.ViewToolbar.TryUpdateViewerBackground();
    if (closing)
      return;
    this.GetPageStyle();
  }

  public void SetPageStyle()
  {
    MainView mainView = App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
    double contentWidth1 = mainView.LeftNavigationView.ContentWidth;
    bool leftViewIsExpand = mainView.LeftNavigationView.SelectedIndex >= 0;
    double contentWidth2 = mainView.RightNavigationView.ContentWidth;
    string name = this.Menus.SelectedLeftNavItem?.Name;
    ConfigManager.SetPageStyleAsync(contentWidth1, leftViewIsExpand, name, contentWidth2);
  }

  public void GetPageStyle()
  {
    PageStyleModel pageStyle = ConfigManager.GetPageStyleAsync(new CancellationToken()).ConfigureAwait(false).GetAwaiter().GetResult();
    if (pageStyle == null)
      return;
    MainView mainView = App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
    mainView.LeftNavigationView.ContentWidth = pageStyle.LeftNavigationViewWidth;
    if (pageStyle.RightNavigationViewWidth != 0.0)
      mainView.RightNavigationView.ContentWidth = pageStyle.RightNavigationViewWidth;
    if (string.IsNullOrEmpty(pageStyle.LeftNavigationViewSelectItem))
      return;
    ObservableCollection<NavigationModel> leftNavList = this.Menus.LeftNavList;
    NavigationModel navigationModel = leftNavList != null ? leftNavList.FirstOrDefault<NavigationModel>((Func<NavigationModel, bool>) (x => x.Name.Equals(pageStyle.LeftNavigationViewSelectItem))) : (NavigationModel) null;
    if (navigationModel == null)
      return;
    this.Menus.SelectedLeftNavItem = navigationModel;
  }

  internal void UpdateDocumentCore()
  {
    this.PageCommetSource?.Dispose();
    this.PageCommetSource = (AllPageCommetCollectionView) null;
    this.UpdateBookmarks();
    this.CopilotHelper?.Dispose();
    this.CopilotHelper = (CopilotHelper) null;
    if (this.DocumentWrapper?.Document != null)
    {
      this.Menus.SearchModel = new SearchModel(this.DocumentWrapper.Document);
      this.ThumbnailItemSource = this.DocumentWrapper.Document.Pages.Select<PdfPage, PdfThumbnailModel>((Func<PdfPage, int, PdfThumbnailModel>) ((_, i) => new PdfThumbnailModel(this.DocumentWrapper.Document, i))).ToList<PdfThumbnailModel>();
      PdfPageEditList pdfPageEditList = new PdfPageEditList(this.ThumbnailItemSource.Select<PdfThumbnailModel, PdfPageEditListModel>((Func<PdfThumbnailModel, PdfPageEditListModel>) (c => new PdfPageEditListModel(c.Document, c.PageIndex))));
      if (this.PageEditors != null)
        pdfPageEditList.Scale = this.PageEditors.PageEditerThumbnailScale;
      this.PageEditors.PageEditListItemSource = pdfPageEditList;
      this.PageCommetSource = new AllPageCommetCollectionView(this.DocumentWrapper.Document);
      this.CopilotHelper = new CopilotHelper(this.DocumentWrapper.Document, this.DocumentWrapper.DocumentPath);
    }
    else
    {
      this.ThumbnailItemSource = (List<PdfThumbnailModel>) null;
      this.PageEditors.PageEditListItemSource = (PdfPageEditList) null;
      this.PageCommetSource = (AllPageCommetCollectionView) null;
      this.ViewToolbar.AutoScrollButtonModel.IsChecked = false;
      this.ExitTransientMode();
    }
    this.pageEditorViewModel.FlushViewerAndThumbnail();
    NavigationModel selectedLeftNavItem = this.Menus.SelectedLeftNavItem;
    if (this.DocumentWrapper?.Document == null)
      this.Menus.SelectedLeftNavItem = (NavigationModel) null;
    else if (selectedLeftNavItem?.Name == "Bookmark")
    {
      if (this.Bookmarks == null || this.Bookmarks.Children.Count == 0)
        this.Menus.SelectedLeftNavItem = (NavigationModel) null;
    }
    else if (selectedLeftNavItem?.Name == "Annotation")
    {
      this.PageCommetSource?.StartLoad();
      this.AllCount = 0;
      foreach (PageCommetCollection commetCollection in (Collection<PageCommetCollection>) this.PageCommetSource)
        this.AllCount += commetCollection.Count;
    }
    this.OnPropertyChanged("TotalPagesCount");
    this.OnPropertyChanged("IsLeftNavigationMenuEnabled");
    this.OnPropertyChanged("IsFirstPage");
    this.OnPropertyChanged("IsLastPage");
  }

  internal void UpdateBookmarks()
  {
    if (this.DocumentWrapper.Document != null)
      this.Bookmarks = BookmarkModel.Create(this.DocumentWrapper.Document);
    else
      this.Bookmarks = (BookmarkModel) null;
  }

  public PdfDocument Document => this.DocumentWrapper?.Document;

  public List<PdfThumbnailModel> ThumbnailItemSource
  {
    get => this.thumbnailItemSource;
    set
    {
      this.SetProperty<List<PdfThumbnailModel>>(ref this.thumbnailItemSource, value, nameof (ThumbnailItemSource));
    }
  }

  public BookmarkModel Bookmarks
  {
    get => this.bookmarks;
    set => this.SetProperty<BookmarkModel>(ref this.bookmarks, value, nameof (Bookmarks));
  }

  public AllPageCommetCollectionView PageCommetSource
  {
    get => this.pageCommetSource;
    set
    {
      this.SetProperty<AllPageCommetCollectionView>(ref this.pageCommetSource, value, nameof (PageCommetSource));
    }
  }

  public CopilotHelper CopilotHelper
  {
    get => this.copilotHelper;
    set => this.SetProperty<CopilotHelper>(ref this.copilotHelper, value, nameof (CopilotHelper));
  }

  public Thickness PdfToWordMargin
  {
    get => this.pdfToWordMargin;
    set => this.SetProperty<Thickness>(ref this.pdfToWordMargin, value, nameof (PdfToWordMargin));
  }

  public int SelectedPageIndex
  {
    get => this.selectedPageIndex;
    set
    {
      int num1 = 1;
      if (this.DocumentWrapper.Document != null)
        num1 = this.DocumentWrapper.Document.Pages.Count;
      int num2 = value;
      if (num2 > num1 - 1)
        num2 = num1 - 1;
      else if (num2 < 0)
      {
        try
        {
          num2 = Math.Max(0, this.LastViewPage == null ? 0 : this.LastViewPage.PageIndex);
        }
        catch (Exception ex)
        {
          num2 = 0;
        }
      }
      if ((DateTime.Now - this.time).TotalMilliseconds >= 200.0 && !this.Jumping && this.CurrnetPageIndex >= 1)
      {
        this.viewJumpManager.StackChange();
        this.viewJumpManager.NewRecord(this.CurrnetPageIndex - 1);
        this.Jumping = false;
      }
      if (num2 != this.selectedPageIndex && PageDisposeHelper.TryFixPageAnnotations(this.DocumentWrapper.Document, num2) && this.DocumentWrapper.Document.Pages[num2].IsLoaded)
        this.DocumentWrapper.Document.Pages[num2].Dispose();
      if (this.SetProperty<int>(ref this.selectedPageIndex, num2, nameof (SelectedPageIndex)))
      {
        this.OnPropertyChanged("IsFirstPage");
        this.OnPropertyChanged("IsLastPage");
        if (this.DocumentWrapper.Document != null)
        {
          string documentPath = this.DocumentWrapper?.DocumentPath;
          if (!string.IsNullOrEmpty(documentPath))
            ConfigManager.SetDocumentCurrentPageNumberAsync(documentPath, num2);
        }
      }
      this.OnPropertyChanged("CurrnetPageIndex");
      this.Jumping = false;
      this.time = DateTime.Now;
    }
  }

  public bool IsLeftNavigationMenuEnabled => this.Document != null;

  public bool IsDocumentOpened
  {
    get => this.isDocumentOpened;
    set => this.SetProperty<bool>(ref this.isDocumentOpened, value, nameof (IsDocumentOpened));
  }

  public int TotalPagesCount
  {
    get => this.Document == null || this.Document.Pages == null ? 0 : this.Document.Pages.Count;
  }

  public int CurrnetPageIndex
  {
    get => this.Document == null || this.Document.Pages == null ? 0 : this.SelectedPageIndex + 1;
    set => this.SelectedPageIndex = value - 1;
  }

  public bool IsFirstPage
  {
    get
    {
      return this.Document == null || this.Document.Pages == null || this.Document.Pages.CurrentIndex <= 0;
    }
  }

  public bool IsLastPage
  {
    get
    {
      return this.Document == null || this.Document.Pages == null || this.Document.Pages.CurrentIndex >= this.Document.Pages.Count - 1;
    }
  }

  public bool FillForm
  {
    get => this.fillForm;
    set
    {
      this.SetProperty<bool>(ref this.fillForm, value, nameof (FillForm));
      ((MainView) App.Current.MainWindow).PdfControl.Viewer.IsFillFormHighlighted = value;
    }
  }

  public AllPageCommetCollectionView AllPageCommetSource
  {
    get => this.allpageCommetSource;
    set
    {
      this.SetProperty<AllPageCommetCollectionView>(ref this.allpageCommetSource, value, nameof (AllPageCommetSource));
    }
  }

  public bool? IsUserFilterAllChecked
  {
    get => this.isUserFilterAllChecked;
    set
    {
      this.IsFilterAllChecked = !value.GetValueOrDefault() || !this.IsKindFilterAllChecked.GetValueOrDefault() ? new bool?(false) : new bool?(true);
      this.SetProperty<bool?>(ref this.isUserFilterAllChecked, value, nameof (IsUserFilterAllChecked));
    }
  }

  public int AllCount
  {
    get => this.allCount;
    set => this.SetProperty<int>(ref this.allCount, value, nameof (AllCount));
  }

  public bool? IsFilterAllChecked
  {
    get => this.isFilterAllChecked;
    set => this.SetProperty<bool?>(ref this.isFilterAllChecked, value, nameof (IsFilterAllChecked));
  }

  public bool? IsKindFilterAllChecked
  {
    get => this.isKindFilterAllChecked;
    set
    {
      this.IsFilterAllChecked = !value.GetValueOrDefault() || !this.IsUserFilterAllChecked.GetValueOrDefault() ? new bool?(false) : new bool?(true);
      this.SetProperty<bool?>(ref this.isKindFilterAllChecked, value, nameof (IsKindFilterAllChecked));
    }
  }

  public void FilterAnnotations()
  {
    this.IsSelectedAll = new bool?(false);
    this.pageCommetSource.FilterShowItems();
  }

  public RelayCommand UserMannulCmd
  {
    get
    {
      return this.userMannulCmd ?? (this.userMannulCmd = new RelayCommand((Action) (async () => UserGuideUtils.OpenUserGuide())));
    }
  }

  public RelayCommand UserGuideCmd
  {
    get
    {
      return this.userGuideCmd ?? (this.userGuideCmd = new RelayCommand((Action) (() => this.OpenUserGuide()), (Func<bool>) (() => this.CanDoFeedBack())));
    }
  }

  public RelayCommand GetPhoneStoreCmd
  {
    get
    {
      return this.getPhoneStoreCmd ?? (this.getPhoneStoreCmd = new RelayCommand((Action) (() => this.OpenPhoneWin()), (Func<bool>) (() => this.CanDoFeedBack())));
    }
  }

  private void OpenPhoneWin()
  {
    CommomLib.Commom.GAManager.SendEvent("Ads", "GearForMobile", "ToolbarBtn", 1L);
    GearForMobilephone gearForMobilephone = new GearForMobilephone();
    gearForMobilephone.Owner = Application.Current.MainWindow;
    gearForMobilephone.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    gearForMobilephone.ShowDialog();
  }

  private void OpenUserGuide()
  {
    CommomLib.Commom.GAManager.SendEvent("MainWindow", "UserGuide", "Count", 1L);
    string actualAppLanguage = CultureInfoUtils.ActualAppLanguage;
    string path1 = "https://www.pdfgear.com/";
    string path2 = "windows-user-guide/introduction-pdfgear.htm";
    string gearRate = Path.Combine(path1, path2).Replace("\\", "/");
    if (actualAppLanguage.ToLower() == "es")
      gearRate = Path.Combine(path1, "es/windows-user-guide/introduccion-pdfgear.htm").Replace("\\", "/");
    if (actualAppLanguage.ToLower() == "fr")
      gearRate = Path.Combine(path1, "fr/windows-user-guide/introduction-de-pdfgear.htm").Replace("\\", "/");
    if (actualAppLanguage.ToLower() == "it")
      gearRate = Path.Combine(path1, "it/windows-user-guide/introduzione-pdfgear.htm").Replace("\\", "/");
    if (actualAppLanguage.ToLower() == "pt")
      gearRate = Path.Combine(path1, "pt/windows-user-guide/introducao-pdfgear.htm").Replace("\\", "/");
    if (actualAppLanguage.ToLower() == "de")
      gearRate = Path.Combine(path1, "de/anleitung/einfuehrung-in-pdfgear.htm").Replace("\\", "/");
    object locker = new object();
    bool result = false;
    new Thread((ThreadStart) (() =>
    {
      try
      {
        Process.Start(gearRate);
        result = true;
      }
      catch
      {
        result = false;
      }
      finally
      {
        lock (locker)
          Monitor.PulseAll(locker);
      }
    }))
    {
      IsBackground = true
    }.Start();
    lock (locker)
    {
      Monitor.Wait(locker, 5000);
      int num = result ? 1 : 0;
      CommomLib.Commom.GAManager.SendEvent("MainWindow", "GuideBlockExit", "Count", 1L);
    }
  }

  public RelayCommand FeedBackCmd
  {
    get
    {
      return this.feedBackCmd ?? (this.feedBackCmd = new RelayCommand((Action) (() => this.DoFeedBack()), (Func<bool>) (() => this.CanDoFeedBack())));
    }
  }

  private void DoFeedBack() => SupportUtils.ShowFeedbackWindow(this.DocumentWrapper?.DocumentPath);

  private bool CanDoFeedBack() => true;

  public RelayCommand AutoSaveSettingCmd
  {
    get
    {
      return this.autoSaveSettingCmd ?? (this.autoSaveSettingCmd = new RelayCommand((Action) (() => this.DoAutoSaveSetting()), (Func<bool>) (() => this.CanDoAutoSaveSetting())));
    }
  }

  public void DoAutoSaveSetting() => new SettingWindow().Show();

  private bool CanDoAutoSaveSetting() => true;

  public RelayCommand UpgradeCmd
  {
    get
    {
      return this.upgradeCmd ?? (this.upgradeCmd = new RelayCommand((Action) (() => this.ShowUpgrade()), (Func<bool>) (() => this.CanShowUpgrade())));
    }
  }

  private void ShowUpgrade()
  {
    if (!IAPUtils.IsPaidUserWrapper())
      IAPUtils.ShowPurchaseWindows("Toolbar", ".pdf");
    else
      IAPUtils.ShowPaidWindows();
  }

  private bool CanShowUpgrade() => true;

  public RelayCommand AboutCmd
  {
    get
    {
      return this.aboutCmd ?? (this.aboutCmd = new RelayCommand((Action) (() => this.ShowAbout()), (Func<bool>) (() => this.CanShowAbout())));
    }
  }

  public RelayCommand PropertiesCmd
  {
    get
    {
      return this.propertiesCmd ?? (this.propertiesCmd = new RelayCommand((Action) (() => this.ShowPropertiesFToolbar()), (Func<bool>) (() => this.CanShowProperties())));
    }
  }

  public RelayCommand UpdateCmd
  {
    get
    {
      return this.updateCmd ?? (this.updateCmd = new RelayCommand((Action) (() => this.Update()), (Func<bool>) (() => this.CanShowAbout())));
    }
  }

  public AsyncRelayCommand SettingsCmd
  {
    get
    {
      return this.settingsCmd ?? (this.settingsCmd = new AsyncRelayCommand((Func<Task>) (async () =>
      {
        await Ioc.Default.GetRequiredService<AppSettingsViewModel>().RefreshSettingsAsync();
        AppSettingsWindow appSettingsWindow = new AppSettingsWindow()
        {
          Owner = App.Current.MainWindow
        };
        appSettingsWindow.WindowStartupLocation = appSettingsWindow.Owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
        appSettingsWindow.ShowDialog();
      })));
    }
  }

  private void ShowAbout()
  {
    AboutWindow aboutWindow = new AboutWindow();
    if (aboutWindow == null)
      return;
    aboutWindow.Owner = Application.Current.MainWindow;
    aboutWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    aboutWindow.ShowDialog();
  }

  public void ShowPropertiesFToolbar()
  {
    CommomLib.Commom.GAManager.SendEvent("DocumentPropertiesWindow", "ShowSource", "Toolbar", 1L);
    this.ShowProperties();
  }

  public void ShowProperties()
  {
    if (this.Document == null)
      return;
    DocumentPropertiesWindow propertiesWindow = new DocumentPropertiesWindow(this.Document, this.CurrnetPageIndex, this.documentWrapper?.DocumentPath);
    if (propertiesWindow == null)
      return;
    propertiesWindow.Owner = Application.Current.MainWindow;
    propertiesWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    propertiesWindow.ShowDialog();
  }

  private async void Update()
  {
    int num = await UpdateHelper.UpdateAndExit(true) ? 1 : 0;
  }

  private bool CanShowAbout() => true;

  private bool CanShowProperties() => true;

  public AsyncRelayCommand PrintDocCmd
  {
    get
    {
      return this.printDocCmd ?? (this.printDocCmd = new AsyncRelayCommand((Func<Task>) (async () => await this.PrintDoc()), (Func<bool>) (() => this.CanPrintDoc())));
    }
  }

  public AsyncRelayCommand BatchPrintCmd
  {
    get
    {
      return this.batchPrintCmd ?? (this.batchPrintCmd = new AsyncRelayCommand((Func<Task>) (async () => await this.BatchPrintAsync("MainWindow"))));
    }
  }

  public async Task ReleaseViewerFocusAsync(bool beforeSave)
  {
    try
    {
      this.Document?.FormFill?.ForceToKillFocus();
    }
    catch
    {
    }
    if (!beforeSave)
      return;
    PDFKit.PdfControl viewer = PDFKit.PdfControl.GetPdfControl(this.Document);
    if (viewer != null)
    {
      AnnotationHolderManager annotationHolderManager = PdfObjectExtensions.GetAnnotationHolderManager(viewer);
      if (annotationHolderManager != null)
      {
        annotationHolderManager.CancelAll();
        await annotationHolderManager.WaitForCancelCompletedAsync();
      }
      PdfObjectExtensions.GetAnnotationCanvas(viewer)?.PopupHolder.KillFocus();
    }
    viewer = (PDFKit.PdfControl) null;
  }

  public AsyncRelayCommand OpenDocCmd
  {
    get
    {
      return this.openDocCmd ?? (this.openDocCmd = new AsyncRelayCommand((Func<Task>) (async () =>
      {
        if (this.Menus.SearchModel != null && this.Menus.SearchModel.IsSearchVisible)
          this.Menus.SearchModel.IsSearchVisible = false;
        this.ExitTransientMode();
        await this.ReleaseViewerFocusAsync(true);
        if (!await this.TrySaveBeforeCloseDocumentAsync())
          return;
        OpenFileDialog openFileDialog = new OpenFileDialog()
        {
          Filter = "pdf|*.pdf",
          ShowReadOnly = false,
          ReadOnlyChecked = true
        };
        if (!openFileDialog.ShowDialog(App.Current.MainWindow).GetValueOrDefault() || string.IsNullOrEmpty(openFileDialog.FileName))
          return;
        int num = await this.OpenDocumentCoreAsync(openFileDialog.FileName) ? 1 : 0;
      }), (Func<bool>) (() => true)));
    }
  }

  public async Task<bool> OpenDocumentCoreAsync(string filePath)
  {
    MainViewModel mainViewModel = this;
    if (string.IsNullOrEmpty(filePath))
      return false;
    if (filePath == mainViewModel.DocumentWrapper.DocumentPath)
      return true;
    mainViewModel.IsDocumentOpened = true;
    bool res = false;
    Exception exception = (Exception) null;
    try
    {
      PdfDocument oldDocument = mainViewModel.Document;
      CommomLib.Commom.GAManager.SendEvent("MainWindow", "OpenDocumentCore", "Begin", 1L);
      res = await mainViewModel.DocumentWrapper.OpenAsync(filePath);
      if (res)
      {
        HistoryManager.UpdateHistory(filePath);
        ProcessMessageHelper.SendMessageAsync(0UL, "UpdateFileHistory");
        MainView mainView = (MainView) App.Current.MainWindow;
        mainView.lblsaveTime.Content = (object) "";
        mainViewModel.extraSaveOperationName = (string) null;
        if (oldDocument?.FormFill != null)
          oldDocument.FormFill.FieldChanged -= new EventHandler(mainViewModel.DocumentFormFill_FieldChanged);
        int currentPageNumberAsync = await ConfigManager.GetDocumentCurrentPageNumberAsync(mainViewModel.DocumentWrapper.DocumentPath, new CancellationToken());
        mainViewModel.GetPageSizeZoomModel(mainViewModel.DocumentWrapper.DocumentPath);
        mainViewModel.UpdateDocument(false);
        mainViewModel.AutoSaveModel.SourceFileName = filePath;
        pdfeditor.AutoSaveRestore.AutoSaveManager.GetInstance().Start(mainViewModel.AutoSaveModel.SpanMinutes);
        mainViewModel.time = DateTime.Now;
        mainViewModel.SelectedPageIndex = currentPageNumberAsync == -1 || currentPageNumberAsync >= mainViewModel.DocumentWrapper.Document.Pages.Count ? 0 : currentPageNumberAsync;
        mainViewModel.DocumentWrapper.TrimMemory();
        mainViewModel.viewJumpManager.ClearStack();
        mainViewModel.ReadAcitved();
        string fileName = Path.GetFileName(filePath);
        mainViewModel.CurrentFileName = fileName;
        App.Current.MainWindow.Title = fileName + " - PDFgear";
        mainView.PdfControl.Viewer.IsFillFormHighlighted = ConfigManager.GetIsFillFormHighlightedFlag();
        mainViewModel.FillForm = mainView.PdfControl.Viewer.IsFillFormHighlighted;
        HistoryManager.UpdateHistory(filePath);
        ProcessMessageHelper.SendMessageAsync(0UL, "UpdateFileHistory");
        ConfigManager.SetCouldRateFlag(true);
        PDFKit.PdfControl.GetPdfControl(mainViewModel.Document)?.Focus();
        mainView = (MainView) null;
      }
      else
        CommomLib.Commom.GAManager.SendEvent("MainWindow", "OpenDocumentCore", "Failed", 1L);
      oldDocument = (PdfDocument) null;
    }
    catch (Exception ex)
    {
      CommomLib.Commom.GAManager.SendEvent("MainWindow", "OpenDocumentCore", "Exception", 1L);
      exception = ex;
    }
    mainViewModel.IsDocumentOpened = mainViewModel.DocumentWrapper.Document != null;
    LaunchUtils.OnDocumentLoaded(mainViewModel.Document);
    if (!res)
    {
      string msg = Resources.OpenDocFailedExceptionMsg;
      if (!string.IsNullOrEmpty(filePath))
        msg = $"{msg}: \"{filePath}\"";
      if (exception != null)
        msg = $"{msg}\n\n{exception.Message}";
      int num;
      DispatcherHelper.UIDispatcher.Invoke((Action) (() => num = (int) ModernMessageBox.Show(msg, "PDFgear")));
    }
    return res;
  }

  private void ReadAcitved()
  {
    this.speechUtils?.Dispose();
    this.speechUtils = (SpeechUtils) null;
    this.IsReading = false;
    this.ViewToolbar.ReadButtonModel.IsChecked = false;
    ContextMenuModel contextMenu = (this.ViewToolbar.ReadButtonModel.ChildButtonModel as ToolbarChildCheckableButtonModel).ContextMenu as ContextMenuModel;
    (contextMenu[0] as SpeedContextMenuItemModel).IsEnabled = true;
    (contextMenu[1] as SpeedContextMenuItemModel).IsEnabled = true;
    (contextMenu[2] as SpeedContextMenuItemModel).IsEnabled = true;
  }

  private void GetPageSizeZoomModel(string filePath)
  {
    PageSizeZoomModel result = ConfigManager.GetPageSizeZoomModelAsync(filePath, new CancellationToken()).GetAwaiter().GetResult();
    if (result != null)
    {
      this.ViewToolbar.DocSizeModeWrap = (SizeModesWrap) Enum.Parse(typeof (SizeModesWrap), result.SizeMode);
      this.ViewToolbar.DocZoom = result.PageZoom;
    }
    else
    {
      string pageDefaultSize = ConfigManager.GetPageDefaultSize();
      if (pageDefaultSize != "FitToWidth")
        this.ViewToolbar.DocSizeModeWrap = (SizeModesWrap) Enum.Parse(typeof (SizeModesWrap), pageDefaultSize);
      else
        this.ViewToolbar.DocSizeModeWrap = SizeModesWrap.FitToWidth;
    }
  }

  private void DocumentFormFill_FieldChanged(object sender, EventArgs e) => this.SetCanSaveFlag();

  public async Task CloseDocumentAsync()
  {
    MainViewModel mainViewModel = this;
    await mainViewModel.ReleaseViewerFocusAsync(true);
    mainViewModel.DocumentWrapper.CloseDocument();
    mainViewModel.UpdateDocument(true);
    // ISSUE: reference to a compiler-generated method
    await DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (Delegate) new Action(mainViewModel.\u003CCloseDocumentAsync\u003Eb__244_0));
  }

  public AsyncRelayCommand SaveCmd
  {
    get
    {
      return this.saveCmd ?? (this.saveCmd = new AsyncRelayCommand((Func<Task>) (async () =>
      {
        PdfAnnotation selectedAnnot;
        AnnotationHolderManager holders;
        if (this.SaveCmd.IsRunning)
        {
          selectedAnnot = (PdfAnnotation) null;
          holders = (AnnotationHolderManager) null;
        }
        else if (this.Document == null)
        {
          selectedAnnot = (PdfAnnotation) null;
          holders = (AnnotationHolderManager) null;
        }
        else if (this.DocumentWrapper.IsUntitledFile)
        {
          this.saveAsCmd.ExecuteAsync((object) null);
          selectedAnnot = (PdfAnnotation) null;
          holders = (AnnotationHolderManager) null;
        }
        else if (this.viewToolbarViewModel.IsDocumentEdited)
        {
          CommomLib.Commom.GAManager.SendEvent("TextEditor2", "TextEditing", "Save", 1L);
          int num = await this.viewToolbarViewModel.DocumentEditedSaveAsync() ? 1 : 0;
          selectedAnnot = (PdfAnnotation) null;
          holders = (AnnotationHolderManager) null;
        }
        else
        {
          await this.ReleaseViewerFocusAsync(true);
          selectedAnnot = (PdfAnnotation) null;
          holders = (AnnotationHolderManager) null;
          PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.Document);
          if (pdfControl != null)
          {
            holders = PdfObjectExtensions.GetAnnotationHolderManager(pdfControl);
            foreach (PdfPage page in (ReadOnlyList<PdfPage>) this.Document.Pages)
            {
              if (page.Annots != null)
              {
                foreach (PdfAnnotation annot in page.Annots)
                {
                  if (annot is PdfInkAnnotation annotation2 && annotation2.InkList != null && annotation2.InkList.Count <= 1)
                  {
                    int num = 0;
                    foreach (PdfLinePointCollection<PdfInkAnnotation> ink in annotation2.InkList)
                      num += ink.Count;
                    if (num <= 1)
                      await holders.DeleteAnnotationAsync((PdfAnnotation) annotation2);
                  }
                }
              }
            }
            selectedAnnot = holders.SelectedAnnotation;
            holders.CancelAll();
            await holders.WaitForCancelCompletedAsync();
          }
          if (this.IsSaveBySignature)
          {
            this.SignaturesCount = this.GetSignatureObjCountFlag();
            if (this.SignaturesCount > 0)
            {
              CommomLib.Commom.GAManager.SendEvent("PdfStampAnnotationSignature", "NotFlattenSaveAs", "Count", 1L);
              await this.SaveAsCmd.ExecuteAsync((object) null);
              selectedAnnot = (PdfAnnotation) null;
              holders = (AnnotationHolderManager) null;
              return;
            }
          }
          if (this.CanSave)
            this.DocumentWrapper.Metadata.ModificationDate = DateTimeOffset.Now;
          bool saveResult = await this.DocumentWrapper.SaveAsync();
          if (!saveResult && MessageBox.Show(Resources.SaveDocFailedBySaveasMsg, "PDFgear", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            await this.SaveAsCmd.ExecuteAsync((object) null);
          if (saveResult)
          {
            this.extraSaveOperationName = (string) null;
            this.version = this.OperationManager.Version;
            this.CanSave = false;
            this.DelAutoSaveFile(this.DocumentWrapper.DocumentPath);
          }
          if (holders == null)
          {
            selectedAnnot = (PdfAnnotation) null;
            holders = (AnnotationHolderManager) null;
          }
          else if (!((PdfWrapper) selectedAnnot != (PdfWrapper) null))
          {
            selectedAnnot = (PdfAnnotation) null;
            holders = (AnnotationHolderManager) null;
          }
          else
          {
            holders.Select(selectedAnnot, false);
            selectedAnnot = (PdfAnnotation) null;
            holders = (AnnotationHolderManager) null;
          }
        }
      })));
    }
  }

  public RelayCommand EncryptCMD
  {
    get
    {
      return this.encryptCMD ?? (this.encryptCMD = new RelayCommand((Action) (() => this.DocumentWrapper.ShowEncryptWindow())));
    }
  }

  public RelayCommand RemovePasswordCMD
  {
    get
    {
      return this.removePasswordCMD ?? (this.removePasswordCMD = new RelayCommand((Action) (() => this.DocumentWrapper.ShowDecryptWindow())));
    }
  }

  public AsyncRelayCommand SaveAsCmd
  {
    get
    {
      return this.saveAsCmd ?? (this.saveAsCmd = new AsyncRelayCommand((Func<Task>) (async () =>
      {
        MainViewModel mainViewModel1 = this;
        MainViewModel mainViewModel = mainViewModel1;
        if (mainViewModel1.viewToolbarViewModel.IsDocumentEdited)
        {
          CommomLib.Commom.GAManager.SendEvent("TextEditor2", "TextEditing", "SaveAs", 1L);
          int num = await mainViewModel1.viewToolbarViewModel.DocumentEditedSaveAsync(true) ? 1 : 0;
        }
        else
        {
          await mainViewModel1.ReleaseViewerFocusAsync(true);
          if (mainViewModel1.IsSaveBySignature)
          {
            mainViewModel1.SignaturesCount = mainViewModel1.GetSignatureObjCountFlag();
            if (mainViewModel1.SignaturesCount > 0)
            {
              SignatureEmbedConfirmWin signatureEmbedConfirmWin = new SignatureEmbedConfirmWin(EmbedType.All);
              signatureEmbedConfirmWin.ShowDialog();
              bool? dialogResult = signatureEmbedConfirmWin.DialogResult;
              bool flag = false;
              if (dialogResult.GetValueOrDefault() == flag & dialogResult.HasValue)
                return;
            }
          }
          string str1 = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
          string str2 = "Untitled.pdf";
          if (!string.IsNullOrEmpty(mainViewModel1.DocumentWrapper?.DocumentPath))
          {
            FileInfo fileInfo = new FileInfo(mainViewModel1.DocumentWrapper?.DocumentPath);
            str1 = fileInfo.DirectoryName;
            string str3 = fileInfo.Name;
            if (!string.IsNullOrEmpty(fileInfo.Extension))
              str3 = str3.Substring(0, str3.Length - fileInfo.Extension.Length);
            str2 = str3 + (mainViewModel1.SignaturesCount > 0 ? " Signed.pdf" : " Copy.pdf");
          }
          SaveFileDialog saveFileDialog = new SaveFileDialog()
          {
            Filter = "pdf|*.pdf",
            CreatePrompt = false,
            OverwritePrompt = true,
            InitialDirectory = str1,
            FileName = str2
          };
          bool? result = saveFileDialog.ShowDialog();
          PdfDocument document = mainViewModel1.DocumentWrapper.Document;
          if (saveFileDialog.FileName?.Trim() == mainViewModel1.DocumentWrapper.DocumentPath)
          {
            if (!mainViewModel1.CanSave)
              ;
            else
            {
              mainViewModel1.DocumentWrapper.Metadata.ModificationDate = DateTimeOffset.Now;
              if (await mainViewModel1.DocumentWrapper.SaveAsync())
              {
                mainViewModel1.version = mainViewModel1.OperationManager.Version;
                mainViewModel1.CanSave = false;
                mainViewModel1.DelAutoSaveFile(mainViewModel1.DocumentWrapper.DocumentPath);
              }
              else
              {
                int num = (int) MessageBox.Show(Resources.SaveDocFailedByMsg, "PDFgear");
              }
            }
          }
          else
          {
            int curIndex = mainViewModel1.SelectedPageIndex;
            string filePath = saveFileDialog.FileName;
            await Task.Run(CommomLib.Commom.TaskExceptionHelper.ExceptionBoundary((Func<Task>) (async () =>
            {
              if (!result.GetValueOrDefault())
                return;
              if (string.IsNullOrEmpty(filePath))
                return;
              try
              {
                if (File.Exists(filePath))
                  File.Delete(filePath);
                await DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (async () =>
                {
                  using (FileStream stream = File.OpenWrite(filePath))
                  {
                    int num = mainViewModel.IsSaveBySignature ? 1 : 0;
                    if (mainViewModel.SignaturesCount > 0)
                    {
                      await mainViewModel.SaveFlattenSignature();
                      mainViewModel.IsSaveBySignature = false;
                      mainViewModel.SignaturesCount = 0;
                    }
                    stream.Seek(0L, SeekOrigin.Begin);
                    if (mainViewModel.CanSave)
                      mainViewModel.DocumentWrapper.Metadata.ModificationDate = DateTimeOffset.Now;
                    document.Save((Stream) stream, SaveFlags.NoIncremental);
                    stream.SetLength(stream.Position);
                  }
                  await DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (async () =>
                  {
                    int num3 = await ConfigManager.SetDocumentCurrentPageNumberAsync(filePath, curIndex) ? 1 : 0;
                    ConfigManager.SetPageSizeZoomModelAsync(filePath, mainViewModel.ViewToolbar.DocSizeModeWrap.ToString(), mainViewModel.ViewToolbar.DocZoom);
                    int num4 = await mainViewModel.OpenDocumentCoreAsync(filePath) ? 1 : 0;
                  }));
                  mainViewModel.DelAutoSaveFile(mainViewModel.DocumentWrapper.DocumentPath);
                }));
              }
              catch
              {
                int num = (int) MessageBox.Show(Resources.SaveDocFailedByMsg, "PDFgear");
              }
            })));
          }
        }
      }), (Func<bool>) (() => !this.SaveAsCmd.IsRunning)));
    }
  }

  private async Task SaveFlattenSignature()
  {
    MainViewModel mainViewModel = this;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(mainViewModel.Document);
    // ISSUE: reference to a compiler-generated method
    ProgressUtils.ShowProgressBar(new Func<ProgressUtils.ProgressAction, Task>(mainViewModel.\u003CSaveFlattenSignature\u003Eb__257_0), (string) null, (object) Resources.WinSignatureFlattenProcess, false, (Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>());
    await pdfControl.TryRedrawVisiblePageAsync();
  }

  private void ResetSignatureObjCount()
  {
    if (this.Document == null || this.Document.Pages == null)
      return;
    int num1 = 0;
    for (int index = 0; index < this.Document.Pages.Count; ++index)
    {
      PdfPage page = this.Document.Pages[index];
      if (page.Annots != null)
      {
        PdfAnnotationCollection annots = page.Annots;
        if ((annots != null ? (annots.OfType<PdfStampAnnotation>().Count<PdfStampAnnotation>() == 0 ? 1 : 0) : 0) == 0)
        {
          int num2 = page.Annots.OfType<PdfStampAnnotation>().Where<PdfStampAnnotation>((Func<PdfStampAnnotation, bool>) (x => x.Subject == "Signature")).Count<PdfStampAnnotation>();
          num1 += num2;
          if (num1 > 0)
            break;
        }
      }
    }
    if (num1 > 0)
      this.IsSaveBySignature = true;
    else
      this.IsSaveBySignature = false;
  }

  public int GetSignatureObjCountFlag()
  {
    if (this.Document == null || this.Document.Pages == null)
      return 0;
    int signatureObjCountFlag = 0;
    for (int index = 0; index < this.Document.Pages.Count; ++index)
    {
      PdfPage page = this.Document.Pages[index];
      if (page.Annots != null)
      {
        PdfAnnotationCollection annots = page.Annots;
        if ((annots != null ? (annots.OfType<PdfStampAnnotation>().Count<PdfStampAnnotation>() == 0 ? 1 : 0) : 0) == 0)
        {
          int num = page.Annots.OfType<PdfStampAnnotation>().Where<PdfStampAnnotation>((Func<PdfStampAnnotation, bool>) (x => x.Subject == "Signature")).Count<PdfStampAnnotation>();
          signatureObjCountFlag += num;
          if (signatureObjCountFlag > 0)
            break;
        }
      }
    }
    return signatureObjCountFlag;
  }

  public AsyncRelayCommand UndoCmd
  {
    get
    {
      return this.undoCmd ?? (this.undoCmd = new AsyncRelayCommand((Func<Task>) (async () =>
      {
        if (this.UndoCmd.IsRunning)
          return;
        await this.ReleaseViewerFocusAsync(true);
        PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.Document);
        if (pdfControl != null)
        {
          if (pdfControl != null && pdfControl.CanEditorUndo)
          {
            CommomLib.Commom.GAManager.SendEvent("TextEditor2", "TextEditing", "Undo", 1L);
            pdfControl.UndoEditor();
            return;
          }
          if (pdfControl.IsEditing)
            return;
        }
        await this.OperationManager.GoBackAsync();
      }), (Func<bool>) (() =>
      {
        PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.Document);
        if ((pdfControl != null ? (pdfControl.CanEditorUndo ? 1 : 0) : 0) != 0)
          return true;
        OperationManager operationManager = this.OperationManager;
        return operationManager != null && operationManager.CanGoBack;
      })));
    }
  }

  public AsyncRelayCommand RedoCmd
  {
    get
    {
      return this.redoCmd ?? (this.redoCmd = new AsyncRelayCommand((Func<Task>) (async () =>
      {
        if (this.RedoCmd.IsRunning)
          return;
        await this.ReleaseViewerFocusAsync(true);
        PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.Document);
        if (pdfControl != null && pdfControl.CanEditorRedo)
        {
          CommomLib.Commom.GAManager.SendEvent("TextEditor2", "TextEditing", "Redo", 1L);
          pdfControl.RedoEditor();
        }
        else
          await this.OperationManager.GoForwardAsync();
      }), (Func<bool>) (() =>
      {
        PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.Document);
        if ((pdfControl != null ? (pdfControl.CanEditorRedo ? 1 : 0) : 0) != 0)
          return true;
        OperationManager operationManager = this.OperationManager;
        return operationManager != null && operationManager.CanGoForward;
      })));
    }
  }

  private async Task PrintDoc()
  {
    MainViewModel model = this;
    if (model.Document == null || model.Document.Pages == null)
      return;
    CommomLib.Commom.GAManager.SendEvent("MainWindow", "PrintBtn", "Count", 1L);
    await model.ReleaseViewerFocusAsync(true);
    WinPrinterDialog winPrinterDialog = new WinPrinterDialog(model);
    winPrinterDialog.Owner = (Window) (Application.Current.Windows.Cast<Window>().FirstOrDefault<Window>((Func<Window, bool>) (window => window is MainView)) as MainView);
    winPrinterDialog.ShowDialog();
  }

  public async Task BatchPrintAsync(string source)
  {
    if (this.Document == null || this.Document.Pages == null)
      return;
    CommomLib.Commom.GAManager.SendEvent("PdfBatchPrintDocument", "Show", source, 1L);
    await this.ReleaseViewerFocusAsync(true);
    BatchPrintWindow batchPrintWindow = new BatchPrintWindow(Path.GetFileName(this.DocumentWrapper.DocumentPath), this.Document);
    batchPrintWindow.Owner = (Window) (Application.Current.Windows.Cast<Window>().FirstOrDefault<Window>((Func<Window, bool>) (window => window is MainView)) as MainView);
    batchPrintWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    batchPrintWindow.ShowDialog();
  }

  private bool CanPrintDoc() => true;

  public PdfAnnotation SelectedAnnotation
  {
    get => this.selectedAnnotation;
    set
    {
      if (!this.SetProperty<PdfAnnotation>(ref this.selectedAnnotation, value, nameof (SelectedAnnotation)))
        return;
      if ((PdfWrapper) this.selectedAnnotation != (PdfWrapper) null && this.AnnotationToolbar.InkButtonModel.IsChecked)
        this.AnnotationToolbar.InkButtonModel.IsChecked = false;
      if (this.selectedAnnotation is PdfInkAnnotation)
        (this.AnnotationToolbar.InkButtonModel.ToolbarSettingModel[3] as ToolbarSettingInkEraserModel).IsChecked = false;
      this.AnnotationToolbar.SetMenuItemValue();
      this.DeleteSelectedAnnotCmd.NotifyCanExecuteChanged();
      if (!((PdfWrapper) value != (PdfWrapper) null))
        return;
      if (MainViewModel.\u003C\u003Eo__271.\u003C\u003Ep__2 == null)
        MainViewModel.\u003C\u003Eo__271.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MainViewModel), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      Func<CallSite, object, bool> target1 = MainViewModel.\u003C\u003Eo__271.\u003C\u003Ep__2.Target;
      CallSite<Func<CallSite, object, bool>> p2 = MainViewModel.\u003C\u003Eo__271.\u003C\u003Ep__2;
      if (MainViewModel.\u003C\u003Eo__271.\u003C\u003Ep__1 == null)
        MainViewModel.\u003C\u003Eo__271.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, MouseModes, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (MainViewModel), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      Func<CallSite, object, MouseModes, object> target2 = MainViewModel.\u003C\u003Eo__271.\u003C\u003Ep__1.Target;
      CallSite<Func<CallSite, object, MouseModes, object>> p1 = MainViewModel.\u003C\u003Eo__271.\u003C\u003Ep__1;
      if (MainViewModel.\u003C\u003Eo__271.\u003C\u003Ep__0 == null)
        MainViewModel.\u003C\u003Eo__271.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof (MainViewModel), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      object obj1 = MainViewModel.\u003C\u003Eo__271.\u003C\u003Ep__0.Target((CallSite) MainViewModel.\u003C\u003Eo__271.\u003C\u003Ep__0, this.ViewerMouseMode);
      object obj2 = target2((CallSite) p1, obj1, MouseModes.Default);
      if (target1((CallSite) p2, obj2))
      {
        if (MainViewModel.\u003C\u003Eo__271.\u003C\u003Ep__3 == null)
          MainViewModel.\u003C\u003Eo__271.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, MouseModes, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Value", typeof (MainViewModel), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        object obj3 = MainViewModel.\u003C\u003Eo__271.\u003C\u003Ep__3.Target((CallSite) MainViewModel.\u003C\u003Eo__271.\u003C\u003Ep__3, this.ViewerMouseMode, MouseModes.Default);
      }
      this.ExitTransientMode();
    }
  }

  public bool IsAnnotationVisible
  {
    get => this.isAnnotationVisible;
    set
    {
      this.SetProperty<bool>(ref this.isAnnotationVisible, value, nameof (IsAnnotationVisible));
    }
  }

  public RelayCommand ExitAnnotationCmd
  {
    get
    {
      return this.exitAnnotationCmd ?? (this.exitAnnotationCmd = new RelayCommand((Action) (() =>
      {
        this.SelectedAnnotation = (PdfAnnotation) null;
        this.AnnotationMode = AnnotationMode.None;
        this.ExitTransientMode();
      })));
    }
  }

  public AsyncRelayCommand ShowHideAnnotationCmd
  {
    get
    {
      return this.showHideAnnotationCmd ?? (this.showHideAnnotationCmd = new AsyncRelayCommand((Func<Task>) (async () =>
      {
        if (this.Document == null)
          return;
        this.IsAnnotationVisible = !this.IsAnnotationVisible;
        CommomLib.Commom.GAManager.SendEvent("MainWindow", "ShowHideAnnotation", "Count", 1L);
      })));
    }
  }

  public AsyncRelayCommand MannageAnnotationCmd
  {
    get
    {
      return this.mannageAnnotationCmd ?? (this.mannageAnnotationCmd = new AsyncRelayCommand((Func<Task>) (async () =>
      {
        if (this.Document == null)
          return;
        CommomLib.Commom.GAManager.SendEvent("MainWindow", "MannageAnnotation", "Count", 1L);
        this.Menus.SelectedLeftNavItem = this.Menus.LeftNavList.First<NavigationModel>((Func<NavigationModel, bool>) (x => x.Name == "Annotation"));
      })));
    }
  }

  public AsyncRelayCommand DeleteSelectedAnnotCmd
  {
    get
    {
      return this.deleteSelectedAnnotCmd ?? (this.deleteSelectedAnnotCmd = new AsyncRelayCommand((Func<Task>) (async () =>
      {
        if (this.Document == null || (PdfWrapper) this.SelectedAnnotation == (PdfWrapper) null)
          return;
        PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.Document);
        if (pdfControl == null)
          return;
        AnnotationHolderManager annotationHolderManager = PdfObjectExtensions.GetAnnotationHolderManager(pdfControl);
        PdfAnnotation selectedAnnotation = this.SelectedAnnotation;
        if (selectedAnnotation != null && selectedAnnotation is PdfFreeTextAnnotation freeTextAnnotation2 && string.IsNullOrEmpty(freeTextAnnotation2.Contents) && freeTextAnnotation2.Intent == AnnotationIntent.FreeTextTypeWriter)
        {
          PdfObjectExtensions.GetAnnotationCanvas(pdfControl).HolderManager.CancelAll();
        }
        else
        {
          if (annotationHolderManager == null)
            return;
          await annotationHolderManager.DeleteAnnotationAsync(this.SelectedAnnotation);
        }
      }), (Func<bool>) (() => !this.DeleteSelectedAnnotCmd.IsRunning && (PdfWrapper) this.SelectedAnnotation != (PdfWrapper) null)));
    }
  }

  public bool IsDeleteAreaVisible
  {
    get => this.isDeleteAreaVisible;
    set
    {
      this.SetProperty<bool>(ref this.isDeleteAreaVisible, value, nameof (IsDeleteAreaVisible));
    }
  }

  public bool? IsSelectedAll
  {
    get => this.isSelectedAll;
    set => this.SetProperty<bool?>(ref this.isSelectedAll, value, nameof (IsSelectedAll));
  }

  public AsyncRelayCommand CanceldeleteAnnotCmd
  {
    get
    {
      return this.canceldeleteAnnotCmd ?? (this.canceldeleteAnnotCmd = new AsyncRelayCommand((Func<Task>) (async () =>
      {
        this.IsDeleteAreaVisible = false;
        this.PageEditors?.NotifyPageAnnotationChanged(0);
      }), (Func<bool>) (() => !this.CanceldeleteAnnotCmd.IsRunning)));
    }
  }

  public AsyncRelayCommand SelectAllAnnotCmd
  {
    get
    {
      return this.selectAllAnnotCmd ?? (this.selectAllAnnotCmd = new AsyncRelayCommand((Func<Task>) (async () =>
      {
        bool? isSelectedAll = this.IsSelectedAll;
        bool flag = false;
        if (isSelectedAll.GetValueOrDefault() == flag & isSelectedAll.HasValue)
        {
          this.IsSelectedAll = new bool?(false);
          if (this.Document == null)
            return;
          PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.Document);
          if (pdfControl == null || PdfObjectExtensions.GetAnnotationHolderManager(pdfControl) == null)
            return;
          foreach (PageCommetCollection commetCollection in (Collection<PageCommetCollection>) this.pageCommetSource)
          {
            for (int index = commetCollection.Count - 1; index >= 0; --index)
            {
              if (commetCollection[index].IsChecked)
                commetCollection[index].IsChecked = false;
            }
          }
        }
        else
        {
          this.IsSelectedAll = new bool?(true);
          if (this.Document == null)
            return;
          PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.Document);
          if (pdfControl == null || PdfObjectExtensions.GetAnnotationHolderManager(pdfControl) == null)
            return;
          foreach (PageCommetCollection commetCollection in (Collection<PageCommetCollection>) this.pageCommetSource)
          {
            for (int index = commetCollection.Count - 1; index >= 0; --index)
            {
              if (!commetCollection[index].IsChecked)
                commetCollection[index].IsChecked = true;
            }
          }
        }
      }), (Func<bool>) (() => !this.SelectAllAnnotCmd.IsRunning)));
    }
  }

  public AsyncRelayCommand BatchdeleteAnnotCmd
  {
    get
    {
      return this.batchdeleteAnnotCmd ?? (this.batchdeleteAnnotCmd = new AsyncRelayCommand((Func<Task>) (async () =>
      {
        this.IsSelectedAll = new bool?(false);
        this.PageEditors?.NotifyPageAnnotationChanged(0);
      }), (Func<bool>) (() => !this.BatchdeleteAnnotCmd.IsRunning)));
    }
  }

  public AsyncRelayCommand DeleteAnnotCmd
  {
    get
    {
      return this.deleteAnnotCmd ?? (this.deleteAnnotCmd = new AsyncRelayCommand((Func<Task>) (async () =>
      {
        MainViewModel mainViewModel1 = this;
        if (mainViewModel1.Document == null)
          return;
        string strLabel = "DelPart";
        bool? isSelectedAll = mainViewModel1.IsSelectedAll;
        bool flag = false;
        if (isSelectedAll.GetValueOrDefault() == flag & isSelectedAll.HasValue)
        {
          int num = (int) ModernMessageBox.Show(Resources.BatchDeleteSelectNoneWarning, "PDFgear", MessageBoxButton.YesNo);
        }
        else
        {
          if (mainViewModel1.isSelectedAll.GetValueOrDefault())
            strLabel = "DelAll";
          CommomLib.Commom.GAManager.SendEvent("AnnotationMgmt", "BatchDeleteBtn", strLabel, 1L);
          PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(mainViewModel1.Document);
          if (pdfControl != null)
          {
            MainViewModel mainViewModel = mainViewModel1;
            AnnotationHolderManager holders = PdfObjectExtensions.GetAnnotationHolderManager(pdfControl);
            if (holders != null)
            {
              if (ModernMessageBox.Show(Resources.BatchDeleteWarning, "PDFgear", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
              List<PdfAnnotation> annotations = new List<PdfAnnotation>();
              foreach (PageCommetCollection commetCollection in (Collection<PageCommetCollection>) mainViewModel1.pageCommetSource)
              {
                for (int index = commetCollection.Count - 1; index >= 0; --index)
                {
                  if (commetCollection[index].IsChecked)
                    annotations.Add(mainViewModel1.Document.Pages[commetCollection[index].Annotation.PageIndex].Annots[commetCollection[index].Annotation.AnnotIndex]);
                }
              }
              ProgressUtils.ShowProgressBar((Func<ProgressUtils.ProgressAction, Task>) (async c =>
              {
                Progress<double> progress = new Progress<double>();
                progress.ProgressChanged += (EventHandler<double>) ((s, a) => c.Report(a));
                await holders.BatchDeleteAnnotationsAsync((System.Collections.Generic.IReadOnlyList<PdfAnnotation>) annotations, (IProgress<double>) progress, c.CancellationToken);
                mainViewModel.SetCanSaveFlag("BatchDeleteAnnotation", true);
                c.Complete();
              }), (string) null, (object) Resources.BatchDeletingTitle, true, (Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>(), 100);
              mainViewModel1.pageCommetSource.NotifyDeletePageAnnotationChanged();
            }
          }
          isSelectedAll = mainViewModel1.IsSelectedAll;
          if (isSelectedAll.GetValueOrDefault())
            mainViewModel1.IsDeleteAreaVisible = false;
          mainViewModel1.IsSelectedAll = new bool?(false);
        }
      }), (Func<bool>) (() => !this.DeleteAnnotCmd.IsRunning)));
    }
  }

  public async Task<bool> TrySaveBeforeCloseDocumentAsync()
  {
    if (this.documentClosing)
      return false;
    if (this.Document == null)
      return true;
    try
    {
      this.documentClosing = true;
      if (!this.CanSave)
        return true;
      FileInfo fileInfo = new FileInfo(this.DocumentWrapper.DocumentPath);
      switch (MessageBox.Show((Resources.SaveDocBeforeCloseMsg ?? "").Replace("XXX", fileInfo.Name), "PDFgear", MessageBoxButton.YesNoCancel))
      {
        case MessageBoxResult.Yes:
          if (!this.SaveCmd.IsRunning || this.SaveCmd.ExecutionTask == null)
            await this.SaveCmd.ExecuteAsync((object) null);
          else
            await this.SaveCmd.ExecutionTask;
          this.DelAutoSaveFile(this.DocumentWrapper.DocumentPath);
          return !this.CanSave;
        case MessageBoxResult.No:
          this.DelAutoSaveFile(this.DocumentWrapper.DocumentPath);
          return true;
        default:
          return false;
      }
    }
    finally
    {
      this.documentClosing = false;
    }
  }

  public RelayCommand OpenImgCmd
  {
    get
    {
      return this.openImgCmd ?? (this.openImgCmd = new RelayCommand((Action) (() => { }), (Func<bool>) (() => true)));
    }
  }

  public BookmarkModel SelectedBookmark
  {
    get => this.selectedBookmark;
    set
    {
      if (!this.SetProperty<BookmarkModel>(ref this.selectedBookmark, value, nameof (SelectedBookmark)))
        return;
      this.BookmarkRemoveCommand.NotifyCanExecuteChanged();
    }
  }

  public AsyncRelayCommand<BookmarkModel> BookmarkAddCommand
  {
    get
    {
      return this.bookmarkAddCommand ?? (this.bookmarkAddCommand = new AsyncRelayCommand<BookmarkModel>((Func<BookmarkModel, Task>) (async c =>
      {
        BookmarkModel bookmarkModel = c;
        if (bookmarkModel == null)
        {
          BookmarkModel selectedBookmark = this.SelectedBookmark;
          if (selectedBookmark == null)
          {
            BookmarkModel bookmarks = this.Bookmarks;
            bookmarkModel = bookmarks != null ? bookmarks.Children.LastOrDefault<BookmarkModel>() : (BookmarkModel) null;
          }
          else
            bookmarkModel = selectedBookmark;
        }
        BookmarkModel prev = bookmarkModel;
        BookmarkModel parent = prev?.Parent;
        CommomLib.Commom.GAManager.SendEvent("Bookmark", "AddBookmark", "All", 1L);
        BookmarkRenameDialog bookmarkRenameDialog = BookmarkRenameDialog.Create(Resources.NewBookmarkName);
        if (!bookmarkRenameDialog.ShowDialog().GetValueOrDefault())
          return;
        int num = await this.AddBookmarkAsync(prev, parent, bookmarkRenameDialog.BookmarkTitle, this.SelectedPageIndex, new FS_RECTF?()) ? 1 : 0;
      }), (Predicate<BookmarkModel>) (c => !this.BookmarkAddCommand.IsRunning)));
    }
  }

  public AsyncRelayCommand<BookmarkModel> BookmarkAddCommand2
  {
    get
    {
      return this.bookmarkAddCommand2 ?? (this.bookmarkAddCommand2 = new AsyncRelayCommand<BookmarkModel>((Func<BookmarkModel, Task>) (async c =>
      {
        BookmarkModel bookmarkModel = c;
        if (bookmarkModel == null)
        {
          BookmarkModel selectedBookmark = this.SelectedBookmark;
          if (selectedBookmark == null)
          {
            BookmarkModel bookmarks = this.Bookmarks;
            bookmarkModel = bookmarks != null ? bookmarks.Children.LastOrDefault<BookmarkModel>() : (BookmarkModel) null;
          }
          else
            bookmarkModel = selectedBookmark;
        }
        BookmarkModel prev = bookmarkModel;
        BookmarkModel parent = prev?.Parent;
        CommomLib.Commom.GAManager.SendEvent("Bookmark", "AddBookmark", "All", 1L);
        BookmarkRenameDialog bookmarkRenameDialog = BookmarkRenameDialog.Create(Resources.NewBookmarkName);
        if (!bookmarkRenameDialog.ShowDialog().GetValueOrDefault())
          return;
        FS_RECTF selectedDestination = this.GetSelectedDestination();
        int num = await this.AddBookmarkAsync(prev, parent, bookmarkRenameDialog.BookmarkTitle, this.SelectedPageIndex, new FS_RECTF?(selectedDestination)) ? 1 : 0;
      }), (Predicate<BookmarkModel>) (c => !this.BookmarkAddCommand.IsRunning)));
    }
  }

  private FS_RECTF GetSelectedDestination()
  {
    PdfViewer viewer = PDFKit.PdfControl.GetPdfControl(this.Document)?.Viewer;
    foreach (TextInfo textInfo in PdfTextMarkupAnnotationUtils.GetTextInfos(this.Document, viewer.SelectInfo).ToArray<TextInfo>())
    {
      using (IEnumerator<FS_RECTF> enumerator = PdfTextMarkupAnnotationUtils.GetNormalizedRects(viewer, textInfo, true, true).GetEnumerator())
      {
        if (enumerator.MoveNext())
          return enumerator.Current;
      }
    }
    return new FS_RECTF();
  }

  public AsyncRelayCommand<BookmarkModel> BookmarkAddChildCommand
  {
    get
    {
      return this.bookmarkAddChildCommand ?? (this.bookmarkAddChildCommand = new AsyncRelayCommand<BookmarkModel>((Func<BookmarkModel, Task>) (async c =>
      {
        if (c == null)
          return;
        BookmarkModel prev = c.Children.LastOrDefault<BookmarkModel>();
        BookmarkModel parent = c;
        CommomLib.Commom.GAManager.SendEvent("Bookmark", "AddBookmarkChild", "All", 1L);
        BookmarkRenameDialog bookmarkRenameDialog = BookmarkRenameDialog.Create(Resources.NewBookmarkName);
        if (!bookmarkRenameDialog.ShowDialog().GetValueOrDefault())
          return;
        int num = await this.AddBookmarkAsync(prev, parent, bookmarkRenameDialog.BookmarkTitle, this.SelectedPageIndex, new FS_RECTF?()) ? 1 : 0;
      }), (Predicate<BookmarkModel>) (c => !this.BookmarkAddChildCommand.IsRunning)));
    }
  }

  public AsyncRelayCommand<BookmarkModel> BookmarkRemoveCommand
  {
    get
    {
      return this.bookmarkRemoveCommand ?? (this.bookmarkRemoveCommand = new AsyncRelayCommand<BookmarkModel>((Func<BookmarkModel, Task>) (async c =>
      {
        CommomLib.Commom.GAManager.SendEvent("Bookmark", "RemoveBookmark", "All", 1L);
        int num = await this.RemoveBookmarkAsync(c ?? this.SelectedBookmark) ? 1 : 0;
      }), (Predicate<BookmarkModel>) (c => (c ?? this.SelectedBookmark) != null && !this.BookmarkRemoveCommand.IsRunning)));
    }
  }

  private async Task<bool> AddBookmarkAsync(
    BookmarkModel prev,
    BookmarkModel parent,
    string title,
    int pageIndex,
    FS_RECTF? destination)
  {
    if (this.Document == null)
      return false;
    if (parent == null)
      parent = prev?.Parent;
    if (parent == null)
      parent = this.Bookmarks;
    if (prev?.Parent == null)
      prev = (BookmarkModel) null;
    if (prev?.Parent != null && parent != null && prev.Parent != parent)
      return false;
    await this.Menus.ShowLeftNavMenuAsync("Bookmark");
    int num1 = -1;
    if (prev != null)
    {
      for (int index = 0; index < parent.Children.Count; ++index)
      {
        if (parent.Children[index] == prev)
        {
          num1 = index;
          break;
        }
      }
      if (num1 == -1)
        return false;
    }
    int num2 = num1 + 1;
    BookmarkRecord record = new BookmarkRecord()
    {
      Title = title ?? "",
      Index = num2,
      Destination = new BookmarkRecord.BookmarkDestination()
      {
        DestinationType = DestinationTypes.XYZ,
        PageIndex = pageIndex
      }
    };
    if (destination.HasValue)
    {
      record.Destination.Left = new float?(destination.Value.left);
      record.Destination.Top = new float?(destination.Value.top);
      record.Destination.Right = new float?(destination.Value.right);
      record.Destination.Bottom = new float?(destination.Value.bottom);
    }
    BookmarkModel result = await this.OperationManager.InsertBookmarkAsync(this.Document, parent, record);
    if (result == null)
      return false;
    this.SelectedBookmark = (BookmarkModel) null;
    DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() => this.SelectedBookmark = result));
    return true;
  }

  private async Task<bool> RemoveBookmarkAsync(BookmarkModel model)
  {
    if (this.Document == null || model == null)
      return false;
    if (model == this.SelectedBookmark)
      this.SelectedBookmark = (BookmarkModel) null;
    if (model.Children.Count > 0 && !ConfigManager.GetDoNotShowFlag("NotShowDigitalSignatureCreateTipFlag"))
    {
      MessageBoxHelper.RichMessageBoxResult messageBoxResult = MessageBoxHelper.Show(new MessageBoxHelper.RichMessageBoxContent()
      {
        Content = (object) Resources.DeleteParentBookmarkWarning,
        ShowLeftBottomCheckbox = true,
        LeftBottomCheckboxContent = Resources.WinPwdPasswordSaveTipNotshowagainContent
      }, UtilManager.GetProductName(), MessageBoxButton.YesNo);
      if (messageBoxResult.Result == MessageBoxResult.Yes)
      {
        bool? checkboxResult = messageBoxResult.CheckboxResult;
        if (checkboxResult.HasValue && checkboxResult.GetValueOrDefault())
          ConfigManager.SetDoNotShowFlag("NotShowDigitalSignatureCreateTipFlag", true);
      }
      if (messageBoxResult.Result != MessageBoxResult.Yes)
        return false;
    }
    return await this.OperationManager.RemoveBookmarkAsync(this.Document, model);
  }

  private BookmarkModel GetNextBookmark(BookmarkModel model)
  {
    List<BookmarkModel> list = model.Parent.Children.ToList<BookmarkModel>();
    int num = list.IndexOf(model);
    if (num == -1)
      return (BookmarkModel) null;
    if (num > 0)
      return list[num - 1];
    return !model.IsTopLevelModel && model.Parent != null ? model.Parent : (BookmarkModel) null;
  }

  public void DelAutoSaveFile(string filePath)
  {
    try
    {
      CommomLib.Commom.AutoSaveManager.DelTempFileByCloseExe(Process.GetCurrentProcess().Id, filePath);
    }
    catch (Exception ex)
    {
      throw;
    }
  }

  public static void RotatePageCore(PdfDocument doc, IEnumerable<int> pageIdxs, bool rotateRight)
  {
    MainViewModel.RotatePageCoreAsync(doc, pageIdxs, rotateRight);
  }

  public static async Task RotatePageCoreAsync(
    PdfDocument doc,
    IEnumerable<int> pageIdxs,
    bool rotateRight,
    IProgress<double> progress = null)
  {
    int count = pageIdxs.Count<int>();
    int progressIdx = 0;
    progress?.Report(0.0);
    foreach (int index in pageIdxs)
    {
      if (index >= 0 && index <= doc.Pages.Count)
      {
        PdfPage page = doc.Pages[index];
        PageRotate rotation = page.Rotation;
        page.Rotation = !rotateRight ? (rotation <= PageRotate.Normal ? PageRotate.Rotate270 : rotation - 1) : (rotation >= PageRotate.Rotate270 ? PageRotate.Normal : rotation + 1);
        StrongReferenceMessenger.Default.Send<ValueChangedMessage<int>, string>(new ValueChangedMessage<int>(index), "MESSAGE_PAGE_ROTATE_CHANGED");
        ++progressIdx;
        if (count > 40 && progress != null && progressIdx % 10 == 0)
          await Task.Yield();
        progress?.Report((double) progressIdx * 1.0 / (double) count);
      }
    }
    progress?.Report(1.0);
  }

  public void ExitTransientMode(
    bool fromShotScreen = false,
    bool fromEditText = false,
    bool fromAutoScroll = false,
    bool formLink = false)
  {
    DataOperationModel viewerOperationModel = this.ViewerOperationModel;
    if (viewerOperationModel != null)
    {
      if (!viewerOperationModel.IsDisposed)
        viewerOperationModel.Dispose();
      this.ViewerOperationModel = (DataOperationModel) null;
    }
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.Document);
    AnnotationCanvas annotationCanvas = PdfObjectExtensions.GetAnnotationCanvas(pdfControl);
    if (!fromShotScreen && annotationCanvas != null)
      annotationCanvas.CloseScreenShot();
    if (!fromEditText)
    {
      this.EditingPageObjectType = PageObjectType.None;
      if (this.viewToolbarViewModel?.EditPageTextObjectButtonModel != null)
        this.viewToolbarViewModel.EditPageTextObjectButtonModel.IsChecked = false;
    }
    if (!fromAutoScroll)
      this.viewToolbarViewModel?.StopAutoScroll();
    if (annotationCanvas != null && annotationCanvas.ImageControl.Visibility != Visibility.Collapsed)
    {
      annotationCanvas.ImageControl.Visibility = Visibility.Collapsed;
      annotationCanvas.ImageControl.quitImageControl();
    }
    if (formLink || pdfControl == null)
      return;
    pdfControl.Viewer.IsLinkAnnotationHighlighted = false;
    if (this.annotationToolbarViewModel.LinkButtonModel.IsChecked || !(this.selectedAnnotation is PdfLinkAnnotation))
      return;
    this.ReleaseViewerFocusAsync(true);
  }

  public bool ChatButtonVisible
  {
    get => this.chatButtonVisible;
    set
    {
      if (!this.SetProperty<bool>(ref this.chatButtonVisible, value, nameof (ChatButtonVisible)))
        return;
      this.OnPropertyChanged("ChatButtonActualVisible");
    }
  }

  public bool ChatPanelVisible
  {
    get => this.chatPanelVisible;
    set
    {
      if (!this.SetProperty<bool>(ref this.chatPanelVisible, value, nameof (ChatPanelVisible)))
        return;
      this.OnPropertyChanged("ChatButtonActualVisible");
    }
  }

  public bool ChatButtonActualVisible => this.ChatButtonVisible && !this.ChatPanelVisible;

  public async Task CreatePdfFileAsync()
  {
    string blankPageAsync = CreateFileHelper.CreateBlankPageAsync();
    if (string.IsNullOrEmpty(blankPageAsync))
      return;
    if (this.Document != null)
    {
      CreateFileHelper.OpenPDFFile(blankPageAsync, "open:CreatedFile");
    }
    else
    {
      if (!await this.OpenDocumentCoreAsync(blankPageAsync))
        return;
      this.DocumentWrapper.SetUntitledFile();
      this.SetCanSaveFlag("CreateNew", false);
      pdfeditor.AutoSaveRestore.AutoSaveManager.GetInstance().LastOperationVersion = "CreateNew";
    }
  }

  public enum SelectReadPages
  {
    CurrentPage,
    FormCurrentPage,
    AllPage,
  }
}
