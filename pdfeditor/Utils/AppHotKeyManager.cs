// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.AppHotKeyManager
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom.HotKeys;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Patagames.Pdf.Net;
using pdfeditor.Controls;
using pdfeditor.Models.Bookmarks;
using pdfeditor.Models.Thumbnails;
using pdfeditor.Properties;
using pdfeditor.ViewModels;
using pdfeditor.Views;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Utils;

public class AppHotKeyManager
{
  private static bool initialized;

  public AppHotKeyManager()
  {
    AppHotKeyManager.InitializeHotKeys();
    this.UpdateHotKeyEnabledStates();
  }

  public void UpdateHotKeyEnabledStates()
  {
    PDFKit.PdfControl pdfControl;
    if (!AppHotKeyManager.TryGetViewModelAndPdfControl(out MainViewModel _, out pdfControl))
      return;
    bool isEditing = pdfControl.IsEditing;
    bool isVisible = pdfControl.Viewer.IsVisible;
    HotKeyManager.GetOrCreate("Editor_CreateNewPDF").IsEnabled = !isEditing;
    HotKeyManager.GetOrCreate("Editor_Open").IsEnabled = !isEditing;
    HotKeyManager.GetOrCreate("Editor_Print").IsEnabled = !isEditing;
    HotKeyManager.GetOrCreate("Editor_PreviousView").IsEnabled = isVisible | isEditing;
    HotKeyManager.GetOrCreate("Editor_NextView").IsEnabled = isVisible | isEditing;
    HotKeyManager.GetOrCreate("Editor_FitPage").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_FitPage2").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_ActualSize").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_ActualSize2").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_FitWidth").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_FitWidth2").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_FitHeight").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_FitHeight2").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_SinglePage").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_SinglePage2").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_DoublePage").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_DoublePage2").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_Continuous").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_Continuous2").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_AutoScroll").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_Screenshot").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_Highlight").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_Underline").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_Strikethrough").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_AreaHighlight").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_Line").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_Rectangle").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_Oval").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_Ink").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_TextBox").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_Note").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_ManageAnnotation").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_HighlightFormField").IsEnabled = isVisible;
    HotKeyManager.GetOrCreate("Editor_Save").IsEnabled = !isEditing;
    HotKeyManager.GetOrCreate("Editor_SaveAs").IsEnabled = !isEditing;
  }

  private static void InitializeHotKeys()
  {
    if (AppHotKeyManager.initialized)
      return;
    AppHotKeyManager.initialized = true;
    HotKeyManager.GetOrCreate("Editor_CreateNewPDF", new HotKeyItem(Key.N, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextCreateNewPDF;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_Open", new HotKeyItem(Key.O, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextOpenDocument;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_Save", new HotKeyItem(Key.S, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextSaveDocument;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_SaveAs", new HotKeyItem(Key.S, ModifierKeys.Control | ModifierKeys.Shift), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextSaveAsDocument;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_Print", new HotKeyItem(Key.P, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextPrint;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_Undo", new HotKeyItem(Key.Z, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextUndo;
      m.IsReadOnly = false;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) ((vm, pdfControl) =>
      {
        AnnotationCanvas annotationCanvas = PdfObjectExtensions.GetAnnotationCanvas(pdfControl);
        if (annotationCanvas.ScreenshotDialog.Visibility == Visibility.Visible)
          annotationCanvas.ScreenshotDialog?.UndoDrawControl();
        else
          vm.QuickToolUndoModel.Command.Execute((object) null);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_Redo", new HotKeyItem(Key.Y, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextRedo;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_Redo2", new HotKeyItem(Key.Z, ModifierKeys.Control | ModifierKeys.Shift), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextRedo;
      m.IsReadOnly = false;
      m.IsVisible = false;
    }));
    HotKeyManager.GetOrCreate("Editor_AddBookmark", new HotKeyItem(Key.B, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextAddBookmark;
      m.IsReadOnly = false;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) (async (vm, pdfControl) =>
      {
        if (!pdfControl.Viewer.IsVisible)
          return;
        await vm.BookmarkAddCommand.ExecuteAsync((BookmarkModel) null);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_Find", new HotKeyItem(Key.F, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextFind;
      m.IsReadOnly = false;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) ((vm, pdfControl) =>
      {
        if (!pdfControl.Viewer.IsVisible)
          return;
        Application.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>()?.ShowSearchBox();
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_DocumentProperties", new HotKeyItem(Key.D, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextDocumentProperties;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_CloseDocument", new HotKeyItem(Key.W, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextCloseDocument;
      m.IsReadOnly = false;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) ((vm, pdfControl) => Application.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>()?.Close()))));
    }));
    HotKeyManager.GetOrCreate("Editor_PreviousView", new HotKeyItem(Key.Left, ModifierKeys.Alt), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextPreviousView;
      m.IsReadOnly = false;
      m.AllowRepeat = true;
    }));
    HotKeyManager.GetOrCreate("Editor_NextView", new HotKeyItem(Key.Right, ModifierKeys.Alt), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextNextView;
      m.IsReadOnly = false;
      m.AllowRepeat = true;
    }));
    HotKeyManager.GetOrCreate("Editor_ZoomIn", new HotKeyItem(Key.Add, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextZoomIn;
      m.IsReadOnly = false;
      m.AllowRepeat = true;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) ((vm, pdfControl) =>
      {
        if (pdfControl.IsVisible)
          vm?.ViewToolbar.DocZoomInCmd.Execute((object) null);
        else
          vm?.PageEditors.PageEditorZoomInCmd.Execute((object) null);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_ZoomIn2", new HotKeyItem(Key.OemPlus, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextZoomIn;
      m.IsReadOnly = false;
      m.IsVisible = false;
      m.AllowRepeat = true;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) ((vm, pdfControl) =>
      {
        if (pdfControl.IsVisible)
          vm?.ViewToolbar.DocZoomInCmd.Execute((object) null);
        else
          vm?.PageEditors.PageEditorZoomInCmd.Execute((object) null);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_ZoomOut", new HotKeyItem(Key.Subtract, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextZoomOut;
      m.IsReadOnly = false;
      m.AllowRepeat = true;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) ((vm, pdfControl) =>
      {
        if (pdfControl.IsVisible)
          vm?.ViewToolbar.DocZoomOutCmd.Execute((object) null);
        else
          vm?.PageEditors.PageEditorZoomOutCmd.Execute((object) null);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_ZoomOut2", new HotKeyItem(Key.OemMinus, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextZoomOut;
      m.IsReadOnly = false;
      m.IsVisible = false;
      m.AllowRepeat = true;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) ((vm, pdfControl) =>
      {
        if (pdfControl.IsVisible)
          vm?.ViewToolbar.DocZoomOutCmd.Execute((object) null);
        else
          vm?.PageEditors.PageEditorZoomOutCmd.Execute((object) null);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_FitPage", new HotKeyItem(Key.D0, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextFitPage;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_FitPage2", new HotKeyItem(Key.NumPad0, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextFitPage;
      m.IsReadOnly = false;
      m.IsVisible = false;
    }));
    HotKeyManager.GetOrCreate("Editor_ActualSize", new HotKeyItem(Key.D1, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextFullSize;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_ActualSize2", new HotKeyItem(Key.NumPad1, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextFullSize;
      m.IsReadOnly = false;
      m.IsVisible = false;
    }));
    HotKeyManager.GetOrCreate("Editor_FitWidth", new HotKeyItem(Key.D2, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextFitWidth;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_FitWidth2", new HotKeyItem(Key.NumPad2, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextFitWidth;
      m.IsReadOnly = false;
      m.IsVisible = false;
    }));
    HotKeyManager.GetOrCreate("Editor_FitHeight", new HotKeyItem(Key.D3, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextFitHeight;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_FitHeight2", new HotKeyItem(Key.NumPad3, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextFitHeight;
      m.IsReadOnly = false;
      m.IsVisible = false;
    }));
    HotKeyManager.GetOrCreate("Editor_SinglePage", new HotKeyItem(Key.D4, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextSinglePage;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_SinglePage2", new HotKeyItem(Key.NumPad4, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextSinglePage;
      m.IsReadOnly = false;
      m.IsVisible = false;
    }));
    HotKeyManager.GetOrCreate("Editor_DoublePage", new HotKeyItem(Key.D5, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextDoublePage;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_DoublePage2", new HotKeyItem(Key.NumPad5, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextDoublePage;
      m.IsReadOnly = false;
      m.IsVisible = false;
    }));
    HotKeyManager.GetOrCreate("Editor_Continuous", new HotKeyItem(Key.D6, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextContinusRead;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_Continuous2", new HotKeyItem(Key.NumPad6, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextContinusRead;
      m.IsReadOnly = false;
      m.IsVisible = false;
    }));
    HotKeyManager.GetOrCreate("Editor_RotateLeft", new HotKeyItem(Key.Left, ModifierKeys.Control | ModifierKeys.Shift), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextRotateLeft;
      m.IsReadOnly = false;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) ((vm, pdfControl) =>
      {
        if (pdfControl.IsEditing)
          return;
        if (pdfControl.IsVisible)
          vm?.ViewToolbar.PageRotateLeftCmd.Execute((object) null);
        else
          vm?.PageEditors.PageEditorRotateLeftCmd.Execute((PdfPageEditListModel) null);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_RotateRight", new HotKeyItem(Key.Right, ModifierKeys.Control | ModifierKeys.Shift), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextRotateRight;
      m.IsReadOnly = false;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) ((vm, pdfControl) =>
      {
        if (pdfControl.IsEditing)
          return;
        if (pdfControl.IsVisible)
          vm?.ViewToolbar.PageRotateRightCmd.Execute((object) null);
        else
          vm?.PageEditors.PageEditorRotateRightCmd.Execute((PdfPageEditListModel) null);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_AutoScroll", new HotKeyItem(Key.H, ModifierKeys.Control | ModifierKeys.Shift), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextAutoScroll;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_Ocr", new HotKeyItem(Key.O, ModifierKeys.Control | ModifierKeys.Shift), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextOcrPages;
      m.IsReadOnly = false;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) ((vm, pdfControl) =>
      {
        if (!pdfControl.Viewer.IsVisible)
          return;
        MainView mainView = App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
        mainView.ocrBtn.IsChecked = new bool?(true);
        mainView.ocrBtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_Screenshot", new HotKeyItem(Key.X, ModifierKeys.Alt | ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextScreenshot;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_Highlight", new HotKeyItem(Key.H, ModifierKeys.Alt), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextHighlight;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_Underline", new HotKeyItem(Key.U, ModifierKeys.Alt), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextUnderline;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_Strikethrough", new HotKeyItem(Key.S, ModifierKeys.Alt), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextStrike;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_AreaHighlight", new HotKeyItem(Key.A, ModifierKeys.Alt), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextAreaHighlight;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_Line", new HotKeyItem(Key.L, ModifierKeys.Alt), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextAnnotateLine;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_Rectangle", new HotKeyItem(Key.R, ModifierKeys.Alt), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextAnnotateShape;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_Oval", new HotKeyItem(Key.O, ModifierKeys.Alt), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextAnnotateEllipse;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_Ink", new HotKeyItem(Key.I, ModifierKeys.Alt), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextAnnotateInk;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_TextBox", new HotKeyItem(Key.B, ModifierKeys.Alt), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextAnnotateTextBox;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_AddText", new HotKeyItem(Key.T, ModifierKeys.Alt), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextAnnotateTypeWriter;
      m.IsReadOnly = false;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) ((vm, pdfControl) =>
      {
        if (!pdfControl.Viewer.IsVisible)
          return;
        if (vm.AnnotationToolbar.TextButtonModel.IsChecked)
          vm.AnnotationToolbar.TextButtonModel.IsChecked = false;
        else
          vm.AnnotationToolbar.TextButtonModel.IsChecked = true;
        vm.AnnotationToolbar.TextButtonModel.Command.Execute((object) vm.AnnotationToolbar.TextButtonModel);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_Note", new HotKeyItem(Key.N, ModifierKeys.Alt), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextAnnotateNote;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_ShowHideComments", new HotKeyItem(Key.S, ModifierKeys.Alt | ModifierKeys.Shift), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextShowHideComments;
      m.IsReadOnly = true;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) ((vm, pdfControl) =>
      {
        if (!pdfControl.Viewer.IsVisible)
          return;
        vm.ShowHideAnnotationCmd.Execute((object) null);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_ManageAnnotation", new HotKeyItem(Key.M, ModifierKeys.Alt | ModifierKeys.Shift), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextAnnotateManage;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_AddImage", new HotKeyItem(Key.I, ModifierKeys.Alt | ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextInsertImage;
      m.IsReadOnly = false;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) ((vm, pdfControl) =>
      {
        if (!pdfControl.Viewer.IsVisible)
          return;
        vm.AnnotationToolbar.ImageButtonModel.Command.Execute((object) vm.AnnotationToolbar.ImageButtonModel);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_HighlightFormField", new HotKeyItem(Key.H, ModifierKeys.Control), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextHighlightForm;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_ExtractPage", new HotKeyItem(Key.E, ModifierKeys.Control | ModifierKeys.Shift), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextPageExtract;
      m.IsReadOnly = false;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) (async (vm, pdfControl) =>
      {
        if (pdfControl.IsEditing)
          return;
        if (pdfControl.IsVisible)
          await vm.PageEditors.PageEditorExtractCmd.ExecuteAsync(new PdfPageEditListModel(vm.Document, vm.SelectedPageIndex));
        else
          await vm.PageEditors.PageEditorExtractCmd.ExecuteAsync((PdfPageEditListModel) null);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_DeletePage", new HotKeyItem(Key.D, ModifierKeys.Control | ModifierKeys.Shift), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextPageDelete;
      m.IsReadOnly = false;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) (async (vm, pdfControl) =>
      {
        if (pdfControl.IsEditing)
          return;
        if (pdfControl.IsVisible)
          await vm.PageEditors.PageEditorDeleteCmd.ExecuteAsync(new PdfPageEditListModel(vm.Document, vm.SelectedPageIndex));
        else
          await vm.PageEditors.PageEditorDeleteCmd.ExecuteAsync((PdfPageEditListModel) null);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_InsertBlankPage", new HotKeyItem(Key.B, ModifierKeys.Control | ModifierKeys.Shift), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextInsertPageSubBlankPage;
      m.IsReadOnly = false;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) (async (vm, pdfControl) =>
      {
        if (pdfControl.IsEditing)
          return;
        if (pdfControl.IsVisible)
          await vm.PageEditors.PageEditorInsertBlankCmd.ExecuteAsync(new PdfPageEditListModel(vm.Document, vm.SelectedPageIndex));
        else
          await vm.PageEditors.PageEditorInsertBlankCmd.ExecuteAsync((PdfPageEditListModel) null);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_InsertPDF", new HotKeyItem(Key.P, ModifierKeys.Control | ModifierKeys.Shift), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextInsertPageSubFromPDF;
      m.IsReadOnly = false;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) (async (vm, pdfControl) =>
      {
        if (pdfControl.IsEditing)
          return;
        if (pdfControl.IsVisible)
          await vm.PageEditors.PageEditorInsertFromPDFCmd.ExecuteAsync(new PdfPageEditListModel(vm.Document, vm.SelectedPageIndex));
        else
          await vm.PageEditors.PageEditorInsertFromPDFCmd.ExecuteAsync((PdfPageEditListModel) null);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_InsertWord", new HotKeyItem(Key.W, ModifierKeys.Control | ModifierKeys.Shift), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextInsertPageSubFromWord;
      m.IsReadOnly = false;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) (async (vm, pdfControl) =>
      {
        if (pdfControl.IsEditing)
          return;
        if (pdfControl.IsVisible)
          await vm.PageEditors.PageEditorInsertFromWordCmd.ExecuteAsync(new PdfPageEditListModel(vm.Document, vm.SelectedPageIndex));
        else
          await vm.PageEditors.PageEditorInsertFromWordCmd.ExecuteAsync((PdfPageEditListModel) null);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_InsertImage", new HotKeyItem(Key.I, ModifierKeys.Control | ModifierKeys.Shift), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextInsertPageSubFromImage;
      m.IsReadOnly = false;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) (async (vm, pdfControl) =>
      {
        if (pdfControl.IsEditing)
          return;
        if (pdfControl.IsVisible)
          await vm.PageEditors.PageEditorInsertFromImageCmd.ExecuteAsync(new PdfPageEditListModel(vm.Document, vm.SelectedPageIndex));
        else
          await vm.PageEditors.PageEditorInsertFromImageCmd.ExecuteAsync((PdfPageEditListModel) null);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_CropPage", new HotKeyItem(Key.C, ModifierKeys.Control | ModifierKeys.Shift), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextCropPage;
      m.IsReadOnly = false;
      m.Command = (ICommand) new RelayCommand((Action) (() => AppHotKeyManager.InvokeHotKeyCommand((Action<MainViewModel, PDFKit.PdfControl>) ((vm, pdfControl) =>
      {
        if (pdfControl.IsEditing)
          return;
        if (pdfControl.IsVisible)
        {
          PdfPageEditListModel parameter = new PdfPageEditListModel(vm.Document, vm.SelectedPageIndex);
          vm.PageEditors.CropPageCmd2.Execute(parameter);
        }
        else
          vm.PageEditors.CropPageCmd.Execute((object) null);
      }))));
    }));
    HotKeyManager.GetOrCreate("Editor_Present", new HotKeyItem(Key.F5), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextPresent;
      m.IsReadOnly = false;
    }));
    HotKeyManager.GetOrCreate("Editor_FullScreen", new HotKeyItem(Key.F11), (Action<HotKeyModel>) (m =>
    {
      m.DisplayName = Resources.ShortcutTextFullScreen;
      m.IsReadOnly = false;
    }));
  }

  private static bool TryGetViewModelAndPdfControl(
    out MainViewModel mainViewModel,
    out PDFKit.PdfControl pdfControl)
  {
    pdfControl = (PDFKit.PdfControl) null;
    mainViewModel = Ioc.Default.GetService<MainViewModel>();
    if (mainViewModel != null)
    {
      PdfDocument document = mainViewModel.Document;
      if (document != null)
        pdfControl = PDFKit.PdfControl.GetPdfControl(document);
    }
    return pdfControl != null;
  }

  private static void InvokeHotKeyCommand(
    Action<MainViewModel, PDFKit.PdfControl> pdfControlVisible)
  {
    MainViewModel mainViewModel;
    PDFKit.PdfControl pdfControl;
    if (!AppHotKeyManager.TryGetViewModelAndPdfControl(out mainViewModel, out pdfControl) || !HotKeyExtensions.IsWindowEnabled((UIElement) pdfControl) || pdfControlVisible == null)
      return;
    pdfControlVisible(mainViewModel, pdfControl);
  }
}
