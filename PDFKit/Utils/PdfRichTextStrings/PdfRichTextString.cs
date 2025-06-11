// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRichTextStrings.PdfRichTextString
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using LruCacheNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml;

#nullable disable
namespace PDFKit.Utils.PdfRichTextStrings;

[DebuggerDisplay("ContentText = {ContentText}")]
public class PdfRichTextString
{
  private static LruCache<PdfRichTextString.PdfRichTextStringCacheKey, PdfRichTextString> cache = new LruCache<PdfRichTextString.PdfRichTextStringCacheKey, PdfRichTextString>(50);

  public PdfRichTextRawElement Text { get; set; }

  public PdfRichTextStyle DefaultStyle { get; set; } = PdfRichTextStyle.Default;

  public string ContentText => this.Text?.Text;

  public override string ToString() => this.ToString(false);

  public string ToString(bool withDefaultStyle)
  {
    XmlDocument doc = new XmlDocument();
    doc.AppendChild((XmlNode) doc.CreateXmlDeclaration("1.0", (string) null, (string) null));
    XmlNode node = PdfRichTextString.CreateNode(this, new PdfRichTextStyle?(this.DefaultStyle), doc, this.Text, withDefaultStyle);
    doc.AppendChild(node);
    return doc.OuterXml;
  }

  private static XmlNode CreateNode(
    PdfRichTextString richTextString,
    PdfRichTextStyle? defaultStyle,
    XmlDocument doc,
    PdfRichTextRawElement element,
    bool withDefaultStyle)
  {
    if (!(element is PdfRichTextElement pdfRichTextElement))
      return (XmlNode) doc.CreateTextNode(element.ToString());
    XmlElement element1 = doc.CreateElement(pdfRichTextElement.Tag.ToString().ToLowerInvariant());
    if (pdfRichTextElement.Tag == PdfRichTextElementTag.Body)
    {
      element1.SetAttribute("xmlns", "http://www.w3.org/1999/xhtml");
      element1.SetAttribute("xmlns:xfa", "http://www.xfa.org/schema/xfa-data/1.0/");
      element1.SetAttribute("xfa:APIVersion", "Acrobat:11.0.0");
      element1.SetAttribute("xfa:spec", "2.0.2");
      if (withDefaultStyle)
        element1.SetAttribute("style", richTextString.DefaultStyle.ToString());
    }
    PdfRichTextStyle? style = pdfRichTextElement.Style;
    if (style.HasValue)
    {
      bool flag = false;
      PdfRichTextStyle pdfRichTextStyle1;
      if (!withDefaultStyle && pdfRichTextElement.Tag == PdfRichTextElementTag.Body && defaultStyle.HasValue)
      {
        style = pdfRichTextElement.Style;
        pdfRichTextStyle1 = style.Value;
        if (!pdfRichTextStyle1.Equals(defaultStyle.Value))
          flag = true;
      }
      else
        flag = true;
      if (flag)
      {
        if (defaultStyle.HasValue)
        {
          ref PdfRichTextStyle? local1 = ref defaultStyle;
          pdfRichTextStyle1 = defaultStyle.Value;
          ref PdfRichTextStyle local2 = ref pdfRichTextStyle1;
          style = pdfRichTextElement.Style;
          PdfRichTextStyle style2 = style.Value;
          PdfRichTextStyle pdfRichTextStyle2 = local2.Merge(style2);
          local1 = new PdfRichTextStyle?(pdfRichTextStyle2);
        }
        else
          defaultStyle = pdfRichTextElement.Style;
        pdfRichTextStyle1 = defaultStyle.Value;
        string str = pdfRichTextStyle1.ToString();
        if (!string.IsNullOrEmpty(str))
          element1.SetAttribute("style", str);
      }
    }
    List<PdfRichTextRawElement> children = pdfRichTextElement.Children;
    // ISSUE: explicit non-virtual call
    if (children != null && __nonvirtual (children.Count) > 0)
    {
      foreach (PdfRichTextRawElement child in pdfRichTextElement.Children)
        element1.AppendChild(PdfRichTextString.CreateNode(richTextString, defaultStyle, doc, child, false));
    }
    return (XmlNode) element1;
  }

