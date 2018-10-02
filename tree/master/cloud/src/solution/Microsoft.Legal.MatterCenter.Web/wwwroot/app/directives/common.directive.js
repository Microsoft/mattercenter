(function () {

    'use strict';
    function onload($timeout) {
        return {
            restrict: 'AE',
            scope: { sortdetails: '@' },
            link: function (scope, element, attrs) {
                scope.$watch("sortdetails", function () {
                    $timeout(function () { $('[id^="asc"]').hide(); }, 1000);
                    $timeout(function () { $('[id^="desc"]').hide(); }, 1000);
                }, true);
            }
        }
    }

    'use strict';
    function showbreadcrumb($rootScope) {
        return {
            restrict: 'AE',
            link: function (scope, element, attrs) {
                $(element).find('ul li ul li').on("dblclick", function (e) {
                    $rootScope.breadcrumb = false;
                    $rootScope.foldercontent = true;
                    $rootScope.$apply();
                    var text = $(this).find('div').attr('title');
                    $('#breadcrumb #currentFolder').html(text);
                });

            }
        }
    }

    'use strict';
    function datefilter() {
        return {
            restrict: 'A',
            scope: { date: '@' },
            link: function (scope, element, attrs) {
                scope.$watch("date", function () {
                    var dDate = attrs.date;
                    var oDates = new Date(dDate), months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"], date = "", oLocalDate = dDate;
                    if (isNaN(oDates)) {
                        var arrSplitedDate = dDate.replace(/[-]/g, "/");
                        arrSplitedDate = arrSplitedDate.split("/");
                        dDate = arrSplitedDate[1] + "-" + arrSplitedDate[0] + "-" + arrSplitedDate[2];
                        oDates = new Date(dDate);
                    }
                    if (0 > oLocalDate.indexOf("Z")) {
                        date += months[parseInt(oDates.getMonth(), 10)] + " ";
                        date += oDates.getDate() + ", ";
                        date += oDates.getFullYear();
                    } else {
                        date += months[parseInt(oDates.getUTCMonth(), 10)] + " ";
                        date += oDates.getUTCDate() + ", ";
                        date += oDates.getUTCFullYear();
                    }
                    $(element).html(date);
                    $(element).attr("title", date);
                    return date;
                });
            }
        }
    }

    'use strict';
    function toggletab() {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                $(element).find('a').click(function (e) {
                    e.preventDefault()
                    $(this).tab('show')
                })
            }
        }
    }

    'use strict';
    function infopopover() {
        return {
            restrict: 'AE',
            link: function (scope, element, attrs) {
                var content = "<div class='cartelementpopup' style='width:250px'>\
                                   <div class='checkOutMetadata marginTop10'>When sending to your OneDrive the DMS version will be checked out to you until you check it back in.</div>\
                               </div>";
                $(element).popover({
                    html: true,
                    trigger: 'click',
                    delay: 500,
                    content: content,
                });
            }
        }
    }

    'use strict';
    function matterflyout($compile, $templateCache, $rootScope) {
        return {
            restrict: 'A',
            scope: {
                control: '&'
            },
            link: function (scope, element, attrs) {
                $(element).click(function (e) {
                    $rootScope.dispcontextualhelpinner = true;
                    $rootScope.contextualhelp = false;
                    $rootScope.displayinfo = false;
                    $rootScope.dispinner = true;
                    $rootScope.appMenuFlyOut = false;
                    $rootScope.flagAppMenuFlyOut = true;
                    $(".topheader").css("z-index", "4");
                    $(".CloseSwitcher").addClass("hide");
                    $(".OpenSwitcher").removeClass("hide");
                    $(".MenuCaption").removeClass("hideMenuCaption");
                    var obj = "";
                    obj = eval('(' + attrs.details + ')');
                    if (obj.matterSubAreaOfLaw == "") {
                        obj.matterSubAreaOfLaw = "NA";
                    }
                    var actualcontent = "";
                    var matterUrl = obj.matterClientUrl + "/" + obj.matterGuid;
                    actualcontent = '<div class="" role="region" aria-live="assertive" aria-atomic="true" style="position:relative;display:table-row" ng-click="stopEvent($event)">\
                                   <div class="FlyoutBoxContent flyoutwidth">\
                                      <div class="flyoutLeftarrow hidden-xs" style="top: 11px;left: -9px;"></div>\
                                      <div class="flyoutToparrow visible-xs" style="top: -8px;"></div>\
                                      <div class="FlyoutContent FlyoutHeading">\
                                          <div class="ms-Callout-content FlyoutHeadingText" title="' + obj.matterName + '">  ' + obj.matterName + ' </div>\
                                       </div>\
                                        <div class="ms-Callout-content commonFlyoutContaint"  ng-show=' + configs.search.searchColumnsUIPickerForMatter.matterClient.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutMenuColumn1DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.matterClient + '">' + obj.matterClient + '</div>\
                                       </div>\
                                        <div class="ms-Callout-content commonFlyoutContaint" ng-show=' + configs.search.searchColumnsUIPickerForMatter.matterClientId.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutMenuColumn3DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.matterClientId + '.' + obj.matterID + '">' + obj.matterClientId + '.' + obj.matterID + '</div>\
                                       </div>\
                                        <div class="ms-Callout-content commonFlyoutContaint" ng-show=' + configs.search.searchColumnsUIPickerForMatter.matterPracticeGroup.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutMenuColumn2DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.matterPracticeGroup + '">' + obj.matterPracticeGroup + '</div>\
                                       </div>\
                                       <div class="ms-Callout-content commonFlyoutContaint"  ng-show=' + configs.search.searchColumnsUIPickerForMatter.matterAreaOfLaw.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutMenuColumn3DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.matterAreaOfLaw + '">' + obj.matterAreaOfLaw + '</div>\
                                       </div>\
                                       <div class="ms-Callout-content commonFlyoutContaint"  ng-show=' + configs.search.searchColumnsUIPickerForMatter.matterSubAreaOfLaw.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutMenuColumn4DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.matterSubAreaOfLaw + '">' + obj.matterSubAreaOfLaw + '</div> \
                                       </div>\
                                       <div class="ms-Callout-content commonFlyoutContaint" ng-show=' + configs.search.searchColumnsUIPickerForMatter.matterResponsibleAttorney.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutMenuColumn6DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.matterResponsibleAttorney + '">' + obj.matterResponsibleAttorney + '</div>\
                                       </div>\
                                        <div ng-if="wordTextDisplay" class="ms-Callout-content commonFlyoutContaint">\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading" style="width:250px">Save the document to this locations</div><br/>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">URL:</div><br/>\
                                          <input type="text" value="' + matterUrl + '"><br/>\
                                       </div>\<a aria-describedby="description" aria-live="assertive" aria-atomic="true" aria-label="' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutButton1Text + '" role="button" id="viewMatters" class="ms-Button-label ms-Button ms-Button--primary ms-Callout-content" href="" ng-keydown="redirectViewMatters(\'' + obj.matterClientUrl + '\',\'' + obj.matterGuid + '\',$event)" ng-click="redirectViewMatters(\'' + obj.matterClientUrl + '\',\'' + obj.matterGuid + '\')">' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutButton1Text + '</a><br/>\
                                          <a aria-describedby="description" ng-if="!wordTextDisplay" aria-live="assertive" aria-label="' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutButton2Text + '" aria-atomic="true" role="button" class="ms-Button-label ms-Button ms-Button--primary ms-Callout-content" href="" id="uploadToMatter" ng-keydown="openUpload(\'' + obj.matterName + '\',\'' + obj.matterClientUrl + '\',\'' + obj.matterGuid + '\',$event)" ng-click="openUpload(\'' + obj.matterName + '\',\'' + obj.matterClientUrl + '\',\'' + obj.matterGuid + '\')" type="button">' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutButton2Text + '</a>\
                                      </div>\
                                </div>';

                    $templateCache.put("test.html", actualcontent);
                    var template = $templateCache.get("test.html");
                    var a = $compile("<div>" + actualcontent + "</div>")(scope);
                    $('.dropdown').removeClass('open');
                    $('.popcontent').css('display', 'none');
                    e.stopPropagation();
                    var obj = e.target.getBoundingClientRect();
                    $(this).parent().find('.popcontent').html(a[0]);
                    var dashboardTop = 350;
                    if ($(".commonContent").css('display') === 'block') {
                        dashboardTop = 450;
                    } else {
                        dashboardTop = 350;
                    }

                    if (obj.top < dashboardTop) {
                        $(this).parent().find('.popcontent').css({ 'display': 'block', 'left': '268px', 'top': '0' });
                        $(this).parent().find('.popcontent').find('.flyoutLeftarrow').css('top', '11px');
                    } else {
                        if (scope.$parent.$parent.$parent.grid.appScope.vm.center == "mattercenter") {
                            $(this).parent().find('.popcontent').css({ 'display': 'block', 'left': '268px', 'top': '-238px' });
                            $(this).parent().find('.popcontent').find('.flyoutLeftarrow').css('top', '244px');
                        } else {
                            $(this).parent().find('.popcontent').css({ 'display': 'block', 'left': '268px', 'top': '-218px' });
                            $(this).parent().find('.popcontent').find('.flyoutLeftarrow').css('top', '220px');
                        }
                    }
                  // if (scope.$$phase && !scope.$$phase) {
                        scope.$apply();
                   //}
                });
            },
            controller: function ($scope) {
                $scope.openUpload = function (matterName, matterUrl, matterGUID, event) {
                    $('.popcontent').css('display', 'none');
                    //To handle Accessability Fixes for KeyBoard Navigation
                    if (event == undefined) {
                        $scope.$parent.$parent.$parent.grid.appScope.vm.Openuploadmodal(matterName, matterUrl, matterGUID);                       
                    }                    
                    if (event && event.keyCode == 13) {                       
                         $scope.$parent.$parent.$parent.grid.appScope.vm.Openuploadmodal(matterName, matterUrl, matterGUID);                       
                     }
                    
                };
                $scope.redirectViewMatters = function (url, guid, event) {
                    //To handle Accessability Fixes for KeyBoard Navigation
                    if (event == undefined) {
                        $scope.$parent.$parent.$parent.grid.appScope.vm.viewMatterDetails(url, guid);
                    }
                    if (event && event.keyCode == 13) {
                        $scope.$parent.$parent.$parent.grid.appScope.vm.viewMatterDetails(url, guid);
                    }
                }
                $scope.stopEvent = function ($event) {
                    $event.stopPropagation();
                };
                if (typeof Word != 'undefined') {
                    $scope.wordTextDisplay = true;
                } else {
                    $scope.wordTextDisplay = false;
                }
            }
        }
    };

    'use strict';
    function documentflyout($http, $compile, $templateCache, $rootScope) {
        return {
            restrict: 'A',
           
            link: function (scope, element, attrs) {
                $(element).click(function (e) {
                    $rootScope.dispcontextualhelpinner = true;
                    $rootScope.contextualhelp = false;
                    $rootScope.displayinfo = false;
                    $rootScope.dispinner = true;
                    $rootScope.appMenuFlyOut = false;
                    $rootScope.flagAppMenuFlyOut = true;
                    $(".topheader").css("z-index", "4");
                    $(".CloseSwitcher").addClass("hide");
                    $(".OpenSwitcher").removeClass("hide");
                    $(".MenuCaption").removeClass("hideMenuCaption");
                    var obj = "";
                    obj = eval('(' + attrs.details + ')');
                    scope.documentInfo = {};
                    scope.documentInfo.documentParentUrl = obj.documentParentUrl;
                    scope.documentInfo.documentMatterUrl = obj.documentMatterUrl;
                    scope.documentInfo.documentClientUrl = obj.documentClientUrl;
                    scope.documentInfo.documentExtension = obj.documentExtension;
                    scope.documentInfo.documentName = obj.documentName;
                    if (obj.documentMatter == "") {
                        obj.documentMatter = "NA";
                    }
                    if (obj.documentClient == "") {
                        obj.documentClient = "NA";
                    }
                    if (obj.documentID == "") {
                        obj.documentID = "NA";
                    }
                    if (obj.documentOwner == "") {
                        obj.documentOwner = "NA";
                    }
                    if (obj.documentModifiedDate == "") {
                        obj.documentModifiedDate = "NA";
                    }
                    if (obj.documentPracticeGroup == "") {
                        obj.documentPracticeGroup = "NA";
                    }

                    var actualcontent = "";
                    actualcontent = '<div class="" ng-click="stopEvent($event)">\
                                   <div class="FlyoutBoxContent flyoutwidth" style="height:270px">\
                                      <div class="flyoutLeftarrow hidden-xs" style="top: 11px;left: -9px;"></div>\
                                           <div  >\
                                       <div class="flyoutToparrow visible-xs" style="top: -8px;"></div>\
                                      <div class="FlyoutContent">\
                                          <div class="ms-Callout-content FlyoutHeadingText" title="' + obj.documentName + '">  ' + obj.documentName + ' </div>\
                                       </div>\
                                       <div class="ms-Callout-content commonFlyoutContaint" ng-show=' + configs.search.searchColumnsUIPickerForDocument.documentMatterName.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.documentConfigContent.FlyoutMenuColumn1DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.documentMatterName + '">' + obj.documentMatterName + '</div>\
                                       </div>\
                                        <div class="ms-Callout-content commonFlyoutContaint" ng-show=' + configs.search.searchColumnsUIPickerForDocument.documentPracticeGroup.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.documentConfigContent.FlyoutMenuColumn2DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.documentPracticeGroup + '">' + obj.documentPracticeGroup + '</div>\
                                       </div>\
                                       <div class="ms-Callout-content commonFlyoutContaint" ng-show=' + configs.search.searchColumnsUIPickerForDocument.documentClient.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.documentConfigContent.GridColumn2Header + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.documentClient + '">' + obj.documentClient + '</div>\
                                       </div>\
                                       <div class="ms-Callout-content commonFlyoutContaint" ng-show=' + configs.search.searchColumnsUIPickerForDocument.documentID.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.documentConfigContent.FlyoutLable2Text + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.documentID + '">' + obj.documentID + '</div>\
                                       </div>\
                                        <div class="ms-Callout-content commonFlyoutContaint" ng-show=' + configs.search.searchColumnsUIPickerForDocument.documentVersion.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.documentConfigContent.FlyoutMenuColumn5DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.documentVersion + '">' + obj.documentVersion + '</div>\
                                       </div>\
                                       <div class="ms-Callout-content commonFlyoutContaint" ng-show=' + configs.search.searchColumnsUIPickerForDocument.documentOwner.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.documentConfigContent.FlyoutMenuColumn6DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.documentOwner + '" toggle="parentToggle(message)">' + obj.documentOwner + '</div> \
                                       </div>\
                                       <div class="ms-Callout-content commonFlyoutContaint" ng-show=' + configs.search.searchColumnsUIPickerForDocument.documentModifiedDate.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.documentConfigContent.FlyoutMenuColumn7DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.documentModifiedDate + '" datefilter date=' + obj.documentModifiedDate + '>' + obj.documentModifiedDate + '</div>\
                                       </div>\
                                       <div class="ms-Callout-content commonFlyoutContaint" ng-show=' + configs.search.searchColumnsUIPickerForDocument.documentCreatedDate.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.documentConfigContent.FlyoutMenuColumn7DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" datefilter title="' + obj.documentCreatedDate + '" date=' + obj.documentCreatedDate + '>' + obj.documentCreatedDate + '</div>\
                                       </div>\
                                       <a class="ms-Button-label ms-Button ms-Button--primary ms-Callout-content" id="viewMatters" style="width:190px;padding-left: 12.5%;" href="' + obj.documentUrl + '" target="_blank">' + scope.$parent.$parent.$parent.grid.appScope.vm.documentConfigContent.FlyoutButton1Text + '</a>\
                                      <a id="uploadToMatter" class="ms-Button-label ms-Button ms-Button--primary ms-Callout-content"  href="" style="width:190px" ng-keydown="gotoUrl(\'' + obj.documentClientUrl + '\',$event)" ng-click="gotoUrl(\'' + obj.documentClientUrl + '\')" target="_blank">' + scope.$parent.$parent.$parent.grid.appScope.vm.documentConfigContent.FlyoutButton2Text + '</a>\
                                      </div>\
                                    </div>\
                                </div>';
                    $templateCache.put("test.html", actualcontent);
                    var template = $templateCache.get("test.html");
                    var a = $compile("<div>" + template + "</div>")(scope);
                    $('.dropdown').removeClass('open');
                    $('.popcontent').css('display', 'none');
                    e.stopPropagation();
                    var obj = e.target.getBoundingClientRect();
                    $(this).parent().find('.popcontent').html(a[0]);
                    var dashboardTop = 350;
                    if ($(".commonContent").css('display') === 'block') {
                        dashboardTop = 450;
                    } else {
                        dashboardTop = 350;
                    }
                    if (obj.top < dashboardTop) {
                        $(this).parent().find('.popcontent').css({ 'display': 'block', 'left': '284px', 'top': '0' });
                        $(this).parent().find('.popcontent').find('.flyoutLeftarrow').css('top', '11px');
                    } else {
                        $(this).parent().find('.popcontent').css({ 'display': 'block', 'left': '284px', 'top': '-238px' });
                        $(this).parent().find('.popcontent').find('.flyoutLeftarrow').css('top', '244px');
                    }
                });
            },
            controller: function ($scope) {
                $scope.gotoUrl = function (url, event) {
                    //To handle Accessability Fixes for KeyBoard Navigation
                    $('.popcontent').css('display', 'none');
                    if (event==undefined) {
                        $scope.$parent.$parent.$parent.grid.appScope.vm.gotoDocumentUrl($scope.documentInfo);
                    }
                    if (event && event.keyCode == 9) {

                    } else if (event && event.keyCode == 13) {
                        $scope.$parent.$parent.$parent.grid.appScope.vm.gotoDocumentUrl($scope.documentInfo);
                    }
                }
                $scope.stopEvent = function ($event) {
                    $event.stopPropagation();
                };
            }
        }
    };


    'use strict';
    function onload($timeout) {
        return {
            restrict: 'AE',
            scope: { sortdetails: '@' },
            link: function (scope, element, attrs) {
                scope.$watch("sortdetails", function () {
                    $timeout(function () { jQuery('[id^="asc"]').hide(); }, 1500);
                    $timeout(function () { jQuery('[id^="desc"]').hide(); }, 1500);
                }, true);
            }
        }
    };

    'use strict';
    function fallbacksrc() {
        return {
            restrict: 'A',
            link: function postLink(scope, iElement, iAttrs) {
                iElement.bind('error', function () {
                    var rest = iAttrs.src.substring(0, iAttrs.src.lastIndexOf("/") + 1);
                    var last = iAttrs.src.substring(iAttrs.src.lastIndexOf("/") + 1, iAttrs.src.length);
                    last = "generaldocument.png";
                    iAttrs.src = rest + last;
                    angular.element(this).attr("src", iAttrs.src);
                });
            }
        }
    };

    'use strict'
    function myEnter() {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.myEnter);
                    });
                    event.preventDefault();
                }
            });
        };
    }

    'use strict'
    function uiGridMenuButton($window, $timeout) {
        return {
            restrict: 'AE',
            link: function (scope, element, attrs) {
                element.find('.ui-grid-icon-menu').addClass('showExpandIcon');

                $(element).on("click", function (e) {
                    e.stopPropagation();
                    $('.dropdown').removeClass("open");
                    $timeout(function () {
                        if (element.find('.ng-isolate-scope').html() != "<!-- ngIf: shown -->") {
                            element.find('.ui-grid-icon-menu').removeClass('showExpandIcon');
                            element.find('.ui-grid-icon-menu').addClass('closeColumnPicker');
                        } else {
                            element.find('.ui-grid-icon-menu').addClass('showExpandIcon');
                            element.find('.ui-grid-icon-menu').removeClass('closeColumnPicker');
                        }
                    }, 200)

                    if (element.find('.ui-grid-menu-mid')) {
                        element.find('.ui-grid-menu-mid').css('height', $window.innerHeight - 160);
                    }
                });
            }
        }
    }

    'use strict'
    function uiGridViewport($window) {
        return {
            restrict: 'AE',
            link: function (scope, element, attrs) {
                $(element).scroll(function (e) {
                    if ($window.innerHeight > 615) {
                        $('.popcontent').css('display', 'none');
                        $('.dropdown').removeClass("open");
                    }
                });
            }
        }
    }


    'use strict'
    function dropdown($rootScope) {
        return {
            restrict: 'AE',
            link: function (scope, element, attrs) {
                $rootScope.displayinfo = false;
                $rootScope.dispinner = true;
                $(element).click(function (e) {
                    var windowHeight = window.innerHeight;
                    var obj = e.target.getBoundingClientRect();
                    if (windowHeight >= 322 && windowHeight <= 637 && obj.top > 273) {
                        $(this).parent().addClass('dropup');
                    }

                    if (obj.top > 450) {
                        $(this).parent().addClass('dropup');
                    }

                });
            }
        }
    }

    'use strict'
    function assignTeamKeyDown() {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 8 || (event.which === 46 && event.key == "Delete")) {
                    scope.$apply(function () {
                        scope.assignTeam.assignedAllUserNamesAndEmails = scope.assignTeam.assignedUser;
                        var userEmails = getUserName(scope.assignTeam.assignedUser, false);
                        var existingTeams = [];
                        for (var i = 0; i < userEmails.length; i++) {
                            if (userEmails[i] != "") {
                                angular.forEach(scope.assignTeam.teamUsers, function (team) {
                                    if (team.userName == userEmails[i]) {
                                        existingTeams.push(team);
                                    }
                                });
                            }
                        }
                        scope.assignTeam.teamUsers = existingTeams;
                        // $scope.projects.splice(i, 1);
                        scope.$parent.cm.errorPopUpBlock = false;
                        scope.$parent.cm.errorBorder = "";
                    });
                } else {
                    if (event.which === 186) {
                        scope.assignTeam.assignedAllUserNamesAndEmails = scope.assignTeam.assignedUser + ";";
                    }
                    scope.$parent.cm.errorPopUpBlock = false;
                    scope.$parent.cm.errorBorder = "";
                }
            });
        };
    }
    'use strict'
    function getUserName(sUserEmails, bIsName) {

        var arrUserNames = [], sEmail = "", oEmailRegex = new RegExp("^[\\s]*\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*[\\s]*$");
        if (sUserEmails && null !== sUserEmails && "" !== sUserEmails) {
            arrUserNames = sUserEmails.split(";");
            for (var iIterator = 0; iIterator < arrUserNames.length - 1; iIterator++) {
                if (arrUserNames[iIterator] && null !== arrUserNames[iIterator] && "" !== arrUserNames[iIterator]) {
                    if (-1 !== arrUserNames[iIterator].lastIndexOf("(")) {
                        sEmail = $.trim(arrUserNames[iIterator].substring(arrUserNames[iIterator].lastIndexOf("(") + 1, arrUserNames[iIterator].lastIndexOf(")")));
                        if (oEmailRegex.test(sEmail)) {
                            arrUserNames[iIterator] = bIsName ? $.trim(arrUserNames[iIterator].substring(0, arrUserNames[iIterator].lastIndexOf("("))) : sEmail;
                        }
                    }
                }
            }
        }
        return arrUserNames;
    }

    'use strict';
    function showupload() {
        return {
            restrict: 'AE',
            link: function (scope, element, attrs) {
                var loginuser = attrs.loginuser.toLowerCase();
                var hideUpload = attrs.hideupload;
                if (hideUpload.toLowerCase().indexOf(loginuser) > -1) {
                    $(element).find('.showUploadImg').remove();
                    $(element).find('.hideUploadImg').css("display", "block");
                } else {
                    $(element).find('.hideUploadImg').remove();
                    $(element).find('.showUploadImg').css("display", "block");
                }

            }
        }
    }

    'use strict';
    function uibTypeaheadPopup() {
        return {
            restrict: 'AE',
            link: function (scope, element, attrs) {
                if ($(element).find("li")) {

                }
            }
        }
    }
    //Directive to render matter extra properties with different data types and controls.
    'use strict';
    function matteradditionalfieldsdirective($compile, $filter) {

        return {
            restrict: 'A',
            replace: true,
            link: function (scope, element, attrs) {
                var obj = "";
                obj = eval('(' + attrs.colorder + ')');
                if (scope.input.columnPosition == obj && scope.input.displayInUI == "true") {
                    var el = angular.element('<span  />');
                    if (scope.input.type.toLowerCase() == 'boolean') {
                        scope.input.required = false;
                        el.append('<div class="directiveMatterExtraBoolLabel"><span ng-show="' + scope.input.required + '" class="mandatory pull-left">*&nbsp;</span>' +
                           '<span ng-hide="' + scope.input.required + '" class="pull-left">&nbsp;&nbsp;&nbsp;&nbsp;</span>' +
                           '<label class="directiveFormFieldsLableWidth" >{{input.name}}: </label></div>');
                    }
                    else {
                        el.append('<div><span ng-show="' + scope.input.required + '" class="mandatory pull-left">*&nbsp;</span>' +
                           '<span ng-hide="' + scope.input.required + '" class="pull-left">&nbsp;&nbsp;&nbsp;&nbsp;</span>' +
                           '<label class="directiveFormFieldsLableWidth" >{{input.name}}: </label></div>');
                    }

                    switch (scope.input.type.toLowerCase()) {
                        case 'boolean':
                            if (scope.input.defaultValue != null && scope.input.defaultValue != undefined) {
                                scope.input.value = scope.input.defaultValue;
                            }
                            el.append('<div class="directiveMatterExtraBoolField"><input  ng-checked="{{input.value}}" id="' + scope.input.fieldInternalName + '"  aria-label="' + scope.input.fieldInternalName + '"  type="checkbox" ng-model="input.value"/></div>');
                            break;
                        case 'text':
                            if (scope.input.defaultValue != null && scope.input.defaultValue != undefined) {
                                scope.input.value = scope.input.defaultValue;
                            }
                            if (scope.input.required == "true") {
                                el.append('<div class="directiveMatterExtraFields"><input id="' + scope.input.fieldInternalName + '" class="directiveFormFields" aria-label="' + scope.input.fieldInternalName + '" ng-class="{errorBorder: (input.value == undefined && cm.addFieldReq == true)}"  required type="text" ng-model="input.value"/></div>');
                            }
                            else {
                                el.append('<div class="directiveMatterExtraFields"><input id="' + scope.input.fieldInternalName + '" aria-label="' + scope.input.fieldInternalName + '" class="directiveFormFields" type="text" ng-model="input.value"/></div>');
                            }
                            break;
                        case 'radiobuttons':
                            if (scope.input.defaultValue != null && scope.input.defaultValue != undefined) {
                                scope.input.value = scope.input.defaultValue;
                            }
                            var radioButtontText = "";

                            if (scope.input.required == "true") {
                                if (scope.input.defaultValue == null || scope.input.defaultValue == undefined) {
                                    scope.input.value = scope.input.values[0].choiceValue;
                                }

                                for (var i = 0; i < scope.input.values.length; i++) {
                                    radioButtontText += '<input name="radioGroup' + scope.input.values[i].fieldInternalName + '" type="radio" aria-label="' + scope.input.fieldInternalName + '" ng-model="input.value" value="' + scope.input.values[i].choiceValue + '"><label id="">' + scope.input.values[i].choiceValue + ' </label>'
                                }
                            }
                            else {
                                for (var i = 0; i < scope.input.values.length; i++) {
                                    radioButtontText += '<input name="radioGroup' + scope.input.values[i].fieldInternalName + '" type="radio"  aria-label="' + scope.input.fieldInternalName + '" ng-model="input.value" value="' + scope.input.values[i].choiceValue + '"><label id="">' + scope.input.values[i].choiceValue + ' </label>'
                                }
                            }
                            el.append('<div class="directiveMatterExtraFields">' + radioButtontText + '</div>')
                            break;
                        case 'dropdown':
                            if (scope.input.defaultValue != null && scope.input.defaultValue != undefined) {
                                var defaultValue = scope.input.defaultValue;
                                angular.forEach(scope.input.values, function (option) {
                                    if (option.choiceValue == defaultValue) {
                                        scope.input.value = option;                                        
                                    }                                   
                                });                               
                            }                           
                            if (scope.input.required == "true") {
                                el.append('<div class="directiveMatterExtraFields"><select id="' + scope.input.fieldInternalName + '" aria-label="' + scope.input.fieldInternalName + '" class="directiveFormFields" ng-class="{errorBorder: (input.value == undefined && cm.addFieldReq == true)}" required ng-model="input.value" ng-options=" x.choiceValue   for x in  input.values "> <option value="" label="- Select -"></option></select></div>')
                            }
                            else {
                                el.append('<div class="directiveMatterExtraFields"><select id="' + scope.input.fieldInternalName + '" aria-label="' + scope.input.fieldInternalName + '" class="directiveFormFields" ng-model="input.value" ng-options=" x.choiceValue   for x in  input.values "> <option value="" label="- Select -"></option></select></div>')
                            }
                            break;
                        case 'datetime':
                            if (scope.input.defaultValue != null && scope.input.defaultValue != undefined) {                              
                                scope.input.value = $filter('date')(scope.input.defaultValue, 'MM/dd/yyyy');
                                scope.input.value = new Date(scope.input.value);
                            }
                            if (scope.input.required == "true") {
                                el.append('<div class="directiveMatterExtraFields"> <input id="' + scope.input.fieldInternalName + '" class="directiveFormFields" aria-label="' + scope.input.fieldInternalName + '" ng-class="{errorBorder: (input.value == undefined && cm.addFieldReq == true)}" required type="text" class="calendar form-control " uib-datepicker-popup="MM/dd/yyyy" data-ng-model="input.value"  is-open="opened" placeholder="mm/dd/yyyy"  data-ng-model="" datepicker-options="dateOptions" ng-required="true" close-text="Close" readonly ng-keydown="$event.keyCode==13?open1():null"  ng-click="open1()"  /> </div>')
                            }
                            else {
                                el.append('<div class="directiveMatterExtraFields"> <input id="' + scope.input.fieldInternalName + '" class="directiveFormFields" aria-label="' + scope.input.fieldInternalName + '" type="text" class="calendar form-control " uib-datepicker-popup="MM/dd/yyyy" data-ng-model="input.value"  is-open="opened" placeholder="mm/dd/yyyy"  data-ng-model="" datepicker-options="dateOptions" ng-required="true" close-text="Close" readonly ng-keydown="$event.keyCode==13?open1():null" ng-click="open1()"  /> </div>')
                            }
                            break;
                        case 'multichoice':
                            if (scope.input.defaultValue != null && scope.input.defaultValue != undefined) {                                
                                var defaultOptions = scope.input.defaultValue.split(',');
                                scope.input.value = [];
                                for (var aCount = 0; aCount < defaultOptions.length; aCount++) {
                                    angular.forEach(scope.input.values, function (option) {
                                        if (option.choiceValue == defaultOptions[aCount]) {
                                            scope.input.value.push(option);
                                        }
                                    });
                                }
                            }
                            if (scope.input.required == "true") {
                                el.append('<div class="directiveMatterExtraFields"><select aria-label="' + scope.input.fieldInternalName + '"  id="' + scope.input.fieldInternalName + '" class="multiSelectHeight directiveFormFields" required style="height:100px;" multiple ng-model="input.value" ng-class="{errorBorder: (input.value == undefined && cm.addFieldReq == true)}" ng-options="x.choiceValue for x  in input.values "> <option value="" label="- Select -"></option></select></div>')
                            }
                            else {
                                el.append('<div class="directiveMatterExtraFields"><select aria-label="' + scope.input.fieldInternalName + '"  id="' + scope.input.fieldInternalName + '" class="multiSelectHeight directiveFormFields" required style="height:100px;" multiple ng-model="input.value" ng-options="x.choiceValue for x  in input.values "> <option value="" label="- Select -"></option></select></div>')
                            }
                            break;
                    }

                    $compile(el)(scope);
                    element.append(el);
                }
            },
            controller: function ($scope) {
                $scope.open1 = function ($event) {
                    if ($event) {
                        $event.preventDefault();
                        $event.stopPropagation();
                    }
                    $scope.opened = $scope.opened ? false : true;
                };
                $scope.dateOptions = {

                    formatYear: 'yy',
                    startingDay: 1,
                    shortcutPropagation: true
                };
                $scope.opened = false;
            }
        }
    }
    //Directive to render matter extra properties to set the values for to show or hide field in UI and to make field mandatory.
    'use strict';
    function extramatterpropertiefiledsinsettings($compile) {
        return {
            restrict: 'A',
            replace: true,
            link: function (scope, element, attrs) {
                var el = angular.element('<div></div>');
                var htmlTxt = '<div class="col-xs-12 pad0 marginTop10">\
                     <div class="col-xs-12 col-sm-5 pad0" role="region">\
                         <div class="row margin0">\
                             <div class="col-xs-12 defaultFontStyle displayInline pad0" tabindex="0" aria-live="assertive"  aria-atomic="true" aria-label="' + scope.input.name + '">' +
                               scope.input.name
                             + '</div>\
                             <div class="col-xs-12 col-sm-11 defaultFontStyle contentDescription pad0 marginTop10">\
                             </div>\
                         </div>\
                     </div>\
                     <div class="col-xs-12 col-sm-3 pad0 marginTop7">\
                         <div class="col-xs-12 defaultFontStyle displayInline pad0">'
                           + scope.$parent.vm.settingsConfigs.LabelDirectiveDisplayInUI +
                         '</div>\
                         <span class="marginRight10"> <input type="radio" tabindex="0" aria-live="assertive"  aria-atomic="true" aria-label=" Yes radio button to make ' + scope.input.name + ' display in UI? " ng-model="input.displayInUI" name="ui_' + scope.input.fieldInternalName + '" Value="true" />' + scope.$parent.vm.settingsConfigs.Radio1Option1Text + '</span>\
                         <span><input type="radio" tabindex="0" aria-live="assertive"  aria-atomic="true" aria-label=" No radio button to make ' + scope.input.name + ' not to display in UI? " name="ui_' + scope.input.fieldInternalName + '" Value="false" ng-model="input.displayInUI" />' + scope.$parent.vm.settingsConfigs.Radio1Option2Text + '</span>\
                     </div>\
                     <div class="col-xs-12 col-sm-3 pad0 marginTop7" ng-show="input.displayInUI==\'true\'">\
                         <div class="col-xs-12 defaultFontStyle displayInline pad0">'
                         + scope.$parent.vm.settingsConfigs.LabelDirectiveIsRequired +
                         '</div>\
                         <span class="marginRight10"> <input type="radio" tabindex="0" aria-live="assertive" aria-label=" Yes radio button to make ' + scope.input.name + ' required field? "  aria-atomic="true" ng-model="input.required" name="req_' + scope.input.fieldInternalName + '" value="true" />' + scope.$parent.vm.settingsConfigs.Radio1Option1Text + '</span>\
                         <span><input type="radio" tabindex="0" aria-live="assertive"  aria-atomic="true" aria-label=" No radio button to make ' + scope.input.name + ' not a required field? " name="req_' + scope.input.fieldInternalName + '" value="false" ng-model="input.required" />' + scope.$parent.vm.settingsConfigs.Radio1Option2Text + '</span>\
                     </div>\
                 </div>'
                el.append(htmlTxt);
                $compile(el)(scope);
                element.append(el);
            },
            controller: function ($scope) {
                $scope.open1 = function ($event) {
                    if ($event) {
                        $event.preventDefault();
                        $event.stopPropagation();
                    }
                    $scope.opened = $scope.opened ? false : true;
                };
                $scope.dateOptions = {

                    formatYear: 'yy',
                    startingDay: 1,
                    shortcutPropagation: true
                };

                $scope.opened = false;
            }
        }
    }


    //Directive for escape key
    'use strict';
    function ngEsc() {
        return {
            restrict: 'A',
            replace: true,
            link: function (scope, element, attrs) {
                element.bind("keydown keypress keyup", function (event) {
                    if (event.which === 27) {
                        scope.$apply(function () {
                            scope.$eval(attrs.ngEsc);
                        });

                        event.preventDefault();
                    }
                });
            }
        }
    }
 'use strict';
    function matterbulkuploadflyout($compile, $templateCache, $rootScope) {
        return {
            restrict: 'A',
            scope: {
                control: '&',
                folderlist:'=',
            },
            link: function (scope, element, attrs) {
                $(element).click(function (e) {
                    $rootScope.dispcontextualhelpinner = true;
                    $rootScope.contextualhelp = false;
                    $rootScope.displayinfo = false;
                    $rootScope.dispinner = true;
                    $rootScope.appMenuFlyOut = false;
                    $rootScope.flagAppMenuFlyOut = true;
                    $(".topheader").css("z-index", "4");
                    $(".CloseSwitcher").addClass("hide");
                    $(".OpenSwitcher").removeClass("hide");
                    $(".MenuCaption").removeClass("hideMenuCaption");
                    var obj = "";
                    obj = eval('(' + attrs.details + ')');
                    if (obj.matterSubAreaOfLaw == "") {
                        obj.matterSubAreaOfLaw = "NA";
                    }
                    scope.$parent.$parent.$parent.grid.appScope.vm.currentRow = obj;
                    var actualcontent = "";
                    var matterUrl = obj.matterClientUrl + "/" + obj.matterGuid;
                    actualcontent = '<div class="" style="position:relative;display:table-row" ng-click="stopEvent($event)">\
                                   <div class="FlyoutBoxContent flyoutwidth">\
                                      <div class="flyoutLeftarrow hidden-xs" style="top: 11px;left: -9px;"></div>\
                                      <div class="flyoutToparrow visible-xs" style="top: -8px;"></div>\
                                      <div class="FlyoutContent FlyoutHeading">\
                                          <div class="ms-Callout-content FlyoutHeadingText" title="' + obj.matterName + '">  ' + obj.matterName + ' </div>\
                                       </div>\
                                        <div class="ms-Callout-content commonFlyoutContaint"  ng-show=' + configs.search.searchColumnsUIPickerForMatter.matterClient.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutMenuColumn1DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.matterClient + '">' + obj.matterClient + '</div>\
                                       </div>\
                                        <div class="ms-Callout-content commonFlyoutContaint" ng-show=' + configs.search.searchColumnsUIPickerForMatter.matterClientId.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutMenuColumn3DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.matterClientId + '.' + obj.matterID + '">' + obj.matterClientId + '.' + obj.matterID + '</div>\
                                       </div>\
                                        <div class="ms-Callout-content commonFlyoutContaint" ng-show=' + configs.search.searchColumnsUIPickerForMatter.matterPracticeGroup.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutMenuColumn2DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.matterPracticeGroup + '">' + obj.matterPracticeGroup + '</div>\
                                       </div>\
                                       <div class="ms-Callout-content commonFlyoutContaint"  ng-show=' + configs.search.searchColumnsUIPickerForMatter.matterAreaOfLaw.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutMenuColumn3DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.matterAreaOfLaw + '">' + obj.matterAreaOfLaw + '</div>\
                                       </div>\
                                       <div class="ms-Callout-content commonFlyoutContaint"  ng-show=' + configs.search.searchColumnsUIPickerForMatter.matterSubAreaOfLaw.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutMenuColumn4DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.matterSubAreaOfLaw + '">' + obj.matterSubAreaOfLaw + '</div> \
                                       </div>\
                                       <div class="ms-Callout-content commonFlyoutContaint" ng-show=' + configs.search.searchColumnsUIPickerForMatter.matterResponsibleAttorney.displayInFlyOut + '>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutMenuColumn6DisplayName + ':</div>\
                                          <div class="ms-font-m FlyoutContent" title="' + obj.matterResponsibleAttorney + '">' + obj.matterResponsibleAttorney + '</div>\
                                       </div>\
                                        <div ng-if="wordTextDisplay" class="ms-Callout-content commonFlyoutContaint">\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading" style="width:250px">Save the document to this locations</div><br/>\
                                          <div class="fontWeight600 ms-font-m FlyoutContentHeading">URL:</div><br/>\
                                          <input type="text" value="' + matterUrl + '"><br/>\
                                       </div>\<a id="viewMatters" class="ms-Button-label ms-Button ms-Button--primary ms-Callout-content" href="" ng-click="redirectViewMatters(\'' + obj.matterClientUrl + '\',\'' + obj.matterGuid + '\')">' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutButton1Text + '</a><br/>\
                                          <a ng-if="!wordTextDisplay" class="ms-Button-label ms-Button ms-Button--primary ms-Callout-content"  id="uploadToMatter" ng-click="openUpload(\'' + obj.matterName + '\',\'' + obj.matterClientUrl + '\',\'' + obj.matterGuid + '\')" type="button">' + scope.$parent.$parent.$parent.grid.appScope.vm.matterConfigContent.FlyoutButton2Text + '</a>\
                                      </div>\
                                </div>';

                    $templateCache.put("test.html", actualcontent);
                    var template = $templateCache.get("test.html");
                    var a = $compile("<div>" + actualcontent + "</div>")(scope);
                    $('.dropdown').removeClass('open');
                    $('.popcontent').css('display', 'none');
                    $('.popcontent1').css('display', 'none');
                    e.stopPropagation();
                    var obj = e.target.getBoundingClientRect();
                    $(this).parent().find('.popcontent').html(a[0]);
                    var dashboardTop = 350;
                    if ($(".commonContent").css('display') === 'block') {
                        dashboardTop = 450;
                    } else {
                        dashboardTop = 350;
                    }

                    if (obj.top < dashboardTop) {
                      
                        $(this).parent().find('.popcontent').css({ 'display': 'block', 'left': '80px', 'top': '0' });
                        $(this).parent().find('.popcontent').find('.flyoutLeftarrow').css('top', '11px');

                        $(this).parent().find('.popcontent1').css({ 'display': 'none', 'left': '80px', 'top': '0' });
                        $(this).parent().find('.popcontent1').find('.flyoutLeftarrow').css('top', '11px');
                    } else {
                        if (scope.$parent.$parent.$parent.grid.appScope.vm.center == "mattercenter") {
                            $(this).parent().find('.popcontent').css({ 'display': 'block', 'left': '80px', 'top': '-238px' });
                            $(this).parent().find('.popcontent').find('.flyoutLeftarrow').css('top', '244px');

                            $(this).parent().find('.popcontent1').css({ 'display': 'none', 'left': '80px', 'top': '-238px' });
                            $(this).parent().find('.popcontent1').find('.flyoutLeftarrow').css('top', '244px');
                        } else {
                            $(this).parent().find('.popcontent').css({ 'display': 'block', 'left': '80px', 'top': '-218px' });
                            $(this).parent().find('.popcontent').find('.flyoutLeftarrow').css('top', '220px');
                            $(this).parent().find('.popcontent1').css({ 'display': 'none', 'left': '80px', 'top': '-218px' });
                            $(this).parent().find('.popcontent1').find('.flyoutLeftarrow').css('top', '220px');
                        }
                    }
                    scope.$apply();
                });
            },
            controller: function ($scope) {
                $scope.openUpload = function (matterName, matterUrl, matterGUID) {
                    $scope.$parent.$parent.$parent.grid.appScope.vm.Openuploadmodal(matterName, matterUrl, matterGUID);
                    $('.popcontent').css('display', 'none');
                    $('#' + matterGUID).css('display', 'block');
                };
                $scope.redirectViewMatters = function (url, guid) {
                    $scope.$parent.$parent.$parent.grid.appScope.vm.viewMatterDetails(url, guid);
                }
                $scope.stopEvent = function ($event) {
                    $event.stopPropagation();
                };
                if (typeof Word != 'undefined') {
                    $scope.wordTextDisplay = true;
                } else {
                    $scope.wordTextDisplay = false;
                }
            }
        }
    };


    var app = angular.module('matterMain');
    app.directive('onload', ['$timeout', onload]);
    app.directive('showbreadcrumb', [showbreadcrumb]);
    app.directive('datefilter', [datefilter]);
    app.directive("toggletab", [toggletab]);
    app.directive('infopopover', [infopopover]);
    app.directive('matterflyout', ['$compile', '$templateCache', '$rootScope', matterflyout]);
    app.directive('matterbulkuploadflyout', ['$compile', '$templateCache', '$rootScope', matterbulkuploadflyout]);
    app.directive('documentflyout', ['$http', '$compile', '$templateCache', '$rootScope', documentflyout]);
    app.directive('fallbacksrc', [fallbacksrc]);
    app.directive('myEnter', [myEnter]);
    app.directive('ngEsc', [ngEsc]);
    app.directive('uiGridMenuButton', ['$window', '$timeout', uiGridMenuButton]);
    //Adding Window
    app.directive('uiGridViewport', ['$window', uiGridViewport]);
    app.directive('dropdown', ['$rootScope', dropdown]);
    app.directive('assignteamkeydown', [assignTeamKeyDown]);
    app.directive('showupload', [showupload]);
    app.directive('matteradditionalfieldsdirective', ['$compile','$filter', matteradditionalfieldsdirective]);
    app.directive('extramatterpropertiefiledsinsettings', ['$compile', extramatterpropertiefiledsinsettings]);
    app.directive('uibTypeaheadPopup', [uibTypeaheadPopup]);
})();














