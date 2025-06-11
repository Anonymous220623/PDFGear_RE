// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PageHeaderFooters.PageHeaderFooterUtils
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using PDFKit.Utils.PageContents;
using PDFKit.Utils.PdfRichTextStrings;
using PDFKit.Utils.XObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

#nullable disable
namespace PDFKit.Utils.PageHeaderFooters;

public class PageHeaderFooterUtils
{
  private static Func<PdfFormObject, PdfTypeStream> pdfFormObjectStreamGetter;
  private const double inchPreCm = 0.39370078740157;

  public static System.Collections.Generic.IReadOnlyList<(PdfFormObject obj, int idx)> GetHeaderFooterObjects(
    PdfPage page)
  {
    return page == null ? (System.Collections.Generic.IReadOnlyList<(PdfFormObject, int)>) Array.Empty<(PdfFormObject, int)>() : (System.Collections.Generic.IReadOnlyList<(PdfFormObject, int)>) PageHeaderFooterUtils.GetPaginationObject(page).ToArray<(PdfFormObject, int)>();
  }

  public static IEnumerable<(PdfFormObject obj, int idx)> GetPaginationObject(PdfPage page)
  {
    if (page != null)
    {
      for (int i = 0; i < page.PageObjects.Count; ++i)
      {
        PdfPageObject obj = page.PageObjects[i];
        if (obj.ObjectType == PageObjectTypes.PDFPAGE_FORM)
        {
          if (obj.MarkedContent != null && obj.MarkedContent.Count > 0)
          {
            foreach (PdfMarkedContent markedContent in obj.MarkedContent)
            {
              if (markedContent.Tag == "Artifact")
              {
                PdfTypeDictionary dict = markedContent.Parameters;
                if (dict != null)
                {
                  PdfTypeName typeValue = XObjectHelper.GetDirectValue<PdfTypeName>(dict, "Type");
                  if (XObjectHelper.GetDirectValue<PdfTypeName>(dict, "Type")?.Value == "Pagination" && XObjectHelper.GetDirectValue<PdfTypeName>(dict, "Subtype")?.Value == "Header")
                    yield return ((PdfFormObject) obj, i);
                  typeValue = (PdfTypeName) null;
                }
                dict = (PdfTypeDictionary) null;
              }
            }
          }
          else if (PageHeaderFooterUtils.IsHeaderFooterXObject(((PdfFormObject) obj).Stream))
            yield return ((PdfFormObject) obj, i);
        }
        obj = (PdfPageObject) null;
      }
    }
  }

  private static System.Collections.Generic.IReadOnlyList<(string key, PdfTypeStream xobj)> GetHeaderFooterXObjects(
    PdfPage page)
  {
    if (page == null)
      return (System.Collections.Generic.IReadOnlyList<(string, PdfTypeStream)>) Array.Empty<(string, PdfTypeStream)>();
    PdfTypeDictionary xobjectDict = XObjectHelper.GetXObjectDict(page);
    if (xobjectDict == null)
      return (System.Collections.Generic.IReadOnlyList<(string, PdfTypeStream)>) Array.Empty<(string, PdfTypeStream)>();
    List<(string, PdfTypeStream)> headerFooterXobjects = new List<(string, PdfTypeStream)>();
    foreach (KeyValuePair<string, PdfTypeBase> keyValuePair in xobjectDict)
    {
      if (keyValuePair.Value.Is<PdfTypeStream>() && PageHeaderFooterUtils.IsHeaderFooterXObject(keyValuePair.Value.As<PdfTypeStream>()))
        headerFooterXobjects.Add((keyValuePair.Key, keyValuePair.Value.As<PdfTypeStream>()));
    }
    return (System.Collections.Generic.IReadOnlyList<(string, PdfTypeStream)>) headerFooterXobjects;
  }

