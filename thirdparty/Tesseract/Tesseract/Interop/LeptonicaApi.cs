// Decompiled with JetBrains decompiler
// Type: Tesseract.Interop.LeptonicaApi
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using InteropDotNet;

#nullable disable
namespace Tesseract.Interop;

internal static class LeptonicaApi
{
  private static ILeptonicaApiSignatures native;

  public static ILeptonicaApiSignatures Native
  {
    get
    {
      if (LeptonicaApi.native == null)
        LeptonicaApi.Initialize();
      return LeptonicaApi.native;
    }
  }

  public static void Initialize()
  {
    if (LeptonicaApi.native != null)
      return;
    LeptonicaApi.native = InteropRuntimeImplementer.CreateInstance<ILeptonicaApiSignatures>();
  }
}
