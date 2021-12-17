using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.Models;
using TenmoServer.DAO;
using System.Collections.Generic;
using System;
using TenmoServer.Security;

namespace TenmoServer.Controllers
{
    [Route("/")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountDao _accountDao;
       
        public AccountController(IAccountDao accountDao)
        {
            _accountDao = accountDao;
        }

        [HttpGet("balance")]  //to get this you need to pass a token b/c of authorize
        public ActionResult<decimal> GetBalance()
        {
            int userId = Convert.ToInt32(User.FindFirst("sub")?.Value);  //this gets the id from the token (exists already since the user logged in)

            decimal balance = _accountDao.GetAccount(userId).Balance;

            if (balance != 0)
            {
                return Ok(balance);
            }
            else
            {
                return NotFound();
            }
        }


    }
}
