// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageHeaderFooters.LocationStringParser
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace pdfeditor.Controls.PageHeaderFooters;

public class LocationStringParser
{
  public static IEnumerable<LocationStringParser.LocationToken> GetTokens(string str)
  {
    if (!string.IsNullOrEmpty(str))
    {
      LocationStringParser.LocationToken? curToken = new LocationStringParser.LocationToken?();
      StringBuilder sb = new StringBuilder();
      IEnumerator<LocationStringParser.LocationToken> ie = LocationStringParser.GetTokensCore(str).GetEnumerator();
      while (ie.MoveNext())
      {
        LocationStringParser.LocationToken token = ie.Current;
        if (token.Tokenize == LocationStringParser.LocationTokenize.PagePlaceholder)
        {
          if (curToken.HasValue)
          {
            yield return LocationStringParser.LocationToken.CreateString(sb.ToString());
            sb.Length = 0;
            curToken = new LocationStringParser.LocationToken?();
          }
          yield return token;
        }
        else
        {
          sb.Append(token.Text);
          curToken = new LocationStringParser.LocationToken?(token);
        }
        token = new LocationStringParser.LocationToken();
      }
      if (curToken.HasValue)
        yield return LocationStringParser.LocationToken.CreateString(sb.ToString());
    }
  }

  private static IEnumerable<LocationStringParser.LocationToken> GetTokensCore(string str)
  {
    if (!string.IsNullOrEmpty(str))
    {
      StringInfo strInfo = new StringInfo(str);
      int num = 0;
      int startingTextElement = -1;
      for (int i = 0; i < strInfo.LengthInTextElements; ++i)
      {
        string text = strInfo.SubstringByTextElements(i, 1);
        switch (text)
        {
          case "<":
            switch (num)
            {
              case 0:
                if (startingTextElement != -1)
                  yield return LocationStringParser.LocationToken.CreateString(strInfo.SubstringByTextElements(startingTextElement, i - startingTextElement));
                startingTextElement = i;
                num = 1;
                continue;
              case 1:
                num = 2;
                continue;
              default:
                if (i + 1 < strInfo.LengthInTextElements)
                {
                  yield return LocationStringParser.LocationToken.CreateString(strInfo.SubstringByTextElements(startingTextElement, i - startingTextElement - 1));
                  startingTextElement = i - 1;
                  num = 1;
                  continue;
                }
                continue;
            }
          case ">":
            switch (num)
            {
              case 1:
              case 2:
                num = 0;
                if (startingTextElement == -1)
                {
                  startingTextElement = i;
                  continue;
                }
                continue;
              case 3:
                num = 4;
                continue;
              case 4:
                yield return LocationStringParser.LocationToken.CreatePagePlaceholder(strInfo.SubstringByTextElements(startingTextElement, i - startingTextElement + 1));
                num = 0;
                startingTextElement = -1;
                continue;
              default:
                if (startingTextElement != -1)
                  yield return LocationStringParser.LocationToken.CreateString(strInfo.SubstringByTextElements(startingTextElement, i - startingTextElement));
                else
                  yield return LocationStringParser.LocationToken.CreateString(text);
                num = 0;
                startingTextElement = -1;
                continue;
            }
          default:
            switch (num)
            {
              case 2:
                num = 3;
                continue;
              case 3:
                continue;
              default:
                num = 0;
                if (startingTextElement == -1)
                {
                  startingTextElement = i;
                  continue;
                }
                continue;
            }
        }
      }
      if (startingTextElement != -1)
        yield return LocationStringParser.LocationToken.CreateString(strInfo.SubstringByTextElements(startingTextElement, strInfo.LengthInTextElements - startingTextElement));
    }
  }

  public enum LocationTokenize
  {
    String,
    PagePlaceholder,
  }

  public struct LocationToken
  {
    public LocationToken(LocationStringParser.LocationTokenize tokenize, string text)
    {
      if (string.IsNullOrEmpty(text))
        throw new ArgumentException(nameof (text));
      this.Tokenize = tokenize;
      this.Text = text;
    }

    public LocationStringParser.LocationTokenize Tokenize { get; }

    public string Text { get; }

    public static LocationStringParser.LocationToken CreateString(string text)
    {
      return new LocationStringParser.LocationToken(LocationStringParser.LocationTokenize.String, text);
    }

    public static LocationStringParser.LocationToken CreatePagePlaceholder(string text)
    {
      return new LocationStringParser.LocationToken(LocationStringParser.LocationTokenize.PagePlaceholder, text);
    }
  }
}
