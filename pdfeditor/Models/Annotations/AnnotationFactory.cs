// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.AnnotationFactory
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace pdfeditor.Models.Annotations;

public static class AnnotationFactory
{
  private static Dictionary<Type, AnnotationFactory.AnnotationTypeData> annotTypes;
  private static Dictionary<Type, AnnotationFactory.AnnotationTypeData> pdfAnnotTypes;
  private static Dictionary<AnnotationMode, AnnotationFactory.AnnotationTypeData> annotModeData;

  private static Dictionary<Type, AnnotationFactory.AnnotationTypeData> AnnotTypes
  {
    get
    {
      if (AnnotationFactory.annotTypes == null)
      {
        lock (typeof (AnnotationFactory))
        {
          if (AnnotationFactory.annotTypes == null)
            AnnotationFactory.InitAnnotTypes();
        }
      }
      return AnnotationFactory.annotTypes;
    }
  }

  private static Dictionary<Type, AnnotationFactory.AnnotationTypeData> PdfAnnotTypes
  {
    get
    {
      if (AnnotationFactory.pdfAnnotTypes == null)
      {
        lock (typeof (AnnotationFactory))
        {
          if (AnnotationFactory.pdfAnnotTypes == null)
            AnnotationFactory.InitAnnotTypes();
        }
      }
      return AnnotationFactory.pdfAnnotTypes;
    }
  }

  private static Dictionary<AnnotationMode, AnnotationFactory.AnnotationTypeData> AnnotModeData
  {
    get
    {
      if (AnnotationFactory.annotModeData == null)
      {
        lock (typeof (AnnotationFactory))
        {
          if (AnnotationFactory.annotModeData == null)
            AnnotationFactory.InitAnnotTypes();
        }
      }
      return AnnotationFactory.annotModeData;
    }
  }

