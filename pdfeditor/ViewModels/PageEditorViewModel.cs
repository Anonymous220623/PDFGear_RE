// Decompiled with JetBrains decompiler
// Type: pdfeditor.ViewModels.PageEditorViewModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using CommomLib.Commom;
using CommomLib.Commom.HotKeys;
using CommomLib.Commom.MessageBoxHelper;
using CommomLib.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Microsoft.Win32;
using Patagames.Pdf;
using Patagames.Pdf.Net;
using pdfeditor.Controls;
using pdfeditor.Controls.Menus;
using pdfeditor.Controls.PageEditor;
using pdfeditor.Controls.PageHeaderFooters;
using pdfeditor.Controls.PageOperation;
using pdfeditor.Models.Menus;
using pdfeditor.Models.Thumbnails;
using pdfeditor.Properties;
using pdfeditor.Services;
using pdfeditor.Utils;
using pdfeditor.Views;
using PDFKit.Utils;
using PDFKit.Utils.PageHeaderFooters;
using PDFKit.Utils.XObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

#nullable enable
namespace pdfeditor.ViewModels;

public class PageEditorViewModel : ObservableObject
{
  private const float ImageLandscapeMargin = 90f;
  private const float ImagePortraitMargin = 72f;
  private readonly 
  #nullable disable
  MainViewModel mainViewModel;
  private PdfPageEditList pageEditListItemSource;
  private double pageEditerThumbnailScale = 0.5;
  private double pageEditorMinThumbnailScale = 0.3;
  private double pageEditorMaxThumbnailScale = 2.0;
  private ToolbarButtonModel headerFooterButtonModel;
  private ToolbarButtonModel insertPageButtonModel;
  private ToolbarButtonModel insertPageButtonModel2;
  private ToolbarButtonModel pageNumberButtonModel;
  private RelayCommand<PdfPageEditListModel> pageEditorRotateRightCmd;
  private RelayCommand<PdfPageEditListModel> pageEditorRotateLeftCmd;
  private AsyncRelayCommand<PdfPageEditListModel> pageEditorDeleteCmd;
  private AsyncRelayCommand<PdfPageEditListModel> siderbarDeleteCmd;
  private AsyncRelayCommand<PdfPageEditListModel> siderbarExtractCmd;
  private RelayCommand formBlankPage;
  private RelayCommand formPDF;
  private RelayCommand formWord;
  private RelayCommand formImage;
  private RelayCommand createBlankPage;
  private AsyncRelayCommand<PdfPageEditListModel> pageEditorExtractCmd;
  private RelayCommand cropPageCmd;
  private RelayCommand<PdfPageEditListModel> cropPageCmd2;
  private AsyncRelayCommand<PdfPageEditListModel> pageEditorMergeCmd;
  private AsyncRelayCommand<PdfPageEditListModel> pageEditorInsertBlankCmd;
  private AsyncRelayCommand<PdfPageEditListModel> pageEditorInsertFromPDFCmd;
  private AsyncRelayCommand<PdfPageEditListModel> pageEditorInsertFromWordCmd;
  private AsyncRelayCommand<PdfPageEditListModel> pageEditorInsertFromImgCmd;
  private AsyncRelayCommand pageEditorSplitCmd;
  private RelayCommand pageEditorZoomOutCmd;
  private RelayCommand pageEditorZoomInCmd;
  private RelayCommand allPageRotateRightCmd;
  private RelayCommand allPageRotateLeftCmd;

