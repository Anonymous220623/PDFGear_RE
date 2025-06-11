// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PdfViewerDecorators.PageDefaultContextMenuHolder
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using CommomLib.Commom.HotKeys;
using Microsoft.Toolkit.Mvvm.Input;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Models.Bookmarks;
using pdfeditor.Models.Menus;
using pdfeditor.Models.Thumbnails;
using pdfeditor.Properties;
using pdfeditor.ViewModels;
using pdfeditor.Views;
using PDFKit;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable enable
namespace pdfeditor.Controls.PdfViewerDecorators;

internal class PageDefaultContextMenuHolder
{
  private readonly 
  #nullable disable
  AnnotationCanvas annotationCanvas;
  private PdfViewerContextMenu contextMenu;

  public PageDefaultContextMenuHolder(AnnotationCanvas annotationCanvas)
  {
    this.annotationCanvas = annotationCanvas ?? throw new ArgumentNullException(nameof (annotationCanvas));
    this.InitContextMenu();
  }

  private MainViewModel VM => this.annotationCanvas.DataContext as MainViewModel;

  private void InitContextMenu()
  {
    if (PageDefaultContextMenuHolder.IsDesignMode)
      return;
    ContextMenuHorizontalButton horizontalButton = new ContextMenuHorizontalButton()
    {
      Name = "HeaderButton",
      Caption0 = Resources.MenuPageDeleteContent,
      Icon0 = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/PageEditor/Delete.png"), new Uri("pack://application:,,,/Style/DarkModeResources/PageEditor/Delete.png")),
      Command0 = (ICommand) new AsyncRelayCommand((Func<Task>) (async () =>
      {
        CommomLib.Commom.GAManager.SendEvent("PageContextMenu", "DeletePages", "Count", 1L);
        await this.VM.PageEditors.PageEditorDeleteCmd.ExecuteAsync(new PdfPageEditListModel(this.VM.Document, this.VM.SelectedPageIndex));
        this.Hide();
      })),
      Caption1 = Resources.MenuPageRotateLeftContent,
      Icon1 = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_RotateLeft.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_RotateLeft.png")),
      Command1 = (ICommand) new AsyncRelayCommand((Func<Task>) (async () =>
      {
        this.VM.ViewToolbar.PageRotateLeftCmd.Execute((object) null);
        this.Hide();
      })),
      Caption2 = Resources.MenuPageRotateRightContent,
      Icon2 = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_RotateRight.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_RotateRight.png")),
      Command2 = (ICommand) new AsyncRelayCommand((Func<Task>) (async () =>
      {
        this.VM.ViewToolbar.PageRotateRightCmd.Execute((object) null);
        this.Hide();
      })),
      Caption3 = Resources.WinViewToolPrintTooltipText,
      Icon3 = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_Print.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_Print.png")),
      Command3 = (ICommand) new AsyncRelayCommand((Func<Task>) (async () =>
      {
        this.VM.QuickToolPrintModel.Command.Execute((object) null);
        this.Hide();
      }))
    };
    ContextMenuItemModel contextMenuItemModel1 = new ContextMenuItemModel()
    {
      Name = "EditText",
      Caption = Resources.MenuViewEditTextContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_EditText.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_EditText.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => this.VM.ViewToolbar.EditDocumentButtomModel.Command.Execute((object) this.VM.ViewToolbar.EditDocumentButtomModel))),
      HotKeyInvokeWhen = "Editor_EditText"
    };
    ContextMenuItemModel contextMenuItemModel2 = new ContextMenuItemModel()
    {
      Name = "AddBookmark",
      Caption = Resources.DefaultContextMenuAddBookmark,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_AddBookmark.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_AddBookmark.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () =>
      {
        CommomLib.Commom.GAManager.SendEvent("Bookmark", "AddBookmark", "PageContextMenu", 1L);
        await this.VM.BookmarkAddCommand.ExecuteAsync((BookmarkModel) null);
      })),
      HotKeyInvokeWhen = "Editor_AddBookmark",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel3 = new ContextMenuItemModel()
    {
      Name = "MergeAndSplit",
      Caption = Resources.RightMenuMergeSplitPDFItemText,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_MergeAndSplit.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_MergeAndSplit.png"))
    };
    ContextMenuItemModel contextMenuItemModel4 = new ContextMenuItemModel()
    {
      Name = "MergePDF",
      Caption = Resources.MenuPageMergeContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_MergePDF.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_MergePDF.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () =>
      {
        CommomLib.Commom.GAManager.SendEvent("PageContextMenu", "MergePDF", "Count", 1L);
        await this.VM.PageEditors.PageEditorMergeCmd.ExecuteAsync((PdfPageEditListModel) null);
      }))
    };
    ContextMenuItemModel contextMenuItemModel5 = new ContextMenuItemModel()
    {
      Name = "SplitPDF",
      Caption = Resources.MenuPageSplitContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_SplitPDF.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_SplitPDF.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () =>
      {
        CommomLib.Commom.GAManager.SendEvent("PageContextMenu", "SplitPDF", "Count", 1L);
        await this.VM.PageEditors.PageEditorSplitCmd.ExecuteAsync((object) null);
      }))
    };
    ContextMenuItemModel contextMenuItemModel6 = new ContextMenuItemModel()
    {
      Name = "CompreePDF",
      Caption = Resources.RightMenuCompressPDFItemText,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_CompreePDF.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_CompreePDF.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () =>
      {
        CommomLib.Commom.GAManager.SendEvent("PageContextMenu", "SplitPDF", "Count", 1L);
        this.VM.ConverterCommands.CompressPDF.Execute((object) null);
      }))
    };
    ContextMenuItemModel contextMenuItemModel7 = new ContextMenuItemModel()
    {
      Name = "InsertPage",
      Caption = Resources.RightMenuInsertPageText,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_InsertPage.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_InsertPage.png"))
    };
    ContextMenuItemModel contextMenuItemModel8 = new ContextMenuItemModel()
    {
      Name = "BlankPage",
      Caption = Resources.MenuPageSubInsertBlankPage,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/BlankPage.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/BlankPage.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => await this.VM.PageEditors.PageEditorInsertBlankCmd.ExecuteAsync(new PdfPageEditListModel(this.VM.Document, this.VM.SelectedPageIndex)))),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_InsertBlankPage",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel9 = new ContextMenuItemModel()
    {
      Name = "FromPDF",
      Caption = Resources.RightMenuInsertPageItemPDF,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/FromPDF.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/FromPDF.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => await this.VM.PageEditors.PageEditorInsertFromPDFCmd.ExecuteAsync(new PdfPageEditListModel(this.VM.Document, this.VM.SelectedPageIndex)))),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_InsertPDF",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel10 = new ContextMenuItemModel()
    {
      Name = "FromWord",
      Caption = Resources.RightMenuInsertPageItemWord,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/FromWord.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/FromWord.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => await this.VM.PageEditors.PageEditorInsertFromWordCmd.ExecuteAsync(new PdfPageEditListModel(this.VM.Document, this.VM.SelectedPageIndex)))),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_InsertWord",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel11 = new ContextMenuItemModel()
    {
      Name = "FromImage",
      Caption = Resources.RightMenuInsertPageItemImage,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/FromImage.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/FromImage.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => await this.VM.PageEditors.PageEditorInsertFromImageCmd.ExecuteAsync(new PdfPageEditListModel(this.VM.Document, this.VM.SelectedPageIndex)))),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_InsertImage",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel12 = new ContextMenuItemModel()
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
    ContextMenuItemModel contextMenuItemModel13 = new ContextMenuItemModel()
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
    ContextMenuItemModel contextMenuItemModel14 = new ContextMenuItemModel()
    {
      Name = "CropPages",
      Caption = Resources.MainViewCropPageContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_CropPages.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_CropPages.png")),
      Command = (ICommand) new RelayCommand((Action) (() =>
      {
        CommomLib.Commom.GAManager.SendEvent("PageContextMenu", "CropPages", "Count", 1L);
        this.VM.PageEditors.CropPageCmd2.Execute(new PdfPageEditListModel(this.VM.Document, this.VM.SelectedPageIndex));
      })),
      HotKeyInvokeWhen = "Editor_CropPage",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel15 = new ContextMenuItemModel()
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
    ContextMenuItemModel contextMenuItemModel16 = new ContextMenuItemModel()
    {
      Name = "Add",
      Caption = Resources.RightMenuAddItemText,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_Add.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_Add.png"))
    };
    ContextMenuItemModel contextMenuItemModel17 = new ContextMenuItemModel()
    {
      Name = "Image",
      Caption = Resources.RightMenuAddImageItemText,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_InsertImage.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_InsertImage.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => this.VM.AnnotationToolbar.ImageButtonModel.Command.Execute((object) this.VM.AnnotationToolbar.ImageButtonModel))),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_AddImage",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel18 = new ContextMenuItemModel()
    {
      Name = "Text",
      Caption = Resources.RightMenuAddTextItemText,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_InsertText.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_InsertText.png")),
      Command = (ICommand) new RelayCommand((Action) (() =>
      {
        this.VM.AnnotationToolbar.TextButtonModel.IsChecked = true;
        this.VM.AnnotationToolbar.TextButtonModel.Command.Execute((object) this.VM.AnnotationToolbar.TextButtonModel);
      })),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_AddText",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    ContextMenuItemModel contextMenuItemModel19 = new ContextMenuItemModel()
    {
      Name = "Watermark",
      Caption = Resources.MenuAnnotateWatermarkContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/Watermark.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/Watermark.png")),
      Command = (ICommand) new RelayCommand((Action) (() => this.VM.AnnotationToolbar.DoWatermarkInsertCmd2())),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_CreateWatermark"
    };
    ContextMenuItemModel contextMenuItemModel20 = new ContextMenuItemModel()
    {
      Name = "HeaderAndFooter",
      Caption = Resources.MenuInsertHeaderFooterContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_InsertHeaderAndFooter.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_InsertHeaderAndFooter.png")),
      Command = (ICommand) new RelayCommand((Action) (() => this.VM.PageEditors.AddHeaderFooter2())),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_AddPageHeaderAndFooter"
    };
    ContextMenuItemModel contextMenuItemModel21 = new ContextMenuItemModel()
    {
      Name = "pageNumber",
      Caption = Resources.MenuInsertPageNumberContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/DefaultContextMenu_PageNumber.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/DefaultContextMenu_pageNumber.png")),
      Command = (ICommand) new RelayCommand((Action) (() => this.VM.PageEditors.AddPageNumber2())),
      IsCheckable = false,
      HotKeyInvokeWhen = "Editor_AddPageNumber"
    };
    ContextMenuItemModel contextMenuItemModel22 = new ContextMenuItemModel()
    {
      Name = "ConvertTo",
      Caption = Resources.RightMenuConvertToItemText,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/convert.png"), new Uri("pack://application:,,,/Style/DarkModeResources/converter/convert.png"))
    };
    ContextMenuItemModel contextMenuItemModel23 = new ContextMenuItemModel()
    {
      Name = "PDFtoWord",
      Caption = Resources.MenuConvertPdfToWordContent,
      Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/wordmenu.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => this.VM.ConverterCommands.DoPDFToWord())),
      IsCheckable = false
    };
    ContextMenuItemModel contextMenuItemModel24 = new ContextMenuItemModel()
    {
      Name = "PDFtoExcel",
      Caption = Resources.MenuConvertPdfToExcelContent,
      Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/excelmenu.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => this.VM.ConverterCommands.DoPDFToExcel())),
      IsCheckable = false
    };
    ContextMenuItemModel contextMenuItemModel25 = new ContextMenuItemModel()
    {
      Name = "PDFtoPPT",
      Caption = Resources.MenuConvertPdfToPPTContent,
      Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/pptmenu.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => this.VM.ConverterCommands.DoPDFToPPT())),
      IsCheckable = false
    };
    ContextMenuItemModel contextMenuItemModel26 = new ContextMenuItemModel()
    {
      Name = "PDFtoImage",
      Caption = Resources.MenuConvertPdfToImageContent,
      Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/imagemenu.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => this.VM.ConverterCommands.DoPDFToImage())),
      IsCheckable = false
    };
    ContextMenuItemModel contextMenuItemModel27 = new ContextMenuItemModel()
    {
      Name = "PDFtoJpeg",
      Caption = Resources.MenuConvertPdfToJpegContent,
      Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/imagemenu.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => this.VM.ConverterCommands.DoPDFToJpeg())),
      IsCheckable = false
    };
    ContextMenuItemModel contextMenuItemModel28 = new ContextMenuItemModel()
    {
      Name = "PDFtoText",
      Caption = Resources.MenuConvertPdfToTxtContent,
      Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/txtmenu.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => this.VM.ConverterCommands.DoPDFToTxt())),
      IsCheckable = false
    };
    ContextMenuItemModel contextMenuItemModel29 = new ContextMenuItemModel()
    {
      Name = "PDFtoHtml",
      Caption = Resources.MenuConvertPdfToHtmlContent,
      Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/Htmlmenu.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => this.VM.ConverterCommands.DoPDFToHtml())),
      IsCheckable = false
    };
    ContextMenuItemModel contextMenuItemModel30 = new ContextMenuItemModel()
    {
      Name = "PDFtoRtf",
      Caption = Resources.MenuConvertPdfToRtfContent,
      Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/Rtfmenu.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => this.VM.ConverterCommands.DoPDFToRtf())),
      IsCheckable = false
    };
    ContextMenuItemModel contextMenuItemModel31 = new ContextMenuItemModel()
    {
      Name = "PDFtoXml",
      Caption = Resources.MenuConvertPdfToXmlContent,
      Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/Xmlmenu.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () => this.VM.ConverterCommands.DoPDFToXml())),
      IsCheckable = false
    };
    ContextMenuItemModel contextMenuItemModel32 = new ContextMenuItemModel()
    {
      Name = "DocumentProperties",
      Caption = Resources.DocumentPropertiesWindowTitle,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Properties.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/Properties.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () =>
      {
        CommomLib.Commom.GAManager.SendEvent("DocumentPropertiesWindow", "ShowSource", "ContextMenu", 1L);
        this.VM.ShowProperties();
      })),
      HotKeyInvokeWhen = "Editor_DocumentProperties",
      HotKeyInvokeAction = HotKeyInvokeAction.None
    };
    contextMenuItemModel3.Add((IContextMenuModel) contextMenuItemModel4);
    contextMenuItemModel3.Add((IContextMenuModel) contextMenuItemModel5);
    ContextMenuModel contextMenuModel1 = new ContextMenuModel();
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel8);
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel9);
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel10);
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel11);
    ContextMenuModel contextMenuModel2 = new ContextMenuModel();
    contextMenuModel2.Add((IContextMenuModel) contextMenuItemModel17);
    contextMenuModel2.Add((IContextMenuModel) contextMenuItemModel18);
    contextMenuModel2.Add((IContextMenuModel) new ContextMenuSeparator());
    contextMenuModel2.Add((IContextMenuModel) contextMenuItemModel19);
    contextMenuModel2.Add((IContextMenuModel) contextMenuItemModel20);
    contextMenuModel2.Add((IContextMenuModel) contextMenuItemModel21);
    ContextMenuModel contextMenuModel3 = contextMenuModel2;
    ContextMenuModel contextMenuModel4 = new ContextMenuModel();
    contextMenuModel4.Add((IContextMenuModel) contextMenuItemModel23);
    contextMenuModel4.Add((IContextMenuModel) contextMenuItemModel24);
    contextMenuModel4.Add((IContextMenuModel) contextMenuItemModel25);
    contextMenuModel4.Add((IContextMenuModel) contextMenuItemModel26);
    contextMenuModel4.Add((IContextMenuModel) contextMenuItemModel27);
    contextMenuModel4.Add((IContextMenuModel) contextMenuItemModel28);
    contextMenuModel4.Add((IContextMenuModel) contextMenuItemModel29);
    contextMenuModel4.Add((IContextMenuModel) contextMenuItemModel30);
    contextMenuModel4.Add((IContextMenuModel) contextMenuItemModel31);
    ContextMenuModel contextMenuModel5 = contextMenuModel4;
    foreach (IContextMenuModel contextMenuModel6 in (Collection<IContextMenuModel>) contextMenuModel1)
      contextMenuItemModel7.Add(contextMenuModel6);
    foreach (IContextMenuModel contextMenuModel7 in (Collection<IContextMenuModel>) contextMenuModel3)
      contextMenuItemModel16.Add(contextMenuModel7);
    foreach (IContextMenuModel contextMenuModel8 in (Collection<IContextMenuModel>) contextMenuModel5)
      contextMenuItemModel22.Add(contextMenuModel8);
    ContextMenuModel contextMenuModel9 = new ContextMenuModel();
    contextMenuModel9.Add((IContextMenuModel) horizontalButton);
    contextMenuModel9.Add((IContextMenuModel) new ContextMenuSeparator());
    contextMenuModel9.Add((IContextMenuModel) contextMenuItemModel1);
    contextMenuModel9.Add((IContextMenuModel) contextMenuItemModel2);
    contextMenuModel9.Add((IContextMenuModel) contextMenuItemModel3);
    contextMenuModel9.Add((IContextMenuModel) contextMenuItemModel6);
    contextMenuModel9.Add((IContextMenuModel) new ContextMenuSeparator());
    contextMenuModel9.Add((IContextMenuModel) contextMenuItemModel7);
    contextMenuModel9.Add((IContextMenuModel) contextMenuItemModel13);
    contextMenuModel9.Add((IContextMenuModel) contextMenuItemModel12);
    contextMenuModel9.Add((IContextMenuModel) contextMenuItemModel14);
    contextMenuModel9.Add((IContextMenuModel) contextMenuItemModel15);
    contextMenuModel9.Add((IContextMenuModel) new ContextMenuSeparator());
    contextMenuModel9.Add((IContextMenuModel) contextMenuItemModel16);
    contextMenuModel9.Add((IContextMenuModel) contextMenuItemModel22);
    contextMenuModel9.Add((IContextMenuModel) new ContextMenuSeparator());
    contextMenuModel9.Add((IContextMenuModel) contextMenuItemModel32);
    ContextMenuModel contextMenuModel10 = contextMenuModel9;
    PdfViewerContextMenu viewerContextMenu = new PdfViewerContextMenu();
    viewerContextMenu.ItemsSource = (IEnumerable) contextMenuModel10;
    viewerContextMenu.PlacementTarget = (UIElement) this.annotationCanvas;
    viewerContextMenu.AutoCloseOnMouseLeave = false;
    this.contextMenu = viewerContextMenu;
  }

  public bool Show()
  {
    if (this.annotationCanvas.PdfViewer?.Document == null)
      return false;
    PdfViewer pdfViewer = this.annotationCanvas.PdfViewer;
    if ((pdfViewer != null ? (pdfViewer.MouseMode == MouseModes.PanTool ? 1 : 0) : 0) != 0 || this.annotationCanvas.HasSelectedText() || (PdfWrapper) this.annotationCanvas.SelectedAnnotation != (PdfWrapper) null)
      return false;
    this.contextMenu.Placement = PlacementMode.MousePoint;
    return this.contextMenu.IsOpen = true;
  }

  public void Hide()
  {
    this.contextMenu.IsOpen = false;
    this.contextMenu.Placement = PlacementMode.Absolute;
  }

  private static bool IsDesignMode
  {
    get
    {
      return (bool) DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof (DependencyObject)).DefaultValue;
    }
  }
}
