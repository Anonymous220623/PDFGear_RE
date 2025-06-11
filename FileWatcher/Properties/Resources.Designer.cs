// Decompiled with JetBrains decompiler
// Type: FileWatcher.Properties.Resources
// Assembly: FileWatcher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 60845E26-CA8C-4ACF-A930-D757CD9B4993
// Assembly location: C:\Program Files\PDFgear\FileWatcher.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace FileWatcher.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Resources
{
  private static ResourceManager resourceMan;
  private static CultureInfo resourceCulture;

  internal Resources()
  {
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  public static ResourceManager ResourceManager
  {
    get
    {
      if (FileWatcher.Properties.Resources.resourceMan == null)
        FileWatcher.Properties.Resources.resourceMan = new ResourceManager("FileWatcher.Properties.Resources", typeof (FileWatcher.Properties.Resources).Assembly);
      return FileWatcher.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  public static CultureInfo Culture
  {
    get => FileWatcher.Properties.Resources.resourceCulture;
    set => FileWatcher.Properties.Resources.resourceCulture = value;
  }

  public static string WinRecivedBtnOpenFile
  {
    get
    {
      return FileWatcher.Properties.Resources.ResourceManager.GetString(nameof (WinRecivedBtnOpenFile), FileWatcher.Properties.Resources.resourceCulture);
    }
  }

  public static string WinRecivedFileTitle
  {
    get
    {
      return FileWatcher.Properties.Resources.ResourceManager.GetString(nameof (WinRecivedFileTitle), FileWatcher.Properties.Resources.resourceCulture);
    }
  }

  public static string WinRecivedNotshowagainContent
  {
    get
    {
      return FileWatcher.Properties.Resources.ResourceManager.GetString(nameof (WinRecivedNotshowagainContent), FileWatcher.Properties.Resources.resourceCulture);
    }
  }
}