  public PageEditorViewModel(MainViewModel mainViewModel)
  {
    this.mainViewModel = mainViewModel;
    ToolbarButtonModel toolbarButtonModel1 = new ToolbarButtonModel();
    toolbarButtonModel1.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/HeaderFooter.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/HeaderFooter.png"));
    toolbarButtonModel1.Caption = Resources.MenuInsertHeaderFooterContent;
    ToolbarChildCheckableButtonModel checkableButtonModel1 = new ToolbarChildCheckableButtonModel();
    checkableButtonModel1.IsDropDownIconVisible = true;
    ContextMenuModel contextMenuModel1 = new ContextMenuModel();
    contextMenuModel1.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "Add",
      Caption = Resources.MenuInsertAddContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/HFAdd.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/HFAdd.png")),
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.HeaderFooterContextMenuItemInvoke)),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_AddPageHeaderAndFooter"
    });
    contextMenuModel1.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "Update",
      Caption = Resources.MenuInsertEditContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/HFUpdate.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/HFUpdate.png")),
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.HeaderFooterContextMenuItemInvoke)),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_UpdatePageHeaderAndFooter"
    });
    contextMenuModel1.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "Delete",
      Caption = Resources.MenuInsertDelContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/HFDelete.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/HFDelete.png")),
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.HeaderFooterContextMenuItemInvoke)),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_DeletePageHeaderAndFooter"
    });
    checkableButtonModel1.ContextMenu = (IContextMenuModel) contextMenuModel1;
    toolbarButtonModel1.ChildButtonModel = (ToolbarChildButtonModel) checkableButtonModel1;
    toolbarButtonModel1.Command = (ICommand) new RelayCommand<ToolbarButtonModel>(new Action<ToolbarButtonModel>(this.OpenContextMenuCommandFunc));
    this.headerFooterButtonModel = toolbarButtonModel1;
    ToolbarButtonModel toolbarButtonModel2 = new ToolbarButtonModel();
    toolbarButtonModel2.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/PageNumber.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/PageNumber.png"));
    toolbarButtonModel2.Caption = Resources.MenuInsertPageNumberContent;
    ToolbarChildCheckableButtonModel checkableButtonModel2 = new ToolbarChildCheckableButtonModel();
    checkableButtonModel2.IsDropDownIconVisible = true;
    ContextMenuModel contextMenuModel2 = new ContextMenuModel();
    contextMenuModel2.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "Add",
      Caption = Resources.MenuInsertAddContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/HFAdd.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/HFAdd.png")),
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.PageNumberContextMenuItemInvoke)),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_AddPageNumber"
    });
    contextMenuModel2.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "Update",
      Caption = Resources.MenuInsertEditContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/HFUpdate.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/HFUpdate.png")),
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.PageNumberContextMenuItemInvoke)),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_UpdatePageNumber"
    });
    contextMenuModel2.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "Delete",
      Caption = Resources.MenuInsertDelContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/HFDelete.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/HFDelete.png")),
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.PageNumberContextMenuItemInvoke)),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_DeletePageNumber"
    });
    checkableButtonModel2.ContextMenu = (IContextMenuModel) contextMenuModel2;
    toolbarButtonModel2.ChildButtonModel = (ToolbarChildButtonModel) checkableButtonModel2;
    toolbarButtonModel2.Command = (ICommand) new RelayCommand<ToolbarButtonModel>(new Action<ToolbarButtonModel>(this.OpenContextMenuCommandFunc));
    this.pageNumberButtonModel = toolbarButtonModel2;
    ToolbarButtonModel toolbarButtonModel3 = new ToolbarButtonModel();
    toolbarButtonModel3.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/Insert.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/Insert.png"));
    toolbarButtonModel3.Caption = Resources.MenuPageInsertContent;
    ToolbarChildCheckableButtonModel checkableButtonModel3 = new ToolbarChildCheckableButtonModel();
    checkableButtonModel3.IsDropDownIconVisible = true;
    ContextMenuModel contextMenuModel3 = new ContextMenuModel();
    contextMenuModel3.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "BlankPage",
      Caption = Resources.MenuPageSubInsertBlankPage,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/BlankPage.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/BlankPage.png")),
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.InsertPageContextMenuItemInvoke)),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_InsertBlankPage",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    });
    contextMenuModel3.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "FromPDF",
      Caption = Resources.MenuPageSubInsertFromPDF,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/FromPDF.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/FromPDF.png")),
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.InsertPageContextMenuItemInvoke)),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_InsertPDF",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    });
    contextMenuModel3.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "FromWord",
      Caption = Resources.MenuPageSubInsertFromWord,
      Icon = (ImageSource) new BitmapImage(new Uri("/Style/Resources/PageEditor/FromWord.png", UriKind.Relative)),
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.InsertPageContextMenuItemInvoke)),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_InsertWord",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    });
    contextMenuModel3.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "FromImage",
      Caption = Resources.MenuPageSubInsertFromImage,
      Icon = (ImageSource) new BitmapImage(new Uri("/Style/Resources/PageEditor/FromImage.png", UriKind.Relative)),
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.InsertPageContextMenuItemInvoke)),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_InsertImage",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    });
    checkableButtonModel3.ContextMenu = (IContextMenuModel) contextMenuModel3;
    toolbarButtonModel3.ChildButtonModel = (ToolbarChildButtonModel) checkableButtonModel3;
    toolbarButtonModel3.Command = (ICommand) new RelayCommand<ToolbarButtonModel>(new Action<ToolbarButtonModel>(this.OpenContextMenuCommandFunc));
    this.insertPageButtonModel = toolbarButtonModel3;
    ToolbarButtonModel toolbarButtonModel4 = new ToolbarButtonModel();
    toolbarButtonModel4.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/Insert.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/Insert.png"));
    toolbarButtonModel4.Caption = Resources.MenuPageInsertContent;
    ToolbarChildCheckableButtonModel checkableButtonModel4 = new ToolbarChildCheckableButtonModel();
    checkableButtonModel4.IsDropDownIconVisible = true;
    ContextMenuModel contextMenuModel4 = new ContextMenuModel();
    contextMenuModel4.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "BlankPage",
      Caption = Resources.MenuPageSubInsertBlankPage,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/BlankPage.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/BlankPage.png")),
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.InsertPageContextMenuItemInvoke2)),
      IsCheckable = false
    });
    contextMenuModel4.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "FromPDF",
      Caption = Resources.MenuPageSubInsertFromPDF,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/FromPDF.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/FromPDF.png")),
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.InsertPageContextMenuItemInvoke2)),
      IsCheckable = false
    });
    contextMenuModel4.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "FromWord",
      Caption = Resources.MenuPageSubInsertFromWord,
      Icon = (ImageSource) new BitmapImage(new Uri("/Style/Resources/PageEditor/FromWord.png", UriKind.Relative)),
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.InsertPageContextMenuItemInvoke2)),
      IsCheckable = false
    });
    contextMenuModel4.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "FromImage",
      Caption = Resources.MenuPageSubInsertFromImage,
      Icon = (ImageSource) new BitmapImage(new Uri("/Style/Resources/PageEditor/FromImage.png", UriKind.Relative)),
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.InsertPageContextMenuItemInvoke2)),
      IsCheckable = false
    });
    checkableButtonModel4.ContextMenu = (IContextMenuModel) contextMenuModel4;
    toolbarButtonModel4.ChildButtonModel = (ToolbarChildButtonModel) checkableButtonModel4;
    toolbarButtonModel4.Command = (ICommand) new RelayCommand<ToolbarButtonModel>(new Action<ToolbarButtonModel>(this.OpenContextMenuCommandFunc));
    this.insertPageButtonModel2 = toolbarButtonModel4;
  }

  public ToolbarButtonModel HeaderFooterButtonModel => this.headerFooterButtonModel;

  public ToolbarButtonModel PageNumberButtonModel => this.pageNumberButtonModel;

  public ToolbarButtonModel InsertPageButtonModel => this.insertPageButtonModel;

  public ToolbarButtonModel InsertPageButtonModel2 => this.insertPageButtonModel2;

  public PdfPageEditList PageEditListItemSource
  {
    get => this.pageEditListItemSource;
    set
    {
      this.SetProperty<PdfPageEditList>(ref this.pageEditListItemSource, value, nameof (PageEditListItemSource));
    }
  }

  public double PageEditorMinThumbnailScale
  {
    get => this.pageEditorMinThumbnailScale;
    set
    {
      if (!this.SetProperty<double>(ref this.pageEditorMinThumbnailScale, value, nameof (PageEditorMinThumbnailScale)) || this.PageEditerThumbnailScale >= value)
        return;
      this.PageEditerThumbnailScale = value;
    }
  }

  public double PageEditorMaxThumbnailScale
  {
    get => this.pageEditorMaxThumbnailScale;
    set
    {
      if (!this.SetProperty<double>(ref this.pageEditorMaxThumbnailScale, value, nameof (PageEditorMaxThumbnailScale)) || this.PageEditerThumbnailScale <= value)
        return;
      this.PageEditerThumbnailScale = value;
    }
  }

  public double PageEditerThumbnailScale
  {
    get => this.pageEditerThumbnailScale;
    set
    {
      double newValue = Math.Max(this.PageEditorMinThumbnailScale, Math.Min(this.PageEditorMaxThumbnailScale, value));
      if (!this.SetProperty<double>(ref this.pageEditerThumbnailScale, newValue, nameof (PageEditerThumbnailScale)))
        return;
      PdfPageEditList editListItemSource = this.PageEditListItemSource;
      if (editListItemSource == null)
        return;
      editListItemSource.Scale = newValue;
    }
  }

  public RelayCommand AllPageRotateRightCmd
  {
    get
    {
      return this.allPageRotateRightCmd ?? (this.allPageRotateRightCmd = new RelayCommand((Action) (() => this.RotateAllPageCoreCmd(true))));
    }
  }

  public RelayCommand AllPageRotateLeftCmd
  {
    get
    {
      return this.allPageRotateLeftCmd ?? (this.allPageRotateLeftCmd = new RelayCommand((Action) (() => this.RotateAllPageCoreCmd(false))));
    }
  }

  public RelayCommand<PdfPageEditListModel> PageEditorRotateRightCmd
  {
    get
    {
      return this.pageEditorRotateRightCmd ?? (this.pageEditorRotateRightCmd = new RelayCommand<PdfPageEditListModel>((Action<PdfPageEditListModel>) (model =>
      {
        CommomLib.Commom.GAManager.SendEvent("PageView", nameof (PageEditorRotateRightCmd), "Count", 1L);
        this.RotatePageCoreCmd(model, true);
      })));
    }
  }

  public RelayCommand<PdfPageEditListModel> PageEditorRotateLeftCmd
  {
    get
    {
      return this.pageEditorRotateLeftCmd ?? (this.pageEditorRotateLeftCmd = new RelayCommand<PdfPageEditListModel>((Action<PdfPageEditListModel>) (model =>
      {
        CommomLib.Commom.GAManager.SendEvent("PageView", nameof (PageEditorRotateLeftCmd), "Count", 1L);
        this.RotatePageCoreCmd(model, false);
      })));
    }
  }

  public RelayCommand CropPageCmd
  {
    get
    {
      return this.cropPageCmd ?? (this.cropPageCmd = new RelayCommand((Action) (() =>
      {
        CommomLib.Commom.GAManager.SendEvent("PageView", nameof (CropPageCmd), "Page", 1L);
        if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
          return;
        PdfPageEditList editListItemSource = this.PageEditListItemSource;
        PdfPageEditListModel[] pageEditListModelArray1;
        if (editListItemSource == null)
        {
          pageEditListModelArray1 = (PdfPageEditListModel[]) null;
        }
        else
        {
          IReadOnlyCollection<PdfPageEditListModel> selectedItems = editListItemSource.SelectedItems;
          pageEditListModelArray1 = selectedItems != null ? selectedItems.ToArray<PdfPageEditListModel>() : (PdfPageEditListModel[]) null;
        }
        PdfPageEditListModel[] pageEditListModelArray2 = pageEditListModelArray1;
        if (pageEditListModelArray2 != null && pageEditListModelArray2.Length > 1)
        {
          int num1 = (int) ModernMessageBox.Show(Resources.MainCropPageSelectedMulPageNote, UtilManager.GetProductName());
        }
        else if (pageEditListModelArray2 != null && pageEditListModelArray2.Length == 0)
        {
          int num2 = (int) ModernMessageBox.Show(Resources.MainCropPageNotSelectedPageNote, UtilManager.GetProductName());
        }
        else
        {
          this.mainViewModel.CurrnetPageIndex = pageEditListModelArray2[0].DisplayPageIndex;
          MainView mainView = Application.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
          ToolbarRadioButton cropPageBtn = mainView.cropPageBtn;
          cropPageBtn.IsChecked = new bool?(true);
          mainView.Menus.SelectedIndex = mainView.Menus.Items.IndexOf((object) this.mainViewModel.Menus.MainMenus.FirstOrDefault<MainMenuGroup>((Func<MainMenuGroup, bool>) (x => x.Tag == "View")));
          mainView.ToolbarScreenShotButton_Click((object) cropPageBtn, (RoutedEventArgs) null);
        }
      })));
    }
  }

  public RelayCommand<PdfPageEditListModel> CropPageCmd2
  {
    get
    {
      return this.cropPageCmd2 ?? (this.cropPageCmd2 = new RelayCommand<PdfPageEditListModel>((Action<PdfPageEditListModel>) (model =>
      {
        if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null || model == null)
          return;
        PdfPageEditListModel[] pageEditListModelArray = new PdfPageEditListModel[1]
        {
          model
        };
        if (pageEditListModelArray != null && pageEditListModelArray.Length > 1)
        {
          int num1 = (int) ModernMessageBox.Show(Resources.MainCropPageSelectedMulPageNote, UtilManager.GetProductName());
        }
        else if (pageEditListModelArray != null && pageEditListModelArray.Length == 0)
        {
          int num2 = (int) ModernMessageBox.Show(Resources.MainCropPageNotSelectedPageNote, UtilManager.GetProductName());
        }
        else
        {
          this.mainViewModel.CurrnetPageIndex = pageEditListModelArray[0].DisplayPageIndex;
          MainView mainView = Application.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
          ToolbarRadioButton cropPageBtn = mainView.cropPageBtn;
          cropPageBtn.IsChecked = new bool?(true);
          mainView.Menus.SelectedIndex = mainView.Menus.Items.IndexOf((object) this.mainViewModel.Menus.MainMenus.FirstOrDefault<MainMenuGroup>((Func<MainMenuGroup, bool>) (x => x.Tag == "View")));
          mainView.ToolbarScreenShotButton_Click((object) cropPageBtn, (RoutedEventArgs) null);
        }
      })));
    }
  }

  private void RotatePagesCore(IEnumerable<int> pageNumbers, bool rotateRight)
  {
    if (pageNumbers == null || this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
      return;
    int[] nums = pageNumbers.ToArray<int>();
    if (nums.Length == 0)
      return;
    RotateCore(this.mainViewModel.Document, nums, rotateRight);
    this.mainViewModel.OperationManager.AddOperationAsync((Action<PdfDocument>) (doc =>
    {
      RotateCore(this.mainViewModel.Document, nums, !rotateRight);
      PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
      if (pdfControl == null || pdfControl.Document == null)
        return;
      pdfControl.UpdateDocLayout();
    }), (Action<PdfDocument>) (doc =>
    {
      RotateCore(this.mainViewModel.Document, nums, rotateRight);
      PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
      if (pdfControl == null || pdfControl.Document == null)
        return;
      pdfControl.UpdateDocLayout();
    }));
    PDFKit.PdfControl pdfControl1 = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
    if (pdfControl1 == null || pdfControl1.Document == null)
      return;
    pdfControl1.UpdateDocLayout();

    static void RotateCore(PdfDocument _doc, int[] _nums, bool _rotateRight)
    {
      ProgressUtils.ShowProgressBar((Func<ProgressUtils.ProgressAction, Task>) (async p =>
      {
        Progress<double> progress = new Progress<double>((Action<double>) (v => p.Report(v)));
        try
        {
          await MainViewModel.RotatePageCoreAsync(_doc, (IEnumerable<int>) _nums, _rotateRight, (IProgress<double>) progress);
        }
        catch
        {
        }
        p.Complete();
      }), (string) null, (object) null, false, Application.Current.MainWindow);
    }
  }

  private void RotateAllPageCoreCmd(bool rotateRight)
  {
    if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
      return;
    this.RotatePagesCore(Enumerable.Range(0, this.mainViewModel.Document.Pages.Count), rotateRight);
  }

  private void RotatePageCoreCmd(PdfPageEditListModel model, bool rotateRight)
  {
    if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
      return;
    PdfPageEditListModel[] source;
    if (model != null)
    {
      source = new PdfPageEditListModel[1]{ model };
    }
    else
    {
      PdfPageEditList editListItemSource = this.PageEditListItemSource;
      PdfPageEditListModel[] pageEditListModelArray;
      if (editListItemSource == null)
      {
        pageEditListModelArray = (PdfPageEditListModel[]) null;
      }
      else
      {
        IReadOnlyCollection<PdfPageEditListModel> selectedItems = editListItemSource.SelectedItems;
        pageEditListModelArray = selectedItems != null ? selectedItems.ToArray<PdfPageEditListModel>() : (PdfPageEditListModel[]) null;
      }
      source = pageEditListModelArray;
    }
    if (source == null || source.Length == 0)
      return;
    this.RotatePagesCore(((IEnumerable<PdfPageEditListModel>) source).Select<PdfPageEditListModel, int>((Func<PdfPageEditListModel, int>) (c => c.PageIndex)), rotateRight);
  }

  public AsyncRelayCommand<PdfPageEditListModel> SiderbarExtractCmd
  {
    get
    {
      return this.siderbarExtractCmd ?? (this.siderbarExtractCmd = new AsyncRelayCommand<PdfPageEditListModel>((Func<PdfPageEditListModel, Task>) (async model =>
      {
        CommomLib.Commom.GAManager.SendEvent("PageView", nameof (SiderbarExtractCmd), "Siderbar", 1L);
        if (!IAPUtils.IsPaidUserWrapper())
        {
          IAPUtils.ShowPurchaseWindows("extractpages", ".pdf");
        }
        else
        {
          PdfPageEditListModel[] source;
          if (model != null)
            source = new PdfPageEditListModel[1]{ model };
          else
            source = new PdfPageEditListModel[1]
            {
              new PdfPageEditListModel(this.mainViewModel.Document, this.mainViewModel.SelectedPageIndex)
            };
          if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
            return;
          string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
          int[] array = ((IEnumerable<PdfPageEditListModel>) source).Select<PdfPageEditListModel, int>((Func<PdfPageEditListModel, int>) (c => c.PageIndex)).ToArray<int>();
          string range = ((IEnumerable<int>) array).ConvertToRange(out int[] _);
          if (source != null && source.Length != 0)
          {
            ExtractPagesWindow extractPagesWindow = new ExtractPagesWindow(array, range, folderPath, this.mainViewModel);
            extractPagesWindow.ShowDialog();
            extractPagesWindow.Owner = App.Current.MainWindow;
            extractPagesWindow.WindowStartupLocation = extractPagesWindow.Owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
          }
          else
          {
            ExtractPagesWindow extractPagesWindow = new ExtractPagesWindow(folderPath, this.mainViewModel);
            extractPagesWindow.ShowDialog();
            extractPagesWindow.Owner = App.Current.MainWindow;
            extractPagesWindow.WindowStartupLocation = extractPagesWindow.Owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
          }
        }
      })));
    }
  }

  public AsyncRelayCommand<PdfPageEditListModel> SiderbarDeleteCmd
  {
    get
    {
      return this.siderbarDeleteCmd ?? (this.siderbarDeleteCmd = new AsyncRelayCommand<PdfPageEditListModel>((Func<PdfPageEditListModel, Task>) (async model =>
      {
        CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorDeleteCmd", "Siderbar", 1L);
        if (!IAPUtils.IsPaidUserWrapper())
        {
          IAPUtils.ShowPurchaseWindows("deletepages", ".pdf");
        }
        else
        {
          if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
            return;
          this.mainViewModel.ViewJumpManager.ClearStack();
          PdfPageEditListModel[] source;
          if (model != null)
            source = new PdfPageEditListModel[1]{ model };
          else
            source = new PdfPageEditListModel[1]
            {
              new PdfPageEditListModel(this.mainViewModel.Document, this.mainViewModel.SelectedPageIndex)
            };
          if (source != null && source.Length != 0)
          {
            DeletePagesWindow deletePagesWindow = new DeletePagesWindow(this.mainViewModel, ((IEnumerable<PdfPageEditListModel>) source).Select<PdfPageEditListModel, int>((Func<PdfPageEditListModel, int>) (c => c.PageIndex)).ToArray<int>());
            deletePagesWindow.ShowDialog();
            deletePagesWindow.Owner = App.Current.MainWindow;
            deletePagesWindow.WindowStartupLocation = deletePagesWindow.Owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
          }
          else
          {
            DeletePagesWindow deletePagesWindow = new DeletePagesWindow(this.mainViewModel);
            deletePagesWindow.ShowDialog();
            deletePagesWindow.Owner = App.Current.MainWindow;
            deletePagesWindow.WindowStartupLocation = deletePagesWindow.Owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
          }
        }
      }), (Predicate<PdfPageEditListModel>) (_ => !this.PageEditorDeleteCmd.IsRunning)));
    }
  }

  public RelayCommand FormBlankPage
  {
    get
    {
      return this.formBlankPage ?? (this.formBlankPage = new RelayCommand((Action) (() => this.DoInsertBlankPage(IsSiderbar: true))));
    }
  }

  public RelayCommand CreateBlankPage
  {
    get
    {
      return this.createBlankPage ?? (this.createBlankPage = new RelayCommand((Action) (() => this.DoCreateBlankPageAsync(IsSiderbar: true))));
    }
  }

  public RelayCommand FormPDF
  {
    get
    {
      return this.formPDF ?? (this.formPDF = new RelayCommand((Action) (() => this.DoInsertPDF(IsSiderbar: true))));
    }
  }

  public RelayCommand FormWord
  {
    get
    {
      return this.formWord ?? (this.formWord = new RelayCommand((Action) (() => this.DoInsertWord(IsSiderbar: true))));
    }
  }

  public RelayCommand FormImage
  {
    get
    {
      return this.formImage ?? (this.formImage = new RelayCommand((Action) (() => this.DoInsertImage(IsSiderbar: true))));
    }
  }

  public AsyncRelayCommand<PdfPageEditListModel> PageEditorDeleteCmd
  {
    get
    {
      return this.pageEditorDeleteCmd ?? (this.pageEditorDeleteCmd = new AsyncRelayCommand<PdfPageEditListModel>((Func<PdfPageEditListModel, Task>) (async model =>
      {
        this.mainViewModel.ExitTransientMode();
        CommomLib.Commom.GAManager.SendEvent("PageView", nameof (PageEditorDeleteCmd), "PageToolbar", 1L);
        if (!IAPUtils.IsPaidUserWrapper())
        {
          IAPUtils.ShowPurchaseWindows("deletepages", ".pdf");
        }
        else
        {
          if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
            return;
          this.mainViewModel.ViewJumpManager.ClearStack();
          PdfPageEditListModel[] source;
          if (model != null)
          {
            source = new PdfPageEditListModel[1]{ model };
          }
          else
          {
            PdfPageEditList editListItemSource = this.PageEditListItemSource;
            PdfPageEditListModel[] pageEditListModelArray;
            if (editListItemSource == null)
            {
              pageEditListModelArray = (PdfPageEditListModel[]) null;
            }
            else
            {
              IReadOnlyCollection<PdfPageEditListModel> selectedItems = editListItemSource.SelectedItems;
              pageEditListModelArray = selectedItems != null ? selectedItems.ToArray<PdfPageEditListModel>() : (PdfPageEditListModel[]) null;
            }
            source = pageEditListModelArray;
          }
          if (source != null && source.Length != 0)
          {
            DeletePagesWindow deletePagesWindow = new DeletePagesWindow(this.mainViewModel, ((IEnumerable<PdfPageEditListModel>) source).Select<PdfPageEditListModel, int>((Func<PdfPageEditListModel, int>) (c => c.PageIndex)).ToArray<int>());
            deletePagesWindow.ShowDialog();
            deletePagesWindow.Owner = App.Current.MainWindow;
            deletePagesWindow.WindowStartupLocation = deletePagesWindow.Owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
          }
          else
          {
            DeletePagesWindow deletePagesWindow = new DeletePagesWindow(this.mainViewModel);
            deletePagesWindow.ShowDialog();
            deletePagesWindow.Owner = App.Current.MainWindow;
            deletePagesWindow.WindowStartupLocation = deletePagesWindow.Owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
          }
        }
      }), (Predicate<PdfPageEditListModel>) (_ => !this.PageEditorDeleteCmd.IsRunning)));
    }
  }

  public AsyncRelayCommand<PdfPageEditListModel> PageEditorExtractCmd
  {
    get
    {
      return this.pageEditorExtractCmd ?? (this.pageEditorExtractCmd = new AsyncRelayCommand<PdfPageEditListModel>((Func<PdfPageEditListModel, Task>) (async model =>
      {
        CommomLib.Commom.GAManager.SendEvent("PageView", nameof (PageEditorExtractCmd), "PageToolbar", 1L);
        if (!IAPUtils.IsPaidUserWrapper())
        {
          IAPUtils.ShowPurchaseWindows("extractpages", ".pdf");
        }
        else
        {
          PdfPageEditListModel[] source;
          if (model != null)
          {
            source = new PdfPageEditListModel[1]{ model };
          }
          else
          {
            PdfPageEditList editListItemSource = this.PageEditListItemSource;
            PdfPageEditListModel[] pageEditListModelArray;
            if (editListItemSource == null)
            {
              pageEditListModelArray = (PdfPageEditListModel[]) null;
            }
            else
            {
              IReadOnlyCollection<PdfPageEditListModel> selectedItems = editListItemSource.SelectedItems;
              pageEditListModelArray = selectedItems != null ? selectedItems.ToArray<PdfPageEditListModel>() : (PdfPageEditListModel[]) null;
            }
            source = pageEditListModelArray;
          }
          if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
            return;
          string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
          int[] array = ((IEnumerable<PdfPageEditListModel>) source).Select<PdfPageEditListModel, int>((Func<PdfPageEditListModel, int>) (c => c.PageIndex)).ToArray<int>();
          string range = ((IEnumerable<int>) array).ConvertToRange(out int[] _);
          if (source != null && source != null && source.Length != 0)
          {
            ExtractPagesWindow extractPagesWindow = new ExtractPagesWindow(array, range, folderPath, this.mainViewModel);
            extractPagesWindow.ShowDialog();
            extractPagesWindow.Owner = App.Current.MainWindow;
            extractPagesWindow.WindowStartupLocation = extractPagesWindow.Owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
          }
          else
          {
            ExtractPagesWindow extractPagesWindow = new ExtractPagesWindow(folderPath, this.mainViewModel);
            extractPagesWindow.ShowDialog();
            extractPagesWindow.Owner = App.Current.MainWindow;
            extractPagesWindow.WindowStartupLocation = extractPagesWindow.Owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
          }
        }
      })));
    }
  }

  public AsyncRelayCommand<PdfPageEditListModel> PageEditorMergeCmd
  {
    get
    {
      return this.pageEditorMergeCmd ?? (this.pageEditorMergeCmd = new AsyncRelayCommand<PdfPageEditListModel>((Func<PdfPageEditListModel, Task>) (async model =>
      {
        if (this.mainViewModel.CanSave)
        {
          int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeSendMsg, UtilManager.GetProductName());
        }
        else
        {
          this.mainViewModel.ViewJumpManager.ClearStack();
          CommomLib.Commom.GAManager.SendEvent("PageView", nameof (PageEditorMergeCmd), "Count", 1L);
          AppManager.OpenPDFConverterToPdf(ConvToPDFType.MergePDF, this.mainViewModel.Password, new string[1]
          {
            this.mainViewModel.DocumentWrapper?.DocumentPath
          });
        }
      })));
    }
  }

  public AsyncRelayCommand<PdfPageEditListModel> PageEditorInsertBlankCmd
  {
    get
    {
      return this.pageEditorInsertBlankCmd ?? (this.pageEditorInsertBlankCmd = new AsyncRelayCommand<PdfPageEditListModel>((Func<PdfPageEditListModel, Task>) (async model => this.DoInsertBlankPage(model))));
    }
  }

  public AsyncRelayCommand<PdfPageEditListModel> PageEditorInsertFromPDFCmd
  {
    get
    {
      return this.pageEditorInsertFromPDFCmd ?? (this.pageEditorInsertFromPDFCmd = new AsyncRelayCommand<PdfPageEditListModel>((Func<PdfPageEditListModel, Task>) (async model => this.DoInsertPDF(model))));
    }
  }

  public AsyncRelayCommand<PdfPageEditListModel> PageEditorInsertFromWordCmd
  {
    get
    {
      return this.pageEditorInsertFromWordCmd ?? (this.pageEditorInsertFromWordCmd = new AsyncRelayCommand<PdfPageEditListModel>((Func<PdfPageEditListModel, Task>) (async model => this.DoInsertWord(model))));
    }
  }

  public AsyncRelayCommand<PdfPageEditListModel> PageEditorInsertFromImageCmd
  {
    get
    {
      return this.pageEditorInsertFromImgCmd ?? (this.pageEditorInsertFromImgCmd = new AsyncRelayCommand<PdfPageEditListModel>((Func<PdfPageEditListModel, Task>) (async model => this.DoInsertImage(model))));
    }
  }

  private (int startPage, int endPage) InsertPagesIntoDocument(
    PdfDocument doc,
    string sourceFilePath,
    int insertIndex,
    bool isBefore)
  {
    using (FileStream fileStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
    {
      using (PdfDocument pdfDocument = PdfDocument.Load((Stream) fileStream))
      {
        PageDisposeHelper.TryFixResource(pdfDocument, 0, pdfDocument.Pages.Count - 1);
        int index = isBefore ? insertIndex : insertIndex + 1;
        doc.Pages.ImportPages(pdfDocument, $"1-{pdfDocument.Pages.Count}", index);
        return (index, index + pdfDocument.Pages.Count - 1);
      }
    }
  }

  public AsyncRelayCommand PageEditorSplitCmd
  {
    get
    {
      return this.pageEditorSplitCmd ?? (this.pageEditorSplitCmd = new AsyncRelayCommand((Func<Task>) (async () =>
      {
        if (this.mainViewModel.CanSave)
        {
          int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeSendMsg, UtilManager.GetProductName());
        }
        else
        {
          this.mainViewModel.ViewJumpManager.ClearStack();
          CommomLib.Commom.GAManager.SendEvent("PageView", nameof (PageEditorSplitCmd), "Count", 1L);
          AppManager.OpenPDFConverterToPdf(ConvToPDFType.SplitPDF, this.mainViewModel.Password, new string[1]
          {
            this.mainViewModel.DocumentWrapper?.DocumentPath
          });
        }
      })));
    }
  }

  public RelayCommand PageEditorZoomOutCmd
  {
    get
    {
      return this.pageEditorZoomOutCmd ?? (this.pageEditorZoomOutCmd = new RelayCommand((Action) (() =>
      {
        if (this.mainViewModel.Document == null)
          return;
        this.PageEditerThumbnailScale -= 0.1;
      })));
    }
  }

  public RelayCommand PageEditorZoomInCmd
  {
    get
    {
      return this.pageEditorZoomInCmd ?? (this.pageEditorZoomInCmd = new RelayCommand((Action) (() =>
      {
        if (this.mainViewModel.Document == null)
          return;
        this.PageEditerThumbnailScale += 0.1;
      })));
    }
  }

  public void NotifyPageAnnotationChanged(int pageIndex)
  {
    this.mainViewModel.PageCommetSource?.NotifyPageAnnotationChanged(pageIndex);
  }

  private void OpenContextMenuCommandFunc(ToolbarButtonModel model)
  {
    DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (async () =>
    {
      await Task.Delay(50);
      if (!(model.ChildButtonModel is ToolbarChildCheckableButtonModel childButtonModel2) || !childButtonModel2.IsEnabled)
        return;
      childButtonModel2.IsChecked = true;
    }));
  }

  private bool GetHeaderFooterSettings(out System.Collections.Generic.IReadOnlyList<HeaderFooterData> hfData)
  {
    hfData = (System.Collections.Generic.IReadOnlyList<HeaderFooterData>) null;
    System.Collections.Generic.IReadOnlyList<HeaderFooterData> _hfData = hfData;
    int num = ProgressUtils.ShowProgressBar((Func<ProgressUtils.ProgressAction, Task>) (async c =>
    {
      Progress<double> progress = new Progress<double>();
      progress.ProgressChanged += (EventHandler<double>) ((s, a) => c.Report(a));
      _hfData = await PageHeaderFooterUtils.GetHeaderFooterSettingsAsync(this.mainViewModel.Document, (IProgress<double>) progress, c.CancellationToken);
      c.Complete();
    }), (string) null, (object) Resources.WinPageLoadingHeaderorFooterContent, true, (Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>()) ? 1 : 0;
    hfData = _hfData;
    return num != 0;
  }

  private void HeaderFooterContextMenuItemInvoke(ContextMenuItemModel model)
  {
    this.mainViewModel.ExitTransientMode();
    this.mainViewModel.AnnotationMode = AnnotationMode.None;
    this.mainViewModel.ReleaseViewerFocusAsync(true);
    switch (model?.Name)
    {
      case "Add":
        this.AddHeaderFooter();
        break;
      case "Update":
        this.UpdateHeaderFooter();
        break;
      case "Delete":
        this.DeleteHeaderFooter();
        break;
    }
  }

  public void AddHeaderFooter2()
  {
    this.mainViewModel.ExitTransientMode();
    this.mainViewModel.AnnotationMode = AnnotationMode.None;
    this.mainViewModel.ReleaseViewerFocusAsync(true);
    this.AddHeaderFooter();
  }

  private void AddHeaderFooter()
  {
    CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorAddHeaderFooterCmd", "Count", 1L);
    this.AddHeaderFooterOrPageNumber(false);
  }

  private void UpdateHeaderFooter()
  {
    CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorUpdateHeaderFooterCmd", "Count", 1L);
    this.UpdateHeaderFooterOrPageNumber(false);
  }

  private void DeleteHeaderFooter()
  {
    CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorDeleteHeaderFooterCmd", "Count", 1L);
    this.DeleteHeaderFooterOrPageNumber(false);
  }

  private void PageNumberContextMenuItemInvoke(ContextMenuItemModel model)
  {
    this.mainViewModel.ExitTransientMode();
    this.mainViewModel.AnnotationMode = AnnotationMode.None;
    this.mainViewModel.ReleaseViewerFocusAsync(true);
    switch (model?.Name)
    {
      case "Add":
        this.AddPageNumber();
        break;
      case "Update":
        this.UpdatePageNumber();
        break;
      case "Delete":
        this.DeletePageNumber();
        break;
    }
  }

  private async void InsertPageContextMenuItemInvoke(ContextMenuItemModel model)
  {
    this.mainViewModel.ViewJumpManager.ClearStack();
    switch (model?.Name)
    {
      case "BlankPage":
        await this.PageEditorInsertBlankCmd.ExecuteAsync((PdfPageEditListModel) null);
        break;
      case "FromPDF":
        await this.PageEditorInsertFromPDFCmd.ExecuteAsync((PdfPageEditListModel) null);
        break;
      case "FromWord":
        await this.PageEditorInsertFromWordCmd.ExecuteAsync((PdfPageEditListModel) null);
        break;
      case "FromImage":
        await this.PageEditorInsertFromImageCmd.ExecuteAsync((PdfPageEditListModel) null);
        break;
    }
  }

  private void InsertPageContextMenuItemInvoke2(ContextMenuItemModel model)
  {
    int selectedPageIndex = this.mainViewModel.SelectedPageIndex;
    ((MainView) App.Current.MainWindow).Menus_SelectItem("Page");
    this.PageEditListItemSource.AllItemSelected = new bool?(false);
    if (selectedPageIndex >= 0 && selectedPageIndex < this.PageEditListItemSource.Count)
      this.PageEditListItemSource[selectedPageIndex].Selected = true;
    this.InsertPageContextMenuItemInvoke(model);
  }

  public void AddPageNumber()
  {
    CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorAddPageNumberCmd", "Count", 1L);
    this.AddHeaderFooterOrPageNumber(true);
  }

  public void AddPageNumber2()
  {
    CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorAddPageNumberCmd", "Count", 1L);
    this.mainViewModel.ExitTransientMode();
    this.mainViewModel.AnnotationMode = AnnotationMode.None;
    this.mainViewModel.ReleaseViewerFocusAsync(true);
    this.AddHeaderFooterOrPageNumber(true);
  }

  private void UpdatePageNumber()
  {
    CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorUpdatePageNumberCmd", "Count", 1L);
    this.UpdateHeaderFooterOrPageNumber(true);
  }

  private void DeletePageNumber()
  {
    CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorDeletePageNumberCmd", "Count", 1L);
    this.DeleteHeaderFooterOrPageNumber(true);
  }

  private void AddHeaderFooterOrPageNumber(bool isPageNumber)
  {
    PdfPageEditList editListItemSource = this.PageEditListItemSource;
    PdfPageEditListModel[] pageEditListModelArray;
    if (editListItemSource == null)
    {
      pageEditListModelArray = (PdfPageEditListModel[]) null;
    }
    else
    {
      IReadOnlyCollection<PdfPageEditListModel> selectedItems = editListItemSource.SelectedItems;
      pageEditListModelArray = selectedItems != null ? selectedItems.ToArray<PdfPageEditListModel>() : (PdfPageEditListModel[]) null;
    }
    System.Collections.Generic.IReadOnlyList<HeaderFooterData> hfData;
    if ((pageEditListModelArray != null ? (pageEditListModelArray.Length != 0 ? 1 : 0) : 0) != 0 && (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null) || !this.GetHeaderFooterSettings(out hfData))
      return;
    MessageBoxResult messageBoxResult = MessageBoxResult.Yes;
    if (hfData != null && hfData.Count > 0)
    {
      TextBlock element1 = new TextBlock()
      {
        Text = Resources.WinPageInsertPageNumorHeaderFooterAddChekContent,
        TextWrapping = TextWrapping.Wrap,
        FontWeight = FontWeights.SemiBold
      };
      TextBlock textBlock = new TextBlock();
      textBlock.Text = Resources.WinPageInsertPageNumorHeaderFooterAddNoteContent;
      textBlock.TextWrapping = TextWrapping.Wrap;
      textBlock.Margin = new Thickness(0.0, 8.0, 0.0, 0.0);
      textBlock.Foreground = (System.Windows.Media.Brush) new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte) 204, (byte) 0, (byte) 0, (byte) 0));
      TextBlock element2 = textBlock;
      StackPanel stackPanel1 = new StackPanel();
      stackPanel1.Children.Add((UIElement) element1);
      stackPanel1.Children.Add((UIElement) element2);
      StackPanel stackPanel2 = stackPanel1;
      messageBoxResult = ModernMessageBox.Show(new ModernMessageBoxOptions()
      {
        Owner = (Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>(),
        Caption = UtilManager.GetProductName(),
        MessageBoxContent = (object) stackPanel2,
        Button = MessageBoxButton.YesNoCancel,
        UIOverrides = {
          YesButtonContent = (object) Resources.WinPageInsertPageNumorHeaderFooterAddBtnUpdateContent,
          NoButtonContent = (object) Resources.WinPageInsertPageNumorHeaderFooterAddBtnAddNewContent,
          IsButtonsReversed = true,
          HighlightPrimaryButton = true
        }
      });
    }
    if (messageBoxResult == MessageBoxResult.Cancel)
      return;
    bool isReplace = messageBoxResult == MessageBoxResult.Yes;
    HeaderFooterSettings settings1 = (HeaderFooterSettings) null;
    if (isReplace)
    {
      System.Collections.Generic.IReadOnlyList<HeaderFooterData> source = hfData;
      settings1 = source != null ? source.FirstOrDefault<HeaderFooterData>((Func<HeaderFooterData, bool>) (c => c.SettingsData?.Settings != null))?.SettingsData.Settings : (HeaderFooterSettings) null;
    }
    string documentPath = this.mainViewModel.DocumentWrapper.DocumentPath;
    string fileName = "";
    if (!string.IsNullOrEmpty(documentPath))
      fileName = Path.GetFileName(documentPath);
    bool? nullable1 = new bool?();
    HeaderFooterSettings settings = (HeaderFooterSettings) null;
    bool? nullable2;
    if (isPageNumber)
    {
      PageNumberDialog pageNumberDialog = new PageNumberDialog(this.mainViewModel.Document, settings1);
      pageNumberDialog.Owner = (Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
      pageNumberDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      nullable2 = pageNumberDialog.ShowDialog();
      if (nullable2.GetValueOrDefault())
        settings = pageNumberDialog.Result.ToSettings(this.mainViewModel.Document);
    }
    else
    {
      PageHeaderFooterDialog headerFooterDialog = new PageHeaderFooterDialog(this.mainViewModel.Document, fileName, settings1);
      headerFooterDialog.Owner = (Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
      headerFooterDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      nullable2 = headerFooterDialog.ShowDialog();
      if (nullable2.GetValueOrDefault())
        settings = headerFooterDialog.Result.ToSettings(this.mainViewModel.Document);
    }
    if (!nullable2.GetValueOrDefault())
      return;
    ProgressUtils.ShowProgressBar((Func<ProgressUtils.ProgressAction, Task>) (async c =>
    {
      if (isReplace)
      {
        Progress<double> progress = new Progress<double>();
        progress.ProgressChanged += (EventHandler<double>) ((s, a) => c.Report(a / 2.0));
        await PageHeaderFooterUtils.RemoveAllPageHeaderFooterSettingsAsync(this.mainViewModel.Document, hfData, (IProgress<double>) progress);
      }
      Progress<double> progress1 = new Progress<double>();
      progress1.ProgressChanged += (EventHandler<double>) ((s, a) =>
      {
        if (isReplace)
          c.Report(0.5 + a / 2.0);
        else
          c.Report(a);
      });
      int num = await PageHeaderFooterUtils.ApplyHeaderFooterSettingsAsync(this.mainViewModel.Document, settings, isReplace ? (System.Collections.Generic.IReadOnlyList<HeaderFooterData>) null : hfData, (IProgress<double>) progress1) ? 1 : 0;
      c.Complete();
    }), (string) null, (object) Resources.WinPageApplyingHeaderorFooterContent, false, (Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>());
    this.mainViewModel.SetCanSaveFlag();
    this.FlushViewerAndThumbnail();
  }

  private void UpdateHeaderFooterOrPageNumber(bool isPageNumber)
  {
    PdfPageEditList editListItemSource = this.PageEditListItemSource;
    PdfPageEditListModel[] pageEditListModelArray;
    if (editListItemSource == null)
    {
      pageEditListModelArray = (PdfPageEditListModel[]) null;
    }
    else
    {
      IReadOnlyCollection<PdfPageEditListModel> selectedItems = editListItemSource.SelectedItems;
      pageEditListModelArray = selectedItems != null ? selectedItems.ToArray<PdfPageEditListModel>() : (PdfPageEditListModel[]) null;
    }
    System.Collections.Generic.IReadOnlyList<HeaderFooterData> hfData;
    if ((pageEditListModelArray != null ? (pageEditListModelArray.Length != 0 ? 1 : 0) : 0) != 0 && (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null) || !this.GetHeaderFooterSettings(out hfData))
      return;
    if (hfData.Count == 0)
    {
      int num1 = (int) ModernMessageBox.Show((Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>(), Resources.WinPageInsertPageNumorHeaderFooterUpdateCheckEmptyContent, UtilManager.GetProductName());
    }
    else
    {
      System.Collections.Generic.IReadOnlyList<HeaderFooterData> source = hfData;
      HeaderFooterSettings settings1 = source != null ? source.FirstOrDefault<HeaderFooterData>((Func<HeaderFooterData, bool>) (c => c.SettingsData?.Settings != null))?.SettingsData.Settings : (HeaderFooterSettings) null;
      string documentPath = this.mainViewModel.DocumentWrapper.DocumentPath;
      string fileName = "";
      if (!string.IsNullOrEmpty(documentPath))
        fileName = Path.GetFileName(documentPath);
      bool? nullable1 = new bool?();
      HeaderFooterSettings settings = (HeaderFooterSettings) null;
      bool? nullable2;
      if (isPageNumber)
      {
        PageNumberDialog pageNumberDialog = new PageNumberDialog(this.mainViewModel.Document, settings1);
        pageNumberDialog.Owner = (Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
        pageNumberDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        nullable2 = pageNumberDialog.ShowDialog();
        if (nullable2.GetValueOrDefault())
          settings = pageNumberDialog.Result.ToSettings(this.mainViewModel.Document);
      }
      else
      {
        PageHeaderFooterDialog headerFooterDialog = new PageHeaderFooterDialog(this.mainViewModel.Document, fileName, settings1);
        headerFooterDialog.Owner = (Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
        headerFooterDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        nullable2 = headerFooterDialog.ShowDialog();
        if (nullable2.GetValueOrDefault())
          settings = headerFooterDialog.Result.ToSettings(this.mainViewModel.Document);
      }
      if (!nullable2.GetValueOrDefault())
        return;
      ProgressUtils.ShowProgressBar((Func<ProgressUtils.ProgressAction, Task>) (async c =>
      {
        Progress<double> progress1 = new Progress<double>();
        progress1.ProgressChanged += (EventHandler<double>) ((s, a) => c.Report(a / 2.0));
        await PageHeaderFooterUtils.RemoveAllPageHeaderFooterSettingsAsync(this.mainViewModel.Document, hfData, (IProgress<double>) progress1);
        Progress<double> progress2 = new Progress<double>();
        progress2.ProgressChanged += (EventHandler<double>) ((s, a) => c.Report(0.5 + a / 2.0));
        int num2 = await PageHeaderFooterUtils.ApplyHeaderFooterSettingsAsync(this.mainViewModel.Document, settings, (IProgress<double>) progress2) ? 1 : 0;
        c.Complete();
      }), (string) null, (object) Resources.WinPageApplyingHeaderorFooterContent, false, (Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>());
      this.mainViewModel.SetCanSaveFlag();
      this.FlushViewerAndThumbnail();
    }
  }

  private void DeleteHeaderFooterOrPageNumber(bool isPageNumber)
  {
    System.Collections.Generic.IReadOnlyList<HeaderFooterData> hfData;
    if (!this.GetHeaderFooterSettings(out hfData))
      return;
    if (hfData.Count == 0)
    {
      int num = (int) ModernMessageBox.Show((Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>(), Resources.WinPageInsertPageNumorHeaderFooterDelCheckEmptyContent, UtilManager.GetProductName());
    }
    else
    {
      if (ModernMessageBox.Show((Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>(), Resources.WinPageInsertPageNumorHeaderFooterDelCheckExistContent, UtilManager.GetProductName(), MessageBoxButton.YesNo, isButtonReversed: true) != MessageBoxResult.Yes)
        return;
      ProgressUtils.ShowProgressBar((Func<ProgressUtils.ProgressAction, Task>) (async c =>
      {
        Progress<double> progress = new Progress<double>();
        progress.ProgressChanged += (EventHandler<double>) ((s, a) => c.Report(a));
        await PageHeaderFooterUtils.RemoveAllPageHeaderFooterSettingsAsync(this.mainViewModel.Document, hfData, (IProgress<double>) progress);
        c.Complete();
      }), (string) null, (object) Resources.WinPageApplyingHeaderorFooterContent, false, (Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>());
      this.mainViewModel.SetCanSaveFlag();
      this.FlushViewerAndThumbnail();
    }
  }

  public void FlushViewerAndThumbnail(bool forceRedraw = false)
  {
    Ioc.Default.GetRequiredService<PdfThumbnailService>().RefreshAllThumbnail();
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
    pdfControl?.Redraw(forceRedraw);
    AnnotationCanvas annotationCanvas = PdfObjectExtensions.GetAnnotationCanvas(pdfControl);
    if (annotationCanvas == null || annotationCanvas.ImageControl.Visibility != Visibility.Visible)
      return;
    annotationCanvas.ImageControl.Visibility = Visibility.Collapsed;
  }

  public void ReorderPages(
    PdfPageEditListModel beforeItem,
    PdfPageEditListModel afterItem,
    PdfPageEditListModel[] selectedItems)
  {
    if (beforeItem == null && afterItem == null || selectedItems == null || selectedItems.Length == 0)
      return;
    int index1 = -1;
    if (beforeItem != null)
      index1 = beforeItem.PageIndex + 1;
    else if (afterItem != null)
      index1 = afterItem.PageIndex;
    PdfPageEditListModel[] array1 = ((IEnumerable<PdfPageEditListModel>) selectedItems).OrderBy<PdfPageEditListModel, int>((Func<PdfPageEditListModel, int>) (c => c.PageIndex)).ToArray<PdfPageEditListModel>();
    if (this.mainViewModel.CanSave && this.mainViewModel.ExtraSaveOperationName != "Reorder")
    {
      int num1 = (int) ModernMessageBox.Show(Resources.PageSplitMergeCheckMsg, UtilManager.GetProductName());
    }
    else
    {
      if (!ConfigManager.GetDoNotShowFlag("PageReorderDontShow"))
      {
        pdfeditor.Utils.MessageBoxHelper.RichMessageBoxResult messageBoxResult = pdfeditor.Utils.MessageBoxHelper.Show(new pdfeditor.Utils.MessageBoxHelper.RichMessageBoxContent()
        {
          Content = (object) Resources.WinPageReorderToolTipContent,
          ShowLeftBottomCheckbox = true,
          LeftBottomCheckboxContent = Resources.WinPwdPasswordSaveTipNotshowagainContent
        }, UtilManager.GetProductName(), MessageBoxButton.YesNo);
        if (messageBoxResult.Result == MessageBoxResult.Yes)
        {
          bool? checkboxResult = messageBoxResult.CheckboxResult;
          if (checkboxResult.HasValue && checkboxResult.GetValueOrDefault())
            ConfigManager.SetDoNotShowFlag("PageReorderDontShow", true);
        }
        if (messageBoxResult.Result != MessageBoxResult.Yes)
          return;
      }
      CommomLib.Commom.GAManager.SendEvent("PageView", nameof (ReorderPages), "Drag", 1L);
      if (!IAPUtils.IsPaidUserWrapper())
      {
        IAPUtils.ShowPurchaseWindows(nameof (ReorderPages), ".pdf");
      }
      else
      {
        int num2 = index1;
        PdfPageEditListModel[] pageEditListModelArray = array1;
        for (int index2 = 0; index2 < pageEditListModelArray.Length && pageEditListModelArray[index2].PageIndex < num2; ++index2)
          --index1;
        if (index1 < 0)
          index1 = 0;
        int[] array2 = ((IEnumerable<PdfPageEditListModel>) array1).Select<PdfPageEditListModel, int>((Func<PdfPageEditListModel, int>) (c => c.PageIndex)).ToArray<int>();
        int[] sortedPageIndexes;
        string range = ((IEnumerable<int>) array2).ConvertToRange(out sortedPageIndexes);
        PageDisposeHelper.TryFixResource(this.mainViewModel.Document, ((IEnumerable<int>) array2).Min(), ((IEnumerable<int>) array2).Max());
        using (PdfDocument sourceDoc = PdfDocument.CreateNew())
        {
          sourceDoc.Pages.ImportPages(this.mainViewModel.Document, range, 0);
          for (int index3 = sortedPageIndexes.Length - 1; index3 >= 0; --index3)
            this.mainViewModel.Document.Pages.DeleteAt(sortedPageIndexes[index3]);
          this.mainViewModel.Document.Pages.ImportPages(sourceDoc, $"1-{sourceDoc.Pages.Count}", index1);
        }
        PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.mainViewModel.Document);
        if (pdfControl != null && pdfControl.Document != null)
          pdfControl.UpdateDocLayout();
        this.mainViewModel.UpdateDocumentCore();
        for (int index4 = 0; index4 < selectedItems.Length; ++index4)
        {
          int index5 = index4 + index1;
          if (index5 >= 0 && index5 < this.PageEditListItemSource.Count)
            this.PageEditListItemSource[index5].Selected = true;
        }
        this.mainViewModel.SetCanSaveFlag("Reorder", true);
        CommomLib.Commom.GAManager.SendEvent("PageView", nameof (ReorderPages), "Done", 1L);
      }
    }
  }

  public async Task DoCreateBlankPageAsync(PdfPageEditListModel model = null, bool IsSiderbar = false)
  {
    CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorCreateBlankCmd", "Begin", 1L);
    this.mainViewModel.CreatePdfFileAsync();
  }

  private async void DoInsertBlankPage(PdfPageEditListModel model = null, bool IsSiderbar = false)
  {
    this.mainViewModel.ExitTransientMode();
    if (IsSiderbar)
      CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorInsertBlankCmd", "Siderbar", 1L);
    else
      CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorInsertBlankCmd", "PageToolbar", 1L);
    if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
      return;
    PdfPageEditListModel[] source1;
    if (model != null)
      source1 = new PdfPageEditListModel[1]{ model };
    else if (IsSiderbar)
    {
      source1 = new PdfPageEditListModel[1]
      {
        new PdfPageEditListModel(this.mainViewModel.Document, this.mainViewModel.SelectedPageIndex)
      };
    }
    else
    {
      PdfPageEditList editListItemSource = this.PageEditListItemSource;
      PdfPageEditListModel[] pageEditListModelArray;
      if (editListItemSource == null)
      {
        pageEditListModelArray = (PdfPageEditListModel[]) null;
      }
      else
      {
        IReadOnlyCollection<PdfPageEditListModel> selectedItems = editListItemSource.SelectedItems;
        pageEditListModelArray = selectedItems != null ? selectedItems.ToArray<PdfPageEditListModel>() : (PdfPageEditListModel[]) null;
      }
      source1 = pageEditListModelArray;
    }
    int[] selectedPages;
    if (source1 == null)
    {
      selectedPages = (int[]) null;
    }
    else
    {
      IEnumerable<int> source2 = ((IEnumerable<PdfPageEditListModel>) source1).Select<PdfPageEditListModel, int>((Func<PdfPageEditListModel, int>) (c => c.PageIndex));
      selectedPages = source2 != null ? source2.ToArray<int>() : (int[]) null;
    }
    PdfDocument document = this.mainViewModel.Document;
    int num = model != null ? 1 : 0;
    InsertBlankPageDialog insertBlankPageDialog = new InsertBlankPageDialog((IEnumerable<int>) selectedPages, document, num != 0);
    if (!insertBlankPageDialog.ShowDialog().GetValueOrDefault())
      return;
    SizeF size = insertBlankPageDialog.PageSize;
    int insertIndex = insertBlankPageDialog.InsertBefore ? insertBlankPageDialog.InsertPageIndex : insertBlankPageDialog.InsertPageIndex + 1;
    int pageCount = insertBlankPageDialog.PageCount;
    for (int index = 0; index < pageCount; ++index)
    {
      this.mainViewModel.Document.Pages.InsertPageAt(insertIndex, size.Width, size.Height);
      this.mainViewModel.Document.Pages[insertIndex].GenerateContent();
    }
    this.mainViewModel.LastViewPage = this.mainViewModel.Document.Pages[insertIndex];
    this.mainViewModel.UpdateDocumentCore();
    this.FlushViewerAndThumbnail();
    StrongReferenceMessenger.Default.Send<ValueChangedMessage<(int, int)>, string>(new ValueChangedMessage<(int, int)>((insertIndex, insertIndex + pageCount - 1)), "MESSAGE_PAGE_EDITOR_SELECT_INDEX");
    await this.mainViewModel.OperationManager.AddOperationAsync((Action<PdfDocument>) (doc =>
    {
      for (int index = 0; index < pageCount; ++index)
        doc.Pages.DeleteAt(insertIndex);
      this.mainViewModel.UpdateDocumentCore();
      this.FlushViewerAndThumbnail();
    }), (Action<PdfDocument>) (doc =>
    {
      for (int index = 0; index < pageCount; ++index)
      {
        this.mainViewModel.Document.Pages.InsertPageAt(insertIndex, size.Width, size.Height);
        this.mainViewModel.Document.Pages[insertIndex].GenerateContent();
      }
      this.mainViewModel.UpdateDocumentCore();
    }));
  }

  private async void DoInsertPDF(PdfPageEditListModel model = null, bool IsSiderbar = false)
  {
    this.mainViewModel.ExitTransientMode();
    if (IsSiderbar)
      CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorInsertFromPDFCmd", "Siderbar", 1L);
    else
      CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorInsertFromPDFCmd", "PageToolbar", 1L);
    if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
      return;
    OpenFileDialog openFileDialog1 = new OpenFileDialog();
    openFileDialog1.Filter = "pdf|*.pdf";
    OpenFileDialog openFileDialog2 = openFileDialog1;
    PdfPageEditListModel[] source1;
    if (model != null)
      source1 = new PdfPageEditListModel[1]{ model };
    else if (IsSiderbar)
    {
      source1 = new PdfPageEditListModel[1]
      {
        new PdfPageEditListModel(this.mainViewModel.Document, this.mainViewModel.SelectedPageIndex)
      };
    }
    else
    {
      PdfPageEditList editListItemSource = this.PageEditListItemSource;
      PdfPageEditListModel[] pageEditListModelArray;
      if (editListItemSource == null)
      {
        pageEditListModelArray = (PdfPageEditListModel[]) null;
      }
      else
      {
        IReadOnlyCollection<PdfPageEditListModel> selectedItems = editListItemSource.SelectedItems;
        pageEditListModelArray = selectedItems != null ? selectedItems.ToArray<PdfPageEditListModel>() : (PdfPageEditListModel[]) null;
      }
      source1 = pageEditListModelArray;
    }
    if (!openFileDialog2.ShowDialog().GetValueOrDefault())
      return;
    string fileName = openFileDialog2.FileName;
    int[] numArray1;
    if (source1 == null)
    {
      numArray1 = (int[]) null;
    }
    else
    {
      IEnumerable<int> source2 = ((IEnumerable<PdfPageEditListModel>) source1).Select<PdfPageEditListModel, int>((Func<PdfPageEditListModel, int>) (c => c.PageIndex));
      numArray1 = source2 != null ? source2.ToArray<int>() : (int[]) null;
    }
    int[] numArray2 = numArray1;
    PdfDocument document = this.mainViewModel.Document;
    int[] selectedPages = numArray2;
    int num1 = model != null ? 1 : 0;
    PageMergeDialog pageMergeDialog = new PageMergeDialog(fileName, document, (IEnumerable<int>) selectedPages, num1 != 0);
    pageMergeDialog.Owner = (Window) Application.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
    pageMergeDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    if (!pageMergeDialog.ShowDialog().GetValueOrDefault())
      return;
    string sourceFilePath = pageMergeDialog.MergeTempFilePath;
    int insertIndex = pageMergeDialog.InsertPageIndex;
    bool isBefore = pageMergeDialog.InsertBefore;
    int sourceFilePageCount = pageMergeDialog.MergePageCount;
    (int startPage, int endPage) tuple = this.InsertPagesIntoDocument(this.mainViewModel.Document, sourceFilePath, insertIndex, isBefore);
    this.mainViewModel.LastViewPage = this.mainViewModel.Document.Pages[tuple.startPage];
    this.mainViewModel.UpdateDocumentCore();
    this.FlushViewerAndThumbnail();
    if (!isBefore)
    {
      int num2 = insertIndex;
    }
    else
    {
      int num3 = insertIndex;
    }
    StrongReferenceMessenger.Default.Send<ValueChangedMessage<(int, int)>, string>(new ValueChangedMessage<(int, int)>(tuple), "MESSAGE_PAGE_EDITOR_SELECT_INDEX");
    await this.mainViewModel.OperationManager.AddOperationAsync((Action<PdfDocument>) (doc =>
    {
      int num4 = isBefore ? insertIndex : insertIndex + 1;
      for (int index = num4 + sourceFilePageCount - 1; index >= num4; --index)
        doc.Pages.DeleteAt(index);
      this.mainViewModel.UpdateDocumentCore();
      this.FlushViewerAndThumbnail();
    }), (Action<PdfDocument>) (doc =>
    {
      this.InsertPagesIntoDocument(doc, sourceFilePath, insertIndex, isBefore);
      this.mainViewModel.UpdateDocumentCore();
    }));
  }

  private async void DoInsertWord(PdfPageEditListModel model = null, bool IsSiderbar = false)
  {
    this.mainViewModel.ExitTransientMode();
    if (IsSiderbar)
      CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorInsertFromWordCmd", "Siderbar", 1L);
    else
      CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorInsertFromWordCmd", "PageToolbar", 1L);
    if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
      return;
    OpenFileDialog openFileDialog1 = new OpenFileDialog();
    openFileDialog1.Filter = "Microsoft Office Word|" + UtilManager.WordExtention;
    OpenFileDialog openFileDialog2 = openFileDialog1;
    PdfPageEditListModel[] source1;
    if (model != null)
      source1 = new PdfPageEditListModel[1]{ model };
    else if (IsSiderbar)
    {
      source1 = new PdfPageEditListModel[1]
      {
        new PdfPageEditListModel(this.mainViewModel.Document, this.mainViewModel.SelectedPageIndex)
      };
    }
    else
    {
      PdfPageEditList editListItemSource = this.PageEditListItemSource;
      PdfPageEditListModel[] pageEditListModelArray;
      if (editListItemSource == null)
      {
        pageEditListModelArray = (PdfPageEditListModel[]) null;
      }
      else
      {
        IReadOnlyCollection<PdfPageEditListModel> selectedItems = editListItemSource.SelectedItems;
        pageEditListModelArray = selectedItems != null ? selectedItems.ToArray<PdfPageEditListModel>() : (PdfPageEditListModel[]) null;
      }
      source1 = pageEditListModelArray;
    }
    if (!openFileDialog2.ShowDialog().GetValueOrDefault())
      return;
    string fileName = openFileDialog2.FileName;
    int[] numArray1;
    if (source1 == null)
    {
      numArray1 = (int[]) null;
    }
    else
    {
      IEnumerable<int> source2 = ((IEnumerable<PdfPageEditListModel>) source1).Select<PdfPageEditListModel, int>((Func<PdfPageEditListModel, int>) (c => c.PageIndex));
      numArray1 = source2 != null ? source2.ToArray<int>() : (int[]) null;
    }
    int[] numArray2 = numArray1;
    PdfDocument document = this.mainViewModel.Document;
    int[] selectedPages = numArray2;
    int num1 = model != null ? 1 : 0;
    PageMergeDialog pageMergeDialog = new PageMergeDialog(fileName, document, (IEnumerable<int>) selectedPages, num1 != 0, InsertSourceFileType.Doc);
    pageMergeDialog.Owner = (Window) Application.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
    pageMergeDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    if (!pageMergeDialog.ShowDialog().GetValueOrDefault())
      return;
    string sourceFilePath = pageMergeDialog.MergeTempFilePath;
    int insertIndex = pageMergeDialog.InsertPageIndex;
    bool isBefore = pageMergeDialog.InsertBefore;
    int sourceFilePageCount = pageMergeDialog.MergePageCount;
    (int startPage, int endPage) tuple = this.InsertPagesIntoDocument(this.mainViewModel.Document, sourceFilePath, insertIndex, isBefore);
    this.mainViewModel.LastViewPage = this.mainViewModel.Document.Pages[tuple.startPage];
    this.mainViewModel.UpdateDocumentCore();
    this.FlushViewerAndThumbnail();
    if (!isBefore)
    {
      int num2 = insertIndex;
    }
    else
    {
      int num3 = insertIndex;
    }
    StrongReferenceMessenger.Default.Send<ValueChangedMessage<(int, int)>, string>(new ValueChangedMessage<(int, int)>(tuple), "MESSAGE_PAGE_EDITOR_SELECT_INDEX");
    await this.mainViewModel.OperationManager.AddOperationAsync((Action<PdfDocument>) (doc =>
    {
      int num4 = isBefore ? insertIndex : insertIndex + 1;
      for (int index = num4 + sourceFilePageCount - 1; index >= num4; --index)
        doc.Pages.DeleteAt(index);
      this.mainViewModel.UpdateDocumentCore();
    }), (Action<PdfDocument>) (doc =>
    {
      this.InsertPagesIntoDocument(doc, sourceFilePath, insertIndex, isBefore);
      this.mainViewModel.UpdateDocumentCore();
      this.FlushViewerAndThumbnail();
    }));
  }

  private async void DoInsertImage(PdfPageEditListModel model = null, bool IsSiderbar = false)
  {
    this.mainViewModel.ExitTransientMode();
    if (IsSiderbar)
      CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorInsertFromImageCmd", "Siderbar", 1L);
    else
      CommomLib.Commom.GAManager.SendEvent("PageView", "PageEditorInsertFromImageCmd", "PageToolbar", 1L);
    if (this.mainViewModel.Document == null || this.mainViewModel.Document.Pages == null)
      return;
    OpenFileDialog openFileDialog1 = new OpenFileDialog();
    openFileDialog1.Filter = "Image|" + UtilManager.ImageExtention;
    OpenFileDialog openFileDialog2 = openFileDialog1;
    PdfPageEditListModel[] source1;
    if (model != null)
      source1 = new PdfPageEditListModel[1]{ model };
    else if (IsSiderbar)
    {
      source1 = new PdfPageEditListModel[1]
      {
        new PdfPageEditListModel(this.mainViewModel.Document, this.mainViewModel.SelectedPageIndex)
      };
    }
    else
    {
      PdfPageEditList editListItemSource = this.PageEditListItemSource;
      PdfPageEditListModel[] pageEditListModelArray;
      if (editListItemSource == null)
      {
        pageEditListModelArray = (PdfPageEditListModel[]) null;
      }
      else
      {
        IReadOnlyCollection<PdfPageEditListModel> selectedItems = editListItemSource.SelectedItems;
        pageEditListModelArray = selectedItems != null ? selectedItems.ToArray<PdfPageEditListModel>() : (PdfPageEditListModel[]) null;
      }
      source1 = pageEditListModelArray;
    }
    if (!openFileDialog2.ShowDialog().GetValueOrDefault())
      return;
    string file = openFileDialog2.FileName;
    int[] numArray;
    if (source1 == null)
    {
      numArray = (int[]) null;
    }
    else
    {
      IEnumerable<int> source2 = ((IEnumerable<PdfPageEditListModel>) source1).Select<PdfPageEditListModel, int>((Func<PdfPageEditListModel, int>) (c => c.PageIndex));
      numArray = source2 != null ? source2.ToArray<int>() : (int[]) null;
    }
    int[] selectedPages = numArray;
    Bitmap image = (Bitmap) null;
    try
    {
      image = (Bitmap) System.Drawing.Image.FromFile(file, true);
      InsertPageFromImage insertPageFromImage = new InsertPageFromImage(file, this.mainViewModel.Document, (IEnumerable<int>) selectedPages, model != null);
      insertPageFromImage.Owner = (Window) Application.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
      insertPageFromImage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      if (!insertPageFromImage.ShowDialog().GetValueOrDefault())
        return;
      SizeF size = insertPageFromImage.PageSize;
      int insertIndex = insertPageFromImage.InsertBefore ? insertPageFromImage.InsertPageIndex : insertPageFromImage.InsertPageIndex + 1;
      PdfPage pdfPage1 = this.mainViewModel.Document.Pages.InsertPageAt(insertIndex, size.Width, size.Height);
      float renderwidth = size.Width - 180f;
      float renderheight = size.Height - 144f;
      float d1 = (float) (image.Height * 72) / image.VerticalResolution;
      float a1 = (float) (image.Width * 72) / image.HorizontalResolution;
      float num1 = Math.Min(renderwidth / a1, renderheight / d1);
      if ((double) num1 < 1.0)
      {
        d1 *= num1;
        a1 *= num1;
      }
      float num2 = (float) (((double) renderwidth - (double) a1) / 2.0 + 90.0);
      float num3 = (float) (((double) renderheight - (double) d1) / 2.0 + 72.0);
      float e1 = (double) num2 < 0.0 ? 0.0f : num2;
      float f1 = (double) num3 < 0.0 ? 0.0f : num3;
      using (PdfBitmap bitmap = PdfBitmap.FromBitmap(image))
      {
        PdfImageObject pdfImageObject = PdfImageObject.Create(this.mainViewModel.Document, bitmap, 0.0f, 0.0f);
        pdfPage1.PageObjects.Add((PdfPageObject) pdfImageObject);
        pdfImageObject.Matrix = new FS_MATRIX(a1, 0.0f, 0.0f, d1, e1, f1);
        pdfPage1.GenerateContent();
      }
      this.mainViewModel.LastViewPage = pdfPage1;
      this.mainViewModel.UpdateDocumentCore();
      this.FlushViewerAndThumbnail();
      StrongReferenceMessenger.Default.Send<ValueChangedMessage<(int, int)>, string>(new ValueChangedMessage<(int, int)>((insertIndex, insertIndex)), "MESSAGE_PAGE_EDITOR_SELECT_INDEX");
      await this.mainViewModel.OperationManager.AddOperationAsync((Action<PdfDocument>) (doc =>
      {
        doc.Pages.DeleteAt(insertIndex);
        if (!(PDFKit.PdfControl.GetPdfControl(doc)?.DataContext is MainViewModel dataContext2))
          return;
        dataContext2.UpdateDocumentCore();
      }), (Action<PdfDocument>) (doc =>
      {
        PdfPage pdfPage2 = doc.Pages.InsertPageAt(insertIndex, size.Width, size.Height);
        using (Bitmap image1 = System.Drawing.Image.FromFile(file, true) as Bitmap)
        {
          float d2 = (float) (image1.Height * 72) / image1.VerticalResolution;
          float a2 = (float) (image1.Width * 72) / image1.HorizontalResolution;
          float num4 = Math.Min(renderwidth / a2, renderheight / d2);
          if ((double) num4 < 1.0)
          {
            d2 *= num4;
            a2 *= num4;
          }
          float num5 = (float) (((double) renderwidth - (double) a2) / 2.0 + 90.0);
          float num6 = (float) (((double) renderheight - (double) d2) / 2.0 + 72.0);
          float e2 = (double) num5 < 0.0 ? 0.0f : num5;
          float f2 = (double) num6 < 0.0 ? 0.0f : num6;
          using (PdfBitmap bitmap = PdfBitmap.FromBitmap(image1))
          {
            PdfImageObject pdfImageObject = PdfImageObject.Create(doc, bitmap, 0.0f, 0.0f);
            pdfPage2.PageObjects.Add((PdfPageObject) pdfImageObject);
            pdfImageObject.Matrix = new FS_MATRIX(a2, 0.0f, 0.0f, d2, e2, f2);
            pdfPage2.GenerateContent();
          }
        }
        if (PDFKit.PdfControl.GetPdfControl(doc)?.DataContext is MainViewModel dataContext4)
          dataContext4.UpdateDocumentCore();
        dataContext4?.PageEditors?.FlushViewerAndThumbnail();
      }));
    }
    catch
    {
      DrawUtils.ShowUnsupportedImageMessage();
      return;
    }
    finally
    {
      image?.Dispose();
    }
    image = (Bitmap) null;
  }
}
