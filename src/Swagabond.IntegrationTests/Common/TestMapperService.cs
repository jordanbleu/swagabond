// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Logging.Abstractions;
// using Microsoft.OpenApi.Readers;
// using Swagabond.Core;
// using Swagabond.Core.Mappers;
// using Swagabond.Core.ObjectModel;
// using Swagabond.Core.Parsers;
// using Swagabond.ObjectModelV1;
//
// namespace Swagabond.IntegrationTests.Common;
//
// public class TestMapperService
// {
//     private ILogger _logger;
//
//     public Task<ApiV1> MapFromSwaggerToApi(MapperRequest mapperRequest, string swaggerFilePath)
//     {
//         var swaggerStream = File.OpenRead(swaggerFilePath);
//         
//         var logger = NullLogger<OpenApiMapper>.Instance;
//         var reader = new OpenApiStreamReader();
//         var parser = new MicrosoftSwaggerParser(reader);
//         
//         
//         
//         
//         var mapper = new OpenApiMapper(logger, parser);
//
//         return mapper.MapFromStreamV1(mapperRequest, swaggerStream);
//     }
// }