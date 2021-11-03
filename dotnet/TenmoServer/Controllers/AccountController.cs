using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.Models;
using TenmoServer.DAO;
using System.Collections.Generic;


namespace TenmoServer.Controllers
{
    [Route("/")]
    [ApiController]
    [Authorize]
    public class AccountController : Controller
    {
        private static IAccountBalanceDao _accountDao;

        [HttpGet("balance")]
        public ActionResult<AccountBalance> GetBalance(int account_id)
        {
            AccountBalance balance = _accountDao.GetBalance(account_id);

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
