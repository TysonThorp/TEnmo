using RestSharp;
using RestSharp.Authenticators;
using System;
using TenmoClient.Exceptions;
using TenmoClient.Models;

using System.Transactions;

using System.Collections.Generic;

/* */

namespace TenmoClient
{
    public class AuthService
    {
        private readonly static string API_BASE_URL = "http://localhost:53041/";
        private readonly IRestClient client = new RestClient();

        //login endpoints
        public bool Register(LoginUser registerUser)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "login/register");
            request.AddJsonBody(registerUser);
            IRestResponse<ApiUser> response = client.Post<ApiUser>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");
                return false;
            }
            else if (!response.IsSuccessful)
            {
                if (!string.IsNullOrWhiteSpace(response.Data.Message))
                {
                    Console.WriteLine("An error message was received: " + response.Data.Message);
                }
                else
                {
                    Console.WriteLine("An error response was received from the server. The status code is " + (int)response.StatusCode);
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        public ApiUser Login(LoginUser loginUser)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "login");
            request.AddJsonBody(loginUser);
            IRestResponse<ApiUser> response = client.Post<ApiUser>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");
                return null;
            }
            else if (!response.IsSuccessful)
            {
                if (!string.IsNullOrWhiteSpace(response.Data.Message))
                {
                    Console.WriteLine("An error message was received: " + response.Data.Message);
                }
                else
                {
                    Console.WriteLine("An error response was received from the server. The status code is " + (int)response.StatusCode);
                }
                return null;
            }
            else
            {
                client.Authenticator = new JwtAuthenticator(response.Data.Token);
                return response.Data;
            }
        }

        public decimal? GetBalance()
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            RestRequest request = new RestRequest(API_BASE_URL + "balance");
            IRestResponse<decimal> response = client.Get<decimal>(request);
            
            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }

            return 0;
        }
     

        public Transaction GetTransactionById(int id)
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            RestRequest request = new RestRequest(API_BASE_URL + "transactions/{id}");
            request.AddUrlSegment("id", id);
            IRestResponse<Transaction> response = client.Get<Transaction>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }

            return null;
        }

        public string RequestTeBucks()
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            RestRequest request = new RestRequest(API_BASE_URL + "transactions/request");
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

