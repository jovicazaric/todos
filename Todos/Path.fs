module Todos.Paths

module Assets =
    module CSS =
        let style = "/assets/css/style.css"

    module JS =
        module Lib = 
            let jQuery = "/assets/js/lib/jquery-3.3.1.min.js"
        let home = "/assets/js/home.js"
        let main = "/assets/js/main.js"

module Pages =
    let home = "/"
    let login = "/login"
    let logout = "/logout"
    let register = "/register"

