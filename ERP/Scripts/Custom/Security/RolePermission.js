

$(document).ready(function () {
    RolePermission.ReadyFunction();
    RolePermission.RoleDropDown();

});

function EditSpan(inputId, btnSave, btnEdit) {
    $("#" + inputId).removeAttr('disabled').removeAttr('style');
    $("#" + inputId).css({ "border-radius": "4px", "border": "1px solid #DEB0A6", "padding-left": "2px" });
    $("#" + btnEdit).hide();
    $("#" + btnSave).show();
}

function SaveSpan(inputId, btnSave, btnEdit, resId) {
    var jsonParam = { id: resId, displayName: $("#" + inputId).val() };
    var serviceUrl = "/Security/UpdateResourcePermission/";
    JsManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);
    function onSuccess(jsonData) {
        if (jsonData == "0") {
            Message.Error("save");
        }
        else {
            $("#" + inputId).css({ "border": "none", "background": "none" });
            $("#" + inputId).attr("disabled", "disabled");
            $("#" + btnEdit).show();
            $("#" + btnSave).hide();
        }
    }
    function onFailed(xhr, status, err) {
        Message.Exception(xhr);
    }

}


function UpdateStatus(chkId, inputType) {
    var objs = [];
    var dataTypeName;
    var rolePermissionId;
    var resourcePermissionId;

    //only for add edit and delete permission checkbox
    if (inputType == "inputprmi") {
        var obj = new Object();
        dataTypeName = $("#" + chkId).attr("data-type");

        if ($('#' + chkId).is(':checked') == true) {

            $("#" + chkId).prop('checked', true);
            if (dataTypeName == "add") {
                obj.Add = true;
            }
            else if (dataTypeName == "edit") {
                obj.Edit = true;
            }
            else if (dataTypeName == "delete") {
                obj.Delete = true;
            }
        }
        else {

            $("#" + chkId).prop('checked', false);

            if (dataTypeName == "add") {
                obj.Add = false;
            }
            else if (dataTypeName == "edit") {
                obj.Edit = false;
            }
            else if (dataTypeName == "delete") {
                obj.Delete = false;
            }
        }

        //2 for role permission,1 for resource permission
        obj.Flag = 2;
        obj.RolePermissionId = $("#" + chkId).attr("data-roleprmiid");
        obj.ResourcePermissionId = "";
        obj.ResourceStatus = false;
        objs.push(obj);
    }


    else if (inputType == "rolePrmiWithResourcePrmi") {

        $('#' + chkId).parent().find('input[type=checkbox]').each(function (i, v) {
            dataTypeName = $(v).attr("data-type");
            rolePermissionId = $(v).attr("data-roleprmiid");
            resourcePermissionId = $(v).attr("data-resprmiid");
            var obj = new Object();

            if ($('#' + chkId).is(':checked') == true) {

                $("#" + $(v).attr("Id")).prop('checked', true);

                if (dataTypeName == "add") {
                    obj.Add = true;
                    obj.RolePermissionId = rolePermissionId;
                    tmpFlag = 2;
                }
                else if (dataTypeName == "edit") {
                    obj.Edit = true;
                    obj.RolePermissionId = rolePermissionId;
                    tmpFlag = 2;
                }
                else if (dataTypeName == "delete") {
                    obj.Delete = true;
                    obj.RolePermissionId = rolePermissionId;
                    tmpFlag = 2;
                }
                else {
                    obj.ResourcePermissionId = resourcePermissionId;
                    obj.ResourceStatus = true;
                    tmpFlag = 1;
                }

            }
            else {

                $("#" + $(v).attr("Id")).prop('checked', false);

                if (dataTypeName == "add") {
                    obj.Add = false;
                    obj.RolePermissionId = rolePermissionId;
                    tmpFlag = 2;
                }
                else if (dataTypeName == "edit") {
                    obj.Edit = false;
                    obj.RolePermissionId = rolePermissionId;
                    tmpFlag = 2;
                }
                else if (dataTypeName == "delete") {
                    obj.Delete = false;
                    obj.RolePermissionId = rolePermissionId;
                    tmpFlag = 2;
                }
                else {
                    obj.ResourcePermissionId = resourcePermissionId;
                    obj.ResourceStatus = false;
                    tmpFlag = 1;
                }
            }
            obj.Flag = tmpFlag;
            objs.push(obj);
        });
    }

        //this is add,edit,delete with each other checkbox
    else {

        $("#" + chkId).parent().parent().find('input[type=checkbox]').each(function (i, v) {
            var obj = new Object();
            dataTypeName = $(v).attr("data-type");
            rolePermissionId = $(v).attr("data-roleprmiid");
            resourcePermissionId = $(v).attr("data-resprmiid");
            var tmpFlag;

            if ($('#' + chkId).is(':checked') == true) {

                $("#" + $(v).attr("Id")).prop('checked', true);

                if (dataTypeName == "add") {
                    obj.Add = true;
                    obj.RolePermissionId = rolePermissionId;
                    tmpFlag = 2;
                }
                else if (dataTypeName == "edit") {
                    obj.Edit = true;
                    obj.RolePermissionId = rolePermissionId;
                    tmpFlag = 2;
                }
                else if (dataTypeName == "delete") {
                    obj.Delete = true;
                    obj.RolePermissionId = rolePermissionId;
                    tmpFlag = 2;
                }
                else {
                    obj.ResourcePermissionId = resourcePermissionId;
                    obj.ResourceStatus = true;
                    tmpFlag = 1;
                }
            }
            else {

                $("#" + $(v).attr("Id")).prop('checked', false);

                if (dataTypeName == "add") {
                    obj.Add = false;
                    obj.RolePermissionId = rolePermissionId;
                    tmpFlag = 2;
                }
                else if (dataTypeName == "edit") {
                    obj.Edit = false;
                    obj.RolePermissionId = rolePermissionId;
                    tmpFlag = 2;
                }
                else if (dataTypeName == "delete") {
                    obj.Delete = false;
                    obj.RolePermissionId = rolePermissionId;
                    tmpFlag = 2;
                }
                else {
                    obj.ResourcePermissionId = resourcePermissionId;
                    obj.ResourceStatus = false;
                    tmpFlag = 1;
                }
            }
            obj.Flag = tmpFlag;
            if (dataTypeName != "NO") {
                objs.push(obj);
            }

        });
    }

    if (objs.length > 0) {
        var jsonParam = 'objRoleWithResourcePermission=' + JSON.stringify(objs);
        var serviceURL = "/Security/UpdateRoleWithResourcePermission/";
        JsManager.SendJson(serviceURL, jsonParam, onSuccess, onFailed);
    }
    function onSuccess(jsonData) {
        if (jsonData == "0") {
            Message.Error("save");
        }
    }
    function onFailed(xhr, status, err) {
        Message.Exception(xhr);
    }

}


