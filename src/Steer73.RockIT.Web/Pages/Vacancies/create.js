let vacancyReferenceModal = null;

$(function () {

    var formDefinitionService = window.steer73.rockIT.formDefinitions.formDefinitions;
    var vacancyService = window.steer73.rockIT.vacancies.vacancies;

        var l = abp.localization.getResource("RockIT");
        
                $("#vacancybrochureFileFile").change(async function (e) {
            abp.ui.setBusy();
            
            var $vacancybrochureFileInput = $("#Vacancy_BrochureFileId");
            var $fileInput = $(this);
            var file = $fileInput[0].files[0];

            if(file) {
                var formData = new FormData();
                formData.append("input", file);
                
                $.ajax({
                    url: abp.appPath + 'api/app/vacancies/upload-file',
                    data: formData,
                    type: 'POST',
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        $vacancybrochureFileInput.val(data.id);
                        abp.ui.clearBusy();
                    },
                    error: function (data) {
                        abp.message.error(l("UploadFailedMessage"));
                        $vacancybrochureFileInput.val(null);
                        $fileInput.val(null);
                        abp.ui.clearBusy();
                    }
                });
            }
            else {
                $vacancybrochureFileInput.val(null);
                $fileInput.val(null);
                abp.ui.clearBusy();
            }
        });
        $("#vacancyadditionalFileFile").change(async function (e) {
            abp.ui.setBusy();
            
            var $vacancyadditionalFileInput = $("#Vacancy_AdditionalFileId");
            var $fileInput = $(this);
            var file = $fileInput[0].files[0];

            if(file) {
                var formData = new FormData();
                formData.append("input", file);
                
                $.ajax({
                    url: abp.appPath + 'api/app/vacancies/upload-file',
                    data: formData,
                    type: 'POST',
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        $vacancyadditionalFileInput.val(data.id);
                        abp.ui.clearBusy();
                    },
                    error: function (data) {
                        abp.message.error(l("UploadFailedMessage"));
                        $vacancyadditionalFileInput.val(null);
                        $fileInput.val(null);
                        abp.ui.clearBusy();
                    }
                });
            }
            else {
                $vacancyadditionalFileInput.val(null);
                $fileInput.val(null);
                abp.ui.clearBusy();
            }
        });

        
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

        $(".js-clear-file").click(function (e) {
            let input = document.getElementById($(this).data("input-id"));
            let fileIdInput = document.getElementById($(this).data("file-id"));

            return abp.message.confirm()
                .then(function (confirmed) {
                    if (confirmed) {
                        input.value = '';
                        fileIdInput.value = '';
                    }
                });
        });

        $("#Vacancy_CompanyId").change(async function () {

            $("#Vacancy_VacancyFormDefinitionId").val('');
            $("#Vacancy_DiversityFormDefinitionId").val('');

            $("#Vacancy_VacancyFormDefinitionId> option").each(function () {
                let optionValue = $(this).attr('value');
                if (optionValue !== null && optionValue !== 'undefined' && optionValue !== '') {
                    $(this).prop('selected', false);
                    $(this).prop('disabled', true);
                }
            });

            $("#Vacancy_DiversityFormDefinitionId").children("option").each(function () {
                let optionValue = $(this).attr('value');
                if (optionValue !== null && optionValue !== 'undefined' && optionValue !== '') {
                    $(this).prop('selected', false);
                    $(this).prop('disabled', true);
                }
            });

            let selectedCompany = $(this).val();
            if (selectedCompany !== null && selectedCompany !== 'undefined' && selectedCompany !== '') {
                let filters = {
                    companyId: selectedCompany
                };
                formDefinitionService.getList(filters)
                    .then(function (result) {

                        if (result === null || result.items === null || result.items.length === 0) {
                            return;
                        }

                        let listOfAllowedForms = new Array();
                        result.items.forEach(function (arrayItem) {
                            listOfAllowedForms.push(arrayItem.formDefinition.id);
                        });

                        $("#Vacancy_VacancyFormDefinitionId > option").each(function () {
                            let optionValue = $(this).attr('value');

                            if (optionValue === null || optionValue === 'undefined' || optionValue === '') {
                                return;
                            }

                            let checkResult = true;
                            listOfAllowedForms.forEach(function (arrayItem) {
                                if (arrayItem === optionValue) {
                                    checkResult = false;
                                }
                            });

                            $(this).prop('disabled', checkResult);
                        });
                        $("#Vacancy_DiversityFormDefinitionId").children("option").each(function () {
                            let optionValue = $(this).attr('value');
                            if (optionValue === null || optionValue === 'undefined' || optionValue === '') {
                                return;
                            }

                            let checkResult = true;
                            listOfAllowedForms.forEach(function (arrayItem) {
                                if (arrayItem === optionValue) {
                                    checkResult = false;
                                }
                            });

                            $(this).prop('disabled', checkResult);
                        });

                    });
            }
        });

    let recentProjectSearched = null;
    vacancyReferenceModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Shared/VacancyReferenceModal",
        className: 'VacancyReference'
    });

    $('#searchProjectId').on('click', function (e) {
        abp.ui.setBusy();
        recentProjectSearched = null;

        vacancyService.getProjectById($('#projectId').val())
            .then(function (res) {
                recentProjectSearched = res;
                vacancyReferenceModal.open({
                    id: `${recentProjectSearched.ezekiaProject?.id}`,
                    projectId: `${recentProjectSearched.ezekiaProject?.projectId}`,
                    projectName: `${recentProjectSearched.ezekiaProject?.additionalProperties?.name ?? ''}`,
                    companyName: `${recentProjectSearched.ezekiaProject?.relationships?.company?.name ?? ''}`,
                })
                abp.ui.clearBusy();
            })
            .catch(function () {
                abp.ui.clearBusy();
            });
    });

    vacancyReferenceModal.onResult(res => {
        if (res && recentProjectSearched) {
            if ($("#Vacancy_ExternalRefId").val()) {
                abp.message.confirm(l("OverwriteProjectSelection"))
                    .then(function (confirmed) {
                        if (confirmed) {
                            setProjectDetails();
                        }
                    });
            } else {
                setProjectDetails();
            }
        }
        vacancyReferenceModal.close();

    });

    function setProjectDetails() {
        $("#Vacancy_ExternalRefId").val(recentProjectSearched.ezekiaProject?.id);
        $("#Vacancy_ProjectId").val(recentProjectSearched.ezekiaProject?.projectId);
        $("#Vacancy_Title").val(recentProjectSearched.ezekiaProject?.additionalProperties?.name);

        if (recentProjectSearched?.ezekiaProject?.relationships?.company && recentProjectSearched?.companyId) {
            var companySelect2 = $("#Vacancy_CompanyId");
            var option = new Option(recentProjectSearched?.ezekiaProject?.relationships?.company.name, recentProjectSearched?.companyId, true, true);
            companySelect2.append(option).trigger('change');

            // manually trigger the `select2:select` event
            companySelect2.trigger({
                type: 'select2:select',
                params: {
                    data: {
                        id: recentProjectSearched?.companyId,
                        name: recentProjectSearched?.ezekiaProject?.relationships?.company.name
                    }
                }
            });
        }
    }
});
