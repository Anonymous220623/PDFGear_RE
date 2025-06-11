// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.IsEnabledResourceKeyExtension
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Markup;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class IsEnabledResourceKeyExtension : ResourceKey
{
  private Assembly m_asm;

  public ResourceKeyState State { get; set; }

  public string ID { get; set; }

  public IsEnabledResourceKeyExtension()
    : this(ResourceKeyState.Disabled, string.Empty)
  {
  }

  public IsEnabledResourceKeyExtension(ResourceKeyState state, string id)
  {
    this.ID = id;
    this.State = state;
  }

  public override bool Equals(object obj)
  {
    IsEnabledResourceKeyExtension resourceKeyExtension = (IsEnabledResourceKeyExtension) obj;
    return resourceKeyExtension.ID == this.ID && resourceKeyExtension.State == this.State;
  }

  public override int GetHashCode() => this.ID.GetHashCode() ^ this.State.GetHashCode();

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("ID=");
    stringBuilder.Append(this.ID);
    stringBuilder.Append(";State=");
    stringBuilder.Append(this.State.ToString());
    return stringBuilder.ToString();
  }

  public override object ProvideValue(IServiceProvider serviceProvider)
  {
    if (serviceProvider.GetService(typeof (IUriContext)) is IUriContext service)
    {
      string segment = service.BaseUri.Segments[1];
      int startIndex = segment.IndexOf(";");
      if (startIndex >= 0)
      {
        string str = segment.Remove(startIndex);
        Assembly assembly1 = (Assembly) null;
        foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
        {
          if (assembly2.GetName().Name == str)
          {
            assembly1 = assembly2;
            break;
          }
        }
        this.m_asm = assembly1;
      }
      else
        this.m_asm = Application.ResourceAssembly;
    }
    return base.ProvideValue(serviceProvider);
  }

  public override Assembly Assembly
  {
    get
    {
      return !(this.m_asm == (Assembly) null) ? this.m_asm : throw new InvalidOperationException("Target resource assembly can not be found.");
    }
  }
}
