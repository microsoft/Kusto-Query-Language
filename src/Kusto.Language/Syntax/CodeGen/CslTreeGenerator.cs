// NOTE: The following comment line is mandatory for T4 to work correctly. Don't remove it (CslTreeGenerator.cs)
// <#+
#if !KUSTO_BUILD || DEBUG
#if KUSTO_BUILD
namespace Kusto.Language.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
#endif

    #region class CslTreeGeneratorKnownType
    public class CslTreeGeneratorKnownType
    {
        /// <summary>
        /// The name of the type.
        /// </summary>
        public string Name;

        /// <summary>
        /// The namespace in which the type "lives".
        /// </summary>
        public string Namespace;

        /// <summary>
        /// Whether the type is immutable (once created, no modifications).
        /// </summary>
        public bool Immutable;

        /// <summary>
        /// Whether the type is copy-by-value (no need to clone).
        /// </summary>
        public bool CopyByValue;
    }
    #endregion

    #region class GeneratedCslNodeClass
    /// <summary>
    /// Design-time description of a node in the CSL tree (something that derives Kusto.DataNode.Csl.CslNode).
    /// </summary>
    public class GeneratedCslNodeClass
    {
        /// <summary>
        /// The name of the generated class.
        /// </summary>
        public string Name;

        /// <summary>
        /// Docstring for the generated class (summary section).
        /// </summary>
        public string Doc;

        /// <summary>
        /// Docstring for the generated class (remarks section).
        /// </summary>
        public string Remarks;

        /// <summary>
        /// The name of the base class.
        /// </summary>
        public string Base;

        /// <summary>
        /// Is this class abstract?
        /// </summary>
        public bool Abstract;

        /// <summary>
        /// Is this class sealed?
        /// </summary>
        public bool Sealed;

        /// <summary>
        /// How to generate the constructor methods
        /// </summary>
        public CslTreeGeneratorConstructionOptions ConstructionOptions = CslTreeGeneratorConstructionOptions.Default;

        /// <summary>
        /// How to generate the Clone method
        /// </summary>
        public CslTreeGeneratorCloneOptions CloneOptions = CslTreeGeneratorCloneOptions.Default;

        /// <summary>
        /// The type's properties.
        /// </summary>
        public GeneratedProperty[] Properties;

        /// <summary>
        /// The kind of the syntax node.
        /// If not specified, then a property is added
        /// </summary>
        public string Kind;
    }
    #endregion

    #region enum CslTreeGeneratorConstructionOptions
    /// <summary>
    /// Various options that control how <see cref="CslTreeGenerator"/> will create the constructors.
    /// </summary>
    public enum CslTreeGeneratorConstructionOptions
    {
        /// <summary>
        /// Default: Create all constructors
        /// </summary>
        Default = 0,

        /// <summary>
        /// Do not create the constructor that takes all properties
        /// </summary>
        SuppressAllPropertiesConstructor,
    }
    #endregion

    #region enum CslTreeGeneratorCloneOptions
    /// <summary>
    /// Various options that control how <see cref="CslTreeGenerator"/> will create the Clone() method.
    /// </summary>
    public enum CslTreeGeneratorCloneOptions
    {
        /// <summary>
        /// Default: Clone() uses the constructor that takes all properties, in order.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Similar to Default, but adds a last argument to the constructor: Result.Tuple.Clone(),
        /// </summary>
        AppendExpressionResultTuple,

        /// <summary>
        /// Similar to Default, but adds a last argument to the constructor: Result.Clone(), SymbolicName
        /// </summary>
        AppendExpressionResultAndSymbolicName,

        /// <summary>
        /// The Clone() method is not generated -- the class will use customer cloning.
        /// </summary>
        Custom,
    }
    #endregion

    #region class GeneratedProperty
    /// <summary>
    /// Design-time representation of a property.
    /// </summary>
    public class GeneratedProperty
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        public string Name;

        /// <summary>
        /// The .NET type of the property.
        /// </summary>
        public string Type;

        /// <summary>
        /// The docstring of the property.
        /// </summary>
        public string Doc;

        /// <summary>
        /// If true, the setter will be made public.
        /// </summary>
        public bool PublicSetter;

        /// <summary>
        /// If true then the value is optional (can be null)
        /// </summary>
        public bool Optional;

        /// <summary>
        /// If true then the default value specified in the constructor.
        /// </summary>
        public string DefaultValue;

        /// <summary>
        /// True if the property is part of the syntax
        /// </summary>
        public bool IsSyntax = true;

        /// <summary>
        /// The completion kind
        /// </summary>
        public string Completion;
    }
    #endregion

    #region class CslTreeGenerator
    /// <summary>
    /// A code-generating class that is used to generate code for
    /// classes that derive Kusto.DataNode.Csl.CslNode.
    /// </summary>
    public class CslTreeGenerator
    {
        #region Private data
        private GeneratedCslNodeClass[] m_classes;
        private System.Collections.Generic.Dictionary<string, GeneratedCslNodeClass> m_nameToClass;
        private System.Collections.Generic.Dictionary<string, CslTreeGeneratorKnownType> m_knownTypes;

        private CodeGenerator m_writer;
        #endregion

        #region API
        public static string Generate(
            GeneratedCslNodeClass[] classes,
            CslTreeGeneratorKnownType[] knownTypes
            )
        {
            ValidateAndNormalizeClasses(classes);

            var eg = new CslTreeGenerator();

            eg.m_classes = classes;
            eg.m_nameToClass = new Dictionary<string, GeneratedCslNodeClass>();
            foreach (var @class in classes)
            {
                eg.m_nameToClass.Add(@class.Name, @class);
            }

            eg.m_knownTypes = new Dictionary<string, CslTreeGeneratorKnownType>();
            foreach (var knownType in knownTypes)
            {
                eg.m_knownTypes.Add(knownType.Name, knownType);
                if (!string.IsNullOrWhiteSpace(knownType.Namespace))
                {
                    eg.m_knownTypes.Add(knownType.Namespace + "." + knownType.Name, knownType);
                }
            }

            eg.m_writer = new CodeGenerator();

            eg.WriteFileStart();
            eg.m_writer.WriteNamespaceDeclaration("Kusto.Language.Syntax", eg.WriteClasses);
            eg.WriteFileEnd();

            var ret = eg.m_writer.GetText();
            return ret;
        }

        #region File scope
        private static void ThrowSyntaxErrorException(string message)
        {
            throw new Exception("SyntaxError: " + message);
        }

        private static void EnsureArgIsNotNull(object arg, string name)
        {
            if (arg == null)
            {
                ThrowSyntaxErrorException($"Argument '{name}' cannot be null.");
            }
        }

        private static void EnsureArgIsNotNullOrWhitespace(string arg, string name)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                if (arg == null)
                {
                    ThrowSyntaxErrorException($"Argument '{name}' cannot be null.");
                }
                else if (arg == "")
                {
                    ThrowSyntaxErrorException($"Argument '{name}' cannot be empty.");
                }
                else
                {
                    ThrowSyntaxErrorException($"Argument '{name}' cannot be whitespace.");
                }
            }
        }

        private static void ValidateAndNormalizeClasses(GeneratedCslNodeClass[] classes)
        {
            EnsureArgIsNotNull(classes, "classes");

            int i = 0;
            foreach (var c in classes)
            {
                EnsureArgIsNotNullOrWhitespace(c.Name, $"classes[{i}].Name");
                EnsureArgIsNotNullOrWhitespace(c.Base, $"classes[{c.Name}].Base");
                if (c.Properties == null)
                {
                    c.Properties = new GeneratedProperty[0];
                }
                i++;
            }
        }

        private void WriteFileStart()
        {
            m_writer.WriteHeader(".tt", "CslTreeGenerator.t4");

            var ns = new List<string>
            {
                "System",
                "System.Collections.Generic",
                "CompletionKind=Kusto.Language.Editor.CompletionKind",
                "CompletionHint=Kusto.Language.Editor.CompletionHint"
            };

            m_writer.WriteUsingBlock(ns, false, false);
        }

        private void WriteFileEnd()
        {
        }
        #endregion

        #region WriteClasses
        private void WriteClasses()
        {
            m_writer.WriteRegion("SyntaxNodes", WriteClassesImpl);

            m_writer.WriteRegion("Visitors", WriteVisitorsImpl);
        }
        #endregion

        #region WriteVisitors
        private void WriteVisitorsImpl()
        {
            m_writer.WriteRegion("SyntaxVisitor", WriteVisitorImpl);
            m_writer.WriteRegion("SyntaxVisitor<TResult>", WriteVisitorTImpl);
        }

        private void WriteVisitorImpl()
        {
            m_writer.WriteScope($"public partial class SyntaxVisitor", () =>
            {
                foreach (var c in m_classes)
                {
                    if (!c.Abstract)
                    {
                        m_writer.WriteLine($"public abstract void Visit{c.Name}({c.Name} node);");
                    }
                }
            });

            m_writer.WriteScope($"public partial class DefaultSyntaxVisitor : SyntaxVisitor", () =>
            {
                m_writer.WriteLine("protected abstract void DefaultVisit(SyntaxNode node);");
                m_writer.WriteEmptyLineIfNeeded();

                foreach (var c in m_classes)
                {
                    if (!c.Abstract)
                    {
                        m_writer.WriteScope($"public override void Visit{c.Name}({c.Name} node)", () =>
                        {
                            m_writer.WriteLine($"this.DefaultVisit(node);");
                        });
                    }
                }
            });
        }

        private void WriteVisitorTImpl()
        {
            m_writer.WriteScope($"public partial class SyntaxVisitor<TResult>", () =>
            {
                foreach (var c in m_classes)
                {
                    if (!c.Abstract)
                    {
                        m_writer.WriteLine($"public abstract TResult Visit{c.Name}({c.Name} node);");
                    }
                }
            });

            m_writer.WriteScope($"public partial class DefaultSyntaxVisitor<TResult> : SyntaxVisitor<TResult>", () =>
            {
                m_writer.WriteLine("protected abstract TResult DefaultVisit(SyntaxNode node);");
                m_writer.WriteEmptyLineIfNeeded();

                foreach (var c in m_classes)
                {
                    if (!c.Abstract)
                    {
                        m_writer.WriteScope($"public override TResult Visit{c.Name}({c.Name} node)", () =>
                        {
                            m_writer.WriteLine($"return this.DefaultVisit(node);");
                        });
                    }
                }
            });
        }
