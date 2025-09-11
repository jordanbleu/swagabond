using Microsoft.AspNetCore.Mvc;

namespace SampleWebApi.Controllers;

[ApiController]
public class BlankController : Controller
{
    [HttpPost]
    [Route("api/v1/blank")]
    [ProducesResponseType(statusCode:200)]
    public async Task<ActionResult> PostAsync()
    {
        await Task.Delay(250);
        return Ok();
    }
}