// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Commets.CommetModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using pdfeditor.Controls;
using pdfeditor.Controls.Annotations.Holders;
using pdfeditor.Models.Annotations;
using pdfeditor.Properties;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit.Utils.StampUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace pdfeditor.Models.Commets;

public class CommetModel : ObservableObject, ITreeViewNode
{
  private 
  #nullable disable
  string contents;
  private bool isSelected;
  private bool isChecked;
  private AsyncRelayCommand deleteSelectedAnnotCmd;

  private CommetModel()
  {
  }

  public CommetModel(BaseAnnotation annot, System.Collections.Generic.IReadOnlyList<CommetModel> replies, AnnotationMode mode)
  {
    this.Annotation = annot ?? throw new ArgumentNullException(nameof (annot));
    this.Replies = (System.Collections.Generic.IReadOnlyList<CommetModel>) ((object) replies ?? (object) Array.Empty<CommetModel>());
    this.Contents = CommetModel.GetContent(this.Annotation);
    if (this.Annotation is BaseMarkupAnnotation annotation)
    {
      this.Text = annotation.Text;
      this.IsContentReadOnly = !(annotation is FreeTextAnnotation);
    }
    else
      this.IsContentReadOnly = true;
    this.AnnotationMode = mode;
    this.Title = CommetModel.GetTitle(mode, annot);
  }

  public BaseAnnotation Annotation { get; private set; }

  public AnnotationMode AnnotationMode { get; private set; }

  public string Title { get; private set; }

  public string Text { get; private set; }

  public bool IsContentReadOnly { get; private set; }

  public AsyncRelayCommand DeleteSelectedAnnotCmd
  {
    get
    {
      return this.deleteSelectedAnnotCmd ?? (this.deleteSelectedAnnotCmd = new AsyncRelayCommand((Func<Task>) (async () =>
      {
        PdfDocument document = Ioc.Default.GetRequiredService<MainViewModel>().Document;
        if (document == null)
          return;
        CommomLib.Commom.GAManager.SendEvent("AnnotationMgmt", "SignalDeleteBtn", "Count", 1L);
        PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(document);
        if (pdfControl == null)
          return;
        AnnotationHolderManager annotationHolderManager = PdfObjectExtensions.GetAnnotationHolderManager(pdfControl);
        if (annotationHolderManager == null)
          return;
        PdfAnnotation annot = document.Pages[this.Annotation.PageIndex].Annots[this.Annotation.AnnotIndex];
        if (annot != null && annot is PdfFreeTextAnnotation freeTextAnnotation2 && string.IsNullOrEmpty(freeTextAnnotation2.Contents) && freeTextAnnotation2.Intent == AnnotationIntent.FreeTextTypeWriter)
          PdfObjectExtensions.GetAnnotationCanvas(pdfControl).HolderManager.CancelAll();
        else
          await annotationHolderManager.DeleteAnnotationAsync(annot);
      }), (Func<bool>) (() => !this.DeleteSelectedAnnotCmd.IsRunning && this.Annotation != null)));
    }
  }

