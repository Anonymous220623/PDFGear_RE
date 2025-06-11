// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.ParameterAsserts
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace XmpCore.Impl;

internal static class ParameterAsserts
{
  public static void AssertArrayName(string arrayName)
  {
    if (string.IsNullOrEmpty(arrayName))
      throw new XmpException("Empty array name", XmpErrorCode.BadParam);
  }

  public static void AssertPropName(string propName)
  {
    if (string.IsNullOrEmpty(propName))
      throw new XmpException("Empty property name", XmpErrorCode.BadParam);
  }

  public static void AssertSchemaNs(string schemaNs)
  {
    if (string.IsNullOrEmpty(schemaNs))
      throw new XmpException("Empty schema namespace URI", XmpErrorCode.BadParam);
  }

  public static void AssertPrefix(string prefix)
  {
    if (string.IsNullOrEmpty(prefix))
      throw new XmpException("Empty prefix", XmpErrorCode.BadParam);
  }

  public static void AssertSpecificLang(string specificLang)
  {
    if (string.IsNullOrEmpty(specificLang))
      throw new XmpException("Empty specific language", XmpErrorCode.BadParam);
  }

  public static void AssertStructName(string structName)
  {
    if (string.IsNullOrEmpty(structName))
      throw new XmpException("Empty array name", XmpErrorCode.BadParam);
  }

  public static void AssertNotNull(object param)
  {
    if (param == null)
      throw new XmpException("Parameter must not be null", XmpErrorCode.BadParam);
  }

  public static void AssertNotNullOrEmpty(string param)
  {
    switch (param)
    {
      case null:
        throw new XmpException("Parameter must not be null", XmpErrorCode.BadParam);
      case "":
        throw new XmpException("Parameter must not be an empty string", XmpErrorCode.BadParam);
    }
  }

  public static void AssertImplementation(IXmpMeta xmp)
  {
    if (xmp == null)
      throw new XmpException("Parameter must not be null", XmpErrorCode.BadParam);
    if (!(xmp is XmpMeta))
      throw new XmpException("The XMPMeta-object is not compatible with this implementation", XmpErrorCode.BadParam);
  }
}
