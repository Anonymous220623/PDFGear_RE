// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PdfViewerDecorators.AnnotationContextMenuHolder
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using Microsoft.Toolkit.Mvvm.Input;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Models.Menus;
using pdfeditor.Properties;
using pdfeditor.Utils;
using PDFKit;
using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Controls.PdfViewerDecorators;

internal class AnnotationContextMenuHolder
{
  private readonly AnnotationCanvas annotationCanvas;
  private PdfViewerContextMenu selectAnnotationContextMenu;
  private ContextMenuModel contextMenus;
  private ContextMenuItemModel duplicateAnnotItem;
  private ContextMenuItemModel deleteAnnotItem;
  private ContextMenuItemModel copyTextItem;
  private ContextMenuItemModel deleteTextItem;

  public AnnotationContextMenuHolder(AnnotationCanvas annotationCanvas)
  {
    this.annotationCanvas = annotationCanvas ?? throw new ArgumentNullException(nameof (annotationCanvas));
    this.InitContextMenu();
  }

  private void InitContextMenu()
  {
    if (AnnotationContextMenuHolder.IsDesignMode)
      return;
    this.duplicateAnnotItem = new ContextMenuItemModel()
    {
      Name = "Duplicate",
      Caption = Resources.MenuRightAnnotateDuplicate,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/Select_Duplicate.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/Select_Duplicate.png")),
      Command = (ICommand) new AsyncRelayCommand<ContextMenuItemModel>(new Func<ContextMenuItemModel, Task>(this.OnContextDuplicateAnnotation))
    };
    this.copyTextItem = new ContextMenuItemModel()
    {
      Name = "CopyText",
      Caption = Resources.AnnotationContextMenuCopyText,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/Select_Copy.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/Select_Copy.png")),
      Command = (ICommand) new AsyncRelayCommand<ContextMenuItemModel>(new Func<ContextMenuItemModel, Task>(this.OnContextCopyAnnotationText))
    };
    this.deleteAnnotItem = new ContextMenuItemModel()
    {
      Name = "Delete",
      Caption = Resources.RightMenuDeleteAnnotationItemText,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/Select_AnnotDelete.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/Select_AnnotDelete.png")),
      Command = (ICommand) new AsyncRelayCommand<ContextMenuItemModel>(new Func<ContextMenuItemModel, Task>(this.OnContextDeleteAnnotation)),
      HotKeyString = "Delete"
    };
    this.deleteTextItem = new ContextMenuItemModel()
    {
      Name = "DeleteText",
      Caption = Resources.RightMenuDeleteTextItemText,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/Select_DeleteText.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/Select_DeleteText.png"))
    };
    ContextMenuModel contextMenuModel = new ContextMenuModel();
    contextMenuModel.Add((IContextMenuModel) this.duplicateAnnotItem);
    contextMenuModel.Add((IContextMenuModel) this.deleteAnnotItem);
    this.contextMenus = contextMenuModel;
    PdfViewerContextMenu viewerContextMenu = new PdfViewerContextMenu();
    viewerContextMenu.ItemsSource = (IEnumerable) this.contextMenus;
    viewerContextMenu.PlacementTarget = (UIElement) this.annotationCanvas;
    viewerContextMenu.AutoCloseOnMouseLeave = false;
    this.selectAnnotationContextMenu = viewerContextMenu;
  }

