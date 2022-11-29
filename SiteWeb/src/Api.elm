module Api exposing (Credentials(..), signUpRequest, Error(..), Result)

import Http exposing (request, jsonBody, expectStringResponse)
import Json.Encode as Encode
import Base64.Encode as Base64


-- Requests

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


-- Expectations

type Error
  = Internal
  | Network
  | Message String


type alias Result a = Result.Result Error a


expectWhatever : (Result () -> msg) -> Http.Expect msg
expectWhatever toMsg =
  expectStringResponse toMsg <|
    \response ->
      case response of
        Http.BadUrl_ url ->
          Err Internal

        Http.Timeout_ ->
          Err Network

        Http.NetworkError_ ->
          Err Network

        Http.BadStatus_ metadata body ->
          case metadata.statusCode of
            400 -> Err (Message body)
            _ -> Err Internal

        Http.GoodStatus_ metadata body ->
          Ok ()


-- Sign up

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


signUpRequest : (Result () -> msg) -> SignUpPayload -> Cmd msg
signUpRequest response payload =
  noAuthRequest
    { method = "POST"
    , endpoint = "/account/signup"
    , body = jsonBody (encodeSignUpPayload payload)
    , expect = expectWhatever response
    }