#endregion

#region WriteClassesImpl
        private void WriteClassesImpl()
        {
            bool first = true;
            foreach (var c in m_classes)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    m_writer.WriteLine();
                }

                m_writer.WriteRegion($"class {c.Name}", () =>
                {
                    m_writer.WriteEmptyLineIfNeeded();
                    m_writer.WriteDocString(c.Doc, c.Remarks);
                    m_writer.WriteScope($"public {GetAbstractOrSealed(c)}partial class {c.Name} : {c.Base}", () =>
                    {
                        // #region Properties
                        if (c.Properties != null /* && c.Properties.Length > 0 */)
                        {
                            if (!c.Abstract)
                            {
                                m_writer.WriteEmptyLineIfNeeded();
                                if (c.Kind != null)
                                {
                                    m_writer.WriteLine($"public override SyntaxKind Kind => SyntaxKind.{c.Kind};");
                                }
                                else
                                {
                                    m_writer.WriteLine("private readonly SyntaxKind kind;");
                                    m_writer.WriteLine("public override SyntaxKind Kind => this.kind;");
                                }
                            }

                            foreach (var property in c.Properties)
                            {
                                m_writer.WriteEmptyLineIfNeeded();
                                m_writer.WriteDocString(property.Doc/*, property.Remarks*/);
                                if (property.PublicSetter)
                                {
                                    m_writer.WriteLine($"public {property.Type} {property.Name} {{ get; set; }}");
                                }
                                else
                                {
                                    m_writer.WriteLine($"public {property.Type} {property.Name} {{ get; }}");
                                }
                            }
                        }

                        m_writer.WriteEmptyLineIfNeeded();
                        m_writer.WriteDocString($@"Constructs a new instance of <see cref=""{c.Name}""/>.");

                        var lineage = GetLineage(c);
                        var props = lineage.Select(cslNode => cslNode.Properties).ToList();
                        var flattenedProps = props.SelectMany(propsArray => propsArray.Select(propsArrayItem => propsArrayItem)).ToList();

                        var constructorParametersList = 
                            string.Join(", ", 
                                flattenedProps.Select(property => 
                                    $"{property.Type} {CodeGenerator.GetCamelCase(property.Name)}{(property.DefaultValue != null ? " = " + property.DefaultValue : "")}")
                            .Concat(new[] { "IReadOnlyList<Diagnostic> diagnostics = null" }));

                        if (c.Kind == null && !c.Abstract)
                        {
                            constructorParametersList = "SyntaxKind kind, " + constructorParametersList;
                        }

                        var flattenedBaseProps = props.Take(props.Count - 1).SelectMany(propsArray => propsArray.Select(propsArrayItem => propsArrayItem)).ToList();
                        var baseParametersList = string.Join(", ", flattenedBaseProps.Select(property => $"{CodeGenerator.GetCamelCase(property.Name)}")
                            .Concat(new[] { "diagnostics" }));

                        m_writer.WriteScope($"internal {c.Name}({constructorParametersList}) : base({baseParametersList})", () =>
                        {
                            if (!c.Abstract)
                            {
                                if (c.Kind == null)
                                {
                                    m_writer.WriteLine("this.kind = kind;");
                                }

                                foreach (var property in c.Properties)
                                {
                                    if (property.IsSyntax)
                                    {
                                        m_writer.WriteLine($"this.{property.Name} = Attach({CodeGenerator.GetCamelCase(property.Name)}{(property.Optional ? ", optional: true" : "")});");
                                    }
                                    else
                                    {
                                        m_writer.WriteLine($"this.{property.Name} = {CodeGenerator.GetCamelCase(property.Name)};");
                                    }
                                }

                                m_writer.WriteLine("this.Init();");
                            }
                        });

                        var syntaxProperties = c.Properties.Where(p => p.IsSyntax).ToArray();

                        if (!c.Abstract /*&& c.Properties.Length > 0*/)
                        {
                            // ChildCount
                            m_writer.WriteEmptyLineIfNeeded();
                            m_writer.WriteLine($"public override int ChildCount => {syntaxProperties.Length};");

                            // GetChild
                            m_writer.WriteEmptyLineIfNeeded();
                            m_writer.WriteScope("public override SyntaxElement GetChild(int index)", () =>
                            {
                                m_writer.WriteScope("switch (index)", () =>
                                {
                                    for (int i = 0; i < syntaxProperties.Length; i++)
                                    {
                                        m_writer.WriteLine($"case {i}: return {syntaxProperties[i].Name};");
                                    }

                                    m_writer.WriteLine("default: throw new ArgumentOutOfRangeException();");
                                });
                            });

                            // GetName
                            m_writer.WriteEmptyLineIfNeeded();
                            m_writer.WriteScope("public override string GetName(int index)", () =>
                            {
                                m_writer.WriteScope("switch (index)", () =>
                                {
                                    for (int i = 0; i < syntaxProperties.Length; i++)
                                    {
                                        m_writer.WriteLine($"case {i}: return nameof({syntaxProperties[i].Name});");
                                    }

                                    m_writer.WriteLine("default: throw new ArgumentOutOfRangeException();");
                                });
                            });

                            // IsOptional
                            if (syntaxProperties.Any(p => p.Optional))
                            {
                                m_writer.WriteEmptyLineIfNeeded();
                                m_writer.WriteScope("public override bool IsOptional(int index)", () =>
                                {
                                    m_writer.WriteScope("switch (index)", () =>
                                    {
                                        for (int i = 0; i < syntaxProperties.Length; i++)
                                        {
                                            if (syntaxProperties[i].Optional)
                                            {
                                                m_writer.WriteLine($"case {i}:");
                                            }
                                        }

                                        m_writer.WriteLine($"    return true;");

                                        m_writer.WriteLine($"default:");
                                        m_writer.WriteLine($"    return false;");
                                    });
                                });
                            }

                            // GetCompletionHint
                            if (syntaxProperties.Any(p => p.Completion != null))
                            {
                                m_writer.WriteEmptyLineIfNeeded();
                                m_writer.WriteScope("protected override CompletionHint GetCompletionHintCore(int index)", () =>
                                {
                                    m_writer.WriteScope("switch (index)", () =>
                                    {
                                        for (int i = 0; i < syntaxProperties.Length; i++)
                                        {
                                            if (syntaxProperties[i].Completion != null)
                                            {
                                                m_writer.WriteLine($"case {i}: return CompletionHint.{syntaxProperties[i].Completion};");
                                            }
                                        }

                                        m_writer.WriteLine($"default: return CompletionHint.Inherit;");
                                    });
                                });
                            }
                        }

                        // #region CslNode implementation
                        if (!c.Abstract)
                        {
                            m_writer.WriteEmptyLineIfNeeded();
                            m_writer.WriteScope($"public override void Accept(SyntaxVisitor visitor)", () =>
                            {
                                m_writer.WriteLine($"visitor.Visit{c.Name}(this);");
                            });

                            m_writer.WriteScope($"public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)", () =>
                            {
                                m_writer.WriteLine($"return visitor.Visit{c.Name}(this);");
                            });

#if false
                            m_writer.WriteScope($"public override TResult Accept<TContext, TResult>(SyntaxVisitor<TContext, TResult> visitor, TContext context)", () =>
                            {
                                m_writer.WriteLine($"return visitor.Visit{c.Name}(context, this);");
                            });
#endif

                            if (c.CloneOptions != CslTreeGeneratorCloneOptions.Custom && c.Properties != null /*&& c.Properties.Length > 0*/)
                            {
                                var args = "";
                                if (c.Kind == null)
                                {
                                    args = "this.Kind";
                                }

                                if (c.Properties.Length > 0)
                                {
                                    if (args.Length > 0)
                                        args = args + ", ";

                                    args = args + string.Join(", ", c.Properties.Select(p => p.IsSyntax ? $"({p.Type}){p.Name}?.Clone()" : p.Name));
                                }

                                if (args.Length > 0)
                                    args = args + ", ";

                                args = args + "this.SyntaxDiagnostics";

                                m_writer.WriteEmptyLineIfNeeded();
                                m_writer.WriteScope($"protected override SyntaxElement CloneCore()", () =>
                                {
                                    m_writer.WriteLine($"return new {c.Name}({args});");
                                });
                            }
                        }
                    });
                });
            }
        }

        private bool IsDerivedFromCslNode(string type)
        {
            return m_nameToClass.ContainsKey(type);
        }

        private GeneratedCslNodeClass GetBaseOrNull(GeneratedCslNodeClass c)
        {
            GeneratedCslNodeClass ret = null;
            m_nameToClass.TryGetValue(c.Base, out ret);
            return ret;
        }

        private List<GeneratedCslNodeClass> GetLineage(GeneratedCslNodeClass c)
        {
            var lineage = new List<GeneratedCslNodeClass>(); // First is the ultimate base, last is us
            var current = c;
            while (current != null)
            {
                lineage.Add(current);
                current = GetBaseOrNull(current);
            }
            lineage.Reverse();
            return lineage;            
        }

        private string GetAbstractOrSealed(GeneratedCslNodeClass c)
        {
            if (c.Abstract)
            {
                if (c.Sealed)
                {
                    throw new Exception($"{c.Name}: A class can't be both abstract and sealed.");
                }
                return "abstract "; // Note the trailing space
            }
            else if (c.Sealed)
            {
                return "sealed "; // Note the trailing space
            }
            return "";
        }

        private string GetPublicOrProtected(GeneratedCslNodeClass c)
        {
            if (c.Abstract)
            {
                return "protected";
            }
            else
            {
                return "public";
            }
        }

#endregion
    }
#endregion
#endregion

#if KUSTO_BUILD
}
#endif
#endif
// NOTE: The following comment line is mandatory for T4 to work correctly. Don't remove it
// #>