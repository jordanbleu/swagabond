using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Moq;
using Moq.AutoMock;
using Shouldly;
using Swagabond.ObjectModelV1;
using Swagabond.ObjectModelV1.Transformer;

namespace Swagabond.Tests.ObjectModelV1;

public class InfoV1TransformerTests
{
    private readonly InfoV1Transformer _target = new();

    [Fact]
    public void FromOpenApi_NullInfo_ReturnsDefaults()
    {
        var result = _target.FromOpenApi(null);

        result.ContactName.ShouldBe(string.Empty);
        result.ContactEmail.ShouldBe(string.Empty);
        result.ContactUrl.ShouldBe(string.Empty);
        result.LicenseName.ShouldBe(string.Empty);
        result.LicenseUrl.ShouldBe(string.Empty);
        result.TermsOfServiceUrl.ShouldBe(string.Empty);
        result.HasContactInfo.ShouldBeFalse();
        result.HasLicenseInfo.ShouldBeFalse();
    }

    [Fact]
    public void FromOpenApi_FullInfo_MapsAllFields()
    {
        var info = new OpenApiInfo
        {
            Contact = new OpenApiContact
            {
                Name = "Support Team",
                Email = "support@example.com",
                Url = new Uri("https://example.com/support")
            },
            License = new OpenApiLicense
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            },
            TermsOfService = new Uri("https://example.com/tos")
        };

        var result = _target.FromOpenApi(info);

        result.ContactName.ShouldBe("Support Team");
        result.ContactEmail.ShouldBe("support@example.com");
        result.ContactUrl.ShouldBe("https://example.com/support");
        result.HasContactInfo.ShouldBeTrue();

        result.LicenseName.ShouldBe("MIT");
        result.LicenseUrl.ShouldBe("https://opensource.org/licenses/MIT");
        result.HasLicenseInfo.ShouldBeTrue();

        result.TermsOfServiceUrl.ShouldBe("https://example.com/tos");
    }

    [Fact]
    public void FromOpenApi_ContactOnly_HasContactInfoTrue_HasLicenseInfoFalse()
    {
        var info = new OpenApiInfo
        {
            Contact = new OpenApiContact { Email = "a@b.com" }
        };

        var result = _target.FromOpenApi(info);

        result.HasContactInfo.ShouldBeTrue();
        result.HasLicenseInfo.ShouldBeFalse();
    }
}

public class ExternalDocsV1TransformerTests
{
    private readonly ExternalDocsV1Transformer _target = new();

    [Fact]
    public void FromOpenApi_Null_ReturnsEmpty()
    {
        var result = _target.FromOpenApi(null);
        result.IsEmpty.ShouldBeTrue();
    }

    [Fact]
    public void FromOpenApi_ValidDocs_MapsTextAndUrl()
    {
        var docs = new OpenApiExternalDocs
        {
            Description = "Project Wiki",
            Url = new Uri("https://wiki.example.com")
        };

        var result = _target.FromOpenApi(docs);

        result.IsEmpty.ShouldBeFalse();
        result.Text.ShouldBe("Project Wiki");
        result.Url.ShouldBe("https://wiki.example.com/");
    }

    [Fact]
    public void FromOpenApi_NullDescription_DefaultsToEmpty()
    {
        var docs = new OpenApiExternalDocs
        {
            Url = new Uri("https://example.com")
        };

        var result = _target.FromOpenApi(docs);

        result.IsEmpty.ShouldBeFalse();
        result.Text.ShouldBe(string.Empty);
    }
}

public class ServerV1TransformerTests
{
    [Fact]
    public void FromOpenApi_MapsUrlAndDescription()
    {
        var autoMocker = new AutoMocker();

        autoMocker.GetMock<IExtensionV1Transformer>()
            .Setup(x => x.FromOpenApi(It.IsAny<IDictionary<string, IOpenApiExtension>>()))
            .Returns(new List<ExtensionV1>());

        var target = autoMocker.CreateInstance<ServerV1Transformer>();

        var server = new OpenApiServer
        {
            Url = "https://api.example.com/v1",
            Description = "Production"
        };

        var result = target.FromOpenApi(server);

        result.Url.ShouldBe("https://api.example.com/v1");
        result.Description.ShouldBe("Production");
        result.Extensions.ShouldBeEmpty();
    }
}

public class SchemaReferenceV1TransformerTests
{
    private readonly SchemaReferenceV1Transformer _target = new();

    [Fact]
    public void FromOpenApi_MapsNameAndSchema()
    {
        var schema = new SchemaDefinitionV1
        {
            Name = "Pet",
            DataType = DataTypeV1.Object
        };

        var result = _target.FromOpenApi("petId", schema, new ApiV1());

        result.IsEmpty.ShouldBeFalse();
        result.Name.ShouldBe("PetId");
        result.OriginalName.ShouldBe("petId");
        result.Schema.ShouldBeSameAs(schema);
    }

    [Fact]
    public void FromOpenApi_SanitizesName()
    {
        var schema = new SchemaDefinitionV1();

        var result = _target.FromOpenApi("some_snake_name", schema, new ApiV1());

        result.Name.ShouldBe("SomeSnakeName");
        result.OriginalName.ShouldBe("some_snake_name");
    }
}

public class ExtensionV1TransformerTests
{
    private readonly ExtensionV1Transformer _target = new();

    [Fact]
    public void FromOpenApi_EmptyDictionary_ReturnsEmptyList()
    {
        var result = _target.FromOpenApi(new Dictionary<string, IOpenApiExtension>());
        result.ShouldBeEmpty();
    }

    [Fact]
    public void FromOpenApi_MapsKeysAndValues()
    {
        var extensions = new Dictionary<string, IOpenApiExtension>
        {
            { "x-custom-flag", new OpenApiBoolean(true) },
            { "x-version", new OpenApiString("2.0") }
        };

        var result = _target.FromOpenApi(extensions);

        result.Count.ShouldBe(2);
        result.ShouldContain(e => e.Name == "x-custom-flag" && e.Value == "True");
        result.ShouldContain(e => e.Name == "x-version" && e.Value == "2.0");
    }
}