  private static void InitAnnotTypes()
  {
    if (AnnotationFactory.annotTypes == null)
    {
      Dictionary<Type, AnnotationFactory.AnnotationTypeData> dictionary = new Dictionary<Type, AnnotationFactory.AnnotationTypeData>();
      Type key1 = typeof (PopupAnnotation);
      dictionary[key1] = new AnnotationFactory.AnnotationTypeData(typeof (PopupAnnotation), typeof (PdfPopupAnnotation), AnnotationMode.Popup, (Func<BaseAnnotation>) (() => (BaseAnnotation) AnnotationFactory.CreateInstance<PopupAnnotation>()), (Func<PdfPage, BaseAnnotation, PdfAnnotation>) ((p, a) => (PdfAnnotation) new PdfPopupAnnotation(p)));
      Type key2 = typeof (UnderlineAnnotation);
      dictionary[key2] = new AnnotationFactory.AnnotationTypeData(typeof (UnderlineAnnotation), typeof (PdfUnderlineAnnotation), AnnotationMode.Underline, (Func<BaseAnnotation>) (() => (BaseAnnotation) AnnotationFactory.CreateInstance<UnderlineAnnotation>()), (Func<PdfPage, BaseAnnotation, PdfAnnotation>) ((p, a) => (PdfAnnotation) new PdfUnderlineAnnotation(p)));
      Type key3 = typeof (StrikeoutAnnotation);
      dictionary[key3] = new AnnotationFactory.AnnotationTypeData(typeof (StrikeoutAnnotation), typeof (PdfStrikeoutAnnotation), AnnotationMode.Strike, (Func<BaseAnnotation>) (() => (BaseAnnotation) AnnotationFactory.CreateInstance<StrikeoutAnnotation>()), (Func<PdfPage, BaseAnnotation, PdfAnnotation>) ((p, a) => (PdfAnnotation) new PdfStrikeoutAnnotation(p)));
      Type key4 = typeof (HighlightAnnotation);
      dictionary[key4] = new AnnotationFactory.AnnotationTypeData(typeof (HighlightAnnotation), typeof (PdfHighlightAnnotation), new AnnotationMode[2]
      {
        AnnotationMode.Highlight,
        AnnotationMode.HighlightArea
      }, (Func<BaseAnnotation>) (() => (BaseAnnotation) AnnotationFactory.CreateInstance<HighlightAnnotation>()), (Func<PdfPage, BaseAnnotation, PdfAnnotation>) ((p, a) => (PdfAnnotation) new PdfHighlightAnnotation(p)), (Func<PdfAnnotation, AnnotationMode>) (a => !(a is PdfMarkupAnnotation markupAnnotation1) || !(markupAnnotation1.Subject == "AreaHighlight") ? AnnotationMode.Highlight : AnnotationMode.HighlightArea), (Func<BaseAnnotation, AnnotationMode>) (a => !(a is BaseMarkupAnnotation markupAnnotation2) || !(markupAnnotation2.Subject == "AreaHighlight") ? AnnotationMode.Highlight : AnnotationMode.HighlightArea));
      Type key5 = typeof (LineAnnotation);
      dictionary[key5] = new AnnotationFactory.AnnotationTypeData(typeof (LineAnnotation), typeof (PdfLineAnnotation), AnnotationMode.Line, (Func<BaseAnnotation>) (() => (BaseAnnotation) AnnotationFactory.CreateInstance<LineAnnotation>()), (Func<PdfPage, BaseAnnotation, PdfAnnotation>) ((p, a) => (PdfAnnotation) new PdfLineAnnotation(p)));
      Type key6 = typeof (FreeTextAnnotation);
      dictionary[key6] = new AnnotationFactory.AnnotationTypeData(typeof (FreeTextAnnotation), typeof (PdfFreeTextAnnotation), new AnnotationMode[2]
      {
        AnnotationMode.TextBox,
        AnnotationMode.Text
      }, (Func<BaseAnnotation>) (() => (BaseAnnotation) AnnotationFactory.CreateInstance<FreeTextAnnotation>()), (Func<PdfPage, BaseAnnotation, PdfAnnotation>) ((p, a) => (PdfAnnotation) new PdfFreeTextAnnotation(p)), (Func<PdfAnnotation, AnnotationMode>) (a => !(a is PdfFreeTextAnnotation freeTextAnnotation1) || freeTextAnnotation1.Intent != AnnotationIntent.FreeTextTypeWriter ? AnnotationMode.TextBox : AnnotationMode.Text), (Func<BaseAnnotation, AnnotationMode>) (a => !(a is FreeTextAnnotation freeTextAnnotation2) || freeTextAnnotation2.Intent != AnnotationIntent.FreeTextTypeWriter ? AnnotationMode.TextBox : AnnotationMode.Text));
      Type key7 = typeof (InkAnnotation);
      dictionary[key7] = new AnnotationFactory.AnnotationTypeData(typeof (InkAnnotation), typeof (PdfInkAnnotation), AnnotationMode.Ink, (Func<BaseAnnotation>) (() => (BaseAnnotation) AnnotationFactory.CreateInstance<InkAnnotation>()), (Func<PdfPage, BaseAnnotation, PdfAnnotation>) ((p, a) => (PdfAnnotation) new PdfInkAnnotation(p)));
      Type key8 = typeof (SquareAnnotation);
      dictionary[key8] = new AnnotationFactory.AnnotationTypeData(typeof (SquareAnnotation), typeof (PdfSquareAnnotation), AnnotationMode.Shape, (Func<BaseAnnotation>) (() => (BaseAnnotation) AnnotationFactory.CreateInstance<SquareAnnotation>()), (Func<PdfPage, BaseAnnotation, PdfAnnotation>) ((p, a) => (PdfAnnotation) new PdfSquareAnnotation(p)));
      Type key9 = typeof (LinkAnnotation);
      dictionary[key9] = new AnnotationFactory.AnnotationTypeData(typeof (LinkAnnotation), typeof (PdfLinkAnnotation), AnnotationMode.Link, (Func<BaseAnnotation>) (() => (BaseAnnotation) AnnotationFactory.CreateInstance<LinkAnnotation>()), (Func<PdfPage, BaseAnnotation, PdfAnnotation>) ((p, a) => (PdfAnnotation) new PdfLinkAnnotation(p)));
      Type key10 = typeof (CircleAnnotation);
      dictionary[key10] = new AnnotationFactory.AnnotationTypeData(typeof (CircleAnnotation), typeof (PdfCircleAnnotation), AnnotationMode.Ellipse, (Func<BaseAnnotation>) (() => (BaseAnnotation) AnnotationFactory.CreateInstance<CircleAnnotation>()), (Func<PdfPage, BaseAnnotation, PdfAnnotation>) ((p, a) => (PdfAnnotation) new PdfCircleAnnotation(p)));
      Type key11 = typeof (TextAnnotation);
      dictionary[key11] = new AnnotationFactory.AnnotationTypeData(typeof (TextAnnotation), typeof (PdfTextAnnotation), AnnotationMode.Note, (Func<BaseAnnotation>) (() => (BaseAnnotation) AnnotationFactory.CreateInstance<TextAnnotation>()), (Func<PdfPage, BaseAnnotation, PdfAnnotation>) ((p, a) => (PdfAnnotation) new PdfTextAnnotation(p)));
      Type key12 = typeof (StampAnnotation);
      dictionary[key12] = new AnnotationFactory.AnnotationTypeData(typeof (StampAnnotation), typeof (PdfStampAnnotation), new AnnotationMode[2]
      {
        AnnotationMode.Stamp,
        AnnotationMode.Signature
      }, (Func<BaseAnnotation>) (() => (BaseAnnotation) AnnotationFactory.CreateInstance<StampAnnotation>()), (Func<PdfPage, BaseAnnotation, PdfAnnotation>) ((p, a) => (PdfAnnotation) new PdfStampAnnotation(p)), (Func<PdfAnnotation, AnnotationMode>) (a => AnnotationMode.Stamp), (Func<BaseAnnotation, AnnotationMode>) (a => AnnotationMode.Stamp));
      Type key13 = typeof (WatermarkAnnotation);
      dictionary[key13] = new AnnotationFactory.AnnotationTypeData(typeof (WatermarkAnnotation), typeof (PdfWatermarkAnnotation), AnnotationMode.Watermark, (Func<BaseAnnotation>) (() => (BaseAnnotation) AnnotationFactory.CreateInstance<WatermarkAnnotation>()), (Func<PdfPage, BaseAnnotation, PdfAnnotation>) ((p, a) => (PdfAnnotation) new PdfWatermarkAnnotation(p)));
      AnnotationFactory.annotTypes = dictionary;
    }
    if (AnnotationFactory.pdfAnnotTypes == null)
      AnnotationFactory.pdfAnnotTypes = AnnotationFactory.annotTypes.Select<KeyValuePair<Type, AnnotationFactory.AnnotationTypeData>, AnnotationFactory.AnnotationTypeData>((Func<KeyValuePair<Type, AnnotationFactory.AnnotationTypeData>, AnnotationFactory.AnnotationTypeData>) (c => c.Value)).Where<AnnotationFactory.AnnotationTypeData>((Func<AnnotationFactory.AnnotationTypeData, bool>) (c => c.PdfAnnotationType != (Type) null)).GroupBy<AnnotationFactory.AnnotationTypeData, Type>((Func<AnnotationFactory.AnnotationTypeData, Type>) (c => c.PdfAnnotationType)).Where<IGrouping<Type, AnnotationFactory.AnnotationTypeData>>((Func<IGrouping<Type, AnnotationFactory.AnnotationTypeData>, bool>) (c => c.Count<AnnotationFactory.AnnotationTypeData>() > 0)).ToDictionary<IGrouping<Type, AnnotationFactory.AnnotationTypeData>, Type, AnnotationFactory.AnnotationTypeData>((Func<IGrouping<Type, AnnotationFactory.AnnotationTypeData>, Type>) (c => c.Key), (Func<IGrouping<Type, AnnotationFactory.AnnotationTypeData>, AnnotationFactory.AnnotationTypeData>) (c => c.First<AnnotationFactory.AnnotationTypeData>()));
    if (AnnotationFactory.annotModeData != null)
      return;
    AnnotationFactory.annotModeData = AnnotationFactory.annotTypes.SelectMany<KeyValuePair<Type, AnnotationFactory.AnnotationTypeData>, AnnotationMode, (AnnotationMode, AnnotationFactory.AnnotationTypeData)>((Func<KeyValuePair<Type, AnnotationFactory.AnnotationTypeData>, IEnumerable<AnnotationMode>>) (c => (IEnumerable<AnnotationMode>) c.Value.Modes), (Func<KeyValuePair<Type, AnnotationFactory.AnnotationTypeData>, AnnotationMode, (AnnotationMode, AnnotationFactory.AnnotationTypeData)>) ((c, mode) => (mode, c.Value))).GroupBy<(AnnotationMode, AnnotationFactory.AnnotationTypeData), AnnotationMode, AnnotationFactory.AnnotationTypeData>((Func<(AnnotationMode, AnnotationFactory.AnnotationTypeData), AnnotationMode>) (c => c.mode), (Func<(AnnotationMode, AnnotationFactory.AnnotationTypeData), AnnotationFactory.AnnotationTypeData>) (c => c.Value)).Where<IGrouping<AnnotationMode, AnnotationFactory.AnnotationTypeData>>((Func<IGrouping<AnnotationMode, AnnotationFactory.AnnotationTypeData>, bool>) (c => c.Count<AnnotationFactory.AnnotationTypeData>() > 0)).ToDictionary<IGrouping<AnnotationMode, AnnotationFactory.AnnotationTypeData>, AnnotationMode, AnnotationFactory.AnnotationTypeData>((Func<IGrouping<AnnotationMode, AnnotationFactory.AnnotationTypeData>, AnnotationMode>) (c => c.Key), (Func<IGrouping<AnnotationMode, AnnotationFactory.AnnotationTypeData>, AnnotationFactory.AnnotationTypeData>) (c => c.First<AnnotationFactory.AnnotationTypeData>()));
  }

