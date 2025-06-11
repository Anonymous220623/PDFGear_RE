// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.StampUtils.StampUtil
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using PDFKit.Utils.StampUtils.StampTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Utils.StampUtils;

public static class StampUtil
{
  private static readonly System.Collections.Generic.IReadOnlyList<StampTemplateBase> stampTemplates = (System.Collections.Generic.IReadOnlyList<StampTemplateBase>) new StampTemplateBase[4]
  {
    (StampTemplateBase) new StampFormControlTemplate(),
    (StampTemplateBase) new StampDefaultTextTemplate(),
    (StampTemplateBase) new StampImageTemplate(),
    (StampTemplateBase) new StampChop1Template()
  };

  internal static IReadOnlyDictionary<StampIconNames, string> StandardIconNameContent { get; private set; }

  public static void InitStandardIconNameContent(Dictionary<StampIconNames, string> dict)
  {
    StampUtil.StandardIconNameContent = (IReadOnlyDictionary<StampIconNames, string>) dict.ToDictionary<KeyValuePair<StampIconNames, string>, StampIconNames, string>((Func<KeyValuePair<StampIconNames, string>, StampIconNames>) (c => c.Key), (Func<KeyValuePair<StampIconNames, string>, string>) (c => c.Value));
  }

  public static void SetPDFXStampDictionary(PdfStampAnnotation annot, PDFExtStampDictionary dict)
  {
    if (dict == null || dict.AllPropertiesAreDefault)
    {
      if (!annot.Dictionary.ContainsKey("PDFXExtend"))
        return;
      annot.Dictionary.Remove("PDFXExtend");
    }
    else
    {
      PdfTypeDictionary pdfTypeDictionary = PdfTypeDictionary.Create();
      pdfTypeDictionary["Type"] = (PdfTypeBase) PdfTypeName.Create(dict.Type ?? "");
      if (pdfTypeDictionary.ContainsKey("Content"))
        pdfTypeDictionary.Remove("Content");
      if (!string.IsNullOrEmpty(dict.Content))
        pdfTypeDictionary["Content"] = (PdfTypeBase) PdfTypeString.Create(dict.Content, true);
      if (pdfTypeDictionary.ContainsKey("Template"))
        pdfTypeDictionary.Remove("Template");
      if (!string.IsNullOrEmpty(dict.Template))
        pdfTypeDictionary["Template"] = (PdfTypeBase) PdfTypeString.Create(dict.Template, true);
      annot.Dictionary["PDFXExtend"] = (PdfTypeBase) pdfTypeDictionary;
    }
  }

  public static PDFExtStampDictionary GetPDFExtStampDictionary(PdfStampAnnotation annot)
  {
    if (annot.Dictionary.ContainsKey("PDFXExtend") && annot.Dictionary["PDFXExtend"].Is<PdfTypeDictionary>())
    {
      PdfTypeDictionary pdfTypeDictionary = annot.Dictionary["PDFXExtend"].As<PdfTypeDictionary>();
      PDFExtStampDictionary extStampDictionary = new PDFExtStampDictionary();
      if (pdfTypeDictionary.ContainsKey("Type") && pdfTypeDictionary["Type"].Is<PdfTypeName>())
        extStampDictionary.Type = pdfTypeDictionary["Type"].As<PdfTypeName>().Value;
      if (pdfTypeDictionary.ContainsKey("Content") && pdfTypeDictionary["Content"].Is<PdfTypeString>())
        extStampDictionary.Content = pdfTypeDictionary["Content"].As<PdfTypeString>().UnicodeString;
      if (pdfTypeDictionary.ContainsKey("Template") && pdfTypeDictionary["Template"].Is<PdfTypeString>())
        extStampDictionary.Template = pdfTypeDictionary["Template"].As<PdfTypeString>().UnicodeString;
      if (!extStampDictionary.AllPropertiesAreDefault)
        return extStampDictionary;
    }
    return (PDFExtStampDictionary) null;
  }

  public static bool TryGetStandardIconName(string str, out StampIconNames iconName)
  {
    iconName = StampIconNames.Extended;
    if (StampUtil.StandardIconNameContent == null)
      return false;
    foreach (KeyValuePair<StampIconNames, string> keyValuePair in (IEnumerable<KeyValuePair<StampIconNames, string>>) StampUtil.StandardIconNameContent)
    {
      if (keyValuePair.Value == str)
      {
        iconName = keyValuePair.Key;
        return true;
      }
    }
    return false;
  }

  public static bool IsTextStamp(PdfStampAnnotation annot)
  {
    PDFExtStampDictionary extStampDictionary = StampUtil.GetPDFExtStampDictionary(annot);
    StampDefaultTextTemplate defaultTextTemplate = StampUtil.stampTemplates.OfType<StampDefaultTextTemplate>().FirstOrDefault<StampDefaultTextTemplate>();
    return defaultTextTemplate != null && defaultTextTemplate.IsMatch(annot, extStampDictionary);
  }

  public static string GetStampTextContent(PdfStampAnnotation annot)
  {
    PDFExtStampDictionary extStampDictionary = StampUtil.GetPDFExtStampDictionary(annot);
    Dictionary<string, string> contentDictionary = StampUtil.GetTemplate(annot, extStampDictionary)?.GetContentDictionary(annot, extStampDictionary);
    return StampDefaultTextTemplate.GetDefaultText(annot, extStampDictionary, contentDictionary);
  }

