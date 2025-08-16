using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) =>
{
    if (context.Request.Query.ContainsKey("firstNumber") &&
        context.Request.Query.ContainsKey("secondNumber") &&
        context.Request.Query.ContainsKey("operation"))
    {
        string firstNumberString = context.Request.Query["firstNumber"];
        string secondNumberString = context.Request.Query["secondNumber"];
        string operation = context.Request.Query["operation"];

        if (int.TryParse(firstNumberString, out int firstNumber) &&
            int.TryParse(secondNumberString, out int secondNumber))
        {
            double result = 0;
            bool validOperation = true;

            switch (operation)
            {
                case "add":
                    result = firstNumber + secondNumber;
                    break;
                case "subtract":
                    result = firstNumber - secondNumber;
                    break;
                case "multiply":
                    result = firstNumber * secondNumber;
                    break;
                case "divide":
                    if (secondNumber != 0)
                    {
                        result = (double)firstNumber / secondNumber;
                    }
                    else
                    {
                        await context.Response.WriteAsync("Error: Division by zero is not allowed.");
                        return;
                    }
                    break;
                case "%":
                    if (secondNumber != 0)
                    {
                        result = firstNumber % secondNumber;
                    }
                    else
                    {
                        await context.Response.WriteAsync("Error: Modulo by zero is not allowed.");
                        return;
                    }
                    break;
                default:
                    validOperation = false;
                    await context.Response.WriteAsync("Error: Invalid operation specified. Use '+', '-', '*', '/', or '%'.");
                    return;
            }

            if (validOperation)
            {
                await context.Response.WriteAsync($"The result is: {result}");
            }
        }
        else
        {
            await context.Response.WriteAsync("Error: Both 'firstNumber' and 'secondNumber' must be valid integers.");
        }
    }
    else
    {
        await context.Response.WriteAsync("Error: Missing one or more required parameters. Please provide 'firstNumber', 'secondNumber', and 'operation'.");
    }
});

app.Run();