  private static bool TryGetAnnotationDataCore(
    Type annotModeType,
    Type pdfAnnotType,
    out AnnotationFactory.AnnotationTypeData data)
  {
    data = (AnnotationFactory.AnnotationTypeData) null;
    if (annotModeType == (Type) null && pdfAnnotType == (Type) null)
      return false;
    if (annotModeType != (Type) null)
      return AnnotationFactory.AnnotTypes.TryGetValue(annotModeType, out data);
    return pdfAnnotType != (Type) null && AnnotationFactory.PdfAnnotTypes.TryGetValue(pdfAnnotType, out data);
  }

  private static bool TryGetAnnotationDataFromModel<T>(out AnnotationFactory.AnnotationTypeData data) where T : BaseAnnotation
  {
    return AnnotationFactory.TryGetAnnotationDataCore(typeof (T), (Type) null, out data);
  }

  private static bool TryGetAnnotationDataFromPdfType<T>(
    out AnnotationFactory.AnnotationTypeData data)
    where T : PdfAnnotation
  {
    return AnnotationFactory.TryGetAnnotationDataCore((Type) null, typeof (T), out data);
  }

  public static BaseAnnotation Create(PdfAnnotation pdfAnnotation)
  {
    if ((PdfWrapper) pdfAnnotation == (PdfWrapper) null)
      return (BaseAnnotation) null;
    BaseAnnotation instance = AnnotationFactory.CreateInstance(pdfAnnotation);
    BaseAnnotation.InitModelProperties(pdfAnnotation, instance);
    return instance;
  }

