// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.ModernStyleDictionary
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Windows;

#nullable disable
namespace CommomLib.Controls;

public class ModernStyleDictionary : ResourceDictionary
{
  private static string assemblyName;

  public ModernStyleDictionary()
  {
    ModernStyleDictionary.assemblyName = typeof (ModernStyleDictionary).Assembly.GetName().Name;
    this.MergedDictionaries.Add(this.Resource("/Controls/ScrollBar.xaml"));
    this.MergedDictionaries.Add(this.Resource("/Controls/CommonStyles.xaml"));
  }

  private Uri GetUri(string file)
  {
    if (file.StartsWith("/"))
      file = file.Substring(1);
    return new Uri($"pack://application:,,,/{ModernStyleDictionary.assemblyName};component/{file}");
  }

  private ResourceDictionary Resource(string file)
  {
    return new ResourceDictionary()
    {
      Source = this.GetUri(file)
    };
  }
}
