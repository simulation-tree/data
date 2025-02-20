using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Unmanaged;

namespace Data.Generator
{
    [Generator(LanguageNames.CSharp)]
    public class EmbeddedResourceRegistryLoaderGenerator : IIncrementalGenerator
    {
        void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterSourceOutput(context.CompilationProvider, Generate);
        }

        private static void Generate(SourceProductionContext context, Compilation compilation)
        {
            if (compilation.GetEntryPoint(context.CancellationToken) is not null)
            {
                IReadOnlyCollection<ITypeSymbol> types = compilation.GetAllTypes(false);
                EmbeddedResourceBankGenerator.TryGenerate(types, out string typeName, out string _);
                context.AddSource($"{Constants.LoaderTypeName}.generated.cs", Generate(compilation, typeName));
            }
        }

        public static string Generate(Compilation compilation, string additionalDataBankName)
        {
            string? assemblyName = compilation.AssemblyName;
            SourceBuilder builder = new();
            builder.Append("using ");
            builder.Append(Constants.ProjectNameSpace);
            builder.Append(";");
            builder.AppendLine();
            builder.AppendLine();

            if (assemblyName is not null)
            {
                builder.Append("namespace ");
                builder.Append(assemblyName);
                builder.AppendLine();
                builder.BeginGroup();
            }

            builder.Append($"internal static partial class ");
            builder.Append(Constants.LoaderTypeName);
            builder.AppendLine();

            builder.BeginGroup();
            {
                HashSet<string> dataBankNames = new();
                if (!string.IsNullOrEmpty(additionalDataBankName))
                {
                    dataBankNames.Add(additionalDataBankName);
                }

                builder.AppendLine($"public static void Load()");
                builder.BeginGroup();
                {
                    foreach (ITypeSymbol type in compilation.GetAllTypes())
                    {
                        if (type.IsRefLikeType)
                        {
                            continue;
                        }

                        if (type.DeclaredAccessibility == Accessibility.Private || type.DeclaredAccessibility == Accessibility.ProtectedOrInternal)
                        {
                            continue;
                        }

                        if (type is INamedTypeSymbol namedType)
                        {
                            if (namedType.IsGenericType)
                            {
                                continue;
                            }
                        }

                        if (type.HasInterface(Constants.DataBankInterface))
                        {
                            dataBankNames.Add(GetFullTypeName(type));
                        }
                    }

                    foreach (string dataBankName in dataBankNames)
                    {
                        AppendRegistration(builder, dataBankName);
                    }
                }
                builder.EndGroup();
            }
            builder.EndGroup();

            if (assemblyName is not null)
            {
                builder.EndGroup();
            }

            return builder.ToString();
        }

        private static void AppendRegistration(SourceBuilder builder, string dataBankName)
        {
            builder.Append(Constants.RegistryName);
            builder.Append(".Load<");
            builder.Append(dataBankName);
            builder.Append(">();");
            builder.AppendLine();
        }

        private static string GetFullTypeName(ITypeSymbol type)
        {
            return type.ToDisplayString();
        }
    }
}