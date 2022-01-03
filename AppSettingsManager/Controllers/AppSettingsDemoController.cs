using AppSettingsManager.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppSettingsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingsDemoController : ControllerBase
    {
        private readonly MySettings _mySettings;
        private readonly IConfiguration _configuration;
        public AppSettingsDemoController(MySettings mySettings, IConfiguration configuration)
        {
            _mySettings = mySettings;
            _configuration = configuration;
        }

        [HttpGet("GetAccountByExtension")]
        public ActionResult<string> GetAccountByExtension()
        {
            return _mySettings.Account;
        }

        [HttpGet("GetPasswordNotByExtension")]
        public ActionResult<string> GetPasswordNotByExtension()
        {
            var myPassword = _configuration.GetValue<string>("MySettings:Password");

            return myPassword;
        }
    }
}
