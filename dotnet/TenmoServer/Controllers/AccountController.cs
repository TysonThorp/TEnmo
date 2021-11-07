using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.Models;
using TenmoServer.DAO;
using System.Collections.Generic;
using System;

namespace TenmoServer.Controllers
{
    [Route("/")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountBalanceDao _accountDao;
       
        public AccountController(IAccountBalanceDao accountDao)
        {
            _accountDao = accountDao;
        }


        [HttpGet("balance")]  //to get this you need to pass a token b/c of authorize
        public ActionResult<AccountBalance> GetBalance()
        {
            int user_id = Convert.ToInt32(User.FindFirst("sub")?.Value);  //this gets the id from the token (exists already since the user logged in)

            AccountBalance balance = _accountDao.GetBalance(user_id);

            if (balance != null)
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
