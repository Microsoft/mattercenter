﻿(function () {
    'use strict';

    angular.module("matterMain")
        .controller('navigationController', ['$state', '$stateParams', 'api', '$rootScope',
        function ($state, $stateParams, api, $rootScope) {

            var vm = this;
            $rootScope.pageIndex = "0";
            vm.SPOHomePage = configs.uri.SPOsiteURL + '/SitePages/MatterCenterHome.aspx';
            vm.menuClick = function () {
                var oAppMenuFlyout = $(".AppMenuFlyout");
                if (!(oAppMenuFlyout.is(":visible"))) {
                    //// Display the close icon and close the fly out
                    $(".OpenSwitcher").addClass("hide");
                    $(".CloseSwitcher").removeClass("hide");
                    $(".MenuCaption").addClass("hideMenuCaption");
                    oAppMenuFlyout.slideDown();
                } else {
                    oAppMenuFlyout.slideUp();
                    $(".CloseSwitcher").addClass("hide");
                    $(".OpenSwitcher").removeClass("hide");
                    $(".MenuCaption").removeClass("hideMenuCaption");
                }
            }

            //#region Global Variables
            vm.welcomeheader = false;
            //#endregion

        }]);
})();
