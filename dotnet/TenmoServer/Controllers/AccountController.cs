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
        private readonly ITransferDao _transferDao;
        public AccountController(IAccountBalanceDao accountDao, ITransferDao transferDao)
        {
            _transferDao = transferDao;
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



        [HttpPost("transfer/{account_to}/{amount}")]

        public ActionResult<AccountBalance> Transfer(int account_to, decimal amount)
        {
            int userId = Convert.ToInt32(User.FindFirst("sub")?.Value);
            int accountTo = Convert.ToInt32(account_to);
            decimal amountTwo = Convert.ToDecimal(amount);

           string balance = _transferDao.Transfer(userId, accountTo, amountTwo);
            
            if (balance != null)
            {
                return Ok(balance);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("transactions")]
        public ActionResult<List<Transaction>> GetTransactions()
        {

            int user_id = Convert.ToInt32(User.FindFirst("sub")?.Value);  //this gets the id from the token (exists already since the user logged in)
            

            List<Transaction> transactions = _transferDao.GetAllTransactions(user_id);

            if (transactions != null)
            {
                return Ok(transactions);
            }
            else
            {
                return NotFound();
            }



        }

        [HttpGet("transactions/{transfer_id}")]

        public Transaction GetTransactionById(int transfer_id)
    {
        Transaction transaction = _transferDao.GetTransactionById(transfer_id);

        if (transaction != null)
        {
            return transaction;
        }
        else
        {
            return null;
        }
    }
}
}
