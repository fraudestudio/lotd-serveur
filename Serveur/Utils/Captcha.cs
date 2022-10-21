using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Server.Model;

namespace Server.Utils
{
	public class Captcha
	{
        static private HttpClient? _httpClient;
        static private String _secret = Environment.GetEnvironmentVariable("RECAPTCHA_SECRET") ?? "";

        private String _tok;

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