  public static PdfAnnotation Create(PdfPage page, BaseAnnotation annotation)
  {
    if (annotation == null)
      return (PdfAnnotation) null;
    PdfAnnotation annot = (PdfAnnotation) null;
    AnnotationFactory.AnnotationTypeData data;
    if (AnnotationFactory.TryGetAnnotationDataCore(annotation.GetType(), (Type) null, out data))
      annot = data.Create(page, annotation);
    int num = (PdfWrapper) annot == (PdfWrapper) null ? 1 : 0;
    if ((PdfWrapper) annot != (PdfWrapper) null)
      annotation.Apply(annot);
    return annot;
  }

  public static System.Collections.Generic.IReadOnlyList<BaseAnnotation> Create(PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    if (page.Annots == null)
      page.CreateAnnotations();
    if (page.Annots == null || page.Annots.Count == 0)
      return (System.Collections.Generic.IReadOnlyList<BaseAnnotation>) Array.Empty<BaseAnnotation>();
    BaseAnnotation[] array = page.Annots.Select<PdfAnnotation, BaseAnnotation>((Func<PdfAnnotation, BaseAnnotation>) (c => AnnotationFactory.Create(c))).ToArray<BaseAnnotation>();
    AnnotationFactory.CreateRelations(page.Annots, array);
    return (System.Collections.Generic.IReadOnlyList<BaseAnnotation>) array;
  }

