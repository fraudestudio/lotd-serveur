module Page.NotFound exposing (Model, Msg, init, session, update, view)

import Browser exposing (Document)
import Html exposing (Html)
import Html.Attributes as Attr

import Session exposing (..)
import Route exposing (Route)


type alias Model = Session


type alias Msg = ()


init : Session -> ( Model, Cmd Msg )
init oldSession =
  ( oldSession, Cmd.none )


session : Model -> Session
session model = model


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
  ( model, Cmd.none )


view : Model -> Html Msg
view model =
  Html.main_ [ ]
    [ Html.p [ ] [ Html.text "Page introuvable" ]
    , Html.p [ ]
      [ Html.a [ Attr.href "/" ] [ Html.text "revenir à l'accueil" ]
      ]
    ]