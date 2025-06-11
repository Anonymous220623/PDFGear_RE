// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageHeaderFooters.PagePlaceholderFormatter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using PDFKit.Utils.XObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace pdfeditor.Controls.PageHeaderFooters;

public class PagePlaceholderFormatter
{
  private static object _locker = new object();
  private static IReadOnlyList<string> allSupportedPageNumberFormats;
  private static IReadOnlyList<string> allSupportedDateFormats;

  public static IReadOnlyList<string> AllSupportedPageNumberFormats
  {
    get
    {
      if (PagePlaceholderFormatter.allSupportedPageNumberFormats == null)
      {
        lock (PagePlaceholderFormatter._locker)
        {
          if (PagePlaceholderFormatter.allSupportedPageNumberFormats == null)
            PagePlaceholderFormatter.allSupportedPageNumberFormats = (IReadOnlyList<string>) new string[6]
            {
              "1",
              "1 - n",
              "1/n",
              "1 of n",
              "Page 1",
              "Page 1 of n"
            };
        }
      }
      return PagePlaceholderFormatter.allSupportedPageNumberFormats;
    }
  }

  public static IReadOnlyList<string> AllSupportedDateFormats
  {
    get
    {
      if (PagePlaceholderFormatter.allSupportedDateFormats == null)
      {
        lock (PagePlaceholderFormatter._locker)
        {
          if (PagePlaceholderFormatter.allSupportedDateFormats == null)
            PagePlaceholderFormatter.allSupportedDateFormats = (IReadOnlyList<string>) new string[18]
            {
              "m/d",
              "m/d/yy",
              "m/d/yyyy",
              "mm/dd/yy",
              "mm/dd/yyyy",
              "d/m/yy",
              "d/m/yyyy",
              "dd/mm/yy",
              "dd/mm/yyyy",
              "mm/yyyy",
              "m.d.yyyy",
              "mm.dd.yyyy",
              "mm.yy",
              "mm.yyyy",
              "d.m.yy",
              "d.m.yyyy",
              "yy-mm-dd",
              "yyyy-mm-dd"
            };
        }
      }
      return PagePlaceholderFormatter.allSupportedDateFormats;
    }
  }

  public static string DateModelToString(HeaderFooterSettings.DateModel model)
  {
    if (model == null)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<<");
    stringBuilder.Append(PagePlaceholderFormatter.DateModelToPlaceholder(model));
    stringBuilder.Append(">>");
    return stringBuilder.ToString();
  }

  public static HeaderFooterSettings.DateModel StringToDateModel(string str)
  {
    LocationStringParser.LocationToken[] array = LocationStringParser.GetTokens(str).ToArray<LocationStringParser.LocationToken>();
    return array.Length != 1 ? (HeaderFooterSettings.DateModel) null : PagePlaceholderFormatter.GetDateModel(array[0].Text.Substring(2, array[0].Text.Length - 4).Trim());
  }

  public static string PageModelToString(HeaderFooterSettings.PageModel model)
  {
    if (model == null)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<<");
    stringBuilder.Append(PagePlaceholderFormatter.PageModelToPlaceholder(model));
    stringBuilder.Append(">>");
    return stringBuilder.ToString();
  }

  public static HeaderFooterSettings.PageModel StringToPageModel(string str, int pageOffset)
  {
    LocationStringParser.LocationToken[] array = LocationStringParser.GetTokens(str).ToArray<LocationStringParser.LocationToken>();
    return array.Length != 1 ? (HeaderFooterSettings.PageModel) null : PagePlaceholderFormatter.GetPageModel(array[0].Text.Substring(2, array[0].Text.Length - 4).Trim(), pageOffset);
  }

  public static string LocationToString(HeaderFooterSettings.LocationModel model)
  {
    if (model == null)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    foreach (object obj in (HeaderFooterSettings.VariableCollection) model)
    {
      switch (obj)
      {
        case HeaderFooterSettings.PageModel model1:
          stringBuilder.Append(PagePlaceholderFormatter.PageModelToString(model1));
          continue;
        case HeaderFooterSettings.DateModel model2:
          stringBuilder.Append(PagePlaceholderFormatter.DateModelToString(model2));
          continue;
        case string str:
          stringBuilder.Append(str);
          continue;
        default:
          continue;
      }
    }
    return stringBuilder.ToString();
  }

