module Api exposing (Credentials(..), sendSignUpRequest)

import Http exposing (request, jsonBody, expectWhatever, Error)
import Json.Encode as Encode
import Base64.Encode as Base64

type Credentials
  = Basic
    { userId : String
    , password : String
    }
  | Bearer String


credentialsToString : Credentials -> String
credentialsToString creds = 
  case creds of 
    Basic basic ->
      "Basic " ++ Base64.encode (
        Base64.string (
          basic.userId ++ ":" ++ basic.password
        )
      )

    Bearer token ->
      "Bearer " ++ token


authRequest :
  { method : String
  , credentials : Credentials
  , endpoint : String
  , body : Http.Body
  , expect : Http.Expect msg
  }
  -> Cmd msg
authRequest req = 
  request
    { method = req.method
    , headers =
      [ Http.header "Authorization"
        (credentialsToString req.credentials)
      ]
    , url = "/api" ++ req.endpoint 
    , body = req.body
    , expect = req.expect
    , timeout = Nothing
    , tracker = Nothing
    }


noAuthRequest :
  { method : String
  , endpoint : String
  , body : Http.Body
  , expect : Http.Expect msg
  }
  -> Cmd msg
noAuthRequest req = 
  request
    { method = req.method
    , headers = [ ]
    , url = "https://info-dij-sae001.iut21.u-bourgogne.fr/api" ++ req.endpoint 
    , body = req.body
    , expect = req.expect
    , timeout = Nothing
    , tracker = Nothing
    }


-- SIGN UP

type alias SignUpPayload =
  { username : String
  , email : String
  , captcha : String
  }


encodeSignUpPayload : SignUpPayload -> Encode.Value
encodeSignUpPayload payload =
  Encode.object
    [ ( "Username", Encode.string payload.username )
    , ( "Email ", Encode.string payload.email )
    , ( "Captcha", Encode.string payload.captcha )
    ]


sendSignUpRequest : (Result Error () -> msg) -> SignUpPayload -> Cmd msg
sendSignUpRequest response payload =
  noAuthRequest
    { method = "POST"
    , endpoint = "/account/signup"
    , body = jsonBody (encodeSignUpPayload payload)
    , expect = expectWhatever response
    }