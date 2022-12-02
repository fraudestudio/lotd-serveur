module Session exposing (Session, User(..), withUser)

import Browser.Navigation as Nav

import Route exposing (Route)

type alias Session =
  { key : Nav.Key
  , route : Route
  , user : User
  }


type User
  = Validated { sessionToken : String }
  | NotValidated { userId : String, password : String }
  | Visitor


withUser : Session -> User -> Session
withUser session user =
  { session | user = user }