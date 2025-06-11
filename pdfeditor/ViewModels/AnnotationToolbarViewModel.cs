// Decompiled with JetBrains decompiler
// Type: pdfeditor.ViewModels.AnnotationToolbarViewModel
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
using Newtonsoft.Json;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Actions;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Controls;
using pdfeditor.Controls.Annotations;
using pdfeditor.Controls.Annotations.Holders;
using pdfeditor.Controls.Menus;
using pdfeditor.Controls.Signature;
using pdfeditor.Controls.Watermark;
using pdfeditor.Models.Annotations;
using pdfeditor.Models.Menus;
using pdfeditor.Models.Menus.ToolbarSettings;
using pdfeditor.Models.Viewer;
using pdfeditor.Properties;
using pdfeditor.Utils;
using pdfeditor.Utils.Enums;
using pdfeditor.Views;
using PDFKit;
using PDFKit.Utils;
using PDFKit.Utils.StampUtils;
using PDFKit.Utils.WatermarkUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

#nullable enable
namespace pdfeditor.ViewModels;

public class AnnotationToolbarViewModel : ObservableObject
{
  private readonly 
  #nullable disable
  MainViewModel mainViewModel;
  private TypedContextMenuModel stampMenuItems;
  private TypedContextMenuModel stampMenuItems2;
  private TypedContextMenuModel shareMenuItem;
  private TypedContextMenuModel signatureMenuItems;
  private TypedContextMenuModel signatureMenuItems2;
  private TypedContextMenuModel signatureMenuItems3;
  private TypedContextMenuModel waterMenuItems;
  private TypedContextMenuModel linkMenuItems;
  private ToolbarAnnotationButtonModel underlineButtonModel;
  private ToolbarAnnotationButtonModel strikeButtonModel;
  private ToolbarAnnotationButtonModel highlightButtonModel;
  private ToolbarAnnotationButtonModel lineButtonModel;
  private ToolbarAnnotationButtonModel inkButtonModel;
  private ToolbarAnnotationButtonModel squareButtonModel;
  private ToolbarAnnotationButtonModel circleButtonModel;
  private ToolbarAnnotationButtonModel highlightareaButtonModel;
  private ToolbarAnnotationButtonModel textBoxButtonModel;
  private ToolbarAnnotationButtonModel textButtonModel;
  private ToolbarAnnotationButtonModel noteButtonModel;
  private ToolbarAnnotationButtonModel shareButtonModel;
  private ToolbarAnnotationButtonModel stampButtonModel;
  private ToolbarAnnotationButtonModel imageButtonModel;
  private ToolbarAnnotationButtonModel signatureButtonModel;
  private ToolbarAnnotationButtonModel watermarkButtonModel;
  private ToolbarAnnotationButtonModel linkButtonModel;
  private AnnotationMenuPropertyAccessor annotationMenuPropertyAccessor;
  private System.Collections.Generic.IReadOnlyList<ToolbarAnnotationButtonModel> allAnnotationButton;
  private WatermarkAnnonationModel watermarkModel;
  private WatermarkParam watermarkParam;
  private WatermarkImageModel imageWatermarkModel;
  private WatermarkTextModel textWatermarkModel;
  private AsyncRelayCommand addFormControlCheckCmd;
  private AsyncRelayCommand addFormControlCancelCmd;
  private AsyncRelayCommand addFormControlRadioCheckCmd;
  private AsyncRelayCommand addFormControlCheckBoxCmd;
  private AsyncRelayCommand addFormControlIndeterminateCmd;
  private AsyncRelayCommand addFormControlIndeterminateFillCmd;
  public static int DocSignatures;

  public AnnotationToolbarViewModel(MainViewModel mainViewModel)
  {
    this.mainViewModel = mainViewModel;
    this.InitToolbarAnnotationButtonModel();
  }

  public AnnotationMenuPropertyAccessor AnnotationMenuPropertyAccessor
  {
    get
    {
      return this.annotationMenuPropertyAccessor ?? (this.annotationMenuPropertyAccessor = new AnnotationMenuPropertyAccessor(this));
    }
  }

  public TypedContextMenuModel StampMenuItems
  {
    get => this.stampMenuItems;
    set
    {
      this.SetProperty<TypedContextMenuModel>(ref this.stampMenuItems, value, nameof (StampMenuItems));
    }
  }

  public TypedContextMenuModel SignatureMenuItems
  {
    get => this.signatureMenuItems;
    set
    {
      this.SetProperty<TypedContextMenuModel>(ref this.signatureMenuItems, value, nameof (SignatureMenuItems));
    }
  }

  public TypedContextMenuModel WatermakMenuItems
  {
    get => this.waterMenuItems;
    set
    {
      this.SetProperty<TypedContextMenuModel>(ref this.waterMenuItems, value, nameof (WatermakMenuItems));
    }
  }

  public WatermarkAnnonationModel WatermarkModel
  {
    get => this.watermarkModel;
    set
    {
      this.SetProperty<WatermarkAnnonationModel>(ref this.watermarkModel, value, nameof (WatermarkModel));
    }
  }

  public WatermarkParam WatermarkParam
  {
    get => this.watermarkParam;
    set
    {
      this.SetProperty<WatermarkParam>(ref this.watermarkParam, value, nameof (WatermarkParam));
    }
  }

  public WatermarkImageModel ImageWatermarkModel
  {
    get => this.imageWatermarkModel;
    set
    {
      this.SetProperty<WatermarkImageModel>(ref this.imageWatermarkModel, value, nameof (ImageWatermarkModel));
    }
  }

  public WatermarkTextModel TextWatermarkModel
  {
    get => this.textWatermarkModel;
    set
    {
      this.SetProperty<WatermarkTextModel>(ref this.textWatermarkModel, value, nameof (TextWatermarkModel));
    }
  }

  public ToolbarAnnotationButtonModel UnderlineButtonModel
  {
    get => this.underlineButtonModel;
    set
    {
      this.SetButtonProperty(ref this.underlineButtonModel, value, nameof (UnderlineButtonModel));
    }
  }

  public ToolbarAnnotationButtonModel StrikeButtonModel
  {
    get => this.strikeButtonModel;
    set => this.SetButtonProperty(ref this.strikeButtonModel, value, nameof (StrikeButtonModel));
  }

  public ToolbarAnnotationButtonModel HighlightButtonModel
  {
    get => this.highlightButtonModel;
    set
    {
      this.SetButtonProperty(ref this.highlightButtonModel, value, nameof (HighlightButtonModel));
    }
  }

  public ToolbarAnnotationButtonModel LineButtonModel
  {
    get => this.lineButtonModel;
    set => this.SetButtonProperty(ref this.lineButtonModel, value, nameof (LineButtonModel));
  }

  public ToolbarAnnotationButtonModel InkButtonModel
  {
    get => this.inkButtonModel;
    set => this.SetButtonProperty(ref this.inkButtonModel, value, nameof (InkButtonModel));
  }

  public ToolbarAnnotationButtonModel SquareButtonModel
  {
    get => this.squareButtonModel;
    set => this.SetButtonProperty(ref this.squareButtonModel, value, nameof (SquareButtonModel));
  }

  public ToolbarAnnotationButtonModel CircleButtonModel
  {
    get => this.circleButtonModel;
    set => this.SetButtonProperty(ref this.circleButtonModel, value, nameof (CircleButtonModel));
  }

  public ToolbarAnnotationButtonModel HighlightAreaButtonModel
  {
    get => this.highlightareaButtonModel;
    set
    {
      this.SetButtonProperty(ref this.highlightareaButtonModel, value, nameof (HighlightAreaButtonModel));
    }
  }

  public ToolbarAnnotationButtonModel TextBoxButtonModel
  {
    get => this.textBoxButtonModel;
    set => this.SetButtonProperty(ref this.textBoxButtonModel, value, nameof (TextBoxButtonModel));
  }

  public ToolbarAnnotationButtonModel TextButtonModel
  {
    get => this.textButtonModel;
    set => this.SetButtonProperty(ref this.textButtonModel, value, nameof (TextButtonModel));
  }

  public ToolbarAnnotationButtonModel NoteButtonModel
  {
    get => this.noteButtonModel;
    set => this.SetButtonProperty(ref this.noteButtonModel, value, nameof (NoteButtonModel));
  }

  public ToolbarAnnotationButtonModel StampButtonModel
  {
    get => this.stampButtonModel;
    set => this.SetButtonProperty(ref this.stampButtonModel, value, nameof (StampButtonModel));
  }

  public ToolbarAnnotationButtonModel ShareButtonModel
  {
    get => this.shareButtonModel;
    set => this.SetButtonProperty(ref this.shareButtonModel, value, nameof (ShareButtonModel));
  }

  public ToolbarAnnotationButtonModel ImageButtonModel
  {
    get => this.imageButtonModel;
    set => this.SetButtonProperty(ref this.imageButtonModel, value, nameof (ImageButtonModel));
  }

  public ToolbarAnnotationButtonModel SignatureButtonModel
  {
    get => this.signatureButtonModel;
    set
    {
      this.SetButtonProperty(ref this.signatureButtonModel, value, nameof (SignatureButtonModel));
    }
  }

  public ToolbarAnnotationButtonModel WatermarkButtonModel
  {
    get => this.watermarkButtonModel;
    set
    {
      this.SetButtonProperty(ref this.watermarkButtonModel, value, nameof (WatermarkButtonModel));
    }
  }

  public TypedContextMenuModel LinkMenuItems
  {
    get => this.linkMenuItems;
    set
    {
      this.SetProperty<TypedContextMenuModel>(ref this.linkMenuItems, value, nameof (LinkMenuItems));
    }
  }

  public ToolbarAnnotationButtonModel LinkButtonModel
  {
    get => this.linkButtonModel;
    set => this.SetButtonProperty(ref this.linkButtonModel, value, nameof (LinkButtonModel));
  }

  public AsyncRelayCommand AddFormControlCheckCmd
  {
    get
    {
      return this.addFormControlCheckCmd ?? (this.addFormControlCheckCmd = new AsyncRelayCommand((Func<Task>) (async () => await this.AddFormControlAsync("Check"))));
    }
  }

  public AsyncRelayCommand AddFormControlCancelCmd
  {
    get
    {
      return this.addFormControlCancelCmd ?? (this.addFormControlCancelCmd = new AsyncRelayCommand((Func<Task>) (async () => await this.AddFormControlAsync("Cancel"))));
    }
  }

  public AsyncRelayCommand AddFormControlRadioCheckCmd
  {
    get
    {
      return this.addFormControlRadioCheckCmd ?? (this.addFormControlRadioCheckCmd = new AsyncRelayCommand((Func<Task>) (async () => await this.AddFormControlAsync("RadioCheck"))));
    }
  }

  public AsyncRelayCommand AddFormControlCheckBoxCmd
  {
    get
    {
      return this.addFormControlCheckBoxCmd ?? (this.addFormControlCheckBoxCmd = new AsyncRelayCommand((Func<Task>) (async () => await this.AddFormControlAsync("CheckBox"))));
    }
  }

  public AsyncRelayCommand AddFormControlIndeterminateCmd
  {
    get
    {
      return this.addFormControlIndeterminateCmd ?? (this.addFormControlIndeterminateCmd = new AsyncRelayCommand((Func<Task>) (async () => await this.AddFormControlAsync("Indeterminate"))));
    }
  }

