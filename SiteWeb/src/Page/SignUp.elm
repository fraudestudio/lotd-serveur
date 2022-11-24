module Page.SignUp exposing (
  Model,
  Msg,
  init,
  session,
  subscriptions,
  update,
  view )

import Browser exposing (Document)
import Html exposing (Html)
import Html.Attributes as Attr
import Html.Lazy as Lazy
import Html.Events as Events

import Session exposing (..)
import Route exposing (Route, SignUpFragment(..))
import Component
import Port


type alias Model = 
  { session : Session
  , section : Section
  }


type Section
  = SignUpForm FormModel
  | Success


type alias FormModel =
  { username : String
  , email : String
  , captchaFilled : Bool
  , errorMessage : String
  }


type Msg
  = Username String
  | Email String
  | CaptchaFilled Bool
  | SignUpRequest


init : Session -> SignUpFragment -> ( Model, Cmd Msg )
init oldSession fragment =
  ( { session = oldSession
    , section = (
        case fragment of
          JustSignUp ->
            SignUpForm
              { email = ""
              , username = ""
              , captchaFilled = False
              , errorMessage = ""
              }

          SignUpSuccess ->
            Success 
      )
    }
  , if fragment == JustSignUp then
      Port.reloadCaptcha ()
    else
      Cmd.none
  )


session : Model -> Session
session model = model.session


subscriptions : Model -> Sub Msg
subscriptions _ =
  Port.captchaFilled CaptchaFilled


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
  case ( msg, model.section ) of
    ( Username username, SignUpForm form ) ->
      ( { model | section = SignUpForm { form | username = username } }
      , Cmd.none
      )

    ( Email email, SignUpForm form ) ->
      ( { model | section = SignUpForm { form | email = email } }
      , Cmd.none
      )

    ( CaptchaFilled filled, SignUpForm form ) ->
      ( { model | section = SignUpForm { form | captchaFilled = filled } }
      , Cmd.none
      )

    ( SignUpRequest, SignUpForm form ) ->
      if form.captchaFilled then
        ( { model | section = SignUpForm
            { form | errorMessage =
              "incription en cours..."
            }
          }
        , Cmd.none
        )
      else
        ( { model | section = SignUpForm
            { form | errorMessage =
              "Le CAPTCHA n'a pas été validé ou a expiré"
            }
          }
        , Cmd.none
        )

    _ ->
      ( model, Cmd.none )



view : Model -> Html Msg
view model =
  Html.main_ [ ]
    [ Html.div [ Attr.class "box" ]
      ( Html.h1 [ ] [ Html.text "Inscription" ]
      :: ( case model.section of
            SignUpForm form ->
              viewForm form

            Success ->
              [ Html.p [ ]
                [ Html.text "Inscription réussie !"
                ]
              ]
        )
      )
    ]


viewForm : FormModel -> List (Html Msg)
viewForm form =
  [ Html.p [ Attr.id "error" ] [ Html.text form.errorMessage ]
  , Html.form [ Attr.id "signin", Events.onSubmit SignUpRequest ]
    [ Lazy.lazy Component.formInput
        { id = "username"
        , type_ = "text"
        , label = "Nom d'utilisateur"
        , required = True
        , onInput = Username
        }
    , Lazy.lazy Component.formInput
        { id = "email"
        , type_ = "email"
        , label = "Addresse mail"
        , required = True
        , onInput = Email
        }
    , Html.div [ Attr.class "captcha" ]
      [ Html.div
        [ Attr.class "g-recaptcha"
        , ( Attr.attribute
            "data-sitekey"
            "6LfnKFwiAAAAAPd-9GjoxlDOI36qmaFu8o-Fkuy8" 
          )
        , ( Attr.attribute
            "data-callback"
            "captchaFilled"
          )
        , ( Attr.attribute
            "data-expired-callback"
            "captchaExpired"
          )
        ]
        [ ]
      ]
    , Html.div [ Attr.class "input" ]
      [ Html.input [ Attr.type_ "submit", Attr.value "S'inscrire" ] [ ]
      ]
    ]
  ]