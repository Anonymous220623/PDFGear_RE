// Decompiled with JetBrains decompiler
// Type: NLog.SetupExtensionsBuilderExtensions
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using NLog.LayoutRenderers;
using NLog.Targets;
using System;
using System.Reflection;

#nullable disable
namespace NLog;

public static class SetupExtensionsBuilderExtensions
{
  public static ISetupExtensionsBuilder AutoLoadAssemblies(
    this ISetupExtensionsBuilder setupBuilder,
    bool enable)
  {
    ConfigurationItemFactory configurationItemFactory;
    if (!enable)
      configurationItemFactory = new ConfigurationItemFactory(new Assembly[1]
      {
        typeof (SetupBuilderExtensions).GetAssembly()
      });
    else
      configurationItemFactory = (ConfigurationItemFactory) null;
    ConfigurationItemFactory.Default = configurationItemFactory;
    return setupBuilder;
  }

  public static ISetupExtensionsBuilder RegisterAssembly(
    this ISetupExtensionsBuilder setupBuilder,
    Assembly assembly)
  {
    ConfigurationItemFactory.Default.RegisterItemsFromAssembly(assembly);
    return setupBuilder;
  }

  public static ISetupExtensionsBuilder RegisterAssembly(
    this ISetupExtensionsBuilder setupBuilder,
    string assemblyName)
  {
    ConfigurationItemFactory.Default.RegisterItemsFromAssembly(AssemblyHelpers.LoadFromName(assemblyName));
    return setupBuilder;
  }

  public static ISetupExtensionsBuilder RegisterTarget<T>(
    this ISetupExtensionsBuilder setupBuilder,
    string name)
    where T : Target
  {
    Type targetType = typeof (T);
    return setupBuilder.RegisterTarget(name, targetType);
  }

  public static ISetupExtensionsBuilder RegisterTarget(
    this ISetupExtensionsBuilder setupBuilder,
    string name,
    Type targetType)
  {
    ConfigurationItemFactory.Default.Targets.RegisterDefinition(name, targetType);
    return setupBuilder;
  }

  public static ISetupExtensionsBuilder RegisterLayoutRenderer<T>(
    this ISetupExtensionsBuilder setupBuilder,
    string name)
    where T : LayoutRenderer
  {
    Type layoutRendererType = typeof (T);
    return setupBuilder.RegisterLayoutRenderer(name, layoutRendererType);
  }

  public static ISetupExtensionsBuilder RegisterLayoutRenderer(
    this ISetupExtensionsBuilder setupBuilder,
    string name,
    Type layoutRendererType)
  {
    ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition(name, layoutRendererType);
    return setupBuilder;
  }

  public static ISetupExtensionsBuilder RegisterLayoutRenderer(
    this ISetupExtensionsBuilder setupBuilder,
    string name,
    Func<LogEventInfo, object> layoutMethod)
  {
    return setupBuilder.RegisterLayoutRenderer(name, (Func<LogEventInfo, LoggingConfiguration, object>) ((info, configuration) => layoutMethod(info)));
  }

  public static ISetupExtensionsBuilder RegisterLayoutRenderer(
    this ISetupExtensionsBuilder setupBuilder,
    string name,
    Func<LogEventInfo, LoggingConfiguration, object> layoutMethod)
  {
    FuncLayoutRenderer renderer = new FuncLayoutRenderer(name, layoutMethod);
    ConfigurationItemFactory.Default.GetLayoutRenderers().RegisterFuncLayout(name, renderer);
    return setupBuilder;
  }

  public static ISetupExtensionsBuilder RegisterConditionMethod(
    this ISetupExtensionsBuilder setupBuilder,
    string name,
    MethodInfo conditionMethod)
  {
    if (conditionMethod == (MethodInfo) null)
      throw new ArgumentNullException(nameof (conditionMethod));
    if (!conditionMethod.IsStatic)
      throw new ArgumentException(conditionMethod.Name + " must be static", nameof (conditionMethod));
    ConfigurationItemFactory.Default.ConditionMethods.RegisterDefinition(name, conditionMethod);
    return setupBuilder;
  }

  private static ISetupExtensionsBuilder RegisterConditionMethod(
    this ISetupExtensionsBuilder setupBuilder,
    string name,
    MethodInfo conditionMethod,
    ReflectionHelpers.LateBoundMethod lateBoundMethod)
  {
    ConfigurationItemFactory.Default.ConditionMethodDelegates.RegisterDefinition(name, conditionMethod, lateBoundMethod);
    return setupBuilder;
  }

  public static ISetupExtensionsBuilder RegisterConditionMethod(
    this ISetupExtensionsBuilder setupBuilder,
    string name,
    Func<LogEventInfo, object> conditionMethod)
  {
    ReflectionHelpers.LateBoundMethod lateBoundMethod = conditionMethod != null ? (ReflectionHelpers.LateBoundMethod) ((target, args) => conditionMethod((LogEventInfo) args[0])) : throw new ArgumentNullException(nameof (conditionMethod));
    return setupBuilder.RegisterConditionMethod(name, conditionMethod.Method, lateBoundMethod);
  }

  public static ISetupExtensionsBuilder RegisterConditionMethod(
    this ISetupExtensionsBuilder setupBuilder,
    string name,
    Func<object> conditionMethod)
  {
    ReflectionHelpers.LateBoundMethod lateBoundMethod = conditionMethod != null ? (ReflectionHelpers.LateBoundMethod) ((target, args) => conditionMethod()) : throw new ArgumentNullException(nameof (conditionMethod));
    return setupBuilder.RegisterConditionMethod(name, conditionMethod.Method, lateBoundMethod);
  }
}
