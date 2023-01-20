using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Server.Model;

namespace Server.Utils
{
    /// <summary>
    /// Class that represent the captcha
    /// </summary>
	public class Captcha
	{
        static private HttpClient? _httpClient;
        static private String _secret = Environment.GetEnvironmentVariable("RECAPTCHA_SECRET") ?? "";

        private String _tok;

        /// <summary>
        /// Constructor of the captcha
        /// </summary>
        /// <param name="token"> the token of the captcha</param>
        public Captcha(String token)
        {
        	this._tok = token;
        	
        	if (Captcha._httpClient == null)
        	{
        		Captcha._httpClient = new HttpClient();
        		Captcha._httpClient.DefaultRequestHeaders.Accept.Add(
        			new MediaTypeWithQualityHeaderValue("application/json")
        		);
        	}
        }

        /// <summary>
        /// Method that verify the captcha
        /// </summary>
        /// <returns> the boolean if the captcha is valid</returns>
		public async Task<bool> IsValid()
		{
			var response = await Captcha._httpClient!.PostAsync(
				$"https://www.google.com/recaptcha/api/siteverify?secret={Captcha._secret}&response={this._tok}",
				null
			);

			CaptchaResponse captchaResponse = await response.Content.ReadFromJsonAsync<CaptchaResponse>()
				?? new CaptchaResponse { success = false };

			return captchaResponse.success;
		}
	}
}