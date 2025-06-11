// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PdfViewerDecorators.LinkRightMenu
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using pdfeditor.Models.Menus;
using pdfeditor.Properties;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit;
using System;
using System.Collections;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable enable
namespace pdfeditor.Controls.PdfViewerDecorators;

internal class LinkRightMenu
{
  private readonly 
  #nullable disable
  AnnotationCanvas annotationCanvas;
  private PdfViewerContextMenu contextMenu;
  private PdfLinkAnnotation linkAnnot;
  private PdfDocument Document;

  public LinkRightMenu(
    AnnotationCanvas annotationCanvas,
    PdfLinkAnnotation link,
    PdfDocument document)
  {
    this.annotationCanvas = annotationCanvas ?? throw new ArgumentNullException(nameof (annotationCanvas));
    this.linkAnnot = link;
    this.Document = document;
    this.InitContextMenu();
  }

  private MainViewModel VM => this.annotationCanvas.DataContext as MainViewModel;

  private void InitContextMenu()
  {
    if (LinkRightMenu.IsDesignMode)
      return;
    ContextMenuItemModel contextMenuItemModel1 = new ContextMenuItemModel()
    {
      Name = "Edit Link",
      Caption = Resources.LinkWinTitleEdit,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/LinkCE.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/LinkCE.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () =>
      {
        CommomLib.Commom.GAManager.SendEvent("PDFLink", "EditBtnClick", "ContextMenu", 1L);
        LinkAnnotationUtils.LinkAnnotationop(this.linkAnnot, this.Document, this.linkAnnot.Page, Ioc.Default.GetRequiredService<MainViewModel>().ViewToolbar.DocZoom, this.VM);
      }))
    };
    ContextMenuItemModel contextMenuItemModel2 = new ContextMenuItemModel()
    {
      Name = "Delete Link",
      Caption = Resources.LinkRightDelete,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/Select_AnnotDelete.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/Select_AnnotDelete.png")),
      Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () =>
      {
        CommomLib.Commom.GAManager.SendEvent("PDFLink", "DeleteBtnClick", "ContextMenu", 1L);
        if (MessageBox.Show(Resources.LinkDeleteOne, UtilManager.GetProductName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
          return;
        await this.VM.DeleteSelectedAnnotCmd.ExecuteAsync((object) null);
      }))
    };
    ContextMenuModel contextMenuModel1 = new ContextMenuModel();
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel1);
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel2);
    ContextMenuModel contextMenuModel2 = contextMenuModel1;
    PdfViewerContextMenu viewerContextMenu = new PdfViewerContextMenu();
    viewerContextMenu.ItemsSource = (IEnumerable) contextMenuModel2;
    viewerContextMenu.PlacementTarget = (UIElement) this.annotationCanvas;
    viewerContextMenu.AutoCloseOnMouseLeave = false;
    this.contextMenu = viewerContextMenu;
  }

  public bool Show()
  {
    if (this.annotationCanvas.PdfViewer?.Document == null)
      return false;
    PdfViewer pdfViewer = this.annotationCanvas.PdfViewer;
    if ((pdfViewer != null ? (pdfViewer.MouseMode == MouseModes.PanTool ? 1 : 0) : 0) != 0 || this.annotationCanvas.HasSelectedText() || !(this.annotationCanvas.SelectedAnnotation is PdfLinkAnnotation))
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
