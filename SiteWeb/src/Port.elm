port module Port exposing (..)

port captchaFilled : (Bool -> msg) -> Sub msg

port reloadCaptcha : () -> Cmd msg