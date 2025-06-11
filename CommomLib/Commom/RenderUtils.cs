// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.RenderUtils
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media;

#nullable disable
namespace CommomLib.Commom;

public class RenderUtils : INotifyPropertyChanged
{
  private static RenderUtils instance;
  private static object instanceLock = new object();
  private bool isUnsupportedDriver;

  public static RenderUtils Instance
  {
    get
    {
      if (RenderUtils.instance == null)
      {
        lock (RenderUtils.instanceLock)
        {
          if (RenderUtils.instance == null)
          {
            RenderUtils.instance = new RenderUtils();
            if (RenderUtils.instance.RenderModeSoftwareOnly)
            {
              RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
              GAManager.SendEvent("Debug", "SoftwareRenderSet", "AppInitSoftwareOnly", 1L);
            }
          }
        }
      }
      return RenderUtils.instance;
    }
  }

  public static void Init()
  {
    RenderUtils instance = RenderUtils.Instance;
  }

  private RenderUtils()
  {
    if (AppDataHelper.LocalSettings.Values.TryGetValue("debug_use_software_render", out string _))
      return;
    Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
    {
      try
      {
        Intel11thGraphicsDeviceHelper[] graphicsDeviceHelperArray = Intel11thGraphicsDeviceHelper.Create();
        if (graphicsDeviceHelperArray.Length == 0)
          return;
        for (int index = 0; index < graphicsDeviceHelperArray.Length; ++index)
        {
          Intel11thGraphicsDeviceHelper graphicsDeviceHelper = graphicsDeviceHelperArray[index];
          if (graphicsDeviceHelper.DriverVersion2 != new Version(0, 0, 0, 0) && graphicsDeviceHelper.DriverVersion2 < new Version(30, 0, 0, 0))
          {
            this.isUnsupportedDriver = true;
            GAManager.SendEvent("Debug", "IntelDeviceInfo", "IssueDriverDetect", 1L);
            if (this.RenderModeSoftwareOnly)
            {
              RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
              GAManager.SendEvent("Debug", "SoftwareRenderSet", "DetectSoftwareOnly", 1L);
            }
          }
        }
      }
      catch
      {
      }
    })));
  }

  public bool RenderModeSoftwareOnly
  {
    get
    {
      string str;
      return AppDataHelper.LocalSettings.Values.TryGetValue("debug_use_software_render", out str) && str != null ? str == "true" : this.isUnsupportedDriver;
    }
    set
    {
      if (this.RenderModeSoftwareOnly == value)
        return;
      AppDataHelper.LocalSettings.Values["debug_use_software_render"] = value ? "true" : "false";
      if (value)
      {
        GAManager.SendEvent("Debug", "SoftwareRenderSet", "UserSoftwareOnly", 1L);
        RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
      }
      else
      {
        GAManager.SendEvent("Debug", "SoftwareRenderSet", "UserDefault", 1L);
        RenderOptions.ProcessRenderMode = RenderMode.Default;
      }
      this.OnPropertyChanged(nameof (RenderModeSoftwareOnly));
    }
  }

  private void OnPropertyChanged([CallerMemberName] string propName = "")
  {
    PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
    if (propertyChanged == null)
      return;
    propertyChanged((object) this, new PropertyChangedEventArgs(propName));
  }

  public event PropertyChangedEventHandler PropertyChanged;
}
