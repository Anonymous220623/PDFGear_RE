// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.StampAnnotation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using PDFKit.Utils;
using System;
using System.Linq;

#nullable disable
namespace pdfeditor.Models.Annotations;

public class StampAnnotation : BaseMarkupAnnotation
{
  public string ExtendedIconName { get; protected set; }

  public FS_MATRIX ImageMatrix { get; protected set; }

  public new string Contents { get; protected set; }

  public new FS_COLOR Color { get; protected set; }

  public PdfImageObjectModel ImageObject { get; protected set; }

  public bool IsRemoveBg { get; protected set; }

  public string ApplyId { get; protected set; }

  public int[] ApplyPageIndexs { get; protected set; }

  public int[] ImageSource { get; protected set; }

  public PdfTypeDictionary PDFXExtend { get; protected set; }

  protected override void Init(PdfAnnotation pdfAnnotation)
  {
    base.Init(pdfAnnotation);
    PdfStampAnnotation annot = pdfAnnotation as PdfStampAnnotation;
    if (annot == null)
      return;
    PdfPageObjectsCollection normalAppearance = annot.NormalAppearance;
    PdfImageObject imageObject = normalAppearance != null ? normalAppearance.OfType<PdfImageObject>().FirstOrDefault<PdfImageObject>() : (PdfImageObject) null;
    this.IsRemoveBg = annot.Dictionary.ContainsKey("IsRemoveBg");
    if (annot.Dictionary.ContainsKey("ApplyId") && annot.Dictionary["ApplyId"].Is<PdfTypeString>())
      this.ApplyId = annot.Dictionary["ApplyId"].As<PdfTypeString>().UnicodeString.Trim();
    if (annot.Dictionary.ContainsKey("ApplyRange") && annot.Dictionary["ApplyRange"].Is<PdfTypeArray>())
    {
      PdfTypeBase[] array = annot.Dictionary["ApplyRange"].As<PdfTypeArray>().ToArray<PdfTypeBase>();
      int[] numArray = new int[array.Length];
      for (int index = 0; index < array.Length; ++index)
        numArray[index] = (array[index] as PdfTypeNumber).IntValue;
      this.ApplyPageIndexs = numArray;
    }
    if (imageObject?.Bitmap != null)
    {
      this.ImageObject = new PdfImageObjectModel(imageObject);
      this.ImageMatrix = imageObject.Matrix;
    }
    this.ExtendedIconName = BaseAnnotation.ReturnValueOrDefault<string>((Func<string>) (() => annot.ExtendedIconName));
    this.Contents = BaseAnnotation.ReturnValueOrDefault<string>((Func<string>) (() => annot.Contents));
    this.Color = BaseAnnotation.ReturnValueOrDefault<FS_COLOR>((Func<FS_COLOR>) (() => annot.Color));
    this.Rectangle = BaseAnnotation.ReturnValueOrDefault<FS_RECTF>((Func<FS_RECTF>) (() => annot.GetRECT()));
    PdfTypeBase pdfTypeBase;
    this.PDFXExtend = BaseAnnotation.ReturnValueOrDefault<PdfTypeDictionary>((Func<PdfTypeDictionary>) (() => annot.Dictionary.TryGetValue("PDFXExtend", out pdfTypeBase) && pdfTypeBase != null && pdfTypeBase.Is<PdfTypeDictionary>() ? (PdfTypeDictionary) PageDisposeHelper.DeepClone((PdfTypeBase) pdfTypeBase.As<PdfTypeDictionary>()) : (PdfTypeDictionary) null));
  }

