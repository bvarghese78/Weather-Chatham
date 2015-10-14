var app = angular.module('weatherAppChatham', ['ui.router', 'toastr']);

// Routing
app.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/home'); // Default state to load in the event that the user selects a site url that does not have a handled route (url)
    $stateProvider // you must load ALL states that will be viewable in this ui-view here. $stateProvider is a part of ui-router
        .state('home', {  // name of the state
            url: '/home',  // what url will show up at the top of the page in the url bar
            views: {
                'center': {
                    templateUrl: '/app/home.html',  // location of the html file for this state
                    controller: 'homeCtrl'  // controller for this state.
                }
            }
        })
        .state('forecastio', {
            url: '/forecastio',
            views: {
                'center': {
                    templateUrl: "/app/forecastio.html",
                    controller: "weatherCtrl"
                }
            }
        })
        .state('forecastio.location', {
            url: '/{locationName}',
            views: {
                'center@': {
                    templateUrl: "/app/forecastio.html",
                    controller: "forecastioCtrl"
                }
            }
        }
    );
});