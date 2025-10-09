$(function () {
    var l = abp.localization.getResource("RockIT");
	
	var jobApplicationService = window.steer73.rockIT.jobApplications.jobApplications;
	
        var lastNpIdId = '';
        var lastNpDisplayNameId = '';

        var _lookupModal = new abp.ModalManager({
            viewUrl: abp.appPath + "Shared/LookupModal",
            scriptUrl: abp.appPath + "Pages/Shared/lookupModal.js",
            modalClass: "navigationPropertyLookup"
        });

        $('.lookupCleanButton').on('click', '', function () {
            $(this).parent().find('input').val('');
        });

        _lookupModal.onClose(function () {
            var modal = $(_lookupModal.getModal());
            $('#' + lastNpIdId).val(modal.find('#CurrentLookupId').val());
            $('#' + lastNpDisplayNameId).val(modal.find('#CurrentLookupDisplayName').val());
        });
	
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "JobApplications/CreateModal",
        scriptUrl: abp.appPath + "Pages/JobApplications/createModal.js",
        modalClass: "jobApplicationCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "JobApplications/EditModal",
        scriptUrl: abp.appPath + "Pages/JobApplications/editModal.js",
        modalClass: "jobApplicationEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            firstName: $("#FirstNameFilter").val(),
			lastName: $("#LastNameFilter").val(),
			emailAddress: $("#EmailAddressFilter").val(),
			title: $("#TitleFilter").val(),
			phoneNumber: $("#PhoneNumberFilter").val(),
			landline: $("#LandlineFilter").val(),
			currentRole: $("#CurrentRoleFilter").val(),
			currentCompany: $("#CurrentCompanyFilter").val(),
			currentPositionType: $("#CurrentPositionTypeFilter").val(),
			vacancyId: $("#VacancyIdFilter").val()
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
                                visible: abp.auth.isGranted('RockIT.JobApplications.Edit'),
                                action: function (data) {
                                    editModal.open({
                                     id: data.record.jobApplication.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('RockIT.JobApplications.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    jobApplicationService.delete(data.record.jobApplication.id)
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
			{ data: "jobApplication.firstName" },
			{ data: "jobApplication.lastName" },
			{ data: "jobApplication.emailAddress" },
			{ data: "jobApplication.title" },
			{ data: "jobApplication.phoneNumber" },
			{ data: "jobApplication.landline" },
			{ data: "jobApplication.currentRole" },
			{ data: "jobApplication.currentCompany" },
			{ data: "jobApplication.currentPositionType" },
            {
                data: "jobApplication.status",
                render: function (status, _type, row) {
                    return row.jobApplication.statusAsString;
                }
            },
            { 
                data: "jobApplication.cVId",
                render: function (cVId) {
                    if(!cVId || cVId === "00000000-0000-0000-0000-000000000000") {
                        return "-";
                    }
                    
                    return "<span class='jobApplication-c-v-file' style='cursor: pointer;' data-file-id='" + cVId + "'><i class='fa fa-file'></i></span>";
                }
            },
            { 
                data: "jobApplication.coverLetterId",
                render: function (coverLetterId) {
                    if(!coverLetterId || coverLetterId === "00000000-0000-0000-0000-000000000000") {
                        return "-";
                    }
                    
                    return "<span class='jobApplication-cover-letter-file' style='cursor: pointer;' data-file-id='" + coverLetterId + "'><i class='fa fa-file'></i></span>";
                }
            },
            { 
                data: "jobApplication.additionalDocumentId",
                render: function (additionalDocumentId) {
                    if(!additionalDocumentId || additionalDocumentId === "00000000-0000-0000-0000-000000000000") {
                        return "-";
                    }
                    
                    return "<span class='jobApplication-additional-document-file' style='cursor: pointer;' data-file-id='" + additionalDocumentId + "'><i class='fa fa-file'></i></span>";
                }
            },
            {
                data: "vacancy.title",
                defaultContent : ""
            }        
    ];
    
    
    $("body").on("click", "span.jobApplication-c-v-file", function () {
        var fileId = $(this).data("file-id");
        
        
        
        
        jobApplicationService.getDownloadToken().then(
            function(result){
                var url =  abp.appPath + 'api/app/job-applications/file' +
                    abp.utils.buildQueryString([
                        { name: 'downloadToken', value: result.token },
                        { name: 'fileId', value: fileId }
                    ]);

                var downloadWindow = window.open(url, '_blank');
                downloadWindow.focus();
            }
        )
    });


    $("body").on("click", "span.jobApplication-cover-letter-file", function () {
        var fileId = $(this).data("file-id");
        
        
        
        
        jobApplicationService.getDownloadToken().then(
            function(result){
                var url =  abp.appPath + 'api/app/job-applications/file' +
                    abp.utils.buildQueryString([
                        { name: 'downloadToken', value: result.token },
                        { name: 'fileId', value: fileId }
                    ]);

                var downloadWindow = window.open(url, '_blank');
                downloadWindow.focus();
            }
        )
    });


    $("body").on("click", "span.jobApplication-additional-document-file", function () {
        var fileId = $(this).data("file-id");
        
        
        
        
        jobApplicationService.getDownloadToken().then(
            function(result){
                var url =  abp.appPath + 'api/app/job-applications/file' +
                    abp.utils.buildQueryString([
                        { name: 'downloadToken', value: result.token },
                        { name: 'fileId', value: fileId }
                    ]);

                var downloadWindow = window.open(url, '_blank');
                downloadWindow.focus();
            }
        )
    });


        var showDetailRows = abp.auth.isGranted('RockIT.DiversityDatas') || abp.auth.isGranted('RockIT.JobFormResponses') || abp.auth.isGranted('RockIT.DiversityFormResponses') ;
    if(showDetailRows) {
        dataTableColumns.unshift({
            class: "details-control text-center",
            orderable: false,
            data: null,
            defaultContent: '<i class="fa fa-chevron-down"></i>',
            width: "0.1rem"
        });
    }
    else {
        $("#DetailRowTHeader").remove();
    }
    

    var dataTable = $("#JobApplicationsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[3, "asc"],[2, "asc"]],
        ajax: abp.libs.datatables.createAjax(jobApplicationService.getList, getFilter),
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

    $("#NewJobApplicationButton").click(function (e) {
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
    
    
    
        $('#JobApplicationsTable').on('click', 'td.details-control', function () {
        $(this).find("i").toggleClass("fa-chevron-down").toggleClass("fa-chevron-up");
        
        var tr = $(this).parents('tr');
        var row = dataTable.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            var data = row.data();
            
            detailRows(data)
                .done(function (result) {
                    row.child(result).show();
                    initDataGrids(data);
                });

            tr.addClass('shown');
        }
    } );

    function detailRows (data) {
        return $.ajax(abp.appPath + "JobApplications/ChildDataGrid?jobApplicationId=" + data.jobApplication.id)
            .done(function (result) {
                return result;
            });
    }
    
    function initDataGrids(data) {
        initDiversityDataGrid(data)
        $("#JobFormResponses-tab-" + data.jobFormResponse.id).one("click", function (e) {
            initJobFormResponseGrid(data);
        });
        $("#DiversityFormResponses-tab-" + data.diversityFormResponse.id).one("click", function (e) {
            initDiversityFormResponseGrid(data);
        });
    }
    
        function initDiversityDataGrid(data) {
        if(!abp.auth.isGranted("RockIT.DiversityDatas")) {
            return;
        }
        
        var jobApplicationId = data.jobApplication.id;

        
        var diversityDataService = window.steer73.rockIT.diversityDatas.diversityDatas;

        var diversityDataCreateModal = new abp.ModalManager({
            viewUrl: abp.appPath + "DiversityDatas/CreateModal",
            scriptUrl: abp.appPath + "Pages/DiversityDatas/createModal.js",
            modalClass: "diversityDataCreate"
        });

        var diversityDataEditModal = new abp.ModalManager({
            viewUrl: abp.appPath + "DiversityDatas/EditModal",
            scriptUrl: abp.appPath + "Pages/DiversityDatas/editModal.js",
            modalClass: "diversityDataEdit"
        });

        var diversityDataDataTable = $("#DiversityDatasTable-" + jobApplicationId).DataTable(abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: true,
            paging: true,
            searching: false,
            scrollX: true,
            autoWidth: true,
            scrollCollapse: true,
            order: [[1, "asc"]],
            ajax: abp.libs.datatables.createAjax(diversityDataService.getListByJobApplicationId, {
                jobApplicationId: jobApplicationId,
                maxResultCount: 5
            }),
            columnDefs: [
                {
                    rowAction: {
                        items:
                            [
                                {
                                    text: l("Edit"),
                                    visible: abp.auth.isGranted('RockIT.DiversityDatas.Edit'),
                                    action: function (data) {
                                        diversityDataEditModal.open({
                                            id: data.record.id
                                        });
                                    }
                                },
                                {
                                    text: l("Delete"),
                                    visible: abp.auth.isGranted('RockIT.DiversityDatas.Delete'),
                                    confirmMessage: function () {
                                        return l("DeleteConfirmationMessage");
                                    },
                                    action: function (data) {
                                        diversityDataService.delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l("SuccessfullyDeleted"));
                                                diversityDataDataTable.ajax.reloadEx();
                                            });
                                    }
                                }
                            ]
                    },
                    width: "1rem"
                },
                {
                data: "happyToCompleteForm",
                render: function (happyToCompleteForm) {
                    if (happyToCompleteForm === undefined ||
                        happyToCompleteForm === null) {
                        return "";
                    }

                    var localizationKey = "Enum:YesNo." + happyToCompleteForm;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            },
            {
                data: "ageRange",
                render: function (ageRange) {
                    if (ageRange === undefined ||
                        ageRange === null) {
                        return "";
                    }

                    var localizationKey = "Enum:AgeRange." + ageRange;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            },
            {
                data: "gender",
                render: function (gender) {
                    if (gender === undefined ||
                        gender === null) {
                        return "";
                    }

                    var localizationKey = "Enum:GenderOrSex." + gender;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            },
			{ data: "otherGender" },
            {
                data: "genderIdentitySameAsBirth",
                render: function (genderIdentitySameAsBirth) {
                    if (genderIdentitySameAsBirth === undefined ||
                        genderIdentitySameAsBirth === null) {
                        return "";
                    }

                    var localizationKey = "Enum:YesNoPreferNotToSay." + genderIdentitySameAsBirth;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            },
            {
                data: "sex",
                render: function (sex) {
                    if (sex === undefined ||
                        sex === null) {
                        return "";
                    }

                    var localizationKey = "Enum:GenderOrSex." + sex;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            },
			{ data: "otherSex" },
            {
                data: "sexualOrientation",
                render: function (sexualOrientation) {
                    if (sexualOrientation === undefined ||
                        sexualOrientation === null) {
                        return "";
                    }

                    var localizationKey = "Enum:SexualOrientation." + sexualOrientation;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            },
			{ data: "otherSexualOrientation" },
            {
                data: "ethnicity",
                render: function (ethnicity) {
                    if (ethnicity === undefined ||
                        ethnicity === null) {
                        return "";
                    }

                    var localizationKey = "Enum:Ethnicity." + ethnicity;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            },
			{ data: "otherEthnicity" },
            {
                data: "religionOrBelief",
                render: function (religionOrBelief) {
                    if (religionOrBelief === undefined ||
                        religionOrBelief === null) {
                        return "";
                    }

                    var localizationKey = "Enum:Religion." + religionOrBelief;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            },
			{ data: "otherReligionOrBelief" },
            {
                data: "disability",
                render: function (disability) {
                    if (disability === undefined ||
                        disability === null) {
                        return "";
                    }

                    var localizationKey = "Enum:YesNoPreferNotToSay." + disability;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            },
            {
                data: "educationLevel",
                render: function (educationLevel) {
                    if (educationLevel === undefined ||
                        educationLevel === null) {
                        return "";
                    }

                    var localizationKey = "Enum:EducationLevel." + educationLevel;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            },
            {
                data: "typeOfSecondarySchool",
                render: function (typeOfSecondarySchool) {
                    if (typeOfSecondarySchool === undefined ||
                        typeOfSecondarySchool === null) {
                        return "";
                    }

                    var localizationKey = "Enum:TypeOfSecondarySchool." + typeOfSecondarySchool;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            },
			{ data: "otherTypeOfSecondarySchool" },
            {
                data: "higherEducationQualifications",
                render: function (higherEducationQualifications) {
                    if (higherEducationQualifications === undefined ||
                        higherEducationQualifications === null) {
                        return "";
                    }

                    var localizationKey = "Enum:YesNoPreferNotToSayDontKnow." + higherEducationQualifications;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            }
            ]
        }));

        diversityDataCreateModal.onResult(function () {
            diversityDataDataTable.ajax.reloadEx();
        });

        diversityDataEditModal.onResult(function () {
            diversityDataDataTable.ajax.reloadEx();
        });

        $("button.NewDiversityDataButton").off("click").on("click", function (e) {
            diversityDataCreateModal.open({
                jobApplicationId: $(this).data("jobapplication-id")
            });
        });
    }
    function initJobFormResponseGrid(data) {
        if(!abp.auth.isGranted("RockIT.JobFormResponses")) {
            return;
        }
        
        var jobApplicationId = data.jobApplication.id;

        
        var jobFormResponseService = window.steer73.rockIT.jobFormResponses.jobFormResponses;

        var jobFormResponseCreateModal = new abp.ModalManager({
            viewUrl: abp.appPath + "JobFormResponses/CreateModal",
            scriptUrl: abp.appPath + "Pages/JobFormResponses/createModal.js",
            modalClass: "jobFormResponseCreate"
        });

        var jobFormResponseEditModal = new abp.ModalManager({
            viewUrl: abp.appPath + "JobFormResponses/EditModal",
            scriptUrl: abp.appPath + "Pages/JobFormResponses/editModal.js",
            modalClass: "jobFormResponseEdit"
        });

        var jobFormResponseDataTable = $("#JobFormResponsesTable-" + jobApplicationId).DataTable(abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: true,
            paging: true,
            searching: false,
            scrollX: true,
            autoWidth: true,
            scrollCollapse: true,
            order: [[1, "asc"]],
            ajax: abp.libs.datatables.createAjax(jobFormResponseService.getListByJobApplicationId, {
                jobApplicationId: jobApplicationId,
                maxResultCount: 5
            }),
            columnDefs: [
                {
                    rowAction: {
                        items:
                            [
                                {
                                    text: l("Edit"),
                                    visible: abp.auth.isGranted('RockIT.JobFormResponses.Edit'),
                                    action: function (data) {
                                        jobFormResponseEditModal.open({
                                            id: data.record.id
                                        });
                                    }
                                },
                                {
                                    text: l("Delete"),
                                    visible: abp.auth.isGranted('RockIT.JobFormResponses.Delete'),
                                    confirmMessage: function () {
                                        return l("DeleteConfirmationMessage");
                                    },
                                    action: function (data) {
                                        jobFormResponseService.delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l("SuccessfullyDeleted"));
                                                jobFormResponseDataTable.ajax.reloadEx();
                                            });
                                    }
                                }
                            ]
                    },
                    width: "1rem"
                },
                { data: "formStructureJson" },
			{ data: "formResponseJson" }
            ]
        }));

        jobFormResponseCreateModal.onResult(function () {
            jobFormResponseDataTable.ajax.reloadEx();
        });

        jobFormResponseEditModal.onResult(function () {
            jobFormResponseDataTable.ajax.reloadEx();
        });

        $("button.NewJobFormResponseButton").off("click").on("click", function (e) {
            jobFormResponseCreateModal.open({
                jobApplicationId: $(this).data("jobapplication-id")
            });
        });
    }
    function initDiversityFormResponseGrid(data) {
        if(!abp.auth.isGranted("RockIT.DiversityFormResponses")) {
            return;
        }
        
        var jobApplicationId = data.jobApplication.id;

        
        var diversityFormResponseService = window.steer73.rockIT.diversityFormResponses.diversityFormResponses;

        var diversityFormResponseCreateModal = new abp.ModalManager({
            viewUrl: abp.appPath + "DiversityFormResponses/CreateModal",
            scriptUrl: abp.appPath + "Pages/DiversityFormResponses/createModal.js",
            modalClass: "diversityFormResponseCreate"
        });

        var diversityFormResponseEditModal = new abp.ModalManager({
            viewUrl: abp.appPath + "DiversityFormResponses/EditModal",
            scriptUrl: abp.appPath + "Pages/DiversityFormResponses/editModal.js",
            modalClass: "diversityFormResponseEdit"
        });

        var diversityFormResponseDataTable = $("#DiversityFormResponsesTable-" + jobApplicationId).DataTable(abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: true,
            paging: true,
            searching: false,
            scrollX: true,
            autoWidth: true,
            scrollCollapse: true,
            order: [[1, "asc"]],
            ajax: abp.libs.datatables.createAjax(diversityFormResponseService.getListByJobApplicationId, {
                jobApplicationId: jobApplicationId,
                maxResultCount: 5
            }),
            columnDefs: [
                {
                    rowAction: {
                        items:
                            [
                                {
                                    text: l("Edit"),
                                    visible: abp.auth.isGranted('RockIT.DiversityFormResponses.Edit'),
                                    action: function (data) {
                                        diversityFormResponseEditModal.open({
                                            id: data.record.id
                                        });
                                    }
                                },
                                {
                                    text: l("Delete"),
                                    visible: abp.auth.isGranted('RockIT.DiversityFormResponses.Delete'),
                                    confirmMessage: function () {
                                        return l("DeleteConfirmationMessage");
                                    },
                                    action: function (data) {
                                        diversityFormResponseService.delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l("SuccessfullyDeleted"));
                                                diversityFormResponseDataTable.ajax.reloadEx();
                                            });
                                    }
                                }
                            ]
                    },
                    width: "1rem"
                },
                { data: "formStructureJson" },
			{ data: "formResponseJson" }
            ]
        }));

        diversityFormResponseCreateModal.onResult(function () {
            diversityFormResponseDataTable.ajax.reloadEx();
        });

        diversityFormResponseEditModal.onResult(function () {
            diversityFormResponseDataTable.ajax.reloadEx();
        });

        $("button.NewDiversityFormResponseButton").off("click").on("click", function (e) {
            diversityFormResponseCreateModal.open({
                jobApplicationId: $(this).data("jobapplication-id")
            });
        });
    }
    
    
});