  public static Geometry CreateStampFormControlPreviewGrometry(string name, out Size geometrySize)
  {
    return StampFormObjectDrawingHelper.GetGeometry(name, out geometrySize);
  }

  internal static StampTemplateBase GetTemplate(
    PdfStampAnnotation annot,
    PDFExtStampDictionary extDict)
  {
    foreach (StampTemplateBase stampTemplate in (IEnumerable<StampTemplateBase>) StampUtil.stampTemplates)
    {
      if (stampTemplate.IsMatch(annot, extDict))
        return stampTemplate;
    }
    return (StampTemplateBase) null;
  }

  internal static void GenerateAppearance(PdfStampAnnotation annot)
  {
    if (annot == null)
      throw new ArgumentNullException(nameof (annot));
    PageRotate adjustRotate;
    annot.GetRotate(out PageRotate? _, out adjustRotate);
    FS_RECTF rect1 = annot.GetRECT();
    FS_POINTF anchorPoint1 = PdfRotateUtils.GetAnchorPoint(rect1, adjustRotate);
    FS_RECTF originalRectangle = PdfRotateUtils.GetOriginalRectangle(rect1, adjustRotate);
    annot.Rectangle = originalRectangle;
    PDFExtStampDictionary extStampDictionary = StampUtil.GetPDFExtStampDictionary(annot);
    StampTemplateBase template = StampUtil.GetTemplate(annot, extStampDictionary);
    if (template != null && !template.RegenerateAppearances(annot, extStampDictionary))
    {
      FS_RECTF fsRectf = rect1;
      fsRectf.left += 2f;
      fsRectf.right -= 2f;
      fsRectf.top -= 2f;
      fsRectf.bottom += 2f;
      annot.Rectangle = rect1;
      annot.NormalAppearance?.Clear();
      annot.CreateEmptyAppearance(AppearanceStreamModes.Normal);
      annot.RegenerateAppearances();
    }
    foreach (PdfPageObject pdfPageObject in annot.NormalAppearance)
    {
      FS_MATRIX matrix = pdfPageObject.Matrix;
      matrix.Translate((float) (-(double) rect1.Width / 2.0), (float) (-(double) rect1.Height / 2.0));
      PdfRotateUtils.RotateMatrix(adjustRotate, matrix);
      matrix.Translate(rect1.Width / 2f, rect1.Height / 2f);
      pdfPageObject.Matrix = matrix;
    }
    annot.GenerateAppearance(AppearanceStreamModes.Normal);
    FS_RECTF rect2 = annot.GetRECT();
    FS_POINTF anchorPoint2 = PdfRotateUtils.GetAnchorPoint(rect2, adjustRotate);
    float num1 = anchorPoint1.X - anchorPoint2.X;
    float num2 = anchorPoint1.Y - anchorPoint2.Y;
    rect2.left += num1;
    rect2.right += num1;
    rect2.top += num2;
    rect2.bottom += num2;
    annot.Rectangle = rect2;
  }

  public static Visual CreateDefaultTextPreviewVisual(
    string content,
    double width,
    double height,
    Color color,
    string dateTimeFormat,
    string locale,
    double borderThickness)
  {
    StampAnnotationData data = new StampAnnotationData()
    {
      Content = content,
      Width = width,
      Height = height,
      Color = color,
      CreateDate = new DateTimeOffset?((DateTimeOffset) DateTime.Now),
      ContentDictionary = new Dictionary<string, string>()
      {
        ["TimeFormat"] = dateTimeFormat,
        ["Locale"] = locale,
        ["BorderThickness"] = $"{borderThickness}"
      }
    };
    return StampUtil.stampTemplates.OfType<StampDefaultTextTemplate>().FirstOrDefault<StampDefaultTextTemplate>().CreatePreviewVisual(data);
  }

  public static async Task<bool> FlattenAnnotationAsync(PdfAnnotation annot)
  {
    bool flag = await Task.Run<bool>(TaskExceptionHelper.ExceptionBoundary<bool>((Func<bool>) (() => PdfAnnotationExtensions.FlattenAnnotation(annot)))).ConfigureAwait(false);
    return flag;
  }

  public static async Task<bool> FlattenAnnotations(
    PdfPage page,
    System.Collections.Generic.IReadOnlyList<PdfAnnotation> annots)
  {
    bool flag = await Task.Run<bool>(TaskExceptionHelper.ExceptionBoundary<bool>((Func<bool>) (() => PdfAnnotationExtensions.FlattenAnnotations(page, annots)))).ConfigureAwait(false);
    return flag;
  }

  public static bool IsFormControl(PdfStampAnnotation annot)
  {
    PdfTypeBase pdfTypeBase1;
    PdfTypeBase pdfTypeBase2;
    return annot != null && annot.Dictionary.TryGetValue("PDFXExtend", out pdfTypeBase1) && pdfTypeBase1.Is<PdfTypeDictionary>() && pdfTypeBase1.As<PdfTypeDictionary>().TryGetValue("Type", out pdfTypeBase2) && pdfTypeBase2.Is<PdfTypeName>() && pdfTypeBase2.As<PdfTypeName>().Value == "FormControl";
  }
}
