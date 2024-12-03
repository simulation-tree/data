using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Data.Generator
{
    [Generator(LanguageNames.CSharp)]
    public class EmbeddedAddressTableGenerator : IIncrementalGenerator
    {
        private static readonly SourceBuilder source = new();
        private static readonly SourceBuilder debug = new();
        private const string TypeName = "EmbeddedAddressTable";
        private const string Namespace = "Data";

        void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterSourceOutput(context.CompilationProvider, Generate);
        }

        private static void Generate(SourceProductionContext context, Compilation compilation)
        {
            context.AddSource($"{TypeName}.generated.cs", Generate(compilation));
        }

        public static string Generate(Compilation compilation)
        {
            source.Clear();
            source.AppendLine($"namespace {Namespace}");
            source.BeginGroup();
            {
                source.AppendLine($"public static partial class {TypeName}");
                source.BeginGroup();
                {
                    source.AppendLine($"static {TypeName}()");
                    source.BeginGroup();
                    {
                        foreach (MetadataReference assemblyReference in compilation.References)
                        {
                            if (compilation.GetAssemblyOrModuleSymbol(assemblyReference) is IAssemblySymbol assemblySymbol)
                            {
                                Stack<ISymbol> stack = new();
                                stack.Push(assemblySymbol.GlobalNamespace);
                                while (stack.Count > 0)
                                {
                                    ISymbol current = stack.Pop();
                                    if (current is INamespaceSymbol namespaceSymbol)
                                    {
                                        foreach (ISymbol member in namespaceSymbol.GetNamespaceMembers())
                                        {
                                            stack.Push(member);
                                        }

                                        foreach (ISymbol member in namespaceSymbol.GetTypeMembers())
                                        {
                                            stack.Push(member);
                                        }
                                    }
                                    else if (current is ITypeSymbol typeSymbol)
                                    {
                                        debug.AppendLine($"Type: {typeSymbol.ToDisplayString()}");
                                        ImmutableArray<INamedTypeSymbol> interfaces = typeSymbol.AllInterfaces;
                                        foreach (INamedTypeSymbol interfaceSymbol in interfaces)
                                        {
                                            if (interfaceSymbol.ToDisplayString() == "Data.IEmbeddedResources")
                                            {
                                                AppendRegistration(typeSymbol);
                                            }
                                        }

                                        foreach (ISymbol member in typeSymbol.GetMembers())
                                        {
                                            stack.Push(member);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    source.EndGroup();
                }
                source.EndGroup();
            }
            source.EndGroup();
            return source.ToString();
        }

        private static void AppendRegistration(ITypeSymbol type)
        {
            source.AppendLine($"EmbeddedAddress.Register<{GetFullTypeName(type)}>();");
        }

        private static string GetFullTypeName(ITypeSymbol type)
        {
            return type.ToDisplayString();
        }
    }
}