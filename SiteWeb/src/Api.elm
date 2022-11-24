module Api exposing (RequestStatus(..))

import Http


type RequestStatus
  = InProgress
  | AuthenticationFailed
  | RequestFailed
  | Success


type Msg
  = SignInRequest
  | SignInResponse (Result Http.Error SignInResult)
  | ValidationRequest
  | ValidationResponse (Result Http.Error ValidationResult)


type SignInResult
    = Success String
    | NotValidated
    | Failure


sendSignInRequest : ModelSignIn -> Cmd Msg
sendSignInRequest model =
    requestWithAuth
        { method = "POST"
        , endpoint = "/account/signin"
        , expect = Http.expectJson SignInResponse authDecoder
        , body = Http.emptyBody
        , auth = Basic { userId = model.userId, password = model.password }
        }


authDecoder : Decode.Decoder AuthResult
authDecoder =
    Decode.field "Success" Decode.bool
        |> Decode.andThen (\success ->
            if success then authDecoderSuccess else Decode.succeed Failure)


authDecoderSuccess : Decode.Decoder AuthResult
authDecoderSuccess =
    Decode.field "Verified" Decode.bool
        |> Decode.andThen (\verified ->
            if verified then authDecoderValidated else Decode.succeed NotValidated)


authDecoderValidated : Decode.Decoder AuthResult
authDecoderValidated =
    Decode.map (\token -> Success token) (Decode.field "Token" Decode.string)


type alias ValidationResult = ()

sendValidationRequest : ModelValidation -> Cmd Msg
sendValidationRequest model =
    requestWithAuth
        { url = "http://localhost:5000/api/account/setpassword"
        , expect = Http.expectWhatever ValidationResponse
        , body = Http.jsonBody 
            ( Encode.object
                [ ( "NewPassword", Encode.string model.newPassword )
                ]
            )
        , auth = Basic { userId = model.userId, password = model.oldPassword }
        }


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


requestWithAuth :
  { method : String
  , credentials : Credentials
  , endpoint : String
  , body : Http.Body
  , expect : Http.Expect msg
  }
  -> Cmd msg
requestWithAuth request = 
  Http.request
    { method = request.method
    , headers =
      [ Http.header "Authorization"
        (credentialsToString method.credentials)
      ]
    , url = "/api" ++ request.endpoint 
    , body = request.body
    , expect = request.expect
    , timeout = Nothing
    , tracker = Nothing
    }