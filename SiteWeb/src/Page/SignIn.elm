module Page.SignIn exposing (Model, Msg, init, session, update, view)

import Browser exposing (Document)
import Browser.Navigation as Nav
import Html exposing (Html)
import Html.Attributes as Attr
import Html.Events as Events

import Session exposing (..)
import Route exposing (Route, SignUpFragment(..))


type alias Model = 
  { session : Session
  , status : Status
  }


type Status
  = Initial
  | InProgress
  | AuthenticationFailed
  | RequestFailed


type Msg
  = SignInAlice
  | SignInBob
  | SignInEve


init : Session -> ( Model, Cmd Msg )
init oldSession =
  (
    { session = oldSession
    , status = Initial 
    }
  , Cmd.none
  )


session : Model -> Session
session model = model.session


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
  case msg of
    SignInAlice ->
      ( { model | session =
          withUser model.session (
            Validated { sessionToken = "abcdef" }
          )
        }
      , Nav.pushUrl model.session.key "/myaccount"
      )

    SignInBob ->
      ( { model | session =
          withUser model.session (
            NotValidated { userId = "bob", password = "mdp" }
          )
        }
      , Nav.pushUrl model.session.key "/validation"
      )

    SignInEve ->
      ( { model | status = AuthenticationFailed }
      , Cmd.none
      )


view : Model -> Html Msg
view model =
  Html.main_ [ ]
    [ Html.p [ ] [ Html.text "Connexion" ]
    , Html.p [ ]
      [ Html.button [ Events.onClick SignInAlice ]
        [ Html.text "Connexion en tant qu'Alice"
        ]
      , Html.button [ Events.onClick SignInBob ]
        [ Html.text "Connexion en tant que Bob"
        ]
      , Html.button [ Events.onClick SignInEve ]
        [ Html.text "Connexion en tant qu'Ève"
        ]
      ]
    , Html.p [ ]
      [ Html.text (
          case model.status of
            AuthenticationFailed ->
              "L'identifiant et le mot de passe ne correspondent pas"
            RequestFailed ->
              "Impossible de vérifier les informations"
            _ -> ""
        )
      ]
    , Html.p [ ]
      [ Html.text (
          case model.status of
            InProgress ->
              "Chargement ..."
            _ -> ""
        )
      ]
    ]