// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.WindowsIdentityLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System.ComponentModel;
using System.Security.Principal;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("windows-identity")]
[ThreadSafe]
public class WindowsIdentityLayoutRenderer : LayoutRenderer
{
  public WindowsIdentityLayoutRenderer()
  {
    this.UserName = true;
    this.Domain = true;
  }

  [DefaultValue(true)]
  public bool Domain { get; set; }

  [DefaultValue(true)]
  public bool UserName { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    WindowsIdentity currentIdentity = WindowsIdentityLayoutRenderer.GetValue();
    if (currentIdentity == null)
      return;
    string str;
    if (this.UserName)
    {
      str = this.Domain ? WindowsIdentityLayoutRenderer.GetUserNameWithDomain(currentIdentity) : WindowsIdentityLayoutRenderer.GetUserNameWithoutDomain(currentIdentity);
    }
    else
    {
      if (!this.Domain)
        return;
      str = WindowsIdentityLayoutRenderer.GetDomainOnly(currentIdentity);
    }
    builder.Append(str);
  }

  private static string GetDomainOnly(WindowsIdentity currentIdentity)
  {
    int length = currentIdentity.Name.IndexOf('\\');
    return length < 0 ? currentIdentity.Name : currentIdentity.Name.Substring(0, length);
  }

  private static string GetUserNameWithoutDomain(WindowsIdentity currentIdentity)
  {
    int num = currentIdentity.Name.LastIndexOf('\\');
    return num < 0 ? currentIdentity.Name : currentIdentity.Name.Substring(num + 1);
  }

  private static string GetUserNameWithDomain(WindowsIdentity currentIdentity)
  {
    return currentIdentity.Name;
  }

  private static WindowsIdentity GetValue() => WindowsIdentity.GetCurrent();
}
