// Decompiled with JetBrains decompiler
// Type: PDFKit.PdfViewerDecorators.PdfFormFieldDecorator
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit.PdfViewerDecorators;

internal class PdfFormFieldDecorator : IPdfViewerDecorator
{
  private Pen focusBorderPen;

  public bool IsEnabled { get; set; }

  public Color FocusBorderColor
  {
    get
    {
      return !(this.focusBorderPen?.Brush is SolidColorBrush brush) ? Colors.Transparent : brush.Color;
    }
    set
    {
      if (!(this.FocusBorderColor != value))
        return;
      Pen pen = new Pen((Brush) new SolidColorBrush(value), this.FocusBorderThickness);
      pen.Freeze();
      this.focusBorderPen = pen;
    }
  }

  public double FocusBorderThickness
  {
    get
    {
      Pen focusBorderPen = this.focusBorderPen;
      return focusBorderPen == null ? 0.0 : focusBorderPen.Thickness;
    }
    set
    {
      if (this.FocusBorderThickness == value)
        return;
      Pen pen = new Pen((Brush) new SolidColorBrush(this.FocusBorderColor), value);
      pen.Freeze();
      this.focusBorderPen = pen;
    }
  }

  public Color FillColor { get; set; }

  public bool CanDrawPdfBitmap(PdfViewerDecoratorDrawingArgs args)
  {
    if (args.PdfPage?.Document?.FormFill?.InterForm == null || args.Viewer == null || !this.IsEnabled || args.PdfBitmap == null || this.FillColor.A == (byte) 0)
      return false;
    PdfControlCollections pageControls;
    if (!args.GetContext<PdfControlCollections>("FormControls", out pageControls))
    {
      pageControls = args.PdfPage.Document.FormFill.InterForm.GetPageControls(args.PdfPage);
      args.SetContext<PdfControlCollections>("FormControls", pageControls);
    }
    return pageControls.Count != 0;
  }

  public bool CanDrawVisual(PdfViewerDecoratorDrawingArgs args)
  {
    if (args.PdfPage?.Document?.FormFill?.InterForm == null || args.Viewer == null || !this.IsEnabled || args.DrawingContext == null || this.FocusBorderColor.A == (byte) 0 || this.FocusBorderThickness == 0.0 || this.focusBorderPen == null)
      return false;
    PdfControlCollections pageControls;
    if (!args.GetContext<PdfControlCollections>("FormControls", out pageControls))
    {
      pageControls = args.PdfPage.Document.FormFill.InterForm.GetPageControls(args.PdfPage);
      args.SetContext<PdfControlCollections>("FormControls", pageControls);
    }
    return pageControls.Count != 0;
  }

  public void DrawPdfBitmap(PdfViewerDecoratorDrawingArgs args)
  {
    PdfControlCollections controlCollections;
    if (!args.GetContext<PdfControlCollections>("FormControls", out controlCollections))
      return;
    IntPtr num = IntPtr.Zero;
    if (!args.GetContext<IntPtr>("FocusedControl", out num))
    {
      try
      {
        num = Pdfium.FORM_GetFocusAnnot(args.PdfPage.Document.FormFill.Handle);
      }
      catch
      {
      }
      args.SetContext<IntPtr>("FocusedControl", num);
    }
    int count = controlCollections.Count;
    for (int index = 0; index < count; ++index)
    {
      Patagames.Pdf.Net.PdfControl control = controlCollections[index];
      if (PdfFormFieldDecorator.ShouldDrawFieldControl(control))
      {
        FS_RECTF rect = control.Rect;
        Rect clientRect = args.Viewer.PageToClientRect(rect, args.PdfPage.PageIndex);
        int pixels1 = Helpers.UnitsToPixels((DependencyObject) args.Viewer, clientRect.X);
        int pixels2 = Helpers.UnitsToPixels((DependencyObject) args.Viewer, clientRect.Y);
        int pixels3 = Helpers.UnitsToPixels((DependencyObject) args.Viewer, clientRect.Width);
        int pixels4 = Helpers.UnitsToPixels((DependencyObject) args.Viewer, clientRect.Height);
        if (control.Dictionary.Handle != num)
        {
          PdfBitmap pdfBitmap = args.PdfBitmap;
          int left = pixels1;
          int top = pixels2;
          int width = pixels3;
          int height = pixels4;
          Color fillColor = this.FillColor;
          int a = (int) fillColor.A;
          fillColor = this.FillColor;
          int r = (int) fillColor.R;
          fillColor = this.FillColor;
          int g = (int) fillColor.G;
          fillColor = this.FillColor;
          int b = (int) fillColor.B;
          FS_COLOR color = new FS_COLOR(a, r, g, b);
          pdfBitmap.FillRect(left, top, width, height, color);
        }
      }
    }
  }

  public void DrawVisual(PdfViewerDecoratorDrawingArgs args)
  {
    PdfControlCollections controlCollections;
    if (!args.GetContext<PdfControlCollections>("FormControls", out controlCollections))
      return;
    IntPtr num = IntPtr.Zero;
    if (!args.GetContext<IntPtr>("FocusedControl", out num))
    {
      try
      {
        num = Pdfium.FORM_GetFocusAnnot(args.PdfPage.Document.FormFill.Handle);
      }
      catch
      {
      }
      args.SetContext<IntPtr>("FocusedControl", num);
    }
    int count = controlCollections.Count;
    for (int index = 0; index < count; ++index)
    {
      Patagames.Pdf.Net.PdfControl pdfControl = controlCollections[index];
      if (pdfControl.Dictionary.Handle == num && pdfControl.Type != FormFieldTypesEx.CheckBox && pdfControl.Type != FormFieldTypesEx.CheckBox)
      {
        FS_RECTF rect = pdfControl.Rect;
        Rect clientRect = args.Viewer.PageToClientRect(rect, args.PdfPage.PageIndex);
        args.DrawingContext.DrawRectangle((Brush) null, this.focusBorderPen, clientRect);
      }
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static bool ShouldDrawFieldControl(Patagames.Pdf.Net.PdfControl control)
  {
    try
    {
      if (control == null || control.Field == null || control.Field.ReadOnly)
        return false;
      FormFieldTypesEx type = control.Type;
      int num;
      switch (type)
      {
        case FormFieldTypesEx.PushButton:
        case FormFieldTypesEx.RadioButton:
        case FormFieldTypesEx.CheckBox:
        case FormFieldTypesEx.RichText:
        case FormFieldTypesEx.ListBox:
        case FormFieldTypesEx.ComboBox:
          num = 1;
          break;
        default:
          num = type == FormFieldTypesEx.Text ? 1 : 0;
          break;
      }
      if (num != 0)
        return true;
    }
    catch
    {
    }
    return false;
  }
}