var RolePermission = {
    RoleDropDown: function () {
        var jsonParam = '';
        var serviceURL = "/PosDropDown/GetRole/";
        JsManager.SendJsonAsyncON(serviceURL, jsonParam, onSuccess, onFailed);
        function onSuccess(jsonData) {
            JsManager.PopulateCombo('#rollId', jsonData, 'Select Role','0');
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    },

    ReadyFunction: function () {
        // first example
        $("#browser").treeview();

        // second example
        $("#navigation").treeview({
            persist: "location",
            collapsed: false,
            unique: true
        });

        // third example
        $("#red").treeview({
            animated: "fast",
            collapsed: true,
            unique: true,
            persist: "cookie",
            toggle: function () {
                //window.console && console.log("%o was toggled", this);
            }
        });

        // fourth example
        $("#black, #gray").treeview({
            control: "#treecontrol",
            persist: "cookie",
            cookieId: "treeview-black"
        });
        setTimeout(function () {
            $("#rollId").val(localStorage.getItem("RoleIdForResourcePermission") == null ? 0 : localStorage.getItem("RoleIdForResourcePermission"));
            localStorage.removeItem("RoleIdForResourcePermission");
            }, 1000);


        $('#rollId').change(function () {
            RolePermission.SetRoleIdForResourcePermission();
        });
    },

    SetRoleIdForResourcePermission: function () {
        var jsonParam = { roleId: $('#rollId').val() };
        var serviceURL = "/Security/SetRoleIdForResourcePermission/";
        JsManager.SendJson(serviceURL, jsonParam, onSuccess, onFailed);
        function onSuccess(jsonData) {
            if (jsonData == 0) {
                Message.Warning("Reload and try again later");
            }
            else {
                localStorage.setItem("RoleIdForResourcePermission", $('#rollId').val());
                window.location = "/Security/RolePermission ";
            }
        }
        function onFailed(xhr, status, err) {
            Message.Exception(xhr);
        }
    },

}