  public static System.Collections.Generic.IReadOnlyList<AnnotationMode> GetAnnotationModes(
    BaseAnnotation baseAnnotation)
  {
    AnnotationFactory.AnnotationTypeData data;
    if (!AnnotationFactory.TryGetAnnotationDataCore(baseAnnotation?.GetType(), (Type) null, out data))
      return (System.Collections.Generic.IReadOnlyList<AnnotationMode>) Array.Empty<AnnotationMode>();
    AnnotationMode mode = data.GetAnnotationMode(baseAnnotation);
    if (mode == AnnotationMode.None)
      return data.Modes;
    List<AnnotationMode> list = data.Modes.Where<AnnotationMode>((Func<AnnotationMode, bool>) (c => c != mode)).ToList<AnnotationMode>();
    list.Insert(0, mode);
    return (System.Collections.Generic.IReadOnlyList<AnnotationMode>) list;
  }

  public static System.Collections.Generic.IReadOnlyList<AnnotationMode> GetAnnotationModes(
    PdfAnnotation selectedAnnotation)
  {
    AnnotationFactory.AnnotationTypeData data;
    if (!AnnotationFactory.TryGetAnnotationDataCore((Type) null, selectedAnnotation?.GetType(), out data))
      return (System.Collections.Generic.IReadOnlyList<AnnotationMode>) Array.Empty<AnnotationMode>();
    AnnotationMode mode = data.GetAnnotationMode(selectedAnnotation);
    if (mode == AnnotationMode.None)
      return data.Modes;
    List<AnnotationMode> list = data.Modes.Where<AnnotationMode>((Func<AnnotationMode, bool>) (c => c != mode)).ToList<AnnotationMode>();
    list.Insert(0, mode);
    return (System.Collections.Generic.IReadOnlyList<AnnotationMode>) list;
  }

