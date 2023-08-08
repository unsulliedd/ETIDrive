// Delete Confirmation Modal
function showDeleteModal(itemName, deleteUrl) {
    const modal = $(`[class="modal delete-modal"]`).first();
    modal.find(".modal-body span").text(itemName);
    modal.modal('show');

    modal.find("#confirmDeleteButton").click(function () {
        window.location.href = deleteUrl;
    });
}

// Close Button Function
$(".close-button").click(function () {
    $(this).closest('.modal').modal('toggle');
});

$(document).ready(function () {
    var userListContainer = $(".user_list_container");
    var userSearchInput = $("#userSearch");

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

    function updateUserList(selectedDepartmentId, pageIndex = 1) {
        $.ajax({
            url: "/Folder/GetUserList",
            type: "GET",
            data: { pageIndex: pageIndex, selectedDepartmentId: selectedDepartmentId },
            success: function (data) {
                userListContainer.html(data);
                filterUserList();
            },
            error: function () {
                console.error("An error occurred while loading user list.");
            }
        });
    }

    updateUserList(null, 1);

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
