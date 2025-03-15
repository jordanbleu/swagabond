using Shouldly;
using Swagabond.Core.ObjectModel;
using Swagabond.IntegrationTests.Common;

namespace Swagabond.IntegrationTests.Swagger3_0Tests;

public class BasicTest
{
    
    
    [Fact]
    public async Task Swagger3_0_BasicTest()
    {
        var mapper = new TestMapperService();
        var api = await mapper.MapFromSwaggerToApi(new(), "SwaggerFiles/swagger-sample.json");

        api.Type.ShouldBe(ApiSpecType.OpenApi);
        api.SpecVersion.ShouldBe("OpenApi3_0");
        
        api.Info.ShouldNotBeNull();
        api.Info.Title.ShouldBe("TestWebApp");
        api.Info.Description.ShouldBe("This is a sample swagger file to test Swagabond");

        api.Info.HasContactInfo.ShouldBeTrue();
        api.Info.ContactName.ShouldBe("Hugh Man");
        api.Info.ContactEmail.ShouldBe("test@gmail.com");
        api.Info.ContactUrl.ShouldBe("https://www.google.com/");
        
        api.Info.HasLicenseInfo.ShouldBeTrue();
        api.Info.LicenseName.ShouldBe("MIT");
        api.Info.LicenseUrl.ShouldBe("https://opensource.org/licenses/MIT");

        api.Info.TermsOfServiceUrl.ShouldBe("https://en.wikipedia.org/wiki/Lorem_ipsum");
        
        // simple scenarios 
        var simplePath = api.Paths.FirstOrDefault(p=>p.Route == "/Test/simple");
        simplePath.ShouldNotBeNull();
        simplePath.Name.ShouldBe("_Test_simple");
        
        var simpleGet = simplePath.Operations.FirstOrDefault(o => o.Method.Equals("Get"));
        TestSimpleGetOperation(simpleGet);
        
        var simplePost = simplePath.Operations.FirstOrDefault(o => o.Method.Equals("Post"));
        TestSimplePostOperation(simplePost);

        // more complex scenarios 
        var complexPath = api.Paths.FirstOrDefault(p => p.Route == "/Test/complex");

        complexPath.ShouldNotBeNull();
        complexPath.Name.ShouldBe("_Test_complex");

        var getOperation = complexPath.Operations.FirstOrDefault(o => o.Method.Equals("Get"));
        TestComplexGetOperation(getOperation);
        
        var postOperation = complexPath.Operations.FirstOrDefault(o => o.Method.Equals("Post"));
        TestComplexPostOperation(postOperation);
        
    }

    private static void TestSimpleGetOperation(ApiOperation operation)
    {
        operation.ShouldNotBeNull();
        operation.QueryParameters.Count().ShouldBe(2);
        operation.PathParameters.ShouldBeEmpty();
        operation.HeaderParameters.ShouldBeEmpty();
        
        foreach (var param in operation.QueryParameters)
        {
            param.Name.ShouldNotBeNullOrEmpty();
            param.Type.ShouldNotBe(ApiDataType.Object);
        }
        
        operation.Responses.ShouldHaveSingleItem();

        // This one doesn't have a request body
        operation.RequestBody.IsEmpty.ShouldBeTrue();
        
        var response = operation.Responses.First();
        
        response.ParsedStatusCode.ShouldBe(200);
        response.StatusCode.ShouldBe("200");
        response.Description.ShouldBe("Success");

        var schema = response.Schema;
        schema.ShouldNotBeNull();
        schema.IsPrimitive.ShouldBe(true);
        schema.IsDeprecated.ShouldBe(false);
        schema.IsEnum.ShouldBe(false);
        schema.IsArray.ShouldBe(false);
        schema.EnumNames.ShouldBeEmpty();
        schema.EnumOptions.ShouldBeEmpty();
        schema.EnumValues.ShouldBeEmpty();
        
        // should not have properties since this is a simple return value
        schema.Properties.ShouldBeEmpty();
        schema.SchemaId.ShouldBeEmpty();
        schema.Type.ShouldBe(ApiDataType.String);
        schema.Format.ShouldBeEmpty();
    }

    private static void TestSimplePostOperation(ApiOperation operation)
    {
        operation.ShouldNotBeNull();
        operation.QueryParameters.ShouldBeEmpty();
        operation.PathParameters.ShouldBeEmpty();
        operation.HeaderParameters.ShouldBeEmpty();
        
        operation.RequestBody.IsEmpty.ShouldBeFalse();
        var requestBody = operation.RequestBody;
        requestBody.IsEmpty.ShouldBeFalse();
        
        requestBody.Schema.EnumNames.ShouldBeEmpty();
        requestBody.Schema.EnumOptions.ShouldBeEmpty();
        requestBody.Schema.EnumValues.ShouldBeEmpty();
        requestBody.Schema.IsPrimitive.ShouldBeTrue();
        
        operation.Responses.ShouldHaveSingleItem();
        var response = operation.Responses.First();
        response.IsEmpty.ShouldBeFalse();
        response.ParsedStatusCode.ShouldBe(200);
        response.StatusCode.ShouldBe("200");
        response.Schema.EnumNames.ShouldBeEmpty();
        response.Schema.EnumOptions.ShouldBeEmpty();
        response.Schema.EnumValues.ShouldBeEmpty();
        response.Schema.Type.ShouldBe(ApiDataType.String);
        response.Schema.Format.ShouldBeEmpty();

    }

