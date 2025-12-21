$(function () {
    var l = abp.localization.getResource("RockIT");
    var jobAlertRegistrationService = window.steer73.rockIT.jobAlerts.jobAlertRegistrations.jobAlertRegistrations;

    var getFilter = function () {
        var status = $("#StatusFilter").val();
        var practiceGroupId = $("#PracticeGroupFilter").val();

        return {
            filter: $("#FilterText").val(),
            practiceGroupId: practiceGroupId || undefined,
            isSubscribed: status === "" ? undefined : status === "true"
        };
    };

    var dataTable = $("#JobAlertRegistrationsTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: true,
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(jobAlertRegistrationService.getList, getFilter),
            order: [[6, "desc"]],
            columnDefs: [
                {
                    data: "email",
                    targets: 0
                },
                {
                    data: "firstName",
                    targets: 1,
                    defaultContent: ""
                },
                {
                    data: "lastName",
                    targets: 2,
                    defaultContent: ""
                },
                {
                    data: "practiceGroupNames",
                    targets: 3,
                    render: function (data) {
                        if (!data || !data.length) {
                            return "-";
                        }

                        return data.join(", ");
                    }
                },
                {
                    data: "roleTypeNames",
                    targets: 4,
                    render: function (data) {
                        if (!data || !data.length) {
                            return "-";
                        }

                        return data.join(", ");
                    }
                },
                {
                    data: "isSubscribed",
                    targets: 5,
                    render: function (data) {
                        return data
                            ? '<span class="badge bg-success">' + l("Active") + "</span>"
                            : '<span class="badge bg-secondary">' + l("Unsubscribed") + "</span>";
                    }
                },
                {
                    data: "creationTime",
                    targets: 6,
                    render: DataTable.render.datetime("yyyy-MM-DD HH:mm")
                },
                {
                    data: "lastModificationTime",
                    targets: 7,
                    render: function (data, type, row) {
                        if (row.isSubscribed) {
                            return row.lastModificationTime
                                ? DataTable.render.datetime("yyyy-MM-DD HH:mm")(data)
                                : "-";
                        }

                        var display = row.unsubscribedAt
                            ? luxon.DateTime.fromISO(row.unsubscribedAt).toFormat("yyyy-LL-dd HH:mm")
                            : "-";
                        return display;
                    }
                }
            ]
        })
    );

    $("#SearchForm").on("submit", function (event) {
        event.preventDefault();
        dataTable.ajax.reload();
    });

    $("#AdvancedFilterSectionToggler").on("click", function () {
        $("#AdvancedFilterSection").slideToggle();
    });

    $("#PracticeGroupFilter, #StatusFilter").on("change", function () {
        dataTable.ajax.reload();
    });
});
