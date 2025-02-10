using Microsoft.CodeAnalysis;

namespace SampSharp.Analyzer;

public static class Analyzers
{
    public static readonly DiagnosticDescriptor MissingExtensionAttribute = new(
        "SASH0001", 
        "Missing 'ExtensionAttribute'",
        "Type {0} is missing the 'ExtensionAttribute'", 
        DiagnosticCategories.Correctness, 
        DiagnosticSeverity.Error, 
        true, 
        "Type {0} must have the 'ExtensionAttribute'.");
    
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