  public static bool TryParse(
    string richText,
    out PdfRichTextString pdfRichTextString,
    string annotName = "")
  {
    Stopwatch stopwatch = Stopwatch.StartNew();
    try
    {
      return PdfRichTextString.TryParseCore(richText, new PdfRichTextStyle?(), out pdfRichTextString, annotName);
    }
    finally
    {
      stopwatch.Stop();
    }
  }

  public static bool TryParse(
    string richText,
    PdfRichTextStyle defaultStyle,
    out PdfRichTextString pdfRichTextString,
    string annotName = "")
  {
    return PdfRichTextString.TryParseCore(richText, new PdfRichTextStyle?(defaultStyle), out pdfRichTextString, annotName);
  }

  private static bool TryParseCore(
    string richText,
    PdfRichTextStyle? defaultStyle,
    out PdfRichTextString pdfRichTextString,
    string annotName)
  {
    pdfRichTextString = (PdfRichTextString) null;
    if (string.IsNullOrEmpty(richText))
      return false;
    PdfRichTextStyle? defaultStyle1 = defaultStyle;
    if (PdfRichTextString.TryGetCache(annotName, richText, defaultStyle1, out pdfRichTextString))
      return true;
    XmlDocument xmlDocument = new XmlDocument();
    try
    {
      xmlDocument.LoadXml(richText);
    }
    catch
    {
      PdfRichTextString.SetCache("", richText, defaultStyle, (PdfRichTextString) null);
      return false;
    }
    XmlElement documentElement = xmlDocument.DocumentElement;
    PdfRichTextStyle pdfRichTextStyle;
    if (!defaultStyle.HasValue && PdfRichTextStyle.TryParse(documentElement.ChildNodes.OfType<XmlNode>().FirstOrDefault<XmlNode>((Func<XmlNode, bool>) (c => c.Name == "body"))?.Attributes.GetNamedItem("style")?.Value, out pdfRichTextStyle))
      defaultStyle = new PdfRichTextStyle?(pdfRichTextStyle);
    List<PdfRichTextRawElement> list = PdfRichTextString.ParseCore((XmlNode) documentElement, defaultStyle).ToList<PdfRichTextRawElement>();
    if (list == null || list.Count == 0)
      return false;
    pdfRichTextString = new PdfRichTextString()
    {
      Text = list[0]
    };
    if (defaultStyle.HasValue)
      pdfRichTextString.DefaultStyle = defaultStyle.Value;
    PdfRichTextString.SetCache(annotName, richText, defaultStyle1, pdfRichTextString);
    return true;
  }

  private static IEnumerable<PdfRichTextRawElement> GetTextWithActualFontFamily(
    string text,
    PdfRichTextStyle style)
  {
    if (string.IsNullOrEmpty(text))
      return Enumerable.Empty<PdfRichTextRawElement>();
    string fontFamily = style.FontFamily ?? "Arial";
    float? fontSize = style.FontSize;
    IReadOnlyList<TextWithFallbackFontFamily> withFallbackFonts = PdfFontUtils.GetTextWithFallbackFonts(text, fontFamily, 12f, cultureInfo: CultureInfo.CurrentUICulture);
    if (withFallbackFonts.Count == 0)
      return Enumerable.Empty<PdfRichTextRawElement>();
    return withFallbackFonts.Count == 1 && (withFallbackFonts[0].FallbackFontFamily == null || withFallbackFonts[0].FallbackFontFamily.Source == fontFamily) ? Enumerable.Repeat<PdfRichTextRawElement>(new PdfRichTextRawElement(text), 1) : (IEnumerable<PdfRichTextRawElement>) withFallbackFonts.Select<TextWithFallbackFontFamily, PdfRichTextElement>((Func<TextWithFallbackFontFamily, PdfRichTextElement>) (c => new PdfRichTextElement()
    {
      Children = {
        new PdfRichTextRawElement(c.Text)
      },
      Tag = PdfRichTextElementTag.Span,
      Style = new PdfRichTextStyle?(new PdfRichTextStyle()
      {
        FontFamily = c.FallbackFontFamily?.Source ?? fontFamily,
        FontSize = fontSize
      })
    }));
  }