  public AsyncRelayCommand AddFormControlIndeterminateFillCmd
  {
    get
    {
      return this.addFormControlIndeterminateFillCmd ?? (this.addFormControlIndeterminateFillCmd = new AsyncRelayCommand((Func<Task>) (async () => await this.AddFormControlAsync("Indeterminate Fill"))));
    }
  }

  private async Task AddFormControlAsync(string name)
  {
    PDFKit.PdfControl pdfEditor = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
    AnnotationHolderManager holder = PdfObjectExtensions.GetAnnotationHolderManager(pdfEditor);
    if (holder == null)
    {
      pdfEditor = (PDFKit.PdfControl) null;
      holder = (AnnotationHolderManager) null;
    }
    else
    {
      this.mainViewModel.ExitTransientMode();
      this.mainViewModel.AnnotationMode = AnnotationMode.None;
      holder.CancelAll();
      await holder.WaitForCancelCompletedAsync();
      StampFormControlModel formControlModel = new StampFormControlModel()
      {
        Name = name
      };
      Size geometrySize;
      Geometry controlPreviewGrometry = StampUtil.CreateStampFormControlPreviewGrometry(name, out geometrySize);
      System.Windows.Shapes.Path path1 = new System.Windows.Shapes.Path();
      path1.Data = controlPreviewGrometry;
      path1.Fill = (Brush) Brushes.Black;
      path1.Width = geometrySize.Width;
      path1.Height = geometrySize.Height;
      path1.UseLayoutRounding = false;
      path1.SnapsToDevicePixels = false;
      System.Windows.Shapes.Path path2 = path1;
      MainViewModel mainViewModel = this.mainViewModel;
      HoverOperationModel hoverOperationModel = new HoverOperationModel(pdfEditor?.Viewer);
      hoverOperationModel.Data = (object) formControlModel;
      hoverOperationModel.SizeInDocument = new FS_SIZEF(formControlModel.Width, formControlModel.Height);
      Grid grid = new Grid();
      UIElementCollection children1 = grid.Children;
      Viewbox element1 = new Viewbox();
      element1.Child = (UIElement) path2;
      children1.Add((UIElement) element1);
      UIElementCollection children2 = grid.Children;
      Rectangle element2 = new Rectangle();
      element2.Stroke = (Brush) Brushes.Blue;
      element2.StrokeThickness = 2.0;
      element2.StrokeDashArray.Add(2.5);
      element2.StrokeDashArray.Add(1.5);
      element2.Opacity = 0.6;
      element2.UseLayoutRounding = false;
      element2.SnapsToDevicePixels = false;
      children2.Add((UIElement) element2);
      hoverOperationModel.PreviewElement = (UIElement) grid;
      mainViewModel.ViewerOperationModel = (DataOperationModel) hoverOperationModel;
      ViewOperationResult<bool> task = await this.mainViewModel.ViewerOperationModel.Task;
      DataOperationModel viewerOperationModel = this.mainViewModel.ViewerOperationModel;
      if ((task != null ? (task.Value ? 1 : 0) : 0) == 0)
      {
        pdfEditor = (PDFKit.PdfControl) null;
        holder = (AnnotationHolderManager) null;
      }
      else
      {
        holder.CancelAll();
        holder.Stamp.StartCreateNew(this.mainViewModel.Document.Pages[viewerOperationModel.CurrentPage], viewerOperationModel.PositionFromDocument);
        System.Collections.Generic.IReadOnlyList<PdfStampAnnotation> newAsync = await holder.Stamp.CompleteCreateNewAsync();
        if (newAsync == null)
        {
          pdfEditor = (PDFKit.PdfControl) null;
          holder = (AnnotationHolderManager) null;
        }
        else if (newAsync.Count <= 0)
        {
          pdfEditor = (PDFKit.PdfControl) null;
          holder = (AnnotationHolderManager) null;
        }
        else
        {
          holder.Select((PdfAnnotation) newAsync[0], true);
          pdfEditor = (PDFKit.PdfControl) null;
          holder = (AnnotationHolderManager) null;
        }
      }
    }
  }

  public DateTime StampImgFileOkTime { get; set; }

  public System.Collections.Generic.IReadOnlyList<ToolbarAnnotationButtonModel> AllAnnotationButton
  {
    get => this.allAnnotationButton;
  }

  public ToolbarSettingModel CheckedButtonToolbarSetting
  {
    get
    {
      if (this.mainViewModel.ViewToolbar.EditDocumentToolbarSetting != null)
        return this.mainViewModel.ViewToolbar.EditDocumentToolbarSetting;
      if ((PdfWrapper) this.mainViewModel.SelectedAnnotation != (PdfWrapper) null)
      {
        List<AnnotationMode> modes = AnnotationFactory.GetAnnotationModes(this.mainViewModel.SelectedAnnotation).ToList<AnnotationMode>();
        if (modes.Count > 0)
        {
          ToolbarSettingModel[] array = this.AllAnnotationButton.Where<ToolbarAnnotationButtonModel>((Func<ToolbarAnnotationButtonModel, bool>) (c => modes.Contains(c.Mode) && c.ToolbarSettingModel != null)).Select<ToolbarAnnotationButtonModel, ToolbarSettingModel>((Func<ToolbarAnnotationButtonModel, ToolbarSettingModel>) (c => c.ToolbarSettingModel)).OrderBy<ToolbarSettingModel, int>((Func<ToolbarSettingModel, int>) (c => modes.IndexOf(c.Id.AnnotationMode))).ToArray<ToolbarSettingModel>();
          if (array.Length != 0)
            return array[0];
        }
      }
      return this.AllAnnotationButton.FirstOrDefault<ToolbarAnnotationButtonModel>((Func<ToolbarAnnotationButtonModel, bool>) (c => c.IsCheckable && c.IsChecked))?.ToolbarSettingModel;
    }
  }

  public void UpdateViewerToobarPadding()
  {
    PdfDocument document = this.mainViewModel.Document;
    if (document == null)
      return;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(document);
    ScrollViewer scrollViewer = pdfControl?.ScrollViewer;
    if (!(pdfControl.Parent is FrameworkElement parent))
      return;
    if (!(parent.RenderTransform is TranslateTransform translateTransform))
    {
      translateTransform = new TranslateTransform();
      parent.RenderTransform = (Transform) translateTransform;
    }
    if (this.mainViewModel.ViewToolbar.SubViewModeContinuous == SubViewModeContinuous.Discontinuous)
    {
      translateTransform.Y = 0.0;
      if (this.CheckedButtonToolbarSetting != null && scrollViewer.VerticalOffset < 1.0)
      {
        double top = 43.0;
        double? toolbarSettingHeight = GetToolbarSettingHeight(parent);
        if (toolbarSettingHeight.HasValue)
          top = 10.0 + toolbarSettingHeight.Value;
        pdfControl.PagePadding = new Thickness(10.0, top, 10.0, 10.0);
        scrollViewer.ScrollToTop();
        scrollViewer.UpdateLayout();
      }
      else
        pdfControl.PagePadding = new Thickness(10.0, 10.0, 10.0, 10.0);
    }
    else
    {
      pdfControl.PagePadding = new Thickness(10.0, 10.0, 10.0, 10.0);
      if (this.CheckedButtonToolbarSetting != null && scrollViewer.VerticalOffset < 1.0)
      {
        double? toolbarSettingHeight = GetToolbarSettingHeight(parent);
        if (toolbarSettingHeight.HasValue)
          translateTransform.Y = toolbarSettingHeight.Value;
        else
          translateTransform.Y = 33.0;
      }
      else
        translateTransform.Y = 0.0;
    }

    static double? GetToolbarSettingHeight(FrameworkElement _container)
    {
      return _container.FindName("AnnotToolbarSettingPanel") is UserControl name1 && name1.Content is FrameworkElement content && content.FindName("ClipBorder") is FrameworkElement name2 ? new double?(Math.Max(name2.ActualHeight, name2.Height) + name2.Margin.Top) : new double?();
    }
  }

  private void DoMenuItemCmd(ContextMenuItemModel model)
  {
    if (!(model.Parent is TypedContextMenuItemModel parent1))
      return;
    if (!(parent1.Parent is TypedContextMenuModel parent2))
      return;
    try
    {
      CommomLib.Commom.GAManager.SendEvent("AnnotationMenuClick", parent2.Mode.ToString(), parent1.Type.ToString(), 1L);
    }
    catch
    {
    }
    if (model is ColorMoreItemContextMenuItemModel)
    {
      this.ClearAdditionalMenuItem();
      ContextMenuItemModel contextMenuItem = ToolbarContextMenuHelper.CreateContextMenuItem(parent2.Mode, parent1.Type, model.TagData.MenuItemValue, true, new Action<ContextMenuItemModel>(this.DoMenuItemCmd));
      if (contextMenuItem != null)
      {
        int index = parent1.IndexOf((IContextMenuModel) model);
        parent1.Insert(index, (IContextMenuModel) contextMenuItem);
        contextMenuItem.IsChecked = true;
      }
    }
    string propertyName = AnnotationMenuPropertyAccessor.BuildPropertyName(parent2.Mode, parent1.Type);
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
    if (pdfControl != null)
    {
      AnnotationHolderManager annotationHolderManager = PdfObjectExtensions.GetAnnotationHolderManager(pdfControl);
      int pageIndex = -1;
      if (!annotationHolderManager?.OnPropertyChanged(propertyName, out pageIndex).GetValueOrDefault() && parent2 != null)
        parent2.Owner?.Parent?.Tap();
    }
    TagDataModel tagData = model.TagData;
    if ((tagData != null ? (tagData.IsTransient ? 1 : 0) : 0) == 0)
      return;
    int index1 = parent1.IndexOf((IContextMenuModel) model);
    ContextMenuItemModel contextMenuItem1 = ToolbarContextMenuHelper.CreateContextMenuItem(parent2.Mode, parent1.Type, model.TagData.MenuItemValue, false, new Action<ContextMenuItemModel>(this.DoMenuItemCmd));
    if (contextMenuItem1 == null)
      return;
    parent1[index1] = (IContextMenuModel) contextMenuItem1;
    contextMenuItem1.IsChecked = true;
  }

