$(function () {
    var l = abp.localization.getResource("RockIT");
	
	var formDefinitionService = window.steer73.rockIT.formDefinitions.formDefinitions;
	
	
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "FormDefinitions/CreateModal",
        scriptUrl: abp.appPath + "Pages/FormDefinitions/createModal.js",
        modalClass: "formDefinitionCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "FormDefinitions/EditModal",
        scriptUrl: abp.appPath + "Pages/FormDefinitions/editModal.js",
        modalClass: "formDefinitionEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            referenceId: $("#ReferenceIdFilter").val(),
			name: $("#NameFilter").val(),
			/*formDetails: $("#FormDetailsFilter").val(),*/
            formType: $("#FormTypeFilter").val(),
            companyId: $("#CompanyIdFilter").val()
        };
    };
    
    //<suite-custom-code-block-1>
    //</suite-custom-code-block-1>
    
    var dataTableColumns = [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l("Edit"),
                                visible: (data, context) => {
                                    return abp.auth.isGranted('RockIT.FormDefinitions.Edit') 
                                        && data.formDefinition.activeVacanciesCount === 0
                                        && data.formDefinition.closedVacanciesCount === 0;
                                },
                                action: function (data) {
                                    editModal.open({
                                        id: data.record.formDefinition.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: (data, context) => {
                                    return abp.auth.isGranted('RockIT.FormDefinitions.Delete') 
                                        && data.formDefinition.activeVacanciesCount === 0
                                        && data.formDefinition.closedVacanciesCount === 0;
                                },
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    formDefinitionService.delete(data.record.formDefinition.id)
                                        .then(function () {
                                            abp.notify.success(l("SuccessfullyDeleted"));
                                            dataTable.ajax.reloadEx();;
                                        });
                                }
                            }
                        ]
                },
                width: "1rem"
            },
			{ data: "formDefinition.referenceId" },
			{ data: "formDefinition.name" },
			/*{ data: "formDetails" },*/
            {
                data: "formDefinition.formType",
                render: function (formType) {
                    if (formType === undefined ||
                        formType === null) {
                        return "";
                    }

                    var localizationKey = "Enum:FormType." + formType;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            },
            {
                data: "company.name",
                defaultContent: ""
            },
            {
                render: (data, type, row) => {
                    return `<a class=\"btn btn-primary\" href=\"/FormDefinitions/FormBuilder?Id=${row.formDefinition.id}\">${l("EditForm")}</a>`;
                }
            }
    ];
    
    
    
    

    var dataTable = $("#FormDefinitionsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(formDefinitionService.getList, getFilter),
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

    $("#NewFormDefinitionButton").click(function (e) {
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

    $('#AdvancedFilterSection select').change(function() {
        dataTable.ajax.reloadEx();
        
        
    });
    
    //<suite-custom-code-block-3>
    //</suite-custom-code-block-3>
    
    
    
    
    
    
    
    
});
