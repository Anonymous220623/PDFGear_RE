// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.SkinStorageExtension
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Collections.ObjectModel;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal static class SkinStorageExtension
{
  private static ObservableCollection<string> MergedDictionaryPathCollection;

  internal static void AddIntoMergedDictionaryPath(this FrameworkElement element, string path)
  {
    SkinStorageExtension.MergedDictionaryPathCollection = SkinStorage.GetMergedDictionaryPath((DependencyObject) element);
    if (SkinStorageExtension.MergedDictionaryPathCollection == null)
      SkinStorageExtension.MergedDictionaryPathCollection = new ObservableCollection<string>();
    if (SkinStorageExtension.MergedDictionaryPathCollection.Contains(path))
      return;
    SkinStorageExtension.MergedDictionaryPathCollection.Add(path);
  }

  internal static int IndexOfMergedDictionaryPath(this FrameworkElement element, string path)
  {
    SkinStorageExtension.MergedDictionaryPathCollection = SkinStorage.GetMergedDictionaryPath((DependencyObject) element);
    return SkinStorageExtension.MergedDictionaryPathCollection == null ? -1 : SkinStorageExtension.MergedDictionaryPathCollection.IndexOf(path);
  }

  internal static void RemoveFromMergedDictionaryPath(this FrameworkElement element, string path)
  {
    SkinStorageExtension.MergedDictionaryPathCollection = SkinStorage.GetMergedDictionaryPath((DependencyObject) element);
    if (SkinStorageExtension.MergedDictionaryPathCollection == null || !SkinStorageExtension.MergedDictionaryPathCollection.Contains(path))
      return;
    SkinStorageExtension.MergedDictionaryPathCollection.Remove(path);
  }

  internal static bool IsContainsInMergedDictionaryPath(this FrameworkElement element, string path)
  {
    SkinStorageExtension.MergedDictionaryPathCollection = SkinStorage.GetMergedDictionaryPath((DependencyObject) element);
    return SkinStorageExtension.MergedDictionaryPathCollection != null && SkinStorageExtension.MergedDictionaryPathCollection.Contains(path);
  }

  internal static ObservableCollection<string> GetMergedDictionaryPath(this FrameworkElement element)
  {
    SkinStorageExtension.MergedDictionaryPathCollection = SkinStorage.GetMergedDictionaryPath((DependencyObject) element);
    return SkinStorageExtension.MergedDictionaryPathCollection;
  }

  internal static void ClearMergedDictionaryPath(this FrameworkElement element)
  {
    SkinStorageExtension.MergedDictionaryPathCollection = SkinStorage.GetMergedDictionaryPath((DependencyObject) element);
    if (SkinStorageExtension.MergedDictionaryPathCollection == null)
      return;
    SkinStorageExtension.MergedDictionaryPathCollection.Clear();
    SkinStorageExtension.MergedDictionaryPathCollection = (ObservableCollection<string>) null;
  }
}
