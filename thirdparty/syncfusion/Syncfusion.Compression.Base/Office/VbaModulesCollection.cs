// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.VbaModulesCollection
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Office;

internal class VbaModulesCollection : CollectionBase<VbaModule>, IVbaModules, IEnumerable
{
  private VbaProject m_project;

  internal VbaProject Project => this.m_project;

  public IVbaModule this[string name]
  {
    get
    {
      foreach (VbaModule vbaModule in (CollectionBase<VbaModule>) this)
      {
        if (string.Equals(name, vbaModule.Name, StringComparison.OrdinalIgnoreCase))
          return (IVbaModule) vbaModule;
      }
      return (IVbaModule) null;
    }
  }

  public IVbaModule this[int index] => (IVbaModule) this.InnerList[index];

  internal VbaModulesCollection()
  {
  }

  internal VbaModulesCollection(VbaProject project) => this.m_project = project;

  public IVbaModule Add(string name, VbaModuleType type)
  {
    VbaModule vbaModule = new VbaModule(this);
    vbaModule.Name = name;
    vbaModule.Type = type;
    vbaModule.InitializeAttributes(name, this.Project.ProjectCLSID);
    this.Add(vbaModule);
    return (IVbaModule) vbaModule;
  }

  public void Remove(string name)
  {
    if (this[name] == null)
      return;
    this.Remove(this[name] as VbaModule);
  }

  internal void Dispose()
  {
    foreach (VbaModule vbaModule in (CollectionBase<VbaModule>) this)
      vbaModule.Dispose();
  }

  internal VbaModulesCollection Clone(VbaProject parent)
  {
    VbaModulesCollection vbaModules = (VbaModulesCollection) this.Clone();
    vbaModules.m_project = parent;
    for (int index = 0; index < this.InnerList.Count; ++index)
    {
      VbaModule inner = this.InnerList[index];
      vbaModules.Add(inner.Clone(vbaModules));
    }
    return vbaModules;
  }
}
