$(function () {
    var l = abp.localization.getResource("RockIT");

    var practiceGroupService = window.steer73.rockIT.practiceGroups.practiceGroups;


    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "PracticeGroups/CreateModal",
        scriptUrl: abp.appPath + "Pages/PracticeGroups/createModal.js",
        modalClass: "practiceGroupCreate"
    });

    var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "PracticeGroups/EditModal",
        scriptUrl: abp.appPath + "Pages/PracticeGroups/editModal.js",
        modalClass: "practiceGroupEdit"
    });

    var getFilter = function () {
        return {
            filterText: $("#FilterText").val(),
            name: $("#NameFilter").val(),
            isActive: (function () {
                var value = $("#IsActiveFilter").val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })()
        };
    };

    //<suite-custom-code-block-1>
    //</suite-custom-code-block-1>

    var dataTableColumns = [
        {
            title: l("Actions"),
            rowAction: {
                items:
                    [
                        {
                            text: l("Edit"),
                            visible: abp.auth.isGranted('RockIT.PracticeGroups.Edit'),
                            action: function (data) {
                                editModal.open({
                                    id: data.record.id
                                });
                            }
                        }
                    ]
            },
            width: "1rem"
        },
        {
            title: l("PrimaryPracticeGroup"),
            data: "name"
        },
        {
            title: l("IsActive"),
            data: "isActive",
            render: function (isActive) {
                return isActive ? '<i class="fa fa-check"></i>' : '<i class="fa fa-times"></i>';
            }
        }
    ];

    var defaultSortColumnIndex = 1;
    if (!dataTableColumns[0].rowAction.items.some(a => a.visible))
    {
        // Remove row actions column if none if none of the actions is visible to avoid errors in data table library when rendering
        dataTableColumns.shift();
        defaultSortColumnIndex = 0;
    }

    var dataTable = $("#PracticeGroupsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[defaultSortColumnIndex, "asc"]],
        ajax: abp.libs.datatables.createAjax(practiceGroupService.getList, getFilter),
        columnDefs: dataTableColumns
    }));



    //<suite-custom-code-block-2>
    //</suite-custom-code-block-2>

    createModal.onResult(function () {
        dataTable.ajax.reloadEx();;


    });

    editModal.onResult(function () {
        dataTable.ajax.reloadEx();;


    });

    $("#NewPracticeGroupButton").click(function (e) {
        e.preventDefault();
        createModal.open();
    });

    $("#SearchForm").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reloadEx();;


    });

    $('#AdvancedFilterSectionToggler').on('click', function (e) {
        $('#AdvancedFilterSection').toggle();
    });

    $('#AdvancedFilterSection').on('keypress', function (e) {
        if (e.which === 13) {
            dataTable.ajax.reloadEx();


        }
    });

    $('#AdvancedFilterSection select').change(function () {
        dataTable.ajax.reloadEx();


    });

    //<suite-custom-code-block-3>
    //</suite-custom-code-block-3>

});
