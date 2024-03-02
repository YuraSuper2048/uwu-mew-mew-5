using SimpleOpenAi;
using uwu_mew_mew_5;

OpenAi.ApiBase = Environment.GetEnvironmentVariable("OPENAI_API_BASE") ?? OpenAi.ApiBase;
await Bot.RunAsync();