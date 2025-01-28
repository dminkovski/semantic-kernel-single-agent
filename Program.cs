#pragma warning disable SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0001 

// Copyright (c) Microsoft. All rights reserved.
using System.ComponentModel;
using Microsoft.SemanticKernel;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.Extensions.Configuration;

namespace PaymentAssistant
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Launching Payment Assistant...");

            Kernel kernel = buildKernel();

            String systemPrompt =
                """
                You are a home payment assistant, who allows users to pay their bill, by uploading a picture.
                To pay a bill you need the filename of the image of the bill. Then you can scan the image using the filename.
                Always check if a bill has already been paid before submitting a payment. All paid bills are part of the transaction history. 
                Check the transaction history yourself.
                Confirm the payment result.
                """;

            // Configure single agent "PaymentAgent" with Auto Functionchoice behavior
            ChatCompletionAgent agent = new()
            {
                Name = "PaymentAgent",
                Instructions = systemPrompt,
                Kernel = kernel,
                Arguments = new KernelArguments(
                    new AzureOpenAIPromptExecutionSettings() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() }
                 )
            };

            await runChatLoop(agent);
        }


        // Create Kernel based on appsettings values and add BankAssistant Functionality
        private static Kernel buildKernel()
        {
            IConfiguration config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();

            IKernelBuilder builder = Kernel.CreateBuilder();
            builder.AddAzureOpenAIChatCompletion(
                deploymentName: config["AzureOpenai:DeploymentName"],
                endpoint: config["AzureOpenai:Endpoint"],
                apiKey: config["AzureOpenai:ApiKey"]);

            builder.Plugins.AddFromType<PaymentAPIMocks>();

            return builder.Build();
        }

        public static async Task runChatLoop(ChatCompletionAgent agent)
        {
            Console.WriteLine("Welcome to your Payment Assistant!");
            ChatHistory history = [];

            // Run main console interaction loop
            bool isComplete = false;
            do
            {
                Console.WriteLine();
                Console.Write("> ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }
                if (input.Trim().Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                {
                    isComplete = true;
                    break;
                }

                history.Add(new ChatMessageContent(AuthorRole.User, input));

                Console.WriteLine("");

                await foreach (ChatMessageContent response in agent.InvokeAsync(history))
                {
                    Console.WriteLine($">> {response.Content}");
                    history.Add(new ChatMessageContent(AuthorRole.System, response.Content));

                }
            } while (!isComplete);
        }

    }

}