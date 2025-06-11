// Decompiled with JetBrains decompiler
// Type: pdfeditor.ViewModels.ViewToolbarViewModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using CommomLib.Commom;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using pdfeditor.Controls;
using pdfeditor.Controls.Annotations.Holders;
using pdfeditor.Controls.Presentation;
using pdfeditor.Controls.Speech;
using pdfeditor.Models.Menus;
using pdfeditor.Models.Menus.ToolbarSettings;
using pdfeditor.Models.Viewer;
using pdfeditor.Properties;
using pdfeditor.Services;
using pdfeditor.Utils;
using pdfeditor.Utils.Enums;
using PDFKit;
using PDFKit.Contents.Utils;
using PDFKit.Utils;
using PDFKit.Utils.PageContents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#nullable enable
namespace pdfeditor.ViewModels;

public class ViewToolbarViewModel : ObservableObject
{
  private readonly 
  #nullable disable
  MainViewModel mainViewModel;
  private const float ZoomMinValue = 0.01f;
  private const float ZoomMaxValue = 64f;
  private const float ZoomStep = 0.2f;
  private SelectableContextMenuItemModel convertMenuItems;
  private SelectableContextMenuItemModel backgroundMenuItems;
  private SelectableContextMenuItemModel autoScrollMenuItems;
  private ToolbarAnnotationButtonModel convertButtonModel;
  private ToolbarAnnotationButtonModel backgroundButtonModel;
  private ToolbarButtonModel autoScrollButtonModel;
  private AsyncRelayCommand pageRotateLeftCmd;
  private AsyncRelayCommand pageRotateRightCmd;
  private ViewModes docViewMode;
  private SubViewModePage subViewModePage;
  private SubViewModeContinuous subViewModeContinuous;
  private SizeModes docSizeMode;
  private SizeModesWrap docSizeModeWrap;
  private float docZoom = 1f;
  private RelayCommand docZoomInCmd;
  private RelayCommand docZoomoutCmd;
  private RelayCommand gotoPrevPageCmd;
  private RelayCommand gotoNextPageCmd;
  private RelayCommand gotoFirstPageCmd;
  private RelayCommand gotoLastPageCmd;
  private RelayCommand gotoPrevViewCmd;
  private RelayCommand gotoNextViewCmd;
  private ToolbarButtonModel editPageTextObjectButtonModel;
  private ToolbarButtonModel editDocumentButtomModel;
  private ToolbarButtonModel presentButtonModel;
  private ToolbarSettingModel editDocumentToolbarSetting;
  private ToolbarSettingModel editDocumentToolbarSettingCache;
  private int autoScrollSpeed;
  private ToolbarButtonModel readButtonModel;

  public ViewToolbarViewModel(MainViewModel mainViewModel)
  {
    this.mainViewModel = mainViewModel;
    this.InitViewerButton();
  }

  public ToolbarButtonModel EditPageTextObjectButtonModel
  {
    get => this.editPageTextObjectButtonModel;
    set
    {
      this.SetProperty<ToolbarButtonModel>(ref this.editPageTextObjectButtonModel, value, nameof (EditPageTextObjectButtonModel));
    }
  }

  public ToolbarButtonModel EditDocumentButtomModel
  {
    get => this.editDocumentButtomModel;
    set
    {
      this.SetProperty<ToolbarButtonModel>(ref this.editDocumentButtomModel, value, nameof (EditDocumentButtomModel));
    }
  }

  public ToolbarButtonModel PresentButtonModel
  {
    get => this.presentButtonModel;
    set
    {
      this.SetProperty<ToolbarButtonModel>(ref this.presentButtonModel, value, nameof (PresentButtonModel));
    }
  }

  public ToolbarSettingModel EditDocumentToolbarSetting
  {
    get => this.editDocumentToolbarSetting;
    set
    {
      if (!this.SetProperty<ToolbarSettingModel>(ref this.editDocumentToolbarSetting, value, nameof (EditDocumentToolbarSetting)))
        return;
      this.mainViewModel.AnnotationToolbar.NotifyPropertyChanged("CheckedButtonToolbarSetting");
    }
  }

  public SelectableContextMenuItemModel ConvertMenuItems
  {
    get => this.convertMenuItems;
    set
    {
      this.SetProperty<SelectableContextMenuItemModel>(ref this.convertMenuItems, value, nameof (ConvertMenuItems));
    }
  }

  public SelectableContextMenuItemModel BackgroundMenuItems
  {
    get => this.backgroundMenuItems;
    set
    {
      this.SetProperty<SelectableContextMenuItemModel>(ref this.backgroundMenuItems, value, nameof (BackgroundMenuItems));
    }
  }

  public SelectableContextMenuItemModel AutoScrollMenuItems
  {
    get => this.autoScrollMenuItems;
    set
    {
      this.SetProperty<SelectableContextMenuItemModel>(ref this.autoScrollMenuItems, value, nameof (AutoScrollMenuItems));
    }
  }

  public ToolbarAnnotationButtonModel ConvertButtonModel
  {
    get => this.convertButtonModel;
    set
    {
      this.SetProperty<ToolbarAnnotationButtonModel>(ref this.convertButtonModel, value, nameof (ConvertButtonModel));
    }
  }

  public ToolbarAnnotationButtonModel BackgroundButtonModel
  {
    get => this.backgroundButtonModel;
    set
    {
      this.SetProperty<ToolbarAnnotationButtonModel>(ref this.backgroundButtonModel, value, nameof (BackgroundButtonModel));
    }
  }

  public ToolbarButtonModel AutoScrollButtonModel
  {
    get => this.autoScrollButtonModel;
    set
    {
      this.SetProperty<ToolbarButtonModel>(ref this.autoScrollButtonModel, value, nameof (AutoScrollButtonModel));
    }
  }

  public AsyncRelayCommand PageRotateLeftCmd
  {
    get
    {
      return this.pageRotateLeftCmd ?? (this.pageRotateLeftCmd = new AsyncRelayCommand((Func<Task>) (async () => await this.PageRotateLeft())));
    }
  }

  private async Task PageRotateLeft()
  {
    PDFKit.PdfControl viewer;
    if (this.mainViewModel.Document == null)
      viewer = (PDFKit.PdfControl) null;
    else if (this.mainViewModel.Document.Pages == null)
    {
      viewer = (PDFKit.PdfControl) null;
    }
    else
    {
      int curpgindex = this.mainViewModel.Document.Pages.CurrentIndex;
      if (curpgindex < 0)
        viewer = (PDFKit.PdfControl) null;
      else if (curpgindex > this.mainViewModel.Document.Pages.Count)
      {
        viewer = (PDFKit.PdfControl) null;
      }
      else
      {
        CommomLib.Commom.GAManager.SendEvent("MainWindow", "PageRotate", "Left", 1L);
        viewer = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
        AnnotationHolderManager annotationHolderManager = PdfObjectExtensions.GetAnnotationHolderManager(viewer);
        if (annotationHolderManager != null)
        {
          annotationHolderManager.CancelAll();
          await annotationHolderManager.WaitForCancelCompletedAsync();
        }
        MainViewModel.RotatePageCore(this.mainViewModel.Document, (IEnumerable<int>) new int[1]
        {
          curpgindex
        }, false);
        if (viewer != null && viewer.Document != null)
        {
          viewer.UpdateDocLayout();
          this.mainViewModel.SelectedPageIndex = curpgindex;
        }
        this.mainViewModel.OperationManager.AddOperationAsync((Action<PdfDocument>) (doc =>
        {
          MainViewModel.RotatePageCore(doc, (IEnumerable<int>) new int[1]
          {
            curpgindex
          }, true);
          PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
          if (pdfControl == null || pdfControl.Document == null)
            return;
          pdfControl.UpdateDocLayout();
        }), (Action<PdfDocument>) (doc =>
        {
          MainViewModel.RotatePageCore(this.mainViewModel.Document, (IEnumerable<int>) new int[1]
          {
            curpgindex
          }, false);
          PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
          if (pdfControl == null || pdfControl.Document == null)
            return;
          pdfControl.UpdateDocLayout();
        }));
        viewer = (PDFKit.PdfControl) null;
      }
    }
  }

