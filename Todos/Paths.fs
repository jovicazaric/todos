module Todos.Paths

module Assets =
    module CSS =
        [<Literal>]
        let Style = "/assets/css/style.css"

        [<Literal>]
        let Bootstrap = "/assets/css/bootstrap.min.css"


    module JS =
        module Lib = 

            [<Literal>]
            let JQuery = "/assets/js/lib/jquery-3.3.1.min.js"

        [<Literal>]
        let Main = "/assets/js/main.js"

module Pages =

    [<Literal>]
    let Home = "/"

    [<Literal>]
    let Login = "/login"

    [<Literal>]
    let Logout = "/logout"
    
    [<Literal>]
    let Registration = "/registration"

