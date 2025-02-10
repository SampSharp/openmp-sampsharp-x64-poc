using Microsoft.CodeAnalysis;

namespace SampSharp.Analyzer;

public static class Analyzers
{
    public static readonly DiagnosticDescriptor Sash0001MissingExtensionAttribute = new(
        "SASH0001", 
        "Missing 'ExtensionAttribute'",
        "Type '{0} 'is missing the 'ExtensionAttribute'", 
        DiagnosticCategories.Correctness, 
        DiagnosticSeverity.Error, 
        true, 
        "Type {0} must have the 'ExtensionAttribute'.");
    
    public static readonly DiagnosticDescriptor Sash0002GenericEventHandlerUnsupported = new(
        "SASH0002", 
        "Unsupported generic parameters in open.mp event handler",
        "Unsupported generic parameters in open.mp event handler type '{0}'", 
        DiagnosticCategories.Correctness, 
        DiagnosticSeverity.Error, 
        true, 
        "open.mp event handler type '{0}' must not contain generic parameters.");
    
    public static readonly DiagnosticDescriptor Sash0003ApiStructMustBeParital = new(
        "SASH0003", 
        "open.mp api struct must be partial",
        "open.mp api struct '{0}' must be marked as partial", 
        DiagnosticCategories.Correctness, 
        DiagnosticSeverity.Error, 
        true, 
        "open.mp api struct type '{0}' must be marked as partial.");

    public static readonly DiagnosticDescriptor Sash0004ApiStructMarshalRefReturnUnsupported = new(
        "SASH0004", 
        "'ref return' marshalling not supported for open.mp api structs",
        "open.mp api function '{0}' use of 'ref return' in combination with marshalling not supported", 
        DiagnosticCategories.Correctness, 
        DiagnosticSeverity.Error, 
        true, 
        "open.mp api function '{0}' returns a value by ref while also using a marshaller which is not supported.");

    public static class DiagnosticCategories
    {
        public const string Style = "Style";
        public const string Usage = "Usage";
        public const string Naming = "Naming";
        public const string Performance = "Performance";
        public const string Security = "Security";
        public const string Design = "Design";
        public const string Maintainability = "Maintainability";
        public const string Correctness = "Correctness";
        public const string Documentation = "Documentation";
        public const string Reliability = "Reliability";
    }
}
