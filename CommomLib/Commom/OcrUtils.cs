// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.OcrUtils
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Tesseract;

#nullable disable
namespace CommomLib.Commom;

public static class OcrUtils
{
  private static object locker = new object();
  private static Dictionary<string, OcrUtils.LanguageInfo> cultureInfoLanguageNameDict;

  private static Dictionary<string, OcrUtils.LanguageInfo> CultureInfoLanguageNameDict
  {
    get
    {
      if (OcrUtils.cultureInfoLanguageNameDict == null)
      {
        lock (OcrUtils.locker)
        {
          if (OcrUtils.cultureInfoLanguageNameDict == null)
            OcrUtils.cultureInfoLanguageNameDict = OcrUtils.CreateCultureInfoLanguageDictionary();
        }
      }
      return OcrUtils.cultureInfoLanguageNameDict;
    }
  }

  public static async Task<string> GetStringAsync(BitmapSource image, CultureInfo cultureInfo)
  {
    string lang = OcrUtils.TryGetOCRLanguage(cultureInfo);
    if (string.IsNullOrEmpty(lang))
      lang = "eng";
    using (Pix pix = OcrUtils.ConvertToPix(image))
    {
      if (pix != null)
        return await Task.Run<string>((Func<string>) (() =>
        {
          using (TesseractEngine tesseractEngine = new TesseractEngine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata"), lang, EngineMode.Default))
          {
            using (Page page = tesseractEngine.Process(pix))
            {
              if (page != null)
                return page.GetText();
            }
          }
          return string.Empty;
        }));
    }
    return string.Empty;
  }

  public static async Task<string> GetStringAsync(Image bitmap, CultureInfo cultureInfo)
  {
    string lang = OcrUtils.TryGetOCRLanguage(cultureInfo);
    if (string.IsNullOrEmpty(lang))
      lang = "eng";
    using (MemoryStream ms = new MemoryStream())
    {
      bitmap.Save((Stream) ms, System.Drawing.Imaging.ImageFormat.Png);
      ms.Seek(0L, SeekOrigin.Begin);
      using (Pix pix = Pix.LoadFromMemory(ms.GetBuffer()))
      {
        if (pix != null)
          return await Task.Run<string>((Func<string>) (() =>
          {
            using (TesseractEngine tesseractEngine = new TesseractEngine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata"), lang, EngineMode.Default))
            {
              using (Page page = tesseractEngine.Process(pix))
              {
                if (page != null)
                  return page.GetText();
              }
            }
            return string.Empty;
          }));
      }
    }
    return string.Empty;
  }

  public static async Task<(string, CultureInfo)> GetStringAndCultureAsync(
    Image bitmap,
    CancellationToken cancellationToken)
  {
    List<string> list = new List<string>()
    {
      "eng",
      "jpn",
      "chi_sim",
      "chi_tra",
      "kor",
      "deu",
      "rus"
    };
    Dictionary<string, (float, string)> dict = new Dictionary<string, (float, string)>();
    foreach (OcrUtils.LanguageInfo languageInfo in OcrUtils.CultureInfoLanguageNameDict.Values)
    {
      if (!list.Contains(languageInfo.OCRLanguage))
        list.Add(languageInfo.OCRLanguage);
    }
    try
    {
      using (MemoryStream ms = new MemoryStream())
      {
        bitmap.Save((Stream) ms, System.Drawing.Imaging.ImageFormat.Png);
        ms.Seek(0L, SeekOrigin.Begin);
        using (Pix pix = Pix.LoadFromMemory(ms.GetBuffer()))
        {
          if (pix != null)
            return await Task.Run<(string, CultureInfo)>((Func<(string, CultureInfo)>) (() =>
            {
              foreach (string str in list)
              {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                  using (TesseractEngine tesseractEngine = new TesseractEngine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata"), str, EngineMode.Default))
                  {
                    using (Page page = tesseractEngine.Process(pix))
                    {
                      if (page != null)
                      {
                        float meanConfidence = page.GetMeanConfidence();
                        if ((double) meanConfidence > 0.5)
                        {
                          dict[str] = (meanConfidence, page.GetText());
                          if ((double) meanConfidence > 0.85)
                            break;
                        }
                      }
                    }
                  }
                }
                catch
                {
                }
                cancellationToken.ThrowIfCancellationRequested();
              }
              if (dict.Count <= 0)
                return ();
              KeyValuePair<string, (float, string)> result = dict.OrderByDescending<KeyValuePair<string, (float, string)>, float>((Func<KeyValuePair<string, (float, string)>, float>) (c => c.Value.Item1)).FirstOrDefault<KeyValuePair<string, (float, string)>>();
              string cultureInfoName = OcrUtils.CultureInfoLanguageNameDict.FirstOrDefault<KeyValuePair<string, OcrUtils.LanguageInfo>>((Func<KeyValuePair<string, OcrUtils.LanguageInfo>, bool>) (c => c.Value.OCRLanguage == result.Key)).Value?.CultureInfoName;
              return (result.Value.Item2, CultureInfo.GetCultureInfo(cultureInfoName));
            }), cancellationToken);
        }
      }
    }
    catch
    {
    }
    return ();
  }

  private static Pix ConvertToPix(BitmapSource image)
  {
    if (image == null)
      return (Pix) null;
    PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
    pngBitmapEncoder.Frames.Add(BitmapFrame.Create(image));
    using (MemoryStream memoryStream = new MemoryStream())
    {
      pngBitmapEncoder.Save((Stream) memoryStream);
      memoryStream.Seek(0L, SeekOrigin.Begin);
      byte[] numArray = new byte[memoryStream.Length];
      memoryStream.Read(numArray, 0, numArray.Length);
      return Pix.LoadFromMemory(numArray);
    }
  }

  private static Dictionary<string, OcrUtils.LanguageInfo> CreateCultureInfoLanguageDictionary()
  {
    return new Dictionary<string, OcrUtils.LanguageInfo>()
    {
      ["de"] = new OcrUtils.LanguageInfo("de", "deu", "Deutsch"),
      ["es"] = new OcrUtils.LanguageInfo("es", "spa", "Español"),
      ["fr"] = new OcrUtils.LanguageInfo("fr", "fra", "Français"),
      ["it"] = new OcrUtils.LanguageInfo("it", "ita", "Italiano"),
      ["ja"] = new OcrUtils.LanguageInfo("ja", "jpn", "日本語"),
      ["ko"] = new OcrUtils.LanguageInfo("ko", "kor", "한국어"),
      ["nl"] = new OcrUtils.LanguageInfo("nl", "nld", "Nederlands"),
      ["pt"] = new OcrUtils.LanguageInfo("pt", "por", "Português"),
      ["en"] = new OcrUtils.LanguageInfo("en", "eng", "English"),
      ["ru"] = new OcrUtils.LanguageInfo("ru", "rus", "Pусский"),
      ["zh-Hans"] = new OcrUtils.LanguageInfo("zh-Hans", "chi_sim", "中文 (简体)"),
      ["zh-Hant"] = new OcrUtils.LanguageInfo("zh-Hant", "chi_tra", "中文 (繁體)")
    };
  }

  private static string TryGetOCRLanguage(CultureInfo cultureInfo)
  {
    OcrUtils.LanguageInfo languageInfo;
    return OcrUtils.CultureInfoLanguageNameDict.TryGetValue(OcrUtils.GetPresetCultureInfoName(cultureInfo), out languageInfo) ? languageInfo.OCRLanguage : (string) null;
  }

  public static List<(string cultureInfoName, string displayName)> GetLanguageList()
  {
    return OcrUtils.CultureInfoLanguageNameDict.Select<KeyValuePair<string, OcrUtils.LanguageInfo>, (string, string)>((Func<KeyValuePair<string, OcrUtils.LanguageInfo>, (string, string)>) (c => (c.Value.CultureInfoName, c.Value.DisplayName))).ToList<(string, string)>();
  }

  public static string GetLanguageDisplayName(CultureInfo cultureInfo)
  {
    OcrUtils.LanguageInfo languageInfo;
    return OcrUtils.CultureInfoLanguageNameDict.TryGetValue(OcrUtils.GetPresetCultureInfoName(cultureInfo), out languageInfo) ? languageInfo.DisplayName : (string) null;
  }

  private static string GetPresetCultureInfoName(CultureInfo cultureInfo)
  {
    for (; !string.IsNullOrEmpty(cultureInfo?.Name); cultureInfo = cultureInfo.Parent)
    {
      OcrUtils.LanguageInfo languageInfo;
      if (OcrUtils.CultureInfoLanguageNameDict.TryGetValue(cultureInfo.Name, out languageInfo))
        return languageInfo.CultureInfoName;
    }
    return (string) null;
  }

  public static string GetDefaultCultureInfoName()
  {
    string defaultCultureInfoName = OcrUtils.GetPresetCultureInfoName(CultureInfo.CurrentUICulture);
    if (string.IsNullOrEmpty(defaultCultureInfoName))
      defaultCultureInfoName = "en";
    return defaultCultureInfoName;
  }

  private class LanguageInfo
  {
    public LanguageInfo(string cultureInfoName, string oCRLanguage, string displayName)
    {
      this.CultureInfoName = cultureInfoName;
      this.OCRLanguage = oCRLanguage;
      this.DisplayName = displayName;
    }

    public string CultureInfoName { get; }

    public string OCRLanguage { get; }

    public string DisplayName { get; }
  }
}
