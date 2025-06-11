﻿// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRichTextStrings.HtmlToXaml.CssStylesheet
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#nullable disable
namespace PDFKit.Utils.PdfRichTextStrings.HtmlToXaml;

internal class CssStylesheet
{
  private List<CssStylesheet.StyleDefinition> _styleDefinitions;

  public CssStylesheet(XmlElement htmlElement)
  {
    if (htmlElement == null)
      return;
    this.DiscoverStyleDefinitions(htmlElement);
  }

  public void DiscoverStyleDefinitions(XmlElement htmlElement)
  {
    if (htmlElement.LocalName.ToLower() == "link")
      return;
    if (htmlElement.LocalName.ToLower() != "style")
    {
      for (XmlNode htmlElement1 = htmlElement.FirstChild; htmlElement1 != null; htmlElement1 = htmlElement1.NextSibling)
      {
        if (htmlElement1 is XmlElement)
          this.DiscoverStyleDefinitions((XmlElement) htmlElement1);
      }
    }
    else
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (XmlNode xmlNode = htmlElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
      {
        if (xmlNode is XmlText || xmlNode is XmlComment)
          stringBuilder.Append(this.RemoveComments(xmlNode.Value));
      }
      int index = 0;
      while (index < stringBuilder.Length)
      {
        int startIndex = index;
        for (; index < stringBuilder.Length && stringBuilder[index] != '{'; ++index)
        {
          if (stringBuilder[index] == '@')
          {
            while (index < stringBuilder.Length && stringBuilder[index] != ';')
              ++index;
            startIndex = index + 1;
          }
        }
        if (index < stringBuilder.Length)
        {
          int num = index;
          while (index < stringBuilder.Length && stringBuilder[index] != '}')
            ++index;
          if (index - num > 2)
            this.AddStyleDefinition(stringBuilder.ToString(startIndex, num - startIndex), stringBuilder.ToString(num + 1, index - num - 2));
          if (index < stringBuilder.Length)
            ++index;
        }
      }
    }
  }

  private string RemoveComments(string text)
  {
    int length = text.IndexOf("/*", StringComparison.Ordinal);
    if (length < 0)
      return text;
    int num = text.IndexOf("*/", length + 2, StringComparison.Ordinal);
    return num < 0 ? text.Substring(0, length) : $"{text.Substring(0, length)} {this.RemoveComments(text.Substring(num + 2))}";
  }

  public void AddStyleDefinition(string selector, string definition)
  {
    selector = selector.Trim().ToLower();
    definition = definition.Trim().ToLower();
    if (selector.Length == 0 || definition.Length == 0)
      return;
    if (this._styleDefinitions == null)
      this._styleDefinitions = new List<CssStylesheet.StyleDefinition>();
    string str1 = selector;
    char[] chArray = new char[1]{ ',' };
    foreach (string str2 in str1.Split(chArray))
    {
      string selector1 = str2.Trim();
      if (selector1.Length > 0)
        this._styleDefinitions.Add(new CssStylesheet.StyleDefinition(selector1, definition));
    }
  }

  public string GetStyle(string elementName, List<XmlElement> sourceContext)
  {
    if (this._styleDefinitions != null)
    {
      for (int index1 = this._styleDefinitions.Count - 1; index1 >= 0; --index1)
      {
        string[] strArray = this._styleDefinitions[index1].Selector.Split(' ');
        int index2 = strArray.Length - 1;
        int num = sourceContext.Count - 1;
        if (this.MatchSelectorLevel(strArray[index2].Trim(), sourceContext[sourceContext.Count - 1]))
          return this._styleDefinitions[index1].Definition;
      }
    }
    return (string) null;
  }

  private bool MatchSelectorLevel(string selectorLevel, XmlElement xmlElement)
  {
    if (selectorLevel.Length == 0)
      return false;
    int length1 = selectorLevel.IndexOf('.');
    int length2 = selectorLevel.IndexOf('#');
    string str1 = (string) null;
    string str2 = (string) null;
    string str3 = (string) null;
    if (length1 >= 0)
    {
      if (length1 > 0)
        str3 = selectorLevel.Substring(0, length1);
      str1 = selectorLevel.Substring(length1 + 1);
    }
    else if (length2 >= 0)
    {
      if (length2 > 0)
        str3 = selectorLevel.Substring(0, length2);
      str2 = selectorLevel.Substring(length2 + 1);
    }
    else
      str3 = selectorLevel;
    return (str3 == null || !(str3 != xmlElement.LocalName)) && (str2 == null || !(HtmlToXamlConverter.GetAttribute(xmlElement, "id") != str2)) && (str1 == null || !(HtmlToXamlConverter.GetAttribute(xmlElement, "class") != str1));
  }

  private class StyleDefinition
  {
    public readonly string Definition;
    public readonly string Selector;

    public StyleDefinition(string selector, string definition)
    {
      this.Selector = selector;
      this.Definition = definition;
    }
  }
}
