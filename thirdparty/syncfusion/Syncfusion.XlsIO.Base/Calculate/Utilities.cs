// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.Utilities
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Security;
using System.Security.Permissions;

#nullable disable
namespace Syncfusion.Calculate;

internal class Utilities
{
  public static bool IsSecurityPermissionAvailable()
  {
    bool flag = true;
    SecurityPermission securityPermission = new SecurityPermission(PermissionState.Unrestricted);
    try
    {
      securityPermission.Demand();
    }
    catch (SecurityException ex)
    {
      flag = false;
    }
    return flag;
  }

  internal static void ValidateLicense(Type typeToValidate)
  {
    try
    {
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(XlsIOBaseAssembly.AssemblyResolver);
    }
    finally
    {
      AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(XlsIOBaseAssembly.AssemblyResolver);
    }
  }
}
