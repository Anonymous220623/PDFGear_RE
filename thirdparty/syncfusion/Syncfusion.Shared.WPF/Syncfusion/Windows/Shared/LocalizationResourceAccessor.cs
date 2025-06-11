// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.LocalizationResourceAccessor
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class LocalizationResourceAccessor
{
  public ResourceManager Resources;

  public LocalizationResourceAccessor()
  {
    ResourceManager localizedResourceManager = this.GetLocalizedResourceManager(Application.Current == null ? Application.ResourceAssembly : Application.Current.GetType().Assembly, (string) null);
    if (localizedResourceManager == null)
      return;
    this.Resources = localizedResourceManager;
  }

  protected virtual Assembly GetControlAssembly()
  {
    throw new NotImplementedException("Should be implemented by derived class");
  }

  protected virtual string GetControlAssemblyDefaultNamespace()
  {
    throw new NotImplementedException("Should be implemented by derived class");
  }

  protected virtual ResourceManager GetDefaultResourceManager()
  {
    throw new NotImplementedException("Should be implemented by derived class");
  }

  private ResourceManager GetLocalizedResourceManager(Assembly lookupassembly, string nameSpace)
  {
    string str = this.GetControlAssembly().FullName.Split(',')[0];
    if (lookupassembly == (Assembly) null)
      lookupassembly = Assembly.GetEntryAssembly();
    if (string.IsNullOrEmpty(nameSpace))
    {
      if (lookupassembly != (Assembly) null)
        nameSpace = lookupassembly.FullName.Split(',')[0];
    }
    try
    {
      ResourceManager localizedResourceManager = (ResourceManager) null;
      if (lookupassembly != (Assembly) null)
      {
        string resourcePathForVB = $"{nameSpace}.{str}.resources";
        string resourcePathForCS = $"{nameSpace}.Resources.{str}.resources";
        string[] manifestResourceNames = lookupassembly.GetManifestResourceNames();
        string empty = string.Empty;
        bool flag1 = ((IEnumerable<string>) manifestResourceNames).Where<string>((Func<string, bool>) (resource => resource.ToLower() == resourcePathForCS.ToLower())).Any<string>();
        bool flag2 = ((IEnumerable<string>) manifestResourceNames).Where<string>((Func<string, bool>) (resource => resource.ToLower() == resourcePathForVB.ToLower())).Any<string>();
        if (flag1)
          localizedResourceManager = new ResourceManager($"{nameSpace}.Resources.{str}", lookupassembly);
        else if (flag2)
        {
          localizedResourceManager = new ResourceManager($"{nameSpace}.{str}", lookupassembly);
        }
        else
        {
          Assembly controlAssembly = this.GetControlAssembly();
          try
          {
            lookupassembly = lookupassembly.GetSatelliteAssembly(CultureInfo.CurrentUICulture);
            foreach (string manifestResourceName in lookupassembly.GetManifestResourceNames())
            {
              if (manifestResourceName.Contains(controlAssembly.FullName.Split(',')[0]))
              {
                localizedResourceManager = new ResourceManager(manifestResourceName.Replace(".resources", ""), lookupassembly);
                break;
              }
            }
          }
          catch (Exception ex)
          {
          }
        }
      }
      if (localizedResourceManager == null)
        localizedResourceManager = this.GetDefaultResourceManager();
      if (localizedResourceManager == null)
        localizedResourceManager = new ResourceManager($"{this.GetControlAssemblyDefaultNamespace()}.Resources.{str}", this.GetControlAssembly());
      if (localizedResourceManager != null)
      {
        CultureInfo currentUiCulture = CultureInfo.CurrentUICulture;
        if (localizedResourceManager.GetResourceSet(currentUiCulture, true, true) != null)
          return localizedResourceManager;
      }
    }
    catch (Exception ex)
    {
      return (ResourceManager) null;
    }
    return (ResourceManager) null;
  }

  public string GetString(string name, params object[] args)
  {
    return this.GetString(CultureInfo.CurrentUICulture, name);
  }

  public string GetString(string name) => this.GetString(CultureInfo.CurrentUICulture, name);

  public string GetString(CultureInfo culture, string name)
  {
    try
    {
      string str = this.Resources.GetString(name, culture);
      return !string.IsNullOrEmpty(str) ? str : name;
    }
    catch (Exception ex)
    {
      return name;
    }
  }

  public void SetResources(Assembly lookupassembly, string nameSpace)
  {
    this.Resources = this.GetLocalizedResourceManager(lookupassembly, nameSpace);
  }
}
