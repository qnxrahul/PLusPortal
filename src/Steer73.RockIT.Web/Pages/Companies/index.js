$(function () {
    var l = abp.localization.getResource("RockIT");
	
	var companyService = window.steer73.rockIT.companies.companies;
	
	
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Companies/CreateModal",
        scriptUrl: abp.appPath + "Pages/Companies/createModal.js",
        modalClass: "companyCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Companies/EditModal",
        scriptUrl: abp.appPath + "Pages/Companies/editModal.js",
        modalClass: "companyEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            name: $("#NameFilter").val(),
			phone: $("#PhoneFilter").val(),
			address: $("#AddressFilter").val(),
			postcode: $("#PostcodeFilter").val(),
			primaryContact: $("#PrimaryContactFilter").val()
        };
    };
    
    //<suite-custom-code-block-1>
    //</suite-custom-code-block-1>
    
    var dataTableColumns = [
        {
            data: "logoUrl",
            render: function (data, type, row, meta) {
                var defaultLogo = "/images/company/default-company-logo.jpg";
                var logoSrc = data && data.trim() !== "" ? data : defaultLogo;
                return '<img src="' + logoSrc + '" alt="Image" style="height:50px;"/>';
            }
        },
        { data: "name" },
        { data: "phone" },
        { data: "address" },
        { data: "postcode" },
        { data: "primaryContact" }   
    ];
    
    var dataTable = $("#CompaniesTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[2, "asc"]],
        ajax: abp.libs.datatables.createAjax(companyService.getList, getFilter),
        columnDefs: dataTableColumns
    }));
            
    
    //<suite-custom-code-block-2>
    //</suite-custom-code-block-2>

    createModal.onResult(function () {
        dataTable.ajax.reloadEx();
    });

    editModal.onResult(function () {
        dataTable.ajax.reloadEx();  
    });

    $("#NewCompanyButton").click(function (e) {
        e.preventDefault();
        createModal.open();
    });

	$("#SearchForm").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reloadEx();
    });

    $("#ExportToExcelButton").click(function (e) {
        e.preventDefault();

        companyService.getDownloadToken().then(
            function(result){
                    var input = getFilter();
                    var url =  abp.appPath + 'api/app/companies/as-excel-file' + 
                        abp.utils.buildQueryString([
                            { name: 'downloadToken', value: result.token },
                            { name: 'filterText', value: input.filterText }, 
                            { name: 'name', value: input.name }, 
                            { name: 'phone', value: input.phone }, 
                            { name: 'address', value: input.address }, 
                            { name: 'postcode', value: input.postcode }, 
                            { name: 'primaryContact', value: input.primaryContact }
                            ]);
                            
                    var downloadWindow = window.open(url, '_blank');
                    downloadWindow.focus();
            }
        )
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

    $("#trigger-companies-sync-btn").click(async function (e) {
        e.preventDefault();

        abp.ui.setBusy();

        $.ajax({
            url: abp.appPath + 'api/app/companies/trigger-sync',
            data: '',
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (data) {
                abp.message.success(l("TriggerSyncSuccessMessage"));
                dataTable.ajax.reloadEx();
            },
            error: function (data) {
                abp.message.error(l("TriggerSyncErrorMessage"));
            }
        });

        abp.ui.clearBusy();
    });
    //<suite-custom-code-block-3>
    //</suite-custom-code-block-3>
    
    
    
    
    
    
    
    
});
