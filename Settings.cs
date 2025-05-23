namespace SKProject;

using System.Text.Json;

public static class Settings {    
    public static (string apiKey, string deploymentName,string model, string endpoint)
        LoadFromFile(string configFile = "config/settings.json")
    {
        if (!File.Exists(configFile))
        {
            Console.WriteLine("Configuration not found: " + configFile);
            throw new Exception("Configuration not found");
        }
        try
        {
            var config = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(configFile));

            // check whether config is null
            if (config == null)
            {
                Console.WriteLine("Configuration is null");
                throw new Exception("Configuration is null");
            }

            string apiKey = config["apiKey"];
            string deploymentName = config["deploymentName"];
            string model = config["model"];
            string endpoint = config["endpoint"];


            return (apiKey, deploymentName,model,endpoint);
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong: " + e.Message);
            return ("", "","", "");
        }
    }
}