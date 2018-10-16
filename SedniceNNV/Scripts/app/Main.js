var app = angular.module("ViewForMVC",[]);

var MainController = function ($scope, $http) {

    var Clanovi = function (response) {
        $scope.clanovi = response.data;
    };

    var onError = function (reason) {
        $scope.error = "greska";
    };

    
    $http.get("/api/ClanoviApi").then(Clanovi, onError)
};
app.controller('MainController',MainController);