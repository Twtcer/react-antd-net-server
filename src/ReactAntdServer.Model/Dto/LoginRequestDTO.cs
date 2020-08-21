using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace ReactAntdServer.Model.Dto
{
    public class LoginRequestDTO
    {
        [Required]
        [JsonProperty("username")]
        public string Username { get; set; }


        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