  private void InitMenu()
  {
    TypedContextMenuModel contextMenuModel1 = new TypedContextMenuModel(AnnotationMode.Stamp);
    contextMenuModel1.Add(ToolbarContextMenuHelper.CreatePresetsMenu(AnnotationMode.Stamp, new Action<ContextMenuItemModel>(this.DoStampPresetsCmd)));
    contextMenuModel1.Add(ToolbarContextMenuHelper.CreteStampMenu(AnnotationMode.Stamp, new Action<ContextMenuItemModel>(this.DoStampCmd)));
    this.stampMenuItems = contextMenuModel1;
    TypedContextMenuModel contextMenuModel2 = new TypedContextMenuModel(AnnotationMode.Stamp);
    contextMenuModel2.Add(ToolbarContextMenuHelper.CreatePresetsMenu(AnnotationMode.Stamp, new Action<ContextMenuItemModel>(this.DoStampPresetsCmd)));
    contextMenuModel2.Add(ToolbarContextMenuHelper.CreteStampMenu(AnnotationMode.Stamp, new Action<ContextMenuItemModel>(this.DoStampCmd)));
    this.stampMenuItems2 = contextMenuModel2;
    TypedContextMenuModel contextMenuModel3 = new TypedContextMenuModel(AnnotationMode.None);
    contextMenuModel3.Add(ToolbarContextMenuHelper.CreateShareEmailMenu(AnnotationMode.None, new Action<ContextMenuItemModel>(this.SharebyEmailCmd)));
    contextMenuModel3.Add(ToolbarContextMenuHelper.CreateShareFileMenu(AnnotationMode.None, new Action<ContextMenuItemModel>(this.SharebyFileCmd)));
    this.shareMenuItem = contextMenuModel3;
    TypedContextMenuModel contextMenuModel4 = new TypedContextMenuModel(AnnotationMode.Signature);
    contextMenuModel4.Add(ToolbarContextMenuHelper.CreateSignatureMenu(AnnotationMode.Signature, new Action<ContextMenuItemModel>(this.DoSignatureMenuCmd)));
    this.signatureMenuItems = contextMenuModel4;
    TypedContextMenuModel contextMenuModel5 = new TypedContextMenuModel(AnnotationMode.Signature);
    contextMenuModel5.Add(ToolbarContextMenuHelper.CreateSignatureMenu(AnnotationMode.Signature, new Action<ContextMenuItemModel>(this.DoSignatureMenuCmd)));
    this.signatureMenuItems2 = contextMenuModel5;
    TypedContextMenuModel contextMenuModel6 = new TypedContextMenuModel(AnnotationMode.Signature);
    contextMenuModel6.Add(ToolbarContextMenuHelper.CreateSignatureMenu(AnnotationMode.Signature, new Action<ContextMenuItemModel>(this.DoSignatureMenuCmd)));
    this.signatureMenuItems3 = contextMenuModel6;
    TypedContextMenuModel contextMenuModel7 = new TypedContextMenuModel(AnnotationMode.Watermark);
    contextMenuModel7.Add(ToolbarContextMenuHelper.CreateAddWatermarkMenu(AnnotationMode.Watermark, new Action<ContextMenuItemModel>(this.DoWatermarkInsertCmd)));
    contextMenuModel7.Add(ToolbarContextMenuHelper.CreateDeleteAllWatermarkMenu(AnnotationMode.Watermark, (Action<ContextMenuItemModel>) (m => this.DoWatermarkDelCmd(m, true))));
    this.waterMenuItems = contextMenuModel7;
    TypedContextMenuModel contextMenuModel8 = new TypedContextMenuModel(AnnotationMode.Link);
    contextMenuModel8.Add(ToolbarContextMenuHelper.CreateAddLinkMenu(AnnotationMode.Link, (Action<ContextMenuItemModel>) (m => this.LinkCmd(m))));
    contextMenuModel8.Add(ToolbarContextMenuHelper.CreateDeleteAllLinkMenu(AnnotationMode.Link, (Action<ContextMenuItemModel>) (m => this.DoLinkDelCmd(m))));
    this.linkMenuItems = contextMenuModel8;
  }

  public async void DoSignatureMenuCmd(ContextMenuItemModel model)
  {
    this.mainViewModel.ExitTransientMode();
    if (this.mainViewModel.Document == null)
      return;
    SignatureCreateWin signatureCreateWin = new SignatureCreateWin();
    bool? nullable = signatureCreateWin.ShowDialog();
    bool flag = false;
    if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
    {
      this.mainViewModel.AnnotationMode = AnnotationMode.None;
    }
    else
    {
      this.mainViewModel.AnnotationMode = AnnotationMode.Signature;
      await this.ProcessStampImageModelAsync(new StampImageModel()
      {
        ImageFilePath = signatureCreateWin.ResultModel.ImageFilePath,
        RemoveBackground = signatureCreateWin.ResultModel.RemoveBackground,
        IsSignature = true
      });
      this.mainViewModel.AnnotationMode = AnnotationMode.None;
    }
  }

  private async void DoStampAddImgCmd(ContextMenuItemModel model)
  {
    if (this.mainViewModel.Document == null)
      return;
    this.mainViewModel.AnnotationMode = AnnotationMode.Stamp;
    this.mainViewModel.ViewerOperationModel?.Dispose();
    OpenFileDialog openFileDialog1 = new OpenFileDialog();
    openFileDialog1.Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff|Windows Bitmap(*.bmp)|*.bmp|Windows Icon(*.ico)|*.ico|Graphics Interchange Format (*.gif)|(*.gif)|JPEG File Interchange Format (*.jpg)|*.jpg;*.jpeg|Portable Network Graphics (*.png)|*.png|Tag Image File Format (*.tif)|*.tif;*.tiff";
    openFileDialog1.ShowReadOnly = false;
    openFileDialog1.ReadOnlyChecked = true;
    OpenFileDialog openFileDialog2 = openFileDialog1;
    if (openFileDialog2.ShowDialog((Window) Application.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>()).GetValueOrDefault() && !string.IsNullOrEmpty(openFileDialog2.FileName))
    {
      this.StampImgFileOkTime = DateTime.Now;
      await this.ProcessStampImageModelAsync(new StampImageModel()
      {
        ImageFilePath = openFileDialog2.FileName
      });
    }
    else
      this.mainViewModel.AnnotationMode = AnnotationMode.None;
  }

