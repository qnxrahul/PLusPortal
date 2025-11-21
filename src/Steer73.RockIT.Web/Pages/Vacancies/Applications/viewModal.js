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

        var abpUiResource = abp.localization.getResource("AbpUi");
        var getCandidateActionError = function (xhr) {
            var fallback = abpUiResource
                ? abpUiResource("UnhandledException")
                : "An unexpected error has occurred.";

            if (!xhr) {
                return fallback;
            }

            var response = xhr.responseJSON;
            if (response && response.error) {
                if (response.error.message && response.error.details) {
                    return response.error.message + " - " + response.error.details;
                }

                return response.error.message || response.error.details || fallback;
            }

            if (response && response.message) {
                return response.message;
            }

            return fallback;
        };

        var bindCandidateAction = function (formSelector) {
            var $form = $(formSelector);

            $form.find('button[type="submit"]').on('click', function (e) {
                e.preventDefault();

                var $button = $(this);
                var action = $form.data('candidateAction');

                return abp.message.confirm(l('ConfirmApproveReject').replace("{0}", action))
                    .then(function (confirmed) {
                        if (!confirmed) {
                            return;
                        }

                        abp.ui.setBusy($form);
                        $button.prop('disabled', true);

                        $form.ajaxSubmit({
                            success: function () {
                                publicApi.close();
                                publicApi.setResult();
                            },
                            error: function (xhr) {
                                abp.notify.error(getCandidateActionError(xhr));
                            },
                            complete: function () {
                                abp.ui.clearBusy($form);
                                $button.prop('disabled', false);
                            }
                        });
                    });
            });
        };

        bindCandidateAction('.js-confirm-approve');
        bindCandidateAction('.js-confirm-reject');
    };

    //<suite-custom-code-block-2>
    //</suite-custom-code-block-2>

    return {
        initModal: initModal
    };
};

//<suite-custom-code-block-3>
//</suite-custom-code-block-3>