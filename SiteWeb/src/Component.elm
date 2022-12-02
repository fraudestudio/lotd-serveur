module Component exposing (formInput)

import Html exposing (Html, div, label, text, input)
import Html.Attributes exposing (id, for, type_, required, class)
import Html.Events exposing (onInput)

formInput :
    { id : String
    , type_ : String
    , label : String
    , required : Bool
    , onInput : (String -> msg)
    }
    -> Html msg
formInput args =
    div [ class "input" ]
        [ label [ for args.id ] [ text args.label ]
        , input
            [ id args.id
            , type_ args.type_
            , required args.required
            , onInput args.onInput
            ]
            [ ]
        ]