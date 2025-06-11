// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.SingleOpenHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Tools;

public class SingleOpenHelper
{
  private static readonly Dictionary<string, ISingleOpen> OpenDic = new Dictionary<string, ISingleOpen>();

  public static T CreateControl<T>() where T : Visual, ISingleOpen, new()
  {
    string fullName = typeof (T).FullName;
    if (string.IsNullOrEmpty(fullName))
      return default (T);
    T control = new T();
    if (!SingleOpenHelper.OpenDic.Keys.Contains<string>(fullName))
    {
      SingleOpenHelper.OpenDic.Add(fullName, (ISingleOpen) control);
      return control;
    }
    ISingleOpen singleOpen = SingleOpenHelper.OpenDic[fullName];
    if (!singleOpen.CanDispose)
      return default (T);
    singleOpen.Dispose();
    SingleOpenHelper.OpenDic[fullName] = (ISingleOpen) control;
    return control;
  }
}
