using System.Collections.Generic;
using Swashbuckle.Swagger;
using System.Web.Http.Description;

public class FileUploadOperation : IOperationFilter
{
    public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
    {
        if (apiDescription.ActionDescriptor.ActionName == "Import")
        {
            operation.parameters = operation.parameters ?? new List<Parameter>();

            // Added file upload parameter
            operation.parameters.Add(new Parameter
            {
                name = "file",
                @in = "formData",
                description = "Upload file",
                required = true,
                type = "file"
            });

            // Add consumes as 'multipart/form-data'
            operation.consumes.Add("multipart/form-data");
        }
    }
}