    private static void TestComplexGetOperation(ApiOperation operation)
    {
        operation.ShouldNotBeNull();
                
        operation.QueryParameters.Count().ShouldBe(25);
        operation.PathParameters.ShouldBeEmpty();
        operation.HeaderParameters.ShouldBeEmpty();

        foreach (var queryParam in operation.QueryParameters)
        {
            queryParam.Type.ShouldNotBe(ApiDataType.Object);
            queryParam.Name.ShouldNotBeNullOrEmpty();
            
            if (queryParam.IsEnum)
            {
                queryParam.EnumValues.ShouldNotBeEmpty();
                queryParam.EnumOptions.ShouldNotBeEmpty();
                queryParam.EnumNames.ShouldNotBeEmpty();
            }
        }


        operation.Responses.ShouldHaveSingleItem(); 
        var response = operation.Responses.First();
        response.Description.ShouldBe("Success");
        response.StatusCode.ShouldBe("200");
        response.ParsedStatusCode.ShouldBe(200);
        response.Schema.IsPrimitive.ShouldBeFalse();
        response.Schema.Properties.Count.ShouldBe(24);
        response.Schema.IsEnum.ShouldBeFalse();
        response.Schema.EnumNames.ShouldBeEmpty();
        response.Schema.EnumValues.ShouldBeEmpty();
        response.Schema.EnumOptions.ShouldBeEmpty();
        response.Schema.SchemaId.ShouldBe("TestWebApp.Controllers.ComplexResponseObject");
        response.Schema.Description.ShouldBe("Complex Response Object");

        foreach (var prop in response.Schema.Properties)
        {
            prop.Name.ShouldNotBeNullOrEmpty();
            
            if (prop.IsEnum)
            {
                prop.EnumValues.ShouldNotBeEmpty();
                prop.EnumOptions.ShouldNotBeEmpty();
                prop.EnumNames.ShouldNotBeEmpty();
            }
            
            if (prop.IsPrimitive)
            {
                prop.Properties.ShouldBeEmpty();
                prop.Type.ShouldNotBe(ApiDataType.Object);
                prop.SchemaId.ShouldBeNullOrEmpty();
            }
            else
            {
                prop.Type.ShouldBe(ApiDataType.Object);
                prop.SchemaId.ShouldNotBeNullOrEmpty();

                prop.Properties.ShouldNotBeEmpty();
                

            }

        }
    }

    private static void TestComplexPostOperation(ApiOperation operation)
    {
        operation.ShouldNotBeNull();

        operation.QueryParameters.ShouldBeEmpty();
        operation.PathParameters.ShouldBeEmpty();
        operation.HeaderParameters.ShouldBeEmpty();

        operation.RequestBody.IsEmpty.ShouldBeFalse();
        
        var requestBody = operation.RequestBody;
        requestBody.IsEmpty.ShouldBeFalse();
        
        

        operation.Responses.ShouldHaveSingleItem(); 
        var response = operation.Responses.First();
        response.Description.ShouldBe("Success");
        response.StatusCode.ShouldBe("200");
        response.ParsedStatusCode.ShouldBe(200);
        response.Schema.IsPrimitive.ShouldBeFalse();
        response.Schema.Properties.Count.ShouldBe(24);
        response.Schema.IsEnum.ShouldBeFalse();
        response.Schema.EnumNames.ShouldBeEmpty();
        response.Schema.EnumValues.ShouldBeEmpty();
        response.Schema.EnumOptions.ShouldBeEmpty();
        response.Schema.SchemaId.ShouldBe("TestWebApp.Controllers.ComplexResponseObject");

        response.Schema.Description.ShouldBe("Complex Response Object");

        foreach (var prop in response.Schema.Properties)
        {
            prop.Name.ShouldNotBeNullOrEmpty();
            
            if (prop.IsEnum)
            {
                prop.EnumValues.ShouldNotBeEmpty();
                prop.EnumOptions.ShouldNotBeEmpty();
                prop.EnumNames.ShouldNotBeEmpty();
            }
            
            if (prop.IsPrimitive)
            {
                prop.Properties.ShouldBeEmpty();
                prop.Type.ShouldNotBe(ApiDataType.Object);
                prop.SchemaId.ShouldBeNullOrEmpty();
            }
            else
            {
                prop.Type.ShouldBe(ApiDataType.Object);
                prop.SchemaId.ShouldNotBeNullOrEmpty();

                prop.Properties.ShouldNotBeEmpty();
                

            }

        }
    }

}