  protected override void ApplyCore(PdfAnnotation annot)
  {
    base.ApplyCore(annot);
    if (!(annot is PdfStampAnnotation pdfStampAnnotation))
      return;
    pdfStampAnnotation.ExtendedIconName = this.ExtendedIconName;
    if (!string.IsNullOrEmpty(this.ApplyId))
      pdfStampAnnotation.Dictionary["ApplyId"] = (PdfTypeBase) PdfTypeString.Create(this.ApplyId);
    if (this.ApplyPageIndexs != null && this.ApplyPageIndexs.Length != 0)
    {
      PdfTypeArray pdfTypeArray = PdfTypeArray.Create();
      for (int index = 0; index < this.ApplyPageIndexs.Length; ++index)
        pdfTypeArray.Add((PdfTypeBase) PdfTypeNumber.Create(this.ApplyPageIndexs[index]));
      pdfStampAnnotation.Dictionary["ApplyRange"] = (PdfTypeBase) pdfTypeArray;
    }
    if ((pdfStampAnnotation.NormalAppearance == null || pdfStampAnnotation.NormalAppearance.Count == 0) && this.ImageObject != null)
    {
      pdfStampAnnotation.CreateEmptyAppearance(AppearanceStreamModes.Normal);
      PdfImageObject pdfImageObject = PdfImageObject.Create(annot.Page.Document, this.ImageObject.Bitmap.Clone(), annot.GetRECT().left, annot.GetRECT().top);
      pdfImageObject.Matrix = this.ImageMatrix;
      if (this.IsRemoveBg)
        pdfImageObject.BlendMode = BlendTypes.FXDIB_BLEND_MULTIPLY;
      pdfStampAnnotation.NormalAppearance.Add((PdfPageObject) pdfImageObject);
      if (this.ImageObject.SoftMask != null)
      {
        PdfIndirectList list = PdfIndirectList.FromPdfDocument(annot.Page.Document);
        int objectNumber = list.Add(this.ImageObject.SoftMask.Clone());
        if (pdfImageObject.Stream == null)
          Pdfium.FPDFImageObj_GenerateStream(pdfImageObject.Handle, annot.Page.Handle);
        pdfImageObject.Stream.Dictionary.SetIndirectAt("SMask", list, objectNumber);
      }
      pdfStampAnnotation.GenerateAppearance(AppearanceStreamModes.Normal);
    }
    else
    {
      pdfStampAnnotation.Contents = this.ExtendedIconName;
      pdfStampAnnotation.Color = this.Color;
    }
    if (this.PDFXExtend != null)
      pdfStampAnnotation.Dictionary["PDFXExtend"] = PageDisposeHelper.DeepClone((PdfTypeBase) this.PDFXExtend);
    pdfStampAnnotation.Rectangle = this.Rectangle;
  }

  protected override bool EqualsCore(BaseAnnotation other)
  {
    if (!base.EqualsCore(other) || !(other is StampAnnotation stampAnnotation) || !(other.Color == this.Color) || !(other.Contents == this.Contents) || !(other.Rectangle == this.Rectangle))
      return false;
    if (this.PDFXExtend == null && stampAnnotation.PDFXExtend == null)
      return true;
    PdfTypeBase pdfTypeBase1;
    PdfTypeBase pdfTypeBase2;
    if (this.PDFXExtend == null || stampAnnotation.PDFXExtend == null || !this.PDFXExtend.TryGetValue("Type", out pdfTypeBase1) || !pdfTypeBase1.Is<PdfTypeName>() || !stampAnnotation.PDFXExtend.TryGetValue("Type", out pdfTypeBase2) || !pdfTypeBase2.Is<PdfTypeName>())
      return false;
    string str = pdfTypeBase1.As<PdfTypeName>().Value;
    PdfTypeBase pdfTypeBase3;
    PdfTypeBase pdfTypeBase4;
    return !(str != pdfTypeBase2.As<PdfTypeName>().Value) && (!(str == "Stamp") || !this.PDFXExtend.TryGetValue("Content", out pdfTypeBase3) || !pdfTypeBase3.Is<PdfTypeString>() || !stampAnnotation.PDFXExtend.TryGetValue("Content", out pdfTypeBase4) || !pdfTypeBase4.Is<PdfTypeString>() || !(pdfTypeBase3.As<PdfTypeString>().UnicodeString != pdfTypeBase4.As<PdfTypeString>().UnicodeString));
  }
}
