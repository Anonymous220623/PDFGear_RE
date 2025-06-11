// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PdfViewerDecorators.TextObjectContextMenuHolder
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using Microsoft.Toolkit.Mvvm.Input;
using pdfeditor.Models.Menus;
using pdfeditor.Properties;
using PDFKit;
using System;
using System.Collections;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.PdfViewerDecorators;

internal class TextObjectContextMenuHolder
{
  private readonly AnnotationCanvas annotationCanvas;
  private PdfViewerContextMenu textObjectContextMenu;

  public TextObjectContextMenuHolder(AnnotationCanvas annotationCanvas)
  {
    this.annotationCanvas = annotationCanvas ?? throw new ArgumentNullException(nameof (annotationCanvas));
    this.InitContextMenu();
  }

  public bool IsOpen => this.textObjectContextMenu.IsOpen;

  private void InitContextMenu()
  {
    if (TextObjectContextMenuHolder.IsDesignMode)
      return;
    ContextMenuItemModel contextMenuItemModel1 = new ContextMenuItemModel()
    {
      Name = "Edit",
      Caption = Resources.WinOCRSelectedLineContextMenuEdit,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/Select_TextObjEdit.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/Select_TextObjEdit.png")),
      Command = (ICommand) new AsyncRelayCommand<ContextMenuItemModel>(new Func<ContextMenuItemModel, Task>(this.OnContextEditObject))
    };
    ContextMenuItemModel contextMenuItemModel2 = new ContextMenuItemModel()
    {
      Name = "Delete",
      Caption = Resources.MenuRightAnnotateDelete,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/Select_AnnotDelete.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/Select_AnnotDelete.png")),
      Command = (ICommand) new AsyncRelayCommand<ContextMenuItemModel>(new Func<ContextMenuItemModel, Task>(this.OnContextDeleteObject))
    };
    ContextMenuItemModel contextMenuItemModel3 = new ContextMenuItemModel()
    {
      Name = "ExitEdit",
      Caption = Resources.MenuRightAnnotateExitEdit,
      Command = (ICommand) new AsyncRelayCommand<ContextMenuItemModel>(new Func<ContextMenuItemModel, Task>(this.OnContextExitEditObject))
    };
    ContextMenuModel contextMenuModel1 = new ContextMenuModel();
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel1);
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel2);
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel3);
    ContextMenuModel contextMenuModel2 = contextMenuModel1;
    PdfViewerContextMenu viewerContextMenu = new PdfViewerContextMenu();
    viewerContextMenu.ItemsSource = (IEnumerable) contextMenuModel2;
    viewerContextMenu.PlacementTarget = (UIElement) this.annotationCanvas;
    viewerContextMenu.AutoCloseOnMouseLeave = false;
    this.textObjectContextMenu = viewerContextMenu;
    this.textObjectContextMenu.Closed += new RoutedEventHandler(this.TextObjectContextMenu_Closed);
  }

  private async Task OnContextEditObject(ContextMenuItemModel model)
  {
    if (this.annotationCanvas.TextObjectHolder.SelectedObject == null)
      return;
    CommomLib.Commom.GAManager.SendEvent("ContextRightMenu", "EditTextObject", "Count", 1L);
    await this.annotationCanvas.TextObjectHolder.EditSelectedTextObjectAsync();
  }

  private async Task OnContextDeleteObject(ContextMenuItemModel model)
  {
    if (this.annotationCanvas.TextObjectHolder.SelectedObject == null)
      return;
    CommomLib.Commom.GAManager.SendEvent("ContextRightMenu", "DeleteTextObject", "Count", 1L);
    CommomLib.Commom.GAManager.SendEvent("TextEditor", "DeleteSelectedObject", "ContextRightMenu", 1L);
    await this.annotationCanvas.TextObjectHolder.DeleteSelectedObjectAsync();
  }

  private Task OnContextExitEditObject(ContextMenuItemModel arg)
  {
    this.annotationCanvas.TextObjectHolder.CancelTextObject();
    this.Hide();
    return Task.CompletedTask;
  }

  public async Task<bool> ShowAsync()
  {
    PdfViewer pdfViewer = this.annotationCanvas.PdfViewer;
    if ((pdfViewer != null ? (pdfViewer.MouseMode == MouseModes.PanTool ? 1 : 0) : 0) != 0)
      return false;
    await Task.Delay(50);
    if (this.annotationCanvas.TextObjectHolder.SelectedObject == null)
      return false;
    if (this.textObjectContextMenu.IsOpen)
      return true;
    this.textObjectContextMenu.Placement = PlacementMode.MousePoint;
    this.textObjectContextMenu.PlacementTarget = (UIElement) null;
    this.textObjectContextMenu.IsOpen = true;
    return true;
  }

  public async Task<bool> ShowAtSelectedObejctRightAsync()
  {
    PdfViewer pdfViewer = this.annotationCanvas.PdfViewer;
    if ((pdfViewer != null ? (pdfViewer.MouseMode == MouseModes.PanTool ? 1 : 0) : 0) != 0)
      return false;
    await Task.Delay(50);
    if (this.annotationCanvas.TextObjectHolder.SelectedObject == null)
      return false;
    if (this.textObjectContextMenu.IsOpen)
      return true;
    this.annotationCanvas.UpdateHoverPageObjectRect(Rect.Empty);
    this.textObjectContextMenu.Placement = PlacementMode.Right;
    this.textObjectContextMenu.PlacementTarget = (UIElement) this.annotationCanvas.TextObjectHolder.TextObjectEditControl;
    Rect rect1 = Rect.Empty;
    if (this.annotationCanvas.TextObjectHolder.TextObjectEditControl != null)
    {
      rect1 = new Rect(0.0, 0.0, this.annotationCanvas.TextObjectHolder.TextObjectEditControl.ActualWidth, this.annotationCanvas.TextObjectHolder.TextObjectEditControl.ActualHeight);
      Rect rect2 = this.annotationCanvas.TransformToVisual((Visual) this.annotationCanvas.TextObjectHolder.TextObjectEditControl).TransformBounds(new Rect(0.0, 0.0, this.annotationCanvas.ActualWidth, this.annotationCanvas.ActualWidth));
      rect1.Intersect(rect2);
    }
    if (rect1.Width < 0.01 && rect1.Height < 0.01)
      rect1 = Rect.Empty;
    this.textObjectContextMenu.PlacementRectangle = rect1;
    this.textObjectContextMenu.IsOpen = true;
    return true;
  }

  public void Hide() => this.textObjectContextMenu.IsOpen = false;

  private void TextObjectContextMenu_Closed(object sender, RoutedEventArgs e)
  {
    this.textObjectContextMenu.Placement = PlacementMode.MousePoint;
    this.textObjectContextMenu.PlacementTarget = (UIElement) null;
  }

  private static bool IsDesignMode
  {
    get
    {
      return (bool) DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof (DependencyObject)).DefaultValue;
    }
  }
}