  public AsyncRelayCommand PageRotateRightCmd
  {
    get
    {
      return this.pageRotateRightCmd ?? (this.pageRotateRightCmd = new AsyncRelayCommand((Func<Task>) (async () => await this.PageRotateRight())));
    }
  }

  private async Task PageRotateRight()
  {
    PDFKit.PdfControl viewer;
    if (this.mainViewModel.Document == null)
      viewer = (PDFKit.PdfControl) null;
    else if (this.mainViewModel.Document.Pages == null)
    {
      viewer = (PDFKit.PdfControl) null;
    }
    else
    {
      int curpgindex = this.mainViewModel.Document.Pages.CurrentIndex;
      if (curpgindex < 0)
        viewer = (PDFKit.PdfControl) null;
      else if (curpgindex > this.mainViewModel.Document.Pages.Count)
      {
        viewer = (PDFKit.PdfControl) null;
      }
      else
      {
        CommomLib.Commom.GAManager.SendEvent("MainWindow", "PageRotate", "Right", 1L);
        viewer = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
        AnnotationHolderManager annotationHolderManager = PdfObjectExtensions.GetAnnotationHolderManager(viewer);
        if (annotationHolderManager != null)
        {
          annotationHolderManager.CancelAll();
          await annotationHolderManager.WaitForCancelCompletedAsync();
        }
        MainViewModel.RotatePageCore(this.mainViewModel.Document, (IEnumerable<int>) new int[1]
        {
          curpgindex
        }, true);
        if (viewer != null && viewer.Document != null)
        {
          viewer.UpdateDocLayout();
          this.mainViewModel.SelectedPageIndex = curpgindex;
        }
        this.mainViewModel.OperationManager.AddOperationAsync((Action<PdfDocument>) (doc =>
        {
          MainViewModel.RotatePageCore(doc, (IEnumerable<int>) new int[1]
          {
            curpgindex
          }, false);
          PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
          if (pdfControl == null || pdfControl.Document == null)
            return;
          pdfControl.UpdateDocLayout();
        }), (Action<PdfDocument>) (doc =>
        {
          MainViewModel.RotatePageCore(doc, (IEnumerable<int>) new int[1]
          {
            curpgindex
          }, true);
          PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
          if (pdfControl == null || pdfControl.Document == null)
            return;
          pdfControl.UpdateDocLayout();
        }));
        viewer = (PDFKit.PdfControl) null;
      }
    }
  }

  public ViewModes DocViewMode
  {
    get => this.docViewMode;
    set => this.SetProperty<ViewModes>(ref this.docViewMode, value, nameof (DocViewMode));
  }

  public SubViewModePage SubViewModePage
  {
    get => this.subViewModePage;
    set
    {
      this.SetProperty<SubViewModePage>(ref this.subViewModePage, value, nameof (SubViewModePage));
      this.UpdateDocumentViewMode();
    }
  }

  public SubViewModeContinuous SubViewModeContinuous
  {
    get => this.subViewModeContinuous;
    set
    {
      this.SetProperty<SubViewModeContinuous>(ref this.subViewModeContinuous, value, nameof (SubViewModeContinuous));
      this.UpdateDocumentViewMode();
    }
  }

  private void UpdateDocumentViewMode()
  {
    ConfigManager.SetPageDisplayModeAsync(this.SubViewModePage.ToString(), this.SubViewModeContinuous.ToString());
    if (this.subViewModePage == SubViewModePage.SinglePage)
    {
      if (this.subViewModeContinuous == SubViewModeContinuous.Discontinuous)
      {
        this.DocViewMode = ViewModes.SinglePage;
        this.StopAutoScroll();
      }
      else if (this.subViewModeContinuous == SubViewModeContinuous.Verticalcontinuous)
      {
        this.DocViewMode = ViewModes.Vertical;
      }
      else
      {
        if (this.subViewModeContinuous != SubViewModeContinuous.Horizontalcontinuous)
          return;
        this.DocViewMode = ViewModes.Horizontal;
      }
    }
    else
    {
      if (this.subViewModePage != SubViewModePage.DoublePages)
        return;
      if (this.subViewModeContinuous == SubViewModeContinuous.Discontinuous)
      {
        this.DocViewMode = ViewModes.TilesLine;
        this.StopAutoScroll();
      }
      else if (this.subViewModeContinuous == SubViewModeContinuous.Verticalcontinuous)
      {
        this.DocViewMode = ViewModes.TilesVertical;
      }
      else
      {
        if (this.subViewModeContinuous != SubViewModeContinuous.Horizontalcontinuous)
          return;
        this.DocViewMode = ViewModes.TilesHorizontal;
      }
    }
  }

  public SizeModes DocSizeMode
  {
    get => this.docSizeMode;
    set
    {
      if (!this.SetProperty<SizeModes>(ref this.docSizeMode, value, nameof (DocSizeMode)))
        return;
      this.UpdateSizeModeState();
    }
  }

  public SizeModesWrap DocSizeModeWrap
  {
    get => this.docSizeModeWrap;
    set
    {
      if (!this.SetProperty<SizeModesWrap>(ref this.docSizeModeWrap, value, nameof (DocSizeModeWrap)))
        return;
      if (value != SizeModesWrap.Zoom)
      {
        ConfigManager.SetPageSizeModeAsync(value.ToString());
        ConfigManager.SetPageSizeZoomModelAsync(this.mainViewModel.DocumentWrapper?.DocumentPath, value.ToString(), this.DocZoom);
      }
      this.UpdateSizeModeOccordingtoWrap();
    }
  }

  private void UpdateSizeModeOccordingtoWrap()
  {
    switch (this.DocSizeModeWrap)
    {
      case SizeModesWrap.FitToSize:
        this.DocSizeMode = SizeModes.FitToSize;
        break;
      case SizeModesWrap.FitToWidth:
        this.DocSizeMode = SizeModes.FitToWidth;
        break;
      case SizeModesWrap.FitToHeight:
        this.DocSizeMode = SizeModes.FitToHeight;
        break;
      case SizeModesWrap.Zoom:
        this.DocSizeMode = SizeModes.Zoom;
        break;
      case SizeModesWrap.ZoomActualSize:
        this.DocSizeMode = SizeModes.Zoom;
        this.DocZoom = 1f;
        break;
      default:
        this.DocSizeMode = SizeModes.Zoom;
        break;
    }
  }

  private void UpdateSizeModeState()
  {
    switch (this.DocSizeMode)
    {
      case SizeModes.FitToSize:
        this.DocSizeModeWrap = SizeModesWrap.FitToSize;
        break;
      case SizeModes.FitToWidth:
        this.DocSizeModeWrap = SizeModesWrap.FitToWidth;
        break;
      case SizeModes.FitToHeight:
        this.DocSizeModeWrap = SizeModesWrap.FitToHeight;
        break;
      case SizeModes.Zoom:
        if ((double) this.DocZoom == 1.0)
        {
          this.DocSizeModeWrap = SizeModesWrap.ZoomActualSize;
          break;
        }
        this.DocSizeModeWrap = SizeModesWrap.Zoom;
        break;
    }
  }