  public DateTimeOffset? ModificationDate
  {
    get
    {
      DateTimeOffset dateTime;
      return PdfObjectExtensions.TryParseModificationDate(this.Annotation.ModificationDate, out dateTime) ? new DateTimeOffset?(dateTime) : new DateTimeOffset?();
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

  public string Contents
  {
    get => this.contents;
    set => this.SetProperty<string>(ref this.contents, value, nameof (Contents));
  }

  public System.Collections.Generic.IReadOnlyList<CommetModel> Replies { get; private set; }

  public bool IsSelected
  {
    get => this.isSelected;
    set => this.SetProperty<bool>(ref this.isSelected, value, nameof (IsSelected));
  }

  public bool IsChecked
  {
    get => this.isChecked;
    set
    {
      this.SetProperty<bool>(ref this.isChecked, value, nameof (IsChecked));
      this.judgeSelectall();
    }
  }

  private void judgeSelectall()
  {
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    bool? nullable = new bool?(true);
    foreach (PageCommetCollection commetCollection in (Collection<PageCommetCollection>) requiredService.PageCommetSource)
    {
      foreach (CommetModel commetModel in commetCollection)
      {
        if (!commetModel.IsChecked)
          nullable = new bool?();
      }
    }
    if (!nullable.HasValue)
    {
      nullable = new bool?(false);
      foreach (PageCommetCollection commetCollection in (Collection<PageCommetCollection>) requiredService.PageCommetSource)
      {
        foreach (CommetModel commetModel in commetCollection)
        {
          if (commetModel.IsChecked)
            nullable = new bool?();
        }
      }
    }
    requiredService.IsSelectedAll = nullable;
  }

  public ITreeViewNode Parent { get; set; }

  public bool IsDeleteAreaVisible
  {
    get => Ioc.Default.GetRequiredService<MainViewModel>().IsDeleteAreaVisible;
  }

  public static CommetModel TryCreate(BaseAnnotation annot, System.Collections.Generic.IReadOnlyList<CommetModel> replies)
  {
    switch (annot)
    {
      case PopupAnnotation _:
        return (CommetModel) null;
      case InkAnnotation inkAnnotation:
        if (inkAnnotation.InkList != null && inkAnnotation.InkList.Count <= 1)
        {
          int num = 0;
          foreach (System.Collections.Generic.IReadOnlyList<FS_POINTF> ink in (IEnumerable<System.Collections.Generic.IReadOnlyList<FS_POINTF>>) inkAnnotation.InkList)
            num += ink.Count;
          if (num <= 1)
            return (CommetModel) null;
          break;
        }
        break;
    }
    System.Collections.Generic.IReadOnlyList<AnnotationMode> annotationModes = AnnotationFactory.GetAnnotationModes(annot);
    return annotationModes.Count == 0 || annotationModes[0] == AnnotationMode.None ? (CommetModel) null : new CommetModel(annot, replies, annotationModes[0]);
  }

  private static string GetTitle(AnnotationMode mode, BaseAnnotation annot)
  {
    switch (mode)
    {
      case AnnotationMode.Line:
      case AnnotationMode.Arrow:
        return annot is LineAnnotation lineAnnotation && lineAnnotation.LineEnding != null && lineAnnotation.LineEnding.Count > 0 && lineAnnotation.LineEnding.Any<LineEndingStyles>((Func<LineEndingStyles, bool>) (c => c == LineEndingStyles.ClosedArrow || c == LineEndingStyles.OpenArrow || c == LineEndingStyles.RClosedArrow || c == LineEndingStyles.ROpenArrow)) ? Resources.MenuAnnotateArrowContent : Resources.MenuAnnotateLineContent;
      case AnnotationMode.Ink:
        return Resources.MenuAnnotateInkContent;
      case AnnotationMode.Shape:
        return Resources.MenuAnnotateShapeContent;
      case AnnotationMode.Highlight:
        return annot is HighlightAnnotation highlightAnnotation && !(highlightAnnotation.Subject == "AreaHighlight") ? Resources.MenuAnnotateHighlightContent : Resources.WinToolBarBtnHighlightContent;
      case AnnotationMode.Underline:
        return Resources.MenuAnnotateUnderlineContent;
      case AnnotationMode.Strike:
        return Resources.MenuAnnotateStrikeContent;
      case AnnotationMode.HighlightArea:
        return Resources.MenuAnnotateHighlight;
      case AnnotationMode.Note:
        return Resources.MenuAnnotateNoteContent;
      case AnnotationMode.Ellipse:
        return Resources.MenuAnnotateEllipseContent;
      case AnnotationMode.Stamp:
        return annot is StampAnnotation stampAnnotation && stampAnnotation.Subject == "Signature" ? Resources.MenuAnnotateSignatureContent : Resources.MenuAnnotateStampContent;
      case AnnotationMode.Text:
      case AnnotationMode.TextBox:
        return annot is FreeTextAnnotation freeTextAnnotation && freeTextAnnotation.Intent == AnnotationIntent.FreeTextTypeWriter ? Resources.MenuAnnotateTypeWriterContent : Resources.MenuAnnotateTextBoxContent;
      case AnnotationMode.Signature:
        return Resources.MenuAnnotateSignatureContent;
      default:
        return (string) null;
    }
  }

  private static string GetContent(BaseAnnotation annot)
  {
    if (annot is StampAnnotation stampAnnotation)
    {
      PdfTypeBase pdfTypeBase;
      if (stampAnnotation.PDFXExtend != null && stampAnnotation.PDFXExtend.TryGetValue("Type", out pdfTypeBase) && pdfTypeBase.Is<PdfTypeName>() && pdfTypeBase.As<PdfTypeName>().Value == "FormControl")
        return "";
      if (string.IsNullOrEmpty(annot.Contents) && stampAnnotation.PDFXExtend != null)
      {
        PDFExtStampDictionary extStampDictionary = new PDFExtStampDictionary();
        if (stampAnnotation.PDFXExtend.ContainsKey("Type") && stampAnnotation.PDFXExtend["Type"].Is<PdfTypeName>())
          extStampDictionary.Type = stampAnnotation.PDFXExtend["Type"].As<PdfTypeName>().Value;
        if (stampAnnotation.PDFXExtend.ContainsKey("Content") && stampAnnotation.PDFXExtend["Content"].Is<PdfTypeString>())
          extStampDictionary.Content = stampAnnotation.PDFXExtend["Content"].As<PdfTypeString>().UnicodeString;
        if (stampAnnotation.PDFXExtend.ContainsKey("Template") && stampAnnotation.PDFXExtend["Template"].Is<PdfTypeString>())
          extStampDictionary.Template = stampAnnotation.PDFXExtend["Template"].As<PdfTypeString>().UnicodeString;
        Dictionary<string, string> contentDictionary = extStampDictionary.GetContentDictionary();
        string str;
        return contentDictionary != null && contentDictionary.TryGetValue("ContentText", out str) ? str : stampAnnotation.ExtendedIconName;
      }
    }
    return annot.Contents ?? "";
  }

  internal CommetModel Clone()
  {
    CommetModel commetModel1 = new CommetModel()
    {
      contents = this.contents,
      isSelected = this.isSelected,
      isChecked = this.isChecked,
      Annotation = this.Annotation,
      AnnotationMode = this.AnnotationMode,
      Title = this.Title,
      Text = this.Text,
      IsContentReadOnly = this.IsContentReadOnly
    };
    if (this.Replies != null)
    {
      List<CommetModel> commetModelList = new List<CommetModel>();
      for (int index = 0; index < this.Replies.Count; ++index)
      {
        CommetModel commetModel2 = this.Replies[index].Clone();
        commetModel2.Parent = (ITreeViewNode) commetModel1;
        commetModelList.Add(commetModel2);
      }
      commetModel1.Replies = (System.Collections.Generic.IReadOnlyList<CommetModel>) commetModelList;
    }
    return commetModel1;
  }
}
