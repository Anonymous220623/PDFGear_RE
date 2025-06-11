// Decompiled with JetBrains decompiler
// Type: HandyControl.Themes.SharedResourceDictionary
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Windows;

#nullable disable
namespace HandyControl.Themes;

public class SharedResourceDictionary : ResourceDictionary
{
  public static Dictionary<Uri, ResourceDictionary> SharedDictionaries = new Dictionary<Uri, ResourceDictionary>();
  private Uri _sourceUri;

  public new Uri Source
  {
    get => !DesignerHelper.IsInDesignMode ? this._sourceUri : base.Source;
    set
    {
      if (value == (Uri) null)
        return;
      if (DesignerHelper.IsInDesignMode)
      {
        base.Source = value;
      }
      else
      {
        this._sourceUri = value;
        if (!SharedResourceDictionary.SharedDictionaries.ContainsKey(value))
        {
          base.Source = value;
          SharedResourceDictionary.SharedDictionaries.Add(value, (ResourceDictionary) this);
        }
        else
          this.MergedDictionaries.Add(SharedResourceDictionary.SharedDictionaries[value]);
      }
    }
  }
}
