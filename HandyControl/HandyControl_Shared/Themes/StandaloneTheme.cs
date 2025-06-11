// Decompiled with JetBrains decompiler
// Type: HandyControl.Themes.StandaloneTheme
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools;
using System.Windows;

#nullable disable
namespace HandyControl.Themes;

public class StandaloneTheme : Theme
{
  public override ResourceDictionary GetTheme() => ResourceHelper.GetStandaloneTheme();
}
