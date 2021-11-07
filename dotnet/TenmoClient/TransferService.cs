using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Transactions;
using TenmoClient.Exceptions;
using TenmoClient.Models;

namespace TenmoClient
{
    public class TransferService
    {
        private readonly static string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();

        //login endpoints

        /* 
         * 
        I am SUPER unsure about this SendTeBucks and haven't started on the rest, 
        once the transferservice is built out we should just have to finish out the program.cs 
        and then we'll be done  ... I think?

         */
        public string SendTEBucks()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "transactions/send");
            IRestResponse<Transaction> response = client.Post<Transaction>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.StatusCode.ToString();
            }

            return null;
        }

            private void ProcessErrorResponse(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new NoResponseException("Error occurred - unable to reach server.", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                throw new NonSuccessException((int)response.StatusCode);
            }
        }
    }
}
