var abp = abp || {};

//<suite-custom-code-block-1>
//</suite-custom-code-block-1>

abp.modals.jobApplicationEdit = function () {
    var initModal = function (publicApi, args) {
        var l = abp.localization.getResource("RockIT");
        
        var jobApplicationService = window.steer73.rockIT.jobApplications.jobApplications;
        
        
        $("#RemoveSelectedjobApplicationCV").click(function (e) {
            e.preventDefault();

            var $jobApplicationcVInput = $("#JobApplication_CVId");
            $jobApplicationcVInput.val(null);
            $("#jobApplicationcVManager").addClass("d-none");
            $("#jobApplicationcVInputGroup").removeClass("d-none");

            if($jobApplicationcVInput.prop("required")) {
                $("#jobApplicationcVFile").attr("required", true);
            }
        });

        $("#jobApplicationcVFile").change(async function (e) {
            abp.ui.setBusy();

            var $jobApplicationcVInput = $("#JobApplication_CVId");
            var $fileInput = $(this);
            var file = $fileInput[0].files[0];

            if(file) {
                var formData = new FormData();
                formData.append("input", file);

                $.ajax({
                    url: abp.appPath + 'api/app/job-applications/upload-file',
                    data: formData,
                    type: 'POST',
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        $jobApplicationcVInput.val(data.id);
                    },
                    error: function (data) {
                        abp.message.error(l("UploadFailedMessage"));
                        $jobApplicationcVInput.val(null);
                        $fileInput.val(null);
                    }
                });
            }
            else {
                $jobApplicationcVInput.val(null);
                $fileInput.val(null);
            }

            abp.ui.clearBusy();
        });

        $("#RemoveSelectedjobApplicationCoverLetter").click(function (e) {
            e.preventDefault();

            var $jobApplicationcoverLetterInput = $("#JobApplication_CoverLetterId");
            $jobApplicationcoverLetterInput.val(null);
            $("#jobApplicationcoverLetterManager").addClass("d-none");
            $("#jobApplicationcoverLetterInputGroup").removeClass("d-none");

            if($jobApplicationcoverLetterInput.prop("required")) {
                $("#jobApplicationcoverLetterFile").attr("required", true);
            }
        });

        $("#jobApplicationcoverLetterFile").change(async function (e) {
            abp.ui.setBusy();

            var $jobApplicationcoverLetterInput = $("#JobApplication_CoverLetterId");
            var $fileInput = $(this);
            var file = $fileInput[0].files[0];

            if(file) {
                var formData = new FormData();
                formData.append("input", file);

                $.ajax({
                    url: abp.appPath + 'api/app/job-applications/upload-file',
                    data: formData,
                    type: 'POST',
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        $jobApplicationcoverLetterInput.val(data.id);
                    },
                    error: function (data) {
                        abp.message.error(l("UploadFailedMessage"));
                        $jobApplicationcoverLetterInput.val(null);
                        $fileInput.val(null);
                    }
                });
            }
            else {
                $jobApplicationcoverLetterInput.val(null);
                $fileInput.val(null);
            }

            abp.ui.clearBusy();
        });

        $("#RemoveSelectedjobApplicationAdditionalDocument").click(function (e) {
            e.preventDefault();

            var $jobApplicationadditionalDocumentInput = $("#JobApplication_AdditionalDocumentId");
            $jobApplicationadditionalDocumentInput.val(null);
            $("#jobApplicationadditionalDocumentManager").addClass("d-none");
            $("#jobApplicationadditionalDocumentInputGroup").removeClass("d-none");

            if($jobApplicationadditionalDocumentInput.prop("required")) {
                $("#jobApplicationadditionalDocumentFile").attr("required", true);
            }
        });

        $("#jobApplicationadditionalDocumentFile").change(async function (e) {
            abp.ui.setBusy();

            var $jobApplicationadditionalDocumentInput = $("#JobApplication_AdditionalDocumentId");
            var $fileInput = $(this);
            var file = $fileInput[0].files[0];

            if(file) {
                var formData = new FormData();
                formData.append("input", file);

                $.ajax({
                    url: abp.appPath + 'api/app/job-applications/upload-file',
                    data: formData,
                    type: 'POST',
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        $jobApplicationadditionalDocumentInput.val(data.id);
                    },
                    error: function (data) {
                        abp.message.error(l("UploadFailedMessage"));
                        $jobApplicationadditionalDocumentInput.val(null);
                        $fileInput.val(null);
                    }
                });
            }
            else {
                $jobApplicationadditionalDocumentInput.val(null);
                $fileInput.val(null);
            }

            abp.ui.clearBusy();
        });

        
        $("body").on("click", "button.download-selected-file-btn", function (e) {
          e.preventDefault();
          
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
        
        
        
    };
    
    //<suite-custom-code-block-2>
    //</suite-custom-code-block-2>

    return {
        initModal: initModal
    };
};

//<suite-custom-code-block-3>
//</suite-custom-code-block-3>