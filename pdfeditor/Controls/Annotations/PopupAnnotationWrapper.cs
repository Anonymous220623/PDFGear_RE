// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.PopupAnnotationWrapper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Patagames.Pdf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using pdfeditor.Utils;
using System;
using System.Collections.ObjectModel;

#nullable disable
namespace pdfeditor.Controls.Annotations;

public class PopupAnnotationWrapper : ObservableObject
{
  private PdfPopupAnnotation rawObject;
  private ObservableCollection<PopupAnnotationReplyWrapper> replies;

  public PopupAnnotationWrapper(PdfPopupAnnotation annot) => this.rawObject = annot;

  public PdfPopupAnnotation Annotation => this.rawObject;

  public ObservableCollection<PopupAnnotationReplyWrapper> Replies
  {
    get => this.replies;
    set
    {
      this.SetProperty<ObservableCollection<PopupAnnotationReplyWrapper>>(ref this.replies, value, nameof (Replies));
    }
  }

  public PdfPage Page => this.rawObject.Page;

  public PdfAnnotation Parent => this.rawObject.Parent;

  public int AnnotationIndex
  {
    get
    {
      PdfAnnotationCollection annots = this.rawObject.Page.Annots;
      return annots == null ? -1 : __nonvirtual (annots.IndexOf((PdfAnnotation) this.rawObject));
    }
  }

  public string Contents
  {
    get => this.GetContent();
    set
    {
      string str = value ?? "";
      if (!(this.GetContent() != str))
        return;
      this.rawObject.Contents = str;
      this.OnPropertyChanged(nameof (Contents));
    }
  }

  private string GetContent()
  {
    try
    {
      return this.rawObject.Contents ?? "";
    }
    catch
    {
    }
    return "";
  }

  public bool IsOpen
  {
    get => this.rawObject.IsOpen;
    set
    {
      if (this.rawObject.IsOpen == value)
        return;
      this.rawObject.IsOpen = value;
      this.OnPropertyChanged(nameof (IsOpen));
    }
  }

  public FS_RECTF Rectangle
  {
    get
    {
      PdfTypeBase array1;
      if (this.rawObject.Dictionary.TryGetValue("Rect", out array1))
        return new FS_RECTF(array1);
      PdfTypeBase array2;
      if (this.rawObject.Parent?.Dictionary != null && this.rawObject.Parent.Dictionary.TryGetValue("Rect", out array2))
      {
        FS_RECTF fsRectf = new FS_RECTF(array2);
        FS_RECTF rectangle = new FS_RECTF(fsRectf.right + 40f, fsRectf.top, (float) ((double) fsRectf.right + 40.0 + 180.0), fsRectf.top - 140f);
        this.rawObject.Dictionary["Rect"] = (PdfTypeBase) rectangle.ToArray();
        return rectangle;
      }
      PdfPage page = this.rawObject.Page;
      FS_RECTF rectangle1 = new FS_RECTF(page.Width - 200f, (float) ((double) page.Height / 2.0 + 70.0), page.Width + 20f, (float) ((double) page.Height / 2.0 - 70.0));
      this.rawObject.Dictionary["Rect"] = (PdfTypeBase) rectangle1.ToArray();
      return rectangle1;
    }
    set
    {
      if (!(this.rawObject.Rectangle != value))
        return;
      this.rawObject.Rectangle = value;
      this.OnPropertyChanged(nameof (Rectangle));
    }
  }

  public FS_COLOR Color
  {
    get => this.GetColor();
    set
    {
      if (!(this.GetColor() != value))
        return;
      this.rawObject.Color = value;
      this.OnPropertyChanged(nameof (Color));
      this.OnPropertyChanged("BackgroundColor");
    }
  }

  private FS_COLOR GetColor()
  {
    try
    {
      return this.rawObject.Color;
    }
    catch
    {
    }
    return FS_COLOR.White;
  }

  public System.Windows.Media.Color BackgroundColor => this.Color.ToColor();

  public string Text => this.rawObject.Text;

  public DateTimeOffset? ModificationDate
  {
    get
    {
      DateTimeOffset modificationDate;
      return this.rawObject.TryGetModificationDate(out modificationDate) ? new DateTimeOffset?(modificationDate) : new DateTimeOffset?();
    }
    set
    {
      this.rawObject.ModificationDate = value.Value.ToModificationDateString();
      this.OnPropertyChanged(nameof (ModificationDate));
      this.OnPropertyChanged("ModificationDateText");
      this.OnPropertyChanged("ModificationDateTextShort");
    }
  }

  public string ModificationDateText
  {
    get
    {
      DateTimeOffset? modificationDate = this.ModificationDate;
      return modificationDate.HasValue ? modificationDate.Value.ToString("G") : string.Empty;
    }
  }

  public string ModificationDateTextShort
  {
    get
    {
      DateTimeOffset? modificationDate = this.ModificationDate;
      return modificationDate.HasValue ? modificationDate.Value.ToString("d") : string.Empty;
    }
  }

  public void NotifyAnnotationChanged()
  {
    this.OnPropertyChanged("Contents");
    this.OnPropertyChanged("IsOpen");
    this.OnPropertyChanged("Rectangle");
    this.OnPropertyChanged("Color");
    this.OnPropertyChanged("BackgroundColor");
    this.OnPropertyChanged("ModificationDate");
    this.OnPropertyChanged("ModificationDateText");
    this.OnPropertyChanged("ModificationDateTextShort");
  }
}
