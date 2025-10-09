$(function () {
    var l = abp.localization.getResource("RockIT");

    var vacancyService = window.steer73.rockIT.vacancies.vacancies;

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

    var getFilter = function () {
        return {
            filterText: $("#FilterText").val(),
            title: $("#TitleFilter").val(),
            reference: $("#ReferenceFilter").val(),
            region: $("#RegionFilter").val(),
            role: $("#RoleFilter").val(),
            benefits: $("#BenefitsFilter").val(),
            location: $("#LocationFilter").val(),
            salary: $("#SalaryFilter").val(),
            description: $("#DescriptionFilter").val(),
            formalInterviewDateMin: $("#FormalInterviewDateFilterMin").val(),
            formalInterviewDateMax: $("#FormalInterviewDateFilterMax").val(),
            secondInterviewDateMin: $("#SecondInterviewDateFilterMin").val(),
            secondInterviewDateMax: $("#SecondInterviewDateFilterMax").val(),
            externalPostingDateMin: $("#ExternalPostingDateFilterMin").val(),
            externalPostingDateMax: $("#ExternalPostingDateFilterMax").val(),
            closingDateMin: $("#ClosingDateFilterMin").val(),
            closingDateMax: $("#ClosingDateFilterMax").val(),
            expiringDateMin: $("#ExpiringDateFilterMin").val(),
            expiringDateMax: $("#ExpiringDateFilterMax").val(),
            showDiversity: (function () {
                var value = $("#ShowDiversityFilter").val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })(),
            companyId: $("#CompanyIdFilter").val(),
            identityUserId: $("#IdentityUserIdFilter").val(),
            practiceGroupId: $("#PracticeGroupIdFilter").val(),
            vacancyFormDefinitionId: $("#VacancyFormDefinitionIdFilter").val(),
            diversityFormDefinitionId: $("#DiversityFormDefinitionIdFilter").val(),
            status: $("#StatusFilter").val(),
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
                            visible: abp.auth.isGranted('RockIT.Vacancies.Edit'),
                            action: function (data) {
                                location.href = "/Vacancies/" + data.record.vacancy.id + "/Edit";
                            }
                        },
                        {
                            text: l("Delete"),
                            visible: abp.auth.isGranted('RockIT.Vacancies.Delete'),
                            confirmMessage: function () {
                                return l("DeleteConfirmationMessage");
                            },
                            action: function (data) {
                                vacancyService.delete(data.record.vacancy.id)
                                    .then(function () {
                                        abp.notify.success(l("SuccessfullyDeleted"));
                                        dataTable.ajax.reloadEx();;
                                    });
                            }
                        },
                        {
                            text: l("ViewApplications"),
                            visible: abp.auth.isGranted('RockIT.JobApplications'),
                            action: function (data) {
                                location.href = "/Vacancies/Applications/Index?id=" + data.record.vacancy.id;
                            }
                        }
                    ]
            },
            width: "1rem"
        },
        { data: "vacancy.title" },
        { data: "vacancy.vacancyStatus", orderable: false },
        {
            data: "vacancy.creationTime",
            render: DataTable.render.datetime('yyyy-MM-DD HH:mm')
        },
        { data: "vacancy.reference" },
        {
            data: "vacancy.regionDescriptions",
            orderable: false,
            render: function (regions) {
                return regions.join(', ');
            }
        },
        { data: "vacancy.role" },
        { data: "vacancy.benefits" },
        { data: "vacancy.location" },
        { data: "vacancy.salary" },
        { data: "vacancy.linkedInUrl" },
        { data: "vacancy.formalInterviewDate" },
        { data: "vacancy.secondInterviewDate" },
        { data: "vacancy.externalPostingDate" },
        { data: "vacancy.closingDate" },
        { data: "vacancy.expiringDate" },
        {
            data: "vacancy.brochureFileId",
            render: function (brochureFileId) {
                if (!brochureFileId || brochureFileId === "00000000-0000-0000-0000-000000000000") {
                    return "-";
                }

                return "<span class='vacancy-brochure-file-file' style='cursor: pointer;' data-file-id='" + brochureFileId + "'><i class='fa fa-file'></i></span>";
            }
        },
        {
            data: "vacancy.additionalFileId",
            render: function (additionalFileId) {
                if (!additionalFileId || additionalFileId === "00000000-0000-0000-0000-000000000000") {
                    return "-";
                }

                return "<span class='vacancy-additional-file-file' style='cursor: pointer;' data-file-id='" + additionalFileId + "'><i class='fa fa-file'></i></span>";
            }
        },
        {
            data: "vacancy.showDiversity",
            render: function (showDiversity) {
                return showDiversity ? '<i class="fa fa-check"></i>' : '<i class="fa fa-times"></i>';
            }
        },
        {
            data: "company.name",
            defaultContent: ""
        },
        {
            data: "identityUser.name",
            defaultContent: ""
        },
        {
            data: "practiceGroup.name",
            defaultContent: ""
        },
        {
            data: "formDefinition.name",
            defaultContent: ""
        },
        {
            data: "formDefinition1.name",
            defaultContent: ""
        },
        { data: "vacancy.externalRefId" }
    ];


    $("body").on("click", "span.vacancy-brochure-file-file", function () {
        var fileId = $(this).data("file-id");




        vacancyService.getDownloadToken().then(
            function (result) {
                var url = abp.appPath + 'api/app/vacancies/file' +
                    abp.utils.buildQueryString([
                        { name: 'downloadToken', value: result.token },
                        { name: 'fileId', value: fileId }
                    ]);

                var downloadWindow = window.open(url, '_blank');
                downloadWindow.focus();
            }
        )
    });


    $("body").on("click", "span.vacancy-additional-file-file", function () {
        var fileId = $(this).data("file-id");




        vacancyService.getDownloadToken().then(
            function (result) {
                var url = abp.appPath + 'api/app/vacancies/file' +
                    abp.utils.buildQueryString([
                        { name: 'downloadToken', value: result.token },
                        { name: 'fileId', value: fileId }
                    ]);

                var downloadWindow = window.open(url, '_blank');
                downloadWindow.focus();
            }
        )
    });





    var dataTable = $("#VacanciesTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[3, "desc"]],
        ajax: abp.libs.datatables.createAjax(vacancyService.getListForCurrentUser, getFilter),
        columnDefs: dataTableColumns
    }));



    //<suite-custom-code-block-2>
    //</suite-custom-code-block-2>

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
