// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Thumbnails.PdfPageEditListModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using CommomLib.Commom.HotKeys;
using Microsoft.Toolkit.Mvvm.Input;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using pdfeditor.Models.Menus;
using pdfeditor.Properties;
using pdfeditor.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable enable
namespace pdfeditor.Models.Thumbnails;

public class PdfPageEditListModel : PdfThumbnailModel
{
  private bool selected;
  private double pageWidth;
  private double pageHeight;
  private PageRotate pageRotate;
  private double thumbnailWidth;
  private double thumbnailHeight;

  public PdfPageEditListModel(
  #nullable disable
  PdfDocument document, int pageIndex)
    : base(document, pageIndex)
  {
    Pdfium.FPDF_GetPageSizeByIndex(document.Handle, pageIndex, out this.pageWidth, out this.pageHeight);
    Pdfium.FPDF_GetPageRotationByIndex(document.Handle, pageIndex, out this.pageRotate);
    if (this.pageRotate == PageRotate.Rotate90 || this.pageRotate == PageRotate.Rotate270)
    {
      double pageWidth = this.pageWidth;
      this.pageWidth = this.pageHeight;
      this.pageHeight = pageWidth;
    }
    this.InitContextMenu();
  }

  public static double DefaultThumbnailWidth => 300.0;

  public bool Selected
  {
    get => this.selected;
    set => this.SetProperty<bool>(ref this.selected, value, nameof (Selected));
  }

  public double PageWidth
  {
    get => this.pageWidth;
    set => this.SetProperty<double>(ref this.pageWidth, value, nameof (PageWidth));
  }

  public double PageHeight
  {
    get => this.pageHeight;
    set => this.SetProperty<double>(ref this.pageHeight, value, nameof (PageHeight));
  }

  public PageRotate PageRotate
  {
    get => this.pageRotate;
    set => this.SetProperty<PageRotate>(ref this.pageRotate, value, nameof (PageRotate));
  }

  public double ThumbnailWidth
  {
    get => this.thumbnailWidth;
    set => this.SetProperty<double>(ref this.thumbnailWidth, value, nameof (ThumbnailWidth));
  }

  public double ThumbnailHeight
  {
    get => this.thumbnailHeight;
    set => this.SetProperty<double>(ref this.thumbnailHeight, value, nameof (ThumbnailHeight));
  }

  internal void UpdateThumbnailSize(double scale, double minAspectRatio)
  {
    if (minAspectRatio == 0.0)
    {
      this.ThumbnailWidth = 0.0;
      this.ThumbnailHeight = 0.0;
    }
    else
    {
      double num1 = PdfPageEditListModel.DefaultThumbnailWidth * scale;
      double val2 = num1 * 1.414 * 2.0;
      double num2 = Math.Min(num1 / minAspectRatio, val2);
      this.ThumbnailWidth = num1;
      this.ThumbnailHeight = num2;
    }
  }

