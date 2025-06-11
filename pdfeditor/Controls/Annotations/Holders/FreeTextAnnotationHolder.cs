// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.FreeTextAnnotationHolder
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit.Utils;
using PDFKit.Utils.PdfRichTextStrings;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.Annotations.Holders;

public class FreeTextAnnotationHolder(AnnotationCanvas annotationCanvas) : 
  BaseAnnotationHolder<PdfFreeTextAnnotation>(annotationCanvas)
{
  private Rectangle newRectControl;
  private FS_POINTF createStartPoint;
  private FS_POINTF createEndPoint;
  private AnnotationFreeTextEditor editFreeTextControl;

  public override bool IsTextMarkupAnnotation => false;

  public override void OnPageClientBoundsChanged()
  {
    if (this.State == AnnotationHolderState.CreatingNew)
    {
      if (this.CurrentPage == null)
        throw new ArgumentException("CurrentPage");
      Point clientPoint1;
      Point clientPoint2;
      if (this.newRectControl == null || !this.AnnotationCanvas.PdfViewer.TryGetClientPoint(this.CurrentPage.PageIndex, this.createStartPoint.ToPoint(), out clientPoint1) || !this.AnnotationCanvas.PdfViewer.TryGetClientPoint(this.CurrentPage.PageIndex, this.createEndPoint.ToPoint(), out clientPoint2))
        return;
      Rect rect = new Rect(clientPoint1, clientPoint2);
      this.newRectControl.Width = rect.Width;
      this.newRectControl.Height = rect.Height;
      Canvas.SetLeft((UIElement) this.newRectControl, rect.Left);
      Canvas.SetTop((UIElement) this.newRectControl, rect.Top);
    }
    else
    {
      if (this.State != AnnotationHolderState.Selected)
        return;
      this.editFreeTextControl?.OnPageClientBoundsChanged();
    }
  }

  protected override async void OnCancel()
  {
    FreeTextAnnotationHolder annotationHolder = this;
    if (annotationHolder.newRectControl != null)
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (annotationHolder.AnnotationCanvas).Children.Remove((UIElement) annotationHolder.newRectControl);
      annotationHolder.newRectControl = (Rectangle) null;
    }
    if (annotationHolder.editFreeTextControl != null)
    {
      AnnotationFreeTextEditor eftc = annotationHolder.editFreeTextControl;
      annotationHolder.editFreeTextControl = (AnnotationFreeTextEditor) null;
      // ISSUE: explicit non-virtual call
      if (__nonvirtual (annotationHolder.State) == AnnotationHolderState.Selected)
      {
        PdfPage page = eftc.Annotation.Page;
        eftc.Apply();
        CancellationToken cancellationToken = new CancellationToken();
        await page.TryRedrawPageAsync(cancellationToken);
      }
      // ISSUE: explicit non-virtual call
      __nonvirtual (annotationHolder.AnnotationCanvas).Children.Remove((UIElement) eftc);
      eftc = (AnnotationFreeTextEditor) null;
    }
    annotationHolder.createStartPoint = new FS_POINTF();
    annotationHolder.createEndPoint = new FS_POINTF();
  }

  private static PdfRichTextString CreateRichTextString(string str, PdfRichTextStyle ds)
  {
    PdfRichTextString richTextString = new PdfRichTextString()
    {
      DefaultStyle = ds
    };
    if (string.IsNullOrEmpty(str))
      richTextString.Text = (PdfRichTextRawElement) new PdfRichTextElement()
      {
        Tag = PdfRichTextElementTag.Body,
        Style = new PdfRichTextStyle?(ds)
      };
    else
      richTextString.Text = (PdfRichTextRawElement) new PdfRichTextElement()
      {
        Tag = PdfRichTextElementTag.Body,
        Style = new PdfRichTextStyle?(ds),
        Children = {
          (PdfRichTextRawElement) new PdfRichTextElement()
          {
            Tag = PdfRichTextElementTag.P,
            Children = {
              (PdfRichTextRawElement) new PdfRichTextElement()
              {
                Tag = PdfRichTextElementTag.Span,
                Children = {
                  new PdfRichTextRawElement(str)
                }
              }
            }
          }
        }
      };
    return richTextString;
  }

  protected override async Task<System.Collections.Generic.IReadOnlyList<PdfFreeTextAnnotation>> OnCompleteCreateNewAsync()
  {
    FreeTextAnnotationHolder annotationHolder = this;
    if (annotationHolder.newRectControl != null)
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (annotationHolder.AnnotationCanvas).Children.Remove((UIElement) annotationHolder.newRectControl);
    }
    PdfFreeTextAnnotation freeTextAnnot = (PdfFreeTextAnnotation) null;
    MainViewModel vm = Ioc.Default.GetRequiredService<MainViewModel>();
    AnnotationMode annotationMode = vm.AnnotationMode;
    if (annotationMode == AnnotationMode.TextBox)
      vm.AnnotationMode = AnnotationMode.None;
    // ISSUE: explicit non-virtual call
    PdfPage page = __nonvirtual (annotationHolder.CurrentPage);
    if (page.Annots == null)
      page.CreateAnnotations();
    try
    {
      bool flag;
      switch (annotationMode)
      {
        case AnnotationMode.Text:
          flag = false;
          break;
        case AnnotationMode.TextBox:
          flag = true;
          break;
        default:
          return (System.Collections.Generic.IReadOnlyList<PdfFreeTextAnnotation>) null;
      }
      freeTextAnnot = new PdfFreeTextAnnotation(page);
      freeTextAnnot.Text = AnnotationAuthorUtil.GetAuthorName();
      Color color = (Color) ColorConverter.ConvertFromString(vm.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextBoxStroke);
      FS_COLOR strokeColor = new FS_COLOR((int) color.A, (int) color.R, (int) color.G, (int) color.B);
      PdfDefaultAppearance defaultAppearance = PdfDefaultAppearance.Default;
      PdfRichTextStyle pdfRichTextStyle1 = PdfRichTextStyle.Default;
      PdfRichTextStyle richTextStyle;
      if (flag)
      {
        color = (Color) ColorConverter.ConvertFromString(vm.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextBoxFill);
        freeTextAnnot.Color = color.ToPdfColor();
        color = (Color) ColorConverter.ConvertFromString(vm.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextBoxFontColor);
        FS_COLOR fillColor = new FS_COLOR((int) color.A, (int) color.R, (int) color.G, (int) color.B);
        defaultAppearance = new PdfDefaultAppearance(vm.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextBoxFontName, vm.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextBoxFontSize, strokeColor, fillColor);
        richTextStyle = defaultAppearance.ToRichTextStyle() with
        {
          Color = new Color?(color)
        };
      }
      else
      {
        color = (Color) ColorConverter.ConvertFromString(vm.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextFontColor);
        FS_COLOR fillColor = new FS_COLOR((int) color.A, (int) color.R, (int) color.G, (int) color.B);
        defaultAppearance = new PdfDefaultAppearance(vm.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextBoxFontName, vm.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextFontSize, strokeColor, fillColor);
        richTextStyle = defaultAppearance.ToRichTextStyle();
        freeTextAnnot.Intent = AnnotationIntent.FreeTextTypeWriter;
        defaultAppearance.StrokeColor = new FS_COLOR((int) byte.MaxValue, 0, 0, 0);
        richTextStyle.Color = new Color?(color);
      }
      freeTextAnnot.DefaultAppearance = defaultAppearance.ToString();
      freeTextAnnot.DefaultStyle = richTextStyle.ToString();
      PdfRichTextString richTextString = FreeTextAnnotationHolder.CreateRichTextString("", richTextStyle);
      freeTextAnnot.RichText = richTextString.ToString();
      freeTextAnnot.Contents = richTextString.ContentText;
      if (page.Rotation != PageRotate.Normal)
        freeTextAnnot.Dictionary["Rotate"] = (PdfTypeBase) PdfTypeNumber.Create((int) page.Rotation * 90);
      if (flag)
      {
        freeTextAnnot.BorderStyle = new PdfBorderStyle();
        freeTextAnnot.BorderStyle.Width = vm.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextBoxThickness;
        float l = Math.Min(annotationHolder.createStartPoint.X, annotationHolder.createEndPoint.X);
        float t = Math.Max(annotationHolder.createStartPoint.Y, annotationHolder.createEndPoint.Y);
        float num1 = Math.Max(annotationHolder.createStartPoint.X, annotationHolder.createEndPoint.X) - l;
        float num2 = t - Math.Min(annotationHolder.createStartPoint.Y, annotationHolder.createEndPoint.Y);
        FS_RECTF rect = new FS_RECTF(l, t, l + num1, t - num2);
        if (((double) Math.Abs(annotationHolder.createStartPoint.X - annotationHolder.createEndPoint.X) >= 5.0 ? 0 : ((double) Math.Abs(annotationHolder.createStartPoint.Y - annotationHolder.createEndPoint.Y) < 5.0 ? 1 : 0)) != 0)
        {
          FS_RECTF originalRectangle = PdfRotateUtils.GetOriginalRectangle(rect, page.Rotation);
          if ((double) originalRectangle.Width < 90.0)
            originalRectangle.right = originalRectangle.left + 90f;
          if ((double) originalRectangle.Height < 48.0)
            originalRectangle.bottom = originalRectangle.top - 48f;
          FS_MATRIX _matrix = new FS_MATRIX();
          _matrix.SetIdentity();
          PdfRotateUtils.RotateMatrix(page.Rotation, _matrix);
          _matrix.TransformRect(ref originalRectangle);
          rect = PdfRotateUtils.GetRotatedRect(originalRectangle, annotationHolder.createStartPoint, page.Rotation);
        }
        freeTextAnnot.Rectangle = rect;
      }
      else
      {
        PdfRichTextStyle pdfRichTextStyle2 = PdfRichTextStyle.Default;
        Typeface typeface1 = new Typeface(new FontFamily(pdfRichTextStyle2.FontFamily), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
        string contents = freeTextAnnot.Contents;
        CultureInfo currentCulture = CultureInfo.CurrentCulture;
        int num = CultureInfo.CurrentCulture.TextInfo.IsRightToLeft ? 1 : 0;
        Typeface typeface2 = typeface1;
        pdfRichTextStyle2 = PdfRichTextStyle.Default;
        double emSize = (double) pdfRichTextStyle2.FontSize.Value;
        SolidColorBrush black = Brushes.Black;
        FormattedText formattedText = new FormattedText(contents, currentCulture, (FlowDirection) num, typeface2, emSize, (Brush) black, 1.0);
        float x = annotationHolder.createEndPoint.X;
        float t = annotationHolder.createEndPoint.Y + vm.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextBoxFontSize / 2f;
        float r = (float) ((double) x + 70.0 + 10.0);
        float b = t - vm.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextBoxFontSize;
        FS_RECTF rect = new FS_RECTF(x, t, r, b);
        if (page.Rotation != PageRotate.Normal)
        {
          FS_MATRIX fsMatrix = new FS_MATRIX();
          fsMatrix.SetIdentity();
          fsMatrix.Translate(-rect.left, (float) (-(double) rect.bottom - (double) rect.Height / 2.0));
          fsMatrix.Rotate((float) ((double) ((int) page.Rotation * 90) * Math.PI / 180.0));
          fsMatrix.Translate(rect.left, rect.bottom + rect.Height / 2f);
          fsMatrix.TransformRect(ref rect);
        }
        freeTextAnnot.Rectangle = rect;
      }
      freeTextAnnot.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
      freeTextAnnot.CreationDate = freeTextAnnot.ModificationDate;
      freeTextAnnot.Flags |= AnnotationFlags.Print;
      page.Annots.Add((PdfAnnotation) freeTextAnnot);
      await freeTextAnnot.RegenerateAppearancesWithRichTextAsync();
      await vm.OperationManager.TraceAnnotationInsertAsync((PdfAnnotation) freeTextAnnot);
      await page.TryRedrawPageAsync();
      if (!((PdfWrapper) freeTextAnnot != (PdfWrapper) null))
        return (System.Collections.Generic.IReadOnlyList<PdfFreeTextAnnotation>) null;
      return (System.Collections.Generic.IReadOnlyList<PdfFreeTextAnnotation>) new PdfFreeTextAnnotation[1]
      {
        freeTextAnnot
      };
    }
    finally
    {
      annotationHolder.newRectControl = (Rectangle) null;
      annotationHolder.createStartPoint = new FS_POINTF();
      annotationHolder.createEndPoint = new FS_POINTF();
    }
  }

  protected override void OnProcessCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    if (page != this.CurrentPage)
      return;
    this.createEndPoint = pagePoint;
    Point clientPoint1;
    Point clientPoint2;
    if (this.newRectControl == null || !this.AnnotationCanvas.PdfViewer.TryGetClientPoint(this.CurrentPage.PageIndex, this.createStartPoint.ToPoint(), out clientPoint1) || !this.AnnotationCanvas.PdfViewer.TryGetClientPoint(this.CurrentPage.PageIndex, this.createEndPoint.ToPoint(), out clientPoint2))
      return;
    Rect rect = new Rect(clientPoint1, clientPoint2);
    this.newRectControl.Width = rect.Width;
    this.newRectControl.Height = rect.Height;
    Canvas.SetLeft((UIElement) this.newRectControl, rect.Left);
    Canvas.SetTop((UIElement) this.newRectControl, rect.Top);
  }

  private Rectangle CreateRectangle(Point startPoint)
  {
    object dataContext = this.AnnotationCanvas.DataContext;
    SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 24, (byte) 146, byte.MaxValue));
    Rectangle element = new Rectangle();
    element.Stroke = (Brush) solidColorBrush;
    element.StrokeThickness = 2.0;
    element.Fill = (Brush) new SolidColorBrush(Color.FromArgb((byte) 127 /*0x7F*/, byte.MaxValue, byte.MaxValue, byte.MaxValue));
    element.IsHitTestVisible = false;
    element.Width = 0.0;
    element.Height = 0.0;
    Canvas.SetLeft((UIElement) element, startPoint.X);
    Canvas.SetTop((UIElement) element, startPoint.Y);
    return element;
  }

  protected override bool OnSelecting(PdfFreeTextAnnotation annotation, bool afterCreate)
  {
    if (this.editFreeTextControl != null)
      return false;
    this.editFreeTextControl = new AnnotationFreeTextEditor(annotation, this);
    this.AnnotationCanvas.Children.Add((UIElement) this.editFreeTextControl);
    if (afterCreate)
    {
      this.editFreeTextControl.GoToEditing();
      this.editFreeTextControl.GetRichTextBox().SelectAll();
    }
    return true;
  }

  protected override bool OnStartCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    this.createStartPoint = pagePoint;
    this.createEndPoint = pagePoint;
    Point clientPoint;
    if (!this.AnnotationCanvas.PdfViewer.TryGetClientPoint(page.PageIndex, pagePoint.ToPoint(), out clientPoint))
      return false;
    if (this.AnnotationCanvas.DataContext is MainViewModel dataContext && dataContext.AnnotationMode == AnnotationMode.TextBox)
    {
      this.newRectControl = this.CreateRectangle(clientPoint);
      this.AnnotationCanvas.Children.Add((UIElement) this.newRectControl);
    }
    return true;
  }

  public override bool OnPropertyChanged(string propertyName)
  {
    if (!(propertyName == "TextBoxStroke") && !(propertyName == "TextBoxThickness") && !(propertyName == "TextBoxFill") && !(propertyName == "TextFontColor") && !(propertyName == "TextFontSize") && !(propertyName == "TextBoxFontColor") && !(propertyName == "TextBoxFontSize"))
      return false;
    PdfFreeTextAnnotation selectedAnnotation = (PdfFreeTextAnnotation) this.SelectedAnnotation;
    if (propertyName == "TextFontColor" || propertyName == "TextFontSize" || propertyName == "TextBoxFontColor" || propertyName == "TextBoxFontSize")
      return this.editFreeTextControl.OnPropertyChanged(propertyName);
    this.Cancel();
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    string defaultAppearance = selectedAnnotation.DefaultAppearance;
    if (propertyName == "TextBoxStroke" || propertyName == "TextBoxFill")
    {
      switch (propertyName)
      {
        case "TextBoxStroke":
          FS_COLOR pdfColor1 = ((Color) ColorConverter.ConvertFromString(requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextBoxStroke)).ToPdfColor();
          PdfDefaultAppearance pdfFontStyle;
          if (!PdfDefaultAppearance.TryParse(selectedAnnotation.DefaultAppearance, out pdfFontStyle))
            pdfFontStyle = PdfDefaultAppearance.Default;
          pdfFontStyle.StrokeColor = pdfColor1;
          defaultAppearance = pdfFontStyle.ToString();
          break;
        case "TextBoxFill":
          FS_COLOR pdfColor2 = ((Color) ColorConverter.ConvertFromString(requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextBoxFill)).ToPdfColor();
          selectedAnnotation.Color = pdfColor2;
          break;
      }
    }
    using (requiredService.OperationManager.TraceAnnotationChange(selectedAnnotation.Page))
    {
      selectedAnnotation.DefaultAppearance = defaultAppearance;
      if (propertyName == "TextBoxThickness")
      {
        if ((PdfWrapper) selectedAnnotation.BorderStyle == (PdfWrapper) null)
          selectedAnnotation.BorderStyle = new PdfBorderStyle();
        selectedAnnotation.BorderStyle.Width = requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextBoxThickness;
      }
      selectedAnnotation.RegenerateAppearancesWithRichText();
    }
    selectedAnnotation.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    this.Select((PdfAnnotation) selectedAnnotation, false);
    return true;
  }
}