  private async Task OnContextDuplicateAnnotation(ContextMenuItemModel model)
  {
    PdfAnnotation selectedAnnotation = this.annotationCanvas.SelectedAnnotation;
    if (!AnnotationContextMenuHolder.CanDuplicate(this.annotationCanvas.SelectedAnnotation))
      return;
    CommomLib.Commom.GAManager.SendEvent("ContextRightMenu", "DuplicateAnnotation", "Count", 1L);
    PdfAnnotation annotation = await selectedAnnotation.DuplicateAnnotationAsync();
    if (!((PdfWrapper) annotation != (PdfWrapper) null))
      return;
    this.annotationCanvas.HolderManager.Select(annotation, false);
    try
    {
      if (annotation is PdfHighlightAnnotation highlightAnnotation)
      {
        if (!string.IsNullOrWhiteSpace(highlightAnnotation.Subject) && highlightAnnotation.Subject == "AreaHighlight")
          CommomLib.Commom.GAManager.SendEvent("AnnotationAction", "PdfAreaHighlightAnnotation", "Duplicate", 1L);
        else
          CommomLib.Commom.GAManager.SendEvent("AnnotationAction", "PdfHighlightAnnotation", "Duplicate", 1L);
      }
      if (annotation is PdfStrikeoutAnnotation)
        CommomLib.Commom.GAManager.SendEvent("AnnotationAction", "PdfStrikeoutAnnotation", "Duplicate", 1L);
      if (annotation is PdfUnderlineAnnotation)
        CommomLib.Commom.GAManager.SendEvent("AnnotationAction", "PdfUnderlineAnnotation", "Duplicate", 1L);
      if (annotation is PdfLineAnnotation)
        CommomLib.Commom.GAManager.SendEvent("AnnotationAction", "PdfLineAnnotation", "Duplicate", 1L);
      if (annotation is PdfSquareAnnotation)
        CommomLib.Commom.GAManager.SendEvent("AnnotationAction", "PdfSquareAnnotation", "Duplicate", 1L);
      if (annotation is PdfCircleAnnotation)
        CommomLib.Commom.GAManager.SendEvent("AnnotationAction", "PdfCircleAnnotation", "Duplicate", 1L);
      if (annotation is PdfInkAnnotation)
        CommomLib.Commom.GAManager.SendEvent("AnnotationAction", "PdfInkAnnotation", "Duplicate", 1L);
      if (annotation is PdfFreeTextAnnotation freeTextAnnotation)
      {
        if (freeTextAnnotation.Intent == AnnotationIntent.FreeTextTypeWriter)
          CommomLib.Commom.GAManager.SendEvent("AnnotationAction", "PdfFreeTextAnnotationTransparent", "Duplicate", 1L);
        else
          CommomLib.Commom.GAManager.SendEvent("AnnotationAction", "PdfFreeTextAnnotation", "Duplicate", 1L);
      }
      if (annotation is PdfTextAnnotation)
        CommomLib.Commom.GAManager.SendEvent("AnnotationAction", "PdfTextAnnotation", "Duplicate", 1L);
      if (!(annotation is PdfStampAnnotation pdfStampAnnotation))
        return;
      if (!string.IsNullOrWhiteSpace(pdfStampAnnotation.Subject) && pdfStampAnnotation.Subject == "Signature")
        CommomLib.Commom.GAManager.SendEvent("AnnotationAction", "PdfStampAnnotationSignature", "Duplicate", 1L);
      else if (!string.IsNullOrWhiteSpace(pdfStampAnnotation.Subject) && pdfStampAnnotation.Subject == "FormControl")
        CommomLib.Commom.GAManager.SendEvent("AnnotationAction", "PdfStampAnnotationForm", "Duplicate", 1L);
      else
        CommomLib.Commom.GAManager.SendEvent("AnnotationAction", "PdfStampAnnotation", "Duplicate", 1L);
    }
    catch
    {
    }
  }

  private async Task OnContextDeleteAnnotation(ContextMenuItemModel model)
  {
    PdfAnnotation selectedAnnot = this.annotationCanvas.SelectedAnnotation;
    if (!((PdfWrapper) selectedAnnot != (PdfWrapper) null))
    {
      selectedAnnot = (PdfAnnotation) null;
    }
    else
    {
      CommomLib.Commom.GAManager.SendEvent("ContextRightMenu", "DeleteAnnotation", "Count", 1L);
      PdfPage page = selectedAnnot.Page;
      this.annotationCanvas.HolderManager.CancelAll();
      await this.annotationCanvas.HolderManager.WaitForCancelCompletedAsync();
      await this.annotationCanvas.HolderManager.DeleteAnnotationAsync(selectedAnnot);
      selectedAnnot = (PdfAnnotation) null;
    }
  }