  public async void SharebyEmailCmd(ContextMenuItemModel model)
  {
    if (this.mainViewModel.Document == null)
      return;
    if (this.mainViewModel.CanSave)
    {
      int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeSendMsg, UtilManager.GetProductName());
    }
    else
      await ShareUtils.SendMailAsync(this.mainViewModel.DocumentWrapper.DocumentPath);
  }

  public async void ShareCmd(ContextMenuItemModel model)
  {
    if (this.mainViewModel.Document == null)
      return;
    if (this.mainViewModel.CanSave)
    {
      int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeSendMsg, UtilManager.GetProductName());
    }
    else
      await ShareUtils.WindowsShareAsync(this.mainViewModel.DocumentWrapper.DocumentPath);
  }

  private void LinkCmd(ContextMenuItemModel model, bool Isbutton = true)
  {
    try
    {
      if (this.mainViewModel.Document == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (AnnotationToolbarViewModel.\u003C\u003Eo__145.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AnnotationToolbarViewModel.\u003C\u003Eo__145.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (AnnotationToolbarViewModel), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = AnnotationToolbarViewModel.\u003C\u003Eo__145.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = AnnotationToolbarViewModel.\u003C\u003Eo__145.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (AnnotationToolbarViewModel.\u003C\u003Eo__145.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AnnotationToolbarViewModel.\u003C\u003Eo__145.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, MouseModes, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (AnnotationToolbarViewModel), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, MouseModes, object> target2 = AnnotationToolbarViewModel.\u003C\u003Eo__145.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, MouseModes, object>> p1 = AnnotationToolbarViewModel.\u003C\u003Eo__145.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (AnnotationToolbarViewModel.\u003C\u003Eo__145.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AnnotationToolbarViewModel.\u003C\u003Eo__145.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof (AnnotationToolbarViewModel), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = AnnotationToolbarViewModel.\u003C\u003Eo__145.\u003C\u003Ep__0.Target((CallSite) AnnotationToolbarViewModel.\u003C\u003Eo__145.\u003C\u003Ep__0, this.mainViewModel.ViewerMouseMode);
      object obj2 = target2((CallSite) p1, obj1, MouseModes.Default);
      if (target1((CallSite) p2, obj2))
      {
        // ISSUE: reference to a compiler-generated field
        if (AnnotationToolbarViewModel.\u003C\u003Eo__145.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AnnotationToolbarViewModel.\u003C\u003Eo__145.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, MouseModes, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Value", typeof (AnnotationToolbarViewModel), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = AnnotationToolbarViewModel.\u003C\u003Eo__145.\u003C\u003Ep__3.Target((CallSite) AnnotationToolbarViewModel.\u003C\u003Eo__145.\u003C\u003Ep__3, this.mainViewModel.ViewerMouseMode, MouseModes.Default);
      }
      ToolbarAnnotationButtonModel parent = (model.Parent as TypedContextMenuModel).Owner.Parent as ToolbarAnnotationButtonModel;
      if (Isbutton)
        this.LinkButtonModel.IsChecked = !this.LinkButtonModel.IsChecked;
      if (!this.LinkButtonModel.IsChecked)
      {
        this.mainViewModel.AnnotationMode = AnnotationMode.None;
        this.mainViewModel.SelectedAnnotation = (PdfAnnotation) null;
        this.NotifyCheckedButtonToolbarSettingChanged();
      }
      PdfAnnotation selectedAnnotation = this.mainViewModel.SelectedAnnotation;
      this.mainViewModel.ExitTransientMode(formLink: true);
      PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
      if (parent != null)
      {
        if (parent.IsChecked)
        {
          pdfControl.Viewer.IsLinkAnnotationHighlighted = true;
        }
        else
        {
          pdfControl.Viewer.IsLinkAnnotationHighlighted = false;
          if ((PdfWrapper) selectedAnnotation != (PdfWrapper) null)
            this.mainViewModel.SelectedAnnotation = (PdfAnnotation) null;
        }
      }
      AnnotationMode annotationMode1 = this.mainViewModel.AnnotationMode;
      if (pdfControl != null)
      {
        AnnotationHolderManager annotationHolderManager = PdfObjectExtensions.GetAnnotationHolderManager(pdfControl);
        if (annotationHolderManager?.CurrentHolder is LinkAnnotationHolder && annotationHolderManager != null && annotationHolderManager.CurrentHolder.State == AnnotationHolderState.CreatingNew)
        {
          AnnotationMode annotationMode2 = this.mainViewModel.AnnotationMode;
          annotationHolderManager.CancelAll();
          this.mainViewModel.AnnotationMode = annotationMode2;
        }
      }
      this.mainViewModel.RaiseAnnotationModePropertyChanged();
      if (!((PdfWrapper) selectedAnnotation != (PdfWrapper) null))
        return;
      if (annotationMode1 == AnnotationMode.None)
        this.mainViewModel.ReleaseViewerFocusAsync(false);
      else if (annotationMode1 == AnnotationMode.Ink)
      {
        this.mainViewModel.ReleaseViewerFocusAsync(false);
      }
      else
      {
        if (annotationMode1 == AnnotationMode.None)
          return;
        System.Collections.Generic.IReadOnlyList<AnnotationMode> annotationModes = AnnotationFactory.GetAnnotationModes(selectedAnnotation);
        if (annotationModes.Count != 0 && annotationModes[0] == annotationMode1)
          return;
        this.mainViewModel.ReleaseViewerFocusAsync(true);
        this.NotifyCheckedButtonToolbarSettingChanged();
      }
    }
    catch (Exception ex)
    {
    }
  }

  public async void AddLinkDirectly()
  {
    FS_RECTF selectedDestination = this.GetSelectedDestination();
    if ((double) selectedDestination.Width <= 5.0 && (double) selectedDestination.Height <= 5.0)
      return;
    LinkEditWindows linkEditWindows = new LinkEditWindows(this.mainViewModel.Document);
    linkEditWindows.Owner = App.Current.MainWindow;
    linkEditWindows.WindowStartupLocation = linkEditWindows.Owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
    bool? nullable = linkEditWindows.ShowDialog();
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    PdfPage page = this.mainViewModel.Document.Pages.CurrentPage;
    if (page.Annots == null)
      page.CreateAnnotations();
    if (nullable.GetValueOrDefault())
    {
      PdfLinkAnnotation LinkAnnot = new PdfLinkAnnotation(page);
      if (linkEditWindows.SelectedType == LinkSelect.ToPage)
      {
        int num = linkEditWindows.Page - 1;
        PdfDestination xyz = PdfDestination.CreateXYZ(this.mainViewModel.Document, num, top: new float?(this.mainViewModel.Document.Pages[num].Height));
        LinkAnnot.Link.Action = (PdfAction) new PdfGoToAction(this.mainViewModel.Document, xyz);
      }
      else if (linkEditWindows.SelectedType == LinkSelect.ToWeb)
        LinkAnnot.Link.Action = (PdfAction) new PdfUriAction(this.mainViewModel.Document, linkEditWindows.UrlFilePath);
      else if (linkEditWindows.SelectedType == LinkSelect.ToFile)
        LinkAnnot.Link.Action = (PdfAction) new PdfLaunchAction(this.mainViewModel.Document, new PdfFileSpecification(this.mainViewModel.Document)
        {
          FileName = linkEditWindows.FileDiaoligFiePath
        });
      System.Windows.Media.Color color = (System.Windows.Media.Color) ColorConverter.ConvertFromString(linkEditWindows.SelectedFontground);
      FS_COLOR fsColor = new FS_COLOR((int) color.A, (int) color.R, (int) color.G, (int) color.B);
      float num1;
      if (!linkEditWindows.rectangleVis)
      {
        num1 = 0.0f;
      }
      else
      {
        LinkAnnot.Color = fsColor;
        num1 = linkEditWindows.BorderWidth;
      }
      LinkAnnot.Rectangle = selectedDestination;
      LinkAnnot.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
      LinkAnnot.Flags |= AnnotationFlags.Print;
      LinkAnnot.SetBorderStyle(new PdfBorderStyle()
      {
        Width = num1,
        Style = linkEditWindows.BorderStyles,
        DashPattern = new float[2]{ 2f, 4f }
      });
      page.Annots.Add((PdfAnnotation) LinkAnnot);
      await requiredService.OperationManager.TraceAnnotationInsertAsync((PdfAnnotation) LinkAnnot);
      await page.TryRedrawPageAsync();
      if ((PdfWrapper) LinkAnnot != (PdfWrapper) null)
        CommomLib.Commom.GAManager.SendEvent("AnnotationAction", "PdfLinkAnnotation", "New", 1L);
      LinkAnnot = (PdfLinkAnnotation) null;
    }
    page = (PdfPage) null;
  }

  private FS_RECTF GetSelectedDestination()
  {
    PdfViewer viewer = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document)?.Viewer;
    TextInfo[] array = PdfTextMarkupAnnotationUtils.GetTextInfos(this.mainViewModel.Document, viewer.SelectInfo).ToArray<TextInfo>();
    List<FS_RECTF> source = new List<FS_RECTF>();
    foreach (TextInfo textInfo in array)
      source.AddRange((IEnumerable<FS_RECTF>) PdfTextMarkupAnnotationUtils.GetNormalizedRects(viewer, textInfo, true, true));
    if (source.Count == 1)
      return source.FirstOrDefault<FS_RECTF>();
    float num1 = float.MaxValue;
    float num2 = float.MaxValue;
    float num3 = float.MinValue;
    float num4 = float.MinValue;
    foreach (FS_RECTF fsRectf in source)
    {
      num1 = Math.Min(num1, Math.Min(fsRectf.left, fsRectf.right));
      num2 = Math.Min(num2, Math.Min(fsRectf.top, fsRectf.bottom));
      num3 = Math.Max(num3, Math.Max(fsRectf.left, fsRectf.right));
      num4 = Math.Max(num4, Math.Max(fsRectf.top, fsRectf.bottom));
    }
    return new FS_RECTF(num1, num2, num3, num4);
  }

  public void SharebyFileCmd(ContextMenuItemModel model)
  {
    if (this.mainViewModel.Document == null)
      return;
    if (this.mainViewModel.CanSave)
    {
      int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeSendMsg, UtilManager.GetProductName());
    }
    else
    {
      MainView mainView = App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
      ShareSendFileDialog shareSendFileDialog = new ShareSendFileDialog(this.mainViewModel.DocumentWrapper.DocumentPath);
      shareSendFileDialog.Owner = (Window) mainView;
      shareSendFileDialog.WindowStartupLocation = mainView == null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;
      shareSendFileDialog.ShowDialog();
    }
  }

  public async void DoStampCmd(ContextMenuItemModel model)
  {
    if (this.mainViewModel.Document == null)
      return;
    this.mainViewModel.AnnotationMode = AnnotationMode.Stamp;
    StampEditWindows stampEditWindows = new StampEditWindows();
    if (!stampEditWindows.ShowDialog().GetValueOrDefault())
    {
      this.mainViewModel.ViewerOperationModel?.Dispose();
      this.mainViewModel.AnnotationMode = AnnotationMode.None;
    }
    else
    {
      if (this.mainViewModel.Document != null)
      {
        if (stampEditWindows.isSave)
        {
          if (stampEditWindows.isText)
          {
            this.SaveStamp((StampTextModel) stampEditWindows.StampTextModel);
            await this.ProcessStampTextModelAsync(stampEditWindows.StampTextModel);
          }
          else
            await this.ProcessStampImageModelAsync(new StampImageModel()
            {
              ImageFilePath = stampEditWindows.ResultModel.ImageFilePath,
              RemoveBackground = stampEditWindows.ResultModel.RemoveBackground
            });
        }
        else if (stampEditWindows.isText)
          await this.ProcessStampTextModelAsync(stampEditWindows.StampTextModel);
        else
          await this.ProcessStampImageModelAsync(new StampImageModel()
          {
            ImageFilePath = stampEditWindows.FileDiaoligFiePath,
            RemoveBackground = stampEditWindows.ResultModel.RemoveBackground
          });
      }
      this.mainViewModel.AnnotationMode = AnnotationMode.None;
    }
  }

  private async void DoStampPresetsCmd(ContextMenuItemModel model)
  {
    if (this.mainViewModel.Document == null)
      return;
    CommomLib.Commom.GAManager.SendEvent("PdfStampAnnotation", "DoStamp", "Presets", 1L);
    this.mainViewModel.AnnotationMode = AnnotationMode.Stamp;
    if (model.TagData.MenuItemValue is IStampTextModel menuItemValue)
      await this.ProcessStampTextModelAsync(menuItemValue);
    this.mainViewModel.AnnotationMode = AnnotationMode.None;
  }

  internal async Task ProcessStampTextModelAsync(IStampTextModel model)
  {
    PdfViewer viewer = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document)?.Viewer;
    AnnotationHolderManager holder = PdfObjectExtensions.GetAnnotationHolderManager(viewer);
    if (holder == null)
    {
      viewer = (PdfViewer) null;
      holder = (AnnotationHolderManager) null;
    }
    else
    {
      this.mainViewModel.ExitTransientMode();
      holder.CancelAll();
      await holder.WaitForCancelCompletedAsync();
      if (model == null)
      {
        viewer = (PdfViewer) null;
        holder = (AnnotationHolderManager) null;
      }
      else
      {
        string textContent = model.TextContent;
        string str = "Helvetica-BoldOblique";
        FontStyle fontStyle = FontStyles.Italic;
        System.Windows.FontWeight fontWeight = FontWeights.Bold;
        if (!model.IsPreset && !PdfFontUtils.CheckStockFontSupport(FontStockNames.HelveticaBoldOblique.ToString(), textContent))
        {
          str = "#GLOBAL USER INTERFACE";
          fontStyle = FontStyles.Normal;
          fontWeight = FontWeights.Normal;
        }
        StampAnnotationMoveControl annotationMoveControl1 = new StampAnnotationMoveControl(new TextStampModel()
        {
          TextFontSize = 12.0,
          TextFontFamily = str,
          TextFontStyle = fontStyle,
          TextFontWeight = fontWeight,
          Text = textContent,
          BorderBrush = model.FontColor,
          Foreground = model.FontColor,
          TextWidth = 120.0,
          TextHeight = 30.0,
          PageScale = 1.0
        });
        annotationMoveControl1.Width = 120.0;
        annotationMoveControl1.Height = 30.0;
        StampAnnotationMoveControl annotationMoveControl2 = annotationMoveControl1;
        MainViewModel mainViewModel = this.mainViewModel;
        HoverOperationModel hoverOperationModel = new HoverOperationModel(viewer);
        hoverOperationModel.Data = (object) model;
        hoverOperationModel.SizeInDocument = new FS_SIZEF(120f, 30f);
        Viewbox viewbox = new Viewbox();
        viewbox.Child = (UIElement) annotationMoveControl2;
        hoverOperationModel.PreviewElement = (UIElement) viewbox;
        mainViewModel.ViewerOperationModel = (DataOperationModel) hoverOperationModel;
        ViewOperationResult<bool> task = await this.mainViewModel.ViewerOperationModel.Task;
        DataOperationModel viewerOperationModel = this.mainViewModel.ViewerOperationModel;
        if ((task != null ? (task.Value ? 1 : 0) : 0) == 0)
        {
          viewer = (PdfViewer) null;
          holder = (AnnotationHolderManager) null;
        }
        else
        {
          holder.CancelAll();
          holder.Stamp.StartCreateNew(this.mainViewModel.Document.Pages[viewerOperationModel.CurrentPage], viewerOperationModel.PositionFromDocument);
          System.Collections.Generic.IReadOnlyList<PdfStampAnnotation> newAsync = await holder.Stamp.CompleteCreateNewAsync();
          if (newAsync == null)
          {
            viewer = (PdfViewer) null;
            holder = (AnnotationHolderManager) null;
          }
          else if (newAsync.Count <= 0)
          {
            viewer = (PdfViewer) null;
            holder = (AnnotationHolderManager) null;
          }
          else
          {
            holder.Select((PdfAnnotation) newAsync[0], true);
            viewer = (PdfViewer) null;
            holder = (AnnotationHolderManager) null;
          }
        }
      }
    }
  }

  internal async Task ProcessStampImageModelAsync(StampImageModel model)
  {
    PdfViewer viewer = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document)?.Viewer;
    AnnotationHolderManager holder = PdfObjectExtensions.GetAnnotationHolderManager(viewer);
    if (holder == null)
    {
      viewer = (PdfViewer) null;
      holder = (AnnotationHolderManager) null;
    }
    else
    {
      this.mainViewModel.ExitTransientMode();
      holder.CancelAll();
      await holder.WaitForCancelCompletedAsync();
      ImageStampModel imgmodel = new ImageStampModel();
      WriteableBitmap writeableBitmap = (WriteableBitmap) null;
      try
      {
        using (FileStream fileStream = File.OpenRead(model.ImageFilePath))
        {
          BitmapImage source = new BitmapImage();
          source.CacheOption = BitmapCacheOption.OnLoad;
          source.BeginInit();
          source.StreamSource = (Stream) fileStream;
          source.EndInit();
          writeableBitmap = new WriteableBitmap((BitmapSource) source);
        }
      }
      catch
      {
        DrawUtils.ShowUnsupportedImageMessage();
        viewer = (PdfViewer) null;
        holder = (AnnotationHolderManager) null;
        return;
      }
      imgmodel.StampImageSource = (BitmapSource) writeableBitmap;
      Size size1 = new Size();
      PdfPage currentPage = this.mainViewModel.Document.Pages.CurrentPage;
      FS_RECTF effectiveBox = currentPage.GetEffectiveBox(currentPage.Rotation);
      Size size2 = this.mainViewModel.AnnotationMode != AnnotationMode.Signature ? StampAnnotationHolder.GetStampPageSize(writeableBitmap.Width, writeableBitmap.Height, effectiveBox) : StampAnnotationHolder.GetSignaturePageSize(writeableBitmap.Width, writeableBitmap.Height, effectiveBox);
      imgmodel.ImageWidth = size2.Width;
      imgmodel.ImageHeight = size2.Height;
      imgmodel.PageSize = new FS_SIZEF(size2.Width, size2.Height);
      StampAnnotationMoveControl annotationMoveControl1 = new StampAnnotationMoveControl(imgmodel);
      annotationMoveControl1.Width = size2.Width;
      annotationMoveControl1.Height = size2.Height;
      StampAnnotationMoveControl annotationMoveControl2 = annotationMoveControl1;
      model.ImageStampControlModel = imgmodel;
      MainViewModel mainViewModel = this.mainViewModel;
      HoverOperationModel hoverOperationModel = new HoverOperationModel(viewer);
      hoverOperationModel.Data = (object) model;
      hoverOperationModel.SizeInDocument = new FS_SIZEF(size2.Width, size2.Height);
      Viewbox viewbox = new Viewbox();
      viewbox.Child = (UIElement) annotationMoveControl2;
      hoverOperationModel.PreviewElement = (UIElement) viewbox;
      mainViewModel.ViewerOperationModel = (DataOperationModel) hoverOperationModel;
      ViewOperationResult<bool> task = await this.mainViewModel.ViewerOperationModel.Task;
      DataOperationModel viewerOperationModel = this.mainViewModel.ViewerOperationModel;
      if ((task != null ? (task.Value ? 1 : 0) : 0) == 0)
      {
        viewer = (PdfViewer) null;
        holder = (AnnotationHolderManager) null;
      }
      else
      {
        holder.CancelAll();
        holder.Stamp.StartCreateNew(this.mainViewModel.Document.Pages[viewerOperationModel.CurrentPage], viewerOperationModel.PositionFromDocument);
        System.Collections.Generic.IReadOnlyList<PdfStampAnnotation> newAsync = await holder.Stamp.CompleteCreateNewAsync();
        if (newAsync == null)
        {
          viewer = (PdfViewer) null;
          holder = (AnnotationHolderManager) null;
        }
        else if (newAsync.Count <= 0)
        {
          viewer = (PdfViewer) null;
          holder = (AnnotationHolderManager) null;
        }
        else
        {
          holder.Select((PdfAnnotation) newAsync[0], true);
          viewer = (PdfViewer) null;
          holder = (AnnotationHolderManager) null;
        }
      }
    }
  }

  private async void DoWatermarkInsertCmd(ContextMenuItemModel model)
  {
    if (this.mainViewModel.Document == null)
      return;
    CommomLib.Commom.GAManager.SendEvent("MainWindow", "Watermark", nameof (DoWatermarkInsertCmd), 1L);
    await this.mainViewModel.ReleaseViewerFocusAsync(true);
    this.mainViewModel.ExitTransientMode();
    bool? nullable = new WatermarkEditWin().ShowDialog();
    bool flag = false;
    if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
    {
      this.mainViewModel.AnnotationMode = AnnotationMode.None;
      this.WatermarkParam = (WatermarkParam) null;
      this.TextWatermarkModel = (WatermarkTextModel) null;
      this.ImageWatermarkModel = (WatermarkImageModel) null;
    }
    else
    {
      this.mainViewModel.SetCanSaveFlag();
      this.mainViewModel.AnnotationMode = AnnotationMode.Watermark;
      if (!this.mainViewModel.AnnotationToolbar.LinkButtonModel.IsChecked)
        return;
      this.mainViewModel.AnnotationMode = AnnotationMode.None;
    }
  }

  public async void DoWatermarkInsertCmd2()
  {
    if (this.mainViewModel.Document == null)
      return;
    CommomLib.Commom.GAManager.SendEvent("MainWindow", "Watermark", "DoWatermarkInsertCmd", 1L);
    await this.mainViewModel.ReleaseViewerFocusAsync(true);
    this.mainViewModel.ExitTransientMode();
    bool? nullable = new WatermarkEditWin().ShowDialog();
    bool flag = false;
    if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
    {
      this.mainViewModel.AnnotationMode = AnnotationMode.None;
      this.WatermarkParam = (WatermarkParam) null;
      this.TextWatermarkModel = (WatermarkTextModel) null;
      this.ImageWatermarkModel = (WatermarkImageModel) null;
    }
    else
    {
      this.mainViewModel.SetCanSaveFlag();
      this.mainViewModel.AnnotationMode = AnnotationMode.Watermark;
      if (!this.mainViewModel.AnnotationToolbar.LinkButtonModel.IsChecked)
        return;
      this.mainViewModel.AnnotationMode = AnnotationMode.None;
    }
  }

  private async void DoWatermarkDelCmd(ContextMenuItemModel model, bool allPage)
  {
    if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
      return;
    CommomLib.Commom.GAManager.SendEvent("MainWindow", "Watermark", nameof (DoWatermarkDelCmd), 1L);
    await this.mainViewModel.ReleaseViewerFocusAsync(true);
    this.mainViewModel.ExitTransientMode();
    if (MessageBox.Show(Resources.DeleteWatermarkAskMsg, UtilManager.GetProductName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
      return;
    int num1;
    int num2;
    if (allPage)
    {
      num1 = 0;
      num2 = this.mainViewModel.Document.Pages.Count;
    }
    else
    {
      num1 = this.mainViewModel.Document.Pages.CurrentIndex;
      num2 = 1;
    }
    bool flag = false;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
    (int num3, int num4) = pdfControl != null ? pdfControl.GetVisiblePageRange() : (-1, -1);
    for (int index1 = num1; index1 < num1 + num2; ++index1)
    {
      IntPtr num5 = IntPtr.Zero;
      PdfPage page = (PdfPage) null;
      try
      {
        num5 = Pdfium.FPDF_LoadPage(this.mainViewModel.Document.Handle, index1);
        if (num5 != IntPtr.Zero)
        {
          page = PdfPage.FromHandle(this.mainViewModel.Document, num5, index1);
          if (page.Annots != null)
          {
            if (page.Annots.Count > 0)
            {
              for (int index2 = page.Annots.Count - 1; index2 >= 0; --index2)
              {
                if (page.Annots[index2] is PdfWatermarkAnnotation)
                {
                  flag = true;
                  page.Annots.RemoveAt(index2);
                }
              }
            }
          }
        }
      }
      finally
      {
        if (page != null && (page.PageIndex > num4 || page.PageIndex < num3))
          PageDisposeHelper.DisposePage(page);
        if (num5 != IntPtr.Zero)
          Pdfium.FPDF_ClosePage(num5);
      }
    }
    if (flag)
      this.mainViewModel.SetCanSaveFlag();
    if (pdfControl == null)
      return;
    await pdfControl.TryRedrawVisiblePageAsync();
  }

  private async void DoLinkDelCmd(ContextMenuItemModel model)
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
      CommomLib.Commom.GAManager.SendEvent("MainWindow", "Link", nameof (DoLinkDelCmd), 1L);
      await this.mainViewModel.ReleaseViewerFocusAsync(true);
      this.mainViewModel.ExitTransientMode();
      if (MessageBox.Show(Resources.LinkDeleteAll, UtilManager.GetProductName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
      {
        viewer = (PDFKit.PdfControl) null;
      }
      else
      {
        int count = this.mainViewModel.Document.Pages.Count;
        viewer = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
        PDFKit.PdfControl scrollInfo = viewer;
        (int, int) valueTuple = scrollInfo != null ? scrollInfo.GetVisiblePageRange() : (-1, -1);
        Dictionary<int, List<BaseAnnotation>> dict = LinkOperationManagerExtensions.LinkDeleteAllRedo(this.mainViewModel.Document);
        if (dict != null && dict.Count > 0)
          await this.mainViewModel.OperationManager.AddOperationAsync((Func<PdfDocument, Task>) (async _doc =>
          {
            LinkOperationManagerExtensions.LinkDeleteAllUndo(dict, _doc);
            dict.Clear();
            PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(_doc);
            if (pdfControl == null)
              return;
            await pdfControl.TryRedrawVisiblePageAsync();
          }), (Func<PdfDocument, Task>) (async _doc =>
          {
            dict.Clear();
            dict = LinkOperationManagerExtensions.LinkDeleteAllRedo(_doc);
            PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(_doc);
            if (pdfControl == null)
              return;
            await pdfControl.TryRedrawVisiblePageAsync();
          }));
        if (viewer == null)
        {
          viewer = (PDFKit.PdfControl) null;
        }
        else
        {
          await viewer.TryRedrawVisiblePageAsync();
          viewer = (PDFKit.PdfControl) null;
        }
      }
    }
  }

  private void DoToolbarSettingsItemCmd(ToolbarSettingItemModel model)
  {
    string propertyName = AnnotationMenuPropertyAccessor.BuildPropertyName(model.Id.AnnotationMode, model.Type);
    switch (model.Id.AnnotationMode)
    {
      case AnnotationMode.Line:
        this.mainViewModel.AnnotationToolbar.LineButtonModel.IsChecked = true;
        break;
      case AnnotationMode.Ink:
        this.mainViewModel.AnnotationToolbar.InkButtonModel.IsChecked = true;
        break;
      case AnnotationMode.Shape:
        this.mainViewModel.AnnotationToolbar.SquareButtonModel.IsChecked = true;
        break;
      case AnnotationMode.Highlight:
        this.mainViewModel.AnnotationToolbar.HighlightButtonModel.IsChecked = true;
        break;
      case AnnotationMode.Underline:
        this.mainViewModel.AnnotationToolbar.UnderlineButtonModel.IsChecked = true;
        break;
      case AnnotationMode.Strike:
        this.mainViewModel.AnnotationToolbar.StrikeButtonModel.IsChecked = true;
        break;
      case AnnotationMode.HighlightArea:
        this.mainViewModel.AnnotationToolbar.HighlightAreaButtonModel.IsChecked = true;
        break;
      case AnnotationMode.Ellipse:
        this.mainViewModel.AnnotationToolbar.CircleButtonModel.IsChecked = true;
        break;
      case AnnotationMode.Text:
        this.mainViewModel.AnnotationToolbar.TextButtonModel.IsChecked = true;
        break;
      case AnnotationMode.TextBox:
        this.mainViewModel.AnnotationToolbar.TextBoxButtonModel.IsChecked = true;
        break;
    }
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
    if (pdfControl == null)
      return;
    AnnotationHolderManager annotationHolderManager = PdfObjectExtensions.GetAnnotationHolderManager(pdfControl);
    int pageIndex = -1;
    annotationHolderManager?.OnPropertyChanged(propertyName, out pageIndex);
  }

  private void DoToolbarSettingItemExitCmd(ToolbarSettingItemModel model)
  {
    this.mainViewModel.SelectedAnnotation = (PdfAnnotation) null;
    this.mainViewModel.AnnotationMode = AnnotationMode.None;
    this.NotifyCheckedButtonToolbarSettingChanged();
  }

  private void DoToolbarSettingItemLinkExitCmd(ToolbarSettingItemModel model)
  {
    this.mainViewModel.SelectedAnnotation = (PdfAnnotation) null;
    this.mainViewModel.AnnotationMode = AnnotationMode.None;
    this.NotifyCheckedButtonToolbarSettingChanged();
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
    if (pdfControl == null)
      return;
    pdfControl.Viewer.IsLinkAnnotationHighlighted = false;
  }

  private void InkToolbarSettingItemExitCmd(ToolbarSettingItemModel model)
  {
    (this.mainViewModel.AnnotationToolbar.inkButtonModel.ToolbarSettingModel[3] as ToolbarSettingInkEraserModel).IsChecked = false;
    this.mainViewModel.SelectedAnnotation = (PdfAnnotation) null;
    this.mainViewModel.AnnotationMode = AnnotationMode.None;
    this.NotifyCheckedButtonToolbarSettingChanged();
  }

  private void NotifyCheckedButtonToolbarSettingChanged()
  {
    this.OnPropertyChanged("CheckedButtonToolbarSetting");
    this.UpdateViewerToobarPadding();
  }

  private void InitToolbarAnnotationButtonModel()
  {
    this.InitMenu();
    ToolbarAnnotationButtonModel annotationButtonModel1 = new ToolbarAnnotationButtonModel(AnnotationMode.Highlight);
    annotationButtonModel1.Caption = Resources.MenuAnnotateHighlightContent;
    annotationButtonModel1.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/highlighttext.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/highlighttext.png"));
    // ISSUE: method pointer
    annotationButtonModel1.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__TextMarkupCommandFunc\u007C162_4)));
    ToolbarSettingModel toolbarSettingModel1 = new ToolbarSettingModel(AnnotationMode.Highlight);
    toolbarSettingModel1.Add(ToolbarSettingsHelper.CreateAnnotationModeIcon(AnnotationMode.Highlight));
    toolbarSettingModel1.Add(ToolbarSettingsHelper.CreateColor(AnnotationMode.Highlight, ContextMenuItemType.StrokeColor, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), (ImageSource) null));
    toolbarSettingModel1.Add(ToolbarSettingsHelper.CreateExitEdit(new Action<ToolbarSettingItemModel>(this.DoToolbarSettingItemExitCmd)));
    toolbarSettingModel1.Add((ToolbarSettingItemModel) ToolbarSettingsHelper.CreateApplyToDefault());
    annotationButtonModel1.ToolbarSettingModel = toolbarSettingModel1;
    this.HighlightButtonModel = annotationButtonModel1;
    ToolbarAnnotationButtonModel annotationButtonModel2 = new ToolbarAnnotationButtonModel(AnnotationMode.Underline);
    annotationButtonModel2.Caption = Resources.MenuAnnotateUnderlineContent;
    annotationButtonModel2.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/underline.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/underline.png"));
    // ISSUE: method pointer
    annotationButtonModel2.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__TextMarkupCommandFunc\u007C162_4)));
    ToolbarSettingModel toolbarSettingModel2 = new ToolbarSettingModel(AnnotationMode.Underline);
    toolbarSettingModel2.Add(ToolbarSettingsHelper.CreateAnnotationModeIcon(AnnotationMode.Underline));
    toolbarSettingModel2.Add(ToolbarSettingsHelper.CreateColor(AnnotationMode.Underline, ContextMenuItemType.StrokeColor, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), (ImageSource) null));
    toolbarSettingModel2.Add(ToolbarSettingsHelper.CreateExitEdit(new Action<ToolbarSettingItemModel>(this.DoToolbarSettingItemExitCmd)));
    toolbarSettingModel2.Add((ToolbarSettingItemModel) ToolbarSettingsHelper.CreateApplyToDefault());
    annotationButtonModel2.ToolbarSettingModel = toolbarSettingModel2;
    this.UnderlineButtonModel = annotationButtonModel2;
    ToolbarAnnotationButtonModel annotationButtonModel3 = new ToolbarAnnotationButtonModel(AnnotationMode.Strike);
    annotationButtonModel3.Caption = Resources.MenuAnnotateStrikeContent;
    annotationButtonModel3.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/strike.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/strike.png"));
    // ISSUE: method pointer
    annotationButtonModel3.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__TextMarkupCommandFunc\u007C162_4)));
    ToolbarSettingModel toolbarSettingModel3 = new ToolbarSettingModel(AnnotationMode.Strike);
    toolbarSettingModel3.Add(ToolbarSettingsHelper.CreateAnnotationModeIcon(AnnotationMode.Strike));
    toolbarSettingModel3.Add(ToolbarSettingsHelper.CreateColor(AnnotationMode.Strike, ContextMenuItemType.StrokeColor, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), (ImageSource) null));
    toolbarSettingModel3.Add(ToolbarSettingsHelper.CreateExitEdit(new Action<ToolbarSettingItemModel>(this.DoToolbarSettingItemExitCmd)));
    toolbarSettingModel3.Add((ToolbarSettingItemModel) ToolbarSettingsHelper.CreateApplyToDefault());
    annotationButtonModel3.ToolbarSettingModel = toolbarSettingModel3;
    this.StrikeButtonModel = annotationButtonModel3;
    ToolbarAnnotationButtonModel annotationButtonModel4 = new ToolbarAnnotationButtonModel(AnnotationMode.Line);
    annotationButtonModel4.Caption = Resources.MenuAnnotateLineContent;
    annotationButtonModel4.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/line.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/line.png"));
    // ISSUE: method pointer
    annotationButtonModel4.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__CommandFunc\u007C162_0)));
    ToolbarSettingModel toolbarSettingModel4 = new ToolbarSettingModel(AnnotationMode.Line);
    toolbarSettingModel4.Add(ToolbarSettingsHelper.CreateAnnotationModeIcon(AnnotationMode.Line));
    toolbarSettingModel4.Add(ToolbarSettingsHelper.CreateStrokeThickness(AnnotationMode.Line, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), (ImageSource) null));
    toolbarSettingModel4.Add(ToolbarSettingsHelper.CreateCollapsedColor(AnnotationMode.Line, ContextMenuItemType.StrokeColor, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), ToolbarSettingsHelper.CreateIcon(ContextMenuItemType.StrokeColor)));
    toolbarSettingModel4.Add(ToolbarSettingsHelper.CreateExitEdit(new Action<ToolbarSettingItemModel>(this.DoToolbarSettingItemExitCmd)));
    toolbarSettingModel4.Add((ToolbarSettingItemModel) ToolbarSettingsHelper.CreateApplyToDefault());
    annotationButtonModel4.ToolbarSettingModel = toolbarSettingModel4;
    this.LineButtonModel = annotationButtonModel4;
    ToolbarAnnotationButtonModel annotationButtonModel5 = new ToolbarAnnotationButtonModel(AnnotationMode.Shape);
    annotationButtonModel5.Caption = Resources.MenuAnnotateShapeContent;
    annotationButtonModel5.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/shape.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/shape.png"));
    // ISSUE: method pointer
    annotationButtonModel5.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__CommandFunc\u007C162_0)));
    ToolbarSettingModel toolbarSettingModel5 = new ToolbarSettingModel(AnnotationMode.Shape);
    toolbarSettingModel5.Add(ToolbarSettingsHelper.CreateAnnotationModeIcon(AnnotationMode.Shape));
    toolbarSettingModel5.Add(ToolbarSettingsHelper.CreateStrokeThickness(AnnotationMode.Shape, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), (ImageSource) null));
    toolbarSettingModel5.Add(ToolbarSettingsHelper.CreateCollapsedColor(AnnotationMode.Shape, ContextMenuItemType.StrokeColor, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), ToolbarSettingsHelper.CreateIcon(ContextMenuItemType.StrokeColor)));
    toolbarSettingModel5.Add(ToolbarSettingsHelper.CreateCollapsedColor(AnnotationMode.Shape, ContextMenuItemType.FillColor, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), ToolbarSettingsHelper.CreateIcon(ContextMenuItemType.FillColor)));
    toolbarSettingModel5.Add(ToolbarSettingsHelper.CreateExitEdit(new Action<ToolbarSettingItemModel>(this.DoToolbarSettingItemExitCmd)));
    toolbarSettingModel5.Add((ToolbarSettingItemModel) ToolbarSettingsHelper.CreateApplyToDefault());
    annotationButtonModel5.ToolbarSettingModel = toolbarSettingModel5;
    this.SquareButtonModel = annotationButtonModel5;
    ToolbarAnnotationButtonModel annotationButtonModel6 = new ToolbarAnnotationButtonModel(AnnotationMode.Ellipse);
    annotationButtonModel6.Caption = Resources.MenuAnnotateEllipseContent;
    annotationButtonModel6.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/ellipse.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/ellipse.png"));
    // ISSUE: method pointer
    annotationButtonModel6.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__CommandFunc\u007C162_0)));
    ToolbarSettingModel toolbarSettingModel6 = new ToolbarSettingModel(AnnotationMode.Ellipse);
    toolbarSettingModel6.Add(ToolbarSettingsHelper.CreateAnnotationModeIcon(AnnotationMode.Ellipse));
    toolbarSettingModel6.Add(ToolbarSettingsHelper.CreateStrokeThickness(AnnotationMode.Ellipse, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), (ImageSource) null));
    toolbarSettingModel6.Add(ToolbarSettingsHelper.CreateCollapsedColor(AnnotationMode.Ellipse, ContextMenuItemType.StrokeColor, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), ToolbarSettingsHelper.CreateIcon(ContextMenuItemType.StrokeColor)));
    toolbarSettingModel6.Add(ToolbarSettingsHelper.CreateCollapsedColor(AnnotationMode.Ellipse, ContextMenuItemType.FillColor, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), ToolbarSettingsHelper.CreateIcon(ContextMenuItemType.FillColor)));
    toolbarSettingModel6.Add(ToolbarSettingsHelper.CreateExitEdit(new Action<ToolbarSettingItemModel>(this.DoToolbarSettingItemExitCmd)));
    toolbarSettingModel6.Add((ToolbarSettingItemModel) ToolbarSettingsHelper.CreateApplyToDefault());
    annotationButtonModel6.ToolbarSettingModel = toolbarSettingModel6;
    this.CircleButtonModel = annotationButtonModel6;
    ToolbarAnnotationButtonModel annotationButtonModel7 = new ToolbarAnnotationButtonModel(AnnotationMode.Ink);
    annotationButtonModel7.Caption = Resources.MenuAnnotateInkContent;
    annotationButtonModel7.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/Ink.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/Ink.png"));
    annotationButtonModel7.Tooltip = Resources.MenuAnnotateInkContent;
    // ISSUE: method pointer
    annotationButtonModel7.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__InkCommandFunc\u007C162_9)));
    ToolbarSettingModel toolbarSettingModel7 = new ToolbarSettingModel(AnnotationMode.Ink);
    toolbarSettingModel7.Add(ToolbarSettingsHelper.CreateAnnotationModeIcon(AnnotationMode.Ink));
    toolbarSettingModel7.Add(ToolbarSettingsHelper.CreateStrokeThickness(AnnotationMode.Ink, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), (ImageSource) null));
    toolbarSettingModel7.Add(ToolbarSettingsHelper.CreateColor(AnnotationMode.Ink, ContextMenuItemType.StrokeColor, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), (ImageSource) null));
    toolbarSettingModel7.Add(ToolbarSettingsHelper.CreteEreserState(AnnotationMode.Ink, "Eraser", false, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd)));
    toolbarSettingModel7.Add(ToolbarSettingsHelper.CreateExitEdit(new Action<ToolbarSettingItemModel>(this.InkToolbarSettingItemExitCmd)));
    toolbarSettingModel7.Add((ToolbarSettingItemModel) ToolbarSettingsHelper.CreateApplyToDefault());
    annotationButtonModel7.ToolbarSettingModel = toolbarSettingModel7;
    this.InkButtonModel = annotationButtonModel7;
    ToolbarAnnotationButtonModel annotationButtonModel8 = new ToolbarAnnotationButtonModel(AnnotationMode.HighlightArea);
    annotationButtonModel8.Caption = Resources.WinToolBarBtnHighlightContent;
    annotationButtonModel8.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/highlightarea2.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/highlightarea2.png"));
    // ISSUE: method pointer
    annotationButtonModel8.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__CommandFunc\u007C162_0)));
    ToolbarSettingModel toolbarSettingModel8 = new ToolbarSettingModel(AnnotationMode.HighlightArea);
    toolbarSettingModel8.Add(ToolbarSettingsHelper.CreateAnnotationModeIcon(AnnotationMode.HighlightArea));
    toolbarSettingModel8.Add(ToolbarSettingsHelper.CreateColor(AnnotationMode.HighlightArea, ContextMenuItemType.StrokeColor, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), (ImageSource) null));
    toolbarSettingModel8.Add(ToolbarSettingsHelper.CreateExitEdit(new Action<ToolbarSettingItemModel>(this.DoToolbarSettingItemExitCmd)));
    toolbarSettingModel8.Add((ToolbarSettingItemModel) ToolbarSettingsHelper.CreateApplyToDefault());
    annotationButtonModel8.ToolbarSettingModel = toolbarSettingModel8;
    this.HighlightAreaButtonModel = annotationButtonModel8;
    ToolbarAnnotationButtonModel annotationButtonModel9 = new ToolbarAnnotationButtonModel(AnnotationMode.TextBox);
    annotationButtonModel9.Caption = Resources.MenuAnnotateTextBoxContent;
    annotationButtonModel9.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/textbox.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/textbox.png"));
    // ISSUE: method pointer
    annotationButtonModel9.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__CommandFunc\u007C162_0)));
    ToolbarSettingModel toolbarSettingModel9 = new ToolbarSettingModel(AnnotationMode.TextBox);
    toolbarSettingModel9.Add(ToolbarSettingsHelper.CreateAnnotationModeIcon(AnnotationMode.TextBox));
    toolbarSettingModel9.Add(ToolbarSettingsHelper.CreateFontSize(AnnotationMode.TextBox, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), (ImageSource) null));
    toolbarSettingModel9.Add(ToolbarSettingsHelper.CreateCollapsedColor(AnnotationMode.TextBox, ContextMenuItemType.FontColor, "Test1", new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), ToolbarSettingsHelper.CreateIcon(ContextMenuItemType.FontColor)));
    toolbarSettingModel9.Add(ToolbarSettingsHelper.CreateStrokeThickness(AnnotationMode.TextBox, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), (ImageSource) null));
    toolbarSettingModel9.Add(ToolbarSettingsHelper.CreateCollapsedColor(AnnotationMode.TextBox, ContextMenuItemType.StrokeColor, "Test1", new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), ToolbarSettingsHelper.CreateIcon(ContextMenuItemType.StrokeColor)));
    toolbarSettingModel9.Add(ToolbarSettingsHelper.CreateCollapsedColor(AnnotationMode.TextBox, ContextMenuItemType.FillColor, "Test1", new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), ToolbarSettingsHelper.CreateIcon(ContextMenuItemType.FillColor)));
    toolbarSettingModel9.Add(ToolbarSettingsHelper.CreateExitEdit(new Action<ToolbarSettingItemModel>(this.DoToolbarSettingItemExitCmd)));
    toolbarSettingModel9.Add((ToolbarSettingItemModel) ToolbarSettingsHelper.CreateApplyToDefault());
    annotationButtonModel9.ToolbarSettingModel = toolbarSettingModel9;
    this.TextBoxButtonModel = annotationButtonModel9;
    ToolbarAnnotationButtonModel annotationButtonModel10 = new ToolbarAnnotationButtonModel(AnnotationMode.Text);
    annotationButtonModel10.Caption = Resources.MenuAnnotateTypeWriterContent;
    annotationButtonModel10.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/insertText.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/insertText.png"));
    // ISSUE: method pointer
    annotationButtonModel10.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__TextCommandFunc\u007C162_1)));
    ToolbarSettingModel toolbarSettingModel10 = new ToolbarSettingModel(AnnotationMode.Text);
    toolbarSettingModel10.Add(ToolbarSettingsHelper.CreateAnnotationModeIcon(AnnotationMode.Text));
    toolbarSettingModel10.Add(ToolbarSettingsHelper.CreateFontSize(AnnotationMode.Text, new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), (ImageSource) null));
    toolbarSettingModel10.Add(ToolbarSettingsHelper.CreateCollapsedColor(AnnotationMode.Text, ContextMenuItemType.FontColor, "Test1", new Action<ToolbarSettingItemModel>(this.DoToolbarSettingsItemCmd), ToolbarSettingsHelper.CreateIcon(ContextMenuItemType.FontColor)));
    toolbarSettingModel10.Add(ToolbarSettingsHelper.CreateExitEdit(new Action<ToolbarSettingItemModel>(this.DoToolbarSettingItemExitCmd)));
    toolbarSettingModel10.Add((ToolbarSettingItemModel) ToolbarSettingsHelper.CreateApplyToDefault());
    annotationButtonModel10.ToolbarSettingModel = toolbarSettingModel10;
    this.TextButtonModel = annotationButtonModel10;
    ToolbarAnnotationButtonModel annotationButtonModel11 = new ToolbarAnnotationButtonModel(AnnotationMode.Note);
    annotationButtonModel11.Caption = Resources.MenuAnnotateNoteContent;
    annotationButtonModel11.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/note.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/note.png"));
    annotationButtonModel11.ChildButtonModel = (ToolbarChildButtonModel) null;
    // ISSUE: method pointer
    annotationButtonModel11.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__NoteCommandFunc\u007C162_2)));
    this.NoteButtonModel = annotationButtonModel11;
    ToolbarAnnotationButtonModel annotationButtonModel12 = new ToolbarAnnotationButtonModel(AnnotationMode.Stamp);
    annotationButtonModel12.Caption = Resources.MenuAnnotateStampContent;
    annotationButtonModel12.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/stamp.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/stamp.png"));
    annotationButtonModel12.ChildButtonModel = (ToolbarChildButtonModel) new ToolbarChildCheckableButtonModel()
    {
      ContextMenu = (IContextMenuModel) this.stampMenuItems
    };
    // ISSUE: method pointer
    annotationButtonModel12.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__StampCommandFunc\u007C162_5)));
    this.StampButtonModel = annotationButtonModel12;
    ToolbarAnnotationButtonModel annotationButtonModel13 = new ToolbarAnnotationButtonModel(AnnotationMode.None);
    annotationButtonModel13.Icon = (ImageSource) new BitmapImage(new Uri("/Style/Resources/ShareBtn.png", UriKind.Relative));
    annotationButtonModel13.ChildButtonModel = (ToolbarChildButtonModel) new ToolbarChildCheckableButtonModel()
    {
      ContextMenu = (IContextMenuModel) this.shareMenuItem
    };
    // ISSUE: method pointer
    annotationButtonModel13.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__StampCommandFunc\u007C162_5)));
    this.ShareButtonModel = annotationButtonModel13;
    ToolbarAnnotationButtonModel annotationButtonModel14 = new ToolbarAnnotationButtonModel(AnnotationMode.Image);
    annotationButtonModel14.Caption = Resources.MenuInsertImageContent;
    annotationButtonModel14.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/InsertPicture.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/InsertPicture.png"));
    annotationButtonModel14.ChildButtonModel = (ToolbarChildButtonModel) null;
    // ISSUE: method pointer
    annotationButtonModel14.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__InsertImageCommandFunc\u007C162_6)));
    this.ImageButtonModel = annotationButtonModel14;
    ToolbarAnnotationButtonModel annotationButtonModel15 = new ToolbarAnnotationButtonModel(AnnotationMode.Signature);
    annotationButtonModel15.Caption = Resources.MenuAnnotateSignatureContent;
    annotationButtonModel15.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/signature.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/signature.png"));
    annotationButtonModel15.ChildButtonModel = (ToolbarChildButtonModel) new ToolbarChildCheckableButtonModel()
    {
      ContextMenu = (IContextMenuModel) this.signatureMenuItems
    };
    // ISSUE: method pointer
    annotationButtonModel15.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__SignatureCommandFunc\u007C162_7)));
    this.SignatureButtonModel = annotationButtonModel15;
    ToolbarAnnotationButtonModel annotationButtonModel16 = new ToolbarAnnotationButtonModel(AnnotationMode.Watermark);
    annotationButtonModel16.Caption = Resources.MenuAnnotateWatermarkContent;
    annotationButtonModel16.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/Watermark.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/Watermark.png"));
    annotationButtonModel16.ChildButtonModel = (ToolbarChildButtonModel) new ToolbarChildCheckableButtonModel()
    {
      ContextMenu = (IContextMenuModel) this.waterMenuItems
    };
    // ISSUE: method pointer
    annotationButtonModel16.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__WatermarkCommandFunc\u007C162_10)));
    this.WatermarkButtonModel = annotationButtonModel16;
    ToolbarAnnotationButtonModel annotationButtonModel17 = new ToolbarAnnotationButtonModel(AnnotationMode.Link);
    annotationButtonModel17.Caption = Resources.LinkBtn;
    annotationButtonModel17.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/Link.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/Link.png"));
    annotationButtonModel17.ChildButtonModel = (ToolbarChildButtonModel) new ToolbarChildCheckableButtonModel()
    {
      ContextMenu = (IContextMenuModel) this.linkMenuItems
    };
    // ISSUE: method pointer
    annotationButtonModel17.Command = (ICommand) new RelayCommand<ToolbarAnnotationButtonModel>(new Action<ToolbarAnnotationButtonModel>((object) this, __methodptr(\u003CInitToolbarAnnotationButtonModel\u003Eg__LinkCommandFunc\u007C162_8)));
    ToolbarSettingModel toolbarSettingModel11 = new ToolbarSettingModel(AnnotationMode.Link);
    toolbarSettingModel11.Add(ToolbarSettingsHelper.CreateText(Resources.LinkToolbarTitle));
    toolbarSettingModel11.Add(ToolbarSettingsHelper.CreateImageExitEdit(new Action<ToolbarSettingItemModel>(this.DoToolbarSettingItemLinkExitCmd)));
    annotationButtonModel17.ToolbarSettingModel = toolbarSettingModel11;
    this.LinkButtonModel = annotationButtonModel17;
    this.allAnnotationButton = (System.Collections.Generic.IReadOnlyList<ToolbarAnnotationButtonModel>) new List<ToolbarAnnotationButtonModel>()
    {
      this.UnderlineButtonModel,
      this.StrikeButtonModel,
      this.HighlightButtonModel,
      this.LineButtonModel,
      this.InkButtonModel,
      this.SquareButtonModel,
      this.CircleButtonModel,
      this.HighlightAreaButtonModel,
      this.TextButtonModel,
      this.TextBoxButtonModel,
      this.NoteButtonModel,
      this.StampButtonModel,
      this.SignatureButtonModel,
      this.WatermarkButtonModel,
      this.shareButtonModel,
      this.LinkButtonModel
    };
    foreach (ToolbarAnnotationButtonModel annotationButtonModel18 in (IEnumerable<ToolbarAnnotationButtonModel>) this.allAnnotationButton)
      annotationButtonModel18.ContextMenuSelectionChanged += new EventHandler<SelectedAccessorSelectionChangedEventArgs>(this.ToolbarButton_ContextMenuSelectionChanged);
  }

  private void ToolbarButton_ContextMenuSelectionChanged(
    object sender,
    SelectedAccessorSelectionChangedEventArgs e)
  {
  }

  public void SetMenuItemValue()
  {
    this.ClearAdditionalMenuItem();
    this.TryBeginTransient();
    this.NotifyCheckedButtonToolbarSettingChanged();
  }

  private void TryBeginTransient()
  {
    PdfAnnotation selectedAnnotation = this.mainViewModel.SelectedAnnotation;
    if ((PdfWrapper) selectedAnnotation == (PdfWrapper) null)
      return;
    List<AnnotationMode> modes = AnnotationFactory.GetAnnotationModes(selectedAnnotation).ToList<AnnotationMode>();
    ToolbarSettingModel[] array = this.AllAnnotationButton.Where<ToolbarAnnotationButtonModel>((Func<ToolbarAnnotationButtonModel, bool>) (c => modes.Contains(c.Mode) && c.ToolbarSettingModel != null)).Select<ToolbarAnnotationButtonModel, ToolbarSettingModel>((Func<ToolbarAnnotationButtonModel, ToolbarSettingModel>) (c => c.ToolbarSettingModel)).OrderBy<ToolbarSettingModel, int>((Func<ToolbarSettingModel, int>) (c => modes.IndexOf(c.Id.AnnotationMode))).ToArray<ToolbarSettingModel>();
    if (array.Length == 0)
      return;
    PdfAnnotationPropertyAccessor propertyAccessor = new PdfAnnotationPropertyAccessor(selectedAnnotation, array[0].Id.AnnotationMode);
    foreach (ToolbarSettingItemModel settingItemModel in (Collection<ToolbarSettingItemModel>) array[0])
    {
      object result;
      if (ToolbarContextMenuHelper.TryParseMenuValue(array[0].Id.AnnotationMode, settingItemModel.Type, propertyAccessor.GetValue(settingItemModel.Type), out string _, out result))
        settingItemModel.BeginTransient(result);
      else if (settingItemModel is ToolbarSettingItemApplyToDefaultModel)
        settingItemModel.BeginTransient((object) null);
    }
  }

  private void TryEndAllTransient()
  {
    foreach (ToolbarAnnotationButtonModel annotationButtonModel in (IEnumerable<ToolbarAnnotationButtonModel>) this.AllAnnotationButton)
    {
      if (annotationButtonModel.ToolbarSettingModel != null)
      {
        foreach (ToolbarSettingItemModel settingItemModel in (Collection<ToolbarSettingItemModel>) annotationButtonModel.ToolbarSettingModel)
          settingItemModel.EndTransient();
      }
    }
  }

  private void ClearAdditionalMenuItem() => this.TryEndAllTransient();

  private void TrySetDefaultValueCore(TypedContextMenuItemModel menu)
  {
    if (!(menu.Parent is TypedContextMenuModel parent))
      return;
    ContextMenuItemModel defaultMenuItem = ToolbarContextMenuHelper.GetDefaultMenuItem(parent.Mode, menu);
    if (defaultMenuItem == null)
      return;
    defaultMenuItem.IsChecked = true;
  }

  private bool SetButtonProperty(
    ref ToolbarAnnotationButtonModel field,
    ToolbarAnnotationButtonModel value,
    [CallerMemberName] string propName = "")
  {
    ToolbarAnnotationButtonModel annotationButtonModel = field;
    if (!this.SetProperty<ToolbarAnnotationButtonModel>(ref field, value, propName))
      return false;
    if (annotationButtonModel != null)
      annotationButtonModel.PropertyChanged -= new PropertyChangedEventHandler(this.ToolbarButtonModel_PropertyChanged);
    if (value != null)
      value.PropertyChanged += new PropertyChangedEventHandler(this.ToolbarButtonModel_PropertyChanged);
    return true;
  }

  private void ToolbarButtonModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    this.NotifyCheckedButtonToolbarSettingChanged();
  }

  public async Task SaveToolbarSettingsConfigAsync()
  {
    await Task.WhenAll(this.AllAnnotationButton.Select<ToolbarAnnotationButtonModel, ToolbarSettingModel>((Func<ToolbarAnnotationButtonModel, ToolbarSettingModel>) (c => c.ToolbarSettingModel)).Where<ToolbarSettingModel>((Func<ToolbarSettingModel, bool>) (c => c != null)).Select<ToolbarSettingModel, Task>((Func<ToolbarSettingModel, Task>) (c => ((Func<Task>) (async () =>
    {
      try
      {
        await c.SaveConfigAsync().ConfigureAwait(false);
      }
      catch
      {
      }
    }))())));
  }

  public async Task LoadToolbarSettingsConfigAsync()
  {
    await Task.WhenAll(this.AllAnnotationButton.Select<ToolbarAnnotationButtonModel, ToolbarSettingModel>((Func<ToolbarAnnotationButtonModel, ToolbarSettingModel>) (c => c.ToolbarSettingModel)).Where<ToolbarSettingModel>((Func<ToolbarSettingModel, bool>) (c => c != null)).Select<ToolbarSettingModel, Task>((Func<ToolbarSettingModel, Task>) (c => ((Func<Task>) (async () =>
    {
      try
      {
        await c.LoadConfigAsync().ConfigureAwait(false);
      }
      catch
      {
      }
    }))())));
  }

  public async Task RemoveSignatureAnnotionAsync(PdfDocument doc)
  {
    if (doc == null || doc.Pages == null || doc.Pages.Count == 0)
      return;
    Action<PdfPage> action = this.RemoveImageStampFunc();
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(doc);
    (int num1, int num2) = pdfControl != null ? pdfControl.GetVisiblePageRange() : (-1, -1);
    if (action != null)
    {
      for (int index = 0; index < doc.Pages.Count; ++index)
      {
        int pageIndex = doc.Pages[index].PageIndex;
        IntPtr num3 = IntPtr.Zero;
        PdfPage page = (PdfPage) null;
        try
        {
          num3 = Pdfium.FPDF_LoadPage(doc.Handle, pageIndex);
          if (num3 != IntPtr.Zero)
          {
            page = PdfPage.FromHandle(doc, num3, pageIndex);
            action(page);
          }
        }
        finally
        {
          if (page != null && (page.PageIndex > num2 || page.PageIndex < num1))
            PageDisposeHelper.DisposePage(page);
          if (num3 != IntPtr.Zero)
            Pdfium.FPDF_ClosePage(num3);
        }
      }
    }
    if (pdfControl == null)
      return;
    await pdfControl.TryRedrawVisiblePageAsync();
  }

  private Action<PdfPage> RemoveImageStampFunc()
  {
    return (Action<PdfPage>) (p =>
    {
      PdfAnnotationCollection annots = p.Annots;
      (annots != null ? annots.OfType<PdfStampAnnotation>().ToList<PdfStampAnnotation>().FindAll((Predicate<PdfStampAnnotation>) (x => x.Subject == "Signature")) : (List<PdfStampAnnotation>) null)?.ForEach((Action<PdfStampAnnotation>) (a =>
      {
        PdfAnnotationExtensions.WaitForAnnotationGenerateAsync();
        a.DeleteAnnotation();
        this.mainViewModel.PageEditors?.NotifyPageAnnotationChanged(p.PageIndex);
        p.TryRedrawPageAsync();
      }));
    });
  }

  public async Task ConvertSignatureObj(PdfDocument doc, IProgress<double> progress)
  {
    if (doc == null || doc.Pages.Count == 0)
      return;
    progress?.Report(0.0);
    await Task.Run(CommomLib.Commom.TaskExceptionHelper.ExceptionBoundary((Func<Task>) (async () =>
    {
      for (int p = 0; p < doc.Pages.Count; ++p)
      {
        IntPtr pageHandle = Pdfium.FPDF_LoadPage(doc.Handle, doc.Pages[p].PageIndex);
        if (pageHandle != IntPtr.Zero)
        {
          try
          {
            PdfPage page = PdfPage.FromHandle(doc, pageHandle, doc.Pages[p].PageIndex);
            if (page.Annots != null)
            {
              PdfAnnotationCollection annots = page.Annots;
              if ((annots != null ? (annots.OfType<PdfStampAnnotation>().Where<PdfStampAnnotation>((Func<PdfStampAnnotation, bool>) (x => x.Subject == "Signature")).Count<PdfStampAnnotation>() == 0 ? 1 : 0) : 0) == 0)
                await this.AddEmbedSignatureObjAsync(page);
              else
                continue;
            }
            else
              continue;
          }
          finally
          {
            Pdfium.FPDF_ClosePage(pageHandle);
          }
        }
        progress?.Report(1.0 / (double) doc.Pages.Count * (double) (p + 1));
      }
      progress?.Report(1.0);
    }))).ConfigureAwait(false);
  }

  private async Task AddEmbedSignatureObjAsync(PdfPage page)
  {
    PdfAnnotationCollection annots = page.Annots;
    List<PdfStampAnnotation> imgStamps = annots != null ? annots.OfType<PdfStampAnnotation>().ToList<PdfStampAnnotation>().FindAll((Predicate<PdfStampAnnotation>) (x => x.Subject == "Signature")) : (List<PdfStampAnnotation>) null;
    for (int i = 0; i < imgStamps.Count; ++i)
    {
      PdfStampAnnotation annot = imgStamps[i];
      int num = await StampUtil.FlattenAnnotationAsync((PdfAnnotation) annot) ? 1 : 0;
      annot.DeleteAnnotation();
      annot = (PdfStampAnnotation) null;
    }
    imgStamps = (List<PdfStampAnnotation>) null;
  }

  public bool FindApplySignature(PdfDocument doc)
  {
    if (doc == null || doc.Pages.Count == 0)
      return false;
    for (int index = 0; index < doc.Pages.Count; ++index)
    {
      PdfPage page = doc.Pages[index];
      if (page.Annots != null)
      {
        PdfAnnotationCollection annots = page.Annots;
        if ((annots != null ? (annots.OfType<PdfStampAnnotation>().ToList<PdfStampAnnotation>().FindAll((Predicate<PdfStampAnnotation>) (x => x.Subject == "Signature")).Count == 0 ? 1 : 0) : 0) == 0)
          return true;
      }
    }
    return false;
  }

  public void NotifyPropertyChanged(string propName) => this.OnPropertyChanged(propName);

  private void SaveStamp(StampTextModel stampTextModel)
  {
    try
    {
      string localJsonPath = AnnotationToolbarViewModel.GetLocalJsonPath();
      stampTextModel.GroupId = Guid.NewGuid().ToString();
      stampTextModel.dateTime = DateTime.Now;
      List<StampTextModel> stampTextModelList = ToolbarContextMenuHelper.ReadStamp() ?? new List<StampTextModel>();
      stampTextModelList.Add(stampTextModel);
      using (FileStream fileStream = new FileStream(localJsonPath, FileMode.Create, FileAccess.ReadWrite))
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
        {
          string str = JsonConvert.SerializeObject((object) stampTextModelList, Formatting.Indented, new JsonSerializerSettings()
          {
            TypeNameHandling = TypeNameHandling.Auto
          });
          streamWriter.Write(str);
          streamWriter.Close();
        }
        fileStream.Close();
      }
    }
    catch
    {
    }
  }

  public static string GetLocalJsonPath()
  {
    string str = System.IO.Path.Combine(AppDataHelper.LocalCacheFolder, "Config");
    if (!Directory.Exists(str))
      Directory.CreateDirectory(str);
    return System.IO.Path.Combine(str, "Stamp.json");
  }
}