  private void InitContextMenu()
  {
    ContextMenuHorizontalButton horizontalButton = new ContextMenuHorizontalButton()
    {
      Name = "HeaderButton",
      Caption0 = Resources.MenuPageDeleteContent,
      Icon0 = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/Delete.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/Delete.png")),
      Command0 = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => await this.VM.PageEditors.PageEditorDeleteCmd.ExecuteAsync((PdfPageEditListModel) null))),
      Caption1 = Resources.MenuPageRotateLeftContent,
      Icon1 = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_RotateLeft.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_RotateLeft.png")),
      Command1 = (ICommand) new RelayCommand((Action) (() => this.VM.PageEditors.PageEditorRotateLeftCmd.Execute((PdfPageEditListModel) null))),
      Caption2 = Resources.MenuPageRotateRightContent,
      Icon2 = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_RotateRight.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_RotateRight.png")),
      Command2 = (ICommand) new RelayCommand((Action) (() => this.VM.PageEditors.PageEditorRotateRightCmd.Execute((PdfPageEditListModel) null))),
      Caption3 = Resources.WinViewToolPrintTooltipText,
      Icon3 = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_Print.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_Print.png")),
      Command3 = (ICommand) new RelayCommand((Action) (() => this.VM.QuickToolPrintModel.Command.Execute((object) null)))
    };
    ContextMenuItemModel contextMenuItemModel1 = new ContextMenuItemModel()
    {
      Name = "InsertPage",
      Caption = Resources.RightMenuInsertPageText,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_InsertPage.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_InsertPage.png"))
    };
    ContextMenuItemModel contextMenuItemModel2 = new ContextMenuItemModel()
    {
      Name = "BlankPage",
      Caption = Resources.MenuPageSubInsertBlankPage,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/BlankPage.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/BlankPage.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => await this.VM.PageEditors.PageEditorInsertBlankCmd.ExecuteAsync((PdfPageEditListModel) null))),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_InsertBlankPage",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel3 = new ContextMenuItemModel()
    {
      Name = "FromPDF",
      Caption = Resources.RightMenuInsertPageItemPDF,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/FromPDF.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/FromPDF.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => await this.VM.PageEditors.PageEditorInsertFromPDFCmd.ExecuteAsync((PdfPageEditListModel) null))),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_InsertPDF",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel4 = new ContextMenuItemModel()
    {
      Name = "FromWord",
      Caption = Resources.RightMenuInsertPageItemWord,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/FromWord.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/FromWord.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => await this.VM.PageEditors.PageEditorInsertFromWordCmd.ExecuteAsync((PdfPageEditListModel) null))),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_InsertWord",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel5 = new ContextMenuItemModel()
    {
      Name = "FromImage",
      Caption = Resources.RightMenuInsertPageItemImage,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/FromImage.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/FromImage.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => await this.VM.PageEditors.PageEditorInsertFromImageCmd.ExecuteAsync((PdfPageEditListModel) null))),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_InsertImage",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel6 = new ContextMenuItemModel()
    {
      Name = "ExtractPages",
      Caption = Resources.ShortcutTextPageExtract,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_ExtractPages.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_ExtractPages.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => await this.VM.PageEditors.PageEditorExtractCmd.ExecuteAsync((PdfPageEditListModel) null))),
      HotKeyInvokeWhen = "Editor_ExtractPage",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel7 = new ContextMenuItemModel()
    {
      Name = "DeletePages",
      Caption = Resources.ShortcutTextPageDelete,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/Delete.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/Delete.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => await this.VM.PageEditors.PageEditorDeleteCmd.ExecuteAsync((PdfPageEditListModel) null))),
      HotKeyInvokeWhen = "Editor_DeletePage",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel8 = new ContextMenuItemModel()
    {
      Name = "CropPages",
      Caption = Resources.MainViewCropPageContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_CropPages.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_CropPages.png")),
      Command = (ICommand) new RelayCommand((Action) (() => this.VM.PageEditors.CropPageCmd.Execute((object) null))),
      HotKeyInvokeWhen = "Editor_CropPage",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel9 = new ContextMenuItemModel()
    {
      Name = "OCRPages",
      Caption = "OCR Pages",
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_OcrPages.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_OcrPages.png")),
      Command = (ICommand) new RelayCommand((Action) (() =>
      {
        MainView mainView = App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
        mainView.ocrBtn.IsChecked = new bool?(true);
        mainView.ocrBtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
      })),
      HotKeyInvokeWhen = "Editor_Ocr",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuModel contextMenuModel1 = new ContextMenuModel();
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel2);
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel3);
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel4);
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel5);
    foreach (IContextMenuModel contextMenuModel2 in (Collection<IContextMenuModel>) contextMenuModel1)
      contextMenuItemModel1.Add(contextMenuModel2);
    ContextMenuModel contextMenuModel3 = new ContextMenuModel();
    contextMenuModel3.Add((IContextMenuModel) horizontalButton);
    contextMenuModel3.Add((IContextMenuModel) new ContextMenuSeparator());
    contextMenuModel3.Add((IContextMenuModel) contextMenuItemModel1);
    contextMenuModel3.Add((IContextMenuModel) contextMenuItemModel7);
    contextMenuModel3.Add((IContextMenuModel) contextMenuItemModel6);
    this.ContextMenuModel = contextMenuModel3;
  }
}
