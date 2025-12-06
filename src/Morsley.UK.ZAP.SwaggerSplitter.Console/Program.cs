Display.Title("Morsley.UK Swagger Splitter");

string? swaggerUrl = null;

Arguments.Process(args, ref swaggerUrl);
Arguments.Gather(ref swaggerUrl);
Arguments.Output(swaggerUrl);
Arguments.Verify(swaggerUrl);

var fullSwaggerJson = await Arguments.GetJson(swaggerUrl!);
var splitSwaggerJson = SwaggerSplitter.Split(fullSwaggerJson!);

Persistence.SaveAsFiles(splitSwaggerJson);

Display.Blank();
Display.Mute("Finished");

//Console.ReadKey(); // TEMP - For testing only