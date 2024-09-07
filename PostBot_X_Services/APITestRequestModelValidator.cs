using FluentValidation;
using Models;
using Newtonsoft.Json.Linq;


namespace PostBot_X_Services
{
    public class APITestRequestModelValidator : AbstractValidator<APITestRequestModel>
    {
        public APITestRequestModelValidator()
        {
            // Rule for ApiType: Not null, not empty, and must be a valid HTTP method
            RuleFor(x => x.ApiType)
                .NotEmpty().WithMessage("ApiType cannot be null or empty.")
                .Must(IsValidHttpMethod).WithMessage("ApiType must be a valid HTTP method (GET, POST, PUT, PATCH, DELETE).");

            // Rule for Url: Not null or empty
            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Url cannot be null or empty.");

            // Rule for Payload: If not empty, all payloads should be valid JSON
            RuleFor(x => x.Payload)
                .Must(payloads => payloads == null || payloads.All(IsValidJson))
                .WithMessage("All payloads must be valid JSON.");

            // Rule for Headers: If not empty, each header's key and value must not be null or empty
            RuleForEach(x => x.Headers)
                .ChildRules(header =>
                {
                    header.RuleFor(h => h.Key)
                        .NotEmpty().WithMessage("Header key cannot be null or empty.");
                    header.RuleFor(h => h.Value)
                        .NotEmpty().WithMessage("Header value cannot be null or empty.");
                });

            // Rule for QueryParameters: If not empty, each query parameter's key and values must not be null or empty
            RuleForEach(x => x.QueryParameters)
                .ChildRules(param =>
                {
                    param.RuleFor(p => p.Key)
                        .NotEmpty().WithMessage("Query parameter key cannot be null or empty.");
                    param.RuleFor(p => p.Value)
                        .NotEmpty().WithMessage("Query parameter values cannot be null or empty.")
                        .Must(values => values.All(value => !string.IsNullOrEmpty(value)))
                        .WithMessage("Query parameter values cannot contain null or empty entries.");
                });
        }

        private bool IsValidHttpMethod(string method)
        {
            var validMethods = new[] { "GET", "POST", "PUT", "PATCH", "DELETE" };
            return validMethods.Contains(method.ToUpper());
        }

        private bool IsValidJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return false;

            try
            {
                JToken.Parse(json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
