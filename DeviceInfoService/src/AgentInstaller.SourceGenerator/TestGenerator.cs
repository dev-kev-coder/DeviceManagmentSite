using Microsoft.CodeAnalysis;

namespace SourceGenerator
{
    /**
     * To be a valid Incremental source generator, the class must inherit from `IIncrementalGenerator` and be decorated with the [Generator] attribute. 
     * The interface requires our generator to implement only the’ Initialize’ function.
     * **/
    [Generator]
    class TestGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            /**
             *  CompilationProvider -> Can access data relevant to the entire compilation (assemblies, all source files, various solution-wide options, and configs )
             *  SyntaxProvider -> Access to syntax trees to analyze, transform, and select nodes for future work (Most commonly accessed)
             *  ParseOptionProvider -> Gives access to various bits of info about the code being parsed, such as language, whether it’s regular code files, script files, custom preprocessor names, etc.
             *  AdditionalTextsProvider -> Additional texts are any non-source files you might want to access, such as a JSON file with various user-defined properties
             *  MetadataReferencesProvider -> Allows getting references to various things like assemblies without getting the whole assembly item directly
             *  AnalyzerConfigOptionsProvider -> If a source file has additional analyzer rules applied to it, this can access them
             * **/

            //context.SyntaxProvider.CreateSyntaxProvider();
        }
    }
}
