using APIGate.Models.Poton;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotonWebhookController : ControllerBase
    {
        public PhotonWebhookController()
        { 
        }

        ~PhotonWebhookController()
        { 
        }

        #region Auth
        [HttpPost("auth")]
        public async Task<PhotonAuthResponse> Authenticate(PhotonAuthRequest authRequest)
        {
            var res = new PhotonAuthResponse()
            {
                ResultCode=3
            };

            //TODO: user found
            {
                res.ResultCode = 1;
                res.UserId = "123";
            }

            {
                res.ResultCode = 2;
                res.Message = "1234567";
            }

            return res;
        }
        #endregion
    }
}