  private static void CreateRelations(PdfAnnotationCollection source, BaseAnnotation[] annotations)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (annotations == null)
      throw new ArgumentNullException(nameof (annotations));
    if (source.Count != annotations.Length)
      throw new ArgumentException();
    if (annotations.Length == 0)
      return;
    for (int index1 = 0; index1 < source.Count; ++index1)
    {
      if (source[index1] is PdfMarkupAnnotation markupAnnotation)
      {
        if ((PdfWrapper) markupAnnotation.RelationshipAnnotation != (PdfWrapper) null)
        {
          int index2 = source.IndexOf(markupAnnotation.RelationshipAnnotation);
          if (index2 >= 0)
            (annotations[index1] as BaseMarkupAnnotation).RelationshipAnnotation = annotations[index2];
        }
        if ((PdfWrapper) markupAnnotation.Popup != (PdfWrapper) null)
        {
          int index3 = source.IndexOf((PdfAnnotation) markupAnnotation.Popup);
          if (index3 >= 0)
          {
            (annotations[index1] as BaseMarkupAnnotation).Popup = (PopupAnnotation) annotations[index3];
            (annotations[index3] as PopupAnnotation).Parent = annotations[index1];
          }
        }
      }
    }
  }

  private static BaseAnnotation CreateInstance(PdfAnnotation pdfAnnotation)
  {
    if ((PdfWrapper) pdfAnnotation == (PdfWrapper) null)
      return (BaseAnnotation) null;
    AnnotationFactory.AnnotationTypeData data;
    if (AnnotationFactory.TryGetAnnotationDataCore((Type) null, pdfAnnotation?.GetType(), out data))
      return data.CreateModel();
    return pdfAnnotation is PdfMarkupAnnotation ? (BaseAnnotation) AnnotationFactory.CreateInstance<NotImplementedMarkupAnnotation>() : (BaseAnnotation) AnnotationFactory.CreateInstance<NotImplementedAnnotation>();
  }

  private static T CreateInstance<T>() where T : new() => new T();

  [Conditional("DEBUG")]
  public static void ValidRelationship(System.Collections.Generic.IReadOnlyList<BaseAnnotation> annotations)
  {
    if (annotations == null)
      return;
    for (int index = 0; index < annotations.Count; ++index)
    {
      BaseAnnotation annotation = annotations[index];
      if (annotation is BaseMarkupAnnotation markupAnnotation)
      {
        if (markupAnnotation.RelationshipAnnotation != null && !annotations.Contains<BaseAnnotation>(markupAnnotation.RelationshipAnnotation))
          throw new ArgumentException();
        if (markupAnnotation.Popup != null && !annotations.Contains<BaseAnnotation>((BaseAnnotation) markupAnnotation.Popup))
          throw new ArgumentException();
      }
      if (annotation is PopupAnnotation popupAnnotation && popupAnnotation.Parent != null)
        annotations.Contains<BaseAnnotation>(popupAnnotation.Parent);
    }
  }

  private class AnnotationTypeData
  {
    private Func<BaseAnnotation> createModelFunc;
    private Func<PdfPage, BaseAnnotation, PdfAnnotation> createFunc;
    private readonly Func<PdfAnnotation, AnnotationMode> getAnnotModeFunc1;
    private readonly Func<BaseAnnotation, AnnotationMode> getAnnotModeFunc2;

    public AnnotationTypeData(
      Type annotationModeType,
      Type pdfAnnotationType,
      AnnotationMode mode,
      Func<BaseAnnotation> createModelFunc,
      Func<PdfPage, BaseAnnotation, PdfAnnotation> createFunc)
    {
      this.AnnotationModeType = annotationModeType;
      this.PdfAnnotationType = pdfAnnotationType;
      this.Modes = (System.Collections.Generic.IReadOnlyList<AnnotationMode>) new AnnotationMode[1]
      {
        mode
      };
      this.createModelFunc = createModelFunc;
      this.createFunc = createFunc;
    }

    public AnnotationTypeData(
      Type annotationModeType,
      Type pdfAnnotationType,
      AnnotationMode[] modes,
      Func<BaseAnnotation> createModelFunc,
      Func<PdfPage, BaseAnnotation, PdfAnnotation> createFunc,
      Func<PdfAnnotation, AnnotationMode> getAnnotModeFunc1,
      Func<BaseAnnotation, AnnotationMode> getAnnotModeFunc2)
    {
      this.AnnotationModeType = annotationModeType;
      this.PdfAnnotationType = pdfAnnotationType;
      this.Modes = (System.Collections.Generic.IReadOnlyList<AnnotationMode>) modes;
      this.createModelFunc = createModelFunc;
      this.createFunc = createFunc;
      this.getAnnotModeFunc1 = getAnnotModeFunc1;
      this.getAnnotModeFunc2 = getAnnotModeFunc2;
    }

    public Type AnnotationModeType { get; }

    public Type PdfAnnotationType { get; }

    public BaseAnnotation CreateModel() => this.createModelFunc();

    public PdfAnnotation Create(PdfPage page, BaseAnnotation annotation)
    {
      return this.createFunc(page, annotation);
    }

    public System.Collections.Generic.IReadOnlyList<AnnotationMode> Modes { get; }

    public AnnotationMode GetAnnotationMode(PdfAnnotation instance)
    {
      if (this.Modes != null && this.Modes.Count == 1 && this.Modes[0] != AnnotationMode.None)
        return this.Modes[0];
      return this.getAnnotModeFunc1 != null ? this.getAnnotModeFunc1(instance) : this.GetDefaultAnnotationModeCore();
    }

    public AnnotationMode GetAnnotationMode(BaseAnnotation instance)
    {
      if (this.Modes != null && this.Modes.Count == 1 && this.Modes[0] != AnnotationMode.None)
        return this.Modes[0];
      return this.getAnnotModeFunc2 != null ? this.getAnnotModeFunc2(instance) : this.GetDefaultAnnotationModeCore();
    }

    private AnnotationMode GetDefaultAnnotationModeCore()
    {
      if (this.Modes != null)
      {
        foreach (AnnotationMode mode in (IEnumerable<AnnotationMode>) this.Modes)
        {
          if (mode != AnnotationMode.None)
            return mode;
        }
      }
      return AnnotationMode.None;
    }
  }
}