  public static void StringToLocation(
    HeaderFooterSettings.LocationModel location,
    string str,
    int pageOffset)
  {
    location.Clear();
    foreach (LocationStringParser.LocationToken token in LocationStringParser.GetTokens(str))
    {
      if (token.Tokenize == LocationStringParser.LocationTokenize.String)
        location.Add((object) token.Text);
      else if (token.Tokenize == LocationStringParser.LocationTokenize.PagePlaceholder)
      {
        string placeholder = token.Text.Substring(2, token.Text.Length - 4).Trim();
        HeaderFooterSettings.DateModel dateModel = PagePlaceholderFormatter.GetDateModel(placeholder);
        if (dateModel != null)
        {
          location.Add((object) dateModel);
        }
        else
        {
          HeaderFooterSettings.PageModel pageModel = PagePlaceholderFormatter.GetPageModel(placeholder, pageOffset);
          if (pageModel != null)
            location.Add((object) pageModel);
          else
            location.Add((object) token.Text);
        }
      }
    }
  }

  private static HeaderFooterSettings.PageModel GetPageModel(string placeholder, int pageOffset)
  {
    string str = placeholder.Trim();
    HeaderFooterSettings.PageModel pageModel1 = (HeaderFooterSettings.PageModel) null;
    switch (str)
    {
      case "1":
        HeaderFooterSettings.PageModel pageModel2 = new HeaderFooterSettings.PageModel();
        pageModel2.Add((object) new HeaderFooterSettings.VariableModel("PageIndex")
        {
          Format = "1"
        });
        pageModel1 = pageModel2;
        break;
      case "1 - n":
        HeaderFooterSettings.PageModel pageModel3 = new HeaderFooterSettings.PageModel();
        pageModel3.Add((object) new HeaderFooterSettings.VariableModel("PageIndex")
        {
          Format = "1"
        });
        pageModel3.Add((object) " - ");
        pageModel3.Add((object) new HeaderFooterSettings.VariableModel("PageTotalNum")
        {
          Format = "n"
        });
        pageModel1 = pageModel3;
        break;
      case "1/n":
        HeaderFooterSettings.PageModel pageModel4 = new HeaderFooterSettings.PageModel();
        pageModel4.Add((object) new HeaderFooterSettings.VariableModel("PageIndex")
        {
          Format = "1"
        });
        pageModel4.Add((object) "/");
        pageModel4.Add((object) new HeaderFooterSettings.VariableModel("PageTotalNum")
        {
          Format = "n"
        });
        pageModel1 = pageModel4;
        break;
      case "1 of n":
        HeaderFooterSettings.PageModel pageModel5 = new HeaderFooterSettings.PageModel();
        pageModel5.Add((object) new HeaderFooterSettings.VariableModel("PageIndex")
        {
          Format = "1"
        });
        pageModel5.Add((object) "of");
        pageModel5.Add((object) new HeaderFooterSettings.VariableModel("PageTotalNum")
        {
          Format = "n"
        });
        pageModel1 = pageModel5;
        break;
      case "Page 1":
        HeaderFooterSettings.PageModel pageModel6 = new HeaderFooterSettings.PageModel();
        pageModel6.Add((object) "Page");
        pageModel6.Add((object) new HeaderFooterSettings.VariableModel("PageIndex")
        {
          Format = "1"
        });
        pageModel1 = pageModel6;
        break;
      case "Page 1 of n":
        HeaderFooterSettings.PageModel pageModel7 = new HeaderFooterSettings.PageModel();
        pageModel7.Add((object) "Page");
        pageModel7.Add((object) new HeaderFooterSettings.VariableModel("PageIndex")
        {
          Format = "1"
        });
        pageModel7.Add((object) "of");
        pageModel7.Add((object) new HeaderFooterSettings.VariableModel("PageTotalNum")
        {
          Format = "n"
        });
        pageModel1 = pageModel7;
        break;
    }
    if (pageModel1 != null)
      pageModel1.Offset = Math.Max(0, pageOffset - 1);
    return pageModel1;
  }