  internal static System.Collections.Generic.IReadOnlyList<HeaderFooterData> GetPageHeaderFooterSettings(
    PdfPage page)
  {
    if (page == null)
      return (System.Collections.Generic.IReadOnlyList<HeaderFooterData>) Array.Empty<HeaderFooterData>();
    HeaderFooterData.HeaderFooterSettingsData emptyData = HeaderFooterData.HeaderFooterSettingsData.EmptyData;
    Dictionary<string, (HeaderFooterData.HeaderFooterSettingsData, List<int>)> source = new Dictionary<string, (HeaderFooterData.HeaderFooterSettingsData, List<int>)>();
    source[""] = (emptyData, new List<int>());
    System.Collections.Generic.IReadOnlyList<(PdfFormObject obj, int idx)> headerFooterObjects = PageHeaderFooterUtils.GetHeaderFooterObjects(page);
    (string, int[])[] array = headerFooterObjects.Select<(PdfFormObject, int), (string, int)>((Func<(PdfFormObject, int), (string, int)>) (c => (XObjectHelper.GetDocSettingString(page, c.obj.Stream), c.idx))).GroupBy<(string, int), string>((Func<(string, int), string>) (c => c.Item1), (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase).Select<IGrouping<string, (string, int)>, (string, int[])>((Func<IGrouping<string, (string, int)>, (string, int[])>) (c => (c.Key, c.Select<(string, int), int>((Func<(string, int), int>) (x => x.idx)).ToArray<int>()))).ToArray<(string, int[])>();
    if (array != null && array.Length != 0)
    {
      for (int index = 0; index < array.Length; ++index)
      {
        (string, int[]) tuple = array[index];
        string str = tuple.Item1;
        if (string.IsNullOrEmpty(str))
          str = "";
        (HeaderFooterData.HeaderFooterSettingsData, List<int>) valueTuple;
        if (!source.TryGetValue(str, out valueTuple))
        {
          HeaderFooterSettings hfSettings = XObjectHelper.ConvertToHFSettings(str);
          if (hfSettings != null || string.IsNullOrEmpty(str))
          {
            valueTuple = (new HeaderFooterData.HeaderFooterSettingsData()
            {
              Settings = hfSettings,
              SettingsXml = str
            }, new List<int>());
            source[str] = valueTuple;
          }
        }
        if (valueTuple.Item1 != null)
          valueTuple.Item2.AddRange((IEnumerable<int>) tuple.Item2);
      }
    }
    else
      source[""].Item2.AddRange(headerFooterObjects.Select<(PdfFormObject, int), int>((Func<(PdfFormObject, int), int>) (c => c.idx)));
    List<HeaderFooterData> headerFooterDataList = new List<HeaderFooterData>();
    foreach (KeyValuePair<string, (HeaderFooterData.HeaderFooterSettingsData, List<int>)> keyValuePair in (IEnumerable<KeyValuePair<string, (HeaderFooterData.HeaderFooterSettingsData, List<int>)>>) source.OrderBy<KeyValuePair<string, (HeaderFooterData.HeaderFooterSettingsData, List<int>)>, int>((Func<KeyValuePair<string, (HeaderFooterData.HeaderFooterSettingsData, List<int>)>, int>) (c => !string.IsNullOrEmpty(c.Key) ? 0 : int.MaxValue)))
    {
      if (keyValuePair.Value.Item2 != null && keyValuePair.Value.Item2.Count > 0)
      {
        HeaderFooterData headerFooterData = new HeaderFooterData()
        {
          SettingsData = keyValuePair.Value.Item1
        };
        headerFooterData.FormObjects = new Dictionary<int, System.Collections.Generic.IReadOnlyList<int>>()
        {
          [page.PageIndex] = (System.Collections.Generic.IReadOnlyList<int>) keyValuePair.Value.Item2
        };
        headerFooterDataList.Add(headerFooterData);
      }
    }
    return (System.Collections.Generic.IReadOnlyList<HeaderFooterData>) headerFooterDataList.ToArray();
  }

  public static async Task<System.Collections.Generic.IReadOnlyList<HeaderFooterData>> GetHeaderFooterSettingsAsync(
    PdfDocument doc,
    IProgress<double> progress,
    CancellationToken cancellationToken)
  {
    if (doc == null || doc.Pages == null || doc.Pages.Count == 0)
      return (System.Collections.Generic.IReadOnlyList<HeaderFooterData>) Array.Empty<HeaderFooterData>();
    cancellationToken.ThrowIfCancellationRequested();
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(doc);
    (int num1, int num2) = pdfControl != null ? pdfControl.GetVisiblePageRange() : (-1, -1);
    progress?.Report(0.0);
    HeaderFooterData[] footerSettingsAsync = await Task.Run<HeaderFooterData[]>(TaskExceptionHelper.ExceptionBoundary<HeaderFooterData[]>((Func<HeaderFooterData[]>) (() =>
    {
      List<HeaderFooterData> source = new List<HeaderFooterData>();
      for (int index = 0; index < doc.Pages.Count; ++index)
      {
        cancellationToken.ThrowIfCancellationRequested();
        PdfPage page = doc.Pages[index];
        try
        {
          System.Collections.Generic.IReadOnlyList<HeaderFooterData> headerFooterSettings = PageHeaderFooterUtils.GetPageHeaderFooterSettings(page);
          if (headerFooterSettings != null && headerFooterSettings.Count > 0)
            source.AddRange((IEnumerable<HeaderFooterData>) headerFooterSettings);
        }
        finally
        {
          if (page.PageIndex < num1 || page.PageIndex > num2)
            PageDisposeHelper.DisposePage(page);
        }
        progress?.Report(1.0 / (double) doc.Pages.Count * (double) (index + 1));
      }
      cancellationToken.ThrowIfCancellationRequested();
      List<HeaderFooterData> list = source.GroupBy<HeaderFooterData, string>((Func<HeaderFooterData, string>) (c => c.SettingsData.SettingsXml), (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase).Select<IGrouping<string, HeaderFooterData>, (string, HeaderFooterData.HeaderFooterSettingsData, Dictionary<int, System.Collections.Generic.IReadOnlyList<int>>)>((Func<IGrouping<string, HeaderFooterData>, (string, HeaderFooterData.HeaderFooterSettingsData, Dictionary<int, System.Collections.Generic.IReadOnlyList<int>>)>) (c => (c.Key, c.FirstOrDefault<HeaderFooterData>()?.SettingsData, c.SelectMany<HeaderFooterData, KeyValuePair<int, System.Collections.Generic.IReadOnlyList<int>>>((Func<HeaderFooterData, IEnumerable<KeyValuePair<int, System.Collections.Generic.IReadOnlyList<int>>>>) (x => (IEnumerable<KeyValuePair<int, System.Collections.Generic.IReadOnlyList<int>>>) x.FormObjects)).GroupBy<KeyValuePair<int, System.Collections.Generic.IReadOnlyList<int>>, int>((Func<KeyValuePair<int, System.Collections.Generic.IReadOnlyList<int>>, int>) (x => x.Key)).Select<IGrouping<int, KeyValuePair<int, System.Collections.Generic.IReadOnlyList<int>>>, (int, List<int>)>((Func<IGrouping<int, KeyValuePair<int, System.Collections.Generic.IReadOnlyList<int>>>, (int, List<int>)>) (x => (x.Key, x.SelectMany<KeyValuePair<int, System.Collections.Generic.IReadOnlyList<int>>, int>((Func<KeyValuePair<int, System.Collections.Generic.IReadOnlyList<int>>, IEnumerable<int>>) (z => (IEnumerable<int>) z.Value)).ToList<int>()))).ToDictionary<(int, List<int>), int, System.Collections.Generic.IReadOnlyList<int>>((Func<(int, List<int>), int>) (x => x.k), (Func<(int, List<int>), System.Collections.Generic.IReadOnlyList<int>>) (x => (System.Collections.Generic.IReadOnlyList<int>) x.v))))).Select<(string, HeaderFooterData.HeaderFooterSettingsData, Dictionary<int, System.Collections.Generic.IReadOnlyList<int>>), HeaderFooterData>((Func<(string, HeaderFooterData.HeaderFooterSettingsData, Dictionary<int, System.Collections.Generic.IReadOnlyList<int>>), HeaderFooterData>) (c => new HeaderFooterData()
      {
        SettingsData = c.k2,
        FormObjects = c.v
      })).ToList<HeaderFooterData>();
      progress?.Report(1.0);
      return list.ToArray();
    })), cancellationToken).ConfigureAwait(false);
    return (System.Collections.Generic.IReadOnlyList<HeaderFooterData>) footerSettingsAsync;
  }

  public static async Task RemoveAllPageHeaderFooterSettingsAsync(
    PdfDocument doc,
    System.Collections.Generic.IReadOnlyList<HeaderFooterData> hfData,
    IProgress<double> progress)
  {
    PDFKit.PdfControl viewer;
    if (doc == null || hfData == null || hfData.Count == 0)
    {
      viewer = (PDFKit.PdfControl) null;
    }
    else
    {
      viewer = PDFKit.PdfControl.GetPdfControl(doc);
      PDFKit.PdfControl scrollInfo = viewer;
      (int, int)? visiblePageRange = scrollInfo != null ? new (int, int)?(scrollInfo.GetVisiblePageRange()) : new (int, int)?();
      progress?.Report(0.0);
      await Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
      {
        Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
        foreach (HeaderFooterData headerFooterData in (IEnumerable<HeaderFooterData>) hfData)
        {
          if (headerFooterData.FormObjects != null && headerFooterData.FormObjects.Count > 0)
          {
            foreach (KeyValuePair<int, System.Collections.Generic.IReadOnlyList<int>> formObject in headerFooterData.FormObjects)
            {
              List<int> intList;
              if (!dictionary.TryGetValue(formObject.Key, out intList))
              {
                intList = new List<int>();
                dictionary[formObject.Key] = intList;
              }
              intList.AddRange((IEnumerable<int>) formObject.Value);
            }
          }
        }
        foreach (int key in dictionary.Keys.ToArray<int>())
        {
          List<int> source = dictionary[key];
          dictionary[key] = source.Distinct<int>().OrderBy<int, int>((Func<int, int>) (c => c)).ToList<int>();
        }
        int num = 0;
        int count = dictionary.Count;
        foreach (KeyValuePair<int, List<int>> keyValuePair in dictionary)
        {
          int key = keyValuePair.Key;
          List<int> intList = keyValuePair.Value;
          PdfPage page = doc.Pages[key];
          for (int index1 = intList.Count - 1; index1 >= 0; --index1)
          {
            int index2 = intList[index1];
            page.PageObjects.RemoveAt(index2);
            if (index1 > 0)
              progress?.Report(1.0 / (double) count * ((double) num + 1.0 / (double) intList.Count * (double) (intList.Count - index1)));
          }
          page.GenerateContentAdvance(new GenerateContentOptions()
          {
            KeepHeaderFooterData = false
          });
          if (visiblePageRange.HasValue && (page.PageIndex > visiblePageRange.Value.Item2 || page.PageIndex < visiblePageRange.Value.Item1))
            PageDisposeHelper.DisposePage(page);
          ++num;
          progress?.Report(1.0 / (double) count * (double) num);
        }
        progress?.Report(1.0);
      }))).ConfigureAwait(false);
      viewer = (PDFKit.PdfControl) null;
    }
  }

  public static async Task<bool> ApplyHeaderFooterSettingsAsync(
    PdfDocument doc,
    HeaderFooterSettings settings,
    IProgress<double> progress)
  {
    bool flag = await PageHeaderFooterUtils.ApplyHeaderFooterSettingsAsync(doc, settings, (System.Collections.Generic.IReadOnlyList<HeaderFooterData>) null, progress).ConfigureAwait(false);
    return flag;
  }

  public static async Task<bool> ApplyHeaderFooterSettingsAsync(
    PdfDocument doc,
    HeaderFooterSettings settings,
    System.Collections.Generic.IReadOnlyList<HeaderFooterData> hfData,
    IProgress<double> progress)
  {
    if (doc == null || settings == null)
      return false;
    int startPage = Math.Max(1, settings.PageRange.Start);
    int endPage = Math.Min(settings.PageRange.End, doc.Pages.Count);
    if (startPage == -1)
      startPage = 1;
    if (endPage == -1)
      endPage = doc.Pages.Count;
    if (startPage > endPage)
      return false;
    string fontName = string.IsNullOrEmpty(settings.Font.Name) ? "Arial" : settings.Font.Name;
    float fontSize = (float) settings.Font.Size;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(doc);
    (int, int)? visiblePageRange = pdfControl != null ? new (int, int)?(pdfControl.GetVisiblePageRange()) : new (int, int)?();
    DateTimeOffset date = DateTimeOffset.Now;
    progress?.Report(0.0);
    bool flag = await Task.Run<bool>(TaskExceptionHelper.ExceptionBoundary<bool>((Func<bool>) (() =>
    {
      string xml = settings.ToString();
      bool flag1 = settings.PageRange.Odd && settings.PageRange.Even;
      bool odd = settings.PageRange.Odd;
      bool even = settings.PageRange.Even;
      FS_COLOR fsColor = new FS_COLOR((int) (settings.Color.R * (double) byte.MaxValue), (int) (settings.Color.G * (double) byte.MaxValue), (int) (settings.Color.B * (double) byte.MaxValue));
      System.Collections.Generic.IReadOnlyList<PageHeaderFooterUtils.LocationEnum> allValues = EnumHelper<PageHeaderFooterUtils.LocationEnum>.AllValues;
      int[] array = PageHeaderFooterUtils.GetPageRanges(startPage, endPage, odd, even).ToArray<int>();
      int? nullable1 = new int?();
      int? nullable2 = new int?();
      string modificationDateString = PdfAttributeUtils.ConvertToModificationDateString(DateTimeOffset.Now);
      for (int index1 = 0; index1 < array.Length; ++index1)
      {
        int pageIndex = array[index1];
        PdfPage page = doc.Pages[pageIndex];
        (float, float, float, float) margin = ((float) settings.Margin.Left, (float) settings.Margin.Top, (float) settings.Margin.Right, (float) settings.Margin.Bottom);
        FS_RECTF effectiveBox = page.GetEffectiveBox();
        PdfFormObject pdfFormObject = PdfFormObject.Create(page);
        StringBuilder stringBuilder = new StringBuilder();
        foreach (PageHeaderFooterUtils.LocationEnum location in (IEnumerable<PageHeaderFooterUtils.LocationEnum>) allValues)
        {
          string content = PageHeaderFooterUtils.GetContent(page, index1, array.Length, settings, location, date);
          if (!string.IsNullOrEmpty(content))
          {
            if (stringBuilder.Length > 0)
              stringBuilder.Append(' ');
            stringBuilder.Append(content);
          }
          if (!string.IsNullOrWhiteSpace(content))
          {
            foreach (PdfTextObject textObject in (IEnumerable<PdfTextObject>) PageHeaderFooterUtils.GetTextObjects(doc, content, fontName, fontSize, effectiveBox, margin, location, page.Rotation))
            {
              textObject.FillColor = fsColor;
              pdfFormObject.PageObjects.Add((PdfPageObject) textObject);
            }
          }
        }
        if (pdfFormObject.PageObjects.Count == 0)
        {
          pdfFormObject.Dispose();
          return false;
        }
        int count = page.PageObjects.Count;
        page.PageObjects.Insert(count, (PdfPageObject) pdfFormObject);
        page.GenerateContentAdvance(new GenerateContentOptions()
        {
          KeepHeaderFooterData = false
        });
        PdfTypeStream stream = ((PdfFormObject) page.PageObjects[count]).Stream;
        if (!nullable1.HasValue)
          nullable1 = new int?(PageHeaderFooterUtils.CreateOCDictionary(doc));
        if (!nullable2.HasValue)
          nullable2 = new int?(PageHeaderFooterUtils.CreateDocSettings(doc, xml));
        PageHeaderFooterUtils.WritePageHeaderFooterSettings(page, stream, modificationDateString, nullable1.Value, nullable2.Value);
        List<(int, int, string)> valueTupleList;
        if (hfData != null)
        {
          System.Collections.Generic.IReadOnlyList<int> intList;
          valueTupleList = hfData.SelectMany<HeaderFooterData, int>((Func<HeaderFooterData, IEnumerable<int>>) (c => c.FormObjects != null && c.FormObjects.TryGetValue(pageIndex, out intList) ? (IEnumerable<int>) intList : (IEnumerable<int>) Array.Empty<int>())).Select<int, (int, int, string)>((Func<int, (int, int, string)>) (c => (c, PageHeaderFooterUtils.GetFormObjectOCDictionaryIndex(page.PageObjects[c] as PdfFormObject, page), PageHeaderFooterUtils.GetFormObjectMarkedContentStr(page.PageObjects[c] as PdfFormObject)))).ToList<(int, int, string)>();
          if (valueTupleList.Count > 0)
          {
            foreach ((int index2, int num, string _) in valueTupleList)
            {
              PdfFormObject pageObject = (PdfFormObject) page.PageObjects[index2];
              int ocgsIndirectNum = num != -1 ? num : nullable1.Value;
              PageHeaderFooterUtils.WritePageHeaderFooterSettings(page, pageObject.Stream, modificationDateString, ocgsIndirectNum, nullable2.Value);
            }
          }
        }
        else
          valueTupleList = new List<(int, int, string)>();
        valueTupleList.Add((count, nullable1.Value, stringBuilder.ToString()));
        foreach ((int index3, int _, string str) in valueTupleList)
        {
          PdfFormObject pageObject;
          int num;
          if (index3 >= 0 && index3 < page.PageObjects.Count)
          {
            pageObject = page.PageObjects[index3] as PdfFormObject;
            num = pageObject != null ? 1 : 0;
          }
          else
            num = 0;
          if (num != 0)
          {
            PdfTypeDictionary parameters = PdfTypeDictionary.Create();
            parameters["Type"] = (PdfTypeBase) PdfTypeName.Create("Pagination");
            parameters["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Header");
            parameters["Contents"] = (PdfTypeBase) PdfTypeString.Create(str ?? "", true);
            PdfMarkedContent pdfMarkedContent = new PdfMarkedContent("Artifact", false, PropertyListTypes.DirectDict, parameters);
            pageObject.MarkedContent.Add(pdfMarkedContent);
          }
        }
        if (visiblePageRange.HasValue && (page.PageIndex > visiblePageRange.Value.Item2 || page.PageIndex < visiblePageRange.Value.Item1))
          PageDisposeHelper.DisposePage(page);
        progress?.Report(1.0 / (double) array.Length * (double) (index1 + 1));
      }
      progress?.Report(1.0);
      return true;
    }))).ConfigureAwait(false);
    return flag;
  }

  private static (float left, float top, float right, float bottom) GetMarginWithBleed(
    PdfPage page,
    float left,
    float top,
    float right,
    float bottom)
  {
    FS_RECTF mediaBox = page.MediaBox;
    FS_RECTF effectiveBox = page.GetEffectiveBox();
    return (left + effectiveBox.left, top + (mediaBox.top - effectiveBox.top), right + (mediaBox.right - effectiveBox.right), bottom + effectiveBox.bottom);
  }

  private static PageRotate PageRotation(PdfPage pdfPage, PageRotate? rotate)
  {
    int num = (int) (((int) rotate ?? (int) pdfPage.Rotation) - pdfPage.Rotation);
    if (num < 0)
      num = 4 + num;
    return (PageRotate) num;
  }

  private static void ReloadPage(PdfPage page)
  {
    if (page == null)
      return;
    PageDisposeHelper.DisposePage(page);
    IntPtr handle = page.Handle;
  }

  private static System.Collections.Generic.IReadOnlyList<PdfTextObject> GetTextObjects(
    PdfDocument doc,
    string content,
    string fontName,
    float fontSize,
    FS_RECTF boundingBox,
    (float left, float top, float right, float bottom) margin,
    PageHeaderFooterUtils.LocationEnum location,
    PageRotate rotate,
    List<PdfPageObject> debugRectObjects = null)
  {
    string[] strArray = content.Replace("\r", "").Split('\n');
    if (strArray.Length == 0)
      return (System.Collections.Generic.IReadOnlyList<PdfTextObject>) Array.Empty<PdfTextObject>();
    FS_RECTF[] fsRectfArray = new FS_RECTF[strArray.Length];
    System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily>[] fallbackFontFamilyListArray = new System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily>[strArray.Length];
    FS_RECTF bounds = new FS_RECTF();
    float num = float.MaxValue;
    for (int index = 0; index < strArray.Length; ++index)
    {
      System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily> withFallbackFonts = PdfFontUtils.GetTextWithFallbackFonts(strArray[index], fontName, fontSize);
      fallbackFontFamilyListArray[index] = withFallbackFonts;
      fsRectfArray[index] = PageHeaderFooterUtils.GetTextBounds(withFallbackFonts);
      fsRectfArray[index].top -= (float) index * (fontSize + fontSize / 10f);
      fsRectfArray[index].bottom -= (float) index * (fontSize + fontSize / 10f);
      if (index == 0)
      {
        bounds = fsRectfArray[0];
      }
      else
      {
        bounds.left = Math.Min(fsRectfArray[index].left, bounds.left);
        bounds.right = Math.Max(fsRectfArray[index].right, bounds.right);
        bounds.top = Math.Max(fsRectfArray[index].top, bounds.top);
        bounds.bottom = Math.Min(fsRectfArray[index].bottom, bounds.bottom);
      }
      if (index == strArray.Length - 1)
      {
        foreach (TextWithFallbackFontFamily fallbackFontFamily in (IEnumerable<TextWithFallbackFontFamily>) withFallbackFonts)
          num = Math.Min(num, fallbackFontFamily.Baseline);
      }
    }
    if ((double) num == 3.4028234663852886E+38)
      num = 0.0f;
    FS_POINTF fsPointf = new FS_POINTF(-bounds.left, -bounds.bottom);
    bounds.left += fsPointf.X;
    bounds.right += fsPointf.X;
    bounds.top += fsPointf.Y;
    bounds.bottom += fsPointf.Y;
    FS_POINTF textLocation = PageHeaderFooterUtils.GetTextLocation(boundingBox, bounds, num, margin, location, rotate);
    List<PdfTextObject> textObjects1 = new List<PdfTextObject>();
    for (int index = 0; index < fallbackFontFamilyListArray.Length; ++index)
    {
      FS_POINTF location1 = new FS_POINTF(fsRectfArray[index].left + fsPointf.X, fsRectfArray[index].bottom + fsPointf.Y);
      System.Collections.Generic.IReadOnlyList<PdfTextObject> textObjects2 = PageHeaderFooterUtils.GetTextObjects(doc, fallbackFontFamilyListArray[index], fontName, location1, rotate);
      foreach (PdfTextObject pdfTextObject in (IEnumerable<PdfTextObject>) textObjects2)
      {
        FS_POINTF location2 = pdfTextObject.Location;
        FS_RECTF boundingBox1 = pdfTextObject.BoundingBox;
        boundingBox1.left += textLocation.X;
        boundingBox1.right += textLocation.X;
        boundingBox1.top += textLocation.Y;
        boundingBox1.bottom += textLocation.Y;
        if (debugRectObjects != null)
        {
          List<PdfPathObject> square = AnnotDrawingHelper.CreateSquare(new FS_COLOR((int) sbyte.MaxValue, (int) byte.MaxValue, 0, 0), new FS_COLOR((int) sbyte.MaxValue, (int) byte.MaxValue, 0, 0), 0.0f, (float[]) null, BorderStyles.Solid, BorderEffects.None, 0, boundingBox1);
          debugRectObjects.AddRange((IEnumerable<PdfPageObject>) square);
        }
        FS_MATRIX fsMatrix = new FS_MATRIX();
        fsMatrix.SetIdentity();
        fsMatrix.Translate(location2.X, location2.Y);
        fsMatrix.Rotate((float) ((double) ((int) rotate * 90) * Math.PI / 180.0));
        fsMatrix.Translate(textLocation.X, textLocation.Y);
        pdfTextObject.Matrix = fsMatrix;
      }
      textObjects1.AddRange((IEnumerable<PdfTextObject>) textObjects2);
    }
    return (System.Collections.Generic.IReadOnlyList<PdfTextObject>) textObjects1;
  }

  public static bool IsHeaderFooterXObject(PdfTypeStream stream)
  {
    if (stream == null || XObjectHelper.GetDirectValue<PdfTypeName>(stream.Dictionary, "Subtype")?.Value != "Form")
      return false;
    PdfTypeDictionary directValue = XObjectHelper.GetDirectValue<PdfTypeDictionary>(XObjectHelper.GetDirectValue<PdfTypeDictionary>(stream.Dictionary, "OC"), "OCGs");
    return XObjectHelper.GetDirectValue<PdfTypeString>(directValue, "Name")?.UnicodeString == "Headers/Footers" || XObjectHelper.GetDirectValue<PdfTypeName>(XObjectHelper.GetDirectValue<PdfTypeDictionary>(XObjectHelper.GetDirectValue<PdfTypeDictionary>(directValue, "Usage"), "PageElement"), "Subtype")?.Value == "HF";
  }

  private static int GetFormObjectOCDictionaryIndex(PdfFormObject formObj, PdfPage page)
  {
    PdfTypeBase pdfTypeBase1;
    if (formObj?.Stream?.Dictionary == null || !formObj.Stream.Dictionary.TryGetValue("OC", out pdfTypeBase1) || !pdfTypeBase1.Is<PdfTypeDictionary>())
      return -1;
    PdfTypeBase pdfTypeBase2;
    PdfTypeIndirect pdfTypeIndirect;
    int num;
    if (pdfTypeBase1.As<PdfTypeDictionary>().TryGetValue("OCGs", out pdfTypeBase2) && pdfTypeBase2.Is<PdfTypeDictionary>())
    {
      pdfTypeIndirect = pdfTypeBase2 as PdfTypeIndirect;
      num = pdfTypeIndirect != null ? 1 : 0;
    }
    else
      num = 0;
    return num != 0 ? pdfTypeIndirect.Number : -1;
  }

  private static string GetFormObjectMarkedContentStr(PdfFormObject formObject)
  {
    if (formObject?.MarkedContent != null && formObject.MarkedContent.Count > 0)
    {
      foreach (PdfMarkedContent pdfMarkedContent in formObject.MarkedContent)
      {
        if (pdfMarkedContent.Tag == "Artifact")
        {
          PdfTypeDictionary parameters = pdfMarkedContent.Parameters;
          PdfTypeBase pdfTypeBase;
          if (parameters != null && parameters.TryGetValue("Contents", out pdfTypeBase) && pdfTypeBase.Is<PdfTypeName>())
            return pdfTypeBase.As<PdfTypeName>().Value ?? "";
        }
      }
    }
    return "";
  }

  private static int CreateOCDictionary(PdfDocument doc)
  {
    if (doc == null)
      return -1;
    PdfTypeDictionary dict = PageHeaderFooterUtils.TryGetOrCreateDict(doc.Root, "OCProperties", false, (PdfDocument) null);
    PdfTypeBase pdfTypeBase;
    if (!dict.TryGetValue("OCGs", out pdfTypeBase))
    {
      pdfTypeBase = (PdfTypeBase) PdfTypeArray.Create();
      dict["OCGs"] = pdfTypeBase;
    }
    if (!pdfTypeBase.Is<PdfTypeArray>())
      return -1;
    PdfTypeArray pdfTypeArray = pdfTypeBase.As<PdfTypeArray>();
    PdfTypeDictionary pdfTypeDictionary = PdfTypeDictionary.Create();
    pdfTypeDictionary["Name"] = (PdfTypeBase) PdfTypeString.Create("Headers/Footers");
    pdfTypeDictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("OCG");
    PageHeaderFooterUtils.TryGetOrCreateDict(PageHeaderFooterUtils.TryGetOrCreateDict(pdfTypeDictionary, "Usage", false, (PdfDocument) null), "PageElement", false, (PdfDocument) null)["Subtype"] = (PdfTypeBase) PdfTypeName.Create("HF");
    PdfIndirectList list = PdfIndirectList.FromPdfDocument(doc);
    int num = list.Add((PdfTypeBase) pdfTypeDictionary);
    PdfTypeIndirect pdfTypeIndirect = PdfTypeIndirect.Create(list, num);
    pdfTypeArray.Add((PdfTypeBase) pdfTypeIndirect);
    return num;
  }

  private static int CreateDocSettings(PdfDocument doc, string xml)
  {
    if (string.IsNullOrEmpty(xml))
      return -1;
    PdfTypeStream indirectObject = PdfTypeStream.Create();
    indirectObject.SetContent(Encoding.UTF8.GetBytes(xml), false);
    return PdfIndirectList.FromPdfDocument(doc).Add((PdfTypeBase) indirectObject);
  }

  internal static void WritePageHeaderFooterSettings(
    PdfPage page,
    PdfTypeStream stream,
    string lastModified,
    int ocgsIndirectNum,
    int docSettingsIndirectNum)
  {
    if (page == null || stream == null || stream?.Dictionary == null)
      return;
    stream.Dictionary["LastModified"] = (PdfTypeBase) PdfTypeString.Create(lastModified, true);
    if (ocgsIndirectNum != -1)
    {
      PdfIndirectList list = PdfIndirectList.FromPdfDocument(page.Document);
      if (list[ocgsIndirectNum].Is<PdfTypeDictionary>())
      {
        PdfTypeDictionary dict = PageHeaderFooterUtils.TryGetOrCreateDict(stream.Dictionary, "OC", true, page.Document);
        if (dict != null)
        {
          dict["Type"] = (PdfTypeBase) PdfTypeName.Create("OCMD");
          dict["OCGs"] = (PdfTypeBase) PdfTypeIndirect.Create(list, ocgsIndirectNum);
        }
      }
    }
    PdfTypeDictionary dict1 = PageHeaderFooterUtils.TryGetOrCreateDict(PageHeaderFooterUtils.TryGetOrCreateDict(stream.Dictionary, "PieceInfo", false, (PdfDocument) null), "ADBE_CompoundType", false, (PdfDocument) null);
    if (docSettingsIndirectNum != -1)
    {
      PdfTypeIndirect pdfTypeIndirect = PdfTypeIndirect.Create(PdfIndirectList.FromPdfDocument(page.Document), docSettingsIndirectNum);
      dict1["DocSettings"] = (PdfTypeBase) pdfTypeIndirect;
    }
    dict1["Private"] = (PdfTypeBase) PdfTypeName.Create("Header");
    dict1["LastModified"] = (PdfTypeBase) PdfTypeString.Create(lastModified, true);
  }

  private static PdfTypeDictionary TryGetOrCreateDict(
    PdfTypeDictionary dict,
    string key,
    bool indirect,
    PdfDocument doc)
  {
    if (dict == null || string.IsNullOrEmpty(key))
      return (PdfTypeDictionary) null;
    PdfTypeDictionary indirectObject = (PdfTypeDictionary) null;
    PdfTypeBase pdfTypeBase;
    if (dict.TryGetValue(key, out pdfTypeBase))
    {
      if (pdfTypeBase.Is<PdfTypeDictionary>())
        indirectObject = pdfTypeBase.As<PdfTypeDictionary>();
    }
    else if (indirect)
    {
      if (doc != null)
      {
        PdfIndirectList list = PdfIndirectList.FromPdfDocument(doc);
        indirectObject = PdfTypeDictionary.Create();
        int num = list.Add((PdfTypeBase) indirectObject);
        PdfTypeIndirect pdfTypeIndirect = PdfTypeIndirect.Create(list, num);
        dict[key] = (PdfTypeBase) pdfTypeIndirect;
      }
    }
    else
    {
      indirectObject = PdfTypeDictionary.Create();
      dict[key] = (PdfTypeBase) indirectObject;
    }
    return indirectObject;
  }

  private static PdfTypeStream GetPdfFormObjectStream(PdfFormObject formObject)
  {
    if (formObject == null)
      return (PdfTypeStream) null;
    if (PageHeaderFooterUtils.pdfFormObjectStreamGetter == null)
    {
      lock (typeof (PageHeaderFooterUtils))
      {
        if (PageHeaderFooterUtils.pdfFormObjectStreamGetter == null)
        {
          Type type = typeof (PdfFormObject);
          FieldInfo field = type.GetField("_stream", BindingFlags.Instance | BindingFlags.NonPublic);
          if (field != (FieldInfo) null)
          {
            ParameterExpression parameterExpression = System.Linq.Expressions.Expression.Parameter(type, "c");
            PageHeaderFooterUtils.pdfFormObjectStreamGetter = System.Linq.Expressions.Expression.Lambda<Func<PdfFormObject, PdfTypeStream>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Field((System.Linq.Expressions.Expression) parameterExpression, field), parameterExpression).Compile();
          }
        }
        if (PageHeaderFooterUtils.pdfFormObjectStreamGetter == null)
          PageHeaderFooterUtils.pdfFormObjectStreamGetter = (Func<PdfFormObject, PdfTypeStream>) (c => (PdfTypeStream) null);
      }
    }
    return PageHeaderFooterUtils.pdfFormObjectStreamGetter(formObject);
  }

  public static double PdfPointToCm(double pdfPoint)
  {
    return pdfPoint / 72.0 / (50.0 / (double) sbyte.MaxValue);
  }

  public static double CmToPdfPoint(double cm) => cm * (50.0 / (double) sbyte.MaxValue) * 72.0;

  public static (float left, float top, float right, float bottom) GetPdfMargin(
    double cmLeft,
    double cmTop,
    double cmRight,
    double cmBottom)
  {
    return ((float) PageHeaderFooterUtils.CmToPdfPoint(cmLeft), (float) PageHeaderFooterUtils.CmToPdfPoint(cmTop), (float) PageHeaderFooterUtils.CmToPdfPoint(cmRight), (float) PageHeaderFooterUtils.CmToPdfPoint(cmBottom));
  }

  public static (float left, float top, float right, float bottom) GetCmMargin(
    double left,
    double top,
    double right,
    double bottom)
  {
    return ((float) PageHeaderFooterUtils.PdfPointToCm(left), (float) PageHeaderFooterUtils.PdfPointToCm(top), (float) PageHeaderFooterUtils.PdfPointToCm(right), (float) PageHeaderFooterUtils.PdfPointToCm(bottom));
  }

  private static FS_RECTF GetTextBounds(System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily> texts)
  {
    if (texts == null || texts.Count == 0)
      return new FS_RECTF();
    double num1 = double.MaxValue;
    double num2 = double.MaxValue;
    double num3 = double.MinValue;
    double num4 = double.MinValue;
    for (int index = 0; index < texts.Count; ++index)
    {
      TextWithFallbackFontFamily text = texts[index];
      Rect bounds = text.Bounds;
      num1 = Math.Min(bounds.Left, num1);
      bounds = text.Bounds;
      num2 = Math.Min(bounds.Top, num2);
      bounds = text.Bounds;
      num3 = Math.Max(bounds.Right, num3);
      bounds = text.Bounds;
      num4 = Math.Max(bounds.Bottom, num4);
    }
    return new FS_RECTF(num1, num4, num3, num2);
  }

  private static System.Collections.Generic.IReadOnlyList<PdfTextObject> GetTextObjects(
    PdfDocument _doc,
    System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily> texts,
    string fontFamily,
    FS_POINTF location,
    PageRotate rotate)
  {
    if (texts.Count == 0)
      return (System.Collections.Generic.IReadOnlyList<PdfTextObject>) Array.Empty<PdfTextObject>();
    List<PdfTextObject> textObjects = new List<PdfTextObject>();
    for (int index = 0; index < texts.Count; ++index)
    {
      TextWithFallbackFontFamily text = texts[index];
      PdfFont font = PdfFontUtils.CreateFont(_doc, text.FallbackFontFamily == null ? fontFamily : text.FallbackFontFamily.Source, text.FontWeight, text.FontStyle, text.CharSet);
      PdfTextObject pdfTextObject = PdfTextObject.Create(text.Text, location.X + (float) text.Bounds.X, location.Y, font, text.ScaledFontSize);
      FS_POINTF location1 = pdfTextObject.Location;
      FS_MATRIX fsMatrix = new FS_MATRIX();
      fsMatrix.SetIdentity();
      if (rotate != 0)
        fsMatrix.Rotate((float) ((double) rotate * Math.PI / 180.0));
      fsMatrix.Translate(location1.X, location1.Y);
      pdfTextObject.Matrix = fsMatrix;
      textObjects.Add(pdfTextObject);
    }
    return (System.Collections.Generic.IReadOnlyList<PdfTextObject>) textObjects;
  }

  private static FS_POINTF GetTextLocation(
    FS_RECTF boundingBox,
    FS_RECTF bounds,
    float baseline,
    (float left, float top, float right, float bottom) margin,
    PageHeaderFooterUtils.LocationEnum location,
    PageRotate rotate)
  {
    switch (rotate)
    {
      case PageRotate.Normal:
        return GetTextLocationCore(boundingBox, bounds, baseline, margin, location);
      case PageRotate.Rotate90:
        return GetTextLocationCore90(boundingBox, bounds, baseline, margin, location);
      case PageRotate.Rotate180:
        return GetTextLocationCore180(boundingBox, bounds, baseline, margin, location);
      case PageRotate.Rotate270:
        return GetTextLocationCore270(boundingBox, bounds, baseline, margin, location);
      default:
        return new FS_POINTF();
    }

    static void GetFlags(
      PageHeaderFooterUtils.LocationEnum _location,
      out bool headerFlag,
      out bool leftFlag,
      out bool centerFlag)
    {
      headerFlag = _location == PageHeaderFooterUtils.LocationEnum.HeaderLeft || _location == PageHeaderFooterUtils.LocationEnum.HeaderCenter || _location == PageHeaderFooterUtils.LocationEnum.HeaderRight;
      leftFlag = _location == PageHeaderFooterUtils.LocationEnum.HeaderLeft || _location == PageHeaderFooterUtils.LocationEnum.FooterLeft;
      centerFlag = _location == PageHeaderFooterUtils.LocationEnum.HeaderCenter || _location == PageHeaderFooterUtils.LocationEnum.FooterCenter;
    }

    static FS_POINTF GetTextLocationCore(
      FS_RECTF _boundingBox,
      FS_RECTF _bounds,
      float _baseline,
      (float left, float top, float right, float bottom) _margin,
      PageHeaderFooterUtils.LocationEnum _location)
    {
      FS_POINTF textLocationCore = new FS_POINTF();
      bool headerFlag;
      bool leftFlag;
      bool centerFlag;
      GetFlags(_location, out headerFlag, out leftFlag, out centerFlag);
      textLocationCore.X = !leftFlag ? (!centerFlag ? _boundingBox.right - _margin.right - _bounds.Width : (float) ((double) _boundingBox.left + (double) _boundingBox.Width / 2.0 - (double) _bounds.Width / 2.0)) : _boundingBox.left + _margin.left;
      textLocationCore.Y = !headerFlag ? _boundingBox.bottom + _margin.bottom - _bounds.Height + _baseline : _boundingBox.top - _margin.top + _baseline;
      return textLocationCore;
    }

    static FS_POINTF GetTextLocationCore90(
      FS_RECTF _boundingBox,
      FS_RECTF _bounds,
      float _baseline,
      (float left, float top, float right, float bottom) _margin,
      PageHeaderFooterUtils.LocationEnum _location)
    {
      FS_POINTF textLocationCore90 = new FS_POINTF();
      bool headerFlag;
      bool leftFlag;
      bool centerFlag;
      GetFlags(_location, out headerFlag, out leftFlag, out centerFlag);
      textLocationCore90.Y = !leftFlag ? (!centerFlag ? _boundingBox.top - _margin.right - _bounds.Width : (float) ((double) _boundingBox.bottom + (double) _boundingBox.Height / 2.0 - (double) _bounds.Width / 2.0)) : _boundingBox.bottom + _margin.left;
      textLocationCore90.X = !headerFlag ? _boundingBox.right - _margin.bottom + _bounds.Height - _baseline : _boundingBox.left + _margin.top - _baseline;
      return textLocationCore90;
    }

    static FS_POINTF GetTextLocationCore180(
      FS_RECTF _boundingBox,
      FS_RECTF _bounds,
      float _baseline,
      (float left, float top, float right, float bottom) _margin,
      PageHeaderFooterUtils.LocationEnum _location)
    {
      FS_POINTF textLocationCore180 = new FS_POINTF();
      bool headerFlag;
      bool leftFlag;
      bool centerFlag;
      GetFlags(_location, out headerFlag, out leftFlag, out centerFlag);
      textLocationCore180.X = !leftFlag ? (!centerFlag ? _boundingBox.left + _margin.right + _bounds.Width : (float) ((double) _boundingBox.left + (double) _boundingBox.Width / 2.0 + (double) _bounds.Width / 2.0)) : _boundingBox.right - _margin.left;
      textLocationCore180.Y = !headerFlag ? _boundingBox.top - _margin.bottom + _bounds.Height - _baseline : _boundingBox.bottom + _margin.top - _baseline;
      return textLocationCore180;
    }

    static FS_POINTF GetTextLocationCore270(
      FS_RECTF _boundingBox,
      FS_RECTF _bounds,
      float _baseline,
      (float left, float top, float right, float bottom) _margin,
      PageHeaderFooterUtils.LocationEnum _location)
    {
      FS_POINTF textLocationCore270 = new FS_POINTF();
      bool headerFlag;
      bool leftFlag;
      bool centerFlag;
      GetFlags(_location, out headerFlag, out leftFlag, out centerFlag);
      textLocationCore270.Y = !leftFlag ? (!centerFlag ? _boundingBox.bottom + _margin.right + _bounds.Width : (float) ((double) _boundingBox.bottom + (double) _boundingBox.Height / 2.0 + (double) _bounds.Width / 2.0)) : _boundingBox.top - _margin.left;
      textLocationCore270.X = !headerFlag ? (float) ((double) _boundingBox.left + (double) _margin.bottom - (double) _bounds.Height + (double) _baseline - 0.20000000298023224) : _boundingBox.right - _margin.top + _baseline;
      return textLocationCore270;
    }
  }

  public static string GetContent(
    PdfPage page,
    int index,
    int pageCount,
    HeaderFooterSettings _settings,
    PageHeaderFooterUtils.LocationEnum location,
    DateTimeOffset date)
  {
    switch (location)
    {
      case PageHeaderFooterUtils.LocationEnum.HeaderLeft:
        return PageHeaderFooterUtils.GetLocationContent(page, index, pageCount, _settings.Header.Left, date);
      case PageHeaderFooterUtils.LocationEnum.HeaderCenter:
        return PageHeaderFooterUtils.GetLocationContent(page, index, pageCount, _settings.Header.Center, date);
      case PageHeaderFooterUtils.LocationEnum.HeaderRight:
        return PageHeaderFooterUtils.GetLocationContent(page, index, pageCount, _settings.Header.Right, date);
      case PageHeaderFooterUtils.LocationEnum.FooterLeft:
        return PageHeaderFooterUtils.GetLocationContent(page, index, pageCount, _settings.Footer.Left, date);
      case PageHeaderFooterUtils.LocationEnum.FooterCenter:
        return PageHeaderFooterUtils.GetLocationContent(page, index, pageCount, _settings.Footer.Center, date);
      case PageHeaderFooterUtils.LocationEnum.FooterRight:
        return PageHeaderFooterUtils.GetLocationContent(page, index, pageCount, _settings.Footer.Right, date);
      default:
        return (string) null;
    }
  }

  private static string GetLocationContent(
    PdfPage page,
    int index,
    int pageCount,
    HeaderFooterSettings.LocationModel locationModel,
    DateTimeOffset date)
  {
    if (locationModel == null || locationModel.Count == 0)
      return (string) null;
    StringBuilder stringBuilder = new StringBuilder();
    foreach (object obj in (HeaderFooterSettings.VariableCollection) locationModel)
    {
      switch (obj)
      {
        case HeaderFooterSettings.PageModel pageModel:
          stringBuilder.Append(PageHeaderFooterUtils.GetPageModelResult(index, pageCount, pageModel));
          break;
        case HeaderFooterSettings.DateModel dateModel:
          stringBuilder.Append(PageHeaderFooterUtils.GetDateModelResult(dateModel, date));
          break;
        case string str:
          stringBuilder.Append(str);
          break;
      }
    }
    return stringBuilder.ToString();
  }

  private static IEnumerable<int> GetPageRanges(int _startPage, int _endPage, bool odd, bool even)
  {
    for (int i = _startPage; i <= _endPage; ++i)
    {
      int j = i % 2;
      if (j == 0 & even)
        yield return i - 1;
      else if (j == 1 & odd)
        yield return i - 1;
    }
  }

  private static string GetPageModelResult(
    int index,
    int pageCount,
    HeaderFooterSettings.PageModel pageModel)
  {
    StringBuilder stringBuilder = new StringBuilder();
    int num = -1;
    foreach (object obj in (HeaderFooterSettings.VariableCollection) pageModel)
    {
      if (obj is string str)
      {
        if (num != -1 && num != 0)
          stringBuilder.Append(" ");
        num = 0;
        stringBuilder.Append(str);
      }
      else if (obj is HeaderFooterSettings.VariableModel variableModel)
      {
        if (variableModel.Name == "PageIndex")
        {
          if (num != -1 && num != 1)
            stringBuilder.Append(" ");
          num = 1;
          stringBuilder.Append(index + 1 + pageModel.Offset);
        }
        else if (variableModel.Name == "PageTotalNum")
        {
          if (num != -1 && num != 2)
            stringBuilder.Append(" ");
          num = 2;
          stringBuilder.Append(pageCount + pageModel.Offset);
        }
      }
    }
    return stringBuilder.ToString();
  }

  private static string GetDateModelResult(
    HeaderFooterSettings.DateModel dateModel,
    DateTimeOffset date)
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (object obj in (HeaderFooterSettings.VariableCollection) dateModel)
    {
      if (obj is string str)
      {
        stringBuilder.Append(str);
      }
      else
      {
        int result;
        if (obj is HeaderFooterSettings.VariableModel variableModel && int.TryParse(variableModel.Format, out result))
        {
          string element = "";
          switch (variableModel.Name)
          {
            case "Day":
              element = "d";
              break;
            case "Month":
              element = "m";
              break;
            case "Year":
              element = "y";
              break;
          }
          if (!string.IsNullOrEmpty(element))
          {
            switch (string.Join("", Enumerable.Repeat<string>(element, result)))
            {
              case "m":
                stringBuilder.Append($"{date.Month:#0}");
                break;
              case "mm":
                stringBuilder.Append($"{date.Month:00}");
                break;
              case "d":
                stringBuilder.Append($"{date.Day:#0}");
                break;
              case "dd":
                stringBuilder.Append($"{date.Day:00}");
                break;
              case "yy":
                stringBuilder.Append(date.Year % 100);
                break;
              case "yyyy":
                stringBuilder.Append(date.Year);
                break;
            }
          }
        }
      }
    }
    return stringBuilder.ToString();
  }

  private static PdfTypeArray ConvertContentsToArray(PdfPage page)
  {
    PdfTypeDictionary dictionary = page.Dictionary;
    PdfIndirectList list = PdfIndirectList.FromPdfDocument(page.Document);
    if (!dictionary.ContainsKey("Contents"))
    {
      PdfTypeArray array = PdfTypeArray.Create();
      list.Add((PdfTypeBase) array);
      dictionary.SetIndirectAt("Contents", list, (PdfTypeBase) array);
      return array;
    }
    PdfTypeBase array1 = dictionary["Contents"];
    switch (array1)
    {
      case PdfTypeArray _:
        return array1 as PdfTypeArray;
      case PdfTypeIndirect _:
        if ((array1 as PdfTypeIndirect).Direct is PdfTypeArray)
          return (array1 as PdfTypeIndirect).Direct as PdfTypeArray;
        if (!((array1 as PdfTypeIndirect).Direct is PdfTypeStream))
          throw new Exception("Unexpected content type");
        PdfTypeArray array2 = PdfTypeArray.Create();
        array2.AddIndirect(list, (array1 as PdfTypeIndirect).Direct);
        list.Add((PdfTypeBase) array2);
        dictionary.SetIndirectAt("Contents", list, (PdfTypeBase) array2);
        return array2;
      case PdfTypeStream _:
        list.Add(array1);
        PdfTypeArray array3 = PdfTypeArray.Create();
        array3.AddIndirect(list, array1);
        list.Add((PdfTypeBase) array3);
        dictionary.SetIndirectAt("Contents", list, (PdfTypeBase) array3);
        return array3;
      default:
        throw new Exception("Unexpected content type");
    }
  }

  public enum LocationEnum
  {
    HeaderLeft,
    HeaderCenter,
    HeaderRight,
    FooterLeft,
    FooterCenter,
    FooterRight,
  }
}