  private static IEnumerable<PdfRichTextRawElement> ParseCore(
    XmlNode node,
    PdfRichTextStyle? defaultStyle)
  {
    if (node == null)
      return Enumerable.Empty<PdfRichTextRawElement>();
    if (node is XmlText xmlText && !string.IsNullOrEmpty(xmlText.InnerText))
      return PdfRichTextString.GetTextWithActualFontFamily(xmlText.InnerText, defaultStyle ?? PdfRichTextStyle.Default);
    PdfRichTextElementTag? tag = PdfRichTextString.GetTag(node.Name);
    if (!tag.HasValue)
      return node.Name.ToLowerInvariant() == "div" ? node.ChildNodes.OfType<XmlNode>().SelectMany<XmlNode, PdfRichTextRawElement>((Func<XmlNode, IEnumerable<PdfRichTextRawElement>>) (c => PdfRichTextString.ParseCore(c, defaultStyle))) : Enumerable.Empty<PdfRichTextRawElement>();
    PdfRichTextElement element = new PdfRichTextElement()
    {
      Tag = tag.Value
    };
    XmlNode namedItem = node.Attributes.GetNamedItem("style");
    PdfRichTextStyle? nullable = defaultStyle;
    if (namedItem != null && !string.IsNullOrEmpty(namedItem.Value))
    {
      if (defaultStyle.HasValue)
      {
        PdfRichTextStyle pdfRichTextStyle;
        if (PdfRichTextStyle.TryParse(namedItem.Value, defaultStyle.Value, out pdfRichTextStyle))
        {
          element.Style = new PdfRichTextStyle?(pdfRichTextStyle);
          defaultStyle = new PdfRichTextStyle?(pdfRichTextStyle);
        }
      }
      else
      {
        PdfRichTextStyle pdfRichTextStyle;
        if (PdfRichTextStyle.TryParse(namedItem.Value, out pdfRichTextStyle))
        {
          element.Style = new PdfRichTextStyle?(pdfRichTextStyle);
          defaultStyle = new PdfRichTextStyle?(pdfRichTextStyle);
        }
      }
    }
    if (element.Style.HasValue && nullable.HasValue && nullable.Value.Equals(element.Style.Value))
      element.Style = new PdfRichTextStyle?();
    if (node.ChildNodes.Count != 0)
    {
      foreach (XmlNode node1 in node.ChildNodes.OfType<XmlNode>())
      {
        IEnumerable<PdfRichTextRawElement> core = PdfRichTextString.ParseCore(node1, defaultStyle);
        if (core != null)
          element.Children.AddRange(core);
      }
    }
    return (IEnumerable<PdfRichTextRawElement>) Enumerable.Repeat<PdfRichTextElement>(element, 1);
  }

  private static PdfRichTextElementTag? GetTag(string name)
  {
    if (string.IsNullOrEmpty(name))
      return new PdfRichTextElementTag?();
    switch (name.Trim().ToLowerInvariant())
    {
      case "body":
        return new PdfRichTextElementTag?(PdfRichTextElementTag.Body);
      case "p":
        return new PdfRichTextElementTag?(PdfRichTextElementTag.P);
      case "i":
        return new PdfRichTextElementTag?(PdfRichTextElementTag.I);
      case "b":
        return new PdfRichTextElementTag?(PdfRichTextElementTag.B);
      case "span":
        return new PdfRichTextElementTag?(PdfRichTextElementTag.Span);
      default:
        return new PdfRichTextElementTag?();
    }
  }

  private static void SetCache(
    string name,
    string richText,
    PdfRichTextStyle? defaultStyle,
    PdfRichTextString pdfRichTextString)
  {
    if (string.IsNullOrEmpty(name))
      name = "-- Empty --";
    lock (PdfRichTextString.cache)
    {
      if (pdfRichTextString != null)
      {
        PdfRichTextString.RemoveCacheByName(name, 5);
        PdfRichTextString.PdfRichTextStringCacheKey key = new PdfRichTextString.PdfRichTextStringCacheKey(name, richText, defaultStyle);
        PdfRichTextString.cache[key] = pdfRichTextString;
      }
      else
        PdfRichTextString.RemoveCacheByName(name);
    }
  }

