// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.StampUtils.StampTemplates.ContentSerializer
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace PDFKit.Utils.StampUtils.StampTemplates;

internal static class ContentSerializer
{
  public static string Serialize(Dictionary<string, string> contentDict)
  {
    if (contentDict == null || contentDict.Count == 0)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    foreach (KeyValuePair<string, string> keyValuePair in contentDict)
    {
      if (!string.IsNullOrEmpty(keyValuePair.Key))
        stringBuilder.Append(Uri.EscapeDataString(keyValuePair.Key)).Append('=').Append(Uri.EscapeDataString(keyValuePair.Value ?? "")).Append('&');
    }
    if (stringBuilder.Length > 0)
      --stringBuilder.Length;
    return stringBuilder.ToString();
  }

  public static Dictionary<string, string> Deserialize(string content)
  {
    if (string.IsNullOrEmpty(content))
      return (Dictionary<string, string>) null;
    string[] strArray1 = content.Split('&');
    if (strArray1 == null || strArray1.Length == 0)
      return (Dictionary<string, string>) null;
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    for (int index = 0; index < strArray1.Length; ++index)
    {
      string[] strArray2 = strArray1[index].Split('=');
      if (strArray2 != null && strArray2.Length == 2)
      {
        try
        {
          dictionary[Uri.UnescapeDataString(strArray2[0])] = Uri.UnescapeDataString(strArray2[1]);
        }
        catch
        {
        }
      }
    }
    return dictionary;
  }
}
