$(function () {
    var l = abp.localization.getResource("RockIT");
        
    var vacancyService = window.steer73.rockIT.vacancies.vacancies;
    var formDefinitionService = window.steer73.rockIT.formDefinitions.formDefinitions;
        
        
    $("#RemoveSelectedvacancyBrochureFile").click(function (e) {
        e.preventDefault();

        var $vacancybrochureFileInput = $("#Input_BrochureFileId");
        $vacancybrochureFileInput.val(null);
        $("#vacancybrochureFileManager").addClass("d-none");
        $("#vacancybrochureFileInputGroup").removeClass("d-none");

        if($vacancybrochureFileInput.prop("required")) {
            $("#vacancybrochureFileFile").attr("required", true);
        }
    });

    $("#vacancybrochureFileFile").change(async function (e) {
        abp.ui.setBusy();
        var $vacancybrochureFileInput = $("#Input_BrochureFileId");
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

    $("#RemoveSelectedvacancyAdditionalFile").click(function (e) {
        e.preventDefault();

        var $vacancyadditionalFileInput = $("#Input_AdditionalFileId");
        $vacancyadditionalFileInput.val(null);
        $("#vacancyadditionalFileManager").addClass("d-none");
        $("#vacancyadditionalFileInputGroup").removeClass("d-none");

        if($vacancyadditionalFileInput.prop("required")) {
            $("#vacancyadditionalFileFile").attr("required", true);
        }
    });

    $("#vacancyadditionalFileFile").change(async function (e) {
        abp.ui.setBusy();

        var $vacancyadditionalFileInput = $("#Input_AdditionalFileId");
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

        
    $("body").on("click", "button.download-selected-file-btn", function (e) {
        e.preventDefault();
          
        var fileId = $(this).data("file-id");

        vacancyService.getDownloadToken().then(
            function(result){
                var url =  abp.appPath + 'api/app/vacancies/file' +
                    abp.utils.buildQueryString([
                        { name: 'downloadToken', value: result.token },
                        { name: 'fileId', value: fileId }
                    ]);

                var downloadWindow = window.open(url, '_blank');
                downloadWindow.focus();
            }
        )
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

    $("#Input_CompanyId").change(async function () {

        $("#Input_VacancyFormDefinitionId").val('');
        $("#Input_DiversityFormDefinitionId").val('');

        $("#Input_VacancyFormDefinitionId> option").each(function () {
            let optionValue = $(this).attr('value');
            if (optionValue !== null && optionValue !== 'undefined' && optionValue !== '') {
                $(this).prop('selected', false);
                $(this).prop('disabled', true);
            }
        });

        $("#Input_DiversityFormDefinitionId").children("option").each(function () {
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

                    $("#Input_VacancyFormDefinitionId > option").each(function () {
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
                    $("#Input_DiversityFormDefinitionId").children("option").each(function () {
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
    //<suite-custom-code-block-2>
    //</suite-custom-code-block-2>
});

//<suite-custom-code-block-3>
//</suite-custom-code-block-3>