  public float DocZoom
  {
    get => this.docZoom;
    set
    {
      if (this.SetProperty<float>(ref this.docZoom, value, nameof (DocZoom)))
        this.UpdateSizeModeState();
      this.DocZoomOutCmd.NotifyCanExecuteChanged();
      this.DocZoomInCmd.NotifyCanExecuteChanged();
    }
  }

  public RelayCommand DocZoomInCmd
  {
    get
    {
      return this.docZoomInCmd ?? (this.docZoomInCmd = new RelayCommand((Action) (() => this.DocZoomIn()), (Func<bool>) (() => this.CanDocZoomIn())));
    }
  }

  public void DocZoomIn(bool zoomFromMousePoint = false, float zoomStep = 0.2f)
  {
    if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null || (double) this.DocZoom >= 64.0)
      return;
    this.UpdateDocToZoom(Math.Min(this.DocZoom + zoomStep, 64f), zoomFromMousePoint);
  }

  private bool CanDocZoomIn() => (double) this.DocZoom < 63.9999;

  public RelayCommand DocZoomOutCmd
  {
    get
    {
      return this.docZoomoutCmd ?? (this.docZoomoutCmd = new RelayCommand((Action) (() => this.DocZoomOut()), (Func<bool>) (() => this.CanDocZoomOut())));
    }
  }

  public void DocZoomOut(bool zoomFromMousePoint = false, float zoomStep = 0.2f)
  {
    if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null || (double) this.DocZoom <= 0.0099999997764825821)
      return;
    this.UpdateDocToZoom(Math.Max(this.DocZoom - zoomStep, 0.01f), zoomFromMousePoint);
  }

  private bool CanDocZoomOut() => (double) this.DocZoom > 0.010099999776482581;

  public void UpdateDocToZoom(float newzoom, bool zoomFromMousePoint = false, Point? mousePointOverride = null)
  {
    if (this.DocSizeModeWrap != SizeModesWrap.Zoom)
      this.DocSizeModeWrap = SizeModesWrap.Zoom;
    if (this.DocSizeMode != SizeModes.Zoom)
      this.DocSizeMode = SizeModes.Zoom;
    PDFKit.PdfControl pdfControl = (PDFKit.PdfControl) null;
    ScrollAnchorPointUtils.PdfViewerZoomPointSnapshot snapshot = (ScrollAnchorPointUtils.PdfViewerZoomPointSnapshot) null;
    if (zoomFromMousePoint)
    {
      pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
      snapshot = ScrollAnchorPointUtils.CreateZoomPointSnapshot(pdfControl, mousePointOverride);
      if (snapshot != null && snapshot.MousePointPageIndex == -1 && snapshot.CenterPointPageIndex == -1)
        snapshot = (ScrollAnchorPointUtils.PdfViewerZoomPointSnapshot) null;
    }
    try
    {
      if (pdfControl != null)
        pdfControl.IsRenderPaused = true;
      this.DocZoom = newzoom;
      ConfigManager.SetPageSizeZoomModelAsync(this.mainViewModel.DocumentWrapper?.DocumentPath, this.DocSizeModeWrap.ToString(), this.DocZoom);
      if (pdfControl == null)
        return;
      pdfControl.UpdateLayout();
      ScrollAnchorPointUtils.ApplyZoomScrollOffset(pdfControl, snapshot);
    }
    finally
    {
      if (pdfControl != null)
        pdfControl.IsRenderPaused = false;
    }
  }

  private void InitViewerButton()
  {
    this.convertMenuItems = ToolbarContextMenuHelper.CreateConverterContextMenu(new Action<ContextMenuItemModel>(this.DoConvertMenuItemCmd));
    this.backgroundMenuItems = ToolbarContextMenuHelper.CreateBackgroundContextMenu("", new Action<ContextMenuItemModel>(this.DoBackgroundMenuItemCmd));
    this.autoScrollMenuItems = ToolbarContextMenuHelper.CreateAutoScrollContextMenu(1, (Action<ContextMenuItemModel>) (model =>
    {
      this.AutoScrollSpeed = Convert.ToInt32(model.TagData.MenuItemValue);
      ConfigManager.SetAutoScrollSpeedAsync(this.AutoScrollSpeed).GetAwaiter().GetResult();
      if (this.AutoScrollButtonModel.IsChecked || this.mainViewModel.Document == null)
        return;
      this.AutoScrollButtonModel.Tap();
    }));
    ToolbarAnnotationButtonModel annotationButtonModel1 = new ToolbarAnnotationButtonModel(AnnotationMode.None);
    annotationButtonModel1.Caption = Resources.WinViewToolConvertText;
    annotationButtonModel1.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/convert.png"), new Uri("pack://application:,,,/Style/DarkModeResources/converter/convert.png"));
    annotationButtonModel1.ChildButtonModel = (ToolbarChildButtonModel) new ToolbarChildCheckableButtonModel()
    {
      ContextMenu = (IContextMenuModel) this.convertMenuItems
    };
    annotationButtonModel1.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>(this.OpenContextMenuCommandFunc));
    this.ConvertButtonModel = annotationButtonModel1;
    ToolbarAnnotationButtonModel annotationButtonModel2 = new ToolbarAnnotationButtonModel(AnnotationMode.None);
    annotationButtonModel2.Caption = Resources.WinViewToolBackgroundText;
    annotationButtonModel2.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/Background.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/Background.png"));
    annotationButtonModel2.ChildButtonModel = (ToolbarChildButtonModel) new ToolbarChildCheckableButtonModel()
    {
      ContextMenu = (IContextMenuModel) this.backgroundMenuItems
    };
    annotationButtonModel2.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>(this.OpenContextMenuCommandFunc));
    this.BackgroundButtonModel = annotationButtonModel2;
    this.AutoScrollButtonModel = new ToolbarButtonModel()
    {
      Caption = Resources.MenuViewAutoScrollContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/autoscroll.png"), new Uri("pack://application:,,,/Style/DarkModeResources/autoscroll.png")),
      IsCheckable = true,
      Tooltip = Resources.MenuViewAutoScrollContent,
      Command = (ICommand) new AsyncRelayCommand<ToolbarButtonModel>((Func<ToolbarButtonModel, Task>) (async model =>
      {
        ViewToolbarViewModel toolbarViewModel1 = this;
        ViewToolbarViewModel toolbarViewModel = toolbarViewModel1;
        bool autoScroll = model.IsChecked;
        if (autoScroll && toolbarViewModel1.SubViewModeContinuous == SubViewModeContinuous.Discontinuous)
          toolbarViewModel1.SubViewModeContinuous = SubViewModeContinuous.Verticalcontinuous;
        AnnotationCanvas annotCanvas = PdfObjectExtensions.GetAnnotationCanvas(PDFKit.PdfControl.GetPdfControl(toolbarViewModel1.mainViewModel.Document));
        if (annotCanvas?.AutoScrollHelper != null)
        {
          toolbarViewModel1.mainViewModel.ExitTransientMode(fromAutoScroll: true);
          await toolbarViewModel1.mainViewModel.ReleaseViewerFocusAsync(true);
          await DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() =>
          {
            if (annotCanvas?.AutoScrollHelper == null)
              return;
            if (autoScroll && annotCanvas.AutoScrollHelper.State == PdfViewerAutoScrollState.Stop)
            {
              toolbarViewModel.ExitAnnotationSelectAndMode();
              annotCanvas.AutoScrollHelper.StartAutoScroll();
            }
            else
            {
              if (autoScroll || annotCanvas.AutoScrollHelper.State != PdfViewerAutoScrollState.Scrolling)
                return;
              annotCanvas.AutoScrollHelper.StopAutoScroll();
            }
          }));
        }
        else
          model.IsChecked = false;
      })),
      ChildButtonModel = (ToolbarChildButtonModel) new ToolbarChildCheckableButtonModel()
      {
        ContextMenu = (IContextMenuModel) this.autoScrollMenuItems
      }
    };
    this.EditPageTextObjectButtonModel = new ToolbarButtonModel()
    {
      Caption = Resources.MenuViewEditObjectContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/object.png"), new Uri("pack://application:,,,/Style/DarkModeResources/object.png")),
      IsCheckable = true,
      Command = (ICommand) new AsyncRelayCommand<ToolbarButtonModel>((Func<ToolbarButtonModel, Task>) (async model =>
      {
        ViewToolbarViewModel toolbarViewModel2 = this;
        ViewToolbarViewModel toolbarViewModel = toolbarViewModel2;
        ToolbarButtonModel model1 = model;
        bool edit = model1.IsChecked;
        model1.IsChecked = false;
        toolbarViewModel2.mainViewModel.ExitTransientMode(fromEditText: true);
        await DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() =>
        {
          if (edit && toolbarViewModel.mainViewModel.Document != null)
          {
            toolbarViewModel.ExitAnnotationSelectAndMode();
            model1.IsChecked = true;
            // ISSUE: reference to a compiler-generated field
            if (ViewToolbarViewModel.\u003C\u003Eo__99.\u003C\u003Ep__0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ViewToolbarViewModel.\u003C\u003Eo__99.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, MouseModes, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Value", typeof (ViewToolbarViewModel), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj = ViewToolbarViewModel.\u003C\u003Eo__99.\u003C\u003Ep__0.Target((CallSite) ViewToolbarViewModel.\u003C\u003Eo__99.\u003C\u003Ep__0, toolbarViewModel.mainViewModel.ViewerMouseMode, MouseModes.Default);
            toolbarViewModel.mainViewModel.SelectedAnnotation = (PdfAnnotation) null;
            toolbarViewModel.mainViewModel.EditingPageObjectType = PageObjectType.Text;
            CommomLib.Commom.GAManager.SendEvent("MainWindow", "TextEditor", "Count", 1L);
            PdfPage currentPage = toolbarViewModel.mainViewModel.Document.Pages.CurrentPage;
            PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(toolbarViewModel.mainViewModel.Document);
            FS_RECTF effectiveBox = currentPage.GetEffectiveBox();
            AnnotationCanvas annotationCanvas = PdfObjectExtensions.GetAnnotationCanvas(pdfControl);
            Rect clientRect;
            if (annotationCanvas == null || !pdfControl.TryGetClientRect(currentPage.PageIndex, effectiveBox, out clientRect))
              return;
            double x = clientRect.Left;
            double y = clientRect.Top;
            double num17 = clientRect.Right;
            double num18 = clientRect.Bottom;
            if (x < 0.0)
              x = 0.0;
            if (y < 0.0)
              y = 0.0;
            if (num17 > pdfControl.ViewportWidth)
              num17 = pdfControl.ViewportWidth;
            if (num18 > pdfControl.ViewportHeight)
              num18 = pdfControl.ViewportHeight;
            FS_RECTF pageRect;
            if (x >= pdfControl.ViewportWidth || y >= pdfControl.ViewportHeight || num17 <= x || num18 <= y || !pdfControl.TryGetPageRect(currentPage.PageIndex, new Rect(x, y, num17 - x, num18 - y), out pageRect))
              return;
            pageRect.Inflate(new FS_RECTF(-10f, -10f, -10f, -10f));
            if ((double) pageRect.Width <= 0.0 || (double) pageRect.Height <= 0.0)
              return;
            foreach (PdfTextObject textObject in (IEnumerable<PdfTextObject>) GetAllTextObject(currentPage.PageObjects).OrderBy<PdfTextObject, FS_RECTF>((Func<PdfTextObject, FS_RECTF>) (c =>
            {
              try
              {
                return c.BoundingBox;
              }
              catch
              {
              }
              return new FS_RECTF(-1f, -1f, -1f, -1f);
            }), (IComparer<FS_RECTF>) Comparer<FS_RECTF>.Create((Comparison<FS_RECTF>) ((x, y) =>
            {
              if (x == y)
                return 0;
              float num10 = Math.Min(x.left, x.right);
              float num11 = Math.Min(y.left, y.right);
              float num12 = Math.Max(x.top, x.bottom);
              double num13 = (double) Math.Max(y.top, y.bottom);
              float num14 = num11 - num10;
              double num15 = (double) num12;
              float num16 = (float) (num13 - num15);
              return (double) num16 < -10.0 || (double) num16 <= 10.0 && (double) num14 > 0.0 ? -1 : 1;
            }))))
            {
              FS_RECTF boundingBox = textObject.BoundingBox;
              if (pageRect.IntersectsWith(boundingBox))
              {
                try
                {
                  if (!string.IsNullOrWhiteSpace(textObject.TextUnicode))
                  {
                    annotationCanvas.TextObjectHolder.SelectTextObject(currentPage, textObject);
                    break;
                  }
                }
                catch
                {
                }
              }
            }
          }
          else
          {
            toolbarViewModel.mainViewModel.ExitTransientMode();
            model1.IsChecked = false;
          }
        }));
      }))
    };
    this.PresentButtonModel = new ToolbarButtonModel()
    {
      Caption = Resources.MenuViewPresentContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Present.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Present.png")),
      IsCheckable = false,
      Command = (ICommand) new RelayCommand<ToolbarButtonModel>((Action<ToolbarButtonModel>) (model =>
      {
        CommomLib.Commom.GAManager.SendEvent("MainWindow", "PresentBtn", "Count", 1L);
        this.Present();
      }))
    };
    this.EditDocumentButtomModel = new ToolbarButtonModel()
    {
      Caption = Resources.MenuViewEditTextContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Edit_Text.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Edit_Text.png")),
      IsCheckable = false,
      Command = (ICommand) new AsyncRelayCommand<ToolbarButtonModel>((Func<ToolbarButtonModel, Task>) (async model =>
      {
        ViewToolbarViewModel toolbarViewModel = this;
        if (toolbarViewModel.mainViewModel.IsSaveBySignature && toolbarViewModel.mainViewModel.GetSignatureObjCountFlag() > 0)
        {
          int num = (int) ModernMessageBox.Show(Resources.PageSplitMergeCheckMsg, UtilManager.GetProductName());
        }
        else
        {
          CommomLib.Commom.GAManager.SendEvent("TextEditor2", "BeginEditing", "Count", 1L);
          toolbarViewModel.mainViewModel.Menus.SearchModel.IsSearchVisible = false;
          toolbarViewModel.mainViewModel.QuickToolOpenModel.IsVisible = false;
          toolbarViewModel.mainViewModel.QuickToolPrintModel.IsVisible = false;
          toolbarViewModel.mainViewModel.ChatPanelVisible = false;
          toolbarViewModel.mainViewModel.ChatButtonVisible = false;
          await toolbarViewModel.mainViewModel.ReleaseViewerFocusAsync(false);
          toolbarViewModel.mainViewModel.ExitTransientMode();
          toolbarViewModel.ExitAnnotationSelectAndMode();
          toolbarViewModel.mainViewModel.SelectedAnnotation = (PdfAnnotation) null;
          toolbarViewModel.EditDocumentToolbarSetting = toolbarViewModel.editDocumentToolbarSettingCache;
          toolbarViewModel.UpdateEditDocumentToolbarSettingValues();
          PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(toolbarViewModel.mainViewModel.Document);
          TextProperties textProperties = pdfControl.Editor.TextProperties;
          textProperties.PropertyChanged -= new EventHandler(toolbarViewModel.EditorTextProps_PropertyChanged);
          textProperties.PropertyChanged += new EventHandler(toolbarViewModel.EditorTextProps_PropertyChanged);
          pdfControl.IsEditing = true;
        }
      }))
    };
    ToolbarSettingModel toolbarSettingModel = new ToolbarSettingModel(ToolbarSettingId.CreateEditDocument());
    ToolbarSettingItemBoldModel settingItemBoldModel = new ToolbarSettingItemBoldModel(ContextMenuItemType.FontWeightBold);
    settingItemBoldModel.Command = (ICommand) new AsyncRelayCommand<ToolbarSettingItemModel>(new Func<ToolbarSettingItemModel, Task>(this.OnEditDocumentToolbarSettingInvoked));
    toolbarSettingModel.Add((ToolbarSettingItemModel) settingItemBoldModel);
    ToolbarSettingItemItalicModel settingItemItalicModel = new ToolbarSettingItemItalicModel(ContextMenuItemType.FontStyleItalic);
    settingItemItalicModel.Command = (ICommand) new AsyncRelayCommand<ToolbarSettingItemModel>(new Func<ToolbarSettingItemModel, Task>(this.OnEditDocumentToolbarSettingInvoked));
    toolbarSettingModel.Add((ToolbarSettingItemModel) settingItemItalicModel);
    ToolbarSettingItemFontNameModel itemFontNameModel = new ToolbarSettingItemFontNameModel(ContextMenuItemType.FontName);
    itemFontNameModel.Command = (ICommand) new AsyncRelayCommand<ToolbarSettingItemModel>(new Func<ToolbarSettingItemModel, Task>(this.OnEditDocumentToolbarSettingInvoked));
    toolbarSettingModel.Add((ToolbarSettingItemModel) itemFontNameModel);
    toolbarSettingModel.Add(ToolbarSettingsHelper.CreateCollapsedColor(ToolbarSettingId.CreateEditDocument(), ContextMenuItemType.FontColor, (Action<ToolbarSettingItemModel>) (m => this.OnEditDocumentToolbarSettingInvoked(m)), (ImageSource) null));
    toolbarSettingModel.Add(ToolbarSettingsHelper.CreateFontSize(AnnotationMode.None, (Action<ToolbarSettingItemModel>) (m => this.OnEditDocumentToolbarSettingInvoked(m)), (ImageSource) null));
    ToolbarSettingItemTextEditingButtonsModel editingButtonsModel = new ToolbarSettingItemTextEditingButtonsModel();
    editingButtonsModel.Command = (ICommand) new AsyncRelayCommand<ToolbarSettingItemModel>(new Func<ToolbarSettingItemModel, Task>(this.OnEditDocumentToolbarSettingInvoked));
    toolbarSettingModel.Add((ToolbarSettingItemModel) editingButtonsModel);
    this.editDocumentToolbarSettingCache = toolbarSettingModel;

    static IEnumerable<PdfTextObject> GetAllTextObject(PdfPageObjectsCollection _collection)
    {
      if (_collection != null && _collection.Count >= 0)
      {
        foreach (PdfPageObject pdfPageObject in _collection)
        {
          if (pdfPageObject is PdfTextObject pdfTextObject)
            yield return pdfTextObject;
          else if (pdfPageObject is PdfFormObject pdfFormObject)
          {
            foreach (PdfTextObject pdfTextObject in GetAllTextObject(pdfFormObject.PageObjects))
              yield return pdfTextObject;
          }
        }
      }
    }
  }

  private void EditorTextProps_PropertyChanged(object sender, EventArgs e)
  {
    this.UpdateEditDocumentToolbarSettingValues();
  }

  private void UpdateEditDocumentToolbarSettingValues()
  {
    ToolbarSettingModel documentToolbarSetting = this.EditDocumentToolbarSetting;
    if (documentToolbarSetting == null)
      return;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
    if (!pdfControl.IsEditing)
      return;
    TextProperties textProperties = pdfControl.Editor.TextProperties;
    string caption;
    foreach (ToolbarSettingItemModel settingItemModel in (Collection<ToolbarSettingItemModel>) documentToolbarSetting)
    {
      if (settingItemModel.Type == ContextMenuItemType.FontWeightBold)
        settingItemModel.SelectedValue = (object) textProperties.IsBold;
      else if (settingItemModel.Type == ContextMenuItemType.FontStyleItalic)
        settingItemModel.SelectedValue = (object) textProperties.IsItalic;
      else if (settingItemModel.Type == ContextMenuItemType.FontName)
        settingItemModel.SelectedValue = (object) textProperties.GetFont()?.FontFamily;
      else if (settingItemModel.Type == ContextMenuItemType.FontColor)
      {
        object result;
        if (ToolbarContextMenuHelper.TryParseMenuValue(AnnotationMode.None, ContextMenuItemType.FontColor, (object) textProperties.TextColor, out caption, out result))
          settingItemModel.SelectedValue = result;
      }
      else
      {
        object result;
        if (settingItemModel.Type == ContextMenuItemType.FontSize && ToolbarContextMenuHelper.TryParseMenuValue(AnnotationMode.None, ContextMenuItemType.FontSize, (object) textProperties.FontSize, out caption, out result))
          settingItemModel.SelectedValue = result;
      }
    }
  }

  private void ExitEditing()
  {
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
    TextProperties textProperties = pdfControl?.Editor.TextProperties;
    bool flag;
    if (this.IsDocumentEdited)
    {
      CommomLib.Commom.GAManager.SendEvent("TextEditor2", nameof (ExitEditing), "ExitBtnDocEdited", 1L);
      switch (ModernMessageBox.Show(Resources.ViewToorbarFileChanged, UtilManager.GetProductName(), MessageBoxButton.YesNoCancel))
      {
        case MessageBoxResult.Cancel:
          CommomLib.Commom.GAManager.SendEvent("TextEditor2", "ExitEditingChoice", "Cancel", 1L);
          return;
        case MessageBoxResult.Yes:
          CommomLib.Commom.GAManager.SendEvent("TextEditor2", "ExitEditingChoice", "KeepEditing", 1L);
          flag = true;
          this.UpdateEditedDocumentContent(true);
          this.mainViewModel.SetCanSaveFlag("Editor", true);
          break;
        default:
          CommomLib.Commom.GAManager.SendEvent("TextEditor2", "ExitEditingChoice", "DiscardEditing", 1L);
          flag = true;
          this.UpdateEditedDocumentContent(false);
          break;
      }
    }
    else
    {
      CommomLib.Commom.GAManager.SendEvent("TextEditor2", nameof (ExitEditing), "ExitBtnDocNotEdited", 1L);
      flag = true;
    }
    if (!flag)
      return;
    this.EditDocumentToolbarSetting = (ToolbarSettingModel) null;
    if (pdfControl != null)
      pdfControl.IsEditing = false;
    if (textProperties != null)
      textProperties.PropertyChanged -= new EventHandler(this.EditorTextProps_PropertyChanged);
    this.mainViewModel.QuickToolOpenModel.IsVisible = true;
    this.mainViewModel.QuickToolPrintModel.IsVisible = true;
    this.mainViewModel.ChatButtonVisible = ConfigManager.GetShowcaseChatButtonFlag();
    this.mainViewModel.ChatPanelVisible = !ConfigManager.GetChatPanelClosed();
  }

  private Task OnEditDocumentToolbarSettingInvoked(ToolbarSettingItemModel model)
  {
    if (model == null)
      return Task.CompletedTask;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
    TextProperties textProperties = pdfControl.Editor.TextProperties;
    if (!pdfControl.IsEditing)
      return Task.CompletedTask;
    if (model is ToolbarSettingItemExitModel)
    {
      this.ExitEditing();
      return Task.CompletedTask;
    }
    if (model.Type == ContextMenuItemType.FontWeightBold)
    {
      CommomLib.Commom.GAManager.SendEvent("TextEditor2", "TextEditingTools", "FontWeightBold", 1L);
      if ((bool) model.SelectedValue != textProperties.IsBold)
        textProperties.ToggleBold();
    }
    else if (model.Type == ContextMenuItemType.FontStyleItalic)
    {
      CommomLib.Commom.GAManager.SendEvent("TextEditor2", "TextEditingTools", "FontStyleItalic", 1L);
      if ((bool) model.SelectedValue != textProperties.IsItalic)
        textProperties.ToggleItalic();
    }
    else if (model.Type == ContextMenuItemType.FontName)
    {
      CommomLib.Commom.GAManager.SendEvent("TextEditor2", "TextEditingTools", "FontName", 1L);
      string selectedValue = (string) model.SelectedValue;
      if (!string.IsNullOrEmpty(selectedValue))
        textProperties.SetFont(new FontData(selectedValue));
    }
    else if (model.Type == ContextMenuItemType.FontColor)
    {
      CommomLib.Commom.GAManager.SendEvent("TextEditor2", "TextEditingTools", "FontColor", 1L);
      FS_COLOR pdfColor = ((System.Windows.Media.Color) ColorConverter.ConvertFromString((string) model.SelectedValue)).ToPdfColor();
      textProperties.TextColor = new FS_COLOR?(pdfColor);
    }
    else if (model.Type == ContextMenuItemType.FontSize)
    {
      CommomLib.Commom.GAManager.SendEvent("TextEditor2", "TextEditingTools", "FontSize", 1L);
      textProperties.FontSize = (float) model.SelectedValue;
    }
    this.UpdateEditDocumentToolbarSettingValues();
    return Task.CompletedTask;
  }

  private async Task ExitAnnotationSelectAndMode()
  {
    if (this.mainViewModel?.Document == null)
      return;
    this.mainViewModel.AnnotationMode = AnnotationMode.None;
    await this.mainViewModel.ReleaseViewerFocusAsync(true).ConfigureAwait(false);
  }

  private void OpenContextMenuCommandFunc(ToolbarAnnotationButtonModel model)
  {
    DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (async () =>
    {
      await Task.Delay(50);
      if (!(model.ChildButtonModel is ToolbarChildCheckableButtonModel childButtonModel2) || !childButtonModel2.IsEnabled)
        return;
      childButtonModel2.IsChecked = true;
    }));
  }

  private void DoConvertMenuItemCmd(ContextMenuItemModel model)
  {
    if (this.mainViewModel.Document == null)
      return;
    string name = (model as ConvertContextMenuItemModel).Name;
    if (name == null)
      return;
    switch (name.Length)
    {
      case 8:
        switch (name[1])
        {
          case 'D':
            switch (name)
            {
              case "PDFtoPPT":
                this.mainViewModel.ConverterCommands.DoPDFToPPT();
                return;
              case "PDFtoRtf":
                this.mainViewModel.ConverterCommands.DoPDFToRtf();
                return;
              case "PDFtoXml":
                this.mainViewModel.ConverterCommands.DoPDFToXml();
                return;
              default:
                return;
            }
          case 'P':
            if (!(name == "PPTtoPDF"))
              return;
            this.mainViewModel.ConverterCommands.DoPPTToPDF();
            return;
          case 't':
            if (!(name == "RtftoPDF"))
              return;
            this.mainViewModel.ConverterCommands.DoRtfToPDF();
            return;
          case 'x':
            if (!(name == "TxttoPDF"))
              return;
            this.mainViewModel.ConverterCommands.DoTxtToPDF();
            return;
          default:
            return;
        }
      case 9:
        switch (name[5])
        {
          case 'H':
            if (!(name == "PDFtoHtml"))
              return;
            this.mainViewModel.ConverterCommands.DoPDFToHtml();
            return;
          case 'J':
            if (!(name == "PDFtoJpeg"))
              return;
            this.mainViewModel.ConverterCommands.DoPDFToJpeg();
            return;
          case 'T':
            if (!(name == "PDFtoText"))
              return;
            this.mainViewModel.ConverterCommands.DoPDFToTxt();
            return;
          case 'W':
            if (!(name == "PDFtoWord"))
              return;
            this.mainViewModel.ConverterCommands.DoPDFToWord();
            return;
          case 'o':
            if (!(name == "WordtoPDF"))
              return;
            this.mainViewModel.ConverterCommands.DoWordToPDF();
            return;
          default:
            return;
        }
      case 10:
        switch (name[0])
        {
          case 'E':
            if (!(name == "ExceltoPDF"))
              return;
            this.mainViewModel.ConverterCommands.DoExcelToPDF();
            return;
          case 'I':
            if (!(name == "ImagetoPDF"))
              return;
            this.mainViewModel.ConverterCommands.DoImageToPDF();
            return;
          case 'P':
            switch (name)
            {
              case "PDFtoExcel":
                this.mainViewModel.ConverterCommands.DoPDFToExcel();
                return;
              case "PDFtoImage":
                this.mainViewModel.ConverterCommands.DoPDFToImage();
                return;
              default:
                return;
            }
          default:
            return;
        }
    }
  }

  private void DoBackgroundMenuItemCmd(ContextMenuItemModel model)
  {
    this.TryUpdateViewerBackground();
    if (!(this.backgroundMenuItems?.SelectedItem?.TagData.MenuItemValue is BackgroundColorSetting menuItemValue))
      return;
    ConfigManager.SetBackgroundAsync(menuItemValue.Name, menuItemValue.PageMaskColor.ToString(), menuItemValue.BackgroundColor.ToString());
  }

  public void SetViewerBackground(string settingName)
  {
    ContextMenuItemModel model = this.backgroundMenuItems.OfType<ContextMenuItemModel>().FirstOrDefault<ContextMenuItemModel>((Func<ContextMenuItemModel, bool>) (c => (c?.TagData?.MenuItemValue is BackgroundColorSetting menuItemValue ? menuItemValue.Name : (string) null) == settingName));
    if (model == null || !model.IsCheckable)
      return;
    model.IsChecked = true;
    this.DoBackgroundMenuItemCmd(model);
  }

  public void TryUpdateViewerBackground()
  {
    if (this.mainViewModel.Document == null || !(this.BackgroundMenuItems?.SelectedItem?.TagData?.MenuItemValue is BackgroundColorSetting menuItemValue))
      return;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
    if (pdfControl == null)
      return;
    pdfControl.PageMaskBrush = (Brush) new SolidColorBrush(menuItemValue.PageMaskColor);
    pdfControl.PageBackground = (Brush) new SolidColorBrush(menuItemValue.BackgroundColor);
  }

  public RelayCommand GotoPrevPageCmd
  {
    get
    {
      return this.gotoPrevPageCmd ?? (this.gotoPrevPageCmd = new RelayCommand((Action) (() => this.GotoPrevPage()), (Func<bool>) (() => this.CanGotoPrevPage())));
    }
  }

  private void GotoPrevPage()
  {
    if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
      return;
    int currentIndex = this.mainViewModel.Document.Pages.CurrentIndex;
    if (currentIndex < 0 || currentIndex > this.mainViewModel.Document.Pages.Count)
      return;
    int num = currentIndex - 1;
    if (num < 0)
      return;
    this.mainViewModel.SelectedPageIndex = num;
  }

  private bool CanGotoPrevPage() => true;

  public RelayCommand GotoNextPageCmd
  {
    get
    {
      return this.gotoNextPageCmd ?? (this.gotoNextPageCmd = new RelayCommand((Action) (() => this.GotoNextPage()), (Func<bool>) (() => this.CanGotoNextPage())));
    }
  }

  private void GotoNextPage()
  {
    if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
      return;
    int currentIndex = this.mainViewModel.Document.Pages.CurrentIndex;
    if (currentIndex < 0 || currentIndex >= this.mainViewModel.Document.Pages.Count - 1)
      return;
    int num = currentIndex + 1;
    if (num > this.mainViewModel.Document.Pages.Count - 1)
      return;
    this.mainViewModel.SelectedPageIndex = num;
  }

  private bool CanGotoNextPage() => true;

  public RelayCommand GotoFirstPageCmd
  {
    get
    {
      return this.gotoFirstPageCmd ?? (this.gotoFirstPageCmd = new RelayCommand((Action) (() => this.GotoFirstPage()), (Func<bool>) (() => this.CanGotoFirstPage())));
    }
  }

  private void GotoFirstPage()
  {
    if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
      return;
    this.mainViewModel.SelectedPageIndex = 0;
  }

  private bool CanGotoFirstPage() => true;

  public RelayCommand GotoLastPageCmd
  {
    get
    {
      return this.gotoLastPageCmd ?? (this.gotoLastPageCmd = new RelayCommand((Action) (() => this.GotoLastPage()), (Func<bool>) (() => this.CanGotoLastPage())));
    }
  }

  private void GotoLastPage()
  {
    if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
      return;
    this.mainViewModel.SelectedPageIndex = this.mainViewModel.Document.Pages.Count - 1;
  }

  private bool CanGotoLastPage() => true;

  public RelayCommand GotoPrevViewCmd
  {
    get
    {
      return this.gotoPrevViewCmd ?? (this.gotoPrevViewCmd = new RelayCommand((Action) (() => this.GotoPrevView()), (Func<bool>) (() => this.CanGotoPrevView())));
    }
  }

  private void GotoPrevView()
  {
    if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
      return;
    int currentIndex = this.mainViewModel.Document.Pages.CurrentIndex;
    this.mainViewModel.Jumping = true;
    if (this.mainViewModel.ViewJumpManager.IsFirstView)
      return;
    this.mainViewModel.SelectedPageIndex = this.mainViewModel.ViewJumpManager.ViewBackCmd(currentIndex);
  }

  private bool CanGotoPrevView() => true;

  public RelayCommand GotoNextViewCmd
  {
    get
    {
      return this.gotoNextViewCmd ?? (this.gotoNextViewCmd = new RelayCommand((Action) (() => this.GotoNextView()), (Func<bool>) (() => this.CanGotoPrevView())));
    }
  }

  private void GotoNextView()
  {
    if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
      return;
    int currentIndex = this.mainViewModel.Document.Pages.CurrentIndex;
    this.mainViewModel.Jumping = true;
    if (this.mainViewModel.ViewJumpManager.IsLastView)
      return;
    this.mainViewModel.SelectedPageIndex = this.mainViewModel.ViewJumpManager.ViewPreCmd(currentIndex);
  }

  private bool CanGotoNextView() => true;

  public int AutoScrollSpeed
  {
    get => this.autoScrollSpeed;
    set => this.SetProperty<int>(ref this.autoScrollSpeed, value, nameof (AutoScrollSpeed));
  }

  public void PauseAutoScroll(int milliseconds)
  {
    PdfObjectExtensions.GetAnnotationCanvas(PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document))?.AutoScrollHelper?.Pause(milliseconds);
  }

  public void StopAutoScroll()
  {
    if (this.AutoScrollButtonModel == null || !this.AutoScrollButtonModel.IsChecked)
      return;
    this.AutoScrollButtonModel.IsChecked = false;
    this.AutoScrollButtonModel.Command.Execute(this.AutoScrollButtonModel.CommandParameter ?? (object) this.AutoScrollButtonModel);
  }

  public bool IsDocumentEdited
  {
    get
    {
      PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
      return pdfControl != null && pdfControl.IsEditing && pdfControl.GetChangedPageIndexes().Length != 0;
    }
  }

  public async Task<bool> DocumentEditedSaveAsync(bool forceSaveAs = false)
  {
    if (this.IsDocumentEdited)
    {
      bool flag = false;
      bool documentUpdated = false;
      if (!forceSaveAs)
      {
        flag = await this.mainViewModel.DocumentWrapper.SaveAsync();
        if (flag)
        {
          documentUpdated = true;
          this.UpdateEditedDocumentContent(true);
          PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
          if (pdfControl != null)
          {
            pdfControl.IsEditing = false;
            pdfControl.IsEditing = true;
          }
          this.mainViewModel.RemoveCanSaveFlag(true);
          if (this.mainViewModel.CanSave)
            this.mainViewModel.DocumentWrapper.Metadata.ModificationDate = DateTimeOffset.Now;
          flag = await this.mainViewModel.DocumentWrapper.SaveAsync();
          if (flag)
            return true;
        }
      }
      if (!flag)
      {
        string str1 = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        string str2 = "Untitled.pdf";
        if (!string.IsNullOrEmpty(this.mainViewModel.DocumentWrapper?.DocumentPath))
        {
          FileInfo fileInfo = new FileInfo(this.mainViewModel.DocumentWrapper?.DocumentPath);
          str1 = fileInfo.DirectoryName;
          string str3 = fileInfo.Name;
          if (!string.IsNullOrEmpty(fileInfo.Extension))
            str3 = str3.Substring(0, str3.Length - fileInfo.Extension.Length);
          str2 = str3 + " Edited.pdf";
        }
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        saveFileDialog1.Filter = "pdf|*.pdf";
        saveFileDialog1.CreatePrompt = false;
        saveFileDialog1.OverwritePrompt = true;
        saveFileDialog1.InitialDirectory = str1;
        saveFileDialog1.FileName = str2;
        SaveFileDialog saveFileDialog2 = saveFileDialog1;
        string filePath = (string) null;
        FileStream stream = (FileStream) null;
        try
        {
          while (stream == null && saveFileDialog2.ShowDialog(App.Current.MainWindow).GetValueOrDefault())
          {
            if (!(Path.GetFullPath(this.mainViewModel.DocumentWrapper.DocumentPath) == Path.GetFullPath(saveFileDialog2.FileName)))
            {
              filePath = saveFileDialog2.FileName;
              try
              {
                stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
              }
              catch
              {
              }
            }
            if (stream == null && ModernMessageBox.Show(Resources.ViewToolbarSaveFailed, UtilManager.GetProductName(), MessageBoxButton.YesNo) != MessageBoxResult.Yes)
              break;
          }
          if (stream != null)
          {
            if (!documentUpdated)
              this.UpdateEditedDocumentContent(true);
            if (this.mainViewModel.CanSave)
              this.mainViewModel.DocumentWrapper.Metadata.ModificationDate = DateTimeOffset.Now;
            this.mainViewModel.Document.Save((Stream) stream, SaveFlags.NoIncremental);
            stream.Dispose();
            int selectedPageIndex = this.mainViewModel.SelectedPageIndex;
            this.mainViewModel.DelAutoSaveFile(this.mainViewModel.DocumentWrapper.DocumentPath);
            int num1 = await ConfigManager.SetDocumentCurrentPageNumberAsync(filePath, selectedPageIndex) ? 1 : 0;
            ConfigManager.SetPageSizeZoomModelAsync(filePath, this.DocSizeModeWrap.ToString(), this.DocZoom);
            int num2 = await this.mainViewModel.OpenDocumentCoreAsync(filePath) ? 1 : 0;
            return true;
          }
        }
        finally
        {
          stream?.Dispose();
        }
        filePath = (string) null;
        stream = (FileStream) null;
      }
    }
    return false;
  }

  private bool UpdateEditedDocumentContent(bool applyEdit)
  {
    PdfDocument document = this.mainViewModel.Document;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(document);
    if (pdfControl != null && pdfControl.IsEditing)
    {
      int[] changedPageIndexes = pdfControl.GetChangedPageIndexes();
      if (changedPageIndexes.Length != 0)
      {
        pdfControl.IsRenderPaused = true;
        pdfControl.ClearEditorUndoStack();
        try
        {
          for (int index1 = 0; index1 < changedPageIndexes.Length; ++index1)
          {
            int index2 = changedPageIndexes[index1];
            PdfPage page = document.Pages[index2];
            if (applyEdit)
              page.GenerateContentAdvance(new GenerateContentOptions()
              {
                ImagesOnly = false,
                KeepHeaderFooterData = true
              });
            else
              page.Dispose();
          }
        }
        finally
        {
          pdfControl.IsRenderPaused = false;
        }
        Ioc.Default.GetService<PdfThumbnailService>().RefreshThumbnail(changedPageIndexes);
        return true;
      }
    }
    return false;
  }

  public ToolbarButtonModel ReadButtonModel
  {
    get
    {
      ToolbarButtonModel readButtonModel1 = this.readButtonModel;
      if (readButtonModel1 != null)
        return readButtonModel1;
      ToolbarButtonModel toolbarButtonModel = new ToolbarButtonModel();
      toolbarButtonModel.Caption = Resources.ReadWinTitle;
      toolbarButtonModel.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/speach.png"), new Uri("pack://application:,,,/Style/DarkModeResources/speach.png"));
      toolbarButtonModel.IsChecked = false;
      ToolbarChildCheckableButtonModel checkableButtonModel = new ToolbarChildCheckableButtonModel();
      ContextMenuModel contextMenuModel = new ContextMenuModel();
      contextMenuModel.Add(ToolbarContextMenuHelper.SpeakCurrent((Action<ContextMenuItemModel>) null));
      contextMenuModel.Add(ToolbarContextMenuHelper.SpeakFormCurrent((Action<ContextMenuItemModel>) null));
      contextMenuModel.Add(ToolbarContextMenuHelper.SpeakAll((Action<ContextMenuItemModel>) null));
      contextMenuModel.Add(ToolbarContextMenuHelper.SpeechToolbarMenu((Action<ContextMenuItemModel>) null));
      checkableButtonModel.ContextMenu = (IContextMenuModel) contextMenuModel;
      toolbarButtonModel.ChildButtonModel = (ToolbarChildButtonModel) checkableButtonModel;
      toolbarButtonModel.Command = (ICommand) new RelayCommand((Action) (() =>
      {
        MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
        ContextMenuModel contextMenu = (requiredService.ViewToolbar.ReadButtonModel.ChildButtonModel as ToolbarChildCheckableButtonModel).ContextMenu as ContextMenuModel;
        if (!requiredService.IsReading)
        {
          if (requiredService.Document != null)
          {
            PdfDocument document = requiredService.Document;
            requiredService.ViewToolbar.ReadButtonModel.IsChecked = true;
            requiredService.IsReading = true;
            requiredService.speechUtils?.Dispose();
            requiredService.speechUtils = new SpeechUtils(document);
            (contextMenu[0] as SpeedContextMenuItemModel).IsEnabled = false;
            (contextMenu[1] as SpeedContextMenuItemModel).IsEnabled = false;
            (contextMenu[2] as SpeedContextMenuItemModel).IsEnabled = false;
            if (requiredService.speechControl == null)
            {
              requiredService.speechControl = new SpeechControl();
              requiredService.speechControl.Owner = App.Current.MainWindow;
              if (requiredService.speechControl.Owner.WindowState == WindowState.Normal)
              {
                requiredService.speechControl.Top = requiredService.speechControl.Owner.Top + 152.0;
                requiredService.speechControl.Left = requiredService.speechControl.Owner.Left + requiredService.speechControl.Owner.ActualWidth - 520.0;
              }
              else if (requiredService.speechControl.Owner.WindowState == WindowState.Maximized)
              {
                requiredService.speechControl.Top = 152.0;
                requiredService.speechControl.Left = requiredService.speechControl.Owner.ActualWidth - 520.0;
              }
              requiredService.speechControl.Show();
              if (requiredService.speechUtils.ProcessorStream != null)
                return;
              requiredService.speechUtils.SpeakPages(requiredService.CurrnetPageIndex - 1, requiredService.Document.Pages.Count - 1);
              (contextMenu[1] as SpeedContextMenuItemModel).IsChecked = true;
              CommomLib.Commom.GAManager.SendEvent("PDFReader", "Read", "FromCurrentPageDefault", 1L);
            }
            else
            {
              requiredService.speechUtils.Rate = requiredService.speechControl.SpeedSli.Value * 2.0 - 10.0;
              requiredService.speechUtils.SpeechVolume = (float) Convert.ToInt32(requiredService.speechControl.VolumeSlider.Value);
              requiredService.speechUtils.Pitch = (double) Convert.ToInt32(requiredService.speechControl.ToneSli.Value - 5.0);
              requiredService.speechUtils.CultureIndex = requiredService.speechControl.CultureListBox.SelectedIndex >= 0 ? requiredService.speechControl.CultureListBox.SelectedIndex : requiredService.speechUtils.GetcultureIndex();
              if ((contextMenu[0] as SpeedContextMenuItemModel).IsChecked)
              {
                requiredService.speechUtils.SpeakCurrentPage(requiredService.CurrnetPageIndex - 1);
                (contextMenu[0] as SpeedContextMenuItemModel).IsChecked = true;
              }
              else if ((contextMenu[1] as SpeedContextMenuItemModel).IsChecked)
              {
                requiredService.speechUtils.SpeakPages(requiredService.CurrnetPageIndex - 1, requiredService.Document.Pages.Count - 1);
                (contextMenu[1] as SpeedContextMenuItemModel).IsChecked = true;
              }
              else
              {
                requiredService.speechUtils.SpeakPages(0, requiredService.Document.Pages.Count - 1);
                (contextMenu[2] as SpeedContextMenuItemModel).IsChecked = true;
              }
            }
          }
          else
            requiredService.ViewToolbar.ReadButtonModel.IsChecked = false;
        }
        else
        {
          requiredService.ViewToolbar.ReadButtonModel.IsChecked = false;
          requiredService.IsReading = false;
          requiredService.speechUtils?.Dispose();
          requiredService.speechUtils = (SpeechUtils) null;
          (contextMenu[0] as SpeedContextMenuItemModel).IsEnabled = true;
          (contextMenu[1] as SpeedContextMenuItemModel).IsEnabled = true;
          (contextMenu[2] as SpeedContextMenuItemModel).IsEnabled = true;
          (contextMenu[0] as SpeedContextMenuItemModel).Icon = (ImageSource) null;
          (contextMenu[1] as SpeedContextMenuItemModel).Icon = (ImageSource) null;
          (contextMenu[2] as SpeedContextMenuItemModel).Icon = (ImageSource) null;
        }
      }));
      ToolbarButtonModel readButtonModel2 = toolbarButtonModel;
      this.readButtonModel = toolbarButtonModel;
      return readButtonModel2;
    }
  }

  public void Present()
  {
    if (App.Current.Windows.OfType<PresentationWindow>().Any<PresentationWindow>() || this.mainViewModel.Document == null)
      return;
    this.mainViewModel.ExitTransientMode();
    new PresentationWindow(this.mainViewModel.Document, this.mainViewModel.CurrentFileName)
    {
      PageIndex = this.mainViewModel.SelectedPageIndex
    }.Show();
  }
}
