// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageContents.TextObjectHolder
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Patagames.Pdf.Net;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using System.Threading.Tasks;
using System.Windows;

#nullable disable
namespace pdfeditor.Controls.PageContents;

public class TextObjectHolder
{
  private TextObjectEditControl textObjectEditControl;

  public TextObjectHolder(AnnotationCanvas annotationCanvas)
  {
    this.AnnotationCanvas = annotationCanvas;
  }

  public AnnotationCanvas AnnotationCanvas { get; }

  public PdfTextObject SelectedObject => this.textObjectEditControl?.TextObject;

  public TextObjectEditControl TextObjectEditControl => this.textObjectEditControl;

  public async Task DeleteSelectedObjectAsync()
  {
    if (this.textObjectEditControl == null)
      return;
    int pageIndex = this.textObjectEditControl.PageIndex;
    PdfTextObject textObject = this.textObjectEditControl.TextObject;
    if (textObject == null)
      return;
    PdfPage page = this.AnnotationCanvas.PdfViewer.Document.Pages[pageIndex];
    MainViewModel dataContext = this.AnnotationCanvas.DataContext as MainViewModel;
    this.CancelTextObject();
    GAManager.SendEvent("TextEditor", "DeleteSelectedObject", "Count", 1L);
    int num = await dataContext.OperationManager.DeleteTextObjectAsync(page, textObject) ? 1 : 0;
  }

  public void OnPageClientBoundsChanged() => this.textObjectEditControl?.UpdatePosition();

  public bool CancelTextObject()
  {
    if (this.textObjectEditControl == null)
      return false;
    this.AnnotationCanvas.Children.Remove((UIElement) this.textObjectEditControl);
    this.textObjectEditControl = (TextObjectEditControl) null;
    return true;
  }

  public void SelectTextObject(PdfPage page, PdfTextObject textObject, bool showContextMenu = true)
  {
    if (page == null || textObject == null)
    {
      this.CancelTextObject();
    }
    else
    {
      if (this.AnnotationCanvas.EditingPageObjectType != PageObjectType.Text)
        return;
      this.AnnotationCanvas.UpdateHoverPageObjectRect(Rect.Empty);
      if (this.textObjectEditControl != null)
        this.AnnotationCanvas.Children.Remove((UIElement) this.textObjectEditControl);
      this.textObjectEditControl = new TextObjectEditControl(this.AnnotationCanvas, page.PageIndex, textObject);
      this.AnnotationCanvas.Children.Add((UIElement) this.textObjectEditControl);
      if (!showContextMenu)
        return;
      this.AnnotationCanvas.TextObjectContextMenuHolder.ShowAtSelectedObejctRightAsync();
    }
  }

  public async Task EditSelectedTextObjectAsync()
  {
    PdfPage page;
    if (this.textObjectEditControl == null)
    {
      page = (PdfPage) null;
    }
    else
    {
      int pageIndex = this.textObjectEditControl.PageIndex;
      PdfTextObject textObject = this.textObjectEditControl.TextObject;
      if (textObject == null)
      {
        page = (PdfPage) null;
      }
      else
      {
        GAManager.SendEvent("TextEditor", "EditSelectedTextObject", "Show", 1L);
        page = this.AnnotationCanvas.PdfViewer.Document.Pages[pageIndex];
        TextObjectEditDialog objectEditDialog = new TextObjectEditDialog();
        objectEditDialog.Text = textObject.TextUnicode;
        objectEditDialog.Owner = Application.Current.MainWindow;
        objectEditDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        if (!objectEditDialog.ShowDialog().GetValueOrDefault())
        {
          page = (PdfPage) null;
        }
        else
        {
          string text = objectEditDialog.Text;
          if (text == textObject.TextUnicode)
          {
            page = (PdfPage) null;
          }
          else
          {
            GAManager.SendEvent("TextEditor", "EditSelectedTextObject", "EditText", 1L);
            MainViewModel dataContext = this.AnnotationCanvas.DataContext as MainViewModel;
            this.CancelTextObject();
            if (dataContext == null)
              page = (PdfPage) null;
            else if (string.IsNullOrWhiteSpace(text))
            {
              int num = await dataContext.OperationManager.DeleteTextObjectAsync(page, textObject) ? 1 : 0;
              page = (PdfPage) null;
            }
            else
            {
              PdfTextObject[] pdfTextObjectArray = await dataContext.OperationManager.ModifyTextObjectAsync(page, textObject, text);
              if (pdfTextObjectArray == null)
                page = (PdfPage) null;
              else if (pdfTextObjectArray.Length == 0)
              {
                page = (PdfPage) null;
              }
              else
              {
                this.SelectTextObject(page, pdfTextObjectArray[0], false);
                page = (PdfPage) null;
              }
            }
          }
        }
      }
    }
  }
}
