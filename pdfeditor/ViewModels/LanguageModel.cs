// Decompiled with JetBrains decompiler
// Type: pdfeditor.ViewModels.LanguageModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using pdfeditor.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

#nullable disable
namespace pdfeditor.ViewModels;

public class LanguageModel
{
  private static LanguageModel fallbackLanguageModel;
  private static IReadOnlyList<string> allLanguages;
  private static IReadOnlyList<LanguageModel> languageModels;
  private static object locker = new object();

  public static LanguageModel Fallback
  {
    get
    {
      return LanguageModel.fallbackLanguageModel ?? (LanguageModel.fallbackLanguageModel = (LanguageModel) new LanguageModel.FallbackLanguageModel());
    }
  }

  public static IReadOnlyList<string> AllLanguages
  {
    get
    {
      if (LanguageModel.allLanguages == null)
      {
        lock (LanguageModel.locker)
        {
          if (LanguageModel.allLanguages == null)
            LanguageModel.allLanguages = LanguageModel.GetAllLanguages();
        }
      }
      return LanguageModel.allLanguages;
    }
  }

  public static IReadOnlyList<LanguageModel> AllLanguageModel
  {
    get
    {
      if (LanguageModel.languageModels == null)
      {
        lock (LanguageModel.locker)
        {
          if (LanguageModel.languageModels == null)
            LanguageModel.languageModels = (IReadOnlyList<LanguageModel>) LanguageModel.AllLanguages.Select<string, LanguageModel>((Func<string, LanguageModel>) (c => new LanguageModel(c))).ToList<LanguageModel>();
        }
      }
      return LanguageModel.languageModels;
    }
  }

  protected LanguageModel()
  {
  }

  public LanguageModel(string name)
  {
    this.Name = name;
    this.CultureInfo = CultureInfo.GetCultureInfo(name);
    this.NativeName = this.CultureInfo.NativeName;
    this.EnglishName = this.CultureInfo.EnglishName;
  }

  public virtual string NativeName { get; }

  public virtual string EnglishName { get; }

  public virtual string ResourceName => this.Name;

  public virtual CultureInfo CultureInfo { get; }

  public virtual string Name { get; }

  private static IReadOnlyList<string> GetAllLanguages()
  {
    return (IReadOnlyList<string>) ((IEnumerable<string>) new string[1]
    {
      "en"
    }).Concat<string>(((IEnumerable<DirectoryInfo>) new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetDirectories()).Where<DirectoryInfo>((Func<DirectoryInfo, bool>) (c =>
    {
      try
      {
        return ((IEnumerable<FileInfo>) c.GetFiles()).Any<FileInfo>((Func<FileInfo, bool>) (x => x.Name == "pdfeditor.resources.dll"));
      }
      catch
      {
      }
      return false;
    })).Select<DirectoryInfo, string>((Func<DirectoryInfo, string>) (c => c.Name))).OrderBy<string, string>((Func<string, string>) (c => CultureInfo.GetCultureInfo(c)?.EnglishName), (IComparer<string>) StringComparer.OrdinalIgnoreCase).ToList<string>();
  }

  private class FallbackLanguageModel : LanguageModel
  {
    private string englishName;

    public FallbackLanguageModel()
    {
      foreach (LanguageModel languageModel in (IEnumerable<LanguageModel>) LanguageModel.AllLanguageModel)
      {
        if (languageModel.Name == CultureInfoUtils.SuggestAppLanguage)
        {
          this.englishName = languageModel.EnglishName;
          break;
        }
      }
    }

    public override string Name => "";

    public override CultureInfo CultureInfo => CultureInfoUtils.SystemUICultureInfo;

    public override string NativeName => Resources.AppSettingsLanguageAutoItem;

    public override string EnglishName => this.englishName;
  }
}
