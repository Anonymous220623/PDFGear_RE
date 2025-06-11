// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.LicenceManage
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using Syncfusion.Licensing;

#nullable disable
namespace CommomLib.Commom;

public static class LicenceManage
{
  private static bool sycfusionIsCall;

  public static bool SycfusionRegisterLicence()
  {
    if (!LicenceManage.sycfusionIsCall)
    {
      try
      {
        SyncfusionLicenseProvider.RegisterLicense("NTQ0NTIxQDMxMzkyZTMzMmUzMEFOQnNrMC9aSWFVRHdYdTN3UFJPMDZKTTJaZ0UzV21LMG9BcXRGOVRvUTg9");
        LicenceManage.sycfusionIsCall = true;
      }
      catch
      {
        LicenceManage.sycfusionIsCall = false;
      }
    }
    return LicenceManage.sycfusionIsCall;
  }
}
