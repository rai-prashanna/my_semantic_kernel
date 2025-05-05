
using Microsoft.SemanticKernel;
using SKProject;

Console.WriteLine("Hello, World!");

var (apiKey, deploymentName, model, endpoint) = Settings.LoadFromFile();

// Kernel kernel = Kernel.CreateBuilder()
//                         .AddOpenAIChatCompletion("gpt-3.5-turbo", apiKey, orgId, serviceId: "gpt3")
//                         .AddOpenAIChatCompletion("gpt-4", apiKey, orgId, serviceId: "gpt4")
//                         .Build();

var kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName:deploymentName,
        endpoint: endpoint,
        apiKey: apiKey,
        serviceId: model
    )
    .Build();

string prompt = "Finish the following knock-knock joke. Knock, knock. Who's there? {{$input}}, {{$input}} who?";
KernelFunction jokeFunction = kernel.CreateFunctionFromPrompt(prompt);

var showManagerPlugin = kernel.ImportPluginFromObject(new ShowManager());

var theme = await kernel.InvokeAsync(showManagerPlugin["RandomTheme"]);
Console.WriteLine("I will tell a joke about " + theme);


var projectRoot = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..");
var pluginsDirectory = Path.Combine(projectRoot, "plugins", "jokes");
// Import the OrchestratorPlugin from the plugins directory.
var jokesPlugin = kernel.ImportPluginFromPromptDirectory(pluginsDirectory, "jokes"); 
var result = await kernel.InvokeAsync(jokesPlugin["knock_knock_joke"], new KernelArguments() {["input"] = theme.ToString()});

Console.WriteLine(result);



