﻿namespace AuthServer.API.DTOs
{
    public class ClientTokenDto
    {

        public string AccessToken { get; set; }

        public DateTime AccessTokenExpiration { get; set; }


    }
}
