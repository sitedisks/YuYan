(function () {
    'user strict';

    angular.module('yuyanApp').directive("compareTo", [function () {
        return {
            require: "ngModel",
            scope: {
                otherModelValue: "=compareTo"
            },
            link: function (scope, element, attributes, ngModel) {

                ngModel.$validators.compareTo = function (modelValue) {
                    return modelValue == scope.otherModelValue;
                };

                scope.$watch("otherModelValue", function () {
                    ngModel.$validate();
                });
            }
        };
    }])
      .directive('convertToNumber', function () {
          return {
              require: 'ngModel',
              link: function (scope, element, attrs, ngModel) {
                  ngModel.$parsers.push(function (val) {
                      return parseInt(val, 10);
                  });
                  ngModel.$formatters.push(function (val) {
                      return '' + val;
                  });
              }
          };
      });
    
})();