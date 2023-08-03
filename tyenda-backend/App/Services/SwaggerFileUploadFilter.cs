using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;

public class SwaggerFileUploadFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ParameterDescriptions.Any(p => p.ModelMetadata?.ContainerType == typeof(Microsoft.AspNetCore.Http.IFormFile)))
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    {
                        "multipart/form-data", new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    {
                                        "file", new OpenApiSchema
                                        {
                                            Type = "string",
                                            Format = "binary"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}