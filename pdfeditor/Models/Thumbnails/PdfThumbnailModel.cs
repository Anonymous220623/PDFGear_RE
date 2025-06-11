// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Thumbnails.PdfThumbnailModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using CommomLib.Commom.HotKeys;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Patagames.Pdf.Net;
using pdfeditor.Models.Menus;
using pdfeditor.Properties;
using pdfeditor.ViewModels;
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

public class PdfThumbnailModel : ObservableObject
{
  public PdfThumbnailModel(
  #nullable disable
  PdfDocument document, int pageIndex)
  {
    this.Document = document;
    this.PageIndex = pageIndex;
    this.InitContextMenu();
  }

  public PdfDocument Document { get; }

  public int PageIndex { get; }

  public int DisplayPageIndex => this.PageIndex + 1;

  public ContextMenuModel ContextMenuModel { get; set; }

  public MainViewModel VM => PDFKit.PdfControl.GetPdfControl(this.Document)?.DataContext as MainViewModel;

  private void InitContextMenu()
  {
    ContextMenuHorizontalButton horizontalButton = new ContextMenuHorizontalButton()
    {
      Name = "HeaderButton",
      Caption0 = Resources.MenuPageDeleteContent,
      Icon0 = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/Delete.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/Delete.png")),
      Command0 = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => await this.VM.PageEditors.PageEditorDeleteCmd.ExecuteAsync(new PdfPageEditListModel(this.VM.Document, this.VM.SelectedPageIndex)))),
      Caption1 = Resources.MenuPageRotateLeftContent,
      Icon1 = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_RotateLeft.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_RotateLeft.png")),
      Command1 = (ICommand) new RelayCommand((Action) (() => this.VM.ViewToolbar.PageRotateLeftCmd.Execute((object) null))),
      Caption2 = Resources.MenuPageRotateRightContent,
      Icon2 = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_RotateRight.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_RotateRight.png")),
      Command2 = (ICommand) new RelayCommand((Action) (() => this.VM.ViewToolbar.PageRotateRightCmd.Execute((object) null))),
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
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => await this.VM.PageEditors.PageEditorInsertBlankCmd.ExecuteAsync(new PdfPageEditListModel(this.VM.Document, this.VM.SelectedPageIndex)))),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_InsertBlankPage",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel3 = new ContextMenuItemModel()
    {
      Name = "FromPDF",
      Caption = Resources.RightMenuInsertPageItemPDF,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/FromPDF.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/FromPDF.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => await this.VM.PageEditors.PageEditorInsertFromPDFCmd.ExecuteAsync(new PdfPageEditListModel(this.VM.Document, this.VM.SelectedPageIndex)))),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_InsertPDF",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel4 = new ContextMenuItemModel()
    {
      Name = "FromWord",
      Caption = Resources.RightMenuInsertPageItemWord,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/FromWord.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/FromWord.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => await this.VM.PageEditors.PageEditorInsertFromWordCmd.ExecuteAsync(new PdfPageEditListModel(this.VM.Document, this.VM.SelectedPageIndex)))),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_InsertWord",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel5 = new ContextMenuItemModel()
    {
      Name = "FromImage",
      Caption = Resources.RightMenuInsertPageItemImage,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/FromImage.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/FromImage.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => await this.VM.PageEditors.PageEditorInsertFromImageCmd.ExecuteAsync(new PdfPageEditListModel(this.VM.Document, this.VM.SelectedPageIndex)))),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_InsertImage",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel6 = new ContextMenuItemModel()
    {
      Name = "ExtractPages",
      Caption = Resources.ShortcutTextPageExtract,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_ExtractPages.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_ExtractPages.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () =>
      {
        CommomLib.Commom.GAManager.SendEvent("PageContextMenu", "ExtractPages", "Count", 1L);
        await this.VM.PageEditors.PageEditorExtractCmd.ExecuteAsync(new PdfPageEditListModel(this.VM.Document, this.VM.SelectedPageIndex));
      })),
      HotKeyInvokeWhen = "Editor_ExtractPage",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel7 = new ContextMenuItemModel()
    {
      Name = "DeletePages",
      Caption = Resources.ShortcutTextPageDelete,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/Delete.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/Delete.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () =>
      {
        CommomLib.Commom.GAManager.SendEvent("PageContextMenu", "DeletePages", "Count", 1L);
        await this.VM.PageEditors.PageEditorDeleteCmd.ExecuteAsync(new PdfPageEditListModel(this.VM.Document, this.VM.SelectedPageIndex));
      })),
      HotKeyInvokeWhen = "Editor_DeletePage",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel8 = new ContextMenuItemModel()
    {
      Name = "CropPages",
      Caption = Resources.MainViewCropPageContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_CropPages.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_CropPages.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () =>
      {
        CommomLib.Commom.GAManager.SendEvent("PageContextMenu", "CropPages", "Count", 1L);
        this.VM.PageEditors.CropPageCmd2.Execute(new PdfPageEditListModel(this.VM.Document, this.VM.SelectedPageIndex));
      })),
      HotKeyInvokeWhen = "Editor_CropPage",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel9 = new ContextMenuItemModel()
    {
      Name = "OCR",
      Caption = Resources.RightMenuOcrPagesItemText,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_OcrPages.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_OcrPages.png")),
      Command = (ICommand) new RelayCommand((Action) (() =>
      {
        CommomLib.Commom.GAManager.SendEvent("PageContextMenu", "OcrPages", "Count", 1L);
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
    contextMenuModel3.Add((IContextMenuModel) new ContextMenuSeparator());
    contextMenuModel3.Add((IContextMenuModel) contextMenuItemModel8);
    contextMenuModel3.Add((IContextMenuModel) contextMenuItemModel9);
    this.ContextMenuModel = contextMenuModel3;
  }
}
