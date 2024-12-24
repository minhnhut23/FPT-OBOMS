using Newtonsoft.Json;

namespace AuthService.Utils;

public static class ErrorHandler
{
    public static bool IsJson(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        input = input.Trim();

        if ((input.StartsWith("{") && input.EndsWith("}")) ||
            (input.StartsWith("[") && input.EndsWith("]")))
        {
            try
            {
                JsonConvert.DeserializeObject(input);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        return false;
    }

    public static string ProcessErrorMessage(string exceptionMessage)
    {
        if (IsJson(exceptionMessage))
        {
            try
            {
                var errorObj = JsonConvert.DeserializeObject<dynamic>(exceptionMessage);
                return errorObj?.msg ?? "An unknown error occurred in JSON response.";
            }
            catch (JsonException)
            {
                return "Error processing the custom error message.";
            }
        }

        return exceptionMessage;
    }
}