  private async Task OnContextCopyAnnotationText(ContextMenuItemModel model)
  {
    if (!(this.annotationCanvas.SelectedAnnotation is PdfTextMarkupAnnotation selectedAnnotation))
      return;
    StringBuilder sb = new StringBuilder();
    PdfText text = selectedAnnotation.Page.Text;
    foreach (FS_QUADPOINTSF quadPoint in selectedAnnotation.QuadPoints)
    {
      FS_RECTF pdfRect = quadPoint.ToPdfRect();
      sb.AppendLine(text.GetBoundedText(pdfRect.left - 2f, pdfRect.top + 2f, pdfRect.right - 2f, pdfRect.bottom - 2f));
    }
    for (int i = 0; i < 3; ++i)
    {
      int num = 0;
      try
      {
        Clipboard.SetDataObject((object) sb.ToString());
      }
      catch
      {
        num = 1;
      }
      if (num == 1 && i != 2)
        await Task.Delay(100);
    }
    sb = (StringBuilder) null;
  }

  public async Task<bool> ShowAsync()
  {
    PdfViewer pdfViewer = this.annotationCanvas.PdfViewer;
    if ((pdfViewer != null ? (pdfViewer.MouseMode == MouseModes.PanTool ? 1 : 0) : 0) != 0)
      return false;
    await Task.Delay(50);
    if (!AnnotationContextMenuHolder.CanShow(this.annotationCanvas.SelectedAnnotation))
      return false;
    this.annotationCanvas.PdfViewer?.DeselectText();
    if (this.selectAnnotationContextMenu.IsOpen)
      return true;
    this.contextMenus.Remove((IContextMenuModel) this.duplicateAnnotItem);
    if (AnnotationContextMenuHolder.CanDuplicate(this.annotationCanvas.SelectedAnnotation))
      this.contextMenus.Insert(0, (IContextMenuModel) this.duplicateAnnotItem);
    this.contextMenus.Remove((IContextMenuModel) this.copyTextItem);
    if (AnnotationContextMenuHolder.CanCopyOrDeleteText(this.annotationCanvas.SelectedAnnotation))
    {
      this.contextMenus.Remove((IContextMenuModel) this.deleteAnnotItem);
      this.contextMenus.Add((IContextMenuModel) this.copyTextItem);
      this.contextMenus.Add((IContextMenuModel) this.deleteAnnotItem);
    }
    this.selectAnnotationContextMenu.IsOpen = true;
    return true;
  }

  public void Hide() => this.selectAnnotationContextMenu.IsOpen = false;

  private static bool IsDesignMode
  {
    get
    {
      return (bool) DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof (DependencyObject)).DefaultValue;
    }
  }

  private static bool CanShow(PdfAnnotation annot)
  {
    return !((PdfWrapper) annot == (PdfWrapper) null) && (!(annot is PdfStampAnnotation pdfStampAnnotation) || !(pdfStampAnnotation.Subject == "Signature"));
  }

  private static bool CanDuplicate(PdfAnnotation annot)
  {
    if ((PdfWrapper) annot == (PdfWrapper) null)
      return false;
    switch (annot)
    {
      case PdfTextMarkupAnnotation _:
        return false;
      case PdfTextAnnotation _:
        return false;
      case PdfStampAnnotation pdfStampAnnotation:
        if (pdfStampAnnotation.Subject == "Signature")
          return false;
        break;
    }
    return true;
  }

  private static bool CanCopyOrDeleteText(PdfAnnotation annot) => annot is PdfTextMarkupAnnotation;
}
