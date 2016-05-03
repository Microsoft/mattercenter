﻿(function () {
    'use strict';

    var app = angular.module("matterMain");

    app.controller('mattersController', ['$scope', '$state', '$interval', '$stateParams', 'api', '$timeout', 'matterResource',
function ($scope, $state, $interval, $stateParams, api, $timeout, matterResource) {
    var vm = this;
    vm.selected = undefined;
    // Onload show ui grid and hide error div
    //start
    $scope.divuigrid = true;
    $scope.nodata = false;
    //end

    //to hide lazyloader on load
    //start
    $scope.lazyloader = true;
    //end
    var headertemplate = "<div ng-class='{'sortable': sortable }'>\
    <div class='dropdown' style='float:right;'>\
        <a href='javascript:;' class='prisma-header-dropdown-anchor dropdown-toggle' type='button' data-toggle='dropdown'>\
            <img src='../images/icon-combobox.png'/>\
        </a>\
        <div class='dropdown-menu flyoutWrapper' role='menu'>\
           <input class='form-control' placeholder='Search'/> \
             <hr/> \
            <div class='clearFilterText' data-clearfiltertype='text'>\
            <div class='clearFiltersIcon'>\
                <img src='../Images/Filters_30px_X_30px_active_color_666.png' alt='clear' title='Clear filters'>\
            </div>\
            <div class='ms-font-m ms-font-weight-semilight clearText' title='Clear filters from Matter'><span>Clear filters from </span><span class='clearFilterTitle'>Matter</span></div>\
        </div>\
        </div>\
    </div>\
    <div role='button' class='ui-grid-cell-contents ui-grid-header-cell-primary-focus' col-index='renderIndex'>\
        <span class='ui-grid-header-cell-label ng-binding'>{{ col.colDef.displayName }}</span>\
        <span ui-grid-visible='col.sort.direction' ng-class='{ 'ui-grid-icon-up-dir': col.sort.direction == asc, 'ui-grid-icon-down-dir': col.sort.direction == desc, 'ui-grid-icon-blank': !col.sort.direction }' class='ui-grid-invisible ui-grid-icon-blank'>&nbsp;</span>\
    </div>\
    <div class='ui-grid-column-menu-button ng-scope' ng-if='grid.options.enableColumnMenus && !col.isRowHeader && col.colDef.enableColumnMenu !== false' ng-click='toggleMenu($event)' ng-class='{'ui-grid-column-menu-button-last-col': isLastCol}'>\
        <i ng-class='{ 'ui-grid-icon-up-dir': col.sort.direction == asc, 'ui-grid-icon-down-dir': col.sort.direction == desc, 'ui-grid-icon-blank': !col.sort.direction }' title='' aria-hidden='true' class='ui-grid-icon-up-dir'>&nbsp;</i>\
    </div>\
</div>"

    vm.gridOptions = {
        enableGridMenu: true,
        columnDefs: [{
            field: 'matterName', displayName: 'Matter', enableHiding: false, cellTemplate: '<div class="row"><div class="col-sm-8"><button class="btn btn-link" type="button"  data-container="body" data-toggle="popover" data-placement="right" details={{row.entity}} data-content="<div>{{row.entity.matterName}}</div> <div> <b>Client :</b>  {{row.entity.matterClient}} </div> <div><b>Client.Matter ID :</b> {{row.entity.matterClientId}}.{{row.entity.matterID}}</div> <div><b>Sub area of law :</b> {{row.entity.matterSubAreaOfLaw}} </div> <div><b>Responsible attorney</b> : {{row.entity.matterResponsibleAttorney}}</div><div><button><a >View matter details</a></button></div><div><button>Upload to a matter</button></div> " '
            + " > {{row.entity.matterName}} </button></div>"
            + "<div class='col-sm-4 text-right><div class='dropdown'><button class='btn btn-default dropdown-toggle' type='button' data-toggle='dropdown'>...</button><ul class='dropdown-menu'>"
            + "<li class='cursor' ng-click='grid.appScope.Openuploadmodal()'><a>Upload to this Matter</a></li><li><a href='#'>View Matter Details</li><li><a href='https://msmatter.sharepoint.com/sites/microsoft/'  target='_blank'>"
            + "Go to Matter OneNote</li><li class='cursor' ng-show='grid.appScope.ddlMatters.Id==1' ng-click='grid.appScope.PinMatter(row)'><a>Pin this Matter</a></li><li class='cursor'"
            + " ng-show='grid.appScope.ddlMatters.Id==2 || grid.appScope.ddlMatters.Id==3' ng-click='grid.appScope.UnpinMatter(row)'><a>Unpin this Matter</a></li></ul></div> </div></div>'",
            //menuItems: [
            // {
            //     title: 'Outer Scope Alert',
            //     icon: 'ui-grid-icon-info-circled',
            //     action: function ($event) {
            //         this.context.blargh(); // $scope.blargh() would work too, this is just an example
            //     },
            //     context: $scope
            // }],
            headerCellTemplate: headertemplate
        },
            { field: 'matterClient', displayName: 'Client', enableCellEdit: true, headerCellTemplate: headertemplate },
             //matterID 
    { field: 'matterClientId', displayName: 'Client.MatterID', cellTemplate: '<div class="ngCellText">{{row.entity.matterClientId}}.{{row.entity.matterID}}</div>', enableCellEdit: true, },
     { field: 'matterModifiedDate', displayName: 'Modified Date', type: 'date', cellFilter: 'date:\'MMM dd,yyyy\'' },
     { field: 'matterResponsibleAttorney', displayName: 'Responsible attorney', visible: false },
     { field: 'matterSubAreaOfLaw', displayName: 'Sub area of law', visible: false },
     { field: 'matterCreatedDate', displayName: 'Open date', type: 'date', cellFilter: 'date:\'MMM dd,yyyy\'', visible: false },
        ],
        enableColumnMenus: false,
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
            gridApi.core.on.columnVisibilityChanged($scope, function (changedColumn) {
                $scope.columnChanged = { name: changedColumn.colDef.name, visible: changedColumn.colDef.visible };
            });
        }
    };


    //search api call 
    function get(options, callback) {
        api({
            resource: 'matterResource',
            method: 'get',
            data: options,
            success: callback
        });
    }


    function getPinnedMatters(options, callback) {
        api({
            resource: 'matterResource',
            method: 'getPinnedMatters',
            data: options,
            success: callback
        });
    }


    //Callback function for pin 
    function PinMatters(options, callback) {
        api({
            resource: 'matterResource',
            method: 'PinMatters',
            data: options,
            success: callback
        });
    }


    //Callback function for unpin 
    function UnpinMatters(options, callback) {
        api({
            resource: 'matterResource',
            method: 'UnpinMatters',
            data: options,
            success: callback
        });
    }


    vm.searchMatter = function (val) {


        var searchRequest =
          {
              Client: {
                  Id: "123456",
                  Name: "Microsoft",
                  Url: "https://msmatter.sharepoint.com/sites/catalog"
              },
              SearchObject: {
                  PageNumber: 1,
                  ItemsPerPage: 10,
                  SearchTerm: val,
                  Filters: {},
                  Sort:
                          {
                              ByProperty: "LastModifiedTime",
                              Direction: 1
                          }
              }
          };


        return matterResource.get(searchRequest).$promise;
    }


    vm.search = function () {


        var searchRequest =
          {
              Client: {
                  Id: "123456",
                  Name: "Microsoft",
                  Url: "https://msmatter.sharepoint.com/sites/catalog"
              },
              SearchObject: {
                  PageNumber: 1,
                  ItemsPerPage: 10,
                  SearchTerm: vm.searchTerm,
                  Filters: {},
                  Sort:
                          {
                              ByProperty: "LastModifiedTime",
                              Direction: 1
                          }
              }
          };
        get(searchRequest, function (response) {
            vm.gridOptions.data = response.matterDataList;
        });
    }




    //Code written for displaying types in dropdown 
    //Start 
    $scope.Matters = [{ Id: 1, Name: "All Matters" }, { Id: 2, Name: "My Matters" }, { Id: 3, Name: "Pinned Matters" }];
    $scope.ddlMatters = $scope.Matters[0];


    //End  




    //Hits when the Dropdown changes 
    //Start 
    $scope.GetMatters = function (id) {
        $scope.lazyloader = false;
        if (id == 1) {
            var AllMattersRequest = {
                Client: {
                    Id: "123456",
                    Name: "Microsoft",
                    Url: "https://msmatter.sharepoint.com/sites/catalog"
                },
                SearchObject: {
                    PageNumber: 1,
                    ItemsPerPage: 10,
                    SearchTerm: "",
                    Filters: {
                        AOLList: "",
                        ClientsList: [],
                        FilterByMe: 0,
                        FromDate: "",
                        PGList: "",
                        ToDate: "",
                    },
                    Sort:
                            {
                                ByProperty: "LastModifiedTime",
                                Direction: 1
                            }
                }
            }


            get(AllMattersRequest, function (response) {
                $scope.lazyloader = true;
                if (response.errorCode == "404") {
                    $scope.divuigrid = false;
                    $scope.nodata = true;
                    $scope.errorMessage = response.message;
                } else {
                    $scope.divuigrid = true;
                    $scope.nodata = false;
                    vm.gridOptions.data = response;
                }
            });


        } else if (id == 2) {
            $scope.lazyloader = false;
            var MyMattersRequest = {
                Client: {
                    Id: "123456",
                    Name: "Microsoft",
                    Url: "https://msmatter.sharepoint.com/sites/catalog"
                },
                SearchObject: {
                    PageNumber: 1,
                    ItemsPerPage: 10,
                    SearchTerm: "",
                    Filters: {
                        AOLList: "",
                        ClientsList: [],
                        FilterByMe: 1,
                        FromDate: "",
                        PGList: "",
                        ToDate: ""
                    },
                    Sort:
                            {
                                ByProperty: "LastModifiedTime",
                                Direction: 1
                            }
                }
            }


            get(MyMattersRequest, function (response) {
                $scope.lazyloader = true;
                if (response.errorCode == "404") {
                    $scope.divuigrid = false;
                    $scope.nodata = true;
                    $scope.errorMessage = response.message;
                } else {
                    $scope.divuigrid = true;
                    $scope.nodata = false;
                    vm.gridOptions.data = response;
                }
            });
        } else if (id == 3) {
            $scope.lazyloader = false;
            var pinnedMattersRequest = {
                Id: "123456",
                Name: "Microsoft",
                Url: "https://msmatter.sharepoint.com/sites/catalog"
            }
            getPinnedMatters(pinnedMattersRequest, function (response) {
                $scope.lazyloader = true;
                if (response.errorCode == "404") {
                    $scope.divuigrid = false;
                    $scope.nodata = true;
                    $scope.errorMessage = response.message;
                } else {
                    $scope.divuigrid = true;
                    $scope.nodata = false;
                    vm.gridOptions.data = response.matterDataList;
                }
            });
        }
    }
    //End 


    //To run GetMatters function on page load 
    $scope.GetMatters(1);
    //End 


    //Written for unpinning the matter 
    //Start 
    $scope.UnpinMatter = function (data) {
        var alldata = data.entity;
        var unpinRequest = {
            Client: {
                Id: "123456",
                Name: "Microsoft",
                Url: "https://msmatter.sharepoint.com/sites/catalog"
            },
            matterData: {
                matterName: alldata.matterName,
                matterDescription: alldata.matterDescription,
                matterCreatedDate: alldata.matterCreatedDate,
                matterUrl: alldata.matterUrl,
                matterPracticeGroup: alldata.matterPracticeGroup,
                matterAreaOfLaw: alldata.matterAreaOfLaw,
                matterSubAreaOfLaw: alldata.matterSubAreaOfLaw,
                matterClientUrl: alldata.matterClientUrl,
                matterClient: alldata.matterClient,
                matterClientId: alldata.matterClientId,
                hideUpload: alldata.hideUpload,
                matterID: alldata.matterID,
                matterResponsibleAttorney: alldata.matterResponsibleAttorney,
                matterModifiedDate: alldata.matterModifiedDate,
                matterGuid: alldata.matterGuid


            }
        }
        UnpinMatters(unpinRequest, function (response) {
            if (response.isMatterUnPinned) {
                $scope.GetMatters($scope.ddlMatters);
                alert("Success");
            }
        });
    }
    //End 


    //Written for pinning the matter 
    //Start 
    $scope.PinMatter = function (data) {
        var alldata = data.entity;
        var pinRequest = {
            Client: {
                Id: "123456",
                Name: "Microsoft",
                Url: "https://msmatter.sharepoint.com/sites/catalog"
            },
            matterData: {
                matterName: alldata.matterName,
                matterDescription: alldata.matterDescription,
                matterCreatedDate: alldata.matterCreatedDate,
                matterUrl: alldata.matterUrl,
                matterPracticeGroup: alldata.matterPracticeGroup,
                matterAreaOfLaw: alldata.matterAreaOfLaw,
                matterSubAreaOfLaw: alldata.matterSubAreaOfLaw,
                matterClientUrl: alldata.matterClientUrl,
                matterClient: alldata.matterClient,
                matterClientId: alldata.matterClientId,
                hideUpload: alldata.hideUpload,
                matterID: alldata.matterID,
                matterResponsibleAttorney: alldata.matterResponsibleAttorney,
                matterModifiedDate: alldata.matterModifiedDate,
                matterGuid: alldata.matterGuid
            }
        }
        PinMatters(pinRequest, function (response) {
            if (response.isMatterPinned) {
                $scope.GetMatters($scope.ddlMatters);
                alert("Success");
            }
        });
    }
    //End 


    //To open the UploadMatterModal 
    $scope.Openuploadmodal = function () {
        jQuery('#UploadMatterModal').modal("show");
    }






    //To display modal up in center of the screen...
    //Start 
    function reposition() {
        var modal = $(this),
            dialog = modal.find('.modal-dialog');
        modal.css('display', 'block');


        // Dividing by two centers the modal exactly, but dividing by three  
        // or four works better for larger screens. 
        dialog.css("margin-top", Math.max(0, (jQuery(window).height() - dialog.height()) / 2));
    }
    // Reposition when a modal is shown 
    jQuery('.modal').on('show.bs.modal', reposition);
    // Reposition when the window is resized 
    jQuery(window).on('resize', function () {
        jQuery('.modal:visible').each(reposition);
    });


    $timeout(reposition(), 100);
    //End 
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



}]);
    app.directive('toggle', function () {
        return {
            restrict: 'AE',
            link: function (scope, element, attrs) {
                if (attrs.toggle == "popover") {
                    var obj = eval('(' + attrs.details + ')');
                    $(element).popover({
                        html: true,
                        trigger: 'click',
                        delay: 500,
                        //template:
                        //          '<div class="popover">\
                        //             <div class="popover-header">\
                        //                 <h3 class="popover-title"></h3>\
                        //              </div>\
                        //             <div class="popover-content">\
                        //                  ' + obj.matterName + ' \
                        //                  <div> <b>Client :</b> '+ obj.matterClient + '</div>\
                        //                  <div><b>Client.Matter ID :</b> '+ obj.matterClientId + '.' + obj.matterID + '</div>\
                        //                  <div><b>Sub area of law :</b> '+ obj.matterSubAreaOfLaw + '</div> \
                        //                  <div><b>Responsible attorney</b> : '+ obj.matterResponsibleAttorney + '</div>\
                        //                  <div><button ><a href="https://msmatter.sharepoint.com/sites/microsoft/SitePages" target="_blank">View matter details</a></button></div>\
                        //                  <div><button  ng-click="Openuploadmodal()">Upload to a matter</button></div>\
                        //             </div>\
                        //           </div>'

                    });
                }
            }
        };
    })


})();