  private static bool TryGetCache(
    string name,
    string richText,
    PdfRichTextStyle? defaultStyle,
    out PdfRichTextString pdfRichTextString)
  {
    pdfRichTextString = (PdfRichTextString) null;
    if (PdfRichTextString.cache.Count == 0)
      return false;
    if (string.IsNullOrEmpty(name) || name == "-- Empty --")
    {
      lock (PdfRichTextString.cache)
      {
        if (PdfRichTextString.cache.Count != 0)
        {
          PdfRichTextString.PdfRichTextStringCacheKey textStringCacheKey = new PdfRichTextString.PdfRichTextStringCacheKey(name, richText, defaultStyle);
          foreach (KeyValuePair<PdfRichTextString.PdfRichTextStringCacheKey, PdfRichTextString> keyValuePair in PdfRichTextString.cache)
          {
            if (keyValuePair.Key.Equals((object) textStringCacheKey))
            {
              pdfRichTextString = keyValuePair.Value;
              return true;
            }
          }
        }
        pdfRichTextString = (PdfRichTextString) null;
        return false;
      }
    }
    PdfRichTextString.PdfRichTextStringCacheKey key = new PdfRichTextString.PdfRichTextStringCacheKey(name, richText, defaultStyle);
    return PdfRichTextString.cache.TryGetValue(key, out pdfRichTextString);
  }

  public static void RemoveCacheByName(string name, int keepLastCount = 0)
  {
    if (string.IsNullOrEmpty(name))
      name = "-- Empty --";
    if (PdfRichTextString.cache.Count == 0)
      return;
    lock (PdfRichTextString.cache)
    {
      if (PdfRichTextString.cache.Count == 0)
        return;
      KeyValuePair<PdfRichTextString.PdfRichTextStringCacheKey, PdfRichTextString>[] array = PdfRichTextString.cache.Where<KeyValuePair<PdfRichTextString.PdfRichTextStringCacheKey, PdfRichTextString>>((Func<KeyValuePair<PdfRichTextString.PdfRichTextStringCacheKey, PdfRichTextString>, bool>) (c => c.Key.Name == name)).ToArray<KeyValuePair<PdfRichTextString.PdfRichTextStringCacheKey, PdfRichTextString>>();
      for (int index = array.Length - keepLastCount - 1; index >= 0; --index)
        PdfRichTextString.cache.Remove(array[index]);
    }
  }

  private struct PdfRichTextStringCacheKey(
    string name,
    string richText,
    PdfRichTextStyle? defaultStyle)
  {
    public string Name { get; } = name;

    public string RichText { get; } = richText;

    public PdfRichTextStyle? DefaultStyle { get; } = defaultStyle;

    public void Deconstruct(
      out string name,
      out string richText,
      out PdfRichTextStyle? defaultStyle)
    {
      name = this.Name;
      richText = this.RichText;
      defaultStyle = this.DefaultStyle;
    }

    public override bool Equals(object obj)
    {
      int num;
      if (obj is PdfRichTextString.PdfRichTextStringCacheKey textStringCacheKey)
      {
        PdfRichTextStyle? defaultStyle = this.DefaultStyle;
        if (!defaultStyle.HasValue)
        {
          defaultStyle = textStringCacheKey.DefaultStyle;
          if (!defaultStyle.HasValue)
            goto label_6;
        }
        defaultStyle = this.DefaultStyle;
        if (defaultStyle.HasValue)
        {
          defaultStyle = textStringCacheKey.DefaultStyle;
          if (defaultStyle.HasValue)
          {
            defaultStyle = this.DefaultStyle;
            PdfRichTextStyle pdfRichTextStyle = defaultStyle.Value;
            ref PdfRichTextStyle local = ref pdfRichTextStyle;
            defaultStyle = textStringCacheKey.DefaultStyle;
            PdfRichTextStyle other = defaultStyle.Value;
            if (!local.Equals(other))
              goto label_8;
          }
          else
            goto label_8;
        }
        else
          goto label_8;
label_6:
        if (this.Name == textStringCacheKey.Name)
        {
          num = this.RichText == textStringCacheKey.RichText ? 1 : 0;
          goto label_9;
        }
      }
label_8:
      num = 0;
label_9:
      return num != 0;
    }
  }
}
