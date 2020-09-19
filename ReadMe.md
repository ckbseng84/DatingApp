# Dating App
udemy [Build an app with ASPNET Core and Angular from scratch](https://www.udemy.com/course/build-an-app-with-aspnet-core-and-angular-from-scratch/) - Neil Cummings

## Installation
  clone from repo

## development tools
  1. Visual Studio Code
  2. Postman
  3. DB Browser for SQLite

## npm install(angular)
  navigate to dating-spa, then execute
  1. Angular Cli
     > `npm install -g @angular/cli`
     > `npm install --save-dev @angular/cli@latest`
  1. Angular jwt [https://github.com/auth0/angular2-jwt]
     > npm install @auth0/angular-jwt
  1. bootswatch - themes
      > npm install bootswatch
  1. ngx gallery
      > npm i @kolkov/ngx-gallery
  1. third party components
     1. alertify
     1. ngx boostrap
     
## build database
### SQLite
    1. cd to datingapp.api
    2. enter `dotnet ef database update`

## To run the application
    1. cd to datingapp.api and run `dotnet run` or `dotnet watch run`
    2. cd to datingapp-spa and enter `ng serve`
    (`cd ..` to navigate backward)
    3. open browser and enter `localhost:4200`

## Extensions(vscode)
    1. Angular Files
    2. Angular Languange Service
    1. Angular Snippets
    1. Angular2-switcher
    1. Auto Rename Tag
    1. Bracket Pair Colorizer
    2. c#
    2. c# Extensions
    1. Debugger for chrome
    1. Material Icon theme(optional)
    1. NugGet Package Manager(optional)
    1. Prettier - Code formatter(optional)
    1. TSLint

## Third Party tools
  1. [Cloudinary](https://cloudinary.com/) - cloud photos books

## UI Design references
  1. [Bootstrap 4](https://getbootstrap.com/)
  1. [Bootswatch](https://bootswatch.com/)
  1. [Font Awesome](https://fontawesome.com/v4.7.0/icons/)
## To be changed
  1. male and female icon by gender
  1. member list group by gender
## To be Study
### Angular
  1. ActivatedRoute, ActivationEnd
  1. @ViewChild
  1. Resolve
  1. http, .pipe, .map
  1. HttpInterceptor :star::star:
  1. service
  1. Router
  1. JwtHelperService
  1. app.module.ts
     > what is declaration, imports and providers?
  1. route.ts
     > what is the route.ts use for?
     > any other additional features?
  1. exception handling while database disconnected
  1. .subscribe
     > further study for more understanding
### Bootstrap

### Web Api(Asp.net core)
  1. ClaimTypes
  1. CreatedAtRoute



