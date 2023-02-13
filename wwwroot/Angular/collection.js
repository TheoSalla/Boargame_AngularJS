var app = angular.module('myApp', ['ngRoute']);

app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/home', {
            templateUrl: '/angular/home.html',
            controller: 'CollectionController'
        })
        .when('/about/:objectId', {
            templateUrl: '/angular/readMore.html',
            controller: 'GameController'
        })
        .otherwise({
            redirectTo: '/home',
        });

}]);

app.run(['$rootScope', function ($rootScope) {
    $rootScope.addedUsers = [];
    $rootScope.getGamesWithRightValues = [];
    $rootScope.allUsersCollection = [];
    $rootScope.playerCount = 2;
    $rootScope.timeCount = 90;
    $rootScope.getGamesWithRightValues = [];
    $rootScope.statuscode;
}])

app.controller('GameController', ['$scope', '$routeParams', '$http', function ($scope, $routeParams,$http) {
    objectId = $routeParams.objectId;
    $http.get("/api/game/" + objectId).then(function (response) {
        statuscode = response.status;
            $scope.gameInfo = response.data;
             console.log($scope.gameInfo);

        }, function (response) {
            errorMessage = response.data;
            statuscode = response.status;
        }
    )
}]);
app.controller('CollectionController', ['$scope', '$http', '$location', '$rootScope', function ($scope, $http, $location, $rootScope) {

    $scope.showLoading = "";
    finished = false;
    // Function to get users collection from api
    getCollection = async function (userName) {
        await $http.get("/api/collection/" + userName).then(function (response) {
            $rootScope.statuscode = response.status;
            $rootScope.allUsersCollection.push(response.data);

        }, function (response) {
            $scope.errorMessage = response.data;
            $rootScope.statuscode = response.status;
        }
        )
    };
    // Function to to get games with right parameters
        getRightGames = async function () {
        count = 0;
        $rootScope.getGamesWithRightValues = [];
        if ($rootScope.statuscode == 200)
        {
            for (var j = 0; j < $rootScope.allUsersCollection.length; j++) {
                 $rootScope.allUsersCollection[j].forEach(element => count++);
                for (var i = 0; i < count; i++) {
                    
                    if ($rootScope.allUsersCollection[j][i].maxPlayer < $rootScope.playerCount) {
                        continue;
                    }
                    if ($rootScope.allUsersCollection[j][i].minPlayer > $rootScope.playerCount) {
                        continue;
                    }
                    if ($rootScope.allUsersCollection[j][i].playingTime > $rootScope.timeCount) {
                        continue;
                    }
                    $rootScope.getGamesWithRightValues.push($rootScope.allUsersCollection[j][i]);
                }
                count = 0; 
            }
        }
    };
    // Function to add users and get their collection  
    $scope.addUser = async function (name) {
        $scope.account = "";
        $scope.showLoading = "LOADING..."
        if (!$rootScope.addedUsers.includes(name.toLowerCase())) {
            await getCollection(name);
            if ($scope.statuscode == 200) {
                await getRightGames();
                $rootScope.addedUsers.push(name.toLowerCase());
            }
            else {
                //$scope.$digest();
            }
        }
        else {
        }
        $scope.showLoading = "";
        finished = false;
        $scope.$digest();
    };
    // Function to randomize a game 
    $scope.getRandomGame = async function () {
        //await $scope.getRightGames();
            const random = Math.floor(Math.random() * $rootScope.getGamesWithRightValues.length)
            $scope.randomGame = $rootScope.getGamesWithRightValues[random];
            $scope.gameExist = "#gameModal";

        //$scope.$digest();
    };

     // Function for changing the players count
    $scope.changePlayers = async function (count) {
        $rootScope.playerCount = count;
        getRightGames();

    };
    // Function for changing the time count
    $scope.changeTime = async function (count) {
        $rootScope.timeCount = count;
        getRightGames();

    };

    // Functions for removing user
    $scope.removeUser = async function (removeUser) {
        for (var i = 0; i < $rootScope.addedUsers.length; i++) {
            if (removeUser == $rootScope.allUsersCollection[i][0].userName) {
                $rootScope.allUsersCollection = $rootScope.allUsersCollection.filter(item => item !== $rootScope.allUsersCollection[i]);
                break;
            }
        };
        $rootScope.addedUsers = $rootScope.addedUsers.filter(item => item !== removeUser)
        getRightGames();
    };

    // Function for modal when clicking to watch collection
    $scope.watchCollection = async function (user) {
        $scope.viewUserCollection = user;
        $scope.testCollection = [];
        for (var i = 0; i < $rootScope.allUsersCollection.length; i++) {
            if ($rootScope.allUsersCollection[i][0].userName == user) {
                $scope.testCollection.push($scope.allUsersCollection[i]);
                break;
            }
        }
        $scope.collectionExist = "#collectionModal";
    };

    // Function to sort users collection table
    $scope.Sort = function (col) {
        $scope.key = col;
        $scope.AscOrDesc = !$scope.AscOrDesc;
    };
    // Function to get more info about a game
    $scope.readMore = function (objectId) {
        $location.path("/about/" + objectId);
        console.log("/anuglar/Read more");
    }

}]);