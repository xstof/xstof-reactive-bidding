using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BiddingWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
      
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public record Config(string ApiUrl);

        public ConfigController(ILogger<ConfigController> logger,
                                IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
        }

        [HttpGet]
        public ActionResult<Config> Get()
        {
           var apiBaseUrl = _configuration.GetValue<string>("ApiBaseUrl");
           return new Config(ApiUrl: apiBaseUrl);
        }
    }
}