  private static HeaderFooterSettings.DateModel GetDateModel(string placeholder)
  {
    HashSet<char> hash = new HashSet<char>()
    {
      'd',
      'm',
      'y',
      '.',
      '/',
      '-'
    };
    if (placeholder.Any<char>((Func<char, bool>) (c => !hash.Contains(c))))
      return (HeaderFooterSettings.DateModel) null;
    char ch = char.MinValue;
    int capacity = 0;
    HeaderFooterSettings.DateModel dateModel = new HeaderFooterSettings.DateModel();
    for (int index1 = 0; index1 <= placeholder.Length; ++index1)
    {
      char minValue = char.MinValue;
      bool flag;
      if (index1 < placeholder.Length)
      {
        minValue = placeholder[index1];
        flag = ch != char.MinValue && (int) minValue != (int) ch;
      }
      else
        flag = true;
      if (flag)
      {
        switch (ch)
        {
          case 'd':
            dateModel.Add((object) new HeaderFooterSettings.VariableModel("Day")
            {
              Format = $"{capacity}"
            });
            break;
          case 'm':
            dateModel.Add((object) new HeaderFooterSettings.VariableModel("Month")
            {
              Format = $"{capacity}"
            });
            break;
          case 'y':
            dateModel.Add((object) new HeaderFooterSettings.VariableModel("Year")
            {
              Format = $"{capacity}"
            });
            break;
          default:
            StringBuilder stringBuilder = new StringBuilder(capacity);
            for (int index2 = 0; index2 < capacity; ++index2)
              stringBuilder.Append(ch);
            dateModel.Add((object) stringBuilder.ToString());
            break;
        }
        capacity = 0;
      }
      ch = minValue;
      ++capacity;
    }
    return dateModel;
  }

  private static string PageModelToPlaceholder(HeaderFooterSettings.PageModel page)
  {
    if (page == null)
      return PagePlaceholderFormatter.AllSupportedPageNumberFormats[0];
    string placeholderCore = PagePlaceholderFormatter.PageModelToPlaceholderCore(page);
    if (string.IsNullOrEmpty(placeholderCore))
      return PagePlaceholderFormatter.AllSupportedPageNumberFormats[0];
    for (int index = 0; index < PagePlaceholderFormatter.AllSupportedPageNumberFormats.Count; ++index)
    {
      if (PagePlaceholderFormatter.AllSupportedPageNumberFormats[index] == placeholderCore)
        return PagePlaceholderFormatter.AllSupportedPageNumberFormats[index];
    }
    return PagePlaceholderFormatter.AllSupportedPageNumberFormats[0];
  }

  private static string DateModelToPlaceholder(HeaderFooterSettings.DateModel date)
  {
    if (date == null)
      return PagePlaceholderFormatter.AllSupportedDateFormats[0];
    string placeholderCore = PagePlaceholderFormatter.DateModelToPlaceholderCore(date);
    if (string.IsNullOrEmpty(placeholderCore))
      return PagePlaceholderFormatter.AllSupportedDateFormats[0];
    for (int index = 0; index < PagePlaceholderFormatter.AllSupportedDateFormats.Count; ++index)
    {
      if (PagePlaceholderFormatter.AllSupportedDateFormats[index] == placeholderCore)
        return PagePlaceholderFormatter.AllSupportedDateFormats[index];
    }
    return PagePlaceholderFormatter.AllSupportedDateFormats[0];
  }

  private static string PageModelToPlaceholderCore(HeaderFooterSettings.PageModel page)
  {
    if (page == null)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    foreach (object obj in (HeaderFooterSettings.VariableCollection) page)
    {
      if (obj is string str)
        stringBuilder.Append(str);
      else if (obj is HeaderFooterSettings.VariableModel variableModel)
      {
        if (variableModel.Name == "PageIndex")
          stringBuilder.Append('1');
        else if (variableModel.Name == "PageTotalNum")
          stringBuilder.Append('n');
      }
    }
    string placeholderCore = stringBuilder.ToString().Trim();
    if (placeholderCore == "1ofn")
      placeholderCore = "1 of n";
    if (placeholderCore == "Page1")
      placeholderCore = "Page 1";
    if (placeholderCore == "Page1ofn")
      placeholderCore = "Page 1 of n";
    return placeholderCore;
  }

  private static string DateModelToPlaceholderCore(HeaderFooterSettings.DateModel date)
  {
    if (date == null)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    foreach (object obj in (HeaderFooterSettings.VariableCollection) date)
    {
      if (obj is string str)
        stringBuilder.Append(str);
      else if (obj is HeaderFooterSettings.VariableModel variableModel)
      {
        int result1;
        if (variableModel.Name == "Day" && int.TryParse(variableModel.Format, out result1))
        {
          for (int index = 0; index < result1; ++index)
            stringBuilder.Append('d');
        }
        else
        {
          int result2;
          if (variableModel.Name == "Month" && int.TryParse(variableModel.Format, out result2))
          {
            for (int index = 0; index < result2; ++index)
              stringBuilder.Append('m');
          }
          else
          {
            int result3;
            if (variableModel.Name == "Year" && int.TryParse(variableModel.Format, out result3))
            {
              for (int index = 0; index < result3; ++index)
                stringBuilder.Append('y');
            }
          }
        }
      }
    }
    return stringBuilder.ToString().Trim();
  }
}
