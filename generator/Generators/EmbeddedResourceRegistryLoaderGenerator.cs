using Microsoft.CodeAnalysis;
using Types;

namespace Data.Generator
{
    [Generator(LanguageNames.CSharp)]
    public class EmbeddedResourceRegistryLoaderGenerator : IIncrementalGenerator
    {
        private const string TypeName = "EmbeddedResourceRegistryLoader";

        void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterSourceOutput(context.CompilationProvider, Generate);
        }

        private static void Generate(SourceProductionContext context, Compilation compilation)
        {
            if (compilation.GetEntryPoint(context.CancellationToken) is not null)
            {
                context.AddSource($"{TypeName}.generated.cs", Generate(compilation));
            }
        }

        public static string Generate(Compilation compilation)
        {
            string? assemblyName = compilation.AssemblyName;
            SourceBuilder builder = new();
            builder.AppendLine("using Data;");
            builder.AppendLine();

            if (assemblyName is not null)
            {
                builder.Append("namespace ");
                builder.Append(assemblyName);
                builder.AppendLine();
                builder.BeginGroup();
            }

            builder.AppendLine($"internal static partial class {TypeName}");
            builder.BeginGroup();
            {
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

                        if (type.HasInterface("Data.IEmbeddedResourceBank"))
                        {
                            AppendRegistration(builder, type);
                        }
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

        private static void AppendRegistration(SourceBuilder builder, ITypeSymbol type)
        {
            builder.Append("EmbeddedResourceRegistry.Load<");
            builder.Append(GetFullTypeName(type));
            builder.Append(">();");
            builder.AppendLine();
        }

        private static string GetFullTypeName(ITypeSymbol type)
        {
            return type.ToDisplayString();
        }
    }
}