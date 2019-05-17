'use strict';

angular.module('followingList', ['ngRoute'])
  .component('followingList', {
    templateUrl: 'following/following.html',
    controller: ['$http', '$rootScope', function TweetListController($http, $rootScope) {
      var self = this;

      const requestOptions = {
          headers: { 'X-session': $rootScope.x_session }
      };

      $http.get('http://localhost:8080/twitterapi/following/', requestOptions).then(function (response) {
        self.followings = response.data;
        });
        self.Follow = function Follow(name) {   //600611030 give me advice anc teach me
            const tmp = "followingname=" + encodeURIComponent(name);
            $http.post('http://localhost:8080/twitterapi/following/', tmp, requestOptions);
        }
        self.UnFollow = function UnFollow(name) {
            $http.defaults.headers.delete = { 'X-session': $rootScope.x_session };
            const tmp = "followingname=" + encodeURIComponent(name);
            $http.delete('http://localhost:8080/twitterapi/following/?' + tmp);
        }
    }]
});