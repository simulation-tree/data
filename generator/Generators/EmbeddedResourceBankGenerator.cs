using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Types;

namespace Data.Generator
{
    [Generator(LanguageNames.CSharp)]
    public class EmbeddedResourceBankGenerator : IIncrementalGenerator
    {
        void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
        {
            IncrementalValuesProvider<ITypeSymbol?> types = context.SyntaxProvider.CreateSyntaxProvider(Predicate, Transform);
            context.RegisterSourceOutput(types.Collect(), Generate);
        }

        private void Generate(SourceProductionContext context, ImmutableArray<ITypeSymbol?> typesArray)
        {
            List<ITypeSymbol> types = new();
            foreach (ITypeSymbol? type in typesArray)
            {
                if (type is not null)
                {
                    types.Add(type);
                }
            }

            if (TryGenerate(types, out string typeName, out string sourceCode))
            {
                context.AddSource($"{typeName}.generated.cs", sourceCode);
            }
        }

        private static bool Predicate(SyntaxNode node, CancellationToken token)
        {
            return node.IsKind(SyntaxKind.StructDeclaration);
        }

        private static ITypeSymbol? Transform(GeneratorSyntaxContext context, CancellationToken token)
        {
            StructDeclarationSyntax node = (StructDeclarationSyntax)context.Node;
            SemanticModel semanticModel = context.SemanticModel;
            ITypeSymbol? type = semanticModel.GetDeclaredSymbol(node);
            if (type is null)
            {
                return null;
            }

            if (type is INamedTypeSymbol namedType)
            {
                if (namedType.IsGenericType)
                {
                    return null;
                }
            }

            if (type.IsRefLikeType)
            {
                return null;
            }

            if (type.DeclaredAccessibility != Accessibility.Public && type.DeclaredAccessibility != Accessibility.Internal)
            {
                return null;
            }

            if (type.IsUnmanaged())
            {
                if (type.HasInterface(Constants.DataInterface))
                {
                    return type;
                }
            }

            return null;
        }

        public static bool TryGenerate(IReadOnlyCollection<ITypeSymbol> types, out string typeName, out string sourceCode)
        {
            if (types.Count == 0)
            {
                typeName = string.Empty;
                sourceCode = string.Empty;
                return false;
            }

            string? assemblyName = null;
            foreach (ITypeSymbol type in types)
            {
                assemblyName = type.ContainingAssembly?.Name;
                break;
            }

            if (assemblyName is not null && assemblyName.EndsWith(".Core"))
            {
                assemblyName = assemblyName.Substring(0, assemblyName.Length - 5);
            }

            SourceBuilder source = new();
            source.AppendLine("using Unmanaged;");
            source.AppendLine("using Worlds;");
            source.AppendLine("using Data;");
            source.AppendLine("using Data.Functions;");
            source.AppendLine();

            if (assemblyName is not null)
            {
                source.Append("namespace ");
                source.AppendLine(assemblyName);
                source.BeginGroup();
            }

            typeName = Constants.DataBankTypeName.Replace("{0}", assemblyName ?? "");
            typeName = typeName.Replace(".", "");
            source.Append("public readonly struct ");
            source.Append(typeName);
            source.Append(" : ");
            source.Append(Constants.DataBankInterface);
            source.AppendLine();

            source.BeginGroup();
            {
                source.Append("readonly void ");
                source.Append(Constants.DataBankInterface);
                source.Append(".Load(Register function)");
                source.AppendLine();

                source.BeginGroup();
                {
                    foreach (ITypeSymbol type in types)
                    {
                        AppendRegister(source, type);
                    }
                }
                source.EndGroup();
            }
            source.EndGroup();

            if (assemblyName is not null)
            {
                source.EndGroup();
                typeName = $"{assemblyName}.{typeName}";
            }

            sourceCode = source.ToString();
            return true;
        }

        private static void AppendRegister(SourceBuilder source, ITypeSymbol type)
        {
            source.Append("function.Invoke<");
            source.Append(type.ToDisplayString());
            source.Append(">();");
            source.AppendLine();
        }
    }
}