// Delete Confirmation Modal
function showDeleteModal(itemName, deleteUrl) {
    const modal = $(`[class="modal delete-modal"]`).first();
    modal.find(".modal-body span").text(itemName);
    modal.modal('show');

    modal.find("#confirmDeleteButton").click(function () {
        window.location.href = deleteUrl;
    });

}
document.getElementById("goBackButton").addEventListener("click", function () {
    window.history.back();
});

// Close Button Function
$(".close-button").click(function () {
    $(this).closest('.modal').modal('toggle');
});

$(document).ready(function () {
    var userListLoaded = false;

    function loadUserListOnce(selectedDepartmentId, pageIndex) {
        if (!userListLoaded) {
            updateUserList(selectedDepartmentId, pageIndex);
            userListLoaded = true;
        }
    }

    loadUserListOnce(null, 1);

    var userListContainer = $(".user_list_container");
    var userSearchInput = $("#userSearch");
    var selectedCheckboxes = {
        view: [],
        edit: [],
        delete: [],
        upload: [],
        download: []
    };

    function filterUserList() {
        var searchKeyword = userSearchInput.val().toLowerCase();
        var userListItems = userListContainer.find(".user_item");

        userListItems.each(function () {
            var username = $(this).find(".user_username").text().toLowerCase();

            if (username.includes(searchKeyword)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    }

    userSearchInput.on("input", function () {
        updateUserList(null, 1);
        filterUserList();
    });

    userListContainer.on("change", "input[type='checkbox']", function () {
        var checkbox = $(this);
        var userId = checkbox.data("user-id");
        var checkboxType = checkbox.data("type");

        if (checkbox.prop("checked")) {
            selectedCheckboxes[checkboxType].push(userId);
        } else {
            var indexToRemove = selectedCheckboxes[checkboxType].indexOf(userId);
            if (indexToRemove !== -1) {
                selectedCheckboxes[checkboxType].splice(indexToRemove, 1);
            }
        }
    });

    function updateUserList(selectedDepartmentId, pageIndex = 1) {
        $.ajax({
            url: "/Folder/GetUserList",
            type: "GET",
            data: { pageIndex: pageIndex, selectedDepartmentId: selectedDepartmentId },
            success: function (data) {
                userListContainer.html(data);
                filterUserList();

                for (var type in selectedCheckboxes) {
                    for (var i = 0; i < selectedCheckboxes[type].length; i++) {
                        var userId = selectedCheckboxes[type][i];
                        $("input[type='checkbox'][data-user-id='" + userId + "'][data-type='" + type + "']").prop("checked", true);
                    }
                }
            },
            error: function () {
                console.error("An error occurred while loading user list.");
            }
        });
    }

    $("#departmentFilter").on("change", function () {
        var selectedDepartmentId = $(this).val();
        updateUserList(selectedDepartmentId);
    });

    $("#userListContainer").on("click", ".pagination-link", function () {
        var pageIndex = $(this).data("page");
        var selectedDepartmentId = $("#departmentFilter").val();
        updateUserList(selectedDepartmentId, pageIndex);
    });
});