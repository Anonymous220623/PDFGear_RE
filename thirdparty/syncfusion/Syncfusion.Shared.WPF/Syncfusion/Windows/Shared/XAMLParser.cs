// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.XAMLParser
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class XAMLParser
{
  [Obsolete("GetFomattedXAML is deprecated, please use GetFormattedXAML instead.")]
  public static Paragraph GetFomattedXAML(string xamlText) => XAMLParser.GetFormattedXAML(xamlText);

  public static Paragraph GetFormattedXAML(string xamlText)
  {
    xamlText = Regex.Replace(xamlText, "[ ]*=[ ]*", "=", RegexOptions.Multiline);
    XamlTokenizer xamlTokenizer = new XamlTokenizer();
    XamlTokenizerMode x_mode = XamlTokenizerMode.OutsideElement;
    List<XamlToken> xamlTokenList = xamlTokenizer.Tokenize(xamlText, ref x_mode);
    List<string> stringList = new List<string>(xamlTokenList.Count);
    List<Color> colorList = new List<Color>(xamlTokenList.Count);
    int startIndex = 0;
    foreach (XamlToken token in xamlTokenList)
    {
      string tokenText = xamlText.Substring(startIndex, (int) token.Length);
      stringList.Add(tokenText);
      Color color = XamlTokenizer.ColorForToken(token, tokenText);
      colorList.Add(color);
      startIndex += (int) token.Length;
    }
    Paragraph formattedXaml = new Paragraph();
    for (int index = 0; index < stringList.Count; ++index)
    {
      Run run = new Run(stringList[index]);
      run.Foreground = (Brush) new SolidColorBrush(colorList[index]);
      formattedXaml.Inlines.Add((Inline) run);
    }
    return formattedXaml;
  }
}
