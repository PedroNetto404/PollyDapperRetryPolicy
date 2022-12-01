using Microsoft.AspNetCore.Mvc;
using PollyDapperRetryPolicy.Data;

namespace PollyDapperRetryPolicy.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    [HttpGet(Name = "GetProduct")]
    public async Task<IActionResult> Get([FromQuery]int productId)
    {
        return Ok(await new ProductRepository().GetById(productId)); 
    }
}