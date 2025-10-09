$(function () {
	var l = abp.localization.getResource("RockIT");

    var jobApplicationService = window.steer73.rockIT.jobApplications.jobApplications;

    var viewModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Vacancies/Applications/ViewModal",
        scriptUrl: abp.appPath + "Pages/Vacancies/Applications/ViewModal.js",
        modalClass: "viewModal"
    });

    var dataTableColumns = [
        {
            rowAction: {
                items:
                    [
                        {
                            text: l("ViewApplicant"),
                            visible: abp.auth.isGranted('RockIT.JobApplications'),
                            action: function (data) {
                                viewModal.open({
                                    id: data.record.jobApplication.id
                                });
                            }
                        }
                    ]
            },
            width: "1rem"
        },
        {
            orderable: false,
            render: DataTable.render.select(),
            targets: 0
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
            data: "jobApplication.cvUrl",
            render: function (cvUrl, _type, row) {
                if (!cvUrl || cvUrl === "") {
                    return "-";
                }

                return "<a href='/api/app/job-applications/file-by-type?fileType=CV&jobApplicationId=" + row.jobApplication.id + "'><span class='jobApplication-c-v-file' style='cursor: pointer;'><i class='fa fa-file'></i></span></a>";
            }
        },
        {
            data: "jobApplication.coverLetterUrl",
            render: function (coverLetterUrl, _type, row) {
                if (!coverLetterUrl || coverLetterUrl === "") {
                    return "-";
                }

                return "<a href='/api/app/job-applications/file-by-type?fileType=CoverLetter&jobApplicationId=" + row.jobApplication.id + "'><span class='jobApplication-cover-letter-file' style='cursor: pointer;'><i class='fa fa-file'></i></span></a>";
            }
        },
        {
            data: "jobApplication.additionalDocumentUrl",
            render: function (additionalDocumentUrl, _type, row) {
                if (!additionalDocumentUrl || additionalDocumentUrl === "") {
                    return "-";
                }

                return "<a href='/api/app/job-applications/file-by-type?fileType=AdditionalDocument&jobApplicationId=" + row.jobApplication.id + "'><span class='jobApplication-additional-document-file' style='cursor: pointer;'><i class='fa fa-file'></i></span></a>";
            }
        },
        {
            data: "vacancy.title",
            defaultContent: ""
        },
        {
            data: "jobApplication.creationTime",
            render: DataTable.render.datetime('yyyy-MM-DD HH:mm')

        }

    ];
    var getFilter = function () {
        return {
            filterText: $("#FilterText").val(),
            firstName: null,
            lastName: null,
            emailAddress: null,
            title: null,
            phoneNumber: null,
            landline: null,
            currentRole: null,
            currentCompany: null,
            currentPositionType: null,
            vacancyId: $("#VacancyId").val()
        };
    };

    var dataTable = $("#JobApplicationsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [],
        dom: 'Bft<"row dataTable_footer"<"col-auto"l><"col-auto me-auto"i><"col-auto"p>>',
        ajax: abp.libs.datatables.createAjax(jobApplicationService.getList, getFilter),
        columnDefs: dataTableColumns,
        rowId: 'jobApplication.id',
        select: {
            style: 'multi'
        },
        buttons: [
            {
                extend: 'selected',
                text: 'Approve',
                action: function (e, dt, node, config) {

                    return abp.message.confirm(l('ConfirmApplicationsApproval'))
                        .then(function (confirmed) {
                            if (!confirmed) { return; }

                            abp.ui.setBusy();
                            const jobApplications = [];
                            dt.rows({ selected: true }).every(function () {
                                const row = this.data();
                                if (row.jobApplication.statusAsString == 'Pending') {
                                    jobApplications.push(
                                        {
                                            id: row.jobApplication.id,
                                            concurrencyStamp: row.jobApplication.concurrencyStamp
                                        }
                                    );
                                }
                            });

                            if (!jobApplications.length)
                            {
                                abp.ui.clearBusy();
                                return;
                            }

                            jobApplicationService.approveMany(jobApplications)
                                .then(function () {
                                    abp.notify.info(l("ApplicationsApproved"));
                                    dt.rows().deselect();
                                    dt.ajax.reload();
                                    abp.ui.clearBusy();
                                })
                                .catch(function () {
                                    abp.ui.clearBusy();
                                });
                        });
                }
            }
        ]
    }));

  
    viewModal.onResult(function () {
        dataTable.ajax.reloadEx();;
    });

    $("#SearchForm").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reloadEx();

    });
});