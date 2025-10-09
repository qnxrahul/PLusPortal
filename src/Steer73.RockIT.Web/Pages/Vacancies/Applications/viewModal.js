var abp = abp || {};

//<suite-custom-code-block-1>
//</suite-custom-code-block-1>

abp.modals.viewModal = function () {
    var initModal = function (publicApi, args) {

        var l = abp.localization.getResource("RockIT");
        var jobApplicationService = window.steer73.rockIT.jobApplications.jobApplications;

        $("body").off("click", "button.download-selected-file-btn");
        $("body").on("click", "button.download-selected-file-btn", function (e) {
            e.preventDefault();
            var fileType = $(this).data("file-type");
            var jobApplicationId = $(this).data("job-application-id");

            jobApplicationService.getDownloadToken().then(
                function (result) {
                    var url = abp.appPath + 'api/app/job-applications/file-by-type' +
                        abp.utils.buildQueryString([
                            { name: 'downloadToken', value: result.token },
                            { name: 'jobApplicationId', value: jobApplicationId },
                            { name: 'FileType', value: fileType }
                        ]);

                    var downloadWindow = window.open(url, '_blank');
                    downloadWindow.focus();
                }
            )
        });

        $('.js-confirm-approve button[type="submit"]').on('click', function (e) {
            e.preventDefault();
            return abp.message.confirm(l('ConfirmApproveReject').replace("{0}", $('.js-confirm-approve').data('candidateAction')))
                .then(function (confirmed) {
                    if (confirmed) {
                        $('.js-confirm-approve').ajaxSubmit({
                            success: function (responseText, statusText, xhr, form) {
                                publicApi.close();
                                publicApi.setResult();
                            }
                        });
                    }
                });
        });

        $('.js-confirm-reject button[type="submit"]').on('click', function (e) {
            e.preventDefault();
            return abp.message.confirm(l('ConfirmApproveReject').replace("{0}", $('.js-confirm-reject').data('candidateAction')))
                .then(function (confirmed) {
                    if (confirmed) {
                        $('.js-confirm-reject').ajaxSubmit({
                            success: function (responseText, statusText, xhr, form) {
                                publicApi.close();
                                publicApi.setResult();
                            }
                        });
                    }